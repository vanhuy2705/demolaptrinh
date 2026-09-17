using System.Drawing.Drawing2D;
using SportFieldBooking.WinForms.Controls;

namespace SportFieldBooking.WinForms.Helpers;

/// <summary>
/// Bảng màu, font và các hàm định dạng dùng chung cho toàn bộ giao diện.
/// Mọi Form đều gọi GiaoDien.ApDung(this) để đồng bộ phong cách.
/// Hỗ trợ 2 chủ đề: Sáng (mặc định WinForms) và Tối (nền #282828, điểm nhấn tím #A080E0).
/// Đổi chủ đề bằng GiaoDien.ChuyenTheme(true/false) TRƯỚC khi tạo Form.
/// </summary>
public static class GiaoDien
{
    /// <summary>Bảng màu lưu trọn bộ màu của một chủ đề.</summary>
    private sealed class BangMau
    {
        public Color ManHinhNen;    // Nền ứng dụng
        public Color BeMat;         // Nền thẻ / lưới / panel nội dung
        public Color Vien;          // Đường viền
        public Color ONhap;         // Nền ô nhập liệu
        public Color ONhapVien;     // Viền ô nhập liệu

        public Color ThanhBen;      // Nền thanh bên / đầu trang
        public Color ThanhBenSang;  // Nền thanh bên khi rê chuột
        public Color ThanhBenChon;  // Nền thanh bên khi đang chọn

        public Color Chinh;         // Màu chủ đạo (nút chính, tiêu điểm)
        public Color ChinhDam;      // Màu chủ đạo đậm (hover)
        public Color ChinhNhat;     // Màu chủ đạo nhạt (nền nhãn, thẻ phụ)
        public Color DiemNhan;      // Điểm nhấn (cảnh báo, huy hiệu)

        public Color Chu;           // Chữ chính
        public Color ChuPhu;        // Chữ phụ / mô tả
        public Color ChuTrenNenDam; // Chữ trên nền tối (thanh bên, đầu trang)

        public Color NguyHiem;
        public Color ThanhCong;
        public Color CanhBao;
        public Color ThongTin;
        public Color VienNguyHiem;  // Viền nút nguy hiểm

        public Color LuoiTieuDe;    // Nền tiêu đề cột
        public Color LuoiTieuDeChu; // Chữ tiêu đề cột
        public Color LuoiChan;      // Nền dòng chẵn
        public Color LuoiChon;      // Nền dòng đang chọn
        public Color LuoiChonChu;   // Chữ dòng đang chọn
        public Color LuoiVien;      // Kẻ ngang giữa các dòng
    }

    // --- Chủ đề Sáng: xanh teal (chủ đạo) + xanh lá (thể thao) + hổ phách (cảnh báo) ---
    private static readonly BangMau MauSang = new()
    {
        ManHinhNen = Color.FromArgb(246, 247, 251),         // #F6F7FB - sứ nhạt ánh lam
        BeMat = Color.White,
        Vien = Color.FromArgb(230, 233, 242),               // #E6E9F2
        ONhap = Color.FromArgb(251, 252, 254),              // #FBFCFE
        ONhapVien = Color.FromArgb(215, 220, 232),          // #D7DCE8

        ThanhBen = Color.FromArgb(18, 22, 43),              // #12162B - navy chàm sâu
        ThanhBenSang = Color.FromArgb(29, 35, 64),          // #1D2340
        ThanhBenChon = Color.FromArgb(79, 70, 229),         // #4F46E5

        Chinh = Color.FromArgb(79, 70, 229),                // #4F46E5 - chàm hiện đại
        ChinhDam = Color.FromArgb(67, 56, 202),             // #4338CA
        ChinhNhat = Color.FromArgb(238, 240, 255),          // #EEF0FF
        DiemNhan = Color.FromArgb(245, 158, 11),            // #F59E0B

        Chu = Color.FromArgb(20, 24, 43),                   // #14182B
        ChuPhu = Color.FromArgb(91, 100, 120),              // #5B6478
        ChuTrenNenDam = Color.White,

        NguyHiem = Color.FromArgb(220, 38, 38),
        ThanhCong = Color.FromArgb(5, 150, 105),
        CanhBao = Color.FromArgb(217, 119, 6),
        ThongTin = Color.FromArgb(37, 99, 235),
        VienNguyHiem = Color.FromArgb(246, 201, 201),

        LuoiTieuDe = Color.FromArgb(241, 243, 249),
        LuoiTieuDeChu = Color.FromArgb(58, 67, 88),
        LuoiChan = Color.FromArgb(250, 251, 253),
        LuoiChon = Color.FromArgb(236, 239, 254),
        LuoiChonChu = Color.FromArgb(20, 24, 43),
        LuoiVien = Color.FromArgb(236, 239, 245)
    };

