namespace SportFieldBooking.Core.Entities;

/// <summary>Bảng SU_DUNG_VOUCHER: lịch sử sử dụng voucher gắn với một booking.</summary>
public class SuDungVoucher
{
    public int MaSuDung { get; set; }
    public int MaVoucher { get; set; }
    public int MaDat { get; set; }
    public int MaKH { get; set; }
    public decimal SoTienGiam { get; set; }
    public DateTime NgaySuDung { get; set; }

    public string MaCode { get; set; } = "";
    public string TenKH { get; set; } = "";
}
