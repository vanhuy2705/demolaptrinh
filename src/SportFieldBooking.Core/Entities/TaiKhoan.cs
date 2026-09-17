namespace SportFieldBooking.Core.Entities;

/// <summary>Bảng TAIKHOAN: tài khoản đăng nhập của Admin / Nhân viên / Khách hàng.</summary>
public class TaiKhoan
{
    public int MaTK { get; set; }
    public string TenDangNhap { get; set; } = "";
    /// <summary>Chuỗi mật khẩu đã băm (PBKDF2) - xem PasswordHasher.</summary>
    public string MatKhau { get; set; } = "";
    public string HoTen { get; set; } = "";
    /// <summary>Admin | NhanVien | KhachHang (xem lớp VaiTro).</summary>
    public string VaiTro { get; set; } = "";
    /// <summary>HoatDong | BiKhoa (xem lớp TrangThaiTaiKhoan).</summary>
    public string TrangThai { get; set; } = TrangThaiTaiKhoan.HoatDong;
    public DateTime NgayTao { get; set; }
}
