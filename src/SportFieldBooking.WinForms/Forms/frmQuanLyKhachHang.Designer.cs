using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmQuanLyKhachHang
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
        dgvKhachHang = new DataGridView();
        pnlThanhCongCu = new Panel();
        btnLamMoi = new Button();
        btnTim = new Button();
        txtTimKiem = new TextBox();
        pnlPhai = new Panel();
        pnlNut = new Panel();
        btnHuy = new Button();
        btnLuu = new Button();
        btnXoa = new Button();
        btnSua = new Button();
        btnThem = new Button();
        lblNhanDiaChi = new Label();
        lblNhanEmail = new Label();
        lblNhanDienThoai = new Label();
        lblNhanHoTen = new Label();
        lblMaKH = new Label();
        lblNhanMa = new Label();
        txtDiaChi = new TextBox();
        txtEmail = new TextBox();
        txtSoDienThoai = new TextBox();
        txtHoTen = new TextBox();
        ((System.ComponentModel.ISupportInitialize)errLoi).BeginInit();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlTrai.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvKhachHang).BeginInit();
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
        picBieuTuong.TenBieuTuong = "user";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(76, 14);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(220, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Quản lý khách hàng";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(360, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Tìm kiếm theo tên/số điện thoại, mỗi khách hàng có một số điện thoại duy nhất";

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
        pnlTrai.Controls.Add(dgvKhachHang);
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

        dgvKhachHang.Dock = DockStyle.Fill;
        dgvKhachHang.Location = new System.Drawing.Point(0, 58);
        dgvKhachHang.Name = "dgvKhachHang";
        dgvKhachHang.Size = new System.Drawing.Size(788, 552);
        dgvKhachHang.TabIndex = 1;
        dgvKhachHang.SelectionChanged += dgvKhachHang_SelectionChanged;

        pnlPhai.BackColor = GiaoDien.BeMat;
        pnlPhai.Controls.Add(pnlNut);
        pnlPhai.Controls.Add(lblNhanDiaChi);
        pnlPhai.Controls.Add(lblNhanEmail);
        pnlPhai.Controls.Add(lblNhanDienThoai);
        pnlPhai.Controls.Add(lblNhanHoTen);
        pnlPhai.Controls.Add(lblMaKH);
        pnlPhai.Controls.Add(lblNhanMa);
        pnlPhai.Controls.Add(txtDiaChi);
        pnlPhai.Controls.Add(txtEmail);
        pnlPhai.Controls.Add(txtSoDienThoai);
        pnlPhai.Controls.Add(txtHoTen);
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
        lblNhanMa.Text = "Mã khách hàng";

        lblMaKH.AutoSize = true;
        lblMaKH.Font = GiaoDien.ChuDam;
        lblMaKH.ForeColor = GiaoDien.ChinhDam;
        lblMaKH.Location = new System.Drawing.Point(150, 19);
        lblMaKH.Name = "lblMaKH";
        lblMaKH.Size = new System.Drawing.Size(20, 17);
        lblMaKH.TabIndex = 1;
        lblMaKH.Text = "—";

        lblNhanHoTen.AutoSize = true;
        GiaoDien.DangNhan(lblNhanHoTen);
        lblNhanHoTen.Location = new System.Drawing.Point(20, 58);
        lblNhanHoTen.Name = "lblNhanHoTen";
        lblNhanHoTen.TabIndex = 2;
        lblNhanHoTen.Text = "Họ tên (*)";

        txtHoTen.BackColor = GiaoDien.ManHinhNen;
        txtHoTen.BorderStyle = BorderStyle.FixedSingle;
        txtHoTen.Font = GiaoDien.ChuThuong;
        txtHoTen.Location = new System.Drawing.Point(20, 78);
        txtHoTen.Name = "txtHoTen";
        txtHoTen.ReadOnly = true;
        txtHoTen.Size = new System.Drawing.Size(340, 25);
        txtHoTen.TabIndex = 0;

        lblNhanDienThoai.AutoSize = true;
        GiaoDien.DangNhan(lblNhanDienThoai);
        lblNhanDienThoai.Location = new System.Drawing.Point(20, 116);
        lblNhanDienThoai.Name = "lblNhanDienThoai";
        lblNhanDienThoai.TabIndex = 4;
        lblNhanDienThoai.Text = "Số điện thoại (*) - duy nhất";

        txtSoDienThoai.BackColor = GiaoDien.ManHinhNen;
        txtSoDienThoai.BorderStyle = BorderStyle.FixedSingle;
        txtSoDienThoai.Font = GiaoDien.ChuThuong;
        txtSoDienThoai.Location = new System.Drawing.Point(20, 136);
        txtSoDienThoai.Name = "txtSoDienThoai";
        txtSoDienThoai.ReadOnly = true;
        txtSoDienThoai.Size = new System.Drawing.Size(340, 25);
        txtSoDienThoai.TabIndex = 1;

        lblNhanEmail.AutoSize = true;
        GiaoDien.DangNhan(lblNhanEmail);
        lblNhanEmail.Location = new System.Drawing.Point(20, 174);
        lblNhanEmail.Name = "lblNhanEmail";
        lblNhanEmail.TabIndex = 6;
        lblNhanEmail.Text = "Email";

        txtEmail.BackColor = GiaoDien.ManHinhNen;
        txtEmail.BorderStyle = BorderStyle.FixedSingle;
        txtEmail.Font = GiaoDien.ChuThuong;
        txtEmail.Location = new System.Drawing.Point(20, 194);
        txtEmail.Name = "txtEmail";
        txtEmail.ReadOnly = true;
        txtEmail.Size = new System.Drawing.Size(340, 25);
        txtEmail.TabIndex = 2;

        lblNhanDiaChi.AutoSize = true;
        GiaoDien.DangNhan(lblNhanDiaChi);
        lblNhanDiaChi.Location = new System.Drawing.Point(20, 232);
        lblNhanDiaChi.Name = "lblNhanDiaChi";
        lblNhanDiaChi.TabIndex = 8;
        lblNhanDiaChi.Text = "Địa chỉ";

        txtDiaChi.BackColor = GiaoDien.ManHinhNen;
        txtDiaChi.BorderStyle = BorderStyle.FixedSingle;
        txtDiaChi.Font = GiaoDien.ChuThuong;
        txtDiaChi.Location = new System.Drawing.Point(20, 252);
        txtDiaChi.Multiline = true;
        txtDiaChi.Name = "txtDiaChi";
        txtDiaChi.ReadOnly = true;
        txtDiaChi.Size = new System.Drawing.Size(340, 70);
        txtDiaChi.TabIndex = 3;

        pnlNut.Controls.Add(btnHuy);
        pnlNut.Controls.Add(btnLuu);
        pnlNut.Controls.Add(btnXoa);
        pnlNut.Controls.Add(btnSua);
        pnlNut.Controls.Add(btnThem);
        pnlNut.Dock = DockStyle.Bottom;
        pnlNut.Location = new System.Drawing.Point(20, 476);
        pnlNut.Name = "pnlNut";
        pnlNut.Size = new System.Drawing.Size(340, 116);
        pnlNut.TabIndex = 9;

        GiaoDien.DangNutChinh(btnThem);
        btnThem.Location = new System.Drawing.Point(0, 8);
        btnThem.Name = "btnThem";
        btnThem.Size = new System.Drawing.Size(100, 38);
        btnThem.TabIndex = 4;
        btnThem.Text = "Thêm";
        btnThem.UseVisualStyleBackColor = false;
        btnThem.Click += btnThem_Click;

        GiaoDien.DangNutPhu(btnSua);
        btnSua.Location = new System.Drawing.Point(110, 8);
        btnSua.Name = "btnSua";
        btnSua.Size = new System.Drawing.Size(90, 38);
        btnSua.TabIndex = 5;
        btnSua.Text = "Sửa";
        btnSua.UseVisualStyleBackColor = false;
        btnSua.Click += btnSua_Click;

        GiaoDien.DangNutNguyHiem(btnXoa);
        btnXoa.Location = new System.Drawing.Point(210, 8);
        btnXoa.Name = "btnXoa";
        btnXoa.Size = new System.Drawing.Size(90, 38);
        btnXoa.TabIndex = 6;
        btnXoa.Text = "Xóa";
        btnXoa.UseVisualStyleBackColor = false;
        btnXoa.Click += btnXoa_Click;

        GiaoDien.DangNutChinh(btnLuu);
        btnLuu.Location = new System.Drawing.Point(0, 56);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new System.Drawing.Size(160, 38);
        btnLuu.TabIndex = 7;
        btnLuu.Text = "Lưu";
        btnLuu.UseVisualStyleBackColor = false;
        btnLuu.Click += btnLuu_Click;

        GiaoDien.DangNutPhu(btnHuy);
        btnHuy.Location = new System.Drawing.Point(170, 56);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new System.Drawing.Size(130, 38);
        btnHuy.TabIndex = 8;
        btnHuy.Text = "Hủy bỏ";
        btnHuy.UseVisualStyleBackColor = false;
        btnHuy.Click += btnHuy_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmQuanLyKhachHang";
        Text = "Quản lý khách hàng";
        ((System.ComponentModel.ISupportInitialize)errLoi).EndInit();
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlTrai.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvKhachHang).EndInit();
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
    private DataGridView dgvKhachHang;
    private Panel pnlThanhCongCu;
    private Button btnLamMoi;
    private Button btnTim;
    private TextBox txtTimKiem;
    private Panel pnlPhai;
    private Panel pnlNut;
    private Button btnHuy;
    private Button btnLuu;
    private Button btnXoa;
    private Button btnSua;
    private Button btnThem;
    private Label lblNhanDiaChi;
    private Label lblNhanEmail;
    private Label lblNhanDienThoai;
    private Label lblNhanHoTen;
    private Label lblMaKH;
    private Label lblNhanMa;
    private TextBox txtDiaChi;
    private TextBox txtEmail;
    private TextBox txtSoDienThoai;
    private TextBox txtHoTen;
}
