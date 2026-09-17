namespace SportFieldBooking.Core.Entities;

/// <summary>Bảng NHAN_VIEN: hồ sơ nhân viên, liên kết 1-1 với TAIKHOAN.</summary>
public class NhanVien
{
    public int MaNV { get; set; }
    public int? MaTK { get; set; }
    public string HoTen { get; set; } = "";
    public string SDT { get; set; } = "";
    public string Email { get; set; } = "";
    public string DiaChi { get; set; } = "";
    public string ChucVu { get; set; } = "";
    public DateTime? NgayVaoLam { get; set; }
    public string TrangThai { get; set; } = TrangThaiTaiKhoan.HoatDong;

    // --- Thuộc tính hiển thị (đọc thêm từ bảng TAIKHOAN, không ánh xạ trực tiếp) ---
    public string TenDangNhap { get; set; } = "";
    public string VaiTro { get; set; } = "";
}
