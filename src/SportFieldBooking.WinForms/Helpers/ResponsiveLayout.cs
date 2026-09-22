#nullable enable
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace SportFieldBooking.WinForms.Helpers;

/// <summary>
/// Bộ dàn trang dùng chung cho toàn bộ WinForms — phiên bản hoàn thiện V6.
/// Mục tiêu: không chồng control, không cắt chữ, hỗ trợ DPI và cửa sổ nhỏ,
/// đồng thời giữ phần xử lý nghiệp vụ ở các Form logic riêng.
/// </summary>
public static class ResponsiveLayout
{
    private const int HeaderHeight = 66;
    private const int SidebarWide = 240;
    private const int SidebarMedium = 220;
    private const int SidebarNarrow = 200;

    private static readonly HashSet<string> MainForms = new(StringComparer.Ordinal)
    {
        "frmAdminMain", "frmNhanVienMain", "frmKhachHangMain"
    };

    private static readonly HashSet<string> ManagementForms = new(StringComparer.Ordinal)
    {
        "frmQuanLyKhachHang", "frmQuanLyNhanVien", "frmQuanLyTaiKhoan",
        "frmSan", "frmLoaiSan", "frmVoucher", "frmKhuyenMai", "frmNhatKyHoatDong"
    };

    private static readonly HashSet<string> BookingForms = new(StringComparer.Ordinal)
    {
        "frmDatSanAdmin", "frmDatSanNhanVien", "frmDatSanKhachHang"
    };

