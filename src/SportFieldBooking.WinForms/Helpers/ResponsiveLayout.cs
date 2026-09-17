#nullable enable
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

        // Chạy TRƯỚC mọi xử lý chuyên biệt: dồn lại các hàng công cụ cho hết
        // chồng chéo/cắt cạnh, rồi các hàm bên dưới mới đo kích thước thật.
        TuDongDanTrang(form);

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

        // Form đặt sân: 2 cột khi đủ rộng, 1 cột khi hẹp.
        if (ConTrucTiep(form, "pnlTrai") is Panel trai && ConTrucTiep(form, "pnlPhai") is Panel phai &&
            form.Name.StartsWith("frmDatSan", StringComparison.Ordinal))
        {
            form.Resize -= BookingResize;
            form.Resize += BookingResize;
            BookingResize(form, EventArgs.Empty);
        }

        // Các màn hình quản lý có danh sách + chi tiết.
        if (ManagementForms.Contains(form.Name) && ConTrucTiep(form, "pnlTrai") is Panel list && ConTrucTiep(form, "pnlPhai") is Panel detail)
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

        // Trang chủ khách hàng: 4 thẻ KPI + 2 vùng nội dung.
        // (Trước đây nhánh này nằm trong ApDungMain nên KHÔNG BAO GIỜ CHẠY:
        //  frmTrangChuKhachHang là Form con nhúng, không phải màn hình chính
        //  => 4 thẻ KPI đứng yên ở toạ độ Designer 4x280px = 1168px và chồng lấn
        //  lên nhau khi vùng nội dung thực tế hẹp hơn.)
        if (form.Name == "frmTrangChuKhachHang")
        {
            form.Resize -= CustomerHomeResize;
            form.Resize += CustomerHomeResize;
            CustomerHomeResize(form, EventArgs.Empty);
        }

        // Khi kích thước form đổi (cửa sổ co giãn, form con được nhúng vào vùng
        // nội dung hẹp hơn) thì xếp lại mọi hàng công cụ theo bề rộng mới.
        form.Resize -= FormDoiKichThuoc;
        form.Resize += FormDoiKichThuoc;

        // Các trang danh sách có bộ lọc/footer: tự thu gọn và cho phép cuộn ngang.
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

        // QUAN TRONG: cac control nay la CON CUA header (pnlTren), khong phai con
        // cua form. Neu tim theo form se tra ve null -> khoi canh header khong chay
        // -> giu nguyen toa do Designer + Anchor=Right -> khi header rong ra thi
        // ten/vai tro/icon DE LEN nut chu de va tran meo phai.
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

            // Vung chu de chiem ben phai; phan con lai chia: tieu de | icon | ten/vai tro.
            int bienPhai = hw - themeWidth;      // me trai cua vung nut chu de
            int margin = 14;

            int stackW = Math.Min(200, Math.Max(120, hw / 6));
            int stackRight = bienPhai - margin;
            int stackLeft = Math.Max(220, stackRight - stackW);

            // Man hinh hep: an dong vai tro de ten khong bi cat.
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
        GiaoDien.TrangTriDoc(left);          // nền gradient + quầng sáng thương hiệu

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
            appTitle.TextAlign = ContentAlignment.MiddleLeft;
            appTitle.Left = 20;
            appTitle.Width = Math.Max(190, left.ClientSize.Width - 40);
            appTitle.Top = (logo?.Bottom ?? 190) + 18;
            // Đo chiều cao thật theo font/chữ để tiêu đề không bị cắt cạnh dưới.
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

        // LẤY THEO TÊN TRONG CON TRỰC TIẾP: khi form này được nhúng làm form con,
        // pnlNoiDung của nó đang chứa một Form khác; Tim() đệ quy sẽ nhặt nhầm
        // panel của form bên trong và phá bố cục của form đó.
        if (ConTrucTiep(form, "pnlNoiDung") is not Panel content) return;

        // Header nội dung: cho phép cao thêm nếu tiêu đề/mô tả cần chỗ, không ép cứng.
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
        }

        // Các panel nội dung có Dock sẽ tự chiếm vùng còn lại.
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
        var content = ConTrucTiep(form, "pnlNoiDung") as Panel;
        var kpi = ConTrucTiep(form, "pnlKpi") as Panel;
        var chart = ConTrucTiep(form, "pnlBieuDo") as Panel;
        var bottom = ConTrucTiep(form, "pnlDuoi") as Panel;
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

        // Ba khối trên đã được căn đúng bề rộng w, nên TẮT cuộn ngang của vùng
        // chứa. Nếu để cuộn ngang, các panel Dock=Top (pnlDau chứa nút "Làm mới")
        // sẽ bị kéo giãn theo bề rộng cuộn => nút bấm văng ra ngoài màn hình.
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

    // ==================================================================
    //  ENGINE TỰ ĐỘNG DÀN TRANG (bổ sung bản khắc phục chồng chéo)
    // ------------------------------------------------------------------
    //  Vấn đề gốc của bản V5:
    //   1. GiaoDien.DangNutChinh/Phu/NguyHiem() ép mọi Button về Height=38
    //      và MinimumSize=96x34. Designer lại vẽ panel chứa theo nút cao 34px
    //      => nút bị panel CẮT MẤT cạnh dưới, đồng thời nút hẹp (<96px) bị
    //      nới rộng ra và ĐÈ LÊN nút bên cạnh.
    //   2. Toạ độ trong Designer được đặt cho form rộng 1200px. Khi form bị
    //      nhúng vào pnlNoiDung của màn hình chính (chỉ còn 670-1020px), mọi
    //      control giữ nguyên toạ độ cũ => tràn ra ngoài / chồng lên nhau.
    //   3. Tim() tìm control ĐỆ QUY nên khi một Form con đã được nhúng vào
    //      pnlNoiDung, hàm có thể "nhặt nhầm" panel của form con (ví dụ
    //      ContentResize đổi chiều cao pnlDau của form con) => vỡ bố cục.
    //
    //  Cách khắc phục: sau khi mọi style đã áp xong (OnLoad), tự động
    //   - dồn các control cùng hàng về một đường cơ sở chung (canh giữa hàng),
    //   - đẩy control nào chồng lên nhau xuống hàng mới,
    //   - giữ nguyên nhóm control neo phải (Anchor Right),
    //   - nới chiều cao panel cho vừa nội dung thật,
    //   - làm ngược từ panel trong cùng ra ngoài nên kích thước lan truyền đúng.
    // ==================================================================

    /// <summary>Khoảng cách tối thiểu giữa 2 control cạnh nhau khi xếp lại hàng.</summary>
    private const int KhoangCachToiThieu = 8;

    /// <summary>
    /// Các panel "chứa công cụ" được đặt tên theo quy ước trong toàn dự án.
    /// Chỉ những panel này mới được xếp lại hàng tự động - panel nhập liệu
    /// dạng biểu mẫu (nhãn trên / ô nhập dưới) vẫn giữ nguyên toạ độ Designer.
    /// </summary>
    private static readonly HashSet<string> TenHangCongCu = new(StringComparer.Ordinal)
    {
        "pnlBoLoc", "pnlChan", "pnlThanhCongCu", "pnlTaoHoaDon", "pnlNut", "pnlHanhDong"
    };

    /// <summary>Kích thước thật của control sau khi style/MinimumSize có hiệu lực.</summary>
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

    /// <summary>Neo phải = control luôn bám mép phải khi form co giãn.</summary>
    private static bool LaNeoPhai(Control c) => (c.Anchor & AnchorStyles.Right) == AnchorStyles.Right;

    /// <summary>
    /// Tìm control THEO TÊN trong danh sách con TRỰC TIẾP.
    /// Khác Tim(): không đệ quy nên không bao giờ nhặt nhầm control của
    /// Form con đang được nhúng bên trong pnlNoiDung.
    /// </summary>
    private static Control? ConTrucTiep(Control cha, string ten) =>
        cha?.Controls.Find(ten, false).FirstOrDefault();

    /// <summary>
    /// Xếp lại các control KHÔNG Dock bên trong một panel:
    /// gom hàng theo toạ độ thiết kế, canh giữa hàng, đẩy control chồng chéo xuống hàng mới,
    /// nhóm neo phải luôn bám mép phải, không đủ chỗ thì cho cuộn ngang.
    /// </summary>
    private static void XepLaiHang(Panel pnl)
    {
        if (pnl == null || pnl.IsDisposed) return;
        if (!_dangXep.Add(pnl)) return;   // đang trong một lượt xếp rồi: bỏ qua để tránh đệ quy
        try
        {
        bool dockNgangTuDo = pnl.Dock is DockStyle.None or DockStyle.Top or DockStyle.Bottom;

        // Đưa panel về đúng kích thước thiết kế trước khi xếp lại, nhờ vậy
        // form rộng ra thì panel CO LẠI được chứ không phình vĩnh viễn.
        KhoiPhucKichThuoc(pnl);
        int chieuCaoGoc = pnl.Height;

        // Thứ tự Controls: index 0 = thêm SAU CÙNG = vẽ trên cùng.
        // Designer viết Controls.Add theo thứ tự đọc (trái -> phải) nên đảo lại.
        var items = new List<Control>();
        foreach (Control c in pnl.Controls)
        {
            if (c.Dock != DockStyle.None || !c.Visible) continue;
            items.Add(c);
        }
        items.Reverse();
        if (items.Count == 0) return;

        // Ghi nhớ toạ độ thiết kế (một lần duy nhất) để những lần resize sau
        // không bị "trôi" dần vị trí control.
        foreach (Control c in items)
            if (c.Tag is not string t || !t.StartsWith(NhanToaDoGoc, StringComparison.Ordinal))
                c.Tag = NhanToaDoGoc + c.Left + "|" + c.Top;

        var pads = pnl.Padding;
        int mePhai = pnl.ClientSize.Width - pads.Right;

        // --- 1. Gom hàng theo tâm dọc của toạ độ thiết kế (dung sai 16px) ---
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

        // --- 2. Xếp từng hàng --------------------------------------------
        int y = pads.Top;
        foreach (var nhom in hang)
        {
            // Nút bị nới cao/rộng nên đè lên ô nhập cùng dải dọc => tách xuống hàng riêng.
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

                // Nửa phải: bám mép phải, luôn cách nhau >= KhoangCachToiThieu.
                int xPhai = mePhai;
                foreach (Control c in phai)
                {
                    Size sc = KichThuocThat(c);
                    xPhai -= sc.Width;
                    c.Left = Math.Max(pads.Left, xPhai);
                    c.Top = y + (cao - sc.Height) / 2;
                    xPhai -= KhoangCachToiThieu;
                }

                // Nửa trái: giữ toạ độ thiết kế, chỉ đẩy sang phải khi bị chồng.
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

        // --- 3. Nới chiều cao panel cho vừa nội dung thật -----------------
        int canCao = y + pads.Bottom;
        if (dockNgangTuDo && canCao > pnl.Height) pnl.Height = canCao;
        LuuKichThuocGoc(pnl, chieuCaoGoc);
        }
        finally { _dangXep.Remove(pnl); }
    }

    /// <summary>Hình chữ nhật thật (đã tính MinimumSize) theo toạ độ thiết kế.</summary>
    private static Rectangle HcnThat(Control c)
    {
        Size s = KichThuocThat(c);
        Point g = DocToaDoGoc(c);
        return new Rectangle(g.X, g.Y, s.Width, s.Height);
    }

    /// <summary>Đọc toạ độ thiết kế đã lưu trong Tag (chưa bị xếp lại làm thay đổi).</summary>
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
        // Chỉ dùng Tag khi nó còn trống, tránh giẫm lên dữ liệu của chức năng khác.
        if (c.Tag != null) return;
        c.Tag = NhanKichThuocGoc + c.Width + "|" + chieuCaoGoc;
    }

    /// <summary>Trả control về kích thước thiết kế (để panel co lại được khi form rộng ra).</summary>
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

    /// <summary>
    /// Bảo đảm panel chứa đủ nội dung: nới chiều cao khi cần và bật cuộn
    /// để không control nào bị cắt mất. Bỏ qua panel có con Dock=Fill
    /// (thanh cuộn sẽ làm panel con co lại gây giật layout).
    /// </summary>
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

        if (pnl.Dock is DockStyle.None or DockStyle.Top or DockStyle.Bottom && canCao > pnl.Height)
            pnl.Height = canCao;

        if (!coConFill && pnl.Dock != DockStyle.Fill)
        {
            if (canCao > pnl.ClientSize.Height) pnl.AutoScroll = true;
            if (phaiNhat + pnl.Padding.Right > pnl.ClientSize.Width) pnl.AutoScroll = true;
        }
    }

    /// <summary>Chạy engine cho toàn bộ panel của một form (trong cùng -> ra ngoài).</summary>
    private static void TuDongDanTrang(Control root)
    {
        if (root == null || root.IsDisposed) return;

        foreach (Control c in root.Controls)
            if (c is Panel p && TenHangCongCu.Contains(p.Name))
            {
                // Gắn hook để panel tự xếp lại mỗi khi bề rộng đổi (form co giãn,
                // panel cha bị ManagementResize/BookingResize thu hẹp...).
                GanXuLyResize(p);
                XepLaiHang(p);
            }

        // Đệ quy trước rồi mới đo cha: kích thước con đã chốt thì cha mới tính đúng.
        foreach (Control c in root.Controls) TuDongDanTrang(c);

        foreach (Control c in root.Controls)
            if (c is Panel p && p.Dock != DockStyle.Fill)
                VuaKhopNoiDung(p);
    }

    /// <summary>Gắn lại bộ dàn trang khi panel đổi kích thước (form con nhúng, đổi chủ đề...).</summary>
    private static void GanXuLyResize(Panel pnl)
    {
        pnl.Resize -= PanelResize;
        pnl.Resize += PanelResize;
    }

    private static void PanelResize(object? sender, EventArgs e)
    {
        if (sender is Panel p) XepLaiHang(p);
    }

    /// <summary>Các panel đang được xếp lại - chống vòng lặp Resize -> Height -> Resize.</summary>
    private static readonly HashSet<Panel> _dangXep = new();

    /// <summary>
    /// Form đổi kích thước => xếp lại các hàng công cụ theo bề rộng mới.
    /// Chỉ chạy khi bề rộng thật sự thay đổi (tránh dàn trang lại liên tục
    /// khi người dùng kéo giãn chiều cao cửa sổ).
    /// </summary>
    private static void FormDoiKichThuoc(object? sender, EventArgs e)
    {
        if (sender is not Form form || form.IsDisposed) return;
        if (form.Tag is string s && s.StartsWith("RESPONSIVE_W|", StringComparison.Ordinal)
            && int.TryParse(s.AsSpan(13), out int cu) && cu == form.ClientSize.Width) return;

        form.Tag = "RESPONSIVE_W|" + form.ClientSize.Width;
        TuDongDanTrang(form);
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
