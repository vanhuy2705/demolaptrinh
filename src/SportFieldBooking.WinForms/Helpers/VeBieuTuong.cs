using System.Drawing.Drawing2D;

namespace SportFieldBooking.WinForms.Helpers;

/// <summary>
/// Bộ icon vector vẽ bằng GDI+ trên lưới thiết kế 24x24 (phong cách Lucide / Feather):
/// nét vẽ đồng nhất 1.75/24, đầu nét bo tròn, không phụ thuộc font icon hay file ảnh
/// nên sắc nét ở mọi DPI và chạy được trên mọi máy Windows.
/// </summary>
public static class VeBieuTuong
{
    public static readonly string[] DanhSachTen =
    {
        // Điều hướng
        "home", "dashboard", "thongke", "bieudo", "chart", "calendar", "lich", "clock",
        // Sân & danh mục
        "san", "sanbong", "field", "loaisan", "ball", "building",
        // Người dùng
        "user", "users", "taikhoan", "nhanvien", "khachhang",
        // Giao dịch
        "hoadon", "invoice", "voucher", "ticket", "khuyenmai", "gift", "cash", "thanhtoan",
        // Hệ thống
        "cauhinh", "settings", "dangxuat", "logout", "key", "khoa", "lock",
        // Thao tác
        "search", "timkiem", "plus", "them", "edit", "sua", "trash", "xoa", "save", "luu",
        "print", "in", "refresh", "check", "back", "filter", "eye", "note",
        // Trạng thái & khác
        "canhbao", "warning", "phone", "dienthoai", "star", "percent", "tag", "moon", "sun", "chude"
    };

    /// <summary>Vẽ icon <paramref name="ten"/> vào vùng <paramref name="vung"/> với màu <paramref name="mau"/>.</summary>
    public static void Ve(Graphics g, string ten, Rectangle vung, Color mau)
    {
        if (g == null || vung.Width <= 1 || vung.Height <= 1) return;

        SmoothingMode cu = g.SmoothingMode;
        PixelOffsetMode cuLech = g.PixelOffsetMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;

        float s = Math.Min(vung.Width, vung.Height) / 24f;
        float ox = vung.X + (vung.Width - 24f * s) / 2f;
        float oy = vung.Y + (vung.Height - 24f * s) / 2f;

        using var but = new Pen(mau, Math.Max(1.15f, 1.75f * s))
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round,
            LineJoin = LineJoin.Round,
            MiterLimit = 2f
        };
        using var co = new SolidBrush(mau);

        var b = new ButVe(g, but, co, s, ox, oy);

