using System.Data;
using Microsoft.Data.SqlClient;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Data.Repositories;

/// <summary>Repository bảng DAT_SAN (có truy vấn kiểm tra trùng lịch).</summary>
public class DatSanRepository : BaseRepository, IDatSanRepository
{
    // ===== Tự thích ứng lược đồ CSDL =====
    // Cột DAT_SAN.MaVoucher chỉ có từ bản CSDL v3 (script 05_BoSungVoucherDatSan_v3.sql).
    // Nếu người dùng chưa nâng cấp CSDL - hoặc tài khoản SQL không có quyền ALTER để app
    // tự nâng cấp - thì mọi câu SELECT/INSERT/UPDATE có cột này sẽ làm SQL Server báo
    // "Invalid column name 'MaVoucher'" và ứng dụng không chạy được. Vì vậy repository tự
    // kiểm tra cột có tồn tại hay không (1 lần/phiên, có nhớ kết quả) rồi mới lắp SQL.
    private static int? _coMaVoucher;

    /// <summary>Cột DAT_SAN.MaVoucher có tồn tại trong CSDL hiện tại không.</summary>
    public static bool CoCotMaVoucher
    {
        get
        {
            if (_coMaVoucher == null)
            {
                try
                {
                    object kq = DbHelper.GiaTriDon(
                        "SELECT CASE WHEN COL_LENGTH(N'dbo.DAT_SAN', N'MaVoucher') IS NULL THEN 0 ELSE 1 END");
                    _coMaVoucher = (kq == null || kq == DBNull.Value) ? 0 : Convert.ToInt32(kq);
                }
                catch
                {
                    // Không kết nối được / không có quyền đọc metadata: coi như chưa có cột.
                    _coMaVoucher = 0;
                }
            }
            return _coMaVoucher.Value == 1;
        }
    }

    /// <summary>Quên kết quả kiểm tra cột - gọi sau khi vừa nâng cấp lược đồ.</summary>
    public static void DatLaiKiemTraCot() => _coMaVoucher = null;

    // Khung truy vấn; 3 chỗ {…} được lấp bằng đoạn voucher hoặc để trống.
    private const string SqlSelectMau = @"
        SELECT ds.MaDat, ds.MaKH, ds.MaSan, ds.NgayDat, ds.GioBatDau, ds.GioKetThuc, ds.TienSan,
               ds.TrangThai, ISNULL(ds.GhiChu, '') AS GhiChu, ds.NgayTao, ds.MaNguoiTao{COT_VOUCHER},
               ISNULL(kh.HoTen, '') AS TenKH, ISNULL(kh.SDT, '') AS SDT,
               ISNULL(s.TenSan, '') AS TenSan, ISNULL(ls.TenLoaiSan, '') AS TenLoaiSan, ISNULL(s.DonGia, 0) AS DonGia{COT_MA_CODE}
        FROM DAT_SAN ds
        LEFT JOIN KHACH_HANG kh ON kh.MaKH = ds.MaKH
        LEFT JOIN SAN s ON s.MaSan = ds.MaSan
        LEFT JOIN LOAI_SAN ls ON ls.MaLoaiSan = s.MaLoaiSan{JOIN_VOUCHER}";

    private static string SqlSelect => LapSql(SqlSelectMau, CoCotMaVoucher);

    /// <summary>
    /// Lắp (hoặc bỏ) các đoạn liên quan voucher vào khung SQL.
    /// Công khai để bộ kiểm thử xác minh được SQL sinh ra hợp lệ ở cả 2 trạng thái CSDL.
    /// </summary>
    public static string LapSql(string mau, bool coVoucher) => mau
        .Replace("{COT_VOUCHER}", coVoucher ? ", ds.MaVoucher" : "")
        .Replace("{COT_MA_CODE}", coVoucher ? ", ISNULL(v.MaCode, '') AS MaVoucherCode" : "")
        .Replace("{JOIN_VOUCHER}", coVoucher ? "\n        LEFT JOIN VOUCHER v ON v.MaVoucher = ds.MaVoucher" : "");

    /// <summary>Bản SELECT có khoá dải bản ghi, dùng khi kiểm tra trùng lịch ngay trước lúc ghi.</summary>
    private const string KhoaChongTrung = "WITH (UPDLOCK, HOLDLOCK)";

    private static DatSan AnhXa(DataRow dong) => new()
    {
        MaDat = dong.SoNguyen("MaDat"),
        MaKH = dong.SoNguyen("MaKH"),
        MaSan = dong.SoNguyen("MaSan"),
        NgayDat = dong.NgayGio("NgayDat"),
        GioBatDau = dong.Gio("GioBatDau"),
        GioKetThuc = dong.Gio("GioKetThuc"),
        TienSan = dong.SoThapPhan("TienSan"),
        TrangThai = dong.Chuoi("TrangThai"),
        GhiChu = dong.Chuoi("GhiChu"),
        NgayTao = dong.NgayGio("NgayTao"),
        MaNguoiTao = dong.SoNguyenCoTheNull("MaNguoiTao"),
        MaVoucher = dong.SoNguyenCoTheNull("MaVoucher"),
        MaVoucherCode = dong.Chuoi("MaVoucherCode"),
        TenKH = dong.Chuoi("TenKH"),
        SDT = dong.Chuoi("SDT"),
        TenSan = dong.Chuoi("TenSan"),
        TenLoaiSan = dong.Chuoi("TenLoaiSan"),
        DonGia = dong.SoThapPhan("DonGia")
    };

