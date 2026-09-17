namespace SportFieldBooking.Core.Entities;

/// <summary>Bảng LOAI_SAN: loại sân (sân 5, sân 7, sân 11, cầu lông...).</summary>
public class LoaiSan
{
    public int MaLoaiSan { get; set; }
    public string TenLoaiSan { get; set; } = "";
    public string MoTa { get; set; } = "";

    /// <summary>Số sân đang thuộc loại này (chỉ phục vụ hiển thị).</summary>
    public int SoLuongSan { get; set; }
}