        switch ((ten ?? "").Trim().ToLowerInvariant())
        {
            // ---------------- Điều hướng ----------------
            case "home":
                b.Poly(false, 3, 10.5f, 12, 3.5f, 21, 10.5f, 21, 20.5f, 3, 20.5f);
                b.Round(false, 9.4f, 13.6f, 5.2f, 6.9f, 1.2f);
                break;

            case "dashboard":
                b.Round(true, 3.4f, 3.4f, 7.4f, 7.4f, 1.6f);
                b.Round(true, 13.2f, 3.4f, 7.4f, 7.4f, 1.6f);
                b.Round(true, 3.4f, 13.2f, 7.4f, 7.4f, 1.6f);
                b.Round(true, 13.2f, 13.2f, 7.4f, 7.4f, 1.6f);
                break;

            case "thongke":
                b.Line(3.2f, 20.4f, 20.8f, 20.4f);
                b.Round(true, 4.2f, 13.4f, 3.8f, 7f, 1f);
                b.Round(true, 10.1f, 9.6f, 3.8f, 10.8f, 1f);
                b.Round(true, 16f, 11.6f, 3.8f, 8.8f, 1f);
                break;

            case "bieudo":
            case "chart":
                b.Line(3.2f, 20.4f, 20.8f, 20.4f);
                b.Poly(false, 3.6f, 16.4f, 8.2f, 11.2f, 12f, 14.6f, 16.6f, 7.6f, 20.8f, 10.4f);
                b.Dot(16.6f, 7.6f, 1.5f);
                break;

            case "calendar":
            case "lich":
                b.Round(false, 3.6f, 5.2f, 16.8f, 15.8f, 2.2f);
                b.Line(3.6f, 9.6f, 20.4f, 9.6f);
                b.Line(8f, 3.2f, 8f, 6f);
                b.Line(16f, 3.2f, 16f, 6f);
                b.Dot(8.2f, 13.2f, 1.15f);
                b.Dot(12f, 13.2f, 1.15f);
                b.Dot(15.8f, 13.2f, 1.15f);
                b.Dot(8.2f, 17.2f, 1.15f);
                b.Dot(12f, 17.2f, 1.15f);
                break;

            case "clock":
                b.Circle(false, 12, 12, 8.6f);
                b.Line(12, 12, 12, 7.2f);
                b.Line(12, 12, 15.9f, 13.6f);
                break;

            // ---------------- Sân & danh mục ----------------
            case "san":
            case "field":
                b.Round(false, 3, 5.6f, 18, 14.8f, 2.4f);
                b.Line(12, 5.6f, 12, 20.4f);
                b.Circle(false, 12, 13, 3.6f);
                b.Dot(12, 13, 1.1f);
                break;

            case "sanbong":
                b.Circle(false, 12, 12, 8.8f);
                b.Poly(false, 12, 6.6f, 16.4f, 9.4f, 15.1f, 15.2f, 8.9f, 15.2f, 7.6f, 9.4f);
                break;

            case "loaisan":
                b.Poly(false, 12, 3f, 21f, 7.6f, 12f, 12.2f, 3f, 7.6f);
                b.Poly(false, 12, 12.2f, 21f, 16.8f, 12f, 21.4f, 3f, 16.8f);
                break;

            case "ball":
                b.Circle(false, 12, 12, 8.8f);
                b.Ellipse(false, 3.2f, 7.4f, 17.6f, 9.2f);
                b.Line(3.2f, 12f, 20.8f, 12f);
                b.Arc(8f, 3.2f, 8f, 17.6f, 180, 180);
                break;

            case "building":
                b.Poly(false, 4.6f, 20.6f, 4.6f, 3.4f, 19.4f, 3.4f, 19.4f, 20.6f);
                b.Line(4.6f, 8.2f, 19.4f, 8.2f);
                b.RectF(7.4f, 10.6f, 2.4f, 2.6f);
                b.RectF(10.8f, 10.6f, 2.4f, 2.6f);
                b.RectF(14.2f, 10.6f, 2.4f, 2.6f);
                b.RectF(7.4f, 15f, 2.4f, 2.6f);
                b.RectF(14.2f, 15f, 2.4f, 2.6f);
                break;

            // ---------------- Người dùng ----------------
            case "user":
            case "khachhang":
                b.Circle(false, 12, 8.2f, 3.9f);
                b.Arc(4.6f, 12.4f, 14.8f, 14.2f, 180, 180);
                break;

            case "users":
                b.Circle(false, 9.4f, 8f, 3.6f);
                b.Arc(3.4f, 12.6f, 12f, 11.6f, 180, 180);
                b.Poly(false, 16.2f, 5.4f, 20.6f, 6.4f, 20.6f, 11.4f, 17.8f, 12.4f);
                b.Arc(14.6f, 14.4f, 8.6f, 8.4f, 180, 180);
                break;

            case "taikhoan":
                b.Round(false, 2.8f, 5f, 18.4f, 14.4f, 2.6f);
                b.Circle(false, 8.4f, 11f, 2.6f);
                b.Arc(6.2f, 14.4f, 4.4f, 5.2f, 180, 180);
                b.Line(14f, 10.2f, 18.2f, 10.2f);
                b.Line(14f, 13.6f, 18.2f, 13.6f);
                break;

            case "nhanvien":
                b.Circle(false, 9.6f, 8.2f, 3.6f);
                b.Arc(3.6f, 12.6f, 12f, 11.4f, 180, 180);
                b.Circle(true, 17.8f, 17.8f, 4.2f);
                b.Poly(false, 16.2f, 17.8f, 17.4f, 19f, 19.4f, 16.4f);
                break;

            // ---------------- Giao dịch ----------------
            case "hoadon":
            case "invoice":
                b.Poly(false, 5.4f, 3.2f, 18.6f, 3.2f, 18.6f, 20.8f, 15.8f, 19.1f, 13f, 20.8f,
                                10.2f, 19.1f, 7.4f, 20.8f, 5.4f, 20.8f);
                b.Line(9f, 8f, 15f, 8f);
                b.Line(9f, 11.8f, 15f, 11.8f);
                b.Line(9f, 15.6f, 13.2f, 15.6f);
                break;

            case "voucher":
            case "ticket":
                b.Round(false, 2.8f, 6.6f, 18.4f, 10.8f, 2.4f);
                b.Dash(15.6f, 8.4f, 15.6f, 15.6f, 4);
                b.Circle(false, 8.6f, 12f, 1.9f);
                b.Line(11.4f, 12f, 13.4f, 12f);
                break;

            case "khuyenmai":
            case "gift":
                b.Round(false, 3.4f, 9.2f, 17.2f, 11.4f, 2.2f);
                b.Line(3.4f, 13.4f, 20.6f, 13.4f);
                b.Line(12f, 9.2f, 12f, 20.6f);
                b.Arc(7.6f, 4.8f, 4.4f, 4.8f, 180, 180);
                b.Arc(12f, 4.8f, 4.4f, 4.8f, 180, 180);
                break;

            case "cash":
            case "thanhtoan":
                b.Round(false, 2.6f, 6.4f, 18.8f, 11.2f, 2.4f);
                b.Circle(false, 12f, 12f, 2.9f);
                b.Line(5.6f, 9.2f, 5.6f, 14.8f);
                b.Line(18.4f, 9.2f, 18.4f, 14.8f);
                break;

            // ---------------- Hệ thống ----------------
            case "cauhinh":
            case "settings":
                b.Circle(false, 12f, 12f, 3.1f);
                b.Circle(false, 12f, 12f, 8.2f);
                for (int i = 0; i < 8; i++)
                {
                    double goc = i * Math.PI / 4;
                    float r1 = 5.1f, r2 = 8.2f;
                    b.Line(12f + r1 * (float)Math.Cos(goc), 12f + r1 * (float)Math.Sin(goc),
                           12f + r2 * (float)Math.Cos(goc), 12f + r2 * (float)Math.Sin(goc));
                }
                break;

            case "dangxuat":
            case "logout":
                b.Round(false, 3.4f, 3.4f, 11.2f, 17.2f, 2.2f);
                b.Line(20.8f, 12f, 10.4f, 12f);
                b.Poly(false, 15.6f, 7f, 20.8f, 12f, 15.6f, 17f);
                break;

            case "key":
                b.Circle(false, 8.2f, 12f, 4.4f);
                b.Line(12.6f, 12f, 20.8f, 12f);
                b.Line(16.6f, 12f, 16.6f, 15.2f);
                b.Line(19.6f, 12f, 19.6f, 14.4f);
                break;

            case "khoa":
            case "lock":
                b.Round(false, 4.4f, 10.4f, 15.2f, 10.4f, 2.4f);
                b.Arc(7.6f, 3.4f, 8.8f, 11.6f, 180, 180);
                b.Line(12f, 14.4f, 12f, 16.6f);
                break;

            // ---------------- Thao tác ----------------
            case "search":
            case "timkiem":
                b.Circle(false, 10.6f, 10.6f, 6.6f);
                b.Line(15.4f, 15.4f, 20.6f, 20.6f);
                break;

            case "plus":
            case "them":
                b.Line(12f, 4.6f, 12f, 19.4f);
                b.Line(4.6f, 12f, 19.4f, 12f);
                break;

            case "edit":
            case "sua":
                b.Poly(false, 4.4f, 19.6f, 4.4f, 15.2f, 16.4f, 3.2f, 20.8f, 7.6f, 8.6f, 19.6f);
                b.Line(14.6f, 5f, 18.2f, 8.6f);
                break;

            case "trash":
            case "xoa":
                b.Line(3.4f, 6.6f, 20.6f, 6.6f);
                b.Poly(false, 5.4f, 6.6f, 5.4f, 20.2f, 18.6f, 20.2f, 18.6f, 6.6f);
                b.Arc(9f, 6.6f, 6f, 6.4f, 180, 180);
                b.Line(9.8f, 10.6f, 9.8f, 16.6f);
                b.Line(14.2f, 10.6f, 14.2f, 16.6f);
                break;

            case "save":
            case "luu":
                b.Round(false, 3.4f, 3.4f, 17.2f, 17.2f, 2.6f);
                b.RectF(7.4f, 3.4f, 9.2f, 5.6f);
                b.Round(false, 7.4f, 12.8f, 9.2f, 7.8f, 1.4f);
                break;

            case "print":
            case "in":
                b.Round(false, 7f, 3.4f, 10f, 6f, 1.2f);
                b.Round(false, 3.4f, 9.4f, 17.2f, 6.6f, 2f);
                b.Round(false, 7f, 14.4f, 10f, 6.4f, 1.2f);
                break;

            case "refresh":
                b.Arc(3.4f, 3.4f, 17.2f, 17.2f, 0, 315);
                b.Poly(false, 20.8f, 2.8f, 20.8f, 9f, 14.6f, 9f);
                break;

            case "check":
                b.Poly(false, 4.4f, 12.6f, 9.4f, 17.6f, 19.6f, 6.4f);
                break;

            case "back":
                b.Line(20.4f, 12f, 4.6f, 12f);
                b.Poly(false, 11f, 5.8f, 4.6f, 12f, 11f, 18.2f);
                break;

            case "filter":
                b.Poly(false, 3.2f, 4.6f, 20.8f, 4.6f, 13.4f, 13.2f, 13.4f, 20.6f);
                break;

            case "eye":
                b.Arc(2.4f, 6.6f, 19.2f, 10.8f, 180, 180);
                b.Arc(2.4f, 6.6f, 19.2f, 10.8f, 0, 180);
                b.Circle(false, 12f, 12f, 3f);
                break;

            case "note":
                b.Poly(false, 13.6f, 3.2f, 6.4f, 3.2f, 4.4f, 5.4f, 4.4f, 19.4f, 6.4f, 21.4f,
                                17.6f, 21.4f, 19.6f, 19.4f, 19.6f, 9.4f);
                b.Poly(false, 13.6f, 3.2f, 13.6f, 9.4f, 19.6f, 9.4f);
                b.Line(7.8f, 13.2f, 16.2f, 13.2f);
                b.Line(7.8f, 16.8f, 13.4f, 16.8f);
                break;

            // ---------------- Trạng thái & khác ----------------
            case "thongtin":
            case "info":
                b.Circle(false, 12f, 12f, 8.8f);
                b.Dot(12f, 8f, 1.1f);
                b.Line(12f, 11.4f, 12f, 17.6f);
                break;

            case "canhbao":
            case "warning":
                b.Poly(false, 12f, 3.2f, 21.6f, 20.4f, 2.4f, 20.4f);
                b.Line(12f, 9f, 12f, 14.2f);
                b.Dot(12f, 17.2f, 1.15f);
                break;

            case "phone":
            case "dienthoai":
                b.Arc(5.4f, 3.6f, 13.2f, 16.8f, 42f, 96f);
                b.Arc(5.4f, 3.6f, 13.2f, 16.8f, 222f, 96f);
                b.Line(5.4f, 8.2f, 7.4f, 12f);
                b.Line(18.6f, 15.8f, 16.6f, 12f);
                break;

            case "star":
                b.Star(12f, 12.4f, 9.2f, 4.1f, 5);
                break;

            case "moon":
            {
                const float rNgoai = 8.5f, rTrong = 8.41f;
                using var duong = new GraphicsPath();
                duong.AddArc(ox + (12f - rNgoai) * s, oy + (12f - rNgoai) * s, rNgoai * 2 * s, rNgoai * 2 * s, -70, 140);
                duong.AddArc(ox + (17.5f - rTrong) * s, oy + (12f - rTrong) * s, rTrong * 2 * s, rTrong * 2 * s, 108, 144);
                g.DrawPath(but, duong);
                break;
            }

            case "sun":
                b.Circle(false, 12f, 12f, 4.2f);
                for (int i = 0; i < 8; i++)
                {
                    double goc = i * Math.PI / 4;
                    b.Line(12f + 6.7f * (float)Math.Cos(goc), 12f + 6.7f * (float)Math.Sin(goc),
                           12f + 9.3f * (float)Math.Cos(goc), 12f + 9.3f * (float)Math.Sin(goc));
                }
                break;

            case "chude":
                b.Circle(false, 12f, 12f, 8.6f);
                b.Arc(12f, 3.4f, 8.6f, 17.2f, 90, 180);
                b.Dot(12f, 12f, 2.4f);
                break;

            case "percent":
                b.Line(19.4f, 4.6f, 4.6f, 19.4f);
                b.Circle(false, 7.8f, 7.8f, 2.7f);
                b.Circle(false, 16.2f, 16.2f, 2.7f);
                break;

            case "tag":
                b.Poly(false, 11.2f, 3.4f, 20.6f, 3.4f, 20.6f, 12.8f, 12.9f, 20.6f, 3.4f, 11.2f);
                b.Circle(false, 16.6f, 8f, 1.7f);
                break;

            default:
                b.Circle(false, 12f, 12f, 8.6f);
                b.Dot(12f, 12f, 2.2f);
                break;
        }

