using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmVoucher
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
        dgvVoucher = new DataGridView();
        pnlThanhCongCu = new Panel();
        chkChiConHan = new CheckBox();
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
        lblNhanMoTa = new Label();
        lblNhanTrangThai = new Label();
        lblNhanNgayKetThuc = new Label();
        lblNhanNgayBatDau = new Label();
        lblDaDung = new Label();
        lblNhanDaDung = new Label();
        lblNhanSoLuong = new Label();
        lblNhanDonToiThieu = new Label();
        lblNhanGiaTriGiam = new Label();
        lblNhanLoaiGiam = new Label();
        lblNhanTenVoucher = new Label();
        lblNhanMaCode = new Label();
        lblMaVoucher = new Label();
        lblNhanMa = new Label();
        txtMoTa = new TextBox();
        cboTrangThai = new ComboBox();
        dtpNgayKetThuc = new DateTimePicker();
        dtpNgayBatDau = new DateTimePicker();
        txtSoLuong = new TextBox();
        txtDonToiThieu = new TextBox();
        txtGiaTriGiam = new TextBox();
        cboLoaiGiam = new ComboBox();
        txtTenVoucher = new TextBox();
        txtMaCode = new TextBox();
        ((System.ComponentModel.ISupportInitialize)errLoi).BeginInit();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlTrai.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvVoucher).BeginInit();
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
        picBieuTuong.TenBieuTuong = "voucher";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(76, 14);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(180, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Quản lý voucher";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(400, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Phát hành mã giảm giá, giới hạn số lượng và giá trị đơn tối thiểu";

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
        pnlTrai.Controls.Add(dgvVoucher);
        pnlTrai.Controls.Add(pnlThanhCongCu);
        pnlTrai.Dock = DockStyle.Fill;
        pnlTrai.Location = new System.Drawing.Point(16, 16);
        pnlTrai.Name = "pnlTrai";
        pnlTrai.Size = new System.Drawing.Size(788, 610);
        pnlTrai.TabIndex = 0;

        pnlThanhCongCu.Controls.Add(chkChiConHan);
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
        txtTimKiem.Size = new System.Drawing.Size(320, 25);
        txtTimKiem.TabIndex = 0;
        txtTimKiem.KeyDown += txtTimKiem_KeyDown;

        GiaoDien.DangNutChinh(btnTim);
        btnTim.Location = new System.Drawing.Point(344, 10);
        btnTim.Name = "btnTim";
        btnTim.Size = new System.Drawing.Size(100, 34);
        btnTim.TabIndex = 1;
        btnTim.Text = "Tìm";
        btnTim.UseVisualStyleBackColor = false;
        btnTim.Click += btnTim_Click;

        chkChiConHan.AutoSize = true;
        chkChiConHan.Font = GiaoDien.ChuThuong;
        chkChiConHan.ForeColor = GiaoDien.Chu;
        chkChiConHan.Location = new System.Drawing.Point(460, 18);
        chkChiConHan.Name = "chkChiConHan";
        chkChiConHan.Size = new System.Drawing.Size(160, 20);
        chkChiConHan.TabIndex = 2;
        chkChiConHan.Text = "Chỉ hiện voucher còn hạn";
        chkChiConHan.CheckedChanged += chkChiConHan_CheckedChanged;

        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Location = new System.Drawing.Point(650, 10);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(110, 34);
        btnLamMoi.TabIndex = 3;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        dgvVoucher.Dock = DockStyle.Fill;
        dgvVoucher.Location = new System.Drawing.Point(0, 58);
        dgvVoucher.Name = "dgvVoucher";
        dgvVoucher.Size = new System.Drawing.Size(788, 552);
        dgvVoucher.TabIndex = 1;
        dgvVoucher.SelectionChanged += dgvVoucher_SelectionChanged;

        pnlPhai.AutoScroll = true;
        pnlPhai.AutoScrollMinSize = new System.Drawing.Size(0, 700);
        pnlPhai.BackColor = GiaoDien.BeMat;
        pnlPhai.Controls.Add(pnlNut);
        pnlPhai.Controls.Add(lblNhanMoTa);
        pnlPhai.Controls.Add(lblNhanTrangThai);
        pnlPhai.Controls.Add(lblNhanNgayKetThuc);
        pnlPhai.Controls.Add(lblNhanNgayBatDau);
        pnlPhai.Controls.Add(lblDaDung);
        pnlPhai.Controls.Add(lblNhanDaDung);
        pnlPhai.Controls.Add(lblNhanSoLuong);
        pnlPhai.Controls.Add(lblNhanDonToiThieu);
        pnlPhai.Controls.Add(lblNhanGiaTriGiam);
        pnlPhai.Controls.Add(lblNhanLoaiGiam);
        pnlPhai.Controls.Add(lblNhanTenVoucher);
        pnlPhai.Controls.Add(lblNhanMaCode);
        pnlPhai.Controls.Add(lblMaVoucher);
        pnlPhai.Controls.Add(lblNhanMa);
        pnlPhai.Controls.Add(txtMoTa);
        pnlPhai.Controls.Add(cboTrangThai);
        pnlPhai.Controls.Add(dtpNgayKetThuc);
        pnlPhai.Controls.Add(dtpNgayBatDau);
        pnlPhai.Controls.Add(txtSoLuong);
        pnlPhai.Controls.Add(txtDonToiThieu);
        pnlPhai.Controls.Add(txtGiaTriGiam);
        pnlPhai.Controls.Add(cboLoaiGiam);
        pnlPhai.Controls.Add(txtTenVoucher);
        pnlPhai.Controls.Add(txtMaCode);
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
        lblNhanMa.Text = "Mã voucher";

        lblMaVoucher.AutoSize = true;
        lblMaVoucher.Font = GiaoDien.ChuDam;
        lblMaVoucher.ForeColor = GiaoDien.ChinhDam;
        lblMaVoucher.Location = new System.Drawing.Point(150, 19);
        lblMaVoucher.Name = "lblMaVoucher";
        lblMaVoucher.Size = new System.Drawing.Size(20, 17);
        lblMaVoucher.TabIndex = 1;
        lblMaVoucher.Text = "—";

        lblNhanMaCode.AutoSize = true;
        GiaoDien.DangNhan(lblNhanMaCode);
        lblNhanMaCode.Location = new System.Drawing.Point(20, 58);
        lblNhanMaCode.Name = "lblNhanMaCode";
        lblNhanMaCode.TabIndex = 2;
        lblNhanMaCode.Text = "Mã code (*)";

        txtMaCode.BackColor = GiaoDien.ManHinhNen;
        txtMaCode.BorderStyle = BorderStyle.FixedSingle;
        txtMaCode.CharacterCasing = CharacterCasing.Upper;
        txtMaCode.Font = GiaoDien.ChuThuong;
        txtMaCode.Location = new System.Drawing.Point(20, 78);
        txtMaCode.Name = "txtMaCode";
        txtMaCode.ReadOnly = true;
        txtMaCode.Size = new System.Drawing.Size(340, 25);
        txtMaCode.TabIndex = 0;

        lblNhanTenVoucher.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTenVoucher);
        lblNhanTenVoucher.Location = new System.Drawing.Point(20, 116);
        lblNhanTenVoucher.Name = "lblNhanTenVoucher";
        lblNhanTenVoucher.TabIndex = 4;
        lblNhanTenVoucher.Text = "Tên voucher (*)";

        txtTenVoucher.BackColor = GiaoDien.ManHinhNen;
        txtTenVoucher.BorderStyle = BorderStyle.FixedSingle;
        txtTenVoucher.Font = GiaoDien.ChuThuong;
        txtTenVoucher.Location = new System.Drawing.Point(20, 136);
        txtTenVoucher.Name = "txtTenVoucher";
        txtTenVoucher.ReadOnly = true;
        txtTenVoucher.Size = new System.Drawing.Size(340, 25);
        txtTenVoucher.TabIndex = 1;

        lblNhanLoaiGiam.AutoSize = true;
        GiaoDien.DangNhan(lblNhanLoaiGiam);
        lblNhanLoaiGiam.Location = new System.Drawing.Point(20, 174);
        lblNhanLoaiGiam.Name = "lblNhanLoaiGiam";
        lblNhanLoaiGiam.TabIndex = 6;
        lblNhanLoaiGiam.Text = "Loại giảm";

        cboLoaiGiam.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLoaiGiam.Enabled = false;
        cboLoaiGiam.Font = GiaoDien.ChuThuong;
        cboLoaiGiam.Location = new System.Drawing.Point(20, 194);
        cboLoaiGiam.Name = "cboLoaiGiam";
        cboLoaiGiam.Size = new System.Drawing.Size(160, 25);
        cboLoaiGiam.TabIndex = 2;

        lblNhanGiaTriGiam.AutoSize = true;
        GiaoDien.DangNhan(lblNhanGiaTriGiam);
        lblNhanGiaTriGiam.Location = new System.Drawing.Point(200, 174);
        lblNhanGiaTriGiam.Name = "lblNhanGiaTriGiam";
        lblNhanGiaTriGiam.TabIndex = 8;
        lblNhanGiaTriGiam.Text = "Giá trị giảm (*)";

        txtGiaTriGiam.BackColor = GiaoDien.ManHinhNen;
        txtGiaTriGiam.BorderStyle = BorderStyle.FixedSingle;
        txtGiaTriGiam.Font = GiaoDien.ChuThuong;
        txtGiaTriGiam.Location = new System.Drawing.Point(200, 194);
        txtGiaTriGiam.Name = "txtGiaTriGiam";
        txtGiaTriGiam.ReadOnly = true;
        txtGiaTriGiam.Size = new System.Drawing.Size(160, 25);
        txtGiaTriGiam.TabIndex = 3;

        lblNhanDonToiThieu.AutoSize = true;
        GiaoDien.DangNhan(lblNhanDonToiThieu);
        lblNhanDonToiThieu.Location = new System.Drawing.Point(20, 232);
        lblNhanDonToiThieu.Name = "lblNhanDonToiThieu";
        lblNhanDonToiThieu.TabIndex = 10;
        lblNhanDonToiThieu.Text = "Đơn tối thiểu (đ)";

        txtDonToiThieu.BackColor = GiaoDien.ManHinhNen;
        txtDonToiThieu.BorderStyle = BorderStyle.FixedSingle;
        txtDonToiThieu.Font = GiaoDien.ChuThuong;
        txtDonToiThieu.Location = new System.Drawing.Point(20, 252);
        txtDonToiThieu.Name = "txtDonToiThieu";
        txtDonToiThieu.ReadOnly = true;
        txtDonToiThieu.Size = new System.Drawing.Size(160, 25);
        txtDonToiThieu.TabIndex = 4;

        lblNhanSoLuong.AutoSize = true;
        GiaoDien.DangNhan(lblNhanSoLuong);
        lblNhanSoLuong.Location = new System.Drawing.Point(200, 232);
        lblNhanSoLuong.Name = "lblNhanSoLuong";
        lblNhanSoLuong.TabIndex = 12;
        lblNhanSoLuong.Text = "Số lượng phát hành";

        txtSoLuong.BackColor = GiaoDien.ManHinhNen;
        txtSoLuong.BorderStyle = BorderStyle.FixedSingle;
        txtSoLuong.Font = GiaoDien.ChuThuong;
        txtSoLuong.Location = new System.Drawing.Point(200, 252);
        txtSoLuong.Name = "txtSoLuong";
        txtSoLuong.ReadOnly = true;
        txtSoLuong.Size = new System.Drawing.Size(160, 25);
        txtSoLuong.TabIndex = 5;

        lblNhanDaDung.AutoSize = true;
        GiaoDien.DangNhan(lblNhanDaDung);
        lblNhanDaDung.Location = new System.Drawing.Point(20, 290);
        lblNhanDaDung.Name = "lblNhanDaDung";
        lblNhanDaDung.TabIndex = 14;
        lblNhanDaDung.Text = "Đã dùng / Tổng";

        lblDaDung.AutoSize = true;
        lblDaDung.Font = GiaoDien.ChuDam;
        lblDaDung.ForeColor = GiaoDien.Chu;
        lblDaDung.Location = new System.Drawing.Point(20, 310);
        lblDaDung.Name = "lblDaDung";
        lblDaDung.Size = new System.Drawing.Size(20, 17);
        lblDaDung.TabIndex = 15;
        lblDaDung.Text = "0";

        lblNhanNgayBatDau.AutoSize = true;
        GiaoDien.DangNhan(lblNhanNgayBatDau);
        lblNhanNgayBatDau.Location = new System.Drawing.Point(20, 348);
        lblNhanNgayBatDau.Name = "lblNhanNgayBatDau";
        lblNhanNgayBatDau.TabIndex = 16;
        lblNhanNgayBatDau.Text = "Ngày bắt đầu";

        dtpNgayBatDau.Enabled = false;
        dtpNgayBatDau.Font = GiaoDien.ChuThuong;
        dtpNgayBatDau.Format = DateTimePickerFormat.Short;
        dtpNgayBatDau.Location = new System.Drawing.Point(20, 368);
        dtpNgayBatDau.Name = "dtpNgayBatDau";
        dtpNgayBatDau.Size = new System.Drawing.Size(160, 25);
        dtpNgayBatDau.TabIndex = 6;

        lblNhanNgayKetThuc.AutoSize = true;
        GiaoDien.DangNhan(lblNhanNgayKetThuc);
        lblNhanNgayKetThuc.Location = new System.Drawing.Point(200, 348);
        lblNhanNgayKetThuc.Name = "lblNhanNgayKetThuc";
        lblNhanNgayKetThuc.TabIndex = 18;
        lblNhanNgayKetThuc.Text = "Ngày kết thúc";

        dtpNgayKetThuc.Enabled = false;
        dtpNgayKetThuc.Font = GiaoDien.ChuThuong;
        dtpNgayKetThuc.Format = DateTimePickerFormat.Short;
        dtpNgayKetThuc.Location = new System.Drawing.Point(200, 368);
        dtpNgayKetThuc.Name = "dtpNgayKetThuc";
        dtpNgayKetThuc.Size = new System.Drawing.Size(160, 25);
        dtpNgayKetThuc.TabIndex = 7;

        lblNhanTrangThai.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTrangThai);
        lblNhanTrangThai.Location = new System.Drawing.Point(20, 406);
        lblNhanTrangThai.Name = "lblNhanTrangThai";
        lblNhanTrangThai.TabIndex = 20;
        lblNhanTrangThai.Text = "Trạng thái";

        cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTrangThai.Enabled = false;
        cboTrangThai.Font = GiaoDien.ChuThuong;
        cboTrangThai.Location = new System.Drawing.Point(20, 426);
        cboTrangThai.Name = "cboTrangThai";
        cboTrangThai.Size = new System.Drawing.Size(160, 25);
        cboTrangThai.TabIndex = 8;

        lblNhanMoTa.AutoSize = true;
        GiaoDien.DangNhan(lblNhanMoTa);
        lblNhanMoTa.Location = new System.Drawing.Point(20, 464);
        lblNhanMoTa.Name = "lblNhanMoTa";
        lblNhanMoTa.TabIndex = 22;
        lblNhanMoTa.Text = "Mô tả";

        txtMoTa.BackColor = GiaoDien.ManHinhNen;
        txtMoTa.BorderStyle = BorderStyle.FixedSingle;
        txtMoTa.Font = GiaoDien.ChuThuong;
        txtMoTa.Location = new System.Drawing.Point(20, 484);
        txtMoTa.Multiline = true;
        txtMoTa.Name = "txtMoTa";
        txtMoTa.ReadOnly = true;
        txtMoTa.Size = new System.Drawing.Size(340, 60);
        txtMoTa.TabIndex = 9;

        pnlNut.Controls.Add(btnHuy);
        pnlNut.Controls.Add(btnLuu);
        pnlNut.Controls.Add(btnXoa);
        pnlNut.Controls.Add(btnSua);
        pnlNut.Controls.Add(btnThem);
        pnlNut.Location = new System.Drawing.Point(20, 560);
        pnlNut.Name = "pnlNut";
        pnlNut.Size = new System.Drawing.Size(340, 116);
        pnlNut.TabIndex = 24;

        GiaoDien.DangNutChinh(btnThem);
        btnThem.Location = new System.Drawing.Point(0, 8);
        btnThem.Name = "btnThem";
        btnThem.Size = new System.Drawing.Size(100, 38);
        btnThem.TabIndex = 10;
        btnThem.Text = "Thêm";
        btnThem.UseVisualStyleBackColor = false;
        btnThem.Click += btnThem_Click;

        GiaoDien.DangNutPhu(btnSua);
        btnSua.Location = new System.Drawing.Point(110, 8);
        btnSua.Name = "btnSua";
        btnSua.Size = new System.Drawing.Size(90, 38);
        btnSua.TabIndex = 11;
        btnSua.Text = "Sửa";
        btnSua.UseVisualStyleBackColor = false;
        btnSua.Click += btnSua_Click;

        GiaoDien.DangNutNguyHiem(btnXoa);
        btnXoa.Location = new System.Drawing.Point(210, 8);
        btnXoa.Name = "btnXoa";
        btnXoa.Size = new System.Drawing.Size(90, 38);
        btnXoa.TabIndex = 12;
        btnXoa.Text = "Xóa";
        btnXoa.UseVisualStyleBackColor = false;
        btnXoa.Click += btnXoa_Click;

        GiaoDien.DangNutChinh(btnLuu);
        btnLuu.Location = new System.Drawing.Point(0, 56);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new System.Drawing.Size(160, 38);
        btnLuu.TabIndex = 13;
        btnLuu.Text = "Lưu";
        btnLuu.UseVisualStyleBackColor = false;
        btnLuu.Click += btnLuu_Click;

        GiaoDien.DangNutPhu(btnHuy);
        btnHuy.Location = new System.Drawing.Point(170, 56);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new System.Drawing.Size(130, 38);
        btnHuy.TabIndex = 14;
        btnHuy.Text = "Hủy bỏ";
        btnHuy.UseVisualStyleBackColor = false;
        btnHuy.Click += btnHuy_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmVoucher";
        Text = "Quản lý voucher";
        ((System.ComponentModel.ISupportInitialize)errLoi).EndInit();
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlTrai.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvVoucher).EndInit();
        pnlThanhCongCu.ResumeLayout(false);
        pnlThanhCongCu.PerformLayout();
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
    private DataGridView dgvVoucher;
    private Panel pnlThanhCongCu;
    private CheckBox chkChiConHan;
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
    private Label lblNhanTrangThai;
    private Label lblNhanNgayKetThuc;
    private Label lblNhanNgayBatDau;
    private Label lblDaDung;
    private Label lblNhanDaDung;
    private Label lblNhanSoLuong;
    private Label lblNhanDonToiThieu;
    private Label lblNhanGiaTriGiam;
    private Label lblNhanLoaiGiam;
    private Label lblNhanTenVoucher;
    private Label lblNhanMaCode;
    private Label lblMaVoucher;
    private Label lblNhanMa;
    private TextBox txtMoTa;
    private ComboBox cboTrangThai;
    private DateTimePicker dtpNgayKetThuc;
    private DateTimePicker dtpNgayBatDau;
    private TextBox txtSoLuong;
    private TextBox txtDonToiThieu;
    private TextBox txtGiaTriGiam;
    private ComboBox cboLoaiGiam;
    private TextBox txtTenVoucher;
    private TextBox txtMaCode;
}
