using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmAdminMain
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
        pnlChan = new Panel();
        btnDangXuat = new SidebarButton();
        btnDoiMatKhau = new SidebarButton();
        pnlMenu = new Panel();
        btnCauHinh = new SidebarButton();
        btnThongKe = new SidebarButton();
        btnNhanVien = new SidebarButton();
        btnTaiKhoan = new SidebarButton();
        btnKhuyenMai = new SidebarButton();
        btnVoucher = new SidebarButton();
        btnHoaDon = new SidebarButton();
        btnLichDatSan = new SidebarButton();
        btnDatSan = new SidebarButton();
        btnKhachHang = new SidebarButton();
        btnSan = new SidebarButton();
        btnLoaiSan = new SidebarButton();
        btnTongQuan = new SidebarButton();
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
        pnlChan.SuspendLayout();
        pnlMenu.SuspendLayout();
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
        lblTenTrungTam.Size = new System.Drawing.Size(148, 40);
        lblTenTrungTam.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblTenTrungTam.TabIndex = 1;
        lblTenTrungTam.Text = "THUÊ SÂN THỂ THAO";

        lblTenPhanMem.AutoSize = true;
        lblTenPhanMem.Font = GiaoDien.ChuNho;
        lblTenPhanMem.ForeColor = Color.FromArgb(148, 163, 184);
        lblTenPhanMem.Location = new System.Drawing.Point(70, 55);
        lblTenPhanMem.Name = "lblTenPhanMem";
        lblTenPhanMem.Size = new System.Drawing.Size(148, 17);
        lblTenPhanMem.TabIndex = 2;
        lblTenPhanMem.Text = "Quản trị viên";

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
        pnlMenu.Controls.Add(btnCauHinh);
        pnlMenu.Controls.Add(btnThongKe);
        pnlMenu.Controls.Add(btnNhanVien);
        pnlMenu.Controls.Add(btnTaiKhoan);
        pnlMenu.Controls.Add(btnKhuyenMai);
        pnlMenu.Controls.Add(btnVoucher);
        pnlMenu.Controls.Add(btnHoaDon);
        pnlMenu.Controls.Add(btnLichDatSan);
        pnlMenu.Controls.Add(btnDatSan);
        pnlMenu.Controls.Add(btnKhachHang);
        pnlMenu.Controls.Add(btnSan);
        pnlMenu.Controls.Add(btnLoaiSan);
        pnlMenu.Controls.Add(btnTongQuan);
        pnlMenu.Dock = DockStyle.Fill;
        pnlMenu.Location = new System.Drawing.Point(0, 86);
        pnlMenu.Name = "pnlMenu";
        pnlMenu.Padding = new Padding(12, 10, 12, 10);
        pnlMenu.Size = new System.Drawing.Size(240, 562);
        pnlMenu.TabIndex = 2;

        btnTongQuan.Dock = DockStyle.Top;
        btnTongQuan.Location = new System.Drawing.Point(12, 10);
        btnTongQuan.Name = "btnTongQuan";
        btnTongQuan.Size = new System.Drawing.Size(216, 42);
        btnTongQuan.TabIndex = 0;
        btnTongQuan.TenBieuTuong = "thongke";
        btnTongQuan.Text = "Tổng quan";
        btnTongQuan.Click += NhanMenu;

        btnLoaiSan.Dock = DockStyle.Top;
        btnLoaiSan.Location = new System.Drawing.Point(12, 52);
        btnLoaiSan.Name = "btnLoaiSan";
        btnLoaiSan.Size = new System.Drawing.Size(216, 42);
        btnLoaiSan.TabIndex = 1;
        btnLoaiSan.TenBieuTuong = "loaisan";
        btnLoaiSan.Text = "Loại sân";
        btnLoaiSan.Click += NhanMenu;

        btnSan.Dock = DockStyle.Top;
        btnSan.Location = new System.Drawing.Point(12, 94);
        btnSan.Name = "btnSan";
        btnSan.Size = new System.Drawing.Size(216, 42);
        btnSan.TabIndex = 2;
        btnSan.TenBieuTuong = "sanbong";
        btnSan.Text = "Quản lý sân";
        btnSan.Click += NhanMenu;

        btnKhachHang.Dock = DockStyle.Top;
        btnKhachHang.Location = new System.Drawing.Point(12, 136);
        btnKhachHang.Name = "btnKhachHang";
        btnKhachHang.Size = new System.Drawing.Size(216, 42);
        btnKhachHang.TabIndex = 3;
        btnKhachHang.TenBieuTuong = "user";
        btnKhachHang.Text = "Khách hàng";
        btnKhachHang.Click += NhanMenu;

        btnDatSan.Dock = DockStyle.Top;
        btnDatSan.Location = new System.Drawing.Point(12, 178);
        btnDatSan.Name = "btnDatSan";
        btnDatSan.Size = new System.Drawing.Size(216, 42);
        btnDatSan.TabIndex = 4;
        btnDatSan.TenBieuTuong = "calendar";
        btnDatSan.Text = "Đặt sân";
        btnDatSan.Click += NhanMenu;

        btnLichDatSan.Dock = DockStyle.Top;
        btnLichDatSan.Location = new System.Drawing.Point(12, 220);
        btnLichDatSan.Name = "btnLichDatSan";
        btnLichDatSan.Size = new System.Drawing.Size(216, 42);
        btnLichDatSan.TabIndex = 5;
        btnLichDatSan.TenBieuTuong = "lich";
        btnLichDatSan.Text = "Lịch đặt sân";
        btnLichDatSan.Click += NhanMenu;

        btnHoaDon.Dock = DockStyle.Top;
        btnHoaDon.Location = new System.Drawing.Point(12, 262);
        btnHoaDon.Name = "btnHoaDon";
        btnHoaDon.Size = new System.Drawing.Size(216, 42);
        btnHoaDon.TabIndex = 6;
        btnHoaDon.TenBieuTuong = "hoadon";
        btnHoaDon.Text = "Hóa đơn";
        btnHoaDon.Click += NhanMenu;

        btnVoucher.Dock = DockStyle.Top;
        btnVoucher.Location = new System.Drawing.Point(12, 304);
        btnVoucher.Name = "btnVoucher";
        btnVoucher.Size = new System.Drawing.Size(216, 42);
        btnVoucher.TabIndex = 7;
        btnVoucher.TenBieuTuong = "voucher";
        btnVoucher.Text = "Voucher";
        btnVoucher.Click += NhanMenu;

        btnKhuyenMai.Dock = DockStyle.Top;
        btnKhuyenMai.Location = new System.Drawing.Point(12, 346);
        btnKhuyenMai.Name = "btnKhuyenMai";
        btnKhuyenMai.Size = new System.Drawing.Size(216, 42);
        btnKhuyenMai.TabIndex = 8;
        btnKhuyenMai.TenBieuTuong = "khuyenmai";
        btnKhuyenMai.Text = "Khuyến mãi";
        btnKhuyenMai.Click += NhanMenu;

        btnTaiKhoan.Dock = DockStyle.Top;
        btnTaiKhoan.Location = new System.Drawing.Point(12, 388);
        btnTaiKhoan.Name = "btnTaiKhoan";
        btnTaiKhoan.Size = new System.Drawing.Size(216, 42);
        btnTaiKhoan.TabIndex = 9;
        btnTaiKhoan.TenBieuTuong = "taikhoan";
        btnTaiKhoan.Text = "Tài khoản";
        btnTaiKhoan.Click += NhanMenu;

        btnNhanVien.Dock = DockStyle.Top;
        btnNhanVien.Location = new System.Drawing.Point(12, 430);
        btnNhanVien.Name = "btnNhanVien";
        btnNhanVien.Size = new System.Drawing.Size(216, 42);
        btnNhanVien.TabIndex = 10;
        btnNhanVien.TenBieuTuong = "nhanvien";
        btnNhanVien.Text = "Nhân viên";
        btnNhanVien.Click += NhanMenu;

        btnThongKe.Dock = DockStyle.Top;
        btnThongKe.Location = new System.Drawing.Point(12, 472);
        btnThongKe.Name = "btnThongKe";
        btnThongKe.Size = new System.Drawing.Size(216, 42);
        btnThongKe.TabIndex = 11;
        btnThongKe.TenBieuTuong = "bieudo";
        btnThongKe.Text = "Thống kê & báo cáo";
        btnThongKe.Click += NhanMenu;

        btnCauHinh.Dock = DockStyle.Top;
        btnCauHinh.Location = new System.Drawing.Point(12, 514);
        btnCauHinh.Name = "btnCauHinh";
        btnCauHinh.Size = new System.Drawing.Size(216, 42);
        btnCauHinh.TabIndex = 12;
        btnCauHinh.TenBieuTuong = "cauhinh";
        btnCauHinh.Text = "Cấu hình";
        btnCauHinh.Click += NhanMenu;

        pnlTren.BackColor = GiaoDien.BeMat;
        pnlTren.Controls.Add(lblVaiTro);
        pnlTren.Controls.Add(lblTenNguoiDung);
        pnlTren.Controls.Add(picNguoiDung);
        pnlTren.Controls.Add(pnlChuDe);
        pnlTren.Controls.Add(lblTieuDeTrang);
        pnlTren.Dock = DockStyle.Top;
        pnlTren.Location = new System.Drawing.Point(260, 0);
        pnlTren.Name = "pnlTren";
        pnlTren.Size = new System.Drawing.Size(1020, 66);
        pnlTren.TabIndex = 1;

        lblTieuDeTrang.AutoSize = true;
        lblTieuDeTrang.Font = GiaoDien.TieuDe;
        lblTieuDeTrang.ForeColor = GiaoDien.Chu;
        lblTieuDeTrang.Location = new System.Drawing.Point(24, 16);
        lblTieuDeTrang.Name = "lblTieuDeTrang";
        lblTieuDeTrang.Size = new System.Drawing.Size(220, 31);
        lblTieuDeTrang.TabIndex = 0;
        lblTieuDeTrang.Text = "Tổng quan hệ thống";

        picNguoiDung.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        picNguoiDung.Location = new System.Drawing.Point(766, 12);
        picNguoiDung.MauBieuTuong = GiaoDien.Chinh;
        picNguoiDung.Name = "picNguoiDung";
        picNguoiDung.Size = new System.Drawing.Size(40, 40);
        picNguoiDung.TabIndex = 1;
        picNguoiDung.TenBieuTuong = "taikhoan";

        lblTenNguoiDung.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblTenNguoiDung.Font = GiaoDien.ChuDam;
        lblTenNguoiDung.ForeColor = GiaoDien.Chu;
        lblTenNguoiDung.Location = new System.Drawing.Point(820, 14);
        lblTenNguoiDung.Name = "lblTenNguoiDung";
        lblTenNguoiDung.Size = new System.Drawing.Size(170, 17);
        lblTenNguoiDung.TabIndex = 2;
        lblTenNguoiDung.Text = "Quản trị viên";
        lblTenNguoiDung.TextAlign = ContentAlignment.MiddleRight;

        lblVaiTro.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblVaiTro.Font = GiaoDien.ChuNho;
        lblVaiTro.ForeColor = GiaoDien.ChuPhu;
        lblVaiTro.Location = new System.Drawing.Point(820, 34);
        lblVaiTro.Name = "lblVaiTro";
        lblVaiTro.Size = new System.Drawing.Size(170, 15);
        lblVaiTro.TabIndex = 3;
        lblVaiTro.Text = "Quản trị viên";
        lblVaiTro.TextAlign = ContentAlignment.MiddleRight;

        pnlNoiDung.BackColor = GiaoDien.ManHinhNen;
        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(260, 66);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Size = new System.Drawing.Size(1020, 684);
        pnlNoiDung.TabIndex = 2;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1280, 750);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlTren);
        Controls.Add(pnlSidebar);
        MinimumSize = new System.Drawing.Size(900, 600);
        Name = "frmAdminMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Quản lý cho thuê sân thể thao - Quản trị viên";
        WindowState = FormWindowState.Maximized;
        FormClosing += frmAdminMain_FormClosing;
        pnlSidebar.ResumeLayout(false);
        pnlChan.ResumeLayout(false);
        pnlMenu.ResumeLayout(false);
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
    private Panel pnlChan;
    private SidebarButton btnDangXuat;
    private SidebarButton btnDoiMatKhau;
    private Panel pnlMenu;
    private SidebarButton btnCauHinh;
    private SidebarButton btnThongKe;
    private SidebarButton btnNhanVien;
    private SidebarButton btnTaiKhoan;
    private SidebarButton btnKhuyenMai;
    private SidebarButton btnVoucher;
    private SidebarButton btnHoaDon;
    private SidebarButton btnLichDatSan;
    private SidebarButton btnDatSan;
    private SidebarButton btnKhachHang;
    private SidebarButton btnSan;
    private SidebarButton btnLoaiSan;
    private SidebarButton btnTongQuan;
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
