namespace SportFieldBooking.Core.Entities;

/// <summary>Bảng SAN: sân cụ thể, đơn giá thuê theo giờ.</summary>
public class San
{
    public int MaSan { get; set; }
    public string TenSan { get; set; } = "";
    public int MaLoaiSan { get; set; }
    /// <summary>Đơn giá cố định theo giờ (không chia sáng/chiều/tối).</summary>
    public decimal DonGia { get; set; }
    /// <summary>Trong | DangThue | BaoTri (xem lớp TrangThaiSan).</summary>
    public string TrangThai { get; set; } = Core.Common.TrangThaiSan.Trong;
    public string MoTa { get; set; } = "";

    /// <summary>Tên loại sân (chỉ phục vụ hiển thị trên lưới).</summary>
    public string TenLoaiSan { get; set; } = "";
}
