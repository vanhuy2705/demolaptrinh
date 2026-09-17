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
        if (_doDayVien <= 0 || _banKinh <= 0) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle vung = ClientRectangle;
        vung.Width -= 1;
        vung.Height -= 1;
        using var duong = GiaoDien.BoGoc(vung, _banKinh);
        using var but = new Pen(_mauVien, _doDayVien);
        e.Graphics.DrawPath(but, duong);
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
