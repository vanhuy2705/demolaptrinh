namespace SportFieldBooking.Core.Entities;

/// <summary>Bảng THAM_SO: tham số cấu hình hệ thống (giảm cuối tuần, giờ mở cửa...).</summary>
public class ThamSo
{
    public string TenThamSo { get; set; } = "";
    public string GiaTri { get; set; } = "";
    public string MoTa { get; set; } = "";
}
