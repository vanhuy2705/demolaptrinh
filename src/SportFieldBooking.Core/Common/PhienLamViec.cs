namespace SportFieldBooking.Core.Common;

/// <summary>
/// Thông tin người dùng đang đăng nhập (phiên làm việc). Chỉ giữ dữ liệu, không truy vấn CSDL.
/// </summary>
public static class PhienLamViec
{
    public static int MaTK { get; set; }
    public static string TenDangNhap { get; set; } = "";
    public static string HoTen { get; set; } = "";
    public static string VaiTro { get; set; } = "";

    /// <summary>Mã nhân viên (nếu tài khoản là nhân viên/quản trị viên).</summary>
    public static int? MaNV { get; set; }

    /// <summary>Mã khách hàng (nếu tài khoản là khách hàng).</summary>
    public static int? MaKH { get; set; }

    public static bool DaDangNhap => MaTK > 0 && !string.IsNullOrWhiteSpace(VaiTro);

    public static bool LaAdmin => VaiTro == Core.Common.VaiTro.Admin;
    public static bool LaNhanVien => VaiTro == Core.Common.VaiTro.NhanVien;
    public static bool LaKhachHang => VaiTro == Core.Common.VaiTro.KhachHang;

    public static void DangNhap(Entities.TaiKhoan taiKhoan, int? maNV = null, int? maKH = null)
    {
        MaTK = taiKhoan?.MaTK ?? 0;
        TenDangNhap = taiKhoan?.TenDangNhap ?? "";
        HoTen = taiKhoan?.HoTen ?? "";
        VaiTro = taiKhoan?.VaiTro ?? "";
        MaNV = maNV;
        MaKH = maKH;
    }

    public static void DangXuat()
    {
        MaTK = 0;
        TenDangNhap = "";
        HoTen = "";
        VaiTro = "";
        MaNV = null;
        MaKH = null;
    }
}
