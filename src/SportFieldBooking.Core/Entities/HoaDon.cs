namespace SportFieldBooking.Core.Entities;

/// <summary>Bảng HOA_DON: hóa đơn thanh toán cho một booking.</summary>
public class HoaDon
{
    public int MaHD { get; set; }
    public int MaDat { get; set; }
    public DateTime NgayLap { get; set; }
    /// <summary>Tiền sân trước khi giảm.</summary>
    public decimal TienGoc { get; set; }
    /// <summary>Khong | Voucher | KhuyenMai | CuoiTuan (xem lớp LoaiGiamGia).</summary>
    public string LoaiGiamGia { get; set; } = Core.Common.LoaiGiamGia.Khong;
    public decimal TienGiam { get; set; }
    public decimal TongTien { get; set; }
    /// <summary>TienMat | ChuyenKhoan (xem lớp PhuongThucThanhToan).</summary>
    public string PhuongThucThanhToan { get; set; } = Core.Common.PhuongThucThanhToan.TienMat;
    /// <summary>ChuaThanhToan | DaThanhToan | DaHuy (xem lớp TrangThaiHoaDon).</summary>
    public string TrangThai { get; set; } = Core.Common.TrangThaiHoaDon.ChuaThanhToan;
    public int? MaNguoiLap { get; set; }
    /// <summary>Mã voucher được áp dụng (mở rộng để theo dõi lượt sử dụng khi thanh toán).</summary>
    public int? MaVoucher { get; set; }
    public string GhiChu { get; set; } = "";

    // --- Thuộc tính hiển thị (join từ bảng khác) ---
    public string TenKH { get; set; } = "";
    public string SDT { get; set; } = "";
    public string TenSan { get; set; } = "";
    public DateTime NgayDat { get; set; }
    public TimeSpan GioBatDau { get; set; }
    public TimeSpan GioKetThuc { get; set; }
    public string MaCode { get; set; } = "";
}
