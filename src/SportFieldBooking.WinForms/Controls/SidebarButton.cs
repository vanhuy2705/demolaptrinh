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

        // Nền trong suốt để lộ màu thanh bên; chỉ vẽ "viên" khi hover/đang chọn.
        using (var coNen = new SolidBrush(GiaoDien.ThanhBen))
            g.FillRectangle(coNen, ClientRectangle);

        int bien = 6;
        var vungVien = new Rectangle(bien, 3, Width - bien * 2, Height - 6);
        int banKinh = Math.Min(12, vungVien.Height / 2);

        if (_kichHoat || _dangTro)
        {
            using var duong = GiaoDien.BoGoc(vungVien, banKinh);
            if (_kichHoat)
            {
                using var nenChon = new LinearGradientBrush(vungVien,
                    GiaoDien.SangHon(GiaoDien.ThanhBenChon, 10),
                    GiaoDien.ToiHon(GiaoDien.ThanhBenChon, 12),
                    LinearGradientMode.Horizontal);
                g.FillPath(nenChon, duong);
                using var vienChon = new Pen(Color.FromArgb(60, GiaoDien.Chinh), 1f);
                g.DrawPath(vienChon, duong);
            }
            else
            {
                using var coTro = new SolidBrush(GiaoDien.ThanhBenSang);
                g.FillPath(coTro, duong);
            }
        }

        // Vạch chỉ báo màu chủ đạo bo tròn ở mép trái khi đang chọn.
        if (_kichHoat)
        {
            var vach = new Rectangle(0, Height / 2 - 12, 4, 24);
            using var duongVach = GiaoDien.BoGoc(vach, 2);
            using var coVach = new SolidBrush(GiaoDien.Chinh);
            g.FillPath(coVach, duongVach);
        }

        bool mauCam = _laNutDangXuat && (_dangTro || _kichHoat);
        Color mauChu = _kichHoat ? Color.White
            : mauCam ? GiaoDien.NguyHiem
            : GiaoDien.ChuTrenNenDam;

        // Icon đặt trong "chip" bo tròn nhuộm màu chủ đạo khi đang chọn.
        int canhChip = Math.Min(30, Height - 14);
        var vungChip = new Rectangle(14, (Height - canhChip) / 2, canhChip, canhChip);
        if (_kichHoat)
        {
            using var duongChip = GiaoDien.BoGoc(vungChip, 8);
            using var coChip = new SolidBrush(Color.FromArgb(55, GiaoDien.Chinh));
            g.FillPath(coChip, duongChip);
        }
        int canhIcon = canhChip - 12;
        var vungIcon = new Rectangle(vungChip.X + (canhChip - canhIcon) / 2,
                                     vungChip.Y + (canhChip - canhIcon) / 2, canhIcon, canhIcon);
        VeBieuTuong.Ve(g, _tenBieuTuong, vungIcon, _kichHoat ? GiaoDien.Chinh : mauChu);

        var vungChu = new Rectangle(vungChip.Right + 10, 0, Math.Max(10, Width - vungChip.Right - 14), Height);
        TextRenderer.DrawText(g, Text, _kichHoat ? GiaoDien.ChuVua : Font, vungChu, mauChu,
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
