using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmLoaiSan
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
        pnlPhai = new Panel();
        pnlNut = new Panel();
        btnHuy = new Button();
        btnLuu = new Button();
        btnXoa = new Button();
        btnSua = new Button();
        btnThem = new Button();
        lblNhanMoTa = new Label();
        lblNhanTenLoai = new Label();
        lblMaLoaiSan = new Label();
        lblNhanMa = new Label();
        txtMoTa = new TextBox();
        txtTenLoaiSan = new TextBox();
        pnlTrai = new Panel();
        dgvLoaiSan = new DataGridView();
        pnlThanhCongCu = new Panel();
        btnLamMoi = new Button();
        btnTim = new Button();
        txtTimKiem = new TextBox();
        ((System.ComponentModel.ISupportInitialize)errLoi).BeginInit();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlPhai.SuspendLayout();
        pnlNut.SuspendLayout();
        pnlTrai.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLoaiSan).BeginInit();
        pnlThanhCongCu.SuspendLayout();
        SuspendLayout();

        // errLoi
        errLoi.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errLoi.ContainerControl = this;

        // pnlDau
        pnlDau.BackColor = GiaoDien.ThanhBen;
        pnlDau.Controls.Add(lblMoTaTrang);
        pnlDau.Controls.Add(lblTieuDe);
        pnlDau.Controls.Add(picBieuTuong);
        pnlDau.Dock = DockStyle.Top;
        pnlDau.Location = new System.Drawing.Point(0, 0);
        pnlDau.Name = "pnlDau";
        pnlDau.Size = new System.Drawing.Size(1200, 78);
        pnlDau.TabIndex = 0;

        // picBieuTuong
        picBieuTuong.Location = new System.Drawing.Point(20, 16);
        picBieuTuong.MauBieuTuong = GiaoDien.Chinh;
        picBieuTuong.Name = "picBieuTuong";
        picBieuTuong.Size = new System.Drawing.Size(44, 44);
        picBieuTuong.TabIndex = 0;
        picBieuTuong.TenBieuTuong = "san";

        // lblTieuDe
        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(76, 14);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(200, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Quản lý loại sân";

        // lblMoTaTrang
        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(280, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Thêm, sửa, xóa loại sân (sân 5, sân 7, cầu lông...)";

        // pnlNoiDung
        pnlNoiDung.Controls.Add(pnlTrai);
        pnlNoiDung.Controls.Add(pnlPhai);
        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(0, 78);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(16);
        pnlNoiDung.Size = new System.Drawing.Size(1200, 642);
        pnlNoiDung.TabIndex = 1;

        // pnlTrai
        pnlTrai.BackColor = GiaoDien.BeMat;
        pnlTrai.Controls.Add(dgvLoaiSan);
        pnlTrai.Controls.Add(pnlThanhCongCu);
        pnlTrai.Dock = DockStyle.Fill;
        pnlTrai.Location = new System.Drawing.Point(16, 16);
        pnlTrai.Name = "pnlTrai";
        pnlTrai.Size = new System.Drawing.Size(788, 610);
        pnlTrai.TabIndex = 0;

        // pnlThanhCongCu
        pnlThanhCongCu.BackColor = GiaoDien.BeMat;
        pnlThanhCongCu.Controls.Add(btnLamMoi);
        pnlThanhCongCu.Controls.Add(btnTim);
        pnlThanhCongCu.Controls.Add(txtTimKiem);
        pnlThanhCongCu.Dock = DockStyle.Top;
        pnlThanhCongCu.Location = new System.Drawing.Point(0, 0);
        pnlThanhCongCu.Name = "pnlThanhCongCu";
        pnlThanhCongCu.Padding = new Padding(12, 10, 12, 10);
        pnlThanhCongCu.Size = new System.Drawing.Size(788, 58);
        pnlThanhCongCu.TabIndex = 0;

        // txtTimKiem
        txtTimKiem.BackColor = GiaoDien.ManHinhNen;
        txtTimKiem.BorderStyle = BorderStyle.FixedSingle;
        txtTimKiem.Font = GiaoDien.ChuThuong;
        txtTimKiem.Location = new System.Drawing.Point(12, 14);
        txtTimKiem.Name = "txtTimKiem";
        txtTimKiem.Size = new System.Drawing.Size(360, 25);
        txtTimKiem.TabIndex = 0;
        txtTimKiem.KeyDown += txtTimKiem_KeyDown;

        // btnTim
        GiaoDien.DangNutChinh(btnTim);
        btnTim.Location = new System.Drawing.Point(384, 10);
        btnTim.Name = "btnTim";
        btnTim.Size = new System.Drawing.Size(110, 34);
        btnTim.TabIndex = 1;
        btnTim.Text = "Tìm kiếm";
        btnTim.UseVisualStyleBackColor = false;
        btnTim.Click += btnTim_Click;

        // btnLamMoi
        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Location = new System.Drawing.Point(504, 10);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(110, 34);
        btnLamMoi.TabIndex = 2;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        // dgvLoaiSan
        dgvLoaiSan.Dock = DockStyle.Fill;
        dgvLoaiSan.Location = new System.Drawing.Point(0, 58);
        dgvLoaiSan.Name = "dgvLoaiSan";
        dgvLoaiSan.Size = new System.Drawing.Size(788, 552);
        dgvLoaiSan.TabIndex = 1;
        dgvLoaiSan.SelectionChanged += dgvLoaiSan_SelectionChanged;

        // pnlPhai
        pnlPhai.BackColor = GiaoDien.BeMat;
        pnlPhai.Controls.Add(pnlNut);
        pnlPhai.Controls.Add(lblNhanMoTa);
        pnlPhai.Controls.Add(lblNhanTenLoai);
        pnlPhai.Controls.Add(lblMaLoaiSan);
        pnlPhai.Controls.Add(lblNhanMa);
        pnlPhai.Controls.Add(txtMoTa);
        pnlPhai.Controls.Add(txtTenLoaiSan);
        pnlPhai.Dock = DockStyle.Right;
        pnlPhai.Location = new System.Drawing.Point(804, 16);
        pnlPhai.Name = "pnlPhai";
        pnlPhai.Padding = new Padding(20, 18, 20, 18);
        pnlPhai.Size = new System.Drawing.Size(380, 610);
        pnlPhai.TabIndex = 1;

        // lblNhanMa
        lblNhanMa.AutoSize = true;
        GiaoDien.DangNhan(lblNhanMa);
        lblNhanMa.Location = new System.Drawing.Point(20, 20);
        lblNhanMa.Name = "lblNhanMa";
        lblNhanMa.TabIndex = 0;
        lblNhanMa.Text = "Mã loại sân";

        // lblMaLoaiSan
        lblMaLoaiSan.AutoSize = true;
        lblMaLoaiSan.Font = GiaoDien.ChuDam;
        lblMaLoaiSan.ForeColor = GiaoDien.ChinhDam;
        lblMaLoaiSan.Location = new System.Drawing.Point(150, 19);
        lblMaLoaiSan.Name = "lblMaLoaiSan";
        lblMaLoaiSan.Size = new System.Drawing.Size(20, 17);
        lblMaLoaiSan.TabIndex = 1;
        lblMaLoaiSan.Text = "—";

        // lblNhanTenLoai
        lblNhanTenLoai.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTenLoai);
        lblNhanTenLoai.Location = new System.Drawing.Point(20, 58);
        lblNhanTenLoai.Name = "lblNhanTenLoai";
        lblNhanTenLoai.TabIndex = 2;
        lblNhanTenLoai.Text = "Tên loại sân (*)";

        // txtTenLoaiSan
        txtTenLoaiSan.BackColor = GiaoDien.ManHinhNen;
        txtTenLoaiSan.BorderStyle = BorderStyle.FixedSingle;
        txtTenLoaiSan.Font = GiaoDien.ChuThuong;
        txtTenLoaiSan.Location = new System.Drawing.Point(20, 78);
        txtTenLoaiSan.Name = "txtTenLoaiSan";
        txtTenLoaiSan.ReadOnly = true;
        txtTenLoaiSan.Size = new System.Drawing.Size(340, 25);
        txtTenLoaiSan.TabIndex = 0;

        // lblNhanMoTa
        lblNhanMoTa.AutoSize = true;
        GiaoDien.DangNhan(lblNhanMoTa);
        lblNhanMoTa.Location = new System.Drawing.Point(20, 116);
        lblNhanMoTa.Name = "lblNhanMoTa";
        lblNhanMoTa.TabIndex = 4;
        lblNhanMoTa.Text = "Mô tả";

        // txtMoTa
        txtMoTa.BackColor = GiaoDien.ManHinhNen;
        txtMoTa.BorderStyle = BorderStyle.FixedSingle;
        txtMoTa.Font = GiaoDien.ChuThuong;
        txtMoTa.Location = new System.Drawing.Point(20, 136);
        txtMoTa.Multiline = true;
        txtMoTa.Name = "txtMoTa";
        txtMoTa.ReadOnly = true;
        txtMoTa.Size = new System.Drawing.Size(340, 90);
        txtMoTa.TabIndex = 1;

        // pnlNut
        pnlNut.BackColor = GiaoDien.BeMat;
        pnlNut.Controls.Add(btnHuy);
        pnlNut.Controls.Add(btnLuu);
        pnlNut.Controls.Add(btnXoa);
        pnlNut.Controls.Add(btnSua);
        pnlNut.Controls.Add(btnThem);
        pnlNut.Dock = DockStyle.Bottom;
        pnlNut.Location = new System.Drawing.Point(20, 496);
        pnlNut.Name = "pnlNut";
        pnlNut.Size = new System.Drawing.Size(340, 96);
        pnlNut.TabIndex = 6;

        // btnThem
        GiaoDien.DangNutChinh(btnThem);
        btnThem.Location = new System.Drawing.Point(0, 8);
        btnThem.Name = "btnThem";
        btnThem.Size = new System.Drawing.Size(100, 38);
        btnThem.TabIndex = 2;
        btnThem.Text = "Thêm";
        btnThem.UseVisualStyleBackColor = false;
        btnThem.Click += btnThem_Click;

        // btnSua
        GiaoDien.DangNutPhu(btnSua);
        btnSua.Location = new System.Drawing.Point(110, 8);
        btnSua.Name = "btnSua";
        btnSua.Size = new System.Drawing.Size(90, 38);
        btnSua.TabIndex = 3;
        btnSua.Text = "Sửa";
        btnSua.UseVisualStyleBackColor = false;
        btnSua.Click += btnSua_Click;

        // btnXoa
        GiaoDien.DangNutNguyHiem(btnXoa);
        btnXoa.Location = new System.Drawing.Point(210, 8);
        btnXoa.Name = "btnXoa";
        btnXoa.Size = new System.Drawing.Size(90, 38);
        btnXoa.TabIndex = 4;
        btnXoa.Text = "Xóa";
        btnXoa.UseVisualStyleBackColor = false;
        btnXoa.Click += btnXoa_Click;

        // btnLuu
        GiaoDien.DangNutChinh(btnLuu);
        btnLuu.Location = new System.Drawing.Point(0, 54);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new System.Drawing.Size(160, 38);
        btnLuu.TabIndex = 5;
        btnLuu.Text = "Lưu";
        btnLuu.UseVisualStyleBackColor = false;
        btnLuu.Click += btnLuu_Click;

        // btnHuy
        GiaoDien.DangNutPhu(btnHuy);
        btnHuy.Location = new System.Drawing.Point(170, 54);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new System.Drawing.Size(130, 38);
        btnHuy.TabIndex = 6;
        btnHuy.Text = "Hủy bỏ";
        btnHuy.UseVisualStyleBackColor = false;
        btnHuy.Click += btnHuy_Click;

        // frmLoaiSan
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmLoaiSan";
        Text = "Quản lý loại sân";
        ((System.ComponentModel.ISupportInitialize)errLoi).EndInit();
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlPhai.ResumeLayout(false);
        pnlPhai.PerformLayout();
        pnlNut.ResumeLayout(false);
        pnlTrai.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvLoaiSan).EndInit();
        pnlThanhCongCu.ResumeLayout(false);
        pnlThanhCongCu.PerformLayout();
        ResumeLayout(false);
    }

    private ErrorProvider errLoi;
    private Panel pnlDau;
    private Label lblMoTaTrang;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Panel pnlNoiDung;
    private Panel pnlTrai;
    private DataGridView dgvLoaiSan;
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
    private Label lblNhanMoTa;
    private Label lblNhanTenLoai;
    private Label lblMaLoaiSan;
    private Label lblNhanMa;
    private TextBox txtMoTa;
    private TextBox txtTenLoaiSan;
}
