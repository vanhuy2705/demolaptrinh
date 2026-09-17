using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

/// <summary>Nghiệp vụ voucher: CRUD chính sách + kiểm tra điều kiện áp dụng.</summary>
public class VoucherService
{
    private readonly IVoucherRepository _voucherRepo;
    private readonly ISuDungVoucherRepository _suDungRepo;

    public VoucherService() : this(new Data.Repositories.VoucherRepository(), new Data.Repositories.SuDungVoucherRepository())
    {
    }

    public VoucherService(IVoucherRepository voucherRepo, ISuDungVoucherRepository suDungRepo)
    {
        _voucherRepo = voucherRepo;
        _suDungRepo = suDungRepo;
    }

    public List<Voucher> LayTatCa(string tuKhoa = "", bool? chiConHan = null) =>
        _voucherRepo.LayTatCa(tuKhoa, chiConHan);

    /// <summary>Danh sách voucher khách hàng có thể dùng (còn hạn, còn lượt, đang hoạt động).</summary>
    public List<Voucher> LayVoucherCoTheDung(string tuKhoa = "") =>
        _voucherRepo.LayTatCa(tuKhoa, true);

    public Voucher LayTheoMa(int maVoucher) => _voucherRepo.LayTheoMa(maVoucher);

    public KetQua<Voucher> Them(Voucher voucher)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.VoucherThem))
                return KetQua<Voucher>.Loi("Bạn không có quyền tạo voucher.");

            KetQua hopLe = KiemTraDuLieu(voucher);
            if (!hopLe.ThanhCong) return KetQua<Voucher>.Loi(hopLe.ThongBao);
            if (_voucherRepo.TonTaiMaCode(voucher.MaCode.Trim()))
                return KetQua<Voucher>.Loi("Mã voucher đã tồn tại.");

            voucher.MaCode = voucher.MaCode.Trim().ToUpperInvariant();
            voucher.MaVoucher = _voucherRepo.Them(voucher);
            return KetQua<Voucher>.Tot(voucher, "Thêm voucher thành công.");
        }
        catch (Exception ex)
        {
            return KetQua<Voucher>.Loi("Không thể thêm voucher: " + ex.Message);
        }
    }

    public KetQua CapNhat(Voucher voucher)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.VoucherSua))
                return KetQua.Loi("Bạn không có quyền sửa voucher.");

            KetQua hopLe = KiemTraDuLieu(voucher);
            if (!hopLe.ThanhCong) return hopLe;
            if (voucher.MaVoucher <= 0) return KetQua.Loi("Voucher không hợp lệ.");
            if (_voucherRepo.TonTaiMaCode(voucher.MaCode.Trim(), voucher.MaVoucher))
                return KetQua.Loi("Mã voucher đã được dùng cho voucher khác.");

            voucher.MaCode = voucher.MaCode.Trim().ToUpperInvariant();
            _voucherRepo.CapNhat(voucher);
            return KetQua.Tot("Cập nhật voucher thành công.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể cập nhật voucher: " + ex.Message);
        }
    }

    public KetQua Xoa(int maVoucher)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.VoucherXoa))
                return KetQua.Loi("Bạn không có quyền xóa voucher.");

            _voucherRepo.Xoa(maVoucher);
            return KetQua.Tot("Xóa voucher thành công.");
        }
        catch (Exception ex)
        {
            string thongBao = ex.Message.Contains("REFERENCE")
                ? "Không thể xóa: voucher đã được sử dụng trong lịch sử thanh toán."
                : "Không thể xóa voucher: " + ex.Message;
            return KetQua.Loi(thongBao);
        }
    }

    /// <summary>
    /// Kiểm tra đầy đủ điều kiện áp dụng voucher: tồn tại, trạng thái, hạn dùng, số lượng, đơn tối thiểu.
    /// </summary>
    public KetQua<Voucher> KiemTraVoucher(string maCode, decimal tienGoc, int? maKH = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(maCode))
                return KetQua<Voucher>.Loi("Chưa nhập mã voucher.");

            Voucher voucher = _voucherRepo.LayTheoMaCode(maCode.Trim().ToUpperInvariant());
            if (voucher == null)
                return KetQua<Voucher>.Loi("Mã voucher không tồn tại.");
            if (voucher.TrangThai != TrangThaiVoucher.HoatDong)
                return KetQua<Voucher>.Loi("Voucher này đang tạm ngưng sử dụng.");

            DateTime homNay = DateTime.Now.Date;
            if (homNay < voucher.NgayBatDau.Date)
                return KetQua<Voucher>.Loi($"Voucher chưa có hiệu lực (áp dụng từ {voucher.NgayBatDau:dd/MM/yyyy}).");
            if (homNay > voucher.NgayKetThuc.Date)
                return KetQua<Voucher>.Loi($"Voucher đã hết hạn (hạn dùng đến {voucher.NgayKetThuc:dd/MM/yyyy}).");
            if (voucher.SoLuongDaDung >= voucher.SoLuong)
                return KetQua<Voucher>.Loi("Voucher đã hết lượt sử dụng.");
            if (tienGoc < voucher.DonToiThieu)
                return KetQua<Voucher>.Loi($"Đơn từ {DinhDangTien(voucher.DonToiThieu)} mới được dùng voucher này.");

            return KetQua<Voucher>.Tot(voucher, "Voucher hợp lệ.");
        }
        catch (Exception ex)
        {
            return KetQua<Voucher>.Loi("Không kiểm tra được voucher: " + ex.Message);
        }
    }

    /// <summary>Số tiền được giảm khi áp dụng voucher (không vượt quá tiền gốc).</summary>
    public decimal TinhTienGiam(Voucher voucher, decimal tienGoc)
    {
        if (voucher == null || tienGoc <= 0) return 0m;
        decimal tienGiam = voucher.LoaiGiam == LoaiGiam.SoTien
            ? voucher.GiaTriGiam
            : tienGoc * voucher.GiaTriGiam / 100m;
        return Math.Min(Math.Round(tienGiam, 0), tienGoc);
    }

    public List<SuDungVoucher> LayLichSuSuDung(int maKH) => _suDungRepo.LayTheoKhachHang(maKH);

    private static KetQua KiemTraDuLieu(Voucher voucher)
    {
        if (voucher == null) return KetQua.Loi("Dữ liệu voucher không hợp lệ.");
        if (string.IsNullOrWhiteSpace(voucher.MaCode)) return KetQua.Loi("Vui lòng nhập mã voucher.");
        if (string.IsNullOrWhiteSpace(voucher.TenVoucher)) return KetQua.Loi("Vui lòng nhập tên voucher.");
        if (voucher.LoaiGiam != LoaiGiam.PhanTram && voucher.LoaiGiam != LoaiGiam.SoTien)
            return KetQua.Loi("Loại giảm không hợp lệ.");
        if (voucher.GiaTriGiam <= 0) return KetQua.Loi("Giá trị giảm phải lớn hơn 0.");
        if (voucher.LoaiGiam == LoaiGiam.PhanTram && voucher.GiaTriGiam > 100)
            return KetQua.Loi("Phần trăm giảm không được vượt quá 100%.");
        if (voucher.DonToiThieu < 0) return KetQua.Loi("Đơn tối thiểu không được âm.");
        if (voucher.SoLuong <= 0) return KetQua.Loi("Số lượng phát hành phải lớn hơn 0.");
        if (voucher.NgayKetThuc.Date < voucher.NgayBatDau.Date)
            return KetQua.Loi("Ngày kết thúc phải sau ngày bắt đầu.");
        return KetQua.Tot();
    }

    private static string DinhDangTien(decimal soTien) => soTien.ToString("N0") + " đ";
}
