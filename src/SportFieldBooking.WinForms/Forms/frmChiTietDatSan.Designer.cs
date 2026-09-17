using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmChiTietDatSan
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
        pnlDau = new Panel();
        lblTieuDe = new Label();
        lblMaDat = new Label();
        picBieuTuong = new IconBox();
        pnlThan = new Panel();
        lblTienTamTinh = new Label();
        lblNhanTienTamTinh = new Label();
        lblTienSan = new Label();
        lblNhanTienSan = new Label();
        lblDonGia = new Label();
        lblNhanDonGia = new Label();
        lblNhanGhiChu = new Label();
        lblNhanGioKetThuc = new Label();
        lblNhanGioBatDau = new Label();
        lblNhanNgayDat = new Label();
        lblTrangThai = new Label();
        lblNhanTrangThai = new Label();
        lblSan = new Label();
        lblNhanSan = new Label();
        lblKhachHang = new Label();
        lblNhanKhachHang = new Label();
        txtGhiChu = new TextBox();
        dtpGioKetThuc = new DateTimePicker();
        dtpGioBatDau = new DateTimePicker();
        dtpNgayDat = new DateTimePicker();
        pnlChan = new Panel();
        btnDong = new Button();
        btnHuyBooking = new Button();
        btnLuu = new Button();
        pnlDau.SuspendLayout();
        pnlThan.SuspendLayout();
        pnlChan.SuspendLayout();
        SuspendLayout();

        pnlDau.BackColor = GiaoDien.ThanhBen;
        pnlDau.Controls.Add(lblTieuDe);
        pnlDau.Controls.Add(lblMaDat);
        pnlDau.Controls.Add(picBieuTuong);
        pnlDau.Dock = DockStyle.Top;
        pnlDau.Location = new System.Drawing.Point(0, 0);
        pnlDau.Name = "pnlDau";
        pnlDau.Size = new System.Drawing.Size(560, 80);
        pnlDau.TabIndex = 0;

        picBieuTuong.Location = new System.Drawing.Point(22, 18);
        picBieuTuong.MauBieuTuong = GiaoDien.Chinh;
        picBieuTuong.Name = "picBieuTuong";
        picBieuTuong.Size = new System.Drawing.Size(44, 44);
        picBieuTuong.TabIndex = 0;
        picBieuTuong.TenBieuTuong = "calendar";

        lblMaDat.AutoSize = true;
        lblMaDat.Font = GiaoDien.ChuLon;
        lblMaDat.ForeColor = Color.White;
        lblMaDat.Location = new System.Drawing.Point(78, 16);
        lblMaDat.Name = "lblMaDat";
        lblMaDat.Size = new System.Drawing.Size(60, 25);
        lblMaDat.TabIndex = 1;
        lblMaDat.Text = "#";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.ChuNho;
        lblTieuDe.ForeColor = Color.FromArgb(148, 163, 184);
        lblTieuDe.Location = new System.Drawing.Point(80, 46);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(160, 15);
        lblTieuDe.TabIndex = 2;
        lblTieuDe.Text = "Chi tiết đặt sân";

        pnlThan.BackColor = GiaoDien.BeMat;
        pnlThan.Controls.Add(lblTienTamTinh);
        pnlThan.Controls.Add(lblNhanTienTamTinh);
        pnlThan.Controls.Add(lblTienSan);
        pnlThan.Controls.Add(lblNhanTienSan);
        pnlThan.Controls.Add(lblDonGia);
        pnlThan.Controls.Add(lblNhanDonGia);
        pnlThan.Controls.Add(lblNhanGhiChu);
        pnlThan.Controls.Add(lblNhanGioKetThuc);
        pnlThan.Controls.Add(lblNhanGioBatDau);
        pnlThan.Controls.Add(lblNhanNgayDat);
        pnlThan.Controls.Add(lblTrangThai);
        pnlThan.Controls.Add(lblNhanTrangThai);
        pnlThan.Controls.Add(lblSan);
        pnlThan.Controls.Add(lblNhanSan);
        pnlThan.Controls.Add(lblKhachHang);
        pnlThan.Controls.Add(lblNhanKhachHang);
        pnlThan.Controls.Add(txtGhiChu);
        pnlThan.Controls.Add(dtpGioKetThuc);
        pnlThan.Controls.Add(dtpGioBatDau);
        pnlThan.Controls.Add(dtpNgayDat);
        pnlThan.Dock = DockStyle.Fill;
        pnlThan.Location = new System.Drawing.Point(0, 80);
        pnlThan.Name = "pnlThan";
        pnlThan.Size = new System.Drawing.Size(560, 372);
        pnlThan.TabIndex = 1;

        lblNhanKhachHang.AutoSize = true;
        GiaoDien.DangNhan(lblNhanKhachHang);
        lblNhanKhachHang.Location = new System.Drawing.Point(32, 24);
        lblNhanKhachHang.Name = "lblNhanKhachHang";
        lblNhanKhachHang.TabIndex = 0;
        lblNhanKhachHang.Text = "Khách hàng";

        lblKhachHang.AutoSize = true;
        lblKhachHang.Font = GiaoDien.ChuDam;
        lblKhachHang.ForeColor = GiaoDien.Chu;
        lblKhachHang.Location = new System.Drawing.Point(180, 23);
        lblKhachHang.Name = "lblKhachHang";
        lblKhachHang.Size = new System.Drawing.Size(80, 17);
        lblKhachHang.TabIndex = 1;
        lblKhachHang.Text = "Khách hàng";

        lblNhanSan.AutoSize = true;
        GiaoDien.DangNhan(lblNhanSan);
        lblNhanSan.Location = new System.Drawing.Point(32, 52);
        lblNhanSan.Name = "lblNhanSan";
        lblNhanSan.TabIndex = 2;
        lblNhanSan.Text = "Sân";

        lblSan.AutoSize = true;
        lblSan.Font = GiaoDien.ChuDam;
        lblSan.ForeColor = GiaoDien.Chu;
        lblSan.Location = new System.Drawing.Point(180, 51);
        lblSan.Name = "lblSan";
        lblSan.Size = new System.Drawing.Size(40, 17);
        lblSan.TabIndex = 3;
        lblSan.Text = "Sân";

        lblNhanTrangThai.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTrangThai);
        lblNhanTrangThai.Location = new System.Drawing.Point(32, 80);
        lblNhanTrangThai.Name = "lblNhanTrangThai";
        lblNhanTrangThai.TabIndex = 4;
        lblNhanTrangThai.Text = "Trạng thái";

        lblTrangThai.AutoSize = true;
        lblTrangThai.Font = GiaoDien.ChuDam;
        lblTrangThai.ForeColor = GiaoDien.CanhBao;
        lblTrangThai.Location = new System.Drawing.Point(180, 79);
        lblTrangThai.Name = "lblTrangThai";
        lblTrangThai.Size = new System.Drawing.Size(70, 17);
        lblTrangThai.TabIndex = 5;
        lblTrangThai.Text = "Đã đặt";

        lblNhanNgayDat.AutoSize = true;
        GiaoDien.DangNhan(lblNhanNgayDat);
        lblNhanNgayDat.Location = new System.Drawing.Point(32, 116);
        lblNhanNgayDat.Name = "lblNhanNgayDat";
        lblNhanNgayDat.TabIndex = 6;
        lblNhanNgayDat.Text = "Ngày đặt";

        dtpNgayDat.Font = GiaoDien.ChuThuong;
        dtpNgayDat.Format = DateTimePickerFormat.Short;
        dtpNgayDat.Location = new System.Drawing.Point(180, 112);
        dtpNgayDat.Name = "dtpNgayDat";
        dtpNgayDat.Size = new System.Drawing.Size(330, 25);
        dtpNgayDat.TabIndex = 0;
        dtpNgayDat.ValueChanged += ThayDoiThoiGian;

        lblNhanGioBatDau.AutoSize = true;
        GiaoDien.DangNhan(lblNhanGioBatDau);
        lblNhanGioBatDau.Location = new System.Drawing.Point(32, 152);
        lblNhanGioBatDau.Name = "lblNhanGioBatDau";
        lblNhanGioBatDau.TabIndex = 8;
        lblNhanGioBatDau.Text = "Giờ bắt đầu";

        dtpGioBatDau.Font = GiaoDien.ChuThuong;
        dtpGioBatDau.Location = new System.Drawing.Point(180, 148);
        dtpGioBatDau.Name = "dtpGioBatDau";
        dtpGioBatDau.Size = new System.Drawing.Size(155, 25);
        dtpGioBatDau.TabIndex = 1;
        dtpGioBatDau.ValueChanged += ThayDoiThoiGian;

        lblNhanGioKetThuc.AutoSize = true;
        GiaoDien.DangNhan(lblNhanGioKetThuc);
        lblNhanGioKetThuc.Location = new System.Drawing.Point(355, 152);
        lblNhanGioKetThuc.Name = "lblNhanGioKetThuc";
        lblNhanGioKetThuc.TabIndex = 10;
        lblNhanGioKetThuc.Text = "Giờ kết thúc";

        dtpGioKetThuc.Font = GiaoDien.ChuThuong;
        dtpGioKetThuc.Location = new System.Drawing.Point(355, 148);
        dtpGioKetThuc.Name = "dtpGioKetThuc";
        dtpGioKetThuc.Size = new System.Drawing.Size(155, 25);
        dtpGioKetThuc.TabIndex = 2;
        dtpGioKetThuc.ValueChanged += ThayDoiThoiGian;

        lblNhanGhiChu.AutoSize = true;
        GiaoDien.DangNhan(lblNhanGhiChu);
        lblNhanGhiChu.Location = new System.Drawing.Point(32, 190);
        lblNhanGhiChu.Name = "lblNhanGhiChu";
        lblNhanGhiChu.TabIndex = 12;
        lblNhanGhiChu.Text = "Ghi chú";

        txtGhiChu.BackColor = GiaoDien.ManHinhNen;
        txtGhiChu.BorderStyle = BorderStyle.FixedSingle;
        txtGhiChu.Font = GiaoDien.ChuThuong;
        txtGhiChu.Location = new System.Drawing.Point(180, 188);
        txtGhiChu.Multiline = true;
        txtGhiChu.Name = "txtGhiChu";
        txtGhiChu.Size = new System.Drawing.Size(330, 60);
        txtGhiChu.TabIndex = 3;

        lblNhanDonGia.AutoSize = true;
        GiaoDien.DangNhan(lblNhanDonGia);
        lblNhanDonGia.Location = new System.Drawing.Point(32, 262);
        lblNhanDonGia.Name = "lblNhanDonGia";
        lblNhanDonGia.TabIndex = 14;
        lblNhanDonGia.Text = "Đơn giá thuê";

        lblDonGia.AutoSize = true;
        lblDonGia.Font = GiaoDien.ChuDam;
        lblDonGia.ForeColor = GiaoDien.Chu;
        lblDonGia.Location = new System.Drawing.Point(180, 261);
        lblDonGia.Name = "lblDonGia";
        lblDonGia.Size = new System.Drawing.Size(60, 17);
        lblDonGia.TabIndex = 15;
        lblDonGia.Text = "0 đ/giờ";

        lblNhanTienSan.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTienSan);
        lblNhanTienSan.Location = new System.Drawing.Point(32, 290);
        lblNhanTienSan.Name = "lblNhanTienSan";
        lblNhanTienSan.TabIndex = 16;
        lblNhanTienSan.Text = "Tiền sân hiện tại";

        lblTienSan.AutoSize = true;
        lblTienSan.Font = GiaoDien.ChuDam;
        lblTienSan.ForeColor = GiaoDien.Chu;
        lblTienSan.Location = new System.Drawing.Point(180, 289);
        lblTienSan.Name = "lblTienSan";
        lblTienSan.Size = new System.Drawing.Size(30, 17);
        lblTienSan.TabIndex = 17;
        lblTienSan.Text = "0 đ";

        lblNhanTienTamTinh.AutoSize = true;
        lblNhanTienTamTinh.Font = GiaoDien.ChuDam;
        lblNhanTienTamTinh.ForeColor = GiaoDien.ChinhDam;
        lblNhanTienTamTinh.Location = new System.Drawing.Point(32, 326);
        lblNhanTienTamTinh.Name = "lblNhanTienTamTinh";
        lblNhanTienTamTinh.Size = new System.Drawing.Size(120, 17);
        lblNhanTienTamTinh.TabIndex = 18;
        lblNhanTienTamTinh.Text = "Tiền tạm tính mới";

        lblTienTamTinh.AutoSize = true;
        lblTienTamTinh.Font = GiaoDien.ChuDam;
        lblTienTamTinh.ForeColor = GiaoDien.ChinhDam;
        lblTienTamTinh.Location = new System.Drawing.Point(180, 325);
        lblTienTamTinh.Name = "lblTienTamTinh";
        lblTienTamTinh.Size = new System.Drawing.Size(30, 17);
        lblTienTamTinh.TabIndex = 19;
        lblTienTamTinh.Text = "0 đ";

        pnlChan.BackColor = GiaoDien.ManHinhNen;
        pnlChan.Controls.Add(btnDong);
        pnlChan.Controls.Add(btnHuyBooking);
        pnlChan.Controls.Add(btnLuu);
        pnlChan.Dock = DockStyle.Bottom;
        pnlChan.Location = new System.Drawing.Point(0, 452);
        pnlChan.Name = "pnlChan";
        pnlChan.Padding = new Padding(24, 12, 24, 12);
        pnlChan.Size = new System.Drawing.Size(560, 62);
        pnlChan.TabIndex = 2;

        GiaoDien.DangNutChinh(btnLuu);
        btnLuu.Location = new System.Drawing.Point(198, 12);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new System.Drawing.Size(120, 38);
        btnLuu.TabIndex = 0;
        btnLuu.Text = "Lưu thay đổi";
        btnLuu.UseVisualStyleBackColor = false;
        btnLuu.Click += btnLuu_Click;

        GiaoDien.DangNutNguyHiem(btnHuyBooking);
        btnHuyBooking.Location = new System.Drawing.Point(324, 12);
        btnHuyBooking.Name = "btnHuyBooking";
        btnHuyBooking.Size = new System.Drawing.Size(110, 38);
        btnHuyBooking.TabIndex = 1;
        btnHuyBooking.Text = "Hủy booking";
        btnHuyBooking.UseVisualStyleBackColor = false;
        btnHuyBooking.Click += btnHuyBooking_Click;

        GiaoDien.DangNutPhu(btnDong);
        btnDong.Location = new System.Drawing.Point(440, 12);
        btnDong.Name = "btnDong";
        btnDong.Size = new System.Drawing.Size(96, 38);
        btnDong.TabIndex = 2;
        btnDong.Text = "Đóng";
        btnDong.UseVisualStyleBackColor = false;
        btnDong.Click += btnDong_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(560, 514);
        Controls.Add(pnlThan);
        Controls.Add(pnlChan);
        Controls.Add(pnlDau);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmChiTietDatSan";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Chi tiết đặt sân";
        pnlDau.ResumeLayout(false);
        pnlThan.ResumeLayout(false);
        pnlThan.PerformLayout();
        pnlChan.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlDau;
    private Label lblTieuDe;
    private Label lblMaDat;
    private IconBox picBieuTuong;
    private Panel pnlThan;
    private Label lblTienTamTinh;
    private Label lblNhanTienTamTinh;
    private Label lblTienSan;
    private Label lblNhanTienSan;
    private Label lblDonGia;
    private Label lblNhanDonGia;
    private Label lblNhanGhiChu;
    private Label lblNhanGioKetThuc;
    private Label lblNhanGioBatDau;
    private Label lblNhanNgayDat;
    private Label lblTrangThai;
    private Label lblNhanTrangThai;
    private Label lblSan;
    private Label lblNhanSan;
    private Label lblKhachHang;
    private Label lblNhanKhachHang;
    private TextBox txtGhiChu;
    private DateTimePicker dtpGioKetThuc;
    private DateTimePicker dtpGioBatDau;
    private DateTimePicker dtpNgayDat;
    private Panel pnlChan;
    private Button btnDong;
    private Button btnHuyBooking;
    private Button btnLuu;
}
