using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmDoiMatKhau
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
        chkHienMatKhau = new CheckBox();
        btnHuy = new Button();
        btnXacNhan = new Button();
        lblXacNhan = new Label();
        lblMatKhauMoi = new Label();
        lblMatKhauCu = new Label();
        lblNguoiDung = new Label();
        txtXacNhan = new TextBox();
        txtMatKhauMoi = new TextBox();
        txtMatKhauCu = new TextBox();
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
        pnlDau.Size = new System.Drawing.Size(460, 88);
        pnlDau.TabIndex = 0;

        // picBieuTuong
        picBieuTuong.Location = new System.Drawing.Point(24, 20);
        picBieuTuong.MauBieuTuong = GiaoDien.Chinh;
        picBieuTuong.Name = "picBieuTuong";
        picBieuTuong.Size = new System.Drawing.Size(46, 46);
        picBieuTuong.TabIndex = 1;
        picBieuTuong.TenBieuTuong = "key";

        // lblTieuDe
        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(84, 28);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(170, 31);
        lblTieuDe.TabIndex = 0;
        lblTieuDe.Text = "Đổi mật khẩu";

        // pnlThan
        pnlThan.BackColor = GiaoDien.BeMat;
        pnlThan.Controls.Add(chkHienMatKhau);
        pnlThan.Controls.Add(btnHuy);
        pnlThan.Controls.Add(btnXacNhan);
        pnlThan.Controls.Add(lblXacNhan);
        pnlThan.Controls.Add(lblMatKhauMoi);
        pnlThan.Controls.Add(lblMatKhauCu);
        pnlThan.Controls.Add(lblNguoiDung);
        pnlThan.Controls.Add(txtXacNhan);
        pnlThan.Controls.Add(txtMatKhauMoi);
        pnlThan.Controls.Add(txtMatKhauCu);
        pnlThan.Dock = DockStyle.Fill;
        pnlThan.Location = new System.Drawing.Point(0, 88);
        pnlThan.Name = "pnlThan";
        pnlThan.Size = new System.Drawing.Size(460, 272);
        pnlThan.TabIndex = 1;

        // lblNguoiDung
        lblNguoiDung.AutoSize = true;
        lblNguoiDung.Font = GiaoDien.ChuDam;
        lblNguoiDung.ForeColor = GiaoDien.ChuPhu;
        lblNguoiDung.Location = new System.Drawing.Point(36, 24);
        lblNguoiDung.Name = "lblNguoiDung";
        lblNguoiDung.Size = new System.Drawing.Size(120, 17);
        lblNguoiDung.TabIndex = 0;
        lblNguoiDung.Text = "Người dùng";

        // lblMatKhauCu
        lblMatKhauCu.AutoSize = true;
        GiaoDien.DangNhan(lblMatKhauCu);
        lblMatKhauCu.Location = new System.Drawing.Point(36, 62);
        lblMatKhauCu.Name = "lblMatKhauCu";
        lblMatKhauCu.TabIndex = 1;
        lblMatKhauCu.Text = "Mật khẩu hiện tại (*)";

        // txtMatKhauCu
        txtMatKhauCu.BackColor = GiaoDien.ManHinhNen;
        txtMatKhauCu.BorderStyle = BorderStyle.FixedSingle;
        txtMatKhauCu.Font = GiaoDien.ChuThuong;
        txtMatKhauCu.Location = new System.Drawing.Point(36, 82);
        txtMatKhauCu.Name = "txtMatKhauCu";
        txtMatKhauCu.Size = new System.Drawing.Size(388, 25);
        txtMatKhauCu.TabIndex = 0;
        txtMatKhauCu.UseSystemPasswordChar = true;

        // lblMatKhauMoi
        lblMatKhauMoi.AutoSize = true;
        GiaoDien.DangNhan(lblMatKhauMoi);
        lblMatKhauMoi.Location = new System.Drawing.Point(36, 120);
        lblMatKhauMoi.Name = "lblMatKhauMoi";
        lblMatKhauMoi.TabIndex = 3;
        lblMatKhauMoi.Text = "Mật khẩu mới (*) - ít nhất 6 ký tự";

        // txtMatKhauMoi
        txtMatKhauMoi.BackColor = GiaoDien.ManHinhNen;
        txtMatKhauMoi.BorderStyle = BorderStyle.FixedSingle;
        txtMatKhauMoi.Font = GiaoDien.ChuThuong;
        txtMatKhauMoi.Location = new System.Drawing.Point(36, 140);
        txtMatKhauMoi.Name = "txtMatKhauMoi";
        txtMatKhauMoi.Size = new System.Drawing.Size(388, 25);
        txtMatKhauMoi.TabIndex = 1;
        txtMatKhauMoi.UseSystemPasswordChar = true;

        // lblXacNhan
        lblXacNhan.AutoSize = true;
        GiaoDien.DangNhan(lblXacNhan);
        lblXacNhan.Location = new System.Drawing.Point(36, 178);
        lblXacNhan.Name = "lblXacNhan";
        lblXacNhan.TabIndex = 5;
        lblXacNhan.Text = "Xác nhận mật khẩu mới (*)";

        // txtXacNhan
        txtXacNhan.BackColor = GiaoDien.ManHinhNen;
        txtXacNhan.BorderStyle = BorderStyle.FixedSingle;
        txtXacNhan.Font = GiaoDien.ChuThuong;
        txtXacNhan.Location = new System.Drawing.Point(36, 198);
        txtXacNhan.Name = "txtXacNhan";
        txtXacNhan.Size = new System.Drawing.Size(388, 25);
        txtXacNhan.TabIndex = 2;
        txtXacNhan.UseSystemPasswordChar = true;

        // chkHienMatKhau
        chkHienMatKhau.AutoSize = true;
        chkHienMatKhau.Font = GiaoDien.ChuNho;
        chkHienMatKhau.ForeColor = GiaoDien.ChuPhu;
        chkHienMatKhau.Location = new System.Drawing.Point(36, 232);
        chkHienMatKhau.Name = "chkHienMatKhau";
        chkHienMatKhau.Size = new System.Drawing.Size(110, 19);
        chkHienMatKhau.TabIndex = 3;
        chkHienMatKhau.Text = "Hiển thị mật khẩu";
        chkHienMatKhau.UseVisualStyleBackColor = true;
        chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;

        // btnXacNhan
        GiaoDien.DangNutChinh(btnXacNhan);
        btnXacNhan.Location = new System.Drawing.Point(160, 218);
        btnXacNhan.Name = "btnXacNhan";
        btnXacNhan.Size = new System.Drawing.Size(130, 38);
        btnXacNhan.TabIndex = 4;
        btnXacNhan.Text = "Xác nhận";
        btnXacNhan.UseVisualStyleBackColor = false;
        btnXacNhan.Click += btnXacNhan_Click;

        // btnHuy
        GiaoDien.DangNutPhu(btnHuy);
        btnHuy.Location = new System.Drawing.Point(300, 218);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new System.Drawing.Size(124, 38);
        btnHuy.TabIndex = 5;
        btnHuy.Text = "Hủy bỏ";
        btnHuy.UseVisualStyleBackColor = false;
        btnHuy.Click += btnHuy_Click;

        // frmDoiMatKhau
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(460, 360);
        Controls.Add(pnlThan);
        Controls.Add(pnlDau);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmDoiMatKhau";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Đổi mật khẩu";
        Load += frmDoiMatKhau_Load;
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
    private CheckBox chkHienMatKhau;
    private Button btnHuy;
    private Button btnXacNhan;
    private Label lblXacNhan;
    private Label lblMatKhauMoi;
    private Label lblMatKhauCu;
    private Label lblNguoiDung;
    private TextBox txtXacNhan;
    private TextBox txtMatKhauMoi;
    private TextBox txtMatKhauCu;
}
