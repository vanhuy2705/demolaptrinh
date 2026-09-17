namespace SportFieldBooking.Core.Entities;

/// <summary>Bảng VOUCHER: mã giảm giá phát hành cho khách hàng.</summary>
public class Voucher
{
    public int MaVoucher { get; set; }
    public string MaCode { get; set; } = "";
    public string TenVoucher { get; set; } = "";
    /// <summary>PhanTram | SoTien (xem lớp LoaiGiam).</summary>
    public string LoaiGiam { get; set; } = Core.Common.LoaiGiam.PhanTram;
    public decimal GiaTriGiam { get; set; }
    /// <summary>Giá trị booking tối thiểu để được áp dụng voucher.</summary>
    public decimal DonToiThieu { get; set; }
    public int SoLuong { get; set; }
    public int SoLuongDaDung { get; set; }
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    /// <summary>HoatDong | TamNgung (xem lớp TrangThaiVoucher).</summary>
    public string TrangThai { get; set; } = Core.Common.TrangThaiVoucher.HoatDong;
    public string MoTa { get; set; } = "";

    // --- Thuộc tính phái sinh (chỉ phục vụ hiển thị) ---
    public int SoLuongConLai => Math.Max(0, SoLuong - SoLuongDaDung);
    public bool ConHan => DateTime.Now.Date <= NgayKetThuc.Date && DateTime.Now.Date >= NgayBatDau.Date;
}