    // --- Chủ đề Tối: nền #282828, thẻ #333333, điểm nhấn tím #A080E0 (mặc định) ---
    private static readonly BangMau MauToi = new()
    {
        ManHinhNen = Color.FromArgb(10, 14, 20),            // #0A0E14 - đêm sâu ánh lam
        BeMat = Color.FromArgb(20, 26, 34),                 // #141A22 - thẻ nổi
        Vien = Color.FromArgb(34, 43, 54),                  // #222B36
        ONhap = Color.FromArgb(13, 18, 25),                 // #0D1219 - ô nhập lõm
        ONhapVien = Color.FromArgb(43, 54, 68),             // #2B3644

        ThanhBen = Color.FromArgb(6, 9, 13),                // #06090D - thanh bên sâu nhất
        ThanhBenSang = Color.FromArgb(24, 32, 42),          // #18202A
        ThanhBenChon = Color.FromArgb(27, 42, 74),          // #1B2A4A - chàm trầm

        Chinh = Color.FromArgb(99, 102, 241),               // #6366F1 - chàm rực
        ChinhDam = Color.FromArgb(79, 70, 229),             // #4F46E5
        ChinhNhat = Color.FromArgb(29, 33, 64),             // #1D2140
        DiemNhan = Color.FromArgb(245, 158, 11),            // #F59E0B

        Chu = Color.FromArgb(232, 236, 243),                // #E8ECF3
        ChuPhu = Color.FromArgb(147, 160, 180),             // #93A0B4
        ChuTrenNenDam = Color.FromArgb(245, 248, 252),      // #F5F8FC

        NguyHiem = Color.FromArgb(248, 113, 113),
        ThanhCong = Color.FromArgb(52, 211, 153),
        CanhBao = Color.FromArgb(251, 191, 36),
        ThongTin = Color.FromArgb(96, 165, 250),
        VienNguyHiem = Color.FromArgb(74, 36, 40),

        LuoiTieuDe = Color.FromArgb(16, 22, 30),
        LuoiTieuDeChu = Color.FromArgb(221, 228, 238),
        LuoiChan = Color.FromArgb(24, 32, 42),
        LuoiChon = Color.FromArgb(35, 44, 85),
        LuoiChonChu = Color.White,
        LuoiVien = Color.FromArgb(30, 40, 51)
    };

    private static BangMau _mau = MauToi;      // Chủ đề Tối là mặc định
    private static bool _laThemeToi = true;

    /// <summary>Đang dùng chủ đề Tối hay không.</summary>
    public static bool LaThemeToi => _laThemeToi;

    /// <summary>Chuyển chủ đề. Gọi trước khi tạo Form (thường gọi trong Program.Main).</summary>
    public static void ChuyenTheme(bool toi)
    {
        _laThemeToi = toi;
        _mau = toi ? MauToi : MauSang;
    }

    // --- Truy cập màu theo chủ đề hiện tại ---
    public static Color ManHinhNen => _mau.ManHinhNen;
    public static Color BeMat => _mau.BeMat;
    public static Color Vien => _mau.Vien;
    public static Color ONhap => _mau.ONhap;
    public static Color ONhapVien => _mau.ONhapVien;

    public static Color ThanhBen => _mau.ThanhBen;
    public static Color ThanhBenSang => _mau.ThanhBenSang;
    public static Color ThanhBenChon => _mau.ThanhBenChon;

    public static Color Chinh => _mau.Chinh;
    public static Color ChinhDam => _mau.ChinhDam;
    public static Color ChinhNhat => _mau.ChinhNhat;
    public static Color DiemNhan => _mau.DiemNhan;

    public static Color Chu => _mau.Chu;
    public static Color ChuPhu => _mau.ChuPhu;
    public static Color ChuTrenNenDam => _mau.ChuTrenNenDam;

    public static Color NguyHiem => _mau.NguyHiem;
    public static Color ThanhCong => _mau.ThanhCong;
    public static Color CanhBao => _mau.CanhBao;
    public static Color ThongTin => _mau.ThongTin;
    public static Color VienNguyHiem => _mau.VienNguyHiem;

