using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmDangKy
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
        lblTieuDe = new Label();
        picBieuTuong = new IconBox();
        pnlThan = new Panel();
        btnHuy = new Button();
        btnDangKy = new Button();
        chkHienMatKhau = new CheckBox();
        lblDiaChi = new Label();
        lblEmail = new Label();
        lblXacNhanMatKhau = new Label();
        lblMatKhau = new Label();
        lblTenDangNhap = new Label();
        lblSoDienThoai = new Label();
        lblHoTen = new Label();
        txtDiaChi = new TextBox();
        txtEmail = new TextBox();
        txtXacNhanMatKhau = new TextBox();
        txtMatKhau = new TextBox();
        txtTenDangNhap = new TextBox();
        txtSoDienThoai = new TextBox();
        txtHoTen = new TextBox();
        ((System.ComponentModel.ISupportInitialize)errLoi).BeginInit();
        pnlDau.SuspendLayout();
        pnlThan.SuspendLayout();
        SuspendLayout();

        // errLoi
        errLoi.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errLoi.ContainerControl = this;

        // pnlDau
        pnlDau.BackColor = GiaoDien.ThanhBen;
        pnlDau.Controls.Add(lblTieuDe);
        pnlDau.Controls.Add(picBieuTuong);
        pnlDau.Dock = DockStyle.Top;
        pnlDau.Location = new System.Drawing.Point(0, 0);
        pnlDau.Name = "pnlDau";
        pnlDau.Size = new System.Drawing.Size(560, 92);
        pnlDau.TabIndex = 0;

        // picBieuTuong
        picBieuTuong.Location = new System.Drawing.Point(28, 22);
        picBieuTuong.MauBieuTuong = GiaoDien.Chinh;
        picBieuTuong.Name = "picBieuTuong";
        picBieuTuong.Size = new System.Drawing.Size(48, 48);
        picBieuTuong.TabIndex = 1;
        picBieuTuong.TenBieuTuong = "user";

        // lblTieuDe
        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(92, 32);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(240, 31);
        lblTieuDe.TabIndex = 0;
        lblTieuDe.Text = "Tạo tài khoản khách hàng";

        // pnlThan
        pnlThan.BackColor = GiaoDien.BeMat;
        pnlThan.Controls.Add(btnHuy);
        pnlThan.Controls.Add(btnDangKy);
        pnlThan.Controls.Add(chkHienMatKhau);
        pnlThan.Controls.Add(lblDiaChi);
        pnlThan.Controls.Add(lblEmail);
        pnlThan.Controls.Add(lblXacNhanMatKhau);
        pnlThan.Controls.Add(lblMatKhau);
        pnlThan.Controls.Add(lblTenDangNhap);
        pnlThan.Controls.Add(lblSoDienThoai);
        pnlThan.Controls.Add(lblHoTen);
        pnlThan.Controls.Add(txtDiaChi);
        pnlThan.Controls.Add(txtEmail);
        pnlThan.Controls.Add(txtXacNhanMatKhau);
        pnlThan.Controls.Add(txtMatKhau);
        pnlThan.Controls.Add(txtTenDangNhap);
        pnlThan.Controls.Add(txtSoDienThoai);
        pnlThan.Controls.Add(txtHoTen);
        pnlThan.Dock = DockStyle.Fill;
        pnlThan.Location = new System.Drawing.Point(0, 92);
        pnlThan.Name = "pnlThan";
        pnlThan.Size = new System.Drawing.Size(560, 548);
        pnlThan.TabIndex = 1;

        // lblHoTen
        lblHoTen.AutoSize = true;
        GiaoDien.DangNhan(lblHoTen);
        lblHoTen.Location = new System.Drawing.Point(40, 28);
        lblHoTen.Name = "lblHoTen";
        lblHoTen.TabIndex = 0;
        lblHoTen.Text = "Họ và tên (*)";

        // txtHoTen
        txtHoTen.BackColor = GiaoDien.ManHinhNen;
        txtHoTen.BorderStyle = BorderStyle.FixedSingle;
        txtHoTen.Font = GiaoDien.ChuThuong;
        txtHoTen.Location = new System.Drawing.Point(40, 48);
        txtHoTen.Name = "txtHoTen";
        txtHoTen.Size = new System.Drawing.Size(480, 25);
        txtHoTen.TabIndex = 0;

        // lblSoDienThoai
        lblSoDienThoai.AutoSize = true;
        GiaoDien.DangNhan(lblSoDienThoai);
        lblSoDienThoai.Location = new System.Drawing.Point(40, 88);
        lblSoDienThoai.Name = "lblSoDienThoai";
        lblSoDienThoai.TabIndex = 2;
        lblSoDienThoai.Text = "Số điện thoại (*)";

        // txtSoDienThoai
        txtSoDienThoai.BackColor = GiaoDien.ManHinhNen;
        txtSoDienThoai.BorderStyle = BorderStyle.FixedSingle;
        txtSoDienThoai.Font = GiaoDien.ChuThuong;
        txtSoDienThoai.Location = new System.Drawing.Point(40, 108);
        txtSoDienThoai.Name = "txtSoDienThoai";
        txtSoDienThoai.Size = new System.Drawing.Size(230, 25);
        txtSoDienThoai.TabIndex = 1;

        // lblEmail
        lblEmail.AutoSize = true;
        GiaoDien.DangNhan(lblEmail);
        lblEmail.Location = new System.Drawing.Point(290, 88);
        lblEmail.Name = "lblEmail";
        lblEmail.TabIndex = 4;
        lblEmail.Text = "Email";

        // txtEmail
        txtEmail.BackColor = GiaoDien.ManHinhNen;
        txtEmail.BorderStyle = BorderStyle.FixedSingle;
        txtEmail.Font = GiaoDien.ChuThuong;
        txtEmail.Location = new System.Drawing.Point(290, 108);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new System.Drawing.Size(230, 25);
        txtEmail.TabIndex = 2;

        // lblDiaChi
        lblDiaChi.AutoSize = true;
        GiaoDien.DangNhan(lblDiaChi);
        lblDiaChi.Location = new System.Drawing.Point(40, 148);
        lblDiaChi.Name = "lblDiaChi";
        lblDiaChi.TabIndex = 6;
        lblDiaChi.Text = "Địa chỉ";

        // txtDiaChi
        txtDiaChi.BackColor = GiaoDien.ManHinhNen;
        txtDiaChi.BorderStyle = BorderStyle.FixedSingle;
        txtDiaChi.Font = GiaoDien.ChuThuong;
        txtDiaChi.Location = new System.Drawing.Point(40, 168);
        txtDiaChi.Name = "txtDiaChi";
        txtDiaChi.Size = new System.Drawing.Size(480, 25);
        txtDiaChi.TabIndex = 3;

        // lblTenDangNhap
        lblTenDangNhap.AutoSize = true;
        GiaoDien.DangNhan(lblTenDangNhap);
        lblTenDangNhap.Location = new System.Drawing.Point(40, 208);
        lblTenDangNhap.Name = "lblTenDangNhap";
        lblTenDangNhap.TabIndex = 8;
        lblTenDangNhap.Text = "Tên đăng nhập (*)";

        // txtTenDangNhap
        txtTenDangNhap.BackColor = GiaoDien.ManHinhNen;
        txtTenDangNhap.BorderStyle = BorderStyle.FixedSingle;
        txtTenDangNhap.Font = GiaoDien.ChuThuong;
        txtTenDangNhap.Location = new System.Drawing.Point(40, 228);
        txtTenDangNhap.Name = "txtTenDangNhap";
        txtTenDangNhap.Size = new System.Drawing.Size(480, 25);
        txtTenDangNhap.TabIndex = 4;

        // lblMatKhau
        lblMatKhau.AutoSize = true;
        GiaoDien.DangNhan(lblMatKhau);
        lblMatKhau.Location = new System.Drawing.Point(40, 268);
        lblMatKhau.Name = "lblMatKhau";
        lblMatKhau.TabIndex = 10;
        lblMatKhau.Text = "Mật khẩu (*) - ít nhất 6 ký tự";

        // txtMatKhau
        txtMatKhau.BackColor = GiaoDien.ManHinhNen;
        txtMatKhau.BorderStyle = BorderStyle.FixedSingle;
        txtMatKhau.Font = GiaoDien.ChuThuong;
        txtMatKhau.Location = new System.Drawing.Point(40, 288);
        txtMatKhau.Name = "txtMatKhau";
        txtMatKhau.Size = new System.Drawing.Size(230, 25);
        txtMatKhau.TabIndex = 5;
        txtMatKhau.UseSystemPasswordChar = true;

        // lblXacNhanMatKhau
        lblXacNhanMatKhau.AutoSize = true;
        GiaoDien.DangNhan(lblXacNhanMatKhau);
        lblXacNhanMatKhau.Location = new System.Drawing.Point(290, 268);
        lblXacNhanMatKhau.Name = "lblXacNhanMatKhau";
        lblXacNhanMatKhau.TabIndex = 12;
        lblXacNhanMatKhau.Text = "Xác nhận mật khẩu (*)";

        // txtXacNhanMatKhau
        txtXacNhanMatKhau.BackColor = GiaoDien.ManHinhNen;
        txtXacNhanMatKhau.BorderStyle = BorderStyle.FixedSingle;
        txtXacNhanMatKhau.Font = GiaoDien.ChuThuong;
        txtXacNhanMatKhau.Location = new System.Drawing.Point(290, 288);
        txtXacNhanMatKhau.Name = "txtXacNhanMatKhau";
        txtXacNhanMatKhau.Size = new System.Drawing.Size(230, 25);
        txtXacNhanMatKhau.TabIndex = 6;
        txtXacNhanMatKhau.UseSystemPasswordChar = true;

        // chkHienMatKhau
        chkHienMatKhau.AutoSize = true;
        chkHienMatKhau.Font = GiaoDien.ChuNho;
        chkHienMatKhau.ForeColor = GiaoDien.ChuPhu;
        chkHienMatKhau.Location = new System.Drawing.Point(40, 326);
        chkHienMatKhau.Name = "chkHienMatKhau";
        chkHienMatKhau.Size = new System.Drawing.Size(110, 19);
        chkHienMatKhau.TabIndex = 7;
        chkHienMatKhau.Text = "Hiển thị mật khẩu";
        chkHienMatKhau.UseVisualStyleBackColor = true;
        chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;

        // btnDangKy
        GiaoDien.DangNutChinh(btnDangKy);
        btnDangKy.Location = new System.Drawing.Point(40, 386);
        btnDangKy.Name = "btnDangKy";
        btnDangKy.Size = new System.Drawing.Size(230, 42);
        btnDangKy.TabIndex = 8;
        btnDangKy.Text = "Đăng ký";
        btnDangKy.UseVisualStyleBackColor = false;
        btnDangKy.Click += btnDangKy_Click;

        // btnHuy
        GiaoDien.DangNutPhu(btnHuy);
        btnHuy.Location = new System.Drawing.Point(290, 386);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new System.Drawing.Size(230, 42);
        btnHuy.TabIndex = 9;
        btnHuy.Text = "Hủy bỏ";
        btnHuy.UseVisualStyleBackColor = false;
        btnHuy.Click += btnHuy_Click;

        // frmDangKy
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(560, 640);
        Controls.Add(pnlThan);
        Controls.Add(pnlDau);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmDangKy";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Đăng ký tài khoản";
        Load += frmDangKy_Load;
        ((System.ComponentModel.ISupportInitialize)errLoi).EndInit();
        pnlDau.ResumeLayout(false);
        pnlThan.ResumeLayout(false);
        pnlThan.PerformLayout();
        ResumeLayout(false);
    }

    private ErrorProvider errLoi;
    private Panel pnlDau;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Panel pnlThan;
    private Button btnHuy;
    private Button btnDangKy;
    private CheckBox chkHienMatKhau;
    private Label lblDiaChi;
    private Label lblEmail;
    private Label lblXacNhanMatKhau;
    private Label lblMatKhau;
    private Label lblTenDangNhap;
    private Label lblSoDienThoai;
    private Label lblHoTen;
    private TextBox txtDiaChi;
    private TextBox txtEmail;
    private TextBox txtXacNhanMatKhau;
    private TextBox txtMatKhau;
    private TextBox txtTenDangNhap;
    private TextBox txtSoDienThoai;
    private TextBox txtHoTen;
}