    public List<DatSan> LayTatCa(string tuKhoa = "") =>
        DanhSach($@"{SqlSelect}
                    WHERE (@TuKhoa IS NULL OR kh.HoTen LIKE @TuKhoa OR kh.SDT LIKE @TuKhoa OR s.TenSan LIKE @TuKhoa)
                    ORDER BY ds.NgayDat DESC, ds.GioBatDau",
            AnhXa, ThamSoTimKiem("@TuKhoa", tuKhoa));

    public List<DatSan> LayTheoNgay(DateTime ngay, int? maSan = null) =>
        DanhSach($@"{SqlSelect}
                    WHERE ds.NgayDat = @Ngay AND (@MaSan IS NULL OR ds.MaSan = @MaSan)
                    ORDER BY ds.GioBatDau",
            AnhXa, ThamSo("@Ngay", ngay.Date), ThamSo("@MaSan", (object)maSan ?? DBNull.Value));

    public List<DatSan> LayTheoKhoang(DateTime tuNgay, DateTime denNgay, int? maSan = null, string trangThai = null) =>
        DanhSach($@"{SqlSelect}
                    WHERE ds.NgayDat BETWEEN @TuNgay AND @DenNgay
                      AND (@MaSan IS NULL OR ds.MaSan = @MaSan)
                      AND (@TrangThai IS NULL OR ds.TrangThai = @TrangThai)
                    ORDER BY ds.NgayDat, ds.GioBatDau",
            AnhXa,
            ThamSo("@TuNgay", tuNgay.Date),
            ThamSo("@DenNgay", denNgay.Date),
            ThamSo("@MaSan", (object)maSan ?? DBNull.Value),
            ThamSo("@TrangThai", (object)trangThai ?? DBNull.Value));

    public List<DatSan> LayTheoKhachHang(int maKH) =>
        DanhSach($@"{SqlSelect} WHERE ds.MaKH = @MaKH
                    ORDER BY ds.NgayDat DESC, ds.GioBatDau DESC",
            AnhXa, ThamSo("@MaKH", maKH));

    public List<DatSan> LaySapDienRa(int soLuong) =>
        DanhSach(LapSql(@"SELECT TOP (@SoLuong) * FROM (
                      SELECT ds.MaDat, ds.MaKH, ds.MaSan, ds.NgayDat, ds.GioBatDau, ds.GioKetThuc, ds.TienSan,
                             ds.TrangThai, ISNULL(ds.GhiChu, '') AS GhiChu, ds.NgayTao, ds.MaNguoiTao{COT_VOUCHER},
                             ISNULL(kh.HoTen, '') AS TenKH, ISNULL(kh.SDT, '') AS SDT,
                             ISNULL(s.TenSan, '') AS TenSan, ISNULL(ls.TenLoaiSan, '') AS TenLoaiSan, ISNULL(s.DonGia, 0) AS DonGia{COT_MA_CODE}
                      FROM DAT_SAN ds
                      LEFT JOIN KHACH_HANG kh ON kh.MaKH = ds.MaKH
                      LEFT JOIN SAN s ON s.MaSan = ds.MaSan
                      LEFT JOIN LOAI_SAN ls ON ls.MaLoaiSan = s.MaLoaiSan{JOIN_VOUCHER}
                      WHERE ds.TrangThai IN ('DaDat', 'DangSuDung') AND ds.NgayDat >= CAST(GETDATE() AS DATE)
                    ) x
                    ORDER BY x.NgayDat, x.GioBatDau", CoCotMaVoucher),
            AnhXa, ThamSo("@SoLuong", soLuong));

    public DatSan LayTheoMa(int maDat) =>
        MotHoacNull($"{SqlSelect} WHERE ds.MaDat = @Ma", AnhXa, ThamSo("@Ma", maDat));

    public List<DatSan> LayTrungLich(int maSan, DateTime ngay, TimeSpan gioBatDau, TimeSpan gioKetThuc,
        int? maDatLoaiTru = null, bool khoaBang = false) =>
        DanhSach($@"{(khoaBang ? SqlSelect.Replace("FROM DAT_SAN ds", $"FROM DAT_SAN ds {KhoaChongTrung}") : SqlSelect)}
                    WHERE ds.MaSan = @MaSan AND ds.NgayDat = @Ngay
                      AND ds.TrangThai <> 'DaHuy'
                      AND (@MaLoaiTru IS NULL OR ds.MaDat <> @MaLoaiTru)
                      AND ds.GioBatDau < @GioKetThuc AND ds.GioKetThuc > @GioBatDau
                    ORDER BY ds.GioBatDau",
            AnhXa,
            ThamSo("@MaSan", maSan),
            ThamSo("@Ngay", ngay.Date),
            ThamSo("@GioBatDau", gioBatDau),
            ThamSo("@GioKetThuc", gioKetThuc),
            ThamSo("@MaLoaiTru", (object)maDatLoaiTru ?? DBNull.Value));

