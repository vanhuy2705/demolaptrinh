using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmChiTietHoaDon
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
        lblMaHoaDon = new Label();
        picBieuTuong = new IconBox();
        pnlThan = new Panel();
        lblNhanGhiChu = new Label();
        txtGhiChu = new TextBox();
        lblTrangThai = new Label();
        lblNhanTrangThai = new Label();
        lblPhuongThuc = new Label();
        lblNhanPhuongThuc = new Label();
        lblTongTien = new Label();
        lblNhanTongTien = new Label();
        lblTienGiam = new Label();
        lblNhanTienGiam = new Label();
        lblUuDai = new Label();
        lblNhanUuDai = new Label();
        lblTienGoc = new Label();
        lblNhanTienGoc = new Label();
        lblThoiGian = new Label();
        lblNhanThoiGian = new Label();
        lblSan = new Label();
        lblNhanSan = new Label();
        lblKhachHang = new Label();
        lblNhanKhachHang = new Label();
        lblNgayLap = new Label();
        lblNhanNgayLap = new Label();
        pnlChan = new Panel();
        btnDong = new Button();
        btnHuyHoaDon = new Button();
        btnInHoaDon = new Button();
        btnThanhToan = new Button();
        pnlDau.SuspendLayout();
        pnlThan.SuspendLayout();
        pnlChan.SuspendLayout();
        SuspendLayout();

        pnlDau.BackColor = GiaoDien.ThanhBen;
        pnlDau.Controls.Add(lblTieuDe);
        pnlDau.Controls.Add(lblMaHoaDon);
        pnlDau.Controls.Add(picBieuTuong);
        pnlDau.Dock = DockStyle.Top;
        pnlDau.Location = new System.Drawing.Point(0, 0);
        pnlDau.Name = "pnlDau";
        pnlDau.Size = new System.Drawing.Size(600, 80);
        pnlDau.TabIndex = 0;

        picBieuTuong.Location = new System.Drawing.Point(22, 18);
        picBieuTuong.MauBieuTuong = GiaoDien.Chinh;
        picBieuTuong.Name = "picBieuTuong";
        picBieuTuong.Size = new System.Drawing.Size(44, 44);
        picBieuTuong.TabIndex = 0;
        picBieuTuong.TenBieuTuong = "hoadon";

        lblMaHoaDon.AutoSize = true;
        lblMaHoaDon.Font = GiaoDien.ChuLon;
        lblMaHoaDon.ForeColor = Color.White;
        lblMaHoaDon.Location = new System.Drawing.Point(78, 16);
        lblMaHoaDon.Name = "lblMaHoaDon";
        lblMaHoaDon.Size = new System.Drawing.Size(60, 25);
        lblMaHoaDon.TabIndex = 1;
        lblMaHoaDon.Text = "#";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.ChuNho;
        lblTieuDe.ForeColor = Color.FromArgb(148, 163, 184);
        lblTieuDe.Location = new System.Drawing.Point(80, 46);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(160, 15);
        lblTieuDe.TabIndex = 2;
        lblTieuDe.Text = "Chi tiết hóa đơn";

        pnlThan.BackColor = GiaoDien.BeMat;
        pnlThan.Controls.Add(lblNhanGhiChu);
        pnlThan.Controls.Add(txtGhiChu);
        pnlThan.Controls.Add(lblTrangThai);
        pnlThan.Controls.Add(lblNhanTrangThai);
        pnlThan.Controls.Add(lblPhuongThuc);
        pnlThan.Controls.Add(lblNhanPhuongThuc);
        pnlThan.Controls.Add(lblTongTien);
        pnlThan.Controls.Add(lblNhanTongTien);
        pnlThan.Controls.Add(lblTienGiam);
        pnlThan.Controls.Add(lblNhanTienGiam);
        pnlThan.Controls.Add(lblUuDai);
        pnlThan.Controls.Add(lblNhanUuDai);
        pnlThan.Controls.Add(lblTienGoc);
        pnlThan.Controls.Add(lblNhanTienGoc);
        pnlThan.Controls.Add(lblThoiGian);
        pnlThan.Controls.Add(lblNhanThoiGian);
        pnlThan.Controls.Add(lblSan);
        pnlThan.Controls.Add(lblNhanSan);
        pnlThan.Controls.Add(lblKhachHang);
        pnlThan.Controls.Add(lblNhanKhachHang);
        pnlThan.Controls.Add(lblNgayLap);
        pnlThan.Controls.Add(lblNhanNgayLap);
        pnlThan.Dock = DockStyle.Fill;
        pnlThan.Location = new System.Drawing.Point(0, 80);
        pnlThan.Name = "pnlThan";
        pnlThan.Size = new System.Drawing.Size(600, 424);
        pnlThan.TabIndex = 1;

        lblNhanNgayLap.AutoSize = true;
        GiaoDien.DangNhan(lblNhanNgayLap);
        lblNhanNgayLap.Location = new System.Drawing.Point(32, 24);
        lblNhanNgayLap.Name = "lblNhanNgayLap";
        lblNhanNgayLap.TabIndex = 0;
        lblNhanNgayLap.Text = "Ngày lập";

        lblNgayLap.AutoSize = true;
        lblNgayLap.Font = GiaoDien.ChuDam;
        lblNgayLap.ForeColor = GiaoDien.Chu;
        lblNgayLap.Location = new System.Drawing.Point(190, 23);
        lblNgayLap.Name = "lblNgayLap";
        lblNgayLap.Size = new System.Drawing.Size(110, 17);
        lblNgayLap.TabIndex = 1;
        lblNgayLap.Text = "01/01/2026 08:00";

        lblNhanKhachHang.AutoSize = true;
        GiaoDien.DangNhan(lblNhanKhachHang);
        lblNhanKhachHang.Location = new System.Drawing.Point(32, 52);
        lblNhanKhachHang.Name = "lblNhanKhachHang";
        lblNhanKhachHang.TabIndex = 2;
        lblNhanKhachHang.Text = "Khách hàng";

        lblKhachHang.AutoSize = true;
        lblKhachHang.Font = GiaoDien.ChuDam;
        lblKhachHang.ForeColor = GiaoDien.Chu;
        lblKhachHang.Location = new System.Drawing.Point(190, 51);
        lblKhachHang.Name = "lblKhachHang";
        lblKhachHang.Size = new System.Drawing.Size(80, 17);
        lblKhachHang.TabIndex = 3;
        lblKhachHang.Text = "Khách hàng";

        lblNhanSan.AutoSize = true;
        GiaoDien.DangNhan(lblNhanSan);
        lblNhanSan.Location = new System.Drawing.Point(32, 80);
        lblNhanSan.Name = "lblNhanSan";
        lblNhanSan.TabIndex = 4;
        lblNhanSan.Text = "Sân";

        lblSan.AutoSize = true;
        lblSan.Font = GiaoDien.ChuDam;
        lblSan.ForeColor = GiaoDien.Chu;
        lblSan.Location = new System.Drawing.Point(190, 79);
        lblSan.Name = "lblSan";
        lblSan.Size = new System.Drawing.Size(40, 17);
        lblSan.TabIndex = 5;
        lblSan.Text = "Sân";

        lblNhanThoiGian.AutoSize = true;
        GiaoDien.DangNhan(lblNhanThoiGian);
        lblNhanThoiGian.Location = new System.Drawing.Point(32, 108);
        lblNhanThoiGian.Name = "lblNhanThoiGian";
        lblNhanThoiGian.TabIndex = 6;
        lblNhanThoiGian.Text = "Khung giờ";

        lblThoiGian.AutoSize = true;
        lblThoiGian.Font = GiaoDien.ChuDam;
        lblThoiGian.ForeColor = GiaoDien.Chu;
        lblThoiGian.Location = new System.Drawing.Point(190, 107);
        lblThoiGian.Name = "lblThoiGian";
        lblThoiGian.Size = new System.Drawing.Size(70, 17);
        lblThoiGian.TabIndex = 7;
        lblThoiGian.Text = "Khung giờ";

        lblNhanTienGoc.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTienGoc);
        lblNhanTienGoc.Location = new System.Drawing.Point(32, 146);
        lblNhanTienGoc.Name = "lblNhanTienGoc";
        lblNhanTienGoc.TabIndex = 8;
        lblNhanTienGoc.Text = "Tiền sân";

        lblTienGoc.AutoSize = true;
        lblTienGoc.Font = GiaoDien.ChuDam;
        lblTienGoc.ForeColor = GiaoDien.Chu;
        lblTienGoc.Location = new System.Drawing.Point(190, 145);
        lblTienGoc.Name = "lblTienGoc";
        lblTienGoc.Size = new System.Drawing.Size(30, 17);
        lblTienGoc.TabIndex = 9;
        lblTienGoc.Text = "0 đ";

        lblNhanUuDai.AutoSize = true;
        GiaoDien.DangNhan(lblNhanUuDai);
        lblNhanUuDai.Location = new System.Drawing.Point(32, 174);
        lblNhanUuDai.Name = "lblNhanUuDai";
        lblNhanUuDai.TabIndex = 10;
        lblNhanUuDai.Text = "Ưu đãi";

        lblUuDai.AutoSize = true;
        lblUuDai.Font = GiaoDien.ChuDam;
        lblUuDai.ForeColor = GiaoDien.ThanhCong;
        lblUuDai.Location = new System.Drawing.Point(190, 173);
        lblUuDai.Name = "lblUuDai";
        lblUuDai.Size = new System.Drawing.Size(50, 17);
        lblUuDai.TabIndex = 11;
        lblUuDai.Text = "Không";

        lblNhanTienGiam.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTienGiam);
        lblNhanTienGiam.Location = new System.Drawing.Point(32, 202);
        lblNhanTienGiam.Name = "lblNhanTienGiam";
        lblNhanTienGiam.TabIndex = 12;
        lblNhanTienGiam.Text = "Số tiền giảm";

        lblTienGiam.AutoSize = true;
        lblTienGiam.Font = GiaoDien.ChuDam;
        lblTienGiam.ForeColor = GiaoDien.ThanhCong;
        lblTienGiam.Location = new System.Drawing.Point(190, 201);
        lblTienGiam.Name = "lblTienGiam";
        lblTienGiam.Size = new System.Drawing.Size(30, 17);
        lblTienGiam.TabIndex = 13;
        lblTienGiam.Text = "0 đ";

        lblNhanTongTien.AutoSize = true;
        lblNhanTongTien.Font = GiaoDien.ChuDam;
        lblNhanTongTien.ForeColor = GiaoDien.Chu;
        lblNhanTongTien.Location = new System.Drawing.Point(32, 240);
        lblNhanTongTien.Name = "lblNhanTongTien";
        lblNhanTongTien.Size = new System.Drawing.Size(100, 17);
        lblNhanTongTien.TabIndex = 14;
        lblNhanTongTien.Text = "Tổng thanh toán";

        lblTongTien.AutoSize = true;
        lblTongTien.Font = GiaoDien.TieuDe;
        lblTongTien.ForeColor = GiaoDien.ChinhDam;
        lblTongTien.Location = new System.Drawing.Point(190, 228);
        lblTongTien.Name = "lblTongTien";
        lblTongTien.Size = new System.Drawing.Size(80, 31);
        lblTongTien.TabIndex = 15;
        lblTongTien.Text = "0 đ";

        lblNhanPhuongThuc.AutoSize = true;
        GiaoDien.DangNhan(lblNhanPhuongThuc);
        lblNhanPhuongThuc.Location = new System.Drawing.Point(32, 280);
        lblNhanPhuongThuc.Name = "lblNhanPhuongThuc";
        lblNhanPhuongThuc.TabIndex = 16;
        lblNhanPhuongThuc.Text = "Phương thức";

        lblPhuongThuc.AutoSize = true;
        lblPhuongThuc.Font = GiaoDien.ChuDam;
        lblPhuongThuc.ForeColor = GiaoDien.Chu;
        lblPhuongThuc.Location = new System.Drawing.Point(190, 279);
        lblPhuongThuc.Name = "lblPhuongThuc";
        lblPhuongThuc.Size = new System.Drawing.Size(70, 17);
        lblPhuongThuc.TabIndex = 17;
        lblPhuongThuc.Text = "Tiền mặt";

        lblNhanTrangThai.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTrangThai);
        lblNhanTrangThai.Location = new System.Drawing.Point(32, 308);
        lblNhanTrangThai.Name = "lblNhanTrangThai";
        lblNhanTrangThai.TabIndex = 18;
        lblNhanTrangThai.Text = "Trạng thái";

        lblTrangThai.AutoSize = true;
        lblTrangThai.Font = GiaoDien.ChuDam;
        lblTrangThai.ForeColor = GiaoDien.CanhBao;
        lblTrangThai.Location = new System.Drawing.Point(190, 307);
        lblTrangThai.Name = "lblTrangThai";
        lblTrangThai.Size = new System.Drawing.Size(90, 17);
        lblTrangThai.TabIndex = 19;
        lblTrangThai.Text = "Chưa thanh toán";

        lblNhanGhiChu.AutoSize = true;
        GiaoDien.DangNhan(lblNhanGhiChu);
        lblNhanGhiChu.Location = new System.Drawing.Point(32, 342);
        lblNhanGhiChu.Name = "lblNhanGhiChu";
        lblNhanGhiChu.TabIndex = 20;
        lblNhanGhiChu.Text = "Ghi chú";

        txtGhiChu.BackColor = GiaoDien.ManHinhNen;
        txtGhiChu.BorderStyle = BorderStyle.FixedSingle;
        txtGhiChu.Font = GiaoDien.ChuThuong;
        txtGhiChu.Location = new System.Drawing.Point(190, 340);
        txtGhiChu.Multiline = true;
        txtGhiChu.Name = "txtGhiChu";
        txtGhiChu.Size = new System.Drawing.Size(360, 60);
        txtGhiChu.TabIndex = 0;

        pnlChan.BackColor = GiaoDien.ManHinhNen;
        pnlChan.Controls.Add(btnDong);
        pnlChan.Controls.Add(btnHuyHoaDon);
        pnlChan.Controls.Add(btnInHoaDon);
        pnlChan.Controls.Add(btnThanhToan);
        pnlChan.Dock = DockStyle.Bottom;
        pnlChan.Location = new System.Drawing.Point(0, 504);
        pnlChan.Name = "pnlChan";
        pnlChan.Padding = new Padding(24, 12, 24, 12);
        pnlChan.Size = new System.Drawing.Size(600, 62);
        pnlChan.TabIndex = 2;

        GiaoDien.DangNutChinh(btnThanhToan);
        btnThanhToan.Location = new System.Drawing.Point(150, 12);
        btnThanhToan.Name = "btnThanhToan";
        btnThanhToan.Size = new System.Drawing.Size(130, 38);
        btnThanhToan.TabIndex = 1;
        btnThanhToan.Text = "Thanh toán";
        btnThanhToan.UseVisualStyleBackColor = false;
        btnThanhToan.Click += btnThanhToan_Click;

        GiaoDien.DangNutPhu(btnInHoaDon);
        btnInHoaDon.Location = new System.Drawing.Point(290, 12);
        btnInHoaDon.Name = "btnInHoaDon";
        btnInHoaDon.Size = new System.Drawing.Size(110, 38);
        btnInHoaDon.TabIndex = 2;
        btnInHoaDon.Text = "In hóa đơn";
        btnInHoaDon.UseVisualStyleBackColor = false;
        btnInHoaDon.Click += btnInHoaDon_Click;

        GiaoDien.DangNutNguyHiem(btnHuyHoaDon);
        btnHuyHoaDon.Location = new System.Drawing.Point(410, 12);
        btnHuyHoaDon.Name = "btnHuyHoaDon";
        btnHuyHoaDon.Size = new System.Drawing.Size(70, 38);
        btnHuyHoaDon.TabIndex = 3;
        btnHuyHoaDon.Text = "Hủy";
        btnHuyHoaDon.UseVisualStyleBackColor = false;
        btnHuyHoaDon.Click += btnHuyHoaDon_Click;

        GiaoDien.DangNutPhu(btnDong);
        btnDong.Location = new System.Drawing.Point(490, 12);
        btnDong.Name = "btnDong";
        btnDong.Size = new System.Drawing.Size(86, 38);
        btnDong.TabIndex = 4;
        btnDong.Text = "Đóng";
        btnDong.UseVisualStyleBackColor = false;
        btnDong.Click += btnDong_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(600, 566);
        Controls.Add(pnlThan);
        Controls.Add(pnlChan);
        Controls.Add(pnlDau);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmChiTietHoaDon";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Chi tiết hóa đơn";
        pnlDau.ResumeLayout(false);
        pnlThan.ResumeLayout(false);
        pnlThan.PerformLayout();
        pnlChan.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlDau;
    private Label lblTieuDe;
    private Label lblMaHoaDon;
    private IconBox picBieuTuong;
    private Panel pnlThan;
    private Label lblNhanGhiChu;
    private TextBox txtGhiChu;
    private Label lblTrangThai;
    private Label lblNhanTrangThai;
    private Label lblPhuongThuc;
    private Label lblNhanPhuongThuc;
    private Label lblTongTien;
    private Label lblNhanTongTien;
    private Label lblTienGiam;
    private Label lblNhanTienGiam;
    private Label lblUuDai;
    private Label lblNhanUuDai;
    private Label lblTienGoc;
    private Label lblNhanTienGoc;
    private Label lblThoiGian;
    private Label lblNhanThoiGian;
    private Label lblSan;
    private Label lblNhanSan;
    private Label lblKhachHang;
    private Label lblNhanKhachHang;
    private Label lblNgayLap;
    private Label lblNhanNgayLap;
    private Panel pnlChan;
    private Button btnDong;
    private Button btnHuyHoaDon;
    private Button btnInHoaDon;
    private Button btnThanhToan;
}
