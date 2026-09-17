using System.ComponentModel;
using System.Drawing.Drawing2D;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Controls;

/// <summary>Panel bo góc, có thể đổ bóng nhẹ bằng viền - dùng làm thẻ KPI, khung nhập liệu.</summary>
public partial class RoundedPanel : Panel
{
    private int _banKinh = 14;
    private Color _mauVien = GiaoDien.Vien;
    private bool _dungVienMacDinh = true;
    private int _doDayVien = 1;

    public RoundedPanel()
    {
        InitializeComponent();
        BackColor = GiaoDien.BeMat;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Giao dien"), Description("Bán kính bo góc")]
    public int BanKinh
    {
        get => _banKinh;
        set { _banKinh = Math.Max(0, value); CapNhatVung(); Invalidate(); }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Giao dien"), Description("Màu viền")]
    public Color MauVien
    {
        get => _mauVien;
        set { _mauVien = value; _dungVienMacDinh = value.ToArgb() == GiaoDien.Vien.ToArgb(); Invalidate(); }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Giao dien"), Description("Độ dày viền (0 = không vẽ)")]
    public int DoDayVien
    {
        get => _doDayVien;
        set { _doDayVien = Math.Max(0, value); Invalidate(); }
    }

    protected override void OnResize(EventArgs eventargs)
    {
        base.OnResize(eventargs);
        CapNhatVung();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        if (_banKinh <= 0) return;

        Rectangle vung = ClientRectangle;
        vung.Width -= 1;
        vung.Height -= 1;
        using var duong = GiaoDien.BoGoc(vung, _banKinh);

        // Nền gradient dọc rất nhẹ: sáng hơn ở đỉnh -> tạo cảm giác thẻ nổi, có chiều sâu.
        using (var nen = new LinearGradientBrush(ClientRectangle,
                   GiaoDien.SangHon(BackColor, GiaoDien.LaThemeToi ? 5 : 2),
                   GiaoDien.ToiHon(BackColor, GiaoDien.LaThemeToi ? 3 : 1),
                   LinearGradientMode.Vertical))
            g.FillPath(nen, duong);

        // Ánh sáng viền trong ở mép trên (giống nguồn sáng từ trên chiếu xuống).
        if (ClientRectangle.Height > 6)
        {
            using var butSang = new Pen(Color.FromArgb(GiaoDien.LaThemeToi ? 26 : 60, Color.White), 1.4f);
            using var duongSang = GiaoDien.BoGoc(new Rectangle(1, 1, ClientRectangle.Width - 3, ClientRectangle.Height - 3), Math.Max(2, _banKinh - 1));
            g.SetClip(duongSang);
            g.DrawPath(butSang, duongSang);
            g.ResetClip();
        }

        if (_doDayVien > 0)
            using (var but = new Pen(_mauVien, _doDayVien))
                g.DrawPath(but, duong);
    }

    private void CapNhatVung()
    {
        if (_banKinh <= 0 || Width <= 0 || Height <= 0)
        {
            Region = null;
            return;
        }
        using GraphicsPath duong = GiaoDien.BoGoc(ClientRectangle, _banKinh);
        Region = new Region(duong);
    }

    /// <summary>Cập nhật lại màu viền theo chủ đề hiện tại (chỉ khi đang dùng màu mặc định).</summary>
    public void LamMoiMau()
    {
        if (!_dungVienMacDinh) return;
        _mauVien = GiaoDien.Vien;
        Invalidate();
    }
}
