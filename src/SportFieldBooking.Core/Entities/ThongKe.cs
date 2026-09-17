namespace SportFieldBooking.Core.Entities;

/// <summary>Dòng dữ liệu thống kê doanh thu theo ngày.</summary>
public class DoanhThuNgay
{
    public DateTime Ngay { get; set; }
    public int SoBooking { get; set; }
    public decimal TienGoc { get; set; }
    public decimal TienGiam { get; set; }
    public decimal DoanhThu { get; set; }
}

/// <summary>Dòng dữ liệu thống kê theo loại giảm giá được áp dụng.</summary>
public class ThongKeGiamGia
{
    public string LoaiGiamGia { get; set; } = "";
    public int SoLuong { get; set; }
    public decimal TongTienGiam { get; set; }
}

/// <summary>Thống kê theo sân (doanh thu, số lượt thuê).</summary>
public class ThongKeSan
{
    public int MaSan { get; set; }
    public string TenSan { get; set; } = "";
    public int SoLuotThue { get; set; }
    public decimal DoanhThu { get; set; }
}

/// <summary>Các con số tổng quan cho Dashboard.</summary>
public class TongQuan
{
    public int TongSoSan { get; set; }
    public int SanTrong { get; set; }
    public int SanDangThue { get; set; }
    public int SanBaoTri { get; set; }
    public int BookingHomNay { get; set; }
    public int BookingTrongKy { get; set; }
    public int HoaDonChuaThanhToan { get; set; }
    public int TongKhachHang { get; set; }
    public decimal DoanhThuHomNay { get; set; }
    public decimal DoanhThuTrongKy { get; set; }
    public decimal TongTienGiam { get; set; }
}

/// <summary>Thống kê lịch sử của một khách hàng (cổng khách hàng).</summary>
public class ThongKeCaNhan
{
    public int SoLanDat { get; set; }
    public int SoLanHoanThanh { get; set; }
    public int SoLanHuy { get; set; }
    public decimal TongChiTieu { get; set; }
    public string SanYeuThich { get; set; } = "Chưa có";
    public int SoLanDungVoucher { get; set; }
}
