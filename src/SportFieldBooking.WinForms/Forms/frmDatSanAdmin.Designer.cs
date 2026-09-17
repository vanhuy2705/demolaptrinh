using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmDatSanAdmin
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
        pnlChanPhai = new Panel();
        btnLapHoaDon = new Button();
        btnHuyBooking = new Button();
        btnXemChiTiet = new Button();
        dgvLichTrongNgay = new DataGridView();
        lblTieuDeLich = new Label();
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
        lblDonGia = new Label();
        lblNhanDonGia = new Label();
        lblTrangThaiUuDai = new Label();
        lblTieuDeBangTien = new Label();
        pnlTrai = new Panel();
        pnlNutTrai = new Panel();
        btnDatSan = new Button();
        btnTinhTien = new Button();
        btnLamMoi = new Button();
        lblNhanGhiChu = new Label();
        lblNhanMaVoucher = new Label();
        lblNhanGioKetThuc = new Label();
        lblNhanGioBatDau = new Label();
        lblNhanNgayDat = new Label();
        lblThongTinSan = new Label();
        lblNhanSan = new Label();
        lblThongTinKhach = new Label();
        lblNhanKhachHang = new Label();
        lblCanhBaoTrung = new Label();
        txtMaVoucher = new TextBox();
        txtGhiChu = new TextBox();
        dtpGioKetThuc = new DateTimePicker();
        dtpGioBatDau = new DateTimePicker();
        dtpNgayDat = new DateTimePicker();
        cboSan = new ComboBox();
        cboKhachHang = new ComboBox();
        ((System.ComponentModel.ISupportInitialize)errLoi).BeginInit();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlPhai.SuspendLayout();
        pnlChanPhai.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLichTrongNgay).BeginInit();
        pnlBangTien.SuspendLayout();
        pnlTrai.SuspendLayout();
        pnlNutTrai.SuspendLayout();
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
        lblTieuDe.Size = new System.Drawing.Size(160, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Đặt sân";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(420, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Chọn khách, sân, khung giờ - hệ thống tự chống trùng lịch và tính tiền theo block 30 phút";

        pnlNoiDung.Controls.Add(pnlPhai);
        pnlNoiDung.Controls.Add(pnlTrai);
        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(0, 78);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(16);
        pnlNoiDung.Size = new System.Drawing.Size(1200, 642);
        pnlNoiDung.TabIndex = 1;

        pnlPhai.Controls.Add(pnlChanPhai);
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
        pnlBangTien.Controls.Add(lblDonGia);
        pnlBangTien.Controls.Add(lblNhanDonGia);
        pnlBangTien.Dock = DockStyle.Top;
        pnlBangTien.DoDayVien = 1;
        pnlBangTien.Location = new System.Drawing.Point(0, 0);
        pnlBangTien.MauVien = GiaoDien.Vien;
        pnlBangTien.Name = "pnlBangTien";
        pnlBangTien.Padding = new Padding(20, 16, 20, 16);
        pnlBangTien.Size = new System.Drawing.Size(728, 250);
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
        lblTrangThaiUuDai.Location = new System.Drawing.Point(240, 22);
        lblTrangThaiUuDai.Name = "lblTrangThaiUuDai";
        lblTrangThaiUuDai.Size = new System.Drawing.Size(200, 15);
        lblTrangThaiUuDai.TabIndex = 1;
        lblTrangThaiUuDai.Text = "Chưa tính";

        lblNhanDonGia.AutoSize = true;
        GiaoDien.DangNhan(lblNhanDonGia);
        lblNhanDonGia.Location = new System.Drawing.Point(20, 56);
        lblNhanDonGia.Name = "lblNhanDonGia";
        lblNhanDonGia.TabIndex = 2;
        lblNhanDonGia.Text = "Đơn giá thuê";

        lblDonGia.AutoSize = true;
        lblDonGia.Font = GiaoDien.ChuDam;
        lblDonGia.ForeColor = GiaoDien.Chu;
        lblDonGia.Location = new System.Drawing.Point(220, 55);
        lblDonGia.Name = "lblDonGia";
        lblDonGia.Size = new System.Drawing.Size(40, 17);
        lblDonGia.TabIndex = 3;
        lblDonGia.Text = "—";

        lblNhanSoGio.AutoSize = true;
        GiaoDien.DangNhan(lblNhanSoGio);
        lblNhanSoGio.Location = new System.Drawing.Point(20, 84);
        lblNhanSoGio.Name = "lblNhanSoGio";
        lblNhanSoGio.TabIndex = 4;
        lblNhanSoGio.Text = "Thời lượng";

        lblSoGio.AutoSize = true;
        lblSoGio.Font = GiaoDien.ChuDam;
        lblSoGio.ForeColor = GiaoDien.Chu;
        lblSoGio.Location = new System.Drawing.Point(220, 83);
        lblSoGio.Name = "lblSoGio";
        lblSoGio.Size = new System.Drawing.Size(20, 17);
        lblSoGio.TabIndex = 5;
        lblSoGio.Text = "—";

        lblNhanTienGoc.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTienGoc);
        lblNhanTienGoc.Location = new System.Drawing.Point(20, 112);
        lblNhanTienGoc.Name = "lblNhanTienGoc";
        lblNhanTienGoc.TabIndex = 6;
        lblNhanTienGoc.Text = "Tiền sân (chưa giảm)";

        lblTienGoc.AutoSize = true;
        lblTienGoc.Font = GiaoDien.ChuDam;
        lblTienGoc.ForeColor = GiaoDien.Chu;
        lblTienGoc.Location = new System.Drawing.Point(220, 111);
        lblTienGoc.Name = "lblTienGoc";
        lblTienGoc.Size = new System.Drawing.Size(30, 17);
        lblTienGoc.TabIndex = 7;
        lblTienGoc.Text = "0 đ";

        lblNhanUuDai.AutoSize = true;
        GiaoDien.DangNhan(lblNhanUuDai);
        lblNhanUuDai.Location = new System.Drawing.Point(20, 140);
        lblNhanUuDai.Name = "lblNhanUuDai";
        lblNhanUuDai.TabIndex = 8;
        lblNhanUuDai.Text = "Ưu đãi áp dụng";

        lblUuDai.AutoSize = true;
        lblUuDai.Font = GiaoDien.ChuDam;
        lblUuDai.ForeColor = GiaoDien.ThanhCong;
        lblUuDai.Location = new System.Drawing.Point(220, 139);
        lblUuDai.Name = "lblUuDai";
        lblUuDai.Size = new System.Drawing.Size(20, 17);
        lblUuDai.TabIndex = 9;
        lblUuDai.Text = "—";

        lblNhanTienGiam.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTienGiam);
        lblNhanTienGiam.Location = new System.Drawing.Point(20, 168);
        lblNhanTienGiam.Name = "lblNhanTienGiam";
        lblNhanTienGiam.TabIndex = 10;
        lblNhanTienGiam.Text = "Số tiền giảm";

        lblTienGiam.AutoSize = true;
        lblTienGiam.Font = GiaoDien.ChuDam;
        lblTienGiam.ForeColor = GiaoDien.ThanhCong;
        lblTienGiam.Location = new System.Drawing.Point(220, 167);
        lblTienGiam.Name = "lblTienGiam";
        lblTienGiam.Size = new System.Drawing.Size(30, 17);
        lblTienGiam.TabIndex = 11;
        lblTienGiam.Text = "0 đ";

        lblNhanTongTien.AutoSize = true;
        lblNhanTongTien.Font = GiaoDien.ChuDam;
        lblNhanTongTien.ForeColor = GiaoDien.Chu;
        lblNhanTongTien.Location = new System.Drawing.Point(20, 202);
        lblNhanTongTien.Name = "lblNhanTongTien";
        lblNhanTongTien.Size = new System.Drawing.Size(100, 17);
        lblNhanTongTien.TabIndex = 12;
        lblNhanTongTien.Text = "Tổng thanh toán";

        lblTongTien.AutoSize = true;
        lblTongTien.Font = GiaoDien.TieuDe;
        lblTongTien.ForeColor = GiaoDien.ChinhDam;
        lblTongTien.Location = new System.Drawing.Point(220, 190);
        lblTongTien.Name = "lblTongTien";
        lblTongTien.Size = new System.Drawing.Size(80, 31);
        lblTongTien.TabIndex = 13;
        lblTongTien.Text = "0 đ";

        lblTieuDeLich.Dock = DockStyle.Top;
        lblTieuDeLich.Height = 36;
        lblTieuDeLich.Padding = new Padding(0, 10, 0, 0);
        lblTieuDeLich.Font = GiaoDien.ChuLon;
        lblTieuDeLich.ForeColor = GiaoDien.Chu;
        lblTieuDeLich.Location = new System.Drawing.Point(0, 250);
        lblTieuDeLich.Name = "lblTieuDeLich";
        lblTieuDeLich.Size = new System.Drawing.Size(728, 36);
        lblTieuDeLich.TabIndex = 1;
        lblTieuDeLich.Text = "Lịch sân trong ngày";

        dgvLichTrongNgay.Dock = DockStyle.Fill;
        dgvLichTrongNgay.Location = new System.Drawing.Point(0, 0);
        dgvLichTrongNgay.Name = "dgvLichTrongNgay";
        dgvLichTrongNgay.Size = new System.Drawing.Size(728, 300);
        dgvLichTrongNgay.TabIndex = 2;
        dgvLichTrongNgay.SelectionChanged += dgvLichTrongNgay_SelectionChanged;

        pnlChanPhai.BackColor = GiaoDien.BeMat;
        pnlChanPhai.Controls.Add(btnLapHoaDon);
        pnlChanPhai.Controls.Add(btnHuyBooking);
        pnlChanPhai.Controls.Add(btnXemChiTiet);
        pnlChanPhai.Dock = DockStyle.Bottom;
        pnlChanPhai.Location = new System.Drawing.Point(0, 550);
        pnlChanPhai.Name = "pnlChanPhai";
        pnlChanPhai.Size = new System.Drawing.Size(728, 48);
        pnlChanPhai.TabIndex = 3;

        GiaoDien.DangNutPhu(btnXemChiTiet);
        btnXemChiTiet.Location = new System.Drawing.Point(0, 5);
        btnXemChiTiet.Name = "btnXemChiTiet";
        btnXemChiTiet.Size = new System.Drawing.Size(150, 38);
        btnXemChiTiet.TabIndex = 0;
        btnXemChiTiet.Text = "Chi tiết booking";
        btnXemChiTiet.UseVisualStyleBackColor = false;
        btnXemChiTiet.Click += btnXemChiTiet_Click;

        GiaoDien.DangNutNguyHiem(btnHuyBooking);
        btnHuyBooking.Location = new System.Drawing.Point(160, 5);
        btnHuyBooking.Name = "btnHuyBooking";
        btnHuyBooking.Size = new System.Drawing.Size(140, 38);
        btnHuyBooking.TabIndex = 1;
        btnHuyBooking.Text = "Hủy booking";
        btnHuyBooking.UseVisualStyleBackColor = false;
        btnHuyBooking.Click += btnHuyBooking_Click;

        GiaoDien.DangNutChinh(btnLapHoaDon);
        btnLapHoaDon.Location = new System.Drawing.Point(310, 5);
        btnLapHoaDon.Name = "btnLapHoaDon";
        btnLapHoaDon.Size = new System.Drawing.Size(170, 38);
        btnLapHoaDon.TabIndex = 2;
        btnLapHoaDon.Text = "Lập hóa đơn";
        btnLapHoaDon.UseVisualStyleBackColor = false;
        btnLapHoaDon.Click += btnLapHoaDon_Click;

        pnlTrai.BackColor = GiaoDien.BeMat;
        pnlTrai.Controls.Add(pnlNutTrai);
        pnlTrai.Controls.Add(lblNhanGhiChu);
        pnlTrai.Controls.Add(lblNhanMaVoucher);
        pnlTrai.Controls.Add(lblNhanGioKetThuc);
        pnlTrai.Controls.Add(lblNhanGioBatDau);
        pnlTrai.Controls.Add(lblNhanNgayDat);
        pnlTrai.Controls.Add(lblThongTinSan);
        pnlTrai.Controls.Add(lblNhanSan);
        pnlTrai.Controls.Add(lblThongTinKhach);
        pnlTrai.Controls.Add(lblNhanKhachHang);
        pnlTrai.Controls.Add(lblCanhBaoTrung);
        pnlTrai.Controls.Add(txtMaVoucher);
        pnlTrai.Controls.Add(txtGhiChu);
        pnlTrai.Controls.Add(dtpGioKetThuc);
        pnlTrai.Controls.Add(dtpGioBatDau);
        pnlTrai.Controls.Add(dtpNgayDat);
        pnlTrai.Controls.Add(cboSan);
        pnlTrai.Controls.Add(cboKhachHang);
        pnlTrai.Dock = DockStyle.Left;
        pnlTrai.Location = new System.Drawing.Point(16, 16);
        pnlTrai.Name = "pnlTrai";
        pnlTrai.Padding = new Padding(20, 18, 20, 18);
        pnlTrai.Size = new System.Drawing.Size(440, 610);
        pnlTrai.TabIndex = 0;

        lblNhanKhachHang.AutoSize = true;
        GiaoDien.DangNhan(lblNhanKhachHang);
        lblNhanKhachHang.Location = new System.Drawing.Point(20, 20);
        lblNhanKhachHang.Name = "lblNhanKhachHang";
        lblNhanKhachHang.TabIndex = 0;
        lblNhanKhachHang.Text = "Khách hàng (*)";

        cboKhachHang.DropDownStyle = ComboBoxStyle.DropDownList;
        cboKhachHang.Font = GiaoDien.ChuThuong;
        cboKhachHang.Location = new System.Drawing.Point(20, 40);
        cboKhachHang.Name = "cboKhachHang";
        cboKhachHang.Size = new System.Drawing.Size(400, 25);
        cboKhachHang.TabIndex = 0;
        cboKhachHang.SelectedIndexChanged += cboKhachHang_SelectedIndexChanged;

        lblThongTinKhach.AutoSize = true;
        lblThongTinKhach.Font = GiaoDien.ChuNho;
        lblThongTinKhach.ForeColor = GiaoDien.ChuPhu;
        lblThongTinKhach.Location = new System.Drawing.Point(20, 68);
        lblThongTinKhach.Name = "lblThongTinKhach";
        lblThongTinKhach.Size = new System.Drawing.Size(20, 15);
        lblThongTinKhach.TabIndex = 2;
        lblThongTinKhach.Text = "—";

        lblNhanSan.AutoSize = true;
        GiaoDien.DangNhan(lblNhanSan);
        lblNhanSan.Location = new System.Drawing.Point(20, 96);
        lblNhanSan.Name = "lblNhanSan";
        lblNhanSan.TabIndex = 3;
        lblNhanSan.Text = "Sân (*)";

        cboSan.DropDownStyle = ComboBoxStyle.DropDownList;
        cboSan.Font = GiaoDien.ChuThuong;
        cboSan.Location = new System.Drawing.Point(20, 116);
        cboSan.Name = "cboSan";
        cboSan.Size = new System.Drawing.Size(400, 25);
        cboSan.TabIndex = 1;
        cboSan.SelectedIndexChanged += cboSan_SelectedIndexChanged;

        lblThongTinSan.AutoSize = true;
        lblThongTinSan.Font = GiaoDien.ChuNho;
        lblThongTinSan.ForeColor = GiaoDien.ChuPhu;
        lblThongTinSan.Location = new System.Drawing.Point(20, 144);
        lblThongTinSan.Name = "lblThongTinSan";
        lblThongTinSan.Size = new System.Drawing.Size(20, 15);
        lblThongTinSan.TabIndex = 5;
        lblThongTinSan.Text = "—";

        lblNhanNgayDat.AutoSize = true;
        GiaoDien.DangNhan(lblNhanNgayDat);
        lblNhanNgayDat.Location = new System.Drawing.Point(20, 172);
        lblNhanNgayDat.Name = "lblNhanNgayDat";
        lblNhanNgayDat.TabIndex = 6;
        lblNhanNgayDat.Text = "Ngày đặt (*)";

        dtpNgayDat.Font = GiaoDien.ChuThuong;
        dtpNgayDat.Format = DateTimePickerFormat.Short;
        dtpNgayDat.Location = new System.Drawing.Point(20, 192);
        dtpNgayDat.Name = "dtpNgayDat";
        dtpNgayDat.Size = new System.Drawing.Size(400, 25);
        dtpNgayDat.TabIndex = 2;
        dtpNgayDat.ValueChanged += ThayDoiThoiGian;

        lblNhanGioBatDau.AutoSize = true;
        GiaoDien.DangNhan(lblNhanGioBatDau);
        lblNhanGioBatDau.Location = new System.Drawing.Point(20, 228);
        lblNhanGioBatDau.Name = "lblNhanGioBatDau";
        lblNhanGioBatDau.TabIndex = 8;
        lblNhanGioBatDau.Text = "Giờ bắt đầu (*)";

        dtpGioBatDau.Font = GiaoDien.ChuThuong;
        dtpGioBatDau.Location = new System.Drawing.Point(20, 248);
        dtpGioBatDau.Name = "dtpGioBatDau";
        dtpGioBatDau.Size = new System.Drawing.Size(190, 25);
        dtpGioBatDau.TabIndex = 3;
        dtpGioBatDau.ValueChanged += ThayDoiThoiGian;

        lblNhanGioKetThuc.AutoSize = true;
        GiaoDien.DangNhan(lblNhanGioKetThuc);
        lblNhanGioKetThuc.Location = new System.Drawing.Point(230, 228);
        lblNhanGioKetThuc.Name = "lblNhanGioKetThuc";
        lblNhanGioKetThuc.TabIndex = 10;
        lblNhanGioKetThuc.Text = "Giờ kết thúc (*)";

        dtpGioKetThuc.Font = GiaoDien.ChuThuong;
        dtpGioKetThuc.Location = new System.Drawing.Point(230, 248);
        dtpGioKetThuc.Name = "dtpGioKetThuc";
        dtpGioKetThuc.Size = new System.Drawing.Size(190, 25);
        dtpGioKetThuc.TabIndex = 4;
        dtpGioKetThuc.ValueChanged += ThayDoiThoiGian;

        lblNhanMaVoucher.AutoSize = true;
        GiaoDien.DangNhan(lblNhanMaVoucher);
        lblNhanMaVoucher.Location = new System.Drawing.Point(20, 286);
        lblNhanMaVoucher.Name = "lblNhanMaVoucher";
        lblNhanMaVoucher.TabIndex = 12;
        lblNhanMaVoucher.Text = "Mã voucher (nếu có)";

        txtMaVoucher.BackColor = GiaoDien.ManHinhNen;
        txtMaVoucher.BorderStyle = BorderStyle.FixedSingle;
        txtMaVoucher.CharacterCasing = CharacterCasing.Upper;
        txtMaVoucher.Font = GiaoDien.ChuThuong;
        txtMaVoucher.Location = new System.Drawing.Point(20, 306);
        txtMaVoucher.Name = "txtMaVoucher";
        txtMaVoucher.Size = new System.Drawing.Size(400, 25);
        txtMaVoucher.TabIndex = 5;
        txtMaVoucher.TextChanged += txtMaVoucher_TextChanged;

        lblNhanGhiChu.AutoSize = true;
        GiaoDien.DangNhan(lblNhanGhiChu);
        lblNhanGhiChu.Location = new System.Drawing.Point(20, 342);
        lblNhanGhiChu.Name = "lblNhanGhiChu";
        lblNhanGhiChu.TabIndex = 14;
        lblNhanGhiChu.Text = "Ghi chú";

        txtGhiChu.BackColor = GiaoDien.ManHinhNen;
        txtGhiChu.BorderStyle = BorderStyle.FixedSingle;
        txtGhiChu.Font = GiaoDien.ChuThuong;
        txtGhiChu.Location = new System.Drawing.Point(20, 362);
        txtGhiChu.Multiline = true;
        txtGhiChu.Name = "txtGhiChu";
        txtGhiChu.Size = new System.Drawing.Size(400, 70);
        txtGhiChu.TabIndex = 6;

        lblCanhBaoTrung.AutoSize = true;
        lblCanhBaoTrung.Font = GiaoDien.ChuNho;
        lblCanhBaoTrung.ForeColor = GiaoDien.NguyHiem;
        lblCanhBaoTrung.Location = new System.Drawing.Point(20, 444);
        lblCanhBaoTrung.MaximumSize = new System.Drawing.Size(400, 60);
        lblCanhBaoTrung.Name = "lblCanhBaoTrung";
        lblCanhBaoTrung.Size = new System.Drawing.Size(380, 30);
        lblCanhBaoTrung.TabIndex = 16;
        lblCanhBaoTrung.Text = "";
        lblCanhBaoTrung.Visible = false;

        pnlNutTrai.Controls.Add(btnDatSan);
        pnlNutTrai.Controls.Add(btnTinhTien);
        pnlNutTrai.Controls.Add(btnLamMoi);
        pnlNutTrai.Dock = DockStyle.Bottom;
        pnlNutTrai.Location = new System.Drawing.Point(20, 478);
        pnlNutTrai.Name = "pnlNutTrai";
        pnlNutTrai.Size = new System.Drawing.Size(400, 114);
        pnlNutTrai.TabIndex = 17;

        GiaoDien.DangNutChinh(btnDatSan);
        btnDatSan.Location = new System.Drawing.Point(0, 8);
        btnDatSan.Name = "btnDatSan";
        btnDatSan.Size = new System.Drawing.Size(400, 42);
        btnDatSan.TabIndex = 7;
        btnDatSan.Text = "Xác nhận đặt sân";
        btnDatSan.UseVisualStyleBackColor = false;
        btnDatSan.Click += btnDatSan_Click;

        GiaoDien.DangNutPhu(btnTinhTien);
        btnTinhTien.Location = new System.Drawing.Point(0, 60);
        btnTinhTien.Name = "btnTinhTien";
        btnTinhTien.Size = new System.Drawing.Size(190, 38);
        btnTinhTien.TabIndex = 8;
        btnTinhTien.Text = "Tính lại tiền";
        btnTinhTien.UseVisualStyleBackColor = false;
        btnTinhTien.Click += btnTinhTien_Click;

        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Location = new System.Drawing.Point(210, 60);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(190, 38);
        btnLamMoi.TabIndex = 9;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmDatSanAdmin";
        Text = "Đặt sân";
        ((System.ComponentModel.ISupportInitialize)errLoi).EndInit();
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlPhai.ResumeLayout(false);
        pnlPhai.PerformLayout();
        pnlChanPhai.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvLichTrongNgay).EndInit();
        pnlBangTien.ResumeLayout(false);
        pnlBangTien.PerformLayout();
        pnlTrai.ResumeLayout(false);
        pnlTrai.PerformLayout();
        pnlNutTrai.ResumeLayout(false);
        ResumeLayout(false);
    }

    private ErrorProvider errLoi;
    private Panel pnlDau;
    private Label lblMoTaTrang;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Panel pnlNoiDung;
    private Panel pnlPhai;
    private Panel pnlChanPhai;
    private Button btnLapHoaDon;
    private Button btnHuyBooking;
    private Button btnXemChiTiet;
    private DataGridView dgvLichTrongNgay;
    private Label lblTieuDeLich;
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
    private Label lblDonGia;
    private Label lblNhanDonGia;
    private Label lblTrangThaiUuDai;
    private Label lblTieuDeBangTien;
    private Panel pnlTrai;
    private Panel pnlNutTrai;
    private Button btnDatSan;
    private Button btnTinhTien;
    private Button btnLamMoi;
    private Label lblNhanGhiChu;
    private Label lblNhanMaVoucher;
    private Label lblNhanGioKetThuc;
    private Label lblNhanGioBatDau;
    private Label lblNhanNgayDat;
    private Label lblThongTinSan;
    private Label lblNhanSan;
    private Label lblThongTinKhach;
    private Label lblNhanKhachHang;
    private Label lblCanhBaoTrung;
    private TextBox txtMaVoucher;
    private TextBox txtGhiChu;
    private DateTimePicker dtpGioKetThuc;
    private DateTimePicker dtpGioBatDau;
    private DateTimePicker dtpNgayDat;
    private ComboBox cboSan;
    private ComboBox cboKhachHang;
}
