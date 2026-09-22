using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmKhuyenMai
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
        dgvKhuyenMai = new DataGridView();
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
        lblNhanMoTa = new Label();
        lblNhanTrangThai = new Label();
        lblNhanNgayKetThuc = new Label();
        lblNhanNgayBatDau = new Label();
        lblNhanLoaiKhuyenMai = new Label();
        lblNhanPhanTramGiam = new Label();
        lblNhanTenKM = new Label();
        lblMaKM = new Label();
        lblNhanMa = new Label();
        txtMoTa = new TextBox();
        cboTrangThai = new ComboBox();
        dtpNgayKetThuc = new DateTimePicker();
        dtpNgayBatDau = new DateTimePicker();
        chkApDungCuoiTuan = new CheckBox();
        txtLoaiKhuyenMai = new TextBox();
        txtPhanTramGiam = new TextBox();
        txtTenKM = new TextBox();
        ((System.ComponentModel.ISupportInitialize)errLoi).BeginInit();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlTrai.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvKhuyenMai).BeginInit();
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
        picBieuTuong.TenBieuTuong = "khuyenmai";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(76, 14);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(220, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Chương trình khuyến mãi";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(420, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Giảm giá dịp đặc biệt, tự động áp dụng theo khoảng ngày (ưu tiên sau voucher)";

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
        pnlTrai.Controls.Add(dgvKhuyenMai);
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

        dgvKhuyenMai.Dock = DockStyle.Fill;
        dgvKhuyenMai.Location = new System.Drawing.Point(0, 58);
        dgvKhuyenMai.Name = "dgvKhuyenMai";
        dgvKhuyenMai.Size = new System.Drawing.Size(788, 552);
        dgvKhuyenMai.TabIndex = 1;
        dgvKhuyenMai.SelectionChanged += dgvKhuyenMai_SelectionChanged;

        pnlPhai.BackColor = GiaoDien.BeMat;
        pnlPhai.Controls.Add(pnlNut);
        pnlPhai.Controls.Add(lblNhanMoTa);
        pnlPhai.Controls.Add(lblNhanTrangThai);
        pnlPhai.Controls.Add(lblNhanNgayKetThuc);
        pnlPhai.Controls.Add(lblNhanNgayBatDau);
        pnlPhai.Controls.Add(lblNhanLoaiKhuyenMai);
        pnlPhai.Controls.Add(lblNhanPhanTramGiam);
        pnlPhai.Controls.Add(lblNhanTenKM);
        pnlPhai.Controls.Add(lblMaKM);
        pnlPhai.Controls.Add(lblNhanMa);
        pnlPhai.Controls.Add(txtMoTa);
        pnlPhai.Controls.Add(cboTrangThai);
        pnlPhai.Controls.Add(dtpNgayKetThuc);
        pnlPhai.Controls.Add(dtpNgayBatDau);
        pnlPhai.Controls.Add(chkApDungCuoiTuan);
        pnlPhai.Controls.Add(txtLoaiKhuyenMai);
        pnlPhai.Controls.Add(txtPhanTramGiam);
        pnlPhai.Controls.Add(txtTenKM);
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
        lblNhanMa.Text = "Mã chương trình";

        lblMaKM.AutoSize = true;
        lblMaKM.Font = GiaoDien.ChuDam;
        lblMaKM.ForeColor = GiaoDien.ChinhDam;
        lblMaKM.Location = new System.Drawing.Point(160, 19);
        lblMaKM.Name = "lblMaKM";
        lblMaKM.Size = new System.Drawing.Size(20, 17);
        lblMaKM.TabIndex = 1;
        lblMaKM.Text = "—";

        lblNhanTenKM.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTenKM);
        lblNhanTenKM.Location = new System.Drawing.Point(20, 58);
        lblNhanTenKM.Name = "lblNhanTenKM";
        lblNhanTenKM.TabIndex = 2;
        lblNhanTenKM.Text = "Tên chương trình (*)";

        txtTenKM.BackColor = GiaoDien.ManHinhNen;
        txtTenKM.BorderStyle = BorderStyle.FixedSingle;
        txtTenKM.Font = GiaoDien.ChuThuong;
        txtTenKM.Location = new System.Drawing.Point(20, 78);
        txtTenKM.Name = "txtTenKM";
        txtTenKM.ReadOnly = true;
        txtTenKM.Size = new System.Drawing.Size(340, 25);
        txtTenKM.TabIndex = 0;

        lblNhanLoaiKhuyenMai.AutoSize = true;
        GiaoDien.DangNhan(lblNhanLoaiKhuyenMai);
        lblNhanLoaiKhuyenMai.Location = new System.Drawing.Point(20, 116);
        lblNhanLoaiKhuyenMai.Name = "lblNhanLoaiKhuyenMai";
        lblNhanLoaiKhuyenMai.TabIndex = 4;
        lblNhanLoaiKhuyenMai.Text = "Loại (Lễ tết, Khai trương...)";

        txtLoaiKhuyenMai.BackColor = GiaoDien.ManHinhNen;
        txtLoaiKhuyenMai.BorderStyle = BorderStyle.FixedSingle;
        txtLoaiKhuyenMai.Font = GiaoDien.ChuThuong;
        txtLoaiKhuyenMai.Location = new System.Drawing.Point(20, 136);
        txtLoaiKhuyenMai.Name = "txtLoaiKhuyenMai";
        txtLoaiKhuyenMai.ReadOnly = true;
        txtLoaiKhuyenMai.Size = new System.Drawing.Size(340, 25);
        txtLoaiKhuyenMai.TabIndex = 1;

        lblNhanPhanTramGiam.AutoSize = true;
        GiaoDien.DangNhan(lblNhanPhanTramGiam);
        lblNhanPhanTramGiam.Location = new System.Drawing.Point(20, 174);
        lblNhanPhanTramGiam.Name = "lblNhanPhanTramGiam";
        lblNhanPhanTramGiam.TabIndex = 6;
        lblNhanPhanTramGiam.Text = "Phần trăm giảm (*)";

        txtPhanTramGiam.BackColor = GiaoDien.ManHinhNen;
        txtPhanTramGiam.BorderStyle = BorderStyle.FixedSingle;
        txtPhanTramGiam.Font = GiaoDien.ChuThuong;
        txtPhanTramGiam.Location = new System.Drawing.Point(20, 194);
        txtPhanTramGiam.Name = "txtPhanTramGiam";
        txtPhanTramGiam.ReadOnly = true;
        txtPhanTramGiam.Size = new System.Drawing.Size(340, 25);
        txtPhanTramGiam.TabIndex = 2;

        chkApDungCuoiTuan.AutoSize = true;
        chkApDungCuoiTuan.Enabled = false;
        chkApDungCuoiTuan.Font = GiaoDien.ChuThuong;
        chkApDungCuoiTuan.ForeColor = GiaoDien.Chu;
        chkApDungCuoiTuan.Location = new System.Drawing.Point(20, 232);
        chkApDungCuoiTuan.Name = "chkApDungCuoiTuan";
        chkApDungCuoiTuan.Size = new System.Drawing.Size(200, 20);
        chkApDungCuoiTuan.TabIndex = 3;
        chkApDungCuoiTuan.Text = "Chỉ áp dụng Thứ Bảy / Chủ Nhật";

        lblNhanNgayBatDau.AutoSize = true;
        GiaoDien.DangNhan(lblNhanNgayBatDau);
        lblNhanNgayBatDau.Location = new System.Drawing.Point(20, 268);
        lblNhanNgayBatDau.Name = "lblNhanNgayBatDau";
        lblNhanNgayBatDau.TabIndex = 8;
        lblNhanNgayBatDau.Text = "Từ ngày";

        dtpNgayBatDau.Enabled = false;
        dtpNgayBatDau.Font = GiaoDien.ChuThuong;
        dtpNgayBatDau.Format = DateTimePickerFormat.Short;
        dtpNgayBatDau.Location = new System.Drawing.Point(20, 288);
        dtpNgayBatDau.Name = "dtpNgayBatDau";
        dtpNgayBatDau.Size = new System.Drawing.Size(160, 25);
        dtpNgayBatDau.TabIndex = 4;

        lblNhanNgayKetThuc.AutoSize = true;
        GiaoDien.DangNhan(lblNhanNgayKetThuc);
        lblNhanNgayKetThuc.Location = new System.Drawing.Point(200, 268);
        lblNhanNgayKetThuc.Name = "lblNhanNgayKetThuc";
        lblNhanNgayKetThuc.TabIndex = 10;
        lblNhanNgayKetThuc.Text = "Đến ngày";

        dtpNgayKetThuc.Enabled = false;
        dtpNgayKetThuc.Font = GiaoDien.ChuThuong;
        dtpNgayKetThuc.Format = DateTimePickerFormat.Short;
        dtpNgayKetThuc.Location = new System.Drawing.Point(200, 288);
        dtpNgayKetThuc.Name = "dtpNgayKetThuc";
        dtpNgayKetThuc.Size = new System.Drawing.Size(160, 25);
        dtpNgayKetThuc.TabIndex = 5;

        lblNhanTrangThai.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTrangThai);
        lblNhanTrangThai.Location = new System.Drawing.Point(20, 330);
        lblNhanTrangThai.Name = "lblNhanTrangThai";
        lblNhanTrangThai.TabIndex = 12;
        lblNhanTrangThai.Text = "Trạng thái";

        cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTrangThai.Enabled = false;
        cboTrangThai.Font = GiaoDien.ChuThuong;
        cboTrangThai.Location = new System.Drawing.Point(20, 350);
        cboTrangThai.Name = "cboTrangThai";
        cboTrangThai.Size = new System.Drawing.Size(160, 25);
        cboTrangThai.TabIndex = 6;

        lblNhanMoTa.AutoSize = true;
        GiaoDien.DangNhan(lblNhanMoTa);
        lblNhanMoTa.Location = new System.Drawing.Point(20, 388);
        lblNhanMoTa.Name = "lblNhanMoTa";
        lblNhanMoTa.TabIndex = 14;
        lblNhanMoTa.Text = "Mô tả";

        txtMoTa.BackColor = GiaoDien.ManHinhNen;
        txtMoTa.BorderStyle = BorderStyle.FixedSingle;
        txtMoTa.Font = GiaoDien.ChuThuong;
        txtMoTa.Location = new System.Drawing.Point(20, 408);
        txtMoTa.Multiline = true;
        txtMoTa.Name = "txtMoTa";
        txtMoTa.ReadOnly = true;
        txtMoTa.Size = new System.Drawing.Size(340, 60);
        txtMoTa.TabIndex = 7;

        pnlNut.Controls.Add(btnHuy);
        pnlNut.Controls.Add(btnLuu);
        pnlNut.Controls.Add(btnXoa);
        pnlNut.Controls.Add(btnSua);
        pnlNut.Controls.Add(btnThem);
        pnlNut.Location = new System.Drawing.Point(20, 490);
        pnlNut.Name = "pnlNut";
        pnlNut.Size = new System.Drawing.Size(340, 116);
        pnlNut.TabIndex = 16;

        GiaoDien.DangNutChinh(btnThem);
        btnThem.Location = new System.Drawing.Point(0, 8);
        btnThem.Name = "btnThem";
        btnThem.Size = new System.Drawing.Size(100, 38);
        btnThem.TabIndex = 8;
        btnThem.Text = "Thêm";
        btnThem.UseVisualStyleBackColor = false;
        btnThem.Click += btnThem_Click;

        GiaoDien.DangNutPhu(btnSua);
        btnSua.Location = new System.Drawing.Point(110, 8);
        btnSua.Name = "btnSua";
        btnSua.Size = new System.Drawing.Size(90, 38);
        btnSua.TabIndex = 9;
        btnSua.Text = "Sửa";
        btnSua.UseVisualStyleBackColor = false;
        btnSua.Click += btnSua_Click;

        GiaoDien.DangNutNguyHiem(btnXoa);
        btnXoa.Location = new System.Drawing.Point(210, 8);
        btnXoa.Name = "btnXoa";
        btnXoa.Size = new System.Drawing.Size(90, 38);
        btnXoa.TabIndex = 10;
        btnXoa.Text = "Xóa";
        btnXoa.UseVisualStyleBackColor = false;
        btnXoa.Click += btnXoa_Click;

        GiaoDien.DangNutChinh(btnLuu);
        btnLuu.Location = new System.Drawing.Point(0, 56);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new System.Drawing.Size(160, 38);
        btnLuu.TabIndex = 11;
        btnLuu.Text = "Lưu";
        btnLuu.UseVisualStyleBackColor = false;
        btnLuu.Click += btnLuu_Click;

        GiaoDien.DangNutPhu(btnHuy);
        btnHuy.Location = new System.Drawing.Point(170, 56);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new System.Drawing.Size(130, 38);
        btnHuy.TabIndex = 12;
        btnHuy.Text = "Hủy bỏ";
        btnHuy.UseVisualStyleBackColor = false;
        btnHuy.Click += btnHuy_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmKhuyenMai";
        Text = "Chương trình khuyến mãi";
        ((System.ComponentModel.ISupportInitialize)errLoi).EndInit();
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlTrai.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvKhuyenMai).EndInit();
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
    private DataGridView dgvKhuyenMai;
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
    private Label lblNhanTrangThai;
    private Label lblNhanNgayKetThuc;
    private Label lblNhanNgayBatDau;
    private Label lblNhanLoaiKhuyenMai;
    private Label lblNhanPhanTramGiam;
    private Label lblNhanTenKM;
    private Label lblMaKM;
    private Label lblNhanMa;
    private TextBox txtMoTa;
    private ComboBox cboTrangThai;
    private DateTimePicker dtpNgayKetThuc;
    private DateTimePicker dtpNgayBatDau;
    private CheckBox chkApDungCuoiTuan;
    private TextBox txtLoaiKhuyenMai;
    private TextBox txtPhanTramGiam;
    private TextBox txtTenKM;
}
