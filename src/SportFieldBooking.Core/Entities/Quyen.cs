namespace SportFieldBooking.Core.Entities;

/// <summary>Bảng QUYEN: danh mục quyền trong hệ thống.</summary>
public class Quyen
{
    public string MaQuyen { get; set; } = "";
    public string TenQuyen { get; set; } = "";
    public string NhomQuyen { get; set; } = "";
    public string MoTa { get; set; } = "";
}

/// <summary>Bảng VAI_TRO: danh mục vai trò.</summary>
public class VaiTroDb
{
    public string MaVaiTro { get; set; } = "";
    public string TenVaiTro { get; set; } = "";
    public string MoTa { get; set; } = "";
    public bool LaMacDinh { get; set; }
}

/// <summary>Bảng VAI_TRO_QUYEN: ma trận phân quyền.</summary>
public class VaiTroQuyen
{
    public string MaVaiTro { get; set; } = "";
    public string MaQuyen { get; set; } = "";
    public string TenVaiTro { get; set; } = "";
    public string TenQuyen { get; set; } = "";
    public string NhomQuyen { get; set; } = "";
}