    public int Them(DatSan datSan)
    {
        bool coVoucher = CoCotMaVoucher;
        string sql = coVoucher
            ? @"INSERT INTO DAT_SAN (MaKH, MaSan, NgayDat, GioBatDau, GioKetThuc, TienSan, TrangThai, GhiChu, NgayTao, MaNguoiTao, MaVoucher)
                VALUES (@MaKH, @MaSan, @NgayDat, @GioBatDau, @GioKetThuc, @TienSan, @TrangThai, @GhiChu, GETDATE(), @MaNguoiTao, @MaVoucher);
                SELECT CAST(SCOPE_IDENTITY() AS INT);"
            : @"INSERT INTO DAT_SAN (MaKH, MaSan, NgayDat, GioBatDau, GioKetThuc, TienSan, TrangThai, GhiChu, NgayTao, MaNguoiTao)
                VALUES (@MaKH, @MaSan, @NgayDat, @GioBatDau, @GioKetThuc, @TienSan, @TrangThai, @GhiChu, GETDATE(), @MaNguoiTao);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

        var thamSo = new List<SqlParameter>
        {
            ThamSo("@MaKH", datSan.MaKH),
            ThamSo("@MaSan", datSan.MaSan),
            ThamSo("@NgayDat", datSan.NgayDat.Date),
            ThamSo("@GioBatDau", datSan.GioBatDau),
            ThamSo("@GioKetThuc", datSan.GioKetThuc),
            ThamSo("@TienSan", datSan.TienSan),
            ThamSo("@TrangThai", datSan.TrangThai),
            ThamSo("@GhiChu", datSan.GhiChu ?? ""),
            ThamSo("@MaNguoiTao", (object)datSan.MaNguoiTao ?? DBNull.Value)
        };
        if (coVoucher) thamSo.Add(ThamSo("@MaVoucher", (object)datSan.MaVoucher ?? DBNull.Value));

        return ThemTraVeMa(sql, thamSo.ToArray());
    }

    public int CapNhat(DatSan datSan)
    {
        bool coVoucher = CoCotMaVoucher;
        string sql = coVoucher
            ? @"UPDATE DAT_SAN SET MaKH = @MaKH, MaSan = @MaSan, NgayDat = @NgayDat,
                  GioBatDau = @GioBatDau, GioKetThuc = @GioKetThuc, TienSan = @TienSan,
                  TrangThai = @TrangThai, GhiChu = @GhiChu, MaVoucher = @MaVoucher
                  WHERE MaDat = @Ma"
            : @"UPDATE DAT_SAN SET MaKH = @MaKH, MaSan = @MaSan, NgayDat = @NgayDat,
                  GioBatDau = @GioBatDau, GioKetThuc = @GioKetThuc, TienSan = @TienSan,
                  TrangThai = @TrangThai, GhiChu = @GhiChu
                  WHERE MaDat = @Ma";

        var thamSo = new List<SqlParameter>
        {
            ThamSo("@MaKH", datSan.MaKH),
            ThamSo("@MaSan", datSan.MaSan),
            ThamSo("@NgayDat", datSan.NgayDat.Date),
            ThamSo("@GioBatDau", datSan.GioBatDau),
            ThamSo("@GioKetThuc", datSan.GioKetThuc),
            ThamSo("@TienSan", datSan.TienSan),
            ThamSo("@TrangThai", datSan.TrangThai),
            ThamSo("@GhiChu", datSan.GhiChu ?? ""),
            ThamSo("@Ma", datSan.MaDat)
        };
        if (coVoucher) thamSo.Add(ThamSo("@MaVoucher", (object)datSan.MaVoucher ?? DBNull.Value));

        return ThucThi(sql, thamSo.ToArray());
    }

    public int CapNhatTrangThai(int maDat, string trangThai) =>
        ThucThi("UPDATE DAT_SAN SET TrangThai = @TrangThai WHERE MaDat = @Ma",
            ThamSo("@TrangThai", trangThai), ThamSo("@Ma", maDat));

    public int Xoa(int maDat) =>
        ThucThi("DELETE FROM DAT_SAN WHERE MaDat = @Ma", ThamSo("@Ma", maDat));

    public int DemTheoTrangThai(string trangThai, DateTime? ngay = null) =>
        Convert.ToInt32(GiaTriDon(
            @"SELECT COUNT(1) FROM DAT_SAN
              WHERE TrangThai = @TrangThai AND (@Ngay IS NULL OR NgayDat = @Ngay)",
            ThamSo("@TrangThai", trangThai), ThamSo("@Ngay", (object)ngay?.Date ?? DBNull.Value)));
}
