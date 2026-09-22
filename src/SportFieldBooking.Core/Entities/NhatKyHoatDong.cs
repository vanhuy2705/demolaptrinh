namespace SportFieldBooking.Core.Entities;

/// <summary>Bảng NHAT_KY_HOAT_DONG: audit trail ghi lại mọi thao tác quan trọng.</summary>
public class NhatKyHoatDong
{
    public long MaNhatKy { get; set; }
    public int? MaTK { get; set; }
    public string TenDangNhap { get; set; } = "";
    public string VaiTro { get; set; } = "";
    public string HoatDong { get; set; } = "";
    public string BangDuLieu { get; set; } = "";
    public string MaDuLieu { get; set; } = "";
    public string NoiDung { get; set; } = "";
    public string KetQua { get; set; } = Core.Common.KetQuaNhatKy.ThanhCong;
    public DateTime ThoiGian { get; set; } = DateTime.Now;
    public string MayTram { get; set; } = "";

    // Thuộc tính hiển thị join
    public string HoTen { get; set; } = "";
}
