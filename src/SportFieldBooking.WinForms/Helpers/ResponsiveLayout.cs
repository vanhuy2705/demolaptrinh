using System.Drawing.Drawing2D;

namespace SportFieldBooking.WinForms.Helpers;

/// <summary>
/// Bộ dàn trang dùng chung cho toàn bộ WinForms.
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
        "frmSan", "frmLoaiSan", "frmVoucher", "frmKhuyenMai"
    };

    public static void ApDung(Form form)
    {
        if (form == null || form.IsDisposed) return;

        form.AutoScaleMode = AutoScaleMode.Font;
        form.AutoScroll = false;
        form.BackColor = GiaoDien.ManHinhNen;
        form.ForeColor = GiaoDien.Chu;
        form.Font = GiaoDien.ChuThuong;

        // Chỉ một Form chính mới cần bố cục sidebar/header đặc biệt.
        if (MainForms.Contains(form.Name))
        {
            ApDungMain(form);
            return;
        }

        // Form đăng nhập là bố cục 2 khối đặc biệt.
        if (form.Name == "frmDangNhap" && Tim(form, "pnlTrai") is Panel loginLeft && Tim(form, "pnlPhai") is Panel loginRight)
        {
            form.Resize -= LoginResize;
            form.Resize += LoginResize;
            LoginResize(form, EventArgs.Empty);
            return;
        }

        // Các Form nội dung luôn có vùng cuộn. Điều này là lớp bảo vệ cuối cùng
        // cho máy có độ phân giải thấp hoặc Windows scale 125/150/175%.
        if (Tim(form, "pnlNoiDung") is Panel content)
        {
            content.AutoScroll = true;
            content.HorizontalScroll.Enabled = true;
            content.VerticalScroll.Enabled = true;
            TrangTriNen(content);

            form.Resize -= ContentResize;
            form.Resize += ContentResize;
            ContentResize(form, EventArgs.Empty);
        }

        // Form đặt sân: 2 cột khi đủ rộng, 1 cột khi hẹp.
        if (Tim(form, "pnlTrai") is Panel trai && Tim(form, "pnlPhai") is Panel phai &&
            form.Name.StartsWith("frmDatSan", StringComparison.Ordinal))
        {
            form.Resize -= BookingResize;
            form.Resize += BookingResize;
            BookingResize(form, EventArgs.Empty);
        }

        // Các màn hình quản lý có danh sách + chi tiết.
        if (ManagementForms.Contains(form.Name) && Tim(form, "pnlTrai") is Panel list && Tim(form, "pnlPhai") is Panel detail)
        {
            form.Resize -= ManagementResize;
            form.Resize += ManagementResize;
            ManagementResize(form, EventArgs.Empty);
        }

        // Dashboard/thống kê: xếp dọc khi không đủ chiều rộng.
        if (form.Name.StartsWith("frmDashboard", StringComparison.Ordinal) ||
            form.Name.StartsWith("frmThongKe", StringComparison.Ordinal))
        {
            form.Resize -= DashboardResize;
            form.Resize += DashboardResize;
            DashboardResize(form, EventArgs.Empty);
        }

        // Các trang danh sách có bộ lọc/footer: tự thu gọn và cho phép cuộn ngang.
        if (Tim(form, "pnlBoLoc") is Panel filter)
        {
            filter.AutoScroll = true;
            filter.HorizontalScroll.Enabled = true;
        }
        if (Tim(form, "pnlChan") is Panel footer)
        {
            footer.Resize -= FooterResize;
            footer.Resize += FooterResize;
            FooterResize(footer, EventArgs.Empty);
        }

        // Lưới luôn có thanh cuộn ngang khi nhiều cột.
        foreach (DataGridView grid in TimTatCa<DataGridView>(form))
        {
            grid.ScrollBars = ScrollBars.Both;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.RowHeadersVisible = false;
            grid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
        }
    }

    private static void ApDungMain(Form form)
    {
        var sidebar = Tim(form, "pnlSidebar") as Panel;
        var header = Tim(form, "pnlTren") as Panel;
        var content = Tim(form, "pnlNoiDung") as Panel;
        if (sidebar == null || header == null || content == null) return;

        sidebar.AutoScroll = false;
        content.AutoScroll = true;
        TrangTriNen(content);

        form.Resize -= MainResize;
        form.Resize += MainResize;
        MainResize(form, EventArgs.Empty);

        // Trang chủ khách hàng có KPI + 2 vùng nội dung đặc biệt.
        if (form.Name == "frmTrangChuKhachHang")
        {
            form.Resize -= CustomerHomeResize;
            form.Resize += CustomerHomeResize;
            CustomerHomeResize(form, EventArgs.Empty);
        }
    }

    private static void MainResize(object? sender, EventArgs e)
    {
        if (sender is not Form form) return;

        var sidebar = Tim(form, "pnlSidebar") as Panel;
        var header = Tim(form, "pnlTren") as Panel;
        var menu = Tim(form, "pnlMenu") as Panel;
        var logo = Tim(form, "pnlLogo") as Panel;
        var footer = Tim(form, "pnlChan") as Panel;
        var theme = Tim(form, "pnlChuDe") as Panel;
        var title = Tim(form, "lblTieuDeTrang") as Label;
        var icon = Tim(form, "picNguoiDung") as Control;
        var user = Tim(form, "lblTenNguoiDung") as Label;
        var role = Tim(form, "lblVaiTro") as Label;
        var content = Tim(form, "pnlNoiDung") as Panel;
        if (sidebar == null || header == null) return;

        int totalWidth = Math.Max(760, form.ClientSize.Width);
        int sideWidth = totalWidth >= 1280 ? SidebarWide : totalWidth >= 1040 ? SidebarMedium : SidebarNarrow;

        sidebar.Width = sideWidth;
        if (logo != null) logo.Width = sideWidth;
        if (menu != null) menu.Width = sideWidth;
        if (footer != null) footer.Width = sideWidth;
        if (content != null) content.AutoScroll = true;

        // Đồng bộ các nút sidebar với độ rộng thật, tránh chữ bị cắt do Designer cũ 236px.
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

        var logoTitle = Tim(form, "lblTenTrungTam") as Label;
        var logoSub = Tim(form, "lblTenPhanMem") as Label;
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

        // Header chia vùng rõ ràng: tiêu đề | tài khoản | nút chủ đề.
        // Không dùng Anchor cho vị trí tính toán để tránh Anchor + MaximumSize gây lệch.
        header.SuspendLayout();
        try
        {
            int hw = header.ClientSize.Width;
            int themeWidth = Math.Min(130, Math.Max(108, hw / 7));
            if (theme != null)
            {
                theme.Dock = DockStyle.Right;
                theme.Width = themeWidth;
                theme.Padding = new Padding(0, 10, 12, 10);
                var themeButton = theme.Controls.Count > 0 ? theme.Controls[0] : null;
                if (themeButton != null) themeButton.Dock = DockStyle.Fill;
            }

            int rightZone = themeWidth + 12;
            int userWidth = Math.Min(185, Math.Max(110, hw / 5));
            int userRight = rightZone + 8;
            int userLeft = hw - userRight - userWidth;
            int iconWidth = icon?.Width ?? 36;
            int iconLeft = userLeft - iconWidth - 12;
            int titleLeft = title?.Left ?? 24;
            int titleAvailable = iconLeft - titleLeft - 20;

            // Ở màn hình rất hẹp, ẩn chữ vai trò nhưng vẫn giữ tài khoản/icon.
            bool compact = titleAvailable < 190;
            if (user != null)
            {
                user.AutoSize = false;
                user.AutoEllipsis = true;
                user.Left = Math.Max(titleLeft + 190, userLeft);
                user.Width = Math.Max(90, hw - user.Left - userRight);
                user.TextAlign = ContentAlignment.MiddleRight;
            }
            if (role != null)
            {
                role.AutoSize = false;
                role.AutoEllipsis = true;
                role.Left = user?.Left ?? userLeft;
                role.Width = user?.Width ?? userWidth;
                role.TextAlign = ContentAlignment.MiddleRight;
                role.Visible = !compact;
            }
            if (icon != null)
            {
                icon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                icon.Left = Math.Max(titleLeft + 150, (user?.Left ?? userLeft) - iconWidth - 10);
                icon.Top = Math.Max(10, (HeaderHeight - icon.Height) / 2);
            }
            if (title != null)
            {
                title.AutoSize = false;
                title.AutoEllipsis = true;
                title.Left = titleLeft;
                title.Top = 0;
                title.Height = HeaderHeight;
                title.Width = Math.Max(160, (icon?.Left ?? hw - 180) - title.Left - 18);
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
        var left = Tim(form, "pnlTrai") as Panel;
        var right = Tim(form, "pnlPhai") as Panel;
        var box = Tim(form, "pnlKhungNhap") as Panel;
        if (left == null || right == null || box == null) return;

        int w = Math.Max(720, form.ClientSize.Width);
        int h = Math.Max(500, form.ClientSize.Height);
        bool compact = w < 900;

        left.Dock = DockStyle.Left;
        left.Width = compact ? Math.Max(230, (int)(w * 0.34)) : Math.Min(420, Math.Max(300, (int)(w * 0.42)));
        right.Dock = DockStyle.Fill;

        box.Width = Math.Min(400, Math.Max(320, right.ClientSize.Width - 40));
        box.Height = Math.Min(360, Math.Max(340, right.ClientSize.Height - 50));
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
            appTitle.TextAlign = ContentAlignment.MiddleCenter;
            appTitle.Left = 20;
            appTitle.Width = Math.Max(190, left.ClientSize.Width - 40);
            appTitle.Top = (logo?.Bottom ?? 190) + 18;
        }
        if (desc != null)
        {
            desc.Left = 24;
            desc.Width = Math.Max(180, left.ClientSize.Width - 48);
            desc.Top = (appTitle?.Bottom ?? 270) + 8;
        }

        // Các ô nhập luôn vừa theo khung, không vượt ra ngoài khi DPI tăng.
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
        if (Tim(form, "pnlNoiDung") is not Panel content) return;

        // Header nội dung luôn cố định chiều cao; phần thân được cuộn.
        if (Tim(form, "pnlDau") is Panel head)
        {
            head.Dock = DockStyle.Top;
            head.Height = Math.Max(72, Math.Min(86, head.Height));
            var title = Tim(head, "lblTieuDe") as Label;
            var desc = Tim(head, "lblMoTaTrang") as Label;
            if (title != null)
            {
                title.AutoSize = false;
                title.Left = 76;
                title.Top = 10;
                title.Height = 34;
                title.Width = Math.Max(180, head.ClientSize.Width - 96);
                title.AutoEllipsis = true;
            }
            if (desc != null)
            {
                desc.AutoSize = false;
                desc.Left = 78;
                desc.Top = 45;
                desc.Height = 20;
                desc.Width = Math.Max(180, head.ClientSize.Width - 100);
                desc.AutoEllipsis = true;
            }
        }

        // Các panel nội dung có Dock sẽ tự chiếm vùng còn lại.
        content.AutoScroll = true;
        content.HorizontalScroll.Enabled = true;
        content.VerticalScroll.Enabled = true;
    }

    private static void ManagementResize(object? sender, EventArgs e)
    {
        if (sender is not Form form) return;
        var content = Tim(form, "pnlNoiDung") as Panel;
        var list = Tim(form, "pnlTrai") as Panel;
        var detail = Tim(form, "pnlPhai") as Panel;
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
                list.Dock = DockStyle.Fill;
                list.Width = Math.Max(500, w - detail.Width);
                list.Height = Math.Max(list.Height, 420);
            }
            else
            {
                // Hai cột không đủ chỗ => xếp dọc. Toàn bộ form có thể cuộn, không control nào bị đè.
                detail.Dock = DockStyle.Top;
                detail.Width = Math.Max(320, w);
                detail.Height = Math.Max(detail.Height, form.Name == "frmQuanLyTaiKhoan" ? 560 : 500);
                list.Dock = DockStyle.Top;
                list.Width = Math.Max(320, w);
                list.Height = 430;
                list.BringToFront();
                detail.BringToFront();
                list.SendToBack();

                // Toolbar tìm kiếm không còn cố định 614px khi màn hình hẹp.
                if (Tim(list, "pnlThanhCongCu") is Panel toolbar)
                {
                    toolbar.AutoScroll = true;
                    toolbar.HorizontalScroll.Enabled = true;
                }
            }
        }
        finally { content.ResumeLayout(true); }
    }

    private static void BookingResize(object? sender, EventArgs e)
    {
        if (sender is not Form form) return;
        var content = Tim(form, "pnlNoiDung") as Panel;
        var left = Tim(form, "pnlTrai") as Panel;
        var right = Tim(form, "pnlPhai") as Panel;
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
            }
            else
            {
                left.Dock = DockStyle.Top;
                left.Width = w;
                left.Height = 610;
                right.Dock = DockStyle.Top;
                right.Width = w;
                right.Height = 560;
                left.BringToFront();
            }
        }
        finally { content.ResumeLayout(true); }
    }

    private static void DashboardResize(object? sender, EventArgs e)
    {
        if (sender is not Form form) return;
        var content = Tim(form, "pnlNoiDung") as Panel;
        var kpi = Tim(form, "pnlKpi") as Panel;
        var chart = Tim(form, "pnlBieuDo") as Panel;
        var bottom = Tim(form, "pnlDuoi") as Panel;
        if (content == null || kpi == null || chart == null || bottom == null) return;

        content.AutoScroll = true;
        int w = Math.Max(320, content.ClientSize.Width - content.Padding.Horizontal);
        bool wide = w >= 1050;

        kpi.Dock = DockStyle.Top;
        kpi.Width = w;
        chart.Dock = DockStyle.Top;
        chart.Width = w;
        bottom.Dock = DockStyle.Top;
        bottom.Width = w;

        if (wide)
        {
            kpi.Height = Math.Max(120, kpi.Height);
            chart.Height = Math.Max(260, chart.Height);
            bottom.Height = Math.Max(240, bottom.Height);
        }
        else
        {
            kpi.Height = Math.Max(230, kpi.Height);
            chart.Height = Math.Max(360, chart.Height);
            bottom.Height = Math.Max(300, bottom.Height);
        }
    }

    private static void CustomerHomeResize(object? sender, EventArgs e)
    {
        if (sender is not Form form) return;
        var content = Tim(form, "pnlNoiDung") as Panel;
        var kpi = Tim(form, "pnlKpi") as Panel;
        var bottom = Tim(form, "pnlDuoi") as Panel;
        var upcoming = Tim(form, "pnlSapToi") as Panel;
        var vouchers = Tim(form, "pnlVoucher") as Panel;
        if (content == null || kpi == null || bottom == null || upcoming == null || vouchers == null) return;

        var cards = new[]
        {
            Tim(form, "kpiSoLanDat") as Control,
            Tim(form, "kpiChiTieu") as Control,
            Tim(form, "kpiVoucher") as Control,
            Tim(form, "kpiSanYeuThich") as Control
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
            // Khi hẹp, đưa nút xuống hàng thứ hai thay vì đè lên thông tin thống kê.
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
