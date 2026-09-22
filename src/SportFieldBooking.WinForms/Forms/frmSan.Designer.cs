using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmSan
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
        dgvSan = new DataGridView();
        pnlThanhCongCu = new Panel();
        cboTrangThai = new ComboBox();
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
        btnDoiTrangThai = new Button();
        lblNhanMoTa = new Label();
        lblNhanTrangThai = new Label();
        lblNhanDonGia = new Label();
        lblNhanLoaiSan = new Label();
        lblNhanTenSan = new Label();
        lblMaSan = new Label();
        lblNhanMa = new Label();
        cboTrangThaiNhap = new ComboBox();
        txtMoTa = new TextBox();
        txtDonGia = new TextBox();
        cboLoaiSan = new ComboBox();
        txtTenSan = new TextBox();
        ((System.ComponentModel.ISupportInitialize)errLoi).BeginInit();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlTrai.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvSan).BeginInit();
        pnlThanhCongCu.SuspendLayout();
        pnlPhai.SuspendLayout();
        pnlNut.SuspendLayout();
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
        lblTieuDe.Size = new System.Drawing.Size(160, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Quản lý sân";

        // lblMoTaTrang
        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(320, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Danh sách sân, đơn giá thuê theo giờ và trạng thái sân";

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
        pnlTrai.Controls.Add(dgvSan);
        pnlTrai.Controls.Add(pnlThanhCongCu);
        pnlTrai.Dock = DockStyle.Fill;
        pnlTrai.Location = new System.Drawing.Point(16, 16);
        pnlTrai.Name = "pnlTrai";
        pnlTrai.Size = new System.Drawing.Size(788, 610);
        pnlTrai.TabIndex = 0;

        // pnlThanhCongCu
        pnlThanhCongCu.Controls.Add(cboTrangThai);
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
        txtTimKiem.Size = new System.Drawing.Size(280, 25);
        txtTimKiem.TabIndex = 0;
        txtTimKiem.KeyDown += txtTimKiem_KeyDown;

        // cboTrangThai
        cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTrangThai.Font = GiaoDien.ChuThuong;
        cboTrangThai.Location = new System.Drawing.Point(304, 13);
        cboTrangThai.Name = "cboTrangThai";
        cboTrangThai.Size = new System.Drawing.Size(150, 25);
        cboTrangThai.TabIndex = 1;
        cboTrangThai.SelectedIndexChanged += cboTrangThai_SelectedIndexChanged;

        // btnTim
        GiaoDien.DangNutChinh(btnTim);
        btnTim.Location = new System.Drawing.Point(464, 10);
        btnTim.Name = "btnTim";
        btnTim.Size = new System.Drawing.Size(110, 34);
        btnTim.TabIndex = 2;
        btnTim.Text = "Tìm kiếm";
        btnTim.UseVisualStyleBackColor = false;
        btnTim.Click += btnTim_Click;

        // btnLamMoi
        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Location = new System.Drawing.Point(584, 10);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(110, 34);
        btnLamMoi.TabIndex = 3;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        // dgvSan
        dgvSan.Dock = DockStyle.Fill;
        dgvSan.Location = new System.Drawing.Point(0, 58);
        dgvSan.Name = "dgvSan";
        dgvSan.Size = new System.Drawing.Size(788, 552);
        dgvSan.TabIndex = 1;
        dgvSan.SelectionChanged += dgvSan_SelectionChanged;

        // pnlPhai
        pnlPhai.BackColor = GiaoDien.BeMat;
        pnlPhai.Controls.Add(pnlNut);
        pnlPhai.Controls.Add(lblNhanMoTa);
        pnlPhai.Controls.Add(lblNhanTrangThai);
        pnlPhai.Controls.Add(lblNhanDonGia);
        pnlPhai.Controls.Add(lblNhanLoaiSan);
        pnlPhai.Controls.Add(lblNhanTenSan);
        pnlPhai.Controls.Add(lblMaSan);
        pnlPhai.Controls.Add(lblNhanMa);
        pnlPhai.Controls.Add(cboTrangThaiNhap);
        pnlPhai.Controls.Add(txtMoTa);
        pnlPhai.Controls.Add(txtDonGia);
        pnlPhai.Controls.Add(cboLoaiSan);
        pnlPhai.Controls.Add(txtTenSan);
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
        lblNhanMa.Text = "Mã sân";

        // lblMaSan
        lblMaSan.AutoSize = true;
        lblMaSan.Font = GiaoDien.ChuDam;
        lblMaSan.ForeColor = GiaoDien.ChinhDam;
        lblMaSan.Location = new System.Drawing.Point(150, 19);
        lblMaSan.Name = "lblMaSan";
        lblMaSan.Size = new System.Drawing.Size(20, 17);
        lblMaSan.TabIndex = 1;
        lblMaSan.Text = "—";

        // lblNhanTenSan
        lblNhanTenSan.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTenSan);
        lblNhanTenSan.Location = new System.Drawing.Point(20, 54);
        lblNhanTenSan.Name = "lblNhanTenSan";
        lblNhanTenSan.TabIndex = 2;
        lblNhanTenSan.Text = "Tên sân (*)";

        // txtTenSan
        txtTenSan.BackColor = GiaoDien.ManHinhNen;
        txtTenSan.BorderStyle = BorderStyle.FixedSingle;
        txtTenSan.Font = GiaoDien.ChuThuong;
        txtTenSan.Location = new System.Drawing.Point(20, 74);
        txtTenSan.Name = "txtTenSan";
        txtTenSan.ReadOnly = true;
        txtTenSan.Size = new System.Drawing.Size(340, 25);
        txtTenSan.TabIndex = 0;

        // lblNhanLoaiSan
        lblNhanLoaiSan.AutoSize = true;
        GiaoDien.DangNhan(lblNhanLoaiSan);
        lblNhanLoaiSan.Location = new System.Drawing.Point(20, 108);
        lblNhanLoaiSan.Name = "lblNhanLoaiSan";
        lblNhanLoaiSan.TabIndex = 4;
        lblNhanLoaiSan.Text = "Loại sân (*)";

        // cboLoaiSan
        cboLoaiSan.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLoaiSan.Font = GiaoDien.ChuThuong;
        cboLoaiSan.Location = new System.Drawing.Point(20, 128);
        cboLoaiSan.Name = "cboLoaiSan";
        cboLoaiSan.Size = new System.Drawing.Size(340, 25);
        cboLoaiSan.TabIndex = 1;

        // lblNhanDonGia
        lblNhanDonGia.AutoSize = true;
        GiaoDien.DangNhan(lblNhanDonGia);
        lblNhanDonGia.Location = new System.Drawing.Point(20, 162);
        lblNhanDonGia.Name = "lblNhanDonGia";
        lblNhanDonGia.TabIndex = 6;
        lblNhanDonGia.Text = "Đơn giá (đ/giờ) (*)";

        // txtDonGia
        txtDonGia.BackColor = GiaoDien.ManHinhNen;
        txtDonGia.BorderStyle = BorderStyle.FixedSingle;
        txtDonGia.Font = GiaoDien.ChuThuong;
        txtDonGia.Location = new System.Drawing.Point(20, 182);
        txtDonGia.Name = "txtDonGia";
        txtDonGia.ReadOnly = true;
        txtDonGia.Size = new System.Drawing.Size(340, 25);
        txtDonGia.TabIndex = 2;
        txtDonGia.TextAlign = HorizontalAlignment.Right;

        // lblNhanTrangThai
        lblNhanTrangThai.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTrangThai);
        lblNhanTrangThai.Location = new System.Drawing.Point(20, 216);
        lblNhanTrangThai.Name = "lblNhanTrangThai";
        lblNhanTrangThai.TabIndex = 8;
        lblNhanTrangThai.Text = "Trạng thái";

        // cboTrangThaiNhap
        cboTrangThaiNhap.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTrangThaiNhap.Font = GiaoDien.ChuThuong;
        cboTrangThaiNhap.Location = new System.Drawing.Point(20, 236);
        cboTrangThaiNhap.Name = "cboTrangThaiNhap";
        cboTrangThaiNhap.Size = new System.Drawing.Size(340, 25);
        cboTrangThaiNhap.TabIndex = 3;

        // lblNhanMoTa
        lblNhanMoTa.AutoSize = true;
        GiaoDien.DangNhan(lblNhanMoTa);
        lblNhanMoTa.Location = new System.Drawing.Point(20, 270);
        lblNhanMoTa.Name = "lblNhanMoTa";
        lblNhanMoTa.TabIndex = 10;
        lblNhanMoTa.Text = "Mô tả";

        // txtMoTa
        txtMoTa.BackColor = GiaoDien.ManHinhNen;
        txtMoTa.BorderStyle = BorderStyle.FixedSingle;
        txtMoTa.Font = GiaoDien.ChuThuong;
        txtMoTa.Location = new System.Drawing.Point(20, 290);
        txtMoTa.Multiline = true;
        txtMoTa.Name = "txtMoTa";
        txtMoTa.ReadOnly = true;
        txtMoTa.Size = new System.Drawing.Size(340, 80);
        txtMoTa.TabIndex = 4;

        // pnlNut
        pnlNut.Controls.Add(btnHuy);
        pnlNut.Controls.Add(btnLuu);
        pnlNut.Controls.Add(btnXoa);
        pnlNut.Controls.Add(btnSua);
        pnlNut.Controls.Add(btnThem);
        pnlNut.Controls.Add(btnDoiTrangThai);
        pnlNut.Dock = DockStyle.None;
        pnlNut.Location = new System.Drawing.Point(20, 520);
        pnlNut.Name = "pnlNut";
        pnlNut.Size = new System.Drawing.Size(340, 152);
        pnlNut.TabIndex = 11;

        // btnDoiTrangThai
        GiaoDien.DangNutPhu(btnDoiTrangThai);
        btnDoiTrangThai.Location = new System.Drawing.Point(0, 0);
        btnDoiTrangThai.Name = "btnDoiTrangThai";
        btnDoiTrangThai.Size = new System.Drawing.Size(340, 38);
        btnDoiTrangThai.TabIndex = 5;
        btnDoiTrangThai.Text = "Đổi trạng thái sân";
        btnDoiTrangThai.UseVisualStyleBackColor = false;
        btnDoiTrangThai.Click += btnDoiTrangThai_Click;

        // btnThem
        GiaoDien.DangNutChinh(btnThem);
        btnThem.Location = new System.Drawing.Point(0, 56);
        btnThem.Name = "btnThem";
        btnThem.Size = new System.Drawing.Size(100, 38);
        btnThem.TabIndex = 6;
        btnThem.Text = "Thêm";
        btnThem.UseVisualStyleBackColor = false;
        btnThem.Click += btnThem_Click;

        // btnSua
        GiaoDien.DangNutPhu(btnSua);
        btnSua.Location = new System.Drawing.Point(110, 56);
        btnSua.Name = "btnSua";
        btnSua.Size = new System.Drawing.Size(90, 38);
        btnSua.TabIndex = 7;
        btnSua.Text = "Sửa";
        btnSua.UseVisualStyleBackColor = false;
        btnSua.Click += btnSua_Click;

        // btnXoa
        GiaoDien.DangNutNguyHiem(btnXoa);
        btnXoa.Location = new System.Drawing.Point(210, 56);
        btnXoa.Name = "btnXoa";
        btnXoa.Size = new System.Drawing.Size(90, 38);
        btnXoa.TabIndex = 8;
        btnXoa.Text = "Xóa";
        btnXoa.UseVisualStyleBackColor = false;
        btnXoa.Click += btnXoa_Click;

        // btnLuu
        GiaoDien.DangNutChinh(btnLuu);
        btnLuu.Location = new System.Drawing.Point(0, 102);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new System.Drawing.Size(160, 38);
        btnLuu.TabIndex = 9;
        btnLuu.Text = "Lưu";
        btnLuu.UseVisualStyleBackColor = false;
        btnLuu.Click += btnLuu_Click;

        // btnHuy
        GiaoDien.DangNutPhu(btnHuy);
        btnHuy.Location = new System.Drawing.Point(170, 102);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new System.Drawing.Size(130, 38);
        btnHuy.TabIndex = 10;
        btnHuy.Text = "Hủy bỏ";
        btnHuy.UseVisualStyleBackColor = false;
        btnHuy.Click += btnHuy_Click;

        // frmSan
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmSan";
        Text = "Quản lý sân";
        ((System.ComponentModel.ISupportInitialize)errLoi).EndInit();
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlTrai.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvSan).EndInit();
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
    private DataGridView dgvSan;
    private Panel pnlThanhCongCu;
    private ComboBox cboTrangThai;
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
    private Button btnDoiTrangThai;
    private Label lblNhanMoTa;
    private Label lblNhanTrangThai;
    private Label lblNhanDonGia;
    private Label lblNhanLoaiSan;
    private Label lblNhanTenSan;
    private Label lblMaSan;
    private Label lblNhanMa;
    private ComboBox cboTrangThaiNhap;
    private TextBox txtMoTa;
    private TextBox txtDonGia;
    private ComboBox cboLoaiSan;
    private TextBox txtTenSan;
}