    public static Color LuoiTieuDe => _mau.LuoiTieuDe;
    public static Color LuoiTieuDeChu => _mau.LuoiTieuDeChu;
    public static Color LuoiChonChu => _mau.LuoiChonChu;
    public static Color LuoiChan => _mau.LuoiChan;
    public static Color LuoiChon => _mau.LuoiChon;
    public static Color LuoiVien => _mau.LuoiVien;

    // --- Font ---
    public const string TenFont = "Segoe UI";

    public static Font ChuNho => new(TenFont, 9F);
    public static Font ChuThuong => new(TenFont, 9.75F);
    public static Font ChuVua => new(TenFont, 10F, FontStyle.Bold);      // nhãn nút vừa
    public static Font ChuDam => new(TenFont, 10F, FontStyle.Bold);
    public static Font ChuLon => new(TenFont, 13.5F, FontStyle.Bold);
    public static Font TieuDe => new(TenFont, 17.5F, FontStyle.Bold);
    public static Font TieuDeLon => new(TenFont, 23F, FontStyle.Bold);
    public static Font SoLon => new(TenFont, 20F, FontStyle.Bold);       // số KPI

    /// <summary>Áp dụng font/màu nền chuẩn cho một Form (và toàn bộ control con, đệ quy).</summary>
    public static void ApDung(Form form)
    {
        form.BackColor = ManHinhNen;
        form.Font = ChuThuong;
        form.ForeColor = Chu;
        form.StartPosition = FormStartPosition.CenterScreen;
        // Không ép MinimumSize 1024x640: Form con được nhúng trong vùng nội dung có thể nhỏ hơn.
        // ResponsiveLayout sẽ xử lý bố cục/scroll thay vì ép cửa sổ vượt khỏi màn hình.
        ApDungCho(form);
    }

    /// <summary>Đổi chủ đề Sáng &lt;-&gt; Tối và dọn lại màu cho mọi cửa sổ đang mở (kể cả form con nhúng).</summary>
    public static void DoiChuDe()
    {
        ChuyenTheme(!_laThemeToi);
        CaiDatNguoiDung.DatTheme(_laThemeToi);   // nhớ lựa chọn cho lần chạy sau
        LamMoiTatCa();
    }

    /// <summary>Áp dụng lại bảng màu cho tất cả form đang mở (dùng sau khi đổi chủ đề).</summary>
    public static void LamMoiTatCa()
    {
        for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
        {
            Form f = Application.OpenForms[i];
            if (f == null || f.IsDisposed) continue;

            f.BackColor = ManHinhNen;
            f.ForeColor = Chu;
            f.Font = ChuThuong;
            ApDungCho(f);          // đệ quy: xử lý cả form con nhúng trong panel
            f.Refresh();
        }
    }

    /// <summary>Pha màu sáng hơn (phanTram 0-100).</summary>
    public static Color SangHon(Color mau, int phanTram)
    {
        int t = Math.Clamp(phanTram, 0, 100);
        return Color.FromArgb(mau.A,
            mau.R + (255 - mau.R) * t / 100,
            mau.G + (255 - mau.G) * t / 100,
            mau.B + (255 - mau.B) * t / 100);
    }

    /// <summary>Pha màu tối hơn (phanTram 0-100).</summary>
    public static Color ToiHon(Color mau, int phanTram)
    {
        int t = Math.Clamp(phanTram, 0, 100);
        return Color.FromArgb(mau.A,
            mau.R * (100 - t) / 100,
            mau.G * (100 - t) / 100,
            mau.B * (100 - t) / 100);
    }

    /// <summary>Tạo panel bo góc (dùng cho thẻ KPI, khung nhập liệu...).</summary>
    public static GraphicsPath BoGoc(Rectangle vung, int banKinh)
    {
        var duong = new GraphicsPath();
        int duongKinh = banKinh * 2;
        duong.AddArc(vung.X, vung.Y, duongKinh, duongKinh, 180, 90);
        duong.AddArc(vung.Right - duongKinh, vung.Y, duongKinh, duongKinh, 270, 90);
        duong.AddArc(vung.Right - duongKinh, vung.Bottom - duongKinh, duongKinh, duongKinh, 0, 90);
        duong.AddArc(vung.X, vung.Bottom - duongKinh, duongKinh, duongKinh, 90, 90);
        duong.CloseFigure();
        return duong;
    }

