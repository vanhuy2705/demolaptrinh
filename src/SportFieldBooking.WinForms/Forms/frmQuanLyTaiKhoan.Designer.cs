using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmQuanLyTaiKhoan
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
        dgvTaiKhoan = new DataGridView();
        pnlThanhCongCu = new Panel();
        cboLocVaiTro = new ComboBox();
        btnLamMoi = new Button();
        btnTim = new Button();
        txtTimKiem = new TextBox();
        pnlPhai = new Panel();
        pnlNut = new Panel();
        btnDatLaiMatKhau = new Button();
        btnKhoaMo = new Button();
        btnHuy = new Button();
        btnLuu = new Button();
        btnXoa = new Button();
        btnSua = new Button();
        btnThem = new Button();
        lblGhiChuMatKhau = new Label();
        lblNhanTrangThai = new Label();
        lblNhanVaiTro = new Label();
        lblNhanHoTen = new Label();
        lblNhanMatKhau = new Label();
        lblNhanTenDangNhap = new Label();
        lblMaTK = new Label();
        lblNhanMa = new Label();
        cboTrangThai = new ComboBox();
        cboVaiTro = new ComboBox();
        txtHoTen = new TextBox();
        txtMatKhau = new TextBox();
        txtTenDangNhap = new TextBox();
        ((System.ComponentModel.ISupportInitialize)errLoi).BeginInit();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlTrai.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvTaiKhoan).BeginInit();
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
        lblTieuDe.Text = "Quản lý tài khoản";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(320, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Tạo tài khoản, phân quyền, khóa/mở khóa và đặt lại mật khẩu";

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
        pnlTrai.Controls.Add(dgvTaiKhoan);
        pnlTrai.Controls.Add(pnlThanhCongCu);
        pnlTrai.Dock = DockStyle.Fill;
        pnlTrai.Location = new System.Drawing.Point(16, 16);
        pnlTrai.Name = "pnlTrai";
        pnlTrai.Size = new System.Drawing.Size(788, 610);
        pnlTrai.TabIndex = 0;

        pnlThanhCongCu.Controls.Add(cboLocVaiTro);
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
        txtTimKiem.Size = new System.Drawing.Size(280, 25);
        txtTimKiem.TabIndex = 0;
        txtTimKiem.KeyDown += txtTimKiem_KeyDown;

        cboLocVaiTro.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLocVaiTro.Font = GiaoDien.ChuThuong;
        cboLocVaiTro.Location = new System.Drawing.Point(304, 13);
        cboLocVaiTro.Name = "cboLocVaiTro";
        cboLocVaiTro.Size = new System.Drawing.Size(150, 25);
        cboLocVaiTro.TabIndex = 1;
        cboLocVaiTro.SelectedIndexChanged += cboLocVaiTro_SelectedIndexChanged;

        GiaoDien.DangNutChinh(btnTim);
        btnTim.Location = new System.Drawing.Point(464, 10);
        btnTim.Name = "btnTim";
        btnTim.Size = new System.Drawing.Size(110, 34);
        btnTim.TabIndex = 2;
        btnTim.Text = "Tìm kiếm";
        btnTim.UseVisualStyleBackColor = false;
        btnTim.Click += btnTim_Click;

        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Location = new System.Drawing.Point(584, 10);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(110, 34);
        btnLamMoi.TabIndex = 3;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        dgvTaiKhoan.Dock = DockStyle.Fill;
        dgvTaiKhoan.Location = new System.Drawing.Point(0, 58);
        dgvTaiKhoan.Name = "dgvTaiKhoan";
        dgvTaiKhoan.Size = new System.Drawing.Size(788, 552);
        dgvTaiKhoan.TabIndex = 1;
        dgvTaiKhoan.SelectionChanged += dgvTaiKhoan_SelectionChanged;

        pnlPhai.BackColor = GiaoDien.BeMat;
        pnlPhai.Controls.Add(pnlNut);
        pnlPhai.Controls.Add(lblGhiChuMatKhau);
        pnlPhai.Controls.Add(lblNhanTrangThai);
        pnlPhai.Controls.Add(lblNhanVaiTro);
        pnlPhai.Controls.Add(lblNhanHoTen);
        pnlPhai.Controls.Add(lblNhanMatKhau);
        pnlPhai.Controls.Add(lblNhanTenDangNhap);
        pnlPhai.Controls.Add(lblMaTK);
        pnlPhai.Controls.Add(lblNhanMa);
        pnlPhai.Controls.Add(cboTrangThai);
        pnlPhai.Controls.Add(cboVaiTro);
        pnlPhai.Controls.Add(txtHoTen);
        pnlPhai.Controls.Add(txtMatKhau);
        pnlPhai.Controls.Add(txtTenDangNhap);
        pnlPhai.Dock = DockStyle.Right;
        pnlPhai.Location = new System.Drawing.Point(804, 16);
        pnlPhai.Name = "pnlPhai";
        pnlPhai.Padding = new Padding(20, 18, 20, 18);
        pnlPhai.Size = new System.Drawing.Size(380, 610);
        pnlPhai.TabIndex = 1;

        lblNhanMa.AutoSize = true;
        GiaoDien.DangNhan(lblNhanMa);
        lblNhanMa.Location = new System.Drawing.Point(20, 20);
        lblNhanMa.Name = "lblNhanMa";
        lblNhanMa.TabIndex = 0;
        lblNhanMa.Text = "Mã tài khoản";

        lblMaTK.AutoSize = true;
        lblMaTK.Font = GiaoDien.ChuDam;
        lblMaTK.ForeColor = GiaoDien.ChinhDam;
        lblMaTK.Location = new System.Drawing.Point(150, 19);
        lblMaTK.Name = "lblMaTK";
        lblMaTK.Size = new System.Drawing.Size(20, 17);
        lblMaTK.TabIndex = 1;
        lblMaTK.Text = "—";

        lblNhanTenDangNhap.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTenDangNhap);
        lblNhanTenDangNhap.Location = new System.Drawing.Point(20, 54);
        lblNhanTenDangNhap.Name = "lblNhanTenDangNhap";
        lblNhanTenDangNhap.TabIndex = 2;
        lblNhanTenDangNhap.Text = "Tên đăng nhập (*)";

        txtTenDangNhap.BackColor = GiaoDien.ManHinhNen;
        txtTenDangNhap.BorderStyle = BorderStyle.FixedSingle;
        txtTenDangNhap.Font = GiaoDien.ChuThuong;
        txtTenDangNhap.Location = new System.Drawing.Point(20, 74);
        txtTenDangNhap.Name = "txtTenDangNhap";
        txtTenDangNhap.ReadOnly = true;
        txtTenDangNhap.Size = new System.Drawing.Size(340, 25);
        txtTenDangNhap.TabIndex = 0;

        lblNhanMatKhau.AutoSize = true;
        GiaoDien.DangNhan(lblNhanMatKhau);
        lblNhanMatKhau.Location = new System.Drawing.Point(20, 108);
        lblNhanMatKhau.Name = "lblNhanMatKhau";
        lblNhanMatKhau.TabIndex = 4;
        lblNhanMatKhau.Text = "Mật khẩu (*) - ít nhất 6 ký tự";

        txtMatKhau.BackColor = GiaoDien.ManHinhNen;
        txtMatKhau.BorderStyle = BorderStyle.FixedSingle;
        txtMatKhau.Font = GiaoDien.ChuThuong;
        txtMatKhau.Location = new System.Drawing.Point(20, 128);
        txtMatKhau.Name = "txtMatKhau";
        txtMatKhau.ReadOnly = true;
        txtMatKhau.Size = new System.Drawing.Size(340, 25);
        txtMatKhau.TabIndex = 1;
        txtMatKhau.UseSystemPasswordChar = true;

        lblGhiChuMatKhau.AutoSize = true;
        lblGhiChuMatKhau.Font = GiaoDien.ChuNho;
        lblGhiChuMatKhau.ForeColor = GiaoDien.ChuPhu;
        lblGhiChuMatKhau.Location = new System.Drawing.Point(20, 156);
        lblGhiChuMatKhau.Name = "lblGhiChuMatKhau";
        lblGhiChuMatKhau.Size = new System.Drawing.Size(300, 15);
        lblGhiChuMatKhau.TabIndex = 6;
        lblGhiChuMatKhau.Text = "Để trống khi sửa nếu muốn giữ nguyên mật khẩu cũ.";

        lblNhanHoTen.AutoSize = true;
        GiaoDien.DangNhan(lblNhanHoTen);
        lblNhanHoTen.Location = new System.Drawing.Point(20, 182);
        lblNhanHoTen.Name = "lblNhanHoTen";
        lblNhanHoTen.TabIndex = 7;
        lblNhanHoTen.Text = "Họ tên (*)";

        txtHoTen.BackColor = GiaoDien.ManHinhNen;
        txtHoTen.BorderStyle = BorderStyle.FixedSingle;
        txtHoTen.Font = GiaoDien.ChuThuong;
        txtHoTen.Location = new System.Drawing.Point(20, 202);
        txtHoTen.Name = "txtHoTen";
        txtHoTen.ReadOnly = true;
        txtHoTen.Size = new System.Drawing.Size(340, 25);
        txtHoTen.TabIndex = 2;

        lblNhanVaiTro.AutoSize = true;
        GiaoDien.DangNhan(lblNhanVaiTro);
        lblNhanVaiTro.Location = new System.Drawing.Point(20, 236);
        lblNhanVaiTro.Name = "lblNhanVaiTro";
        lblNhanVaiTro.TabIndex = 9;
        lblNhanVaiTro.Text = "Vai trò";

        cboVaiTro.DropDownStyle = ComboBoxStyle.DropDownList;
        cboVaiTro.Font = GiaoDien.ChuThuong;
        cboVaiTro.Location = new System.Drawing.Point(20, 256);
        cboVaiTro.Name = "cboVaiTro";
        cboVaiTro.Size = new System.Drawing.Size(340, 25);
        cboVaiTro.TabIndex = 3;

        lblNhanTrangThai.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTrangThai);
        lblNhanTrangThai.Location = new System.Drawing.Point(20, 290);
        lblNhanTrangThai.Name = "lblNhanTrangThai";
        lblNhanTrangThai.TabIndex = 11;
        lblNhanTrangThai.Text = "Trạng thái";

        cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTrangThai.Font = GiaoDien.ChuThuong;
        cboTrangThai.Location = new System.Drawing.Point(20, 310);
        cboTrangThai.Name = "cboTrangThai";
        cboTrangThai.Size = new System.Drawing.Size(340, 25);
        cboTrangThai.TabIndex = 4;

        pnlNut.Controls.Add(btnDatLaiMatKhau);
        pnlNut.Controls.Add(btnKhoaMo);
        pnlNut.Controls.Add(btnHuy);
        pnlNut.Controls.Add(btnLuu);
        pnlNut.Controls.Add(btnXoa);
        pnlNut.Controls.Add(btnSua);
        pnlNut.Controls.Add(btnThem);
        pnlNut.Dock = DockStyle.Bottom;
        pnlNut.Location = new System.Drawing.Point(20, 402);
        pnlNut.Name = "pnlNut";
        pnlNut.Size = new System.Drawing.Size(340, 190);
        pnlNut.TabIndex = 12;

        GiaoDien.DangNutPhu(btnKhoaMo);
        btnKhoaMo.Location = new System.Drawing.Point(0, 0);
        btnKhoaMo.Name = "btnKhoaMo";
        btnKhoaMo.Size = new System.Drawing.Size(160, 36);
        btnKhoaMo.TabIndex = 5;
        btnKhoaMo.Text = "Khóa TK";
        btnKhoaMo.UseVisualStyleBackColor = false;
        btnKhoaMo.Click += btnKhoaMo_Click;

        GiaoDien.DangNutPhu(btnDatLaiMatKhau);
        btnDatLaiMatKhau.Location = new System.Drawing.Point(170, 0);
        btnDatLaiMatKhau.Name = "btnDatLaiMatKhau";
        btnDatLaiMatKhau.Size = new System.Drawing.Size(170, 36);
        btnDatLaiMatKhau.TabIndex = 6;
        btnDatLaiMatKhau.Text = "Đặt lại mật khẩu";
        btnDatLaiMatKhau.UseVisualStyleBackColor = false;
        btnDatLaiMatKhau.Click += btnDatLaiMatKhau_Click;

        GiaoDien.DangNutChinh(btnThem);
        btnThem.Location = new System.Drawing.Point(0, 50);
        btnThem.Name = "btnThem";
        btnThem.Size = new System.Drawing.Size(100, 38);
        btnThem.TabIndex = 7;
        btnThem.Text = "Thêm";
        btnThem.UseVisualStyleBackColor = false;
        btnThem.Click += btnThem_Click;

        GiaoDien.DangNutPhu(btnSua);
        btnSua.Location = new System.Drawing.Point(110, 50);
        btnSua.Name = "btnSua";
        btnSua.Size = new System.Drawing.Size(90, 38);
        btnSua.TabIndex = 8;
        btnSua.Text = "Sửa";
        btnSua.UseVisualStyleBackColor = false;
        btnSua.Click += btnSua_Click;

        GiaoDien.DangNutNguyHiem(btnXoa);
        btnXoa.Location = new System.Drawing.Point(210, 50);
        btnXoa.Name = "btnXoa";
        btnXoa.Size = new System.Drawing.Size(90, 38);
        btnXoa.TabIndex = 9;
        btnXoa.Text = "Xóa";
        btnXoa.UseVisualStyleBackColor = false;
        btnXoa.Click += btnXoa_Click;

        GiaoDien.DangNutChinh(btnLuu);
        btnLuu.Location = new System.Drawing.Point(0, 140);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new System.Drawing.Size(160, 38);
        btnLuu.TabIndex = 10;
        btnLuu.Text = "Lưu";
        btnLuu.UseVisualStyleBackColor = false;
        btnLuu.Click += btnLuu_Click;

        GiaoDien.DangNutPhu(btnHuy);
        btnHuy.Location = new System.Drawing.Point(170, 140);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new System.Drawing.Size(130, 38);
        btnHuy.TabIndex = 11;
        btnHuy.Text = "Hủy bỏ";
        btnHuy.UseVisualStyleBackColor = false;
        btnHuy.Click += btnHuy_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmQuanLyTaiKhoan";
        Text = "Quản lý tài khoản";
        ((System.ComponentModel.ISupportInitialize)errLoi).EndInit();
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlTrai.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvTaiKhoan).EndInit();
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
    private DataGridView dgvTaiKhoan;
    private Panel pnlThanhCongCu;
    private ComboBox cboLocVaiTro;
    private Button btnLamMoi;
    private Button btnTim;
    private TextBox txtTimKiem;
    private Panel pnlPhai;
    private Panel pnlNut;
    private Button btnDatLaiMatKhau;
    private Button btnKhoaMo;
    private Button btnHuy;
    private Button btnLuu;
    private Button btnXoa;
    private Button btnSua;
    private Button btnThem;
    private Label lblGhiChuMatKhau;
    private Label lblNhanTrangThai;
    private Label lblNhanVaiTro;
    private Label lblNhanHoTen;
    private Label lblNhanMatKhau;
    private Label lblNhanTenDangNhap;
    private Label lblMaTK;
    private Label lblNhanMa;
    private ComboBox cboTrangThai;
    private ComboBox cboVaiTro;
    private TextBox txtHoTen;
    private TextBox txtMatKhau;
    private TextBox txtTenDangNhap;
}