        g.SmoothingMode = cu;
        g.PixelOffsetMode = cuLech;
    }

    /// <summary>Tạo ảnh bitmap của icon (dùng làm Image cho Button/PictureBox).</summary>
    public static Bitmap TaoAnh(string ten, int kichThuoc, Color mau)
    {
        var anh = new Bitmap(Math.Max(1, kichThuoc), Math.Max(1, kichThuoc));
        using (Graphics g = Graphics.FromImage(anh))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Ve(g, ten, new Rectangle(0, 0, anh.Width, anh.Height), mau);
        }
        return anh;
    }

    /// <summary>Bút vẽ trên lưới 24x24: mọi toạ độ truyền vào đều tính theo đơn vị lưới.</summary>
    private readonly struct ButVe
    {
        private readonly Graphics _g;
        private readonly Pen _but;
        private readonly Brush _co;
        private readonly float _s, _ox, _oy;

        public ButVe(Graphics g, Pen but, Brush co, float s, float ox, float oy)
        {
            _g = g; _but = but; _co = co; _s = s; _ox = ox; _oy = oy;
        }

        private PointF P(float x, float y) => new(_ox + x * _s, _oy + y * _s);
        private RectangleF R(float x, float y, float w, float h) => new(_ox + x * _s, _oy + y * _s, w * _s, h * _s);

        public void Line(float x1, float y1, float x2, float y2) => _g.DrawLine(_but, P(x1, y1), P(x2, y2));

        /// <summary>Vẽ nhiều đoạn thẳng (polyline) hoặc đa giác nếu <paramref name="dong"/> = true.</summary>
        public void Poly(bool to, params float[] d)
        {
            if (d.Length < 4) return;
            var diem = new PointF[d.Length / 2];
            for (int i = 0; i < diem.Length; i++) diem[i] = P(d[i * 2], d[i * 2 + 1]);
            if (to) _g.FillPolygon(_co, diem);
            else
            {
                if (diem.Length == 2) _g.DrawLine(_but, diem[0], diem[1]);
                else _g.DrawPolygon(_but, diem);
            }
        }

        /// <summary>Đường thẳng đứt quãng (nét đứt) - dùng cho vé/voucher.</summary>
        public void Dash(float x1, float y1, float x2, float y2, int doan)
        {
            for (int i = 0; i < doan; i++)
            {
                if (i % 2 != 0) continue;
                float t1 = (float)i / doan, t2 = (float)(i + 0.72f) / doan;
                _g.DrawLine(_but, P(x1 + (x2 - x1) * t1, y1 + (y2 - y1) * t1),
                                  P(x1 + (x2 - x1) * t2, y1 + (y2 - y1) * t2));
            }
        }

        public void RectF(float x, float y, float w, float h) => _g.FillRectangle(_co, R(x, y, w, h));

        public void Round(bool to, float x, float y, float w, float h, float r)
        {
            if (w <= 0 || h <= 0) return;
            r = Math.Min(r, Math.Min(w, h) / 2f);
            using var duong = new GraphicsPath();
            float dg = r * 2f * _s;
            RectangleF v = R(x, y, w, h);
            duong.AddArc(v.X, v.Y, dg, dg, 180, 90);
            duong.AddArc(v.Right - dg, v.Y, dg, dg, 270, 90);
            duong.AddArc(v.Right - dg, v.Bottom - dg, dg, dg, 0, 90);
            duong.AddArc(v.X, v.Bottom - dg, dg, dg, 90, 90);
            duong.CloseFigure();
            if (to) _g.FillPath(_co, duong);
            else _g.DrawPath(_but, duong);
        }

        public void Circle(bool to, float cx, float cy, float r)
        {
            RectangleF v = R(cx - r, cy - r, r * 2f, r * 2f);
            if (to) _g.FillEllipse(_co, v);
            else _g.DrawEllipse(_but, v);
        }

        public void Dot(float cx, float cy, float r) => Circle(true, cx, cy, r);

        public void Ellipse(bool to, float x, float y, float w, float h)
        {
            RectangleF v = R(x, y, w, h);
            if (to) _g.FillEllipse(_co, v);
            else _g.DrawEllipse(_but, v);
        }

        public void Arc(float x, float y, float w, float h, float start, float sweep) =>
            _g.DrawArc(_but, R(x, y, w, h), start, sweep);

        public void Star(float cx, float cy, float rNgoai, float rTrong, int canh)
        {
            var diem = new PointF[canh * 2];
            for (int i = 0; i < canh * 2; i++)
            {
                double goc = -Math.PI / 2 + i * Math.PI / canh;
                float r = i % 2 == 0 ? rNgoai : rTrong;
                diem[i] = P(cx + r * (float)Math.Cos(goc), cy + r * (float)Math.Sin(goc));
            }
            _g.DrawPolygon(_but, diem);
        }
    }
}
