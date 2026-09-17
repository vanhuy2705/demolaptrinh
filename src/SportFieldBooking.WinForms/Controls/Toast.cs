using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Controls;

/// <summary>
/// Thông báo nhẹ dạng toast (snackbar) trượt mờ ở góc dưới-phải, TỰ biến mất,
/// KHÔNG chặn thao tác và KHÔNG cướp focus — dùng cho thông báo thành công.
/// (Xác nhận / cảnh báo / lỗi nghiêm trọng vẫn dùng frmThongBao modal.)
/// </summary>
public class Toast : Form
{
    public enum Loai { ThanhCong, ThongTin, CanhBao }

    private const int CaoToast = 54;
    private const int RongToast = 340;
    private const int BienManHinh = 18;
    private const int GapGiuaCacToast = 10;

    private static readonly List<Toast> _dangHien = new();
    private readonly Loai _loai;
    private readonly string _noiDung;
    private readonly System.Windows.Forms.Timer _timerTat;
    private readonly System.Windows.Forms.Timer _timerMo;
    private bool _dangTat;

    private Toast(string noiDung, Loai loai)
    {
        _noiDung = noiDung;
        _loai = loai;

        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        TopMost = true;
        StartPosition = FormStartPosition.Manual;
        Size = new Size(RongToast, CaoToast);
        BackColor = GiaoDien.BeMat;
        DoubleBuffered = true;
        Opacity = 0;

        // Không cướp focus, không hiện trong Alt-Tab.
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

        _timerMo = new System.Windows.Forms.Timer { Interval = 24 };
        _timerMo.Tick += (_, _) =>
        {
            Opacity = Math.Min(1, Opacity + 0.14);
            if (Opacity >= 1) _timerMo.Stop();
        };

        _timerTat = new System.Windows.Forms.Timer { Interval = 2600 };
        _timerTat.Tick += (_, _) => Tat();

        // Vẽ bo tròn + vạch màu ngữ cảnh + icon + chữ.
        Paint += (_, e) => Ve(e.Graphics);
    }

    protected override bool ShowWithoutActivation => true;

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= 0x08000000 /* WS_EX_NOACTIVATE */ | 0x00000080 /* WS_EX_TOOLWINDOW */;
            return cp;
        }
    }

    private Color MauNgucanh => _loai switch
    {
        Loai.ThanhCong => GiaoDien.ThanhCong,
        Loai.CanhBao => GiaoDien.CanhBao,
        _ => GiaoDien.ThongTin
    };

    private void Ve(Graphics g)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var vung = new Rectangle(0, 0, Width - 1, Height - 1);
        using var duong = GiaoDien.BoGoc(vung, 12);

        using (var nen = new LinearGradientBrush(vung,
                   GiaoDien.SangHon(BackColor, GiaoDien.LaThemeToi ? 6 : 2),
                   GiaoDien.ToiHon(BackColor, GiaoDien.LaThemeToi ? 4 : 1),
                   LinearGradientMode.Vertical))
            g.FillPath(nen, duong);

        using (var vien = new Pen(GiaoDien.Vien, 1f))
            g.DrawPath(vien, duong);

        // Vạch màu ngữ cảnh bo tròn bên trái.
        using (var duongVach = GiaoDien.BoGoc(new Rectangle(0, 10, 5, Height - 21), 2))
        using (var coVach = new SolidBrush(MauNgucanh))
            g.FillPath(coVach, duongVach);

        // Icon tròn nhỏ.
        var vungIcon = new Rectangle(18, (Height - 22) / 2, 22, 22);
        using (var coIcon = new SolidBrush(Color.FromArgb(45, MauNgucanh)))
            g.FillEllipse(coIcon, vungIcon);
        string tenIcon = _loai switch { Loai.ThanhCong => "check", Loai.CanhBao => "canhbao", _ => "thongtin" };
        Helpers.VeBieuTuong.Ve(g, tenIcon, new Rectangle(vungIcon.X + 4, vungIcon.Y + 4, 14, 14), MauNgucanh);

        var vungChu = new Rectangle(vungIcon.Right + 12, 0, Width - vungIcon.Right - 24, Height);
        TextRenderer.DrawText(g, _noiDung, GiaoDien.ChuThuong, vungChu, GiaoDien.Chu,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.WordBreak);
    }

    private void Tat()
    {
        if (_dangTat) return;
        _dangTat = true;
        _timerTat.Stop();
        _timerMo.Stop();

        var timer = new System.Windows.Forms.Timer { Interval = 24 };
        timer.Tick += (_, _) =>
        {
            Opacity = Math.Max(0, Opacity - 0.16);
            if (Opacity <= 0)
            {
                timer.Stop();
                _dangHien.Remove(this);
                XepLaiCacToastConLai();
                Close();
                Dispose();
            }
        };
        timer.Start();
    }

    private static void XepLaiCacToastConLai()
    {
        var manHinh = Screen.FromPoint(Cursor.Position).WorkingArea;
        int y = manHinh.Bottom - BienManHinh;
        for (int i = _dangHien.Count - 1; i >= 0; i--)
        {
            var t = _dangHien[i];
            y -= CaoToast;
            t.Left = manHinh.Right - BienManHinh - RongToast;
            t.Top = y;
            y -= GapGiuaCacToast;
        }
    }

    /// <summary>Hiện một toast. An toàn gọi từ thread nền (tự marshal về UI).</summary>
    public static void Hien(string noiDung, Loai loai = Loai.ThanhCong)
    {
        if (string.IsNullOrWhiteSpace(noiDung)) return;

        void Mo()
        {
            var toast = new Toast(noiDung.Trim(), loai);
            _dangHien.Add(toast);
            XepLaiCacToastConLai();
            toast.Show();
            toast._timerMo.Start();
            toast._timerTat.Start();
        }

        var owner = Form.ActiveForm;
        if (owner != null && owner.IsHandleCreated && owner.InvokeRequired)
            owner.BeginInvoke((Action)Mo);
        else
            Mo();
    }
}
