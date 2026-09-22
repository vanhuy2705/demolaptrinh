using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmQuanLyNhanVien
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
        errLoi = new ErrorProvider(components);
        pnlDau = new Panel();
        lblMoTaTrang = new Label();
        lblTieuDe = new Label();
        picBieuTuong = new IconBox();
        pnlNoiDung = new Panel();
        pnlTrai = new Panel();
        dgvNhanVien = new DataGridView();
        pnlThanhCongCu = new Panel();
        btnLamMoi = new Button();
        btnTim = new Button();
        txtTimKiem = new TextBox();
        pnlPhai = new Panel();
        pnlNut = new Panel();
        btnKhoaMo = new Button();
        btnPhanQuyen = new Button();
        btnHuy = new Button();
        btnLuu = new Button();
        btnXoa = new Button();
        btnSua = new Button();
        btnThem = new Button();
        lblNhanTrangThai = new Label();
        lblNhanMatKhau = new Label();
        lblNhanTenDangNhap = new Label();
        lblNhanNgayVaoLam = new Label();
        lblNhanChucVu = new Label();
        lblNhanDiaChi = new Label();
        lblNhanEmail = new Label();
        lblNhanDienThoai = new Label();
        lblNhanHoTen = new Label();
        lblMaNV = new Label();
        lblNhanMa = new Label();
        lblNhanVaiTro = new Label();
        cboVaiTro = new ComboBox();
        cboTrangThai = new ComboBox();
        dtpNgayVaoLam = new DateTimePicker();
        txtMatKhau = new TextBox();
        txtTenDangNhap = new TextBox();
        txtChucVu = new TextBox();
        txtDiaChi = new TextBox();
        txtEmail = new TextBox();
        txtSoDienThoai = new TextBox();
        txtHoTen = new TextBox();
        ((System.ComponentModel.ISupportInitialize)errLoi).BeginInit();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlTrai.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvNhanVien).BeginInit();
        pnlThanhCongCu.SuspendLayout();
        pnlPhai.SuspendLayout();
        pnlNut.SuspendLayout();
        SuspendLayout();

        errLoi.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errLoi.ContainerControl = this;

        pnlDau.BackColor = GiaoDien.ThanhBen;
        pnlDau.Controls.Add(lblMoTaTrang);
        pnlDau.Controls.Add(lblTieuDe);
        pnlDau.Controls.Add(picBieuTuong);
        pnlDau.Dock = DockStyle.Top;
        pnlDau.Location = new System.Drawing.Point(0, 0);
        pnlDau.Name = "pnlDau";
        pnlDau.Size = new System.Drawing.Size(1200, 78);
        pnlDau.TabIndex = 0;

        picBieuTuong.Location = new System.Drawing.Point(20, 16);
        picBieuTuong.MauBieuTuong = GiaoDien.Chinh;
        picBieuTuong.Name = "picBieuTuong";
        picBieuTuong.Size = new System.Drawing.Size(44, 44);
        picBieuTuong.TabIndex = 0;
        picBieuTuong.TenBieuTuong = "users";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(76, 14);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(220, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Quản lý nhân viên";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(360, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Hồ sơ nhân viên, tài khoản đăng nhập và phân quyền (không cho tự nâng quyền)";

        pnlNoiDung.Controls.Add(pnlTrai);
        pnlNoiDung.Controls.Add(pnlPhai);
        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(0, 78);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(16);
        pnlNoiDung.Size = new System.Drawing.Size(1200, 642);
        pnlNoiDung.TabIndex = 1;

        pnlTrai.BackColor = GiaoDien.BeMat;
        pnlTrai.Controls.Add(dgvNhanVien);
        pnlTrai.Controls.Add(pnlThanhCongCu);
        pnlTrai.Dock = DockStyle.Fill;
        pnlTrai.Location = new System.Drawing.Point(16, 16);
        pnlTrai.Name = "pnlTrai";
        pnlTrai.Size = new System.Drawing.Size(788, 610);
        pnlTrai.TabIndex = 0;

        pnlThanhCongCu.Controls.Add(btnLamMoi);
        pnlThanhCongCu.Controls.Add(btnTim);
        pnlThanhCongCu.Controls.Add(txtTimKiem);
        pnlThanhCongCu.Dock = DockStyle.Top;
        pnlThanhCongCu.Location = new System.Drawing.Point(0, 0);
        pnlThanhCongCu.Name = "pnlThanhCongCu";
        pnlThanhCongCu.Padding = new Padding(12, 10, 12, 10);
        pnlThanhCongCu.Size = new System.Drawing.Size(788, 58);
        pnlThanhCongCu.TabIndex = 0;

        txtTimKiem.BackColor = GiaoDien.ManHinhNen;
        txtTimKiem.BorderStyle = BorderStyle.FixedSingle;
        txtTimKiem.Font = GiaoDien.ChuThuong;
        txtTimKiem.Location = new System.Drawing.Point(12, 14);
        txtTimKiem.Name = "txtTimKiem";
        txtTimKiem.Size = new System.Drawing.Size(360, 25);
        txtTimKiem.TabIndex = 0;
        txtTimKiem.KeyDown += txtTimKiem_KeyDown;

        GiaoDien.DangNutChinh(btnTim);
        btnTim.Location = new System.Drawing.Point(384, 10);
        btnTim.Name = "btnTim";
        btnTim.Size = new System.Drawing.Size(110, 34);
        btnTim.TabIndex = 1;
        btnTim.Text = "Tìm kiếm";
        btnTim.UseVisualStyleBackColor = false;
        btnTim.Click += btnTim_Click;

        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Location = new System.Drawing.Point(504, 10);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(110, 34);
        btnLamMoi.TabIndex = 2;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        dgvNhanVien.Dock = DockStyle.Fill;
        dgvNhanVien.Location = new System.Drawing.Point(0, 58);
        dgvNhanVien.Name = "dgvNhanVien";
        dgvNhanVien.Size = new System.Drawing.Size(788, 552);
        dgvNhanVien.TabIndex = 1;
        dgvNhanVien.SelectionChanged += dgvNhanVien_SelectionChanged;

        pnlPhai.BackColor = GiaoDien.BeMat;
        pnlPhai.Controls.Add(pnlNut);
        pnlPhai.Controls.Add(lblNhanVaiTro);
        pnlPhai.Controls.Add(lblNhanTrangThai);
        pnlPhai.Controls.Add(lblNhanMatKhau);
        pnlPhai.Controls.Add(lblNhanTenDangNhap);
        pnlPhai.Controls.Add(lblNhanNgayVaoLam);
        pnlPhai.Controls.Add(lblNhanChucVu);
        pnlPhai.Controls.Add(lblNhanDiaChi);
        pnlPhai.Controls.Add(lblNhanEmail);
        pnlPhai.Controls.Add(lblNhanDienThoai);
        pnlPhai.Controls.Add(lblNhanHoTen);
        pnlPhai.Controls.Add(lblMaNV);
        pnlPhai.Controls.Add(lblNhanMa);
        pnlPhai.Controls.Add(cboVaiTro);
        pnlPhai.Controls.Add(cboTrangThai);
        pnlPhai.Controls.Add(dtpNgayVaoLam);
        pnlPhai.Controls.Add(txtMatKhau);
        pnlPhai.Controls.Add(txtTenDangNhap);
        pnlPhai.Controls.Add(txtChucVu);
        pnlPhai.Controls.Add(txtDiaChi);
        pnlPhai.Controls.Add(txtEmail);
        pnlPhai.Controls.Add(txtSoDienThoai);
        pnlPhai.Controls.Add(txtHoTen);
        pnlPhai.AutoScroll = true;
        pnlPhai.Dock = DockStyle.Right;
        pnlPhai.Location = new System.Drawing.Point(804, 16);
        pnlPhai.Name = "pnlPhai";
        pnlPhai.Padding = new Padding(20, 18, 20, 18);
        pnlPhai.Size = new System.Drawing.Size(380, 610);
        pnlPhai.TabIndex = 1;

        lblNhanMa.AutoSize = true;
        GiaoDien.DangNhan(lblNhanMa);
        lblNhanMa.Location = new System.Drawing.Point(20, 18);
        lblNhanMa.Name = "lblNhanMa";
        lblNhanMa.TabIndex = 0;
        lblNhanMa.Text = "Mã nhân viên";

        lblMaNV.AutoSize = true;
        lblMaNV.Font = GiaoDien.ChuDam;
        lblMaNV.ForeColor = GiaoDien.ChinhDam;
        lblMaNV.Location = new System.Drawing.Point(150, 17);
        lblMaNV.Name = "lblMaNV";
        lblMaNV.Size = new System.Drawing.Size(20, 17);
        lblMaNV.TabIndex = 1;
        lblMaNV.Text = "—";

        lblNhanHoTen.AutoSize = true;
        GiaoDien.DangNhan(lblNhanHoTen);
        lblNhanHoTen.Location = new System.Drawing.Point(20, 48);
        lblNhanHoTen.Name = "lblNhanHoTen";
        lblNhanHoTen.TabIndex = 2;
        lblNhanHoTen.Text = "Họ tên (*)";

        txtHoTen.BackColor = GiaoDien.ManHinhNen;
        txtHoTen.BorderStyle = BorderStyle.FixedSingle;
        txtHoTen.Font = GiaoDien.ChuThuong;
        txtHoTen.Location = new System.Drawing.Point(20, 66);
        txtHoTen.Name = "txtHoTen";
        txtHoTen.ReadOnly = true;
        txtHoTen.Size = new System.Drawing.Size(340, 25);
        txtHoTen.TabIndex = 0;

        lblNhanDienThoai.AutoSize = true;
        GiaoDien.DangNhan(lblNhanDienThoai);
        lblNhanDienThoai.Location = new System.Drawing.Point(20, 98);
        lblNhanDienThoai.Name = "lblNhanDienThoai";
        lblNhanDienThoai.TabIndex = 4;
        lblNhanDienThoai.Text = "Số điện thoại (*)";

        txtSoDienThoai.BackColor = GiaoDien.ManHinhNen;
        txtSoDienThoai.BorderStyle = BorderStyle.FixedSingle;
        txtSoDienThoai.Font = GiaoDien.ChuThuong;
        txtSoDienThoai.Location = new System.Drawing.Point(20, 116);
        txtSoDienThoai.Name = "txtSoDienThoai";
        txtSoDienThoai.ReadOnly = true;
        txtSoDienThoai.Size = new System.Drawing.Size(340, 25);
        txtSoDienThoai.TabIndex = 1;

        lblNhanEmail.AutoSize = true;
        GiaoDien.DangNhan(lblNhanEmail);
        lblNhanEmail.Location = new System.Drawing.Point(20, 148);
        lblNhanEmail.Name = "lblNhanEmail";
        lblNhanEmail.TabIndex = 6;
        lblNhanEmail.Text = "Email";

        txtEmail.BackColor = GiaoDien.ManHinhNen;
        txtEmail.BorderStyle = BorderStyle.FixedSingle;
        txtEmail.Font = GiaoDien.ChuThuong;
        txtEmail.Location = new System.Drawing.Point(20, 166);
        txtEmail.Name = "txtEmail";
        txtEmail.ReadOnly = true;
        txtEmail.Size = new System.Drawing.Size(340, 25);
        txtEmail.TabIndex = 2;

        lblNhanDiaChi.AutoSize = true;
        GiaoDien.DangNhan(lblNhanDiaChi);
        lblNhanDiaChi.Location = new System.Drawing.Point(20, 198);
        lblNhanDiaChi.Name = "lblNhanDiaChi";
        lblNhanDiaChi.TabIndex = 8;
        lblNhanDiaChi.Text = "Địa chỉ";

        txtDiaChi.BackColor = GiaoDien.ManHinhNen;
        txtDiaChi.BorderStyle = BorderStyle.FixedSingle;
        txtDiaChi.Font = GiaoDien.ChuThuong;
        txtDiaChi.Location = new System.Drawing.Point(20, 216);
        txtDiaChi.Name = "txtDiaChi";
        txtDiaChi.ReadOnly = true;
        txtDiaChi.Size = new System.Drawing.Size(340, 25);
        txtDiaChi.TabIndex = 3;

        lblNhanChucVu.AutoSize = true;
        GiaoDien.DangNhan(lblNhanChucVu);
        lblNhanChucVu.Location = new System.Drawing.Point(20, 248);
        lblNhanChucVu.Name = "lblNhanChucVu";
        lblNhanChucVu.TabIndex = 10;
        lblNhanChucVu.Text = "Chức vụ";

        txtChucVu.BackColor = GiaoDien.ManHinhNen;
        txtChucVu.BorderStyle = BorderStyle.FixedSingle;
        txtChucVu.Font = GiaoDien.ChuThuong;
        txtChucVu.Location = new System.Drawing.Point(20, 266);
        txtChucVu.Name = "txtChucVu";
        txtChucVu.ReadOnly = true;
        txtChucVu.Size = new System.Drawing.Size(340, 25);
        txtChucVu.TabIndex = 4;

        lblNhanNgayVaoLam.AutoSize = true;
        GiaoDien.DangNhan(lblNhanNgayVaoLam);
        lblNhanNgayVaoLam.Location = new System.Drawing.Point(20, 298);
        lblNhanNgayVaoLam.Name = "lblNhanNgayVaoLam";
        lblNhanNgayVaoLam.TabIndex = 12;
        lblNhanNgayVaoLam.Text = "Ngày vào làm";

        dtpNgayVaoLam.Font = GiaoDien.ChuThuong;
        dtpNgayVaoLam.Format = DateTimePickerFormat.Short;
        dtpNgayVaoLam.Location = new System.Drawing.Point(20, 316);
        dtpNgayVaoLam.Name = "dtpNgayVaoLam";
        dtpNgayVaoLam.Size = new System.Drawing.Size(340, 25);
        dtpNgayVaoLam.TabIndex = 5;

        lblNhanTenDangNhap.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTenDangNhap);
        lblNhanTenDangNhap.Location = new System.Drawing.Point(20, 348);
        lblNhanTenDangNhap.Name = "lblNhanTenDangNhap";
        lblNhanTenDangNhap.TabIndex = 14;
        lblNhanTenDangNhap.Text = "Tên đăng nhập (*)";

        txtTenDangNhap.BackColor = GiaoDien.ManHinhNen;
        txtTenDangNhap.BorderStyle = BorderStyle.FixedSingle;
        txtTenDangNhap.Font = GiaoDien.ChuThuong;
        txtTenDangNhap.Location = new System.Drawing.Point(20, 366);
        txtTenDangNhap.Name = "txtTenDangNhap";
        txtTenDangNhap.ReadOnly = true;
        txtTenDangNhap.Size = new System.Drawing.Size(340, 25);
        txtTenDangNhap.TabIndex = 6;

        lblNhanMatKhau.AutoSize = true;
        GiaoDien.DangNhan(lblNhanMatKhau);
        lblNhanMatKhau.Location = new System.Drawing.Point(20, 398);
        lblNhanMatKhau.Name = "lblNhanMatKhau";
        lblNhanMatKhau.TabIndex = 16;
        lblNhanMatKhau.Text = "Mật khẩu (*)";

        txtMatKhau.BackColor = GiaoDien.ManHinhNen;
        txtMatKhau.BorderStyle = BorderStyle.FixedSingle;
        txtMatKhau.Font = GiaoDien.ChuThuong;
        txtMatKhau.Location = new System.Drawing.Point(20, 416);
        txtMatKhau.Name = "txtMatKhau";
        txtMatKhau.ReadOnly = true;
        txtMatKhau.Size = new System.Drawing.Size(340, 25);
        txtMatKhau.TabIndex = 7;
        txtMatKhau.UseSystemPasswordChar = true;

        lblNhanVaiTro.AutoSize = true;
        GiaoDien.DangNhan(lblNhanVaiTro);
        lblNhanVaiTro.Location = new System.Drawing.Point(20, 448);
        lblNhanVaiTro.Name = "lblNhanVaiTro";
        lblNhanVaiTro.TabIndex = 18;
        lblNhanVaiTro.Text = "Quyền (khi thêm mới)";

        cboVaiTro.DropDownStyle = ComboBoxStyle.DropDownList;
        cboVaiTro.Font = GiaoDien.ChuThuong;
        cboVaiTro.Location = new System.Drawing.Point(20, 466);
        cboVaiTro.Name = "cboVaiTro";
        cboVaiTro.Size = new System.Drawing.Size(340, 25);
        cboVaiTro.TabIndex = 8;

        lblNhanTrangThai.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTrangThai);
        lblNhanTrangThai.Location = new System.Drawing.Point(20, 498);
        lblNhanTrangThai.Name = "lblNhanTrangThai";
        lblNhanTrangThai.TabIndex = 20;
        lblNhanTrangThai.Text = "Trạng thái";

        cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTrangThai.Font = GiaoDien.ChuThuong;
        cboTrangThai.Location = new System.Drawing.Point(20, 516);
        cboTrangThai.Name = "cboTrangThai";
        cboTrangThai.Size = new System.Drawing.Size(340, 25);
        cboTrangThai.TabIndex = 9;

        pnlNut.Controls.Add(btnKhoaMo);
        pnlNut.Controls.Add(btnPhanQuyen);
        pnlNut.Controls.Add(btnHuy);
        pnlNut.Controls.Add(btnLuu);
        pnlNut.Controls.Add(btnXoa);
        pnlNut.Controls.Add(btnSua);
        pnlNut.Controls.Add(btnThem);
        pnlNut.Dock = DockStyle.None;
        pnlNut.Location = new System.Drawing.Point(20, 560);
        pnlNut.Name = "pnlNut";
        pnlNut.Size = new System.Drawing.Size(340, 180);
        pnlNut.TabIndex = 21;

        GiaoDien.DangNutPhu(btnPhanQuyen);
        btnPhanQuyen.Location = new System.Drawing.Point(0, 0);
        btnPhanQuyen.Name = "btnPhanQuyen";
        btnPhanQuyen.Size = new System.Drawing.Size(160, 36);
        btnPhanQuyen.TabIndex = 10;
        btnPhanQuyen.Text = "Phân quyền";
        btnPhanQuyen.UseVisualStyleBackColor = false;
        btnPhanQuyen.Click += btnPhanQuyen_Click;

        GiaoDien.DangNutPhu(btnKhoaMo);
        btnKhoaMo.Location = new System.Drawing.Point(170, 0);
        btnKhoaMo.Name = "btnKhoaMo";
        btnKhoaMo.Size = new System.Drawing.Size(170, 36);
        btnKhoaMo.TabIndex = 11;
        btnKhoaMo.Text = "Khóa";
        btnKhoaMo.UseVisualStyleBackColor = false;
        btnKhoaMo.Click += btnKhoaMo_Click;

        GiaoDien.DangNutChinh(btnThem);
        btnThem.Location = new System.Drawing.Point(0, 42);
        btnThem.Name = "btnThem";
        btnThem.Size = new System.Drawing.Size(100, 38);
        btnThem.TabIndex = 12;
        btnThem.Text = "Thêm";
        btnThem.UseVisualStyleBackColor = false;
        btnThem.Click += btnThem_Click;

        GiaoDien.DangNutPhu(btnSua);
        btnSua.Location = new System.Drawing.Point(110, 42);
        btnSua.Name = "btnSua";
        btnSua.Size = new System.Drawing.Size(90, 38);
        btnSua.TabIndex = 13;
        btnSua.Text = "Sửa";
        btnSua.UseVisualStyleBackColor = false;
        btnSua.Click += btnSua_Click;

        GiaoDien.DangNutNguyHiem(btnXoa);
        btnXoa.Location = new System.Drawing.Point(210, 42);
        btnXoa.Name = "btnXoa";
        btnXoa.Size = new System.Drawing.Size(90, 38);
        btnXoa.TabIndex = 14;
        btnXoa.Text = "Xóa";
        btnXoa.UseVisualStyleBackColor = false;
        btnXoa.Click += btnXoa_Click;

        GiaoDien.DangNutChinh(btnLuu);
        btnLuu.Location = new System.Drawing.Point(0, 130);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new System.Drawing.Size(160, 38);
        btnLuu.TabIndex = 15;
        btnLuu.Text = "Lưu";
        btnLuu.UseVisualStyleBackColor = false;
        btnLuu.Click += btnLuu_Click;

        GiaoDien.DangNutPhu(btnHuy);
        btnHuy.Location = new System.Drawing.Point(170, 130);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new System.Drawing.Size(130, 38);
        btnHuy.TabIndex = 16;
        btnHuy.Text = "Hủy bỏ";
        btnHuy.UseVisualStyleBackColor = false;
        btnHuy.Click += btnHuy_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmQuanLyNhanVien";
        Text = "Quản lý nhân viên";
        ((System.ComponentModel.ISupportInitialize)errLoi).EndInit();
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlTrai.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvNhanVien).EndInit();
        pnlThanhCongCu.ResumeLayout(false);
        pnlPhai.ResumeLayout(false);
        pnlPhai.PerformLayout();
        pnlNut.ResumeLayout(false);
        ResumeLayout(false);
    }

    private ErrorProvider errLoi;
    private Panel pnlDau;
    private Label lblMoTaTrang;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Panel pnlNoiDung;
    private Panel pnlTrai;
    private DataGridView dgvNhanVien;
    private Panel pnlThanhCongCu;
    private Button btnLamMoi;
    private Button btnTim;
    private TextBox txtTimKiem;
    private Panel pnlPhai;
    private Panel pnlNut;
    private Button btnKhoaMo;
    private Button btnPhanQuyen;
    private Button btnHuy;
    private Button btnLuu;
    private Button btnXoa;
    private Button btnSua;
    private Button btnThem;
    private Label lblNhanVaiTro;
    private Label lblNhanTrangThai;
    private Label lblNhanMatKhau;
    private Label lblNhanTenDangNhap;
    private Label lblNhanNgayVaoLam;
    private Label lblNhanChucVu;
    private Label lblNhanDiaChi;
    private Label lblNhanEmail;
    private Label lblNhanDienThoai;
    private Label lblNhanHoTen;
    private Label lblMaNV;
    private Label lblNhanMa;
    private ComboBox cboVaiTro;
    private ComboBox cboTrangThai;
    private DateTimePicker dtpNgayVaoLam;
    private TextBox txtMatKhau;
    private TextBox txtTenDangNhap;
    private TextBox txtChucVu;
    private TextBox txtDiaChi;
    private TextBox txtEmail;
    private TextBox txtSoDienThoai;
    private TextBox txtHoTen;
}
