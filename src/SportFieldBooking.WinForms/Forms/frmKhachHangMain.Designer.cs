using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmKhachHangMain
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        pnlSidebar = new Panel();
        pnlMenu = new Panel();
        btnThongTin = new SidebarButton();
        btnVoucher = new SidebarButton();
        btnHoaDon = new SidebarButton();
        btnLichSuDatSan = new SidebarButton();
        btnDatSan = new SidebarButton();
        btnTrangChu = new SidebarButton();
        pnlChan = new Panel();
        btnDangXuat = new SidebarButton();
        btnDoiMatKhau = new SidebarButton();
        pnlLogo = new Panel();
        lblTenPhanMem = new Label();
        lblTenTrungTam = new Label();
        picLogo = new IconBox();
        pnlTren = new Panel();
        pnlChuDe = new Panel();
        btnChuDe = new Button();
        lblVaiTro = new Label();
        lblTenNguoiDung = new Label();
        picNguoiDung = new IconBox();
        lblTieuDeTrang = new Label();
        pnlNoiDung = new Panel();
        pnlSidebar.SuspendLayout();
        pnlMenu.SuspendLayout();
        pnlChan.SuspendLayout();
        pnlLogo.SuspendLayout();
        pnlTren.SuspendLayout();
        pnlChuDe.SuspendLayout();
        SuspendLayout();

        pnlSidebar.BackColor = GiaoDien.ThanhBen;
        pnlSidebar.Controls.Add(pnlMenu);
        pnlSidebar.Controls.Add(pnlChan);
        pnlSidebar.Controls.Add(pnlLogo);
        pnlSidebar.Dock = DockStyle.Left;
        pnlSidebar.Location = new System.Drawing.Point(0, 0);
        pnlSidebar.Name = "pnlSidebar";
        pnlSidebar.Size = new System.Drawing.Size(240, 750);
        pnlSidebar.TabIndex = 0;

        pnlLogo.BackColor = GiaoDien.ThanhBen;
        pnlLogo.Controls.Add(lblTenPhanMem);
        pnlLogo.Controls.Add(lblTenTrungTam);
        pnlLogo.Controls.Add(picLogo);
        pnlLogo.Dock = DockStyle.Top;
        pnlLogo.Location = new System.Drawing.Point(0, 0);
        pnlLogo.Name = "pnlLogo";
        pnlLogo.Size = new System.Drawing.Size(240, 86);
        pnlLogo.TabIndex = 0;

        picLogo.Location = new System.Drawing.Point(18, 18);
        picLogo.MauBieuTuong = GiaoDien.Chinh;
        picLogo.Name = "picLogo";
        picLogo.Size = new System.Drawing.Size(48, 48);
        picLogo.TabIndex = 0;
        picLogo.TenBieuTuong = "sanbong";

        lblTenTrungTam.AutoSize = true;
        lblTenTrungTam.Font = new System.Drawing.Font(GiaoDien.TenFont, 11.5F, System.Drawing.FontStyle.Bold);
        lblTenTrungTam.ForeColor = Color.White;
        lblTenTrungTam.Location = new System.Drawing.Point(70, 17);
        lblTenTrungTam.Name = "lblTenTrungTam";
        lblTenTrungTam.AutoSize = false;
        lblTenTrungTam.Size = new System.Drawing.Size(158, 40);
        lblTenTrungTam.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblTenTrungTam.TabIndex = 1;
        lblTenTrungTam.Text = "THUÊ SÂN THỂ THAO";

        lblTenPhanMem.AutoSize = true;
        lblTenPhanMem.Font = GiaoDien.ChuNho;
        lblTenPhanMem.ForeColor = Color.FromArgb(148, 163, 184);
        lblTenPhanMem.Location = new System.Drawing.Point(70, 55);
        lblTenPhanMem.Name = "lblTenPhanMem";
        lblTenPhanMem.Size = new System.Drawing.Size(158, 17);
        lblTenPhanMem.TabIndex = 2;
        lblTenPhanMem.Text = "Cổng khách hàng";

        pnlChan.BackColor = GiaoDien.ThanhBen;
        pnlChan.Controls.Add(btnDangXuat);
        pnlChan.Controls.Add(btnDoiMatKhau);
        pnlChan.Dock = DockStyle.Bottom;
        pnlChan.Location = new System.Drawing.Point(0, 648);
        pnlChan.Name = "pnlChan";
        pnlChan.Padding = new Padding(12, 8, 12, 12);
        pnlChan.Size = new System.Drawing.Size(240, 102);
        pnlChan.TabIndex = 1;

        btnDoiMatKhau.Dock = DockStyle.Top;
        btnDoiMatKhau.Location = new System.Drawing.Point(12, 8);
        btnDoiMatKhau.Name = "btnDoiMatKhau";
        btnDoiMatKhau.Size = new System.Drawing.Size(216, 40);
        btnDoiMatKhau.TabIndex = 0;
        btnDoiMatKhau.TenBieuTuong = "khoa";
        btnDoiMatKhau.Text = "Đổi mật khẩu";
        btnDoiMatKhau.Click += btnDoiMatKhau_Click;

        btnDangXuat.Dock = DockStyle.Top;
        btnDangXuat.LaNutDangXuat = true;
        btnDangXuat.Location = new System.Drawing.Point(12, 48);
        btnDangXuat.Name = "btnDangXuat";
        btnDangXuat.Size = new System.Drawing.Size(216, 40);
        btnDangXuat.TabIndex = 1;
        btnDangXuat.TenBieuTuong = "dangxuat";
        btnDangXuat.Text = "Đăng xuất";
        btnDangXuat.Click += btnDangXuat_Click;

        pnlMenu.AutoScroll = true;
        pnlMenu.BackColor = GiaoDien.ThanhBen;
        pnlMenu.Controls.Add(btnThongTin);
        pnlMenu.Controls.Add(btnVoucher);
        pnlMenu.Controls.Add(btnHoaDon);
        pnlMenu.Controls.Add(btnLichSuDatSan);
        pnlMenu.Controls.Add(btnDatSan);
        pnlMenu.Controls.Add(btnTrangChu);
        pnlMenu.Dock = DockStyle.Fill;
        pnlMenu.Location = new System.Drawing.Point(0, 86);
        pnlMenu.Name = "pnlMenu";
        pnlMenu.Padding = new Padding(12, 10, 12, 10);
        pnlMenu.Size = new System.Drawing.Size(240, 562);
        pnlMenu.TabIndex = 2;

        btnTrangChu.Dock = DockStyle.Top;
        btnTrangChu.Location = new System.Drawing.Point(12, 10);
        btnTrangChu.Name = "btnTrangChu";
        btnTrangChu.Size = new System.Drawing.Size(216, 42);
        btnTrangChu.TabIndex = 0;
        btnTrangChu.TenBieuTuong = "thongke";
        btnTrangChu.Text = "Trang chủ";
        btnTrangChu.Click += NhanMenu;

        btnDatSan.Dock = DockStyle.Top;
        btnDatSan.Location = new System.Drawing.Point(12, 52);
        btnDatSan.Name = "btnDatSan";
        btnDatSan.Size = new System.Drawing.Size(216, 42);
        btnDatSan.TabIndex = 1;
        btnDatSan.TenBieuTuong = "calendar";
        btnDatSan.Text = "Đặt sân";
        btnDatSan.Click += NhanMenu;

        btnLichSuDatSan.Dock = DockStyle.Top;
        btnLichSuDatSan.Location = new System.Drawing.Point(12, 94);
        btnLichSuDatSan.Name = "btnLichSuDatSan";
        btnLichSuDatSan.Size = new System.Drawing.Size(216, 42);
        btnLichSuDatSan.TabIndex = 2;
        btnLichSuDatSan.TenBieuTuong = "lich";
        btnLichSuDatSan.Text = "Lịch sử đặt sân";
        btnLichSuDatSan.Click += NhanMenu;

        btnHoaDon.Dock = DockStyle.Top;
        btnHoaDon.Location = new System.Drawing.Point(12, 136);
        btnHoaDon.Name = "btnHoaDon";
        btnHoaDon.Size = new System.Drawing.Size(216, 42);
        btnHoaDon.TabIndex = 3;
        btnHoaDon.TenBieuTuong = "hoadon";
        btnHoaDon.Text = "Hóa đơn của tôi";
        btnHoaDon.Click += NhanMenu;

        btnVoucher.Dock = DockStyle.Top;
        btnVoucher.Location = new System.Drawing.Point(12, 178);
        btnVoucher.Name = "btnVoucher";
        btnVoucher.Size = new System.Drawing.Size(216, 42);
        btnVoucher.TabIndex = 4;
        btnVoucher.TenBieuTuong = "voucher";
        btnVoucher.Text = "Voucher của tôi";
        btnVoucher.Click += NhanMenu;

        btnThongTin.Dock = DockStyle.Top;
        btnThongTin.Location = new System.Drawing.Point(12, 220);
        btnThongTin.Name = "btnThongTin";
        btnThongTin.Size = new System.Drawing.Size(216, 42);
        btnThongTin.TabIndex = 5;
        btnThongTin.TenBieuTuong = "user";
        btnThongTin.Text = "Thông tin cá nhân";
        btnThongTin.Click += NhanMenu;

        pnlTren.BackColor = GiaoDien.BeMat;
        pnlTren.Controls.Add(lblVaiTro);
        pnlTren.Controls.Add(lblTenNguoiDung);
        pnlTren.Controls.Add(picNguoiDung);
        pnlTren.Controls.Add(pnlChuDe);
        pnlTren.Controls.Add(lblTieuDeTrang);
        pnlTren.Dock = DockStyle.Top;
        pnlTren.Location = new System.Drawing.Point(240, 0);
        pnlTren.Name = "pnlTren";
        pnlTren.Size = new System.Drawing.Size(1020, 66);
        pnlTren.TabIndex = 1;

        lblTieuDeTrang.AutoSize = true;
        lblTieuDeTrang.Font = GiaoDien.TieuDe;
        lblTieuDeTrang.ForeColor = GiaoDien.Chu;
        lblTieuDeTrang.Location = new System.Drawing.Point(24, 16);
        lblTieuDeTrang.Name = "lblTieuDeTrang";
        lblTieuDeTrang.Size = new System.Drawing.Size(160, 31);
        lblTieuDeTrang.TabIndex = 0;
        lblTieuDeTrang.Text = "Trang chủ";

        picNguoiDung.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        picNguoiDung.Location = new System.Drawing.Point(766, 12);
        picNguoiDung.MauBieuTuong = GiaoDien.Chinh;
        picNguoiDung.Name = "picNguoiDung";
        picNguoiDung.Size = new System.Drawing.Size(40, 40);
        picNguoiDung.TabIndex = 1;
        picNguoiDung.TenBieuTuong = "user";

        lblTenNguoiDung.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblTenNguoiDung.Font = GiaoDien.ChuDam;
        lblTenNguoiDung.ForeColor = GiaoDien.Chu;
        lblTenNguoiDung.Location = new System.Drawing.Point(820, 14);
        lblTenNguoiDung.Name = "lblTenNguoiDung";
        lblTenNguoiDung.Size = new System.Drawing.Size(170, 17);
        lblTenNguoiDung.TabIndex = 2;
        lblTenNguoiDung.Text = "Khách hàng";
        lblTenNguoiDung.TextAlign = ContentAlignment.MiddleRight;

        lblVaiTro.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblVaiTro.Font = GiaoDien.ChuNho;
        lblVaiTro.ForeColor = GiaoDien.ChuPhu;
        lblVaiTro.Location = new System.Drawing.Point(820, 34);
        lblVaiTro.Name = "lblVaiTro";
        lblVaiTro.Size = new System.Drawing.Size(170, 15);
        lblVaiTro.TabIndex = 3;
        lblVaiTro.Text = "Khách hàng";
        lblVaiTro.TextAlign = ContentAlignment.MiddleRight;

        pnlNoiDung.BackColor = GiaoDien.ManHinhNen;
        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(240, 66);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Size = new System.Drawing.Size(1020, 684);
        pnlNoiDung.TabIndex = 2;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1280, 750);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlTren);
        Controls.Add(pnlSidebar);
        MinimumSize = new System.Drawing.Size(860, 560);
        Name = "frmKhachHangMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Thuê sân thể thao - Cổng khách hàng";
        WindowState = FormWindowState.Maximized;
        FormClosing += frmKhachHangMain_FormClosing;
        pnlSidebar.ResumeLayout(false);
        pnlMenu.ResumeLayout(false);
        pnlChan.ResumeLayout(false);
        pnlLogo.ResumeLayout(false);
        pnlChuDe.BackColor = GiaoDien.BeMat;
        pnlChuDe.Controls.Add(btnChuDe);
        pnlChuDe.Dock = DockStyle.Right;
        pnlChuDe.Location = new System.Drawing.Point(860, 0);
        pnlChuDe.Name = "pnlChuDe";
        pnlChuDe.Padding = new Padding(0, 12, 18, 12);
        pnlChuDe.Size = new System.Drawing.Size(156, 66);
        pnlChuDe.TabIndex = 5;

        btnChuDe.Dock = DockStyle.Fill;
        btnChuDe.FlatStyle = FlatStyle.Flat;
        btnChuDe.Font = GiaoDien.ChuDam;
        btnChuDe.Name = "btnChuDe";
        btnChuDe.Size = new System.Drawing.Size(138, 42);
        btnChuDe.TabIndex = 0;
        btnChuDe.Text = "Chủ đề";
        btnChuDe.TextImageRelation = TextImageRelation.ImageBeforeText;
        btnChuDe.UseVisualStyleBackColor = false;
        btnChuDe.Click += btnChuDe_Click;

        pnlChuDe.ResumeLayout(false);
        pnlTren.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlSidebar;
    private Panel pnlMenu;
    private SidebarButton btnThongTin;
    private SidebarButton btnVoucher;
    private SidebarButton btnHoaDon;
    private SidebarButton btnLichSuDatSan;
    private SidebarButton btnDatSan;
    private SidebarButton btnTrangChu;
    private Panel pnlChan;
    private SidebarButton btnDangXuat;
    private SidebarButton btnDoiMatKhau;
    private Panel pnlLogo;
    private Label lblTenPhanMem;
    private Label lblTenTrungTam;
    private IconBox picLogo;
    private Panel pnlTren;
    private Panel pnlChuDe;
    private Button btnChuDe;
    private Label lblVaiTro;
    private Label lblTenNguoiDung;
    private IconBox picNguoiDung;
    private Label lblTieuDeTrang;
    private Panel pnlNoiDung;
}