    /// <summary>Cắt nút thành hình bo tròn (Region) và giữ bo khi đổi kích thước.</summary>
    private static void BoGocNut(Button nut, int banKinh = 10)
    {
        nut.Resize -= NutDoiKichThuoc;
        nut.Resize += NutDoiKichThuoc;
        nut.Tag = "RESPONSIVE_RADIUS|" + banKinh;
        NutDoiKichThuoc(nut, EventArgs.Empty);
    }

    private static void NutDoiKichThuoc(object? sender, EventArgs e)
    {
        if (sender is not Button nut) return;
        int banKinh = 10;
        if (nut.Tag is string t && t.StartsWith("RESPONSIVE_RADIUS|", StringComparison.Ordinal)
            && int.TryParse(t.AsSpan(18), out int r)) banKinh = r;
        nut.Region = nut.ClientSize.Width > 2 && nut.ClientSize.Height > 2
            ? new Region(BoGoc(new Rectangle(0, 0, nut.ClientSize.Width, nut.ClientSize.Height), banKinh))
            : null;
    }

    /// <summary>Nút chính (thao tác tạo/lưu/thanh toán).</summary>
    public static void DangNutChinh(Button nut)
    {
        nut.FlatStyle = FlatStyle.Flat;
        nut.FlatAppearance.BorderSize = 0;
        nut.BackColor = Chinh;
        nut.ForeColor = ChuTrenNenDam;
        nut.Font = ChuVua;
        nut.Cursor = Cursors.Hand;
        nut.Height = 40;
        nut.MinimumSize = new Size(96, 36);
        nut.Padding = new Padding(14, 0, 14, 0);
        nut.FlatAppearance.MouseOverBackColor = ChinhDam;
        nut.FlatAppearance.MouseDownBackColor = ToiHon(ChinhDam, 10);
        BoGocNut(nut, 10);
    }

    /// <summary>Nút phụ (hủy, làm mới, xuất...).</summary>
    public static void DangNutPhu(Button nut)
    {
        nut.FlatStyle = FlatStyle.Flat;
        nut.FlatAppearance.BorderSize = 1;
        nut.FlatAppearance.BorderColor = ONhapVien;
        nut.BackColor = BeMat;
        nut.ForeColor = Chu;
        nut.Font = ChuVua;
        nut.Cursor = Cursors.Hand;
        nut.Height = 40;
        nut.MinimumSize = new Size(96, 36);
        nut.Padding = new Padding(14, 0, 14, 0);
        nut.FlatAppearance.MouseOverBackColor = LaThemeToi ? SangHon(BeMat, 7) : ToiHon(BeMat, 4);
        nut.FlatAppearance.MouseDownBackColor = LaThemeToi ? SangHon(BeMat, 12) : ToiHon(BeMat, 8);
        BoGocNut(nut, 10);
    }

    /// <summary>Nút nguy hiểm (xóa, hủy booking...).</summary>
    public static void DangNutNguyHiem(Button nut)
    {
        DangNutPhu(nut);
        nut.ForeColor = NguyHiem;
        nut.FlatAppearance.BorderColor = VienNguyHiem;
    }

    /// <summary>Nút nổi bật trên nền tối (thanh bên).</summary>
    public static void DangNutThanhBen(Button nut)
    {
        nut.FlatStyle = FlatStyle.Flat;
        nut.FlatAppearance.BorderSize = 0;
        nut.BackColor = ThanhBen;
        nut.ForeColor = ChuTrenNenDam;
        nut.Font = new Font(TenFont, 10.5F);
        nut.TextAlign = ContentAlignment.MiddleLeft;
        nut.TextImageRelation = TextImageRelation.ImageBeforeText;
        nut.ImageAlign = ContentAlignment.MiddleLeft;
        nut.Padding = new Padding(14, 0, 0, 0);
        nut.Height = 46;
        nut.Cursor = Cursors.Hand;
        nut.FlatAppearance.MouseOverBackColor = ThanhBenSang;
    }

    /// <summary>Ô nhập liệu chuẩn.</summary>
    public static void DangONhap(TextBox o)
    {
        o.BorderStyle = BorderStyle.FixedSingle;
        o.BackColor = ONhap;
        o.ForeColor = Chu;
        o.Font = ChuThuong;
    }

