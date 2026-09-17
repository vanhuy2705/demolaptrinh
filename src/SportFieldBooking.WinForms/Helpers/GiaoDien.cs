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
        ManHinhNen = Color.FromArgb(245, 247, 250),         // #F5F7FA
        BeMat = Color.White,
        Vien = Color.FromArgb(228, 233, 240),               // #E4E9F0
        ONhap = Color.White,
        ONhapVien = Color.FromArgb(203, 213, 225),          // #CBD5E1

        ThanhBen = Color.FromArgb(15, 23, 42),              // #0F172A - xanh đậm sang trọng
        ThanhBenSang = Color.FromArgb(30, 41, 59),          // #1E293B
        ThanhBenChon = Color.FromArgb(47, 111, 237),        // #2F6FED

        Chinh = Color.FromArgb(47, 111, 237),               // #2F6FED - xanh azure
        ChinhDam = Color.FromArgb(29, 78, 216),             // #1D4ED8
        ChinhNhat = Color.FromArgb(232, 240, 254),          // #E8F0FE
        DiemNhan = Color.FromArgb(245, 158, 11),            // #F59E0B

        Chu = Color.FromArgb(15, 23, 42),                   // #0F172A
        ChuPhu = Color.FromArgb(90, 107, 130),              // #5A6B82
        ChuTrenNenDam = Color.White,

        NguyHiem = Color.FromArgb(220, 38, 38),             // #DC2626
        ThanhCong = Color.FromArgb(22, 163, 74),            // #16A34A
        CanhBao = Color.FromArgb(217, 119, 6),              // #D97706
        ThongTin = Color.FromArgb(37, 99, 235),             // #2563EB
        VienNguyHiem = Color.FromArgb(251, 201, 201),       // #FBC9C9

        LuoiTieuDe = Color.FromArgb(241, 245, 249),         // #F1F5F9
        LuoiTieuDeChu = Color.FromArgb(51, 65, 85),         // #334155
        LuoiChan = Color.FromArgb(250, 251, 252),           // #FAFBFC
        LuoiChon = Color.FromArgb(232, 240, 254),           // #E8F0FE
        LuoiChonChu = Color.FromArgb(15, 23, 42),
        LuoiVien = Color.FromArgb(233, 238, 245)            // #E9EEF5
    };

    // --- Chủ đề Tối: nền #282828, thẻ #333333, điểm nhấn tím #A080E0 (mặc định) ---
    private static readonly BangMau MauToi = new()
    {
        ManHinhNen = Color.FromArgb(17, 22, 29),            // #11161D - xám thanh ánh xanh
        BeMat = Color.FromArgb(26, 34, 44),                 // #1A222C - thẻ nổi lên
        Vien = Color.FromArgb(42, 53, 65),                  // #2A3541
        ONhap = Color.FromArgb(20, 26, 34),                 // #141A22 - ô nhập lõm xuống
        ONhapVien = Color.FromArgb(51, 64, 79),             // #33404F

        ThanhBen = Color.FromArgb(12, 16, 22),              // #0C1016 - thanh bên sâu nhất
        ThanhBenSang = Color.FromArgb(27, 36, 48),          // #1B2430
        ThanhBenChon = Color.FromArgb(22, 50, 78),          // #16324E - xanh azure trầm

        Chinh = Color.FromArgb(47, 111, 237),               // #2F6FED - azure chủ đạo
        ChinhDam = Color.FromArgb(36, 91, 208),             // #245BD0
        ChinhNhat = Color.FromArgb(22, 35, 58),             // #16233A
        DiemNhan = Color.FromArgb(245, 165, 36),            // #F5A524

        Chu = Color.FromArgb(231, 237, 245),                // #E7EDF5
        ChuPhu = Color.FromArgb(139, 154, 173),             // #8B9AAD
        ChuTrenNenDam = Color.FromArgb(243, 247, 252),      // #F3F7FC

        NguyHiem = Color.FromArgb(248, 113, 113),           // #F87171
        ThanhCong = Color.FromArgb(52, 211, 153),           // #34D399
        CanhBao = Color.FromArgb(251, 191, 36),             // #FBBF24
        ThongTin = Color.FromArgb(96, 165, 250),            // #60A5FA
        VienNguyHiem = Color.FromArgb(90, 42, 46),          // #5A2A2E

        LuoiTieuDe = Color.FromArgb(20, 26, 34),            // #141A22
        LuoiTieuDeChu = Color.FromArgb(231, 237, 245),      // #E7EDF5
        LuoiChan = Color.FromArgb(31, 40, 51),              // #1F2833
        LuoiChon = Color.FromArgb(28, 58, 94),              // #1C3A5E
        LuoiChonChu = Color.White,
        LuoiVien = Color.FromArgb(37, 47, 59)               // #252F3B
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
    public static Font ChuThuong => new(TenFont, 10F);
    public static Font ChuDam => new(TenFont, 10F, FontStyle.Bold);
    public static Font ChuLon => new(TenFont, 13F, FontStyle.Bold);
    public static Font TieuDe => new(TenFont, 17F, FontStyle.Bold);
    public static Font TieuDeLon => new(TenFont, 22F, FontStyle.Bold);

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

    /// <summary>Nút chính (thao tác tạo/lưu/thanh toán).</summary>
    public static void DangNutChinh(Button nut)
    {
        nut.FlatStyle = FlatStyle.Flat;
        nut.FlatAppearance.BorderSize = 0;
        nut.BackColor = Chinh;
        nut.ForeColor = ChuTrenNenDam;
        nut.Font = ChuDam;
        nut.Cursor = Cursors.Hand;
        nut.Height = 38;
        nut.MinimumSize = new Size(96, 34);
        nut.Padding = new Padding(10, 0, 10, 0);
        nut.FlatAppearance.MouseOverBackColor = ChinhDam;
    }

    /// <summary>Nút phụ (hủy, làm mới, xuất...).</summary>
    public static void DangNutPhu(Button nut)
    {
        nut.FlatStyle = FlatStyle.Flat;
        nut.FlatAppearance.BorderSize = 1;
        nut.FlatAppearance.BorderColor = Vien;
        nut.BackColor = BeMat;
        nut.ForeColor = Chu;
        nut.Font = ChuDam;
        nut.Cursor = Cursors.Hand;
        nut.Height = 38;
        nut.MinimumSize = new Size(96, 34);
        nut.Padding = new Padding(10, 0, 10, 0);
        nut.FlatAppearance.MouseOverBackColor = Vien;
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
