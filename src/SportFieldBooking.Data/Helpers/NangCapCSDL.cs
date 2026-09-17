#nullable enable annotations
namespace SportFieldBooking.Data.Helpers;

/// <summary>
/// Tự nâng cấp lược đồ CSDL lúc khởi động (idempotent - chạy lại bao nhiêu lần cũng được).
///
/// Lý do có lớp này: khi phiên bản ứng dụng cần thêm cột/bảng mới, người dùng chỉ
/// `git pull` rồi chạy mà quên chạy script SQL -> ứng dụng chết với lỗi khó hiểu kiểu
/// "Invalid column name". Ở đây ứng dụng tự kiểm tra và tự thêm phần còn thiếu.
///
/// Nguyên tắc an toàn:
///  • CHỈ thêm (cột NULL được, khoá ngoại, index) - không bao giờ xoá/sửa kiểu dữ liệu.
///  • Mỗi bước đều kiểm tra tồn tại trước khi làm.
///  • Mọi lỗi đều được bắt lại và báo về, KHÔNG làm sập ứng dụng: nếu không có quyền
///    ALTER thì DatSanRepository sẽ tự bỏ cột đó khỏi câu SQL và app vẫn chạy bình thường.
/// </summary>
public static class NangCapCSDL
{
    /// <summary>Kết quả một lần kiểm tra/nâng cấp lược đồ.</summary>
    public class KetQua
    {
        public bool ThanhCong { get; set; } = true;
        public bool DaThayDoi { get; set; }
        public string ThongBao { get; set; } = "";
        public string Loi { get; set; } = "";
    }

    /// <summary>
    /// Kiểm tra và bổ sung những phần lược đồ mà phiên bản ứng dụng này cần.
    /// </summary>
    public static KetQua KiemTraVaCapNhat()
    {
        var ketQua = new KetQua();
        var daLam = new List<string>();

        try
        {
            // v3: voucher được lưu ngay trên booking (DAT_SAN.MaVoucher).
            if (!CoCot("DAT_SAN", "MaVoucher"))
            {
                DbHelper.ThucThi("ALTER TABLE dbo.DAT_SAN ADD MaVoucher INT NULL;");
                daLam.Add("thêm cột DAT_SAN.MaVoucher");
            }

            if (CoBang("VOUCHER") && !CoKhoaNgoai("FK_DAT_SAN_VOUCHER"))
            {
                DbHelper.ThucThi(@"ALTER TABLE dbo.DAT_SAN
                    ADD CONSTRAINT FK_DAT_SAN_VOUCHER FOREIGN KEY (MaVoucher) REFERENCES dbo.VOUCHER(MaVoucher);");
                daLam.Add("thêm khoá ngoại FK_DAT_SAN_VOUCHER");
            }

            if (CoCot("DAT_SAN", "MaVoucher") && !CoIndex("DAT_SAN", "IX_DAT_SAN_Voucher"))
            {
                DbHelper.ThucThi("CREATE INDEX IX_DAT_SAN_Voucher ON dbo.DAT_SAN(MaVoucher);");
                daLam.Add("thêm index IX_DAT_SAN_Voucher");
            }

            // Hồi tố dữ liệu cũ: booking đã có hóa đơn/lịch sử dùng voucher thì chép ngược lại.
            // Đây chỉ là tiện ích thêm - nếu thất bại (quyền, khoá...) thì bỏ qua bước đó,
            // KHÔNG được làm hỏng cả lần nâng cấp (phần quan trọng là thêm cột ở trên).
            if (CoCot("DAT_SAN", "MaVoucher"))
            {
                void ThuBackfill(string moTa, Func<int> chay)
                {
                    try
                    {
                        int n = chay();
                        if (n > 0) daLam.Add($"{moTa} cho {n} booking cũ");
                    }
                    catch (Exception exBackfill)
                    {
                        daLam.Add($"{moTa}: bỏ qua ({exBackfill.Message})");
                    }
                }

                if (CoCot("HOA_DON", "MaVoucher"))
                    ThuBackfill("hồi tố voucher từ HOA_DON", () => DbHelper.ThucThi(@"UPDATE ds SET ds.MaVoucher = hd.MaVoucher
                        FROM dbo.DAT_SAN ds
                        INNER JOIN dbo.HOA_DON hd ON hd.MaDat = ds.MaDat
                        WHERE ds.MaVoucher IS NULL AND hd.MaVoucher IS NOT NULL;"));

                if (CoBang("SU_DUNG_VOUCHER"))
                    ThuBackfill("đối soát từ SU_DUNG_VOUCHER", () => DbHelper.ThucThi(@"UPDATE ds SET ds.MaVoucher = sd.MaVoucher
                        FROM dbo.DAT_SAN ds
                        INNER JOIN dbo.SU_DUNG_VOUCHER sd ON sd.MaDat = ds.MaDat
                        WHERE ds.MaVoucher IS NULL;"));
            }

            ketQua.DaThayDoi = daLam.Count > 0;
            ketQua.ThongBao = daLam.Count == 0 ? "Lược đồ CSDL đã đúng phiên bản." : string.Join("; ", daLam);
        }
        catch (Exception ex)
        {
            // Không ném ra ngoài: thiếu quyền nâng cấp thì ứng dụng vẫn phải chạy được.
            ketQua.ThanhCong = false;
            ketQua.Loi = ex.Message;
            ketQua.ThongBao = daLam.Count > 0 ? string.Join("; ", daLam) : "";
        }

        return ketQua;
    }

    private static bool CoBang(string ten) =>
        Convert.ToInt32(DbHelper.GiaTriDon(
            "SELECT CASE WHEN OBJECT_ID(N'dbo." + ten + @"', N'U') IS NULL THEN 0 ELSE 1 END")) == 1;

    private static bool CoCot(string tenBang, string tenCot)
    {
        try
        {
            return Convert.ToInt32(DbHelper.GiaTriDon(
                $"SELECT CASE WHEN COL_LENGTH(N'dbo.{tenBang}', N'{tenCot}') IS NULL THEN 0 ELSE 1 END")) == 1;
        }
        catch
        {
            return false;
        }
    }

    private static bool CoKhoaNgoai(string ten)
    {
        try
        {
            return Convert.ToInt32(DbHelper.GiaTriDon(
                "SELECT CASE WHEN EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'" + ten + @"') THEN 1 ELSE 0 END")) == 1;
        }
        catch
        {
            return false;
        }
    }

    private static bool CoIndex(string tenBang, string tenIndex)
    {
        try
        {
            return Convert.ToInt32(DbHelper.GiaTriDon(
                "SELECT CASE WHEN EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'" + tenIndex +
                "' AND object_id = OBJECT_ID(N'dbo." + tenBang + @"')) THEN 1 ELSE 0 END")) == 1;
        }
        catch
        {
            return false;
        }
    }
}
