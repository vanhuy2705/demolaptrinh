namespace SportFieldBooking.Core.Entities;

/// <summary>Bảng KHUYEN_MAI: chương trình khuyến mãi dịp đặc biệt (áp dụng theo khoảng ngày).</summary>
public class KhuyenMai
{
    public int MaKM { get; set; }
    public string TenKM { get; set; } = "";
    /// <summary>Phân loại chương trình, ví dụ: LeTet, KhaiTruong, SinhNhat...</summary>
    public string LoaiKhuyenMai { get; set; } = "";
    /// <summary>Phần trăm giảm trên tiền sân (0-100).</summary>
    public decimal PhanTramGiam { get; set; }
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    /// <summary>True nếu chương trình chỉ xét cho booking vào Thứ Bảy / Chủ Nhật.</summary>
    public bool ApDungCuoiTuan { get; set; }
    public string TrangThai { get; set; } = Core.Common.TrangThaiVoucher.HoatDong;
    public string MoTa { get; set; } = "";
}
