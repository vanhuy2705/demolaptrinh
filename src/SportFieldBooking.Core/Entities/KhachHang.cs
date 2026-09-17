namespace SportFieldBooking.Core.Entities;

/// <summary>Bảng KHACH_HANG: hồ sơ khách hàng. MaTK liên kết với cổng khách hàng (có thể null).</summary>
public class KhachHang
{
    public int MaKH { get; set; }
    public string HoTen { get; set; } = "";
    public string SDT { get; set; } = "";
    public string Email { get; set; } = "";
    public string DiaChi { get; set; } = "";
    public int? MaTK { get; set; }
    public DateTime NgayTao { get; set; }

    /// <summary>Tên đăng nhập (chỉ phục vụ hiển thị).</summary>
    public string TenDangNhap { get; set; } = "";
}