    public static void ApDung(Form form)
    {
        if (form == null || form.IsDisposed) return;

        form.AutoScaleMode = AutoScaleMode.Font;
        form.AutoScroll = false;
        form.BackColor = GiaoDien.ManHinhNen;
        form.ForeColor = GiaoDien.Chu;
        form.Font = GiaoDien.ChuThuong;

        // Fix chi tiết panel trước khi chạy engine chung
        FixPanelChiTietToanForm(form);

        // Chạy engine dàn trang chung
        TuDongDanTrang(form);

        if (MainForms.Contains(form.Name))
        {
            ApDungMain(form);
            return;
        }

        if (form.Name == "frmDangNhap" && Tim(form, "pnlTrai") is Panel loginLeft && Tim(form, "pnlPhai") is Panel loginRight)
        {
            form.Resize -= LoginResize;
            form.Resize += LoginResize;
            LoginResize(form, EventArgs.Empty);
            return;
        }

        if (ConTrucTiep(form, "pnlNoiDung") is Panel content)
        {
            content.AutoScroll = true;
            content.HorizontalScroll.Enabled = true;
            content.VerticalScroll.Enabled = true;
            TrangTriNen(content);

            form.Resize -= ContentResize;
            form.Resize += ContentResize;
            ContentResize(form, EventArgs.Empty);
        }

        if (ConTrucTiep(form, "pnlTrai") is Panel trai && ConTrucTiep(form, "pnlPhai") is Panel phai &&
            BookingForms.Contains(form.Name))
        {
            form.Resize -= BookingResize;
            form.Resize += BookingResize;
            BookingResize(form, EventArgs.Empty);
        }

        if (ManagementForms.Contains(form.Name) && ConTrucTiep(form, "pnlTrai") is Panel list && ConTrucTiep(form, "pnlPhai") is Panel detail)
        {
            form.Resize -= ManagementResize;
            form.Resize += ManagementResize;
            ManagementResize(form, EventArgs.Empty);
        }

        if (form.Name.StartsWith("frmDashboard", StringComparison.Ordinal) ||
            form.Name.StartsWith("frmThongKe", StringComparison.Ordinal) ||
            form.Name == "frmThongTinCaNhan" ||
            form.Name == "frmTrangChuKhachHang")
        {
            form.Resize -= DashboardResize;
            form.Resize += DashboardResize;
            DashboardResize(form, EventArgs.Empty);
        }

        if (form.Name == "frmTrangChuKhachHang")
        {
            form.Resize -= CustomerHomeResize;
            form.Resize += CustomerHomeResize;
            CustomerHomeResize(form, EventArgs.Empty);
        }

        form.Resize -= FormDoiKichThuoc;
        form.Resize += FormDoiKichThuoc;

        if (ConTrucTiep(form, "pnlBoLoc") is Panel filter)
        {
            filter.AutoScroll = true;
            filter.HorizontalScroll.Enabled = true;
            GanXuLyResize(filter);
        }
        if (ConTrucTiep(form, "pnlChan") is Panel footer)
        {
            footer.Resize -= FooterResize;
            footer.Resize += FooterResize;
            FooterResize(footer, EventArgs.Empty);
        }
        if (ConTrucTiep(form, "pnlThanhCongCu") is Panel thanhCongCu)
        {
            thanhCongCu.AutoScroll = true;
            GanXuLyResize(thanhCongCu);
        }

        foreach (DataGridView grid in TimTatCa<DataGridView>(form))
        {
            grid.ScrollBars = ScrollBars.Both;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.RowHeadersVisible = false;
            grid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            // Đảm bảo không có cột nào quá nhỏ
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }

    // ==================================================================
    // FIX PANEL CHI TIẾT — Sửa lỗi chồng chéo pnlNut Dock Bottom đè lên fields
    // ==================================================================
    private static void FixPanelChiTietToanForm(Control root)
    {
        if (root == null) return;
        foreach (Control c in root.Controls)
        {
            if (c is Panel pnl && (pnl.Name == "pnlPhai" || pnl.Name == "pnlTrai"))
            {
                FixPanelChiTiet(pnl);
            }
            FixPanelChiTietToanForm(c);
        }
    }

    private static void FixPanelChiTiet(Panel pnl)
    {
        if (pnl == null || pnl.IsDisposed) return;
        if (pnl.Controls.Count == 0) return;

        // Tìm pnlNut / pnlChan / pnlHanhDong là panel chứa nút
        var nutPanels = pnl.Controls.OfType<Panel>()
            .Where(p => p.Name == "pnlNut" || p.Name == "pnlChan" || p.Name == "pnlHanhDong" || p.Name == "pnlChanPhai" || p.Name == "pnlNutTrai")
            .ToList();

        if (nutPanels.Count == 0) return;

        foreach (var nut in nutPanels)
        {
            // Nếu Dock Bottom và có control khác chồng lên nó -> chuyển sang Dock None và đặt ở cuối
            if (nut.Dock == DockStyle.Bottom)
            {
                // Kiểm tra có control nào bị đè không
                bool biDe = false;
                foreach (Control other in pnl.Controls)
                {
                    if (other == nut || other.Dock != DockStyle.None || !other.Visible) continue;
                    if (other.Top + other.Height > nut.Top && other.Top < nut.Top + nut.Height)
                    {
                        biDe = true;
                        break;
                    }
                }

                if (biDe)
                {
                    // Chuyển sang không Dock để có thể cuộn
                    nut.Dock = DockStyle.None;
                    // Lưu lại để lần sau không bị trôi
                    if (nut.Tag == null)
                        nut.Tag = NhanKichThuocGoc + nut.Width + "|" + nut.Height;
                }
            }

            // Đảm bảo pnlNut nằm sau cùng, không đè
            if (nut.Dock == DockStyle.None)
            {
                int maxBottom = pnl.Padding.Top;
                foreach (Control other in pnl.Controls)
                {
                    if (other == nut || !other.Visible) continue;
                    if (other.Dock != DockStyle.None) continue;
                    // Chỉ tính các control nhập liệu, không tính nút lẻ
                    if (other is Button) continue;
                    maxBottom = Math.Max(maxBottom, other.Top + KichThuocThat(other).Height);
                }
                // Đặt nút cách maxBottom 12px
                int targetTop = maxBottom + 16;
                if (nut.Top < targetTop || nut.Top > targetTop + 100)
                {
                    nut.Top = targetTop;
                }
                nut.Left = pnl.Padding.Left;
                nut.Width = Math.Max(200, pnl.ClientSize.Width - pnl.Padding.Horizontal);
                // Nới chiều cao panel cha nếu cần
                int canCao = nut.Top + nut.Height + pnl.Padding.Bottom;
                if (canCao > pnl.Height)
                {
                    // Không set Height trực tiếp nếu đang trong layout, chỉ bật AutoScroll
                    pnl.AutoScroll = true;
                }
            }
        }

        // Đảm bảo tất cả TextBox/ComboBox trong pnlPhai có cùng width
        var inputControls = pnl.Controls.OfType<Control>()
            .Where(c => c is TextBox || c is ComboBox || c is DateTimePicker)
            .ToList();
        if (inputControls.Count >= 3)
        {
            int targetWidth = Math.Max(200, pnl.ClientSize.Width - pnl.Padding.Horizontal - 10);
            foreach (var inp in inputControls)
            {
                if (inp.Width < targetWidth - 40 || inp.Width > targetWidth + 10)
                {
                    inp.Width = Math.Min(targetWidth, Math.Max(200, targetWidth));
                }
            }
        }

        pnl.AutoScroll = true;
    }

    private static void ApDungMain(Form form)
    {
        var sidebar = ConTrucTiep(form, "pnlSidebar") as Panel;
        var header = ConTrucTiep(form, "pnlTren") as Panel;
        var content = ConTrucTiep(form, "pnlNoiDung") as Panel;
        if (sidebar == null || header == null || content == null) return;

        sidebar.AutoScroll = false;
        content.AutoScroll = true;
        TrangTriNen(content);

        form.Resize -= MainResize;
        form.Resize += MainResize;
        MainResize(form, EventArgs.Empty);
    }

    private static void MainResize(object? sender, EventArgs e)
    {
        if (sender is not Form form) return;

        var sidebar = ConTrucTiep(form, "pnlSidebar") as Panel;
        var header = ConTrucTiep(form, "pnlTren") as Panel;
        var menu = ConTrucTiep(form, "pnlMenu") as Panel;
        var logo = ConTrucTiep(form, "pnlLogo") as Panel;
        var footer = ConTrucTiep(form, "pnlChan") as Panel;
        var content = ConTrucTiep(form, "pnlNoiDung") as Panel;
        if (sidebar == null || header == null) return;

        var theme = Tim(header, "pnlChuDe") as Panel;
        var title = Tim(header, "lblTieuDeTrang") as Label;
        var icon = Tim(header, "picNguoiDung") as Control;
        var user = Tim(header, "lblTenNguoiDung") as Label;
        var role = Tim(header, "lblVaiTro") as Label;

        int totalWidth = Math.Max(760, form.ClientSize.Width);
        int sideWidth = totalWidth >= 1280 ? SidebarWide : totalWidth >= 1040 ? SidebarMedium : SidebarNarrow;

        sidebar.Width = sideWidth;
        if (logo != null) logo.Width = sideWidth;
        if (menu != null) menu.Width = sideWidth;
        if (footer != null) footer.Width = sideWidth;
        if (content != null) content.AutoScroll = true;

        if (menu != null)
        {
            foreach (Control c in menu.Controls)
                c.Width = Math.Max(150, menu.ClientSize.Width - menu.Padding.Horizontal);
        }
        if (footer != null)
        {
            foreach (Control c in footer.Controls)
                c.Width = Math.Max(150, footer.ClientSize.Width - footer.Padding.Horizontal);
        }

        var logoTitle = ConTrucTiep(form, "lblTenTrungTam") as Label;
        var logoSub = ConTrucTiep(form, "lblTenPhanMem") as Label;
        if (logoTitle != null)
        {
            logoTitle.AutoSize = false;
            logoTitle.AutoEllipsis = true;
            logoTitle.Left = 70;
            logoTitle.Width = Math.Max(100, sideWidth - 82);
            logoTitle.Text = sideWidth <= 205 ? "THUÊ SÂN" : "THUÊ SÂN THỂ THAO";
            logoTitle.Font = new Font(GiaoDien.TenFont, sideWidth <= 205 ? 10.5F : 11.5F, FontStyle.Bold);
        }
        if (logoSub != null)
        {
            logoSub.AutoSize = false;
            logoSub.Left = 70;
            logoSub.Width = Math.Max(100, sideWidth - 82);
            logoSub.Visible = sideWidth > 205;
        }

        header.SuspendLayout();
        try
        {
            int hw = header.ClientSize.Width;
            int themeWidth = Math.Min(160, Math.Max(110, hw / 7));
            if (theme != null)
            {
                theme.Dock = DockStyle.Right;
                theme.Width = themeWidth;
                theme.Padding = new Padding(0, 10, 12, 10);
                var themeButton = theme.Controls.Count > 0 ? theme.Controls[0] : null;
                if (themeButton != null) themeButton.Dock = DockStyle.Fill;
            }

            int bienPhai = hw - themeWidth;
            int margin = 14;

            int stackW = Math.Min(220, Math.Max(120, hw / 6));
            int stackRight = bienPhai - margin;
            int stackLeft = Math.Max(220, stackRight - stackW);

            bool compact = stackLeft - (title?.Left ?? 24) < 260;

            if (user != null)
            {
                user.AutoSize = false;
                user.AutoEllipsis = true;
                user.Anchor = AnchorStyles.None;
                user.Left = stackLeft;
                user.Width = stackRight - stackLeft;
                user.TextAlign = ContentAlignment.MiddleRight;
                user.Top = compact ? (HeaderHeight - user.Height) / 2 : 12;
            }
            if (role != null)
            {
                role.AutoSize = false;
                role.AutoEllipsis = true;
                role.Anchor = AnchorStyles.None;
                role.Left = stackLeft;
                role.Width = stackRight - stackLeft;
                role.TextAlign = ContentAlignment.MiddleRight;
                role.Height = 16;
                role.Top = (user?.Bottom ?? HeaderHeight / 2) + 2;
                role.Visible = !compact;
            }
            if (icon != null)
            {
                icon.Anchor = AnchorStyles.None;
                icon.Left = Math.Max((title?.Left ?? 24) + 150, stackLeft - icon.Width - 10);
                icon.Top = Math.Max(6, (HeaderHeight - icon.Height) / 2);
            }
            if (title != null)
            {
                title.AutoSize = false;
                title.AutoEllipsis = true;
                title.Anchor = AnchorStyles.None;
                title.Left = title.Left > 0 ? title.Left : 24;
                title.Top = 0;
                title.Height = HeaderHeight;
                title.Width = Math.Max(140, (icon?.Left ?? stackLeft) - title.Left - 16);
                title.TextAlign = ContentAlignment.MiddleLeft;
            }
        }
        finally
        {
            header.ResumeLayout(true);
        }
    }

    private static void LoginResize(object? sender, EventArgs e)
    {
        if (sender is not Form form) return;
        var left = ConTrucTiep(form, "pnlTrai") as Panel;
        var right = ConTrucTiep(form, "pnlPhai") as Panel;
        var box = Tim(form, "pnlKhungNhap") as Panel;
        if (left == null || right == null || box == null) return;

        int w = Math.Max(720, form.ClientSize.Width);
        int h = Math.Max(500, form.ClientSize.Height);
        bool compact = w < 900;

        left.Dock = DockStyle.Left;
        left.Width = compact ? Math.Max(230, (int)(w * 0.34)) : Math.Min(420, Math.Max(300, (int)(w * 0.42)));
        right.Dock = DockStyle.Fill;
        GiaoDien.TrangTriDoc(left);

        box.Width = Math.Min(420, Math.Max(320, right.ClientSize.Width - 40));
        box.Height = Math.Min(400, Math.Max(340, right.ClientSize.Height - 50));
        box.Left = Math.Max(20, (right.ClientSize.Width - box.Width) / 2);
        box.Top = Math.Max(20, (right.ClientSize.Height - box.Height) / 2);

        var logo = Tim(form, "picLogo") as Control;
        var appTitle = Tim(form, "lblTenUngDung") as Label;
        var desc = Tim(form, "lblMoTa") as Label;
        if (logo != null)
        {
            logo.Left = Math.Max(20, (left.ClientSize.Width - logo.Width) / 2);
            logo.Top = Math.Max(70, h / 4 - 10);
        }
        if (appTitle != null)
        {
            appTitle.AutoSize = false;
            appTitle.TextAlign = ContentAlignment.MiddleLeft;
            appTitle.Left = 20;
            appTitle.Width = Math.Max(190, left.ClientSize.Width - 40);
            appTitle.Top = (logo?.Bottom ?? 190) + 18;
            appTitle.Height = Math.Max(40, TextRenderer.MeasureText(
                appTitle.Text, appTitle.Font,
                new Size(appTitle.Width, int.MaxValue), TextFormatFlags.WordBreak).Height + 4);
        }
        if (desc != null)
        {
            desc.AutoSize = false;
            desc.Left = 24;
            desc.Width = Math.Max(180, left.ClientSize.Width - 48);
            desc.Top = (appTitle?.Bottom ?? 270) + 8;
            desc.Height = Math.Max(40, TextRenderer.MeasureText(
                desc.Text, desc.Font,
                new Size(desc.Width, int.MaxValue), TextFormatFlags.WordBreak).Height + 4);
        }

        int inner = Math.Max(30, box.ClientSize.Width - 80);
        foreach (string name in new[] { "txtTenDangNhap", "txtMatKhau", "btnDangNhap", "btnDangKy" })
        {
            if (Tim(box, name) is Control c)
            {
                c.Left = 40;
                c.Width = inner;
            }
        }
    }

    private static void ContentResize(object? sender, EventArgs e)
    {
        if (sender is not Form form) return;
        if (ConTrucTiep(form, "pnlNoiDung") is not Panel content) return;

        if (ConTrucTiep(form, "pnlDau") is Panel head)
        {
            head.Dock = DockStyle.Top;
            int canCao = head.Height;
            foreach (Control c in head.Controls)
                if (c.Dock == DockStyle.None && c.Visible)
                    canCao = Math.Max(canCao, c.Top + KichThuocThat(c).Height + head.Padding.Bottom);
            head.Height = Math.Max(72, Math.Min(canCao, 132));

            var title = ConTrucTiep(head, "lblTieuDe") as Label;
            var desc = ConTrucTiep(head, "lblMoTaTrang") as Label;
            int rong = Math.Max(180, head.ClientSize.Width - 100);
            if (title != null)
            {
                title.AutoSize = false;
                title.Left = 76;
                title.Top = 10;
                title.Height = 34;
                title.Width = rong;
                title.AutoEllipsis = true;
            }
            if (desc != null)
            {
                desc.AutoSize = false;
                desc.Left = 78;
                desc.Top = 45;
                desc.Height = 20;
                desc.Width = rong;
                desc.AutoEllipsis = true;
            }

            // Nút làm mới trong pnlDau nếu có
            foreach (Button btn in head.Controls.OfType<Button>())
            {
                if (btn.Anchor.HasFlag(AnchorStyles.Right) || btn.Name.Contains("LamMoi"))
                {
                    btn.Left = Math.Max(rong, head.ClientSize.Width - btn.Width - 16);
                    btn.Top = 20;
                }
            }
        }

        content.AutoScroll = true;
        content.HorizontalScroll.Enabled = true;
        content.VerticalScroll.Enabled = true;
    }

    private static void ManagementResize(object? sender, EventArgs e)
    {
        if (sender is not Form form) return;
        var content = ConTrucTiep(form, "pnlNoiDung") as Panel;
        var list = ConTrucTiep(form, "pnlTrai") as Panel;
        var detail = ConTrucTiep(form, "pnlPhai") as Panel;
        if (content == null || list == null || detail == null) return;

        int w = content.ClientSize.Width - content.Padding.Horizontal;
        bool wide = w >= 1040;
        content.AutoScroll = true;
        content.SuspendLayout();
        try
        {
            if (wide)
            {
                detail.Dock = DockStyle.Right;
                detail.Width = Math.Min(400, Math.Max(340, w / 3));
                detail.Height = content.ClientSize.Height - content.Padding.Vertical;
                list.Dock = DockStyle.Fill;
                list.Width = Math.Max(500, w - detail.Width);
                list.Height = Math.Max(list.Height, 420);

                // Fix chi tiết
                FixPanelChiTiet(detail);
            }
            else
            {
                detail.Dock = DockStyle.Top;
                detail.Width = Math.Max(320, w);
                detail.Height = Math.Max(500, detail.Height);
                list.Dock = DockStyle.Top;
                list.Width = Math.Max(320, w);
                list.Height = 430;
                list.BringToFront();
                detail.BringToFront();
                list.SendToBack();

                if (Tim(list, "pnlThanhCongCu") is Panel toolbar)
                {
                    toolbar.AutoScroll = true;
                    toolbar.HorizontalScroll.Enabled = true;
                }

                FixPanelChiTiet(detail);
            }
        }
        finally { content.ResumeLayout(true); }
    }

    private static void BookingResize(object? sender, EventArgs e)
    {
        if (sender is not Form form) return;
        var content = ConTrucTiep(form, "pnlNoiDung") as Panel;
        var left = ConTrucTiep(form, "pnlTrai") as Panel;
        var right = ConTrucTiep(form, "pnlPhai") as Panel;
        if (content == null || left == null || right == null) return;

        int w = Math.Max(320, content.ClientSize.Width - content.Padding.Horizontal);
        bool wide = w >= 1180;
        content.AutoScroll = true;
        content.SuspendLayout();
        try
        {
            if (wide)
            {
                left.Dock = DockStyle.Left;
                left.Width = 440;
                right.Dock = DockStyle.Fill;
                right.Width = Math.Max(520, w - 440);
                left.Height = Math.Max(560, content.ClientSize.Height - content.Padding.Vertical);
                right.Height = left.Height;

                FixPanelChiTiet(left);
            }
            else
            {
                left.Dock = DockStyle.Top;
                left.Width = w;
                left.Height = 650;
                right.Dock = DockStyle.Top;
                right.Width = w;
                right.Height = 600;
                left.BringToFront();

                FixPanelChiTiet(left);
            }
        }
        finally { content.ResumeLayout(true); }
    }

    private static void DashboardResize(object? sender, EventArgs e)
    {
        if (sender is not Form form) return;
        var content = ConTrucTiep(form, "pnlNoiDung") as Panel;
        var kpi = ConTrucTiep(form, "pnlKpi") as Panel;
        var chart = ConTrucTiep(form, "pnlBieuDo") as Panel;
        var bottom = ConTrucTiep(form, "pnlDuoi") as Panel;
        if (content == null) return;

        content.AutoScroll = true;
        int w = Math.Max(320, content.ClientSize.Width - content.Padding.Horizontal);
        bool wide = w >= 1050;

        if (kpi != null)
        {
            kpi.Dock = DockStyle.Top;
            kpi.Width = w;
            kpi.Height = wide ? Math.Max(120, kpi.Height) : Math.Max(230, kpi.Height);
        }
        if (chart != null)
        {
            chart.Dock = DockStyle.Top;
            chart.Width = w;
            chart.Height = wide ? Math.Max(260, chart.Height) : Math.Max(360, chart.Height);
        }
        if (bottom != null)
        {
            bottom.Dock = DockStyle.Top;
            bottom.Width = w;
            bottom.Height = wide ? Math.Max(240, bottom.Height) : Math.Max(300, bottom.Height);
            DanCot(bottom);
        }

        // Tắt cuộn ngang để nút không văng
        content.HorizontalScroll.Enabled = false;
        content.AutoScrollMinSize = new Size(0, content.AutoScrollMinSize.Height);
    }

    private static void CustomerHomeResize(object? sender, EventArgs e)
    {
        if (sender is not Form form) return;
        var content = ConTrucTiep(form, "pnlNoiDung") as Panel;
        var kpi = ConTrucTiep(form, "pnlKpi") as Panel;
        var bottom = ConTrucTiep(form, "pnlDuoi") as Panel;
        var upcoming = ConTrucTiep(form, "pnlSapToi") as Panel;
        var vouchers = ConTrucTiep(form, "pnlVoucher") as Panel;
        if (content == null || kpi == null || bottom == null || upcoming == null || vouchers == null) return;

        var cards = new[]
        {
            ConTrucTiep(kpi, "kpiSoLanDat") as Control,
            ConTrucTiep(kpi, "kpiChiTieu") as Control,
            ConTrucTiep(kpi, "kpiVoucher") as Control,
            ConTrucTiep(kpi, "kpiSanYeuThich") as Control
        };
        if (Array.Exists(cards, c => c == null)) return;

        int w = Math.Max(320, content.ClientSize.Width - content.Padding.Horizontal);
        int gap = 14;
        bool wide = w >= 1120;

        content.SuspendLayout();
        try
        {
            kpi.Dock = DockStyle.Top;
            kpi.Width = w;
            if (wide)
            {
                kpi.Height = 122;
                int cw = Math.Max(170, (w - gap * 3) / 4);
                for (int i = 0; i < cards.Length; i++)
                {
                    cards[i]!.Size = new Size(cw, 110);
                    cards[i]!.Location = new Point(i * (cw + gap), 0);
                }
            }
            else
            {
                kpi.Height = 236;
                int cw = Math.Max(150, (w - gap) / 2);
                for (int i = 0; i < cards.Length; i++)
                {
                    int col = i % 2;
                    int row = i / 2;
                    cards[i]!.Size = new Size(cw, 104);
                    cards[i]!.Location = new Point(col * (cw + gap), row * 118);
                }
            }

            bottom.Dock = DockStyle.Top;
            bottom.Width = w;
            if (wide)
            {
                bottom.Height = 400;
                upcoming.Dock = DockStyle.Left;
                upcoming.Width = Math.Max(520, (w * 2) / 3);
                vouchers.Dock = DockStyle.Fill;
            }
            else
            {
                bottom.Height = 760;
                upcoming.Dock = DockStyle.Top;
                upcoming.Width = w;
                upcoming.Height = 360;
                vouchers.Dock = DockStyle.Top;
                vouchers.Width = w;
                vouchers.Height = 360;
            }
        }
        finally { content.ResumeLayout(true); }
    }

    private static void FooterResize(object? sender, EventArgs e)
    {
        if (sender is not Panel footer) return;
        int w = footer.ClientSize.Width;
        var buttons = footer.Controls.OfType<Button>().ToList();
        var label = footer.Controls.OfType<Label>().FirstOrDefault();
        if (buttons.Count == 0) return;

        const int gap = 8;
        int total = buttons.Sum(b => b.Width) + gap * Math.Max(0, buttons.Count - 1);
        bool compact = total + (label?.Width ?? 0) + 50 > w;

        if (!compact)
        {
            if (label != null) label.Left = footer.Padding.Left;
            int right = w - footer.Padding.Right;
            foreach (Button b in buttons.AsEnumerable().Reverse())
            {
                b.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                right -= b.Width;
                b.Left = right;
                b.Top = 7;
                right -= gap;
            }
        }
        else
        {
            footer.Height = 88;
            if (label != null)
            {
                label.Left = footer.Padding.Left;
                label.Top = 8;
                label.Width = Math.Max(180, w - footer.Padding.Horizontal);
            }
            int x = footer.Padding.Left;
            int y = 42;
            foreach (Button b in buttons)
            {
                b.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                b.Left = x;
                b.Top = y;
                x += b.Width + gap;
                if (x + b.Width > w - footer.Padding.Right)
                {
                    x = footer.Padding.Left;
                    y += b.Height + 5;
                }
            }
            footer.AutoScroll = true;
        }
    }

    // ==================================================================
    // ENGINE TỰ ĐỘNG DÀN TRANG
    // ==================================================================
    private const int KhoangCachToiThieu = 8;
    private static readonly HashSet<string> TenHangCongCu = new(StringComparer.Ordinal)
    {
        "pnlBoLoc", "pnlChan", "pnlThanhCongCu", "pnlTaoHoaDon", "pnlNut", "pnlHanhDong", "pnlChanPhai", "pnlNutTrai"
    };

    private static Size KichThuocThat(Control c)
    {
        int w = c.Width, h = c.Height;
        if (c is Button)
        {
            if (c.MinimumSize.Width > w) w = c.MinimumSize.Width;
            if (c.MinimumSize.Height > h) h = c.MinimumSize.Height;
        }
        return new Size(Math.Max(0, w), Math.Max(0, h));
    }

    private static bool LaNeoPhai(Control c) => (c.Anchor & AnchorStyles.Right) == AnchorStyles.Right;

    private static Control? ConTrucTiep(Control cha, string ten) =>
        cha?.Controls.Find(ten, false).FirstOrDefault();

    private static void XepLaiHang(Panel pnl)
    {
        if (pnl == null || pnl.IsDisposed) return;
        if (!_dangXep.Add(pnl)) return;
        try
        {
            bool dockNgangTuDo = pnl.Dock is DockStyle.None or DockStyle.Top or DockStyle.Bottom;
            KhoiPhucKichThuoc(pnl);
            int chieuCaoGoc = pnl.Height;

            var items = new List<Control>();
            foreach (Control c in pnl.Controls)
            {
                if (c.Dock != DockStyle.None || !c.Visible) continue;
                items.Add(c);
            }
            items.Reverse();
            if (items.Count == 0) return;

            foreach (Control c in items)
                if (c.Tag is not string t || !t.StartsWith(NhanToaDoGoc, StringComparison.Ordinal))
                    c.Tag = NhanToaDoGoc + c.Left + "|" + c.Top;

            var pads = pnl.Padding;
            int mePhai = pnl.ClientSize.Width - pads.Right;

            var hang = new List<List<Control>>();
            var goc = new List<int>();
            foreach (Control c in items.OrderBy(o => DocToaDoGoc(o).Y).ThenBy(o => DocToaDoGoc(o).X))
            {
                Size sc = KichThuocThat(c);
                Point g = DocToaDoGoc(c);
                int tam = g.Y + sc.Height / 2;
                int i = 0;
                for (; i < goc.Count; i++)
                    if (Math.Abs(goc[i] - tam) <= 16) break;
                if (i == goc.Count) { goc.Add(tam); hang.Add(new List<Control>()); }
                hang[i].Add(c);
            }
            for (int i = 0; i < hang.Count; i++)
                hang[i] = hang[i].OrderBy(o => DocToaDoGoc(o).X).ToList();

            int y = pads.Top;
            foreach (var nhom in hang)
            {
                var day = new List<List<Control>> { new() };
                foreach (Control c in nhom)
                {
                    Rectangle rc = HcnThat(c);
                    bool tach = day[0].Any(k => {
                        Rectangle rk = HcnThat(k);
                        return rk.IntersectsWith(rc) &&
                               Math.Min(rk.Bottom, rc.Bottom) - Math.Max(rk.Top, rc.Top) > 2 &&
                               Math.Min(rk.Right, rc.Right) - Math.Max(rk.Left, rc.Left) > 2;
                    });
                    if (tach) day.Add(new List<Control> { c });
                    else day[0].Add(c);
                }

                foreach (var d in day)
                {
                    if (d.Count == 0) continue;
                    int cao = d.Max(c => KichThuocThat(c).Height);

                    var phai = d.Where(LaNeoPhai).OrderByDescending(c => c.Left).ToList();
                    var trai = d.Where(c => !LaNeoPhai(c)).OrderBy(c => c.Left).ToList();

                    int xPhai = mePhai;
                    foreach (Control c in phai)
                    {
                        Size sc = KichThuocThat(c);
                        xPhai -= sc.Width;
                        c.Left = Math.Max(pads.Left, xPhai);
                        c.Top = y + (cao - sc.Height) / 2;
                        xPhai -= KhoangCachToiThieu;
                    }

                    int gioiHan = phai.Count > 0 ? phai[phai.Count - 1].Left - KhoangCachToiThieu : mePhai;
                    int xTrai = pads.Left;
                    foreach (Control c in trai)
                    {
                        Size sc = KichThuocThat(c);
                        int x = Math.Max(DocToaDoGoc(c).X, xTrai);
                        if (x + sc.Width > gioiHan) x = Math.Max(pads.Left, gioiHan - sc.Width);
                        c.Left = x;
                        c.Top = y + (cao - sc.Height) / 2;
                        xTrai = x + sc.Width + KhoangCachToiThieu;
                    }

                    if (xTrai - KhoangCachToiThieu > mePhai) pnl.AutoScroll = true;
                    y += cao + 6;
                }
            }

            int canCao = y + pads.Bottom;
            if (dockNgangTuDo && canCao > pnl.Height) pnl.Height = canCao;
            LuuKichThuocGoc(pnl, chieuCaoGoc);
        }
        finally { _dangXep.Remove(pnl); }
    }

    private static Rectangle HcnThat(Control c)
    {
        Size s = KichThuocThat(c);
        Point g = DocToaDoGoc(c);
        return new Rectangle(g.X, g.Y, s.Width, s.Height);
    }

    private static Point DocToaDoGoc(Control c)
    {
        if (c.Tag is string s && s.StartsWith(NhanToaDoGoc, StringComparison.Ordinal))
        {
            var p = s.Substring(NhanToaDoGoc.Length).Split('|');
            if (p.Length == 2 && int.TryParse(p[0], out int x) && int.TryParse(p[1], out int y))
                return new Point(x, y);
        }
        return c.Location;
    }

    private const string NhanToaDoGoc = "RESPONSIVE_ORIG|";
    private const string NhanKichThuocGoc = "RESPONSIVE_SIZE_V6|";

    private static void LuuKichThuocGoc(Control c, int chieuCaoGoc)
    {
        if (c.Tag != null) return;
        c.Tag = NhanKichThuocGoc + c.Width + "|" + chieuCaoGoc;
    }

    private static void KhoiPhucKichThuoc(Control c)
    {
        if (c.Tag is not string s || !s.StartsWith(NhanKichThuocGoc, StringComparison.Ordinal)) return;
        var p = s.Substring(NhanKichThuocGoc.Length).Split('|');
        if (p.Length < 2) return;
        if (int.TryParse(p[0], out int w) && int.TryParse(p[1], out int h))
        {
            if (c.Dock is DockStyle.None or DockStyle.Top or DockStyle.Bottom) c.Height = h;
            if (c.Dock is DockStyle.None or DockStyle.Left or DockStyle.Right) c.Width = w;
        }
    }

    private static void VuaKhopNoiDung(Panel pnl)
    {
        if (pnl == null || pnl.IsDisposed) return;

        int thap = 0, phaiNhat = 0;
        foreach (Control c in pnl.Controls)
        {
            if (!c.Visible || c.Dock != DockStyle.None) continue;
            Size s = KichThuocThat(c);
            thap = Math.Max(thap, c.Top + s.Height);
            phaiNhat = Math.Max(phaiNhat, c.Left + s.Width);
        }
        if (thap == 0) return;

        bool coConFill = pnl.Controls.Cast<Control>().Any(c => c.Dock == DockStyle.Fill);
        int canCao = thap + pnl.Padding.Bottom;

        int chiemCho = 0;
        foreach (Control c in pnl.Controls)
            if (c.Visible && c.Dock is DockStyle.Top or DockStyle.Bottom)
                chiemCho += c.Height;
        int khaDung = pnl.ClientSize.Height - chiemCho;

        if (pnl.Dock is DockStyle.None or DockStyle.Top or DockStyle.Bottom && canCao > pnl.Height)
            pnl.Height = canCao;

        if (!coConFill && pnl.Dock != DockStyle.Fill)
        {
            if (canCao > khaDung) pnl.AutoScroll = true;
            if (phaiNhat + pnl.Padding.Right > pnl.ClientSize.Width) pnl.AutoScroll = true;
        }
    }

    private static void TuDongDanTrang(Control root)
    {
        if (root == null || root.IsDisposed) return;

        foreach (Control c in root.Controls)
            if (c is Panel p && TenHangCongCu.Contains(p.Name))
            {
                GanXuLyResize(p);
                XepLaiHang(p);
            }

        if (root is Panel pnlThe) SapThe(pnlThe);

        foreach (Control c in root.Controls) TuDongDanTrang(c);

        foreach (Control c in root.Controls)
            if (c is Panel p && p.Dock != DockStyle.Fill)
                VuaKhopNoiDung(p);
    }

    private static void GanXuLyResize(Panel pnl)
    {
        pnl.Resize -= PanelResize;
        pnl.Resize += PanelResize;
    }

    private static void PanelResize(object? sender, EventArgs e)
    {
        if (sender is Panel p) XepLaiHang(p);
    }

    private static readonly HashSet<Panel> _dangXep = new();

    private static void FormDoiKichThuoc(object? sender, EventArgs e)
    {
        if (sender is not Form form || form.IsDisposed) return;
        if (form.Tag is string s && s.StartsWith("RESPONSIVE_W|", StringComparison.Ordinal)
            && int.TryParse(s.AsSpan(13), out int cu) && cu == form.ClientSize.Width) return;

        form.Tag = "RESPONSIVE_W|" + form.ClientSize.Width;
        TuDongDanTrang(form);
        FixPanelChiTietToanForm(form);
    }

    private static Control? Tim(Control root, string name)
    {
        if (root == null) return null;
        if (string.Equals(root.Name, name, StringComparison.Ordinal)) return root;
        foreach (Control child in root.Controls)
        {
            var found = Tim(child, name);
            if (found != null) return found;
        }
        return null;
    }

    private static IEnumerable<T> TimTatCa<T>(Control root) where T : Control
    {
        foreach (Control child in root.Controls)
        {
            if (child is T wanted) yield return wanted;
            foreach (T nested in TimTatCa<T>(child)) yield return nested;
        }
    }

    // ======================= NHÓM THẺ (KPI / BIỂU ĐỒ) =======================
    private static readonly HashSet<string> TenLoaiThe = new(StringComparer.Ordinal)
    {
        "KpiCard", "FormsPlot"
    };

    private static readonly ConditionalWeakTable<Control, int[]> KichThuocTheGoc = new();
    private static readonly HashSet<Panel> _dangSapThe = new();

    private static bool LaThe(Control c) => TenLoaiThe.Contains(c.GetType().Name);

    private static int[] LayTheGoc(Control c)
    {
        if (KichThuocTheGoc.TryGetValue(c, out int[]? luu) && luu.Length == 4) return luu;
        var moi = new[] { c.Left, c.Top, c.Width, c.Height };
        KichThuocTheGoc.AddOrUpdate(c, moi);
        return moi;
    }

    private static void SapThe(Panel pnl)
    {
        if (pnl == null || pnl.IsDisposed || !_dangSapThe.Add(pnl)) return;
        try
        {
            var the = new List<Control>();
            foreach (Control c in pnl.Controls)
                if (c.Visible && c.Dock == DockStyle.None && LaThe(c)) the.Add(c);
            if (the.Count < 2) return;

            foreach (Control c in the) LayTheGoc(c);
            the.Sort((a, b) => LayTheGoc(a)[0].CompareTo(LayTheGoc(b)[0]));

            int gap = 14;
            int w = pnl.ClientSize.Width - pnl.Padding.Horizontal;
            if (w < 120) return;

            bool coBieuDo = the.Exists(c => string.Equals(c.GetType().Name, "FormsPlot", StringComparison.Ordinal));
            int minRong = coBieuDo ? 280 : 180;

            int cot = the.Count;
            while (cot > 1 && (w - gap * (cot - 1)) / cot < minRong) cot--;

            pnl.SuspendLayout();
            try
            {
                int tongCao;
                if (cot >= the.Count)
                {
                    int tongRong = 0;
                    foreach (Control c in the) tongRong += Math.Max(1, LayTheGoc(c)[2]);

                    int khaDung = Math.Max(minRong, w - gap * (the.Count - 1));

                    var chieuRong = new int[the.Count];
                    bool dungTrongSo = true;
                    for (int i = 0; i < the.Count; i++)
                    {
                        chieuRong[i] = (int)Math.Round((double)khaDung * Math.Max(1, LayTheGoc(the[i])[2]) / tongRong);
                        if (chieuRong[i] < minRong) dungTrongSo = false;
                    }
                    if (!dungTrongSo)
                        for (int i = 0; i < the.Count; i++)
                            chieuRong[i] = khaDung / the.Count;

                    int x = pnl.Padding.Left, daDung = 0;
                    tongCao = 0;
                    for (int i = 0; i < the.Count; i++)
                    {
                        int[] g = LayTheGoc(the[i]);
                        int rong = i == the.Count - 1 ? khaDung - daDung : chieuRong[i];
                        int phongTo = pnl.ClientSize.Width - pnl.Padding.Right - x;
                        rong = Math.Min(Math.Max(60, rong), Math.Max(60, phongTo));
                        the[i].Bounds = new Rectangle(x, pnl.Padding.Top, rong, g[3]);
                        tongCao = Math.Max(tongCao, g[3]);
                        x += rong + gap;
                        daDung += rong;
                    }
                }
                else
                {
                    int rong = cot <= 1 ? w : Math.Max(minRong, (w - gap * (cot - 1)) / cot);
                    int y = pnl.Padding.Top;
                    for (int hang = 0; hang * cot < the.Count; hang++)
                    {
                        int caoHang = 0, x = pnl.Padding.Left;
                        for (int j = 0; j < cot && hang * cot + j < the.Count; j++)
                        {
                            Control c = the[hang * cot + j];
                            int[] g = LayTheGoc(c);
                            c.Bounds = new Rectangle(x, y, rong, g[3]);
                            caoHang = Math.Max(caoHang, g[3]);
                            x += rong + gap;
                        }
                        y += caoHang + gap;
                    }
                    tongCao = Math.Max(0, y - gap - pnl.Padding.Top);
                }

                int canCao = tongCao + pnl.Padding.Vertical;
                if (pnl.Dock != DockStyle.Fill && canCao > pnl.Height)
                    pnl.Height = canCao;
            }
            finally { pnl.ResumeLayout(true); }
        }
        finally { _dangSapThe.Remove(pnl); }
    }

    private static void DanCot(Panel pnl)
    {
        if (pnl == null || pnl.IsDisposed || TenHangCongCu.Contains(pnl.Name)) return;
        if (pnl.AutoScroll) return;

        var con = new List<Control>();
        bool coThe = false;
        foreach (Control c in pnl.Controls)
        {
            if (c.Dock != DockStyle.None || !c.Visible) continue;
            if (LaThe(c)) coThe = true;
            con.Add(c);
        }
        if (con.Count < 2 || coThe) return;

        int w = pnl.ClientSize.Width - pnl.Padding.Horizontal;
        if (w < 160) return;

        var cot = new List<int[]>();
        var nhom = new List<List<Control>>();
        foreach (Control c in con.OrderBy(c => c.Left))
        {
            int trai = c.Left, phai = c.Left + c.Width, vitri = -1;
            for (int i = 0; i < cot.Count; i++)
            {
                int rong = Math.Min(phai - trai, cot[i][1] - cot[i][0]);
                if (rong > 0 && Math.Min(phai, cot[i][1]) - Math.Max(trai, cot[i][0]) > rong / 2) { vitri = i; break; }
            }
            if (vitri < 0)
            {
                cot.Add(new[] { trai, phai });
                nhom.Add(new List<Control> { c });
            }
            else
            {
                cot[vitri][0] = Math.Min(cot[vitri][0], trai);
                cot[vitri][1] = Math.Max(cot[vitri][1], phai);
                nhom[vitri].Add(c);
            }
        }
        if (cot.Count < 2) return;

        var thuTu = Enumerable.Range(0, cot.Count).OrderBy(i => cot[i][0]).ToList();

        int phaiNhat = cot[thuTu[^1]][1];
        bool coVanDe = phaiNhat > w + pnl.Padding.Right || phaiNhat < w - 24;
        for (int i = 0; i < thuTu.Count - 1 && !coVanDe; i++)
            if (cot[thuTu[i + 1]][0] < cot[thuTu[i]][1]) coVanDe = true;
        if (!coVanDe) return;

        var khe = new int[thuTu.Count - 1];
        int tongKhe = 0;
        for (int i = 0; i < khe.Length; i++)
        {
            khe[i] = Math.Max(14, cot[thuTu[i + 1]][0] - cot[thuTu[i]][1]);
            tongKhe += khe[i];
        }

        int tongRongCot = 0;
        foreach (int i in thuTu) tongRongCot += Math.Max(1, cot[i][1] - cot[i][0]);
        int khaDung = Math.Max(cot.Count * 60, w - tongKhe);

        pnl.SuspendLayout();
        try
        {
            int x = pnl.Padding.Left, daDung = 0;
            for (int vi = 0; vi < thuTu.Count; vi++)
            {
                int i = thuTu[vi];
                int rongCu = Math.Max(1, cot[i][1] - cot[i][0]);
                int rongMoi = vi == thuTu.Count - 1 ? khaDung - daDung : (int)Math.Round((double)khaDung * rongCu / tongRongCot);
                rongMoi = Math.Min(Math.Max(60, rongMoi), Math.Max(60, w - x));

                foreach (Control c in nhom[i])
                {
                    c.Anchor = (c.Anchor & (AnchorStyles.Top | AnchorStyles.Bottom)) | AnchorStyles.Left;
                    int lech = c.Left - cot[i][0];
                    int rongCon = (int)Math.Round((double)c.Width * rongMoi / rongCu);
                    c.Left = x + (int)Math.Round((double)lech * rongMoi / rongCu);
                    c.Width = Math.Max(60, Math.Min(rongCon, pnl.ClientSize.Width - pnl.Padding.Right - c.Left));
                }

                x += rongMoi + (vi < khe.Length ? khe[vi] : 0);
                daDung += rongMoi;
            }
        }
        finally { pnl.ResumeLayout(true); }
    }

    private static void TrangTriNen(Control control)
    {
        if (control == null || control.Tag?.ToString() == "RESPONSIVE_BG_V5") return;
        control.Tag = "RESPONSIVE_BG_V5";
        control.Paint += (_, e) =>
        {
            if (control.ClientSize.Width <= 0 || control.ClientSize.Height <= 0) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(18, GiaoDien.Chinh), 1.1f);
            using var pen2 = new Pen(Color.FromArgb(10, GiaoDien.ChuPhu), 1f);
            int w = control.ClientSize.Width;
            int h = control.ClientSize.Height;
            int cx = w - 110;
            int cy = h - 90;
            e.Graphics.DrawEllipse(pen, cx - 100, cy - 100, 200, 200);
            e.Graphics.DrawEllipse(pen2, cx - 62, cy - 62, 124, 124);
            e.Graphics.DrawLine(pen, Math.Max(0, w - 430), Math.Max(20, h - 170), w - 35, h - 35);
            e.Graphics.DrawLine(pen2, Math.Max(0, w - 310), Math.Max(20, h - 35), w - 35, Math.Max(20, h - 310));
        };
    }
}