    /// <summary>Nhãn mô tả trường dữ liệu.</summary>
    public static void DangNhan(Label nhan)
    {
        nhan.ForeColor = ChuPhu;
        nhan.Font = ChuNho;
        nhan.AutoSize = true;
    }

    /// <summary>Tiêu đề trang (đầu mỗi Form).</summary>
    public static void DangTieuDeTrang(Label tieuDe)
    {
        tieuDe.ForeColor = Chu;
        tieuDe.Font = TieuDe;
        tieuDe.AutoSize = true;
    }

    /// <summary>
    /// Vẽ nền gradient dọc + quầng sáng màu chủ đạo + vài nét chéo mờ cho một panel
    /// (dùng cho panel thương hiệu màn đăng nhập / đầu trang). Gắn một lần, tự vẽ lại.
    /// </summary>
    public static void TrangTriDoc(Panel p)
    {
        if (p == null || Equals(p.Tag, "GRADIENT_DOC")) return;
        p.Tag = "GRADIENT_DOC";
        p.Paint += (_, e) =>
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = p.ClientRectangle;
            if (r.Width <= 0 || r.Height <= 0) return;

            using (var nen = new LinearGradientBrush(r,
                       SangHon(ThanhBen, 8), ToiHon(ThanhBen, 38), LinearGradientMode.Vertical))
                g.FillRectangle(nen, r);

            // Quầng sáng màu chủ đạo lệch về góc trên-trái.
            int cx = r.Width / 2, cy = (int)(r.Height * 0.30);
            using (var gp = new GraphicsPath())
            {
                gp.AddEllipse(cx - 260, cy - 260, 520, 520);
                using var pgb = new PathGradientBrush(gp);
                pgb.CenterColor = Color.FromArgb(64, Chinh);
                pgb.SurroundColors = new[] { Color.FromArgb(0, Chinh) };
                g.FillPath(pgb, gp);
            }

            // Vài nét chéo mờ gợi chuyển động (thể thao).
            using (var but = new Pen(Color.FromArgb(20, ChuTrenNenDam), 1.2f))
            {
                g.DrawLine(but, r.Width - 150, r.Height - 40, r.Width - 30, r.Height - 160);
                g.DrawLine(but, r.Width - 110, r.Height - 30, r.Width - 20, r.Height - 120);
            }
        };
    }

    /// <summary>Màu tương ứng với trạng thái sân/booking/hóa đơn.</summary>
    public static Color MauTrangThai(string trangThai) => trangThai switch
    {
        "Trong" or "DaThanhToan" or "HoanThanh" or "HoatDong" => ThanhCong,
        "DangThue" or "DangSuDung" or "ChuaThanhToan" or "DaDat" => CanhBao,
        "BaoTri" or "DaHuy" or "BiKhoa" or "TamNgung" => NguyHiem,
        _ => ChuPhu
    };

    // ------------------------------------------------------------------
    // Đệ quy dọn màu cho toàn bộ control con: giữ nguyên màu cấu trúc
    // (thanh bên, đầu trang, trạng thái), chỉ đổi màu nền/chữ trung tính.
    // ------------------------------------------------------------------
    private static void ApDungCho(Control cha)
    {
        foreach (Control c in cha.Controls)
        {
            switch (c)
            {
                case Button:
                    break;                                  // Đã được tạo kiểu riêng

                case DataGridView luoi:
                    Luoi.DoiMau(luoi);                      // Chỉ đổi màu, giữ nguyên cột/định dạng
                    break;

                case KpiCard the:
                    the.LamMoiMau();
                    break;

                case RoundedPanel khung:
                    khung.LamMoiMau();
                    break;

                case Form formCon:                          // Form con nhúng trong panel nội dung
                    formCon.BackColor = ManHinhNen;
                    formCon.ForeColor = Chu;
                    break;

                case TextBoxBase o:                         // TextBox / RichTextBox
                    if (LaNenTrungTinh(o)) o.BackColor = ONhap;
                    if (!LaMauTrangThai(o.ForeColor)) o.ForeColor = Chu;
                    break;

                case ComboBox cb:
                    if (LaNenTrungTinh(cb)) cb.BackColor = ONhap;
                    if (!LaMauTrangThai(cb.ForeColor)) cb.ForeColor = Chu;
                    cb.FlatStyle = FlatStyle.Flat;   // ComboBox chỉ hỗ trợ Flat (không có FlatAppearance)
                    break;

                case DateTimePicker dtp:
                    if (LaNenTrungTinh(dtp)) dtp.BackColor = ONhap;
                    if (!LaMauTrangThai(dtp.ForeColor)) dtp.ForeColor = Chu;
                    dtp.CalendarForeColor = Chu;
                    dtp.CalendarMonthBackground = BeMat;
                    dtp.CalendarTitleBackColor = ThanhBen;
                    dtp.CalendarTitleForeColor = ChuTrenNenDam;
                    dtp.CalendarTrailingForeColor = ChuPhu;
                    break;

                case NumericUpDown nud:
                    if (LaNenTrungTinh(nud)) nud.BackColor = ONhap;
                    if (!LaMauTrangThai(nud.ForeColor)) nud.ForeColor = Chu;
                    nud.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case ListBox lb:
                    if (LaNenTrungTinh(lb)) lb.BackColor = ONhap;
                    if (!LaMauTrangThai(lb.ForeColor)) lb.ForeColor = Chu;
                    break;

                case CheckBox:
                case RadioButton:
                case LinkLabel:
                    if (!LaMauTrangThai(c.ForeColor) && !LaMauSang(c.ForeColor)) c.ForeColor = Chu;
                    break;

                case Label lbl:
                    // Chỉ đổi những nhãn đang dùng màu chữ trung tính (đen/xám đậm hoặc chữ của
                    // bất kỳ chủ đề nào). Nhãn trắng trên thanh bên/đầu trang và nhãn trạng thái giữ nguyên.
                    if (!LaMauTrangThai(lbl.ForeColor) && LaChuTuDong(lbl.ForeColor)) lbl.ForeColor = Chu;
                    break;

                default:
                    if (LaNenTrungTinh(c)) c.BackColor = BeMat;
                    else if (CungMau(c.BackColor, ManHinhNen) || CungMau(c.BackColor, Vien)) c.BackColor = ManHinhNen;
                    break;
            }

            if (c.HasChildren) ApDungCho(c);
        }
    }

    private static bool CungMau(Color a, Color b) => a.ToArgb() == b.ToArgb();

    /// <summary>Nền trung tính (trắng/xám nhạt/xám đậm) - an toàn để đổi theo chủ đề.</summary>
    private static bool LaNenTrungTinh(Control c)
    {
        Color nen = c.BackColor;
        if (nen.A == 0) return false;                                   // Trong suốt: giữ nguyên
        if (CungMau(nen, SystemColors.Window) || CungMau(nen, SystemColors.Control)
            || CungMau(nen, Color.White) || CungMau(nen, Color.Transparent)) return true;

        foreach (BangMau mau in new[] { MauSang, MauToi })
        {
            if (CungMau(nen, mau.BeMat) || CungMau(nen, mau.ManHinhNen) || CungMau(nen, mau.ONhap)
                || CungMau(nen, mau.Vien) || CungMau(nen, mau.LuoiChan)) return true;
        }
        return false;
    }

    /// <summary>Đang là màu trạng thái (xanh/đỏ/cam/tím) - không đổi.</summary>
    private static bool LaMauTrangThai(Color mau)
    {
        foreach (BangMau bang in new[] { MauSang, MauToi })
        {
            if (CungMau(mau, bang.ThanhCong) || CungMau(mau, bang.NguyHiem) || CungMau(mau, bang.CanhBao)
                || CungMau(mau, bang.ThongTin) || CungMau(mau, bang.Chinh) || CungMau(mau, bang.DiemNhan))
                return true;
        }
        return false;
    }

    /// <summary>Màu sáng (trắng/bạc) - dùng cho chữ trên nền tối, không đổi.</summary>
    private static bool LaMauSang(Color mau) =>
        (mau.R + mau.G + mau.B) / 3 >= 200;

    /// <summary>
    /// Màu chữ trung tính: đen/xám đậm, hoặc đúng màu chữ chính/phụ của một trong hai chủ đề
    /// (cần cập nhật khi đổi chủ đề, nếu không chữ sẽ cùng màu với nền).
    /// </summary>
    private static bool LaChuTuDong(Color mau)
    {
        if (CungMau(mau, Color.Black) || CungMau(mau, SystemColors.ControlText)) return true;

        foreach (BangMau bang in new[] { MauSang, MauToi })
        {
            if (CungMau(mau, bang.Chu) || CungMau(mau, bang.ChuPhu)) return true;
        }
        return (mau.R + mau.G + mau.B) / 3 < 110;
    }
}
