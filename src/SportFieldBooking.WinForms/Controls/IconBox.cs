using System.ComponentModel;
using System.Drawing.Drawing2D;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Controls;

/// <summary>Ô hiển thị icon vector (vẽ bằng VeBieuTuong, không cần file ảnh).</summary>
public partial class IconBox : Control
{
    private string _tenBieuTuong = "home";
    private Color _mauBieuTuong = GiaoDien.Chinh;

    public IconBox()
    {
        InitializeComponent();
        ForeColor = GiaoDien.Chinh;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Giao dien"), Description("Tên icon: home, calendar, san, users, hoadon, voucher...")]
    public string TenBieuTuong
    {
        get => _tenBieuTuong;
        set { _tenBieuTuong = string.IsNullOrWhiteSpace(value) ? "home" : value.Trim().ToLowerInvariant(); Invalidate(); }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Giao dien"), Description("Màu icon")]
    public Color MauBieuTuong
    {
        get => _mauBieuTuong;
        set { _mauBieuTuong = value; Invalidate(); }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        int canh = Math.Min(Width, Height);
        int x = (Width - canh) / 2;
        int y = (Height - canh) / 2;
        VeBieuTuong.Ve(e.Graphics, _tenBieuTuong, new Rectangle(x, y, canh, canh), _mauBieuTuong);
    }
}
