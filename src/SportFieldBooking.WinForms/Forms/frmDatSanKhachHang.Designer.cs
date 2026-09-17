using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmDatSanKhachHang
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
        dgvLichTrongNgay = new DataGridView();
        lblTieuDeLich = new Label();
        pnlTrai = new Panel();
        pnlNut = new Panel();
        btnVoucherCuaToi = new Button();
        btnDatSan = new Button();
        btnLamMoi = new Button();
        lblNhanGhiChu = new Label();
        lblNhanMaVoucher = new Label();
        lblNhanGioKetThuc = new Label();
        lblNhanGioBatDau = new Label();
        lblNhanNgayDat = new Label();
        lblThongTinSan = new Label();
        lblNhanSan = new Label();
        lblCanhBaoTrung = new Label();
        txtGhiChu = new TextBox();
        txtMaVoucher = new TextBox();
        dtpGioKetThuc = new DateTimePicker();
        dtpGioBatDau = new DateTimePicker();
        dtpNgayDat = new DateTimePicker();
        cboSan = new ComboBox();
        pnlBangTien = new RoundedPanel();
        lblTongTien = new Label();
        lblNhanTongTien = new Label();
        lblTienGiam = new Label();
        lblNhanTienGiam = new Label();
        lblUuDai = new Label();
        lblNhanUuDai = new Label();
        lblTienGoc = new Label();
        lblNhanTienGoc = new Label();
        lblSoGio = new Label();
        lblNhanSoGio = new Label();
        lblTrangThaiUuDai = new Label();
        lblTieuDeBangTien = new Label();
        ((System.ComponentModel.ISupportInitialize)errLoi).BeginInit();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlPhai.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLichTrongNgay).BeginInit();
        pnlTrai.SuspendLayout();
        pnlNut.SuspendLayout();
        pnlBangTien.SuspendLayout();
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
        picBieuTuong.TenBieuTuong = "calendar";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(76, 14);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(180, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Đặt sân trực tuyến";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(420, 15);
        lblMoTaTrang.Text = "Chọn sân và khung giờ còn trống, hệ thống tự tính tiền và cảnh báo trùng lịch";

        pnlNoiDung.Controls.Add(pnlPhai);
        pnlNoiDung.Controls.Add(pnlTrai);
        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(0, 78);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(16);
        pnlNoiDung.Size = new System.Drawing.Size(1200, 642);
        pnlNoiDung.TabIndex = 1;

        pnlPhai.BackColor = GiaoDien.BeMat;
        pnlPhai.Controls.Add(dgvLichTrongNgay);
        pnlPhai.Controls.Add(lblTieuDeLich);
        pnlPhai.Controls.Add(pnlBangTien);
        pnlPhai.Dock = DockStyle.Fill;
        pnlPhai.Location = new System.Drawing.Point(456, 16);
        pnlPhai.Name = "pnlPhai";
        pnlPhai.Padding = new Padding(0, 0, 0, 12);
        pnlPhai.Size = new System.Drawing.Size(728, 610);
        pnlPhai.TabIndex = 1;

        pnlBangTien.BackColor = GiaoDien.BeMat;
        pnlBangTien.BanKinh = 14;
        pnlBangTien.Controls.Add(lblTrangThaiUuDai);
        pnlBangTien.Controls.Add(lblTieuDeBangTien);
        pnlBangTien.Controls.Add(lblTongTien);
        pnlBangTien.Controls.Add(lblNhanTongTien);
        pnlBangTien.Controls.Add(lblTienGiam);
        pnlBangTien.Controls.Add(lblNhanTienGiam);
        pnlBangTien.Controls.Add(lblUuDai);
        pnlBangTien.Controls.Add(lblNhanUuDai);
        pnlBangTien.Controls.Add(lblTienGoc);
        pnlBangTien.Controls.Add(lblNhanTienGoc);
        pnlBangTien.Controls.Add(lblSoGio);
        pnlBangTien.Controls.Add(lblNhanSoGio);
        pnlBangTien.Dock = DockStyle.Top;
        pnlBangTien.DoDayVien = 1;
        pnlBangTien.Location = new System.Drawing.Point(0, 0);
        pnlBangTien.MauVien = GiaoDien.Vien;
        pnlBangTien.Name = "pnlBangTien";
        pnlBangTien.Padding = new Padding(20, 16, 20, 16);
        pnlBangTien.Size = new System.Drawing.Size(728, 230);
        pnlBangTien.TabIndex = 0;

        lblTieuDeBangTien.AutoSize = true;
        lblTieuDeBangTien.Font = GiaoDien.ChuLon;
        lblTieuDeBangTien.ForeColor = GiaoDien.Chu;
        lblTieuDeBangTien.Location = new System.Drawing.Point(20, 16);
        lblTieuDeBangTien.Name = "lblTieuDeBangTien";
        lblTieuDeBangTien.Size = new System.Drawing.Size(160, 25);
        lblTieuDeBangTien.TabIndex = 0;
        lblTieuDeBangTien.Text = "Bảng tính tiền";

        lblTrangThaiUuDai.AutoSize = true;
        lblTrangThaiUuDai.Font = GiaoDien.ChuNho;
        lblTrangThaiUuDai.ForeColor = GiaoDien.ChuPhu;
        lblTrangThaiUuDai.Location = new System.Drawing.Point(230, 22);
        lblTrangThaiUuDai.Name = "lblTrangThaiUuDai";
        lblTrangThaiUuDai.Size = new System.Drawing.Size(200, 15);
        lblTrangThaiUuDai.TabIndex = 1;
        lblTrangThaiUuDai.Text = "Chưa tính";

        lblNhanSoGio.AutoSize = true;
        GiaoDien.DangNhan(lblNhanSoGio);
        lblNhanSoGio.Location = new System.Drawing.Point(20, 58);
        lblNhanSoGio.Name = "lblNhanSoGio";
        lblNhanSoGio.TabIndex = 2;
        lblNhanSoGio.Text = "Thời lượng";

        lblSoGio.AutoSize = true;
        lblSoGio.Font = GiaoDien.ChuDam;
        lblSoGio.ForeColor = GiaoDien.Chu;
        lblSoGio.Location = new System.Drawing.Point(210, 57);
        lblSoGio.Name = "lblSoGio";
        lblSoGio.Size = new System.Drawing.Size(20, 17);
        lblSoGio.TabIndex = 3;
        lblSoGio.Text = "—";

        lblNhanTienGoc.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTienGoc);
        lblNhanTienGoc.Location = new System.Drawing.Point(20, 86);
        lblNhanTienGoc.Name = "lblNhanTienGoc";
        lblNhanTienGoc.TabIndex = 4;
        lblNhanTienGoc.Text = "Tiền sân";

        lblTienGoc.AutoSize = true;
        lblTienGoc.Font = GiaoDien.ChuDam;
        lblTienGoc.ForeColor = GiaoDien.Chu;
        lblTienGoc.Location = new System.Drawing.Point(210, 85);
        lblTienGoc.Name = "lblTienGoc";
        lblTienGoc.Size = new System.Drawing.Size(30, 17);
        lblTienGoc.TabIndex = 5;
        lblTienGoc.Text = "0 đ";

        lblNhanUuDai.AutoSize = true;
        GiaoDien.DangNhan(lblNhanUuDai);
        lblNhanUuDai.Location = new System.Drawing.Point(20, 114);
        lblNhanUuDai.Name = "lblNhanUuDai";
        lblNhanUuDai.TabIndex = 6;
        lblNhanUuDai.Text = "Ưu đãi";

        lblUuDai.AutoSize = true;
        lblUuDai.Font = GiaoDien.ChuDam;
        lblUuDai.ForeColor = GiaoDien.ThanhCong;
        lblUuDai.Location = new System.Drawing.Point(210, 113);
        lblUuDai.Name = "lblUuDai";
        lblUuDai.Size = new System.Drawing.Size(20, 17);
        lblUuDai.TabIndex = 7;
        lblUuDai.Text = "—";

        lblNhanTienGiam.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTienGiam);
        lblNhanTienGiam.Location = new System.Drawing.Point(20, 142);
        lblNhanTienGiam.Name = "lblNhanTienGiam";
        lblNhanTienGiam.TabIndex = 8;
        lblNhanTienGiam.Text = "Số tiền giảm";

        lblTienGiam.AutoSize = true;
        lblTienGiam.Font = GiaoDien.ChuDam;
        lblTienGiam.ForeColor = GiaoDien.ThanhCong;
        lblTienGiam.Location = new System.Drawing.Point(210, 141);
        lblTienGiam.Name = "lblTienGiam";
        lblTienGiam.Size = new System.Drawing.Size(30, 17);
        lblTienGiam.TabIndex = 9;
        lblTienGiam.Text = "0 đ";

        lblNhanTongTien.AutoSize = true;
        lblNhanTongTien.Font = GiaoDien.ChuDam;
        lblNhanTongTien.ForeColor = GiaoDien.Chu;
        lblNhanTongTien.Location = new System.Drawing.Point(20, 178);
        lblNhanTongTien.Name = "lblNhanTongTien";
        lblNhanTongTien.Size = new System.Drawing.Size(100, 17);
        lblNhanTongTien.TabIndex = 10;
        lblNhanTongTien.Text = "Tổng thanh toán";

        lblTongTien.AutoSize = true;
        lblTongTien.Font = GiaoDien.TieuDe;
        lblTongTien.ForeColor = GiaoDien.ChinhDam;
        lblTongTien.Location = new System.Drawing.Point(210, 166);
        lblTongTien.Name = "lblTongTien";
        lblTongTien.Size = new System.Drawing.Size(80, 31);
        lblTongTien.TabIndex = 11;
        lblTongTien.Text = "0 đ";

        lblTieuDeLich.AutoSize = true;
        lblTieuDeLich.Font = GiaoDien.ChuLon;
        lblTieuDeLich.ForeColor = GiaoDien.Chu;
        lblTieuDeLich.Location = new System.Drawing.Point(0, 242);
        lblTieuDeLich.Name = "lblTieuDeLich";
        lblTieuDeLich.Size = new System.Drawing.Size(260, 25);
        lblTieuDeLich.TabIndex = 1;
        lblTieuDeLich.Text = "Khung giờ đã có người đặt";

        dgvLichTrongNgay.Dock = DockStyle.Fill;
        dgvLichTrongNgay.Location = new System.Drawing.Point(0, 0);
        dgvLichTrongNgay.Name = "dgvLichTrongNgay";
        dgvLichTrongNgay.Size = new System.Drawing.Size(728, 300);
        dgvLichTrongNgay.TabIndex = 2;

        pnlTrai.BackColor = GiaoDien.BeMat;
        pnlTrai.Controls.Add(pnlNut);
        pnlTrai.Controls.Add(lblNhanGhiChu);
        pnlTrai.Controls.Add(lblNhanMaVoucher);
        pnlTrai.Controls.Add(lblNhanGioKetThuc);
        pnlTrai.Controls.Add(lblNhanGioBatDau);
        pnlTrai.Controls.Add(lblNhanNgayDat);
        pnlTrai.Controls.Add(lblThongTinSan);
        pnlTrai.Controls.Add(lblNhanSan);
        pnlTrai.Controls.Add(lblCanhBaoTrung);
        pnlTrai.Controls.Add(txtGhiChu);
        pnlTrai.Controls.Add(txtMaVoucher);
        pnlTrai.Controls.Add(dtpGioKetThuc);
        pnlTrai.Controls.Add(dtpGioBatDau);
        pnlTrai.Controls.Add(dtpNgayDat);
        pnlTrai.Controls.Add(cboSan);
        pnlTrai.Dock = DockStyle.Left;
        pnlTrai.Location = new System.Drawing.Point(16, 16);
        pnlTrai.Name = "pnlTrai";
        pnlTrai.Padding = new Padding(20, 18, 20, 18);
        pnlTrai.Size = new System.Drawing.Size(440, 610);
        pnlTrai.TabIndex = 0;

        lblNhanSan.AutoSize = true;
        GiaoDien.DangNhan(lblNhanSan);
        lblNhanSan.Location = new System.Drawing.Point(20, 20);
        lblNhanSan.Name = "lblNhanSan";
        lblNhanSan.TabIndex = 0;
        lblNhanSan.Text = "Chọn sân (*)";

        cboSan.DropDownStyle = ComboBoxStyle.DropDownList;
        cboSan.Font = GiaoDien.ChuThuong;
        cboSan.Location = new System.Drawing.Point(20, 40);
        cboSan.Name = "cboSan";
        cboSan.Size = new System.Drawing.Size(400, 25);
        cboSan.TabIndex = 0;
        cboSan.SelectedIndexChanged += cboSan_SelectedIndexChanged;

        lblThongTinSan.AutoSize = true;
        lblThongTinSan.Font = GiaoDien.ChuNho;
        lblThongTinSan.ForeColor = GiaoDien.ChuPhu;
        lblThongTinSan.Location = new System.Drawing.Point(20, 68);
        lblThongTinSan.Name = "lblThongTinSan";
        lblThongTinSan.Size = new System.Drawing.Size(20, 15);
        lblThongTinSan.TabIndex = 2;
        lblThongTinSan.Text = "—";

        lblNhanNgayDat.AutoSize = true;
        GiaoDien.DangNhan(lblNhanNgayDat);
        lblNhanNgayDat.Location = new System.Drawing.Point(20, 96);
        lblNhanNgayDat.Name = "lblNhanNgayDat";
        lblNhanNgayDat.TabIndex = 3;
        lblNhanNgayDat.Text = "Ngày đặt (*)";

        dtpNgayDat.Font = GiaoDien.ChuThuong;
        dtpNgayDat.Format = DateTimePickerFormat.Short;
        dtpNgayDat.Location = new System.Drawing.Point(20, 116);
        dtpNgayDat.Name = "dtpNgayDat";
        dtpNgayDat.Size = new System.Drawing.Size(400, 25);
        dtpNgayDat.TabIndex = 1;
        dtpNgayDat.ValueChanged += ThayDoiThoiGian;

        lblNhanGioBatDau.AutoSize = true;
        GiaoDien.DangNhan(lblNhanGioBatDau);
        lblNhanGioBatDau.Location = new System.Drawing.Point(20, 152);
        lblNhanGioBatDau.Name = "lblNhanGioBatDau";
        lblNhanGioBatDau.TabIndex = 5;
        lblNhanGioBatDau.Text = "Giờ bắt đầu (*)";

        dtpGioBatDau.Font = GiaoDien.ChuThuong;
        dtpGioBatDau.Location = new System.Drawing.Point(20, 172);
        dtpGioBatDau.Name = "dtpGioBatDau";
        dtpGioBatDau.Size = new System.Drawing.Size(190, 25);
        dtpGioBatDau.TabIndex = 2;
        dtpGioBatDau.ValueChanged += ThayDoiThoiGian;

        lblNhanGioKetThuc.AutoSize = true;
        GiaoDien.DangNhan(lblNhanGioKetThuc);
        lblNhanGioKetThuc.Location = new System.Drawing.Point(230, 152);
        lblNhanGioKetThuc.Name = "lblNhanGioKetThuc";
        lblNhanGioKetThuc.TabIndex = 7;
        lblNhanGioKetThuc.Text = "Giờ kết thúc (*)";

        dtpGioKetThuc.Font = GiaoDien.ChuThuong;
        dtpGioKetThuc.Location = new System.Drawing.Point(230, 172);
        dtpGioKetThuc.Name = "dtpGioKetThuc";
        dtpGioKetThuc.Size = new System.Drawing.Size(190, 25);
        dtpGioKetThuc.TabIndex = 3;
        dtpGioKetThuc.ValueChanged += ThayDoiThoiGian;

        lblNhanMaVoucher.AutoSize = true;
        GiaoDien.DangNhan(lblNhanMaVoucher);
        lblNhanMaVoucher.Location = new System.Drawing.Point(20, 210);
        lblNhanMaVoucher.Name = "lblNhanMaVoucher";
        lblNhanMaVoucher.TabIndex = 9;
        lblNhanMaVoucher.Text = "Mã voucher (nếu có)";

        txtMaVoucher.BackColor = GiaoDien.ManHinhNen;
        txtMaVoucher.BorderStyle = BorderStyle.FixedSingle;
        txtMaVoucher.CharacterCasing = CharacterCasing.Upper;
        txtMaVoucher.Font = GiaoDien.ChuThuong;
        txtMaVoucher.Location = new System.Drawing.Point(20, 230);
        txtMaVoucher.Name = "txtMaVoucher";
        txtMaVoucher.Size = new System.Drawing.Size(400, 25);
        txtMaVoucher.TabIndex = 4;
        txtMaVoucher.TextChanged += txtMaVoucher_TextChanged;

        lblNhanGhiChu.AutoSize = true;
        GiaoDien.DangNhan(lblNhanGhiChu);
        lblNhanGhiChu.Location = new System.Drawing.Point(20, 266);
        lblNhanGhiChu.Name = "lblNhanGhiChu";
        lblNhanGhiChu.TabIndex = 11;
        lblNhanGhiChu.Text = "Ghi chú";

        txtGhiChu.BackColor = GiaoDien.ManHinhNen;
        txtGhiChu.BorderStyle = BorderStyle.FixedSingle;
        txtGhiChu.Font = GiaoDien.ChuThuong;
        txtGhiChu.Location = new System.Drawing.Point(20, 286);
        txtGhiChu.Multiline = true;
        txtGhiChu.Name = "txtGhiChu";
        txtGhiChu.Size = new System.Drawing.Size(400, 70);
        txtGhiChu.TabIndex = 5;

        lblCanhBaoTrung.AutoSize = true;
        lblCanhBaoTrung.Font = GiaoDien.ChuNho;
        lblCanhBaoTrung.ForeColor = GiaoDien.NguyHiem;
        lblCanhBaoTrung.Location = new System.Drawing.Point(20, 368);
        lblCanhBaoTrung.MaximumSize = new System.Drawing.Size(400, 60);
        lblCanhBaoTrung.Name = "lblCanhBaoTrung";
        lblCanhBaoTrung.Size = new System.Drawing.Size(380, 30);
        lblCanhBaoTrung.TabIndex = 13;
        lblCanhBaoTrung.Text = "";
        lblCanhBaoTrung.Visible = false;

        pnlNut.Controls.Add(btnVoucherCuaToi);
        pnlNut.Controls.Add(btnDatSan);
        pnlNut.Controls.Add(btnLamMoi);
        pnlNut.Dock = DockStyle.Bottom;
        pnlNut.Location = new System.Drawing.Point(20, 436);
        pnlNut.Name = "pnlNut";
        pnlNut.Size = new System.Drawing.Size(400, 156);
        pnlNut.TabIndex = 14;

        GiaoDien.DangNutChinh(btnDatSan);
        btnDatSan.Location = new System.Drawing.Point(0, 8);
        btnDatSan.Name = "btnDatSan";
        btnDatSan.Size = new System.Drawing.Size(400, 44);
        btnDatSan.TabIndex = 6;
        btnDatSan.Text = "Xác nhận đặt sân";
        btnDatSan.UseVisualStyleBackColor = false;
        btnDatSan.Click += btnDatSan_Click;

        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Location = new System.Drawing.Point(0, 62);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(190, 38);
        btnLamMoi.TabIndex = 7;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        GiaoDien.DangNutPhu(btnVoucherCuaToi);
        btnVoucherCuaToi.Location = new System.Drawing.Point(210, 62);
        btnVoucherCuaToi.Name = "btnVoucherCuaToi";
        btnVoucherCuaToi.Size = new System.Drawing.Size(190, 38);
        btnVoucherCuaToi.TabIndex = 8;
        btnVoucherCuaToi.Text = "Voucher của tôi";
        btnVoucherCuaToi.UseVisualStyleBackColor = false;
        btnVoucherCuaToi.Click += btnVoucherCuaToi_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmDatSanKhachHang";
        Text = "Đặt sân";
        ((System.ComponentModel.ISupportInitialize)errLoi).EndInit();
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlPhai.ResumeLayout(false);
        pnlPhai.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLichTrongNgay).EndInit();
        pnlTrai.ResumeLayout(false);
        pnlTrai.PerformLayout();
        pnlNut.ResumeLayout(false);
        pnlBangTien.ResumeLayout(false);
        pnlBangTien.PerformLayout();
        ResumeLayout(false);
    }

    private ErrorProvider errLoi;
    private Panel pnlDau;
    private Label lblMoTaTrang;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Panel pnlNoiDung;
    private Panel pnlPhai;
    private DataGridView dgvLichTrongNgay;
    private Label lblTieuDeLich;
    private Panel pnlTrai;
    private Panel pnlNut;
    private Button btnVoucherCuaToi;
    private Button btnDatSan;
    private Button btnLamMoi;
    private Label lblNhanGhiChu;
    private Label lblNhanMaVoucher;
    private Label lblNhanGioKetThuc;
    private Label lblNhanGioBatDau;
    private Label lblNhanNgayDat;
    private Label lblThongTinSan;
    private Label lblNhanSan;
    private Label lblCanhBaoTrung;
    private TextBox txtGhiChu;
    private TextBox txtMaVoucher;
    private DateTimePicker dtpGioKetThuc;
    private DateTimePicker dtpGioBatDau;
    private DateTimePicker dtpNgayDat;
    private ComboBox cboSan;
    private RoundedPanel pnlBangTien;
    private Label lblTongTien;
    private Label lblNhanTongTien;
    private Label lblTienGiam;
    private Label lblNhanTienGiam;
    private Label lblUuDai;
    private Label lblNhanUuDai;
    private Label lblTienGoc;
    private Label lblNhanTienGoc;
    private Label lblSoGio;
    private Label lblNhanSoGio;
    private Label lblTrangThaiUuDai;
    private Label lblTieuDeBangTien;
}
