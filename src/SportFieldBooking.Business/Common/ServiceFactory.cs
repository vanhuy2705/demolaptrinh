using SportFieldBooking.Business.Services;

namespace SportFieldBooking.Business.Common;

/// <summary>
/// Nơi tạo sẵn các service dùng chung cho tầng giao diện (đơn giản hóa việc tiêm phụ thuộc).
/// Tầng giao diện chỉ gọi service, không tự tạo repository hay viết SQL.
/// </summary>
public static class ServiceFactory
{
    public static AuthService Auth { get; } = new AuthService();
    public static TaiKhoanService TaiKhoan { get; } = new TaiKhoanService();
    public static NhanVienService NhanVien { get; } = new NhanVienService();
    public static LoaiSanService LoaiSan { get; } = new LoaiSanService();
    public static SanService San { get; } = new SanService();
    public static KhachHangService KhachHang { get; } = new KhachHangService();
    public static DatSanService DatSan { get; } = new DatSanService();
    public static HoaDonService HoaDon { get; } = new HoaDonService();
    public static VoucherService Voucher { get; } = new VoucherService();
    public static KhuyenMaiService KhuyenMai { get; } = new KhuyenMaiService();
    public static CauHinhService CauHinh { get; } = new CauHinhService();
    public static ThongKeService ThongKe { get; } = new ThongKeService();
    public static TinhTienService TinhTien { get; } = new TinhTienService();
    public static NhatKyService NhatKy { get; } = new NhatKyService();
    public static QuyenService Quyen { get; } = new QuyenService();
}
