using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmDangNhap
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
        pnlTrai = new Panel();
        lblPhienBan = new Label();
        lblMoTa = new Label();
        lblTenUngDung = new Label();
        picLogo = new IconBox();
        pnlPhai = new Panel();
        pnlKhungNhap = new Panel();
        chkHienMatKhau = new CheckBox();
        btnDangKy = new Button();
        btnDangNhap = new Button();
        lblMatKhau = new Label();
        lblTenDangNhap = new Label();
        txtMatKhau = new TextBox();
        txtTenDangNhap = new TextBox();
        lblTieuDeDangNhap = new Label();
        lblChaoMung = new Label();
        pnlTrai.SuspendLayout();
        pnlPhai.SuspendLayout();
        pnlKhungNhap.SuspendLayout();
        SuspendLayout();

        // pnlTrai
        pnlTrai.BackColor = GiaoDien.ThanhBen;
        pnlTrai.Controls.Add(lblPhienBan);
        pnlTrai.Controls.Add(lblMoTa);
        pnlTrai.Controls.Add(lblTenUngDung);
        pnlTrai.Controls.Add(picLogo);
        pnlTrai.Dock = DockStyle.Left;
        pnlTrai.Location = new System.Drawing.Point(0, 0);
        pnlTrai.Name = "pnlTrai";
        pnlTrai.Size = new System.Drawing.Size(420, 560);
        pnlTrai.TabIndex = 0;

        // picLogo
        picLogo.Location = new System.Drawing.Point(48, 150);
        picLogo.MauBieuTuong = GiaoDien.Chinh;
        picLogo.Name = "picLogo";
        picLogo.Size = new System.Drawing.Size(72, 72);
        picLogo.TabIndex = 0;
        picLogo.TenBieuTuong = "ball";

        // lblTenUngDung
        lblTenUngDung.AutoSize = true;
        lblTenUngDung.Font = GiaoDien.TieuDeLon;
        lblTenUngDung.ForeColor = Color.White;
        lblTenUngDung.Location = new System.Drawing.Point(44, 236);
        lblTenUngDung.Name = "lblTenUngDung";
        lblTenUngDung.Size = new System.Drawing.Size(300, 41);
        lblTenUngDung.TabIndex = 1;
        lblTenUngDung.Text = "SÂN THỂ THAO PRO";

        // lblMoTa
        lblMoTa.Font = GiaoDien.ChuThuong;
        lblMoTa.ForeColor = GiaoDien.ChuTrenNenDam;
        lblMoTa.Location = new System.Drawing.Point(44, 290);
        lblMoTa.Name = "lblMoTa";
        lblMoTa.Size = new System.Drawing.Size(330, 60);
        lblMoTa.TabIndex = 2;
        lblMoTa.Text = "Hệ thống quản lý cho thuê sân thể thao: đặt sân, lịch sân, hóa đơn, voucher và báo cáo.";

        // lblPhienBan
        lblPhienBan.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblPhienBan.AutoSize = true;
        lblPhienBan.Font = GiaoDien.ChuNho;
        lblPhienBan.ForeColor = Color.FromArgb(148, 163, 184);
        lblPhienBan.Location = new System.Drawing.Point(48, 510);
        lblPhienBan.Name = "lblPhienBan";
        lblPhienBan.Size = new System.Drawing.Size(160, 15);
        lblPhienBan.TabIndex = 3;
        lblPhienBan.Text = "Phiên bản 1.0";

        // pnlPhai
        pnlPhai.BackColor = GiaoDien.ManHinhNen;
        pnlPhai.Controls.Add(pnlKhungNhap);
        pnlPhai.Dock = DockStyle.Fill;
        pnlPhai.Location = new System.Drawing.Point(420, 0);
        pnlPhai.Name = "pnlPhai";
        pnlPhai.Size = new System.Drawing.Size(520, 560);
        pnlPhai.TabIndex = 1;

        // pnlKhungNhap
        pnlKhungNhap.Anchor = AnchorStyles.None;
        pnlKhungNhap.BackColor = GiaoDien.BeMat;
        pnlKhungNhap.Controls.Add(lblChaoMung);
        pnlKhungNhap.Controls.Add(lblTieuDeDangNhap);
        pnlKhungNhap.Controls.Add(chkHienMatKhau);
        pnlKhungNhap.Controls.Add(btnDangKy);
        pnlKhungNhap.Controls.Add(btnDangNhap);
        pnlKhungNhap.Controls.Add(lblMatKhau);
        pnlKhungNhap.Controls.Add(lblTenDangNhap);
        pnlKhungNhap.Controls.Add(txtMatKhau);
        pnlKhungNhap.Controls.Add(txtTenDangNhap);
        pnlKhungNhap.Location = new System.Drawing.Point(60, 90);
        pnlKhungNhap.Name = "pnlKhungNhap";
        pnlKhungNhap.Size = new System.Drawing.Size(400, 380);
        pnlKhungNhap.TabIndex = 0;

        // lblChaoMung
        lblChaoMung.AutoSize = true;
        lblChaoMung.Font = GiaoDien.ChuNho;
        lblChaoMung.ForeColor = GiaoDien.ChuPhu;
        lblChaoMung.Location = new System.Drawing.Point(40, 40);
        lblChaoMung.Name = "lblChaoMung";
        lblChaoMung.Size = new System.Drawing.Size(180, 15);
        lblChaoMung.TabIndex = 0;
        lblChaoMung.Text = "Chào mừng bạn quay trở lại";

        // lblTieuDeDangNhap
        lblTieuDeDangNhap.AutoSize = true;
        lblTieuDeDangNhap.Font = GiaoDien.TieuDe;
        lblTieuDeDangNhap.ForeColor = GiaoDien.Chu;
        lblTieuDeDangNhap.Location = new System.Drawing.Point(38, 60);
        lblTieuDeDangNhap.Name = "lblTieuDeDangNhap";
        lblTieuDeDangNhap.Size = new System.Drawing.Size(160, 31);
        lblTieuDeDangNhap.TabIndex = 1;
        lblTieuDeDangNhap.Text = "Đăng nhập";

        // lblTenDangNhap
        lblTenDangNhap.AutoSize = true;
        GiaoDien.DangNhan(lblTenDangNhap);
        lblTenDangNhap.Location = new System.Drawing.Point(40, 118);
        lblTenDangNhap.Name = "lblTenDangNhap";
        lblTenDangNhap.Size = new System.Drawing.Size(90, 15);
        lblTenDangNhap.TabIndex = 2;
        lblTenDangNhap.Text = "Tên đăng nhập";

        // txtTenDangNhap
        txtTenDangNhap.BackColor = GiaoDien.ManHinhNen;
        txtTenDangNhap.BorderStyle = BorderStyle.FixedSingle;
        txtTenDangNhap.Font = GiaoDien.ChuThuong;
        txtTenDangNhap.Location = new System.Drawing.Point(40, 138);
        txtTenDangNhap.Name = "txtTenDangNhap";
        txtTenDangNhap.Size = new System.Drawing.Size(320, 25);
        txtTenDangNhap.TabIndex = 0;
        txtTenDangNhap.KeyDown += txtTenDangNhap_KeyDown;

        // lblMatKhau
        lblMatKhau.AutoSize = true;
        GiaoDien.DangNhan(lblMatKhau);
        lblMatKhau.Location = new System.Drawing.Point(40, 178);
        lblMatKhau.Name = "lblMatKhau";
        lblMatKhau.Size = new System.Drawing.Size(60, 15);
        lblMatKhau.TabIndex = 4;
        lblMatKhau.Text = "Mật khẩu";

        // txtMatKhau
        txtMatKhau.BackColor = GiaoDien.ManHinhNen;
        txtMatKhau.BorderStyle = BorderStyle.FixedSingle;
        txtMatKhau.Font = GiaoDien.ChuThuong;
        txtMatKhau.Location = new System.Drawing.Point(40, 198);
        txtMatKhau.Name = "txtMatKhau";
        txtMatKhau.Size = new System.Drawing.Size(320, 25);
        txtMatKhau.TabIndex = 1;
        txtMatKhau.UseSystemPasswordChar = true;
        txtMatKhau.KeyDown += txtMatKhau_KeyDown;

        // chkHienMatKhau
        chkHienMatKhau.AutoSize = true;
        chkHienMatKhau.Font = GiaoDien.ChuNho;
        chkHienMatKhau.ForeColor = GiaoDien.ChuPhu;
        chkHienMatKhau.Location = new System.Drawing.Point(40, 232);
        chkHienMatKhau.Name = "chkHienMatKhau";
        chkHienMatKhau.Size = new System.Drawing.Size(110, 19);
        chkHienMatKhau.TabIndex = 2;
        chkHienMatKhau.Text = "Hiển thị mật khẩu";
        chkHienMatKhau.UseVisualStyleBackColor = true;
        chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;

        // btnDangNhap
        GiaoDien.DangNutChinh(btnDangNhap);
        btnDangNhap.Location = new System.Drawing.Point(40, 272);
        btnDangNhap.Name = "btnDangNhap";
        btnDangNhap.Size = new System.Drawing.Size(320, 42);
        btnDangNhap.TabIndex = 3;
        btnDangNhap.Text = "Đăng nhập";
        btnDangNhap.UseVisualStyleBackColor = false;
        btnDangNhap.Click += btnDangNhap_Click;

        // btnDangKy
        GiaoDien.DangNutPhu(btnDangKy);
        btnDangKy.Location = new System.Drawing.Point(40, 322);
        btnDangKy.Name = "btnDangKy";
        btnDangKy.Size = new System.Drawing.Size(320, 38);
        btnDangKy.TabIndex = 4;
        btnDangKy.Text = "Tạo tài khoản khách hàng";
        btnDangKy.UseVisualStyleBackColor = false;
        btnDangKy.Click += btnDangKy_Click;

        // frmDangNhap
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(940, 560);
        Controls.Add(pnlPhai);
        Controls.Add(pnlTrai);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmDangNhap";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Đăng nhập - Quản lý sân thể thao";
        Load += frmDangNhap_Load;
        pnlTrai.ResumeLayout(false);
        pnlPhai.ResumeLayout(false);
        pnlKhungNhap.ResumeLayout(false);
        pnlKhungNhap.PerformLayout();
        ResumeLayout(false);
    }

    private Panel pnlTrai;
    private Label lblPhienBan;
    private Label lblMoTa;
    private Label lblTenUngDung;
    private IconBox picLogo;
    private Panel pnlPhai;
    private Panel pnlKhungNhap;
    private CheckBox chkHienMatKhau;
    private Button btnDangKy;
    private Button btnDangNhap;
    private Label lblMatKhau;
    private Label lblTenDangNhap;
    private TextBox txtMatKhau;
    private TextBox txtTenDangNhap;
    private Label lblTieuDeDangNhap;
    private Label lblChaoMung;
}
