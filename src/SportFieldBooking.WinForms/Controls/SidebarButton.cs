using System.ComponentModel;
using System.Drawing.Drawing2D;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Controls;

/// <summary>Nút menu trên thanh bên: bo góc, có icon, đổi màu khi hover/đang chọn.</summary>
public partial class SidebarButton : Button
{
    private string _tenBieuTuong = "home";
    private bool _kichHoat;
    private bool _dangTro;
    private bool _laNutDangXuat;

    public SidebarButton()
    {
        InitializeComponent();
        GiaoDien.DangNutThanhBen(this);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Giao dien"), Description("Tên icon hiển thị")]
    public string TenBieuTuong
    {
        get => _tenBieuTuong;
        set { _tenBieuTuong = string.IsNullOrWhiteSpace(value) ? "home" : value.Trim().ToLowerInvariant(); Invalidate(); }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Giao dien"), Description("Đang được chọn")]
    public bool KichHoat
    {
        get => _kichHoat;
        set { _kichHoat = value; Invalidate(); }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Giao dien"), Description("Hiển thị màu cảnh báo (nút đăng xuất)")]
    public bool LaNutDangXuat
    {
        get => _laNutDangXuat;
        set { _laNutDangXuat = value; Invalidate(); }
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        Graphics g = pevent.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        Color mauNen = _kichHoat ? GiaoDien.ThanhBenChon : (_dangTro ? GiaoDien.ThanhBenSang : GiaoDien.ThanhBen);
        using (var coNen = new SolidBrush(mauNen))
            g.FillRectangle(coNen, ClientRectangle);

        if (_kichHoat)
            using (var coVach = new SolidBrush(GiaoDien.Chinh))
                g.FillRectangle(coVach, 0, 0, 5, Height);

        Color mauChu = _kichHoat ? Color.White : (_laNutDangXuat && _dangTro
            ? GiaoDien.NguyHiem
            : GiaoDien.ChuTrenNenDam);

        int canhIcon = Math.Min(22, Height - 16);
        var vungIcon = new Rectangle(16, (Height - canhIcon) / 2, canhIcon, canhIcon);
        VeBieuTuong.Ve(g, _tenBieuTuong, vungIcon, mauChu);

        var vungChu = new Rectangle(vungIcon.Right + 12, 0, Width - vungIcon.Right - 16, Height);
        TextRenderer.DrawText(g, Text, Font, vungChu, mauChu,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        _dangTro = true;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _dangTro = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        base.OnEnabledChanged(e);
        Invalidate();
    }
}
