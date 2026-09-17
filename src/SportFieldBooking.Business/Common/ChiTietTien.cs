using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;

namespace SportFieldBooking.Business.Common;

/// <summary>Kết quả tính tiền của một booking (tiền gốc, ưu đãi, tổng tiền).</summary>
public class ChiTietTien
{
    public int MaSan { get; set; }
    public string TenSan { get; set; } = "";
    public decimal DonGia { get; set; }

    /// <summary>Số block (mặc định 30 phút/block) dùng để tính tiền.</summary>
    public int ThoiLuongBlockPhut { get; set; } = 30;
    public int SoBlock { get; set; }
    public decimal SoGio { get; set; }

    public DateTime NgayDat { get; set; }
    public TimeSpan GioBatDau { get; set; }
    public TimeSpan GioKetThuc { get; set; }

    /// <summary>Tiền sân chưa giảm (đơn giá x số giờ quy đổi theo block).</summary>
    public decimal TienGoc { get; set; }

    /// <summary>Khong | Voucher | KhuyenMai | CuoiTuan.</summary>
    public string LoaiGiamGia { get; set; } = Core.Common.LoaiGiamGia.Khong;
    public string TenUuDai { get; set; } = "Không áp dụng ưu đãi";
    public decimal PhanTramGiam { get; set; }
    public decimal TienGiam { get; set; }
    public decimal TongTien { get; set; }

    public Voucher VoucherDuocDung { get; set; }
    public KhuyenMai KhuyenMaiDuocDung { get; set; }

    public string TenLoaiGiamGia => Core.Common.LoaiGiamGia.TenHienThi(LoaiGiamGia);
    public bool CoGiamGia => TienGiam > 0;
}
