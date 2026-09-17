using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmThongTinCaNhan
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
        pnlKpi = new Panel();
        kpiSanYeuThich = new KpiCard();
        kpiVoucher = new KpiCard();
        kpiChiTieu = new KpiCard();
        kpiSoLanDat = new KpiCard();
        pnlThongTin = new Panel();
        pnlNut = new Panel();
        btnDoiMatKhau = new Button();
        btnHuy = new Button();
        btnLuu = new Button();
        btnSua = new Button();
        lblNhanDiaChi = new Label();
        lblNhanEmail = new Label();
        lblNhanDienThoai = new Label();
        lblNhanHoTen = new Label();
        lblNgayTao = new Label();
        lblNhanNgayTao = new Label();
        lblMaKH = new Label();
        lblNhanMa = new Label();
        txtDiaChi = new TextBox();
        txtEmail = new TextBox();
        txtSoDienThoai = new TextBox();
        txtHoTen = new TextBox();
        ((System.ComponentModel.ISupportInitialize)errLoi).BeginInit();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlKpi.SuspendLayout();
        pnlThongTin.SuspendLayout();
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
        lblTieuDe.Size = new System.Drawing.Size(200, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Thông tin cá nhân";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(400, 15);
        lblMoTaTrang.Text = "Hồ sơ, thống kê cá nhân và đổi mật khẩu";

        pnlNoiDung.Controls.Add(pnlThongTin);
        pnlNoiDung.Controls.Add(pnlKpi);
        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(0, 78);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(16);
        pnlNoiDung.Size = new System.Drawing.Size(1200, 642);
        pnlNoiDung.TabIndex = 1;

        pnlKpi.BackColor = GiaoDien.BeMat;
        pnlKpi.Controls.Add(kpiSanYeuThich);
        pnlKpi.Controls.Add(kpiVoucher);
        pnlKpi.Controls.Add(kpiChiTieu);
        pnlKpi.Controls.Add(kpiSoLanDat);
        pnlKpi.Dock = DockStyle.Top;
        pnlKpi.Location = new System.Drawing.Point(16, 16);
        pnlKpi.Name = "pnlKpi";
        pnlKpi.Padding = new Padding(0, 0, 0, 12);
        pnlKpi.Size = new System.Drawing.Size(1168, 122);
        pnlKpi.TabIndex = 0;

        kpiSoLanDat.GiaTri = "0";
        kpiSoLanDat.Location = new System.Drawing.Point(0, 0);
        kpiSoLanDat.MauNhan = GiaoDien.Chinh;
        kpiSoLanDat.Name = "kpiSoLanDat";
        kpiSoLanDat.PhuDe = "Hoàn thành: 0";
        kpiSoLanDat.Size = new System.Drawing.Size(280, 110);
        kpiSoLanDat.TabIndex = 0;
        kpiSoLanDat.TenBieuTuong = "calendar";
        kpiSoLanDat.TieuDe = "Số lần đặt";

        kpiChiTieu.GiaTri = "0 đ";
        kpiChiTieu.Location = new System.Drawing.Point(296, 0);
        kpiChiTieu.MauNhan = GiaoDien.ThanhCong;
        kpiChiTieu.Name = "kpiChiTieu";
        kpiChiTieu.PhuDe = "Hủy: 0 lần";
        kpiChiTieu.Size = new System.Drawing.Size(280, 110);
        kpiChiTieu.TabIndex = 1;
        kpiChiTieu.TenBieuTuong = "hoadon";
        kpiChiTieu.TieuDe = "Tổng chi tiêu";

        kpiVoucher.GiaTri = "0";
        kpiVoucher.Location = new System.Drawing.Point(592, 0);
        kpiVoucher.MauNhan = GiaoDien.CanhBao;
        kpiVoucher.Name = "kpiVoucher";
        kpiVoucher.PhuDe = "";
        kpiVoucher.Size = new System.Drawing.Size(280, 110);
        kpiVoucher.TabIndex = 2;
        kpiVoucher.TenBieuTuong = "voucher";
        kpiVoucher.TieuDe = "Lần dùng voucher";

        kpiSanYeuThich.GiaTri = "Chưa có";
        kpiSanYeuThich.Location = new System.Drawing.Point(888, 0);
        kpiSanYeuThich.MauNhan = GiaoDien.ChinhDam;
        kpiSanYeuThich.Name = "kpiSanYeuThich";
        kpiSanYeuThich.PhuDe = "";
        kpiSanYeuThich.Size = new System.Drawing.Size(280, 110);
        kpiSanYeuThich.TabIndex = 3;
        kpiSanYeuThich.TenBieuTuong = "sanbong";
        kpiSanYeuThich.TieuDe = "Sân yêu thích";

        pnlThongTin.BackColor = GiaoDien.BeMat;
        pnlThongTin.Controls.Add(pnlNut);
        pnlThongTin.Controls.Add(lblNhanDiaChi);
        pnlThongTin.Controls.Add(lblNhanEmail);
        pnlThongTin.Controls.Add(lblNhanDienThoai);
        pnlThongTin.Controls.Add(lblNhanHoTen);
        pnlThongTin.Controls.Add(lblNgayTao);
        pnlThongTin.Controls.Add(lblNhanNgayTao);
        pnlThongTin.Controls.Add(lblMaKH);
        pnlThongTin.Controls.Add(lblNhanMa);
        pnlThongTin.Controls.Add(txtDiaChi);
        pnlThongTin.Controls.Add(txtEmail);
        pnlThongTin.Controls.Add(txtSoDienThoai);
        pnlThongTin.Controls.Add(txtHoTen);
        pnlThongTin.Dock = DockStyle.Fill;
        pnlThongTin.Location = new System.Drawing.Point(16, 138);
        pnlThongTin.Name = "pnlThongTin";
        pnlThongTin.Padding = new Padding(24, 20, 24, 20);
        pnlThongTin.Size = new System.Drawing.Size(1168, 488);
        pnlThongTin.TabIndex = 1;

        lblNhanMa.AutoSize = true;
        GiaoDien.DangNhan(lblNhanMa);
        lblNhanMa.Location = new System.Drawing.Point(24, 20);
        lblNhanMa.Name = "lblNhanMa";
        lblNhanMa.TabIndex = 0;
        lblNhanMa.Text = "Mã khách hàng";

        lblMaKH.AutoSize = true;
        lblMaKH.Font = GiaoDien.ChuDam;
        lblMaKH.ForeColor = GiaoDien.ChinhDam;
        lblMaKH.Location = new System.Drawing.Point(160, 19);
        lblMaKH.Name = "lblMaKH";
        lblMaKH.Size = new System.Drawing.Size(20, 17);
        lblMaKH.TabIndex = 1;
        lblMaKH.Text = "—";

        lblNhanNgayTao.AutoSize = true;
        GiaoDien.DangNhan(lblNhanNgayTao);
        lblNhanNgayTao.Location = new System.Drawing.Point(360, 20);
        lblNhanNgayTao.Name = "lblNhanNgayTao";
        lblNhanNgayTao.TabIndex = 2;
        lblNhanNgayTao.Text = "Ngày tham gia";

        lblNgayTao.AutoSize = true;
        lblNgayTao.Font = GiaoDien.ChuDam;
        lblNgayTao.ForeColor = GiaoDien.Chu;
        lblNgayTao.Location = new System.Drawing.Point(480, 19);
        lblNgayTao.Name = "lblNgayTao";
        lblNgayTao.Size = new System.Drawing.Size(80, 17);
        lblNgayTao.TabIndex = 3;
        lblNgayTao.Text = "—";

        lblNhanHoTen.AutoSize = true;
        GiaoDien.DangNhan(lblNhanHoTen);
        lblNhanHoTen.Location = new System.Drawing.Point(24, 62);
        lblNhanHoTen.Name = "lblNhanHoTen";
        lblNhanHoTen.TabIndex = 4;
        lblNhanHoTen.Text = "Họ tên (*)";

        txtHoTen.BackColor = GiaoDien.ManHinhNen;
        txtHoTen.BorderStyle = BorderStyle.FixedSingle;
        txtHoTen.Font = GiaoDien.ChuThuong;
        txtHoTen.Location = new System.Drawing.Point(24, 82);
        txtHoTen.Name = "txtHoTen";
        txtHoTen.ReadOnly = true;
        txtHoTen.Size = new System.Drawing.Size(420, 25);
        txtHoTen.TabIndex = 0;

        lblNhanDienThoai.AutoSize = true;
        GiaoDien.DangNhan(lblNhanDienThoai);
        lblNhanDienThoai.Location = new System.Drawing.Point(24, 120);
        lblNhanDienThoai.Name = "lblNhanDienThoai";
        lblNhanDienThoai.TabIndex = 6;
        lblNhanDienThoai.Text = "Số điện thoại (*)";

        txtSoDienThoai.BackColor = GiaoDien.ManHinhNen;
        txtSoDienThoai.BorderStyle = BorderStyle.FixedSingle;
        txtSoDienThoai.Font = GiaoDien.ChuThuong;
        txtSoDienThoai.Location = new System.Drawing.Point(24, 140);
        txtSoDienThoai.Name = "txtSoDienThoai";
        txtSoDienThoai.ReadOnly = true;
        txtSoDienThoai.Size = new System.Drawing.Size(420, 25);
        txtSoDienThoai.TabIndex = 1;

        lblNhanEmail.AutoSize = true;
        GiaoDien.DangNhan(lblNhanEmail);
        lblNhanEmail.Location = new System.Drawing.Point(24, 178);
        lblNhanEmail.Name = "lblNhanEmail";
        lblNhanEmail.TabIndex = 8;
        lblNhanEmail.Text = "Email";

        txtEmail.BackColor = GiaoDien.ManHinhNen;
        txtEmail.BorderStyle = BorderStyle.FixedSingle;
        txtEmail.Font = GiaoDien.ChuThuong;
        txtEmail.Location = new System.Drawing.Point(24, 198);
        txtEmail.Name = "txtEmail";
        txtEmail.ReadOnly = true;
        txtEmail.Size = new System.Drawing.Size(420, 25);
        txtEmail.TabIndex = 2;

        lblNhanDiaChi.AutoSize = true;
        GiaoDien.DangNhan(lblNhanDiaChi);
        lblNhanDiaChi.Location = new System.Drawing.Point(24, 236);
        lblNhanDiaChi.Name = "lblNhanDiaChi";
        lblNhanDiaChi.TabIndex = 10;
        lblNhanDiaChi.Text = "Địa chỉ";

        txtDiaChi.BackColor = GiaoDien.ManHinhNen;
        txtDiaChi.BorderStyle = BorderStyle.FixedSingle;
        txtDiaChi.Font = GiaoDien.ChuThuong;
        txtDiaChi.Location = new System.Drawing.Point(24, 256);
        txtDiaChi.Multiline = true;
        txtDiaChi.Name = "txtDiaChi";
        txtDiaChi.ReadOnly = true;
        txtDiaChi.Size = new System.Drawing.Size(420, 70);
        txtDiaChi.TabIndex = 3;

        pnlNut.Controls.Add(btnDoiMatKhau);
        pnlNut.Controls.Add(btnHuy);
        pnlNut.Controls.Add(btnLuu);
        pnlNut.Controls.Add(btnSua);
        pnlNut.Location = new System.Drawing.Point(24, 350);
        pnlNut.Name = "pnlNut";
        pnlNut.Size = new System.Drawing.Size(560, 50);
        pnlNut.TabIndex = 12;

        GiaoDien.DangNutChinh(btnSua);
        btnSua.Location = new System.Drawing.Point(0, 6);
        btnSua.Name = "btnSua";
        btnSua.Size = new System.Drawing.Size(110, 38);
        btnSua.TabIndex = 4;
        btnSua.Text = "Cập nhật";
        btnSua.UseVisualStyleBackColor = false;
        btnSua.Click += btnSua_Click;

        GiaoDien.DangNutChinh(btnLuu);
        btnLuu.Location = new System.Drawing.Point(120, 6);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new System.Drawing.Size(110, 38);
        btnLuu.TabIndex = 5;
        btnLuu.Text = "Lưu";
        btnLuu.UseVisualStyleBackColor = false;
        btnLuu.Click += btnLuu_Click;

        GiaoDien.DangNutPhu(btnHuy);
        btnHuy.Location = new System.Drawing.Point(240, 6);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new System.Drawing.Size(110, 38);
        btnHuy.TabIndex = 6;
        btnHuy.Text = "Hủy bỏ";
        btnHuy.UseVisualStyleBackColor = false;
        btnHuy.Click += btnHuy_Click;

        GiaoDien.DangNutPhu(btnDoiMatKhau);
        btnDoiMatKhau.Location = new System.Drawing.Point(360, 6);
        btnDoiMatKhau.Name = "btnDoiMatKhau";
        btnDoiMatKhau.Size = new System.Drawing.Size(150, 38);
        btnDoiMatKhau.TabIndex = 7;
        btnDoiMatKhau.Text = "Đổi mật khẩu";
        btnDoiMatKhau.UseVisualStyleBackColor = false;
        btnDoiMatKhau.Click += btnDoiMatKhau_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmThongTinCaNhan";
        Text = "Thông tin cá nhân";
        ((System.ComponentModel.ISupportInitialize)errLoi).EndInit();
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlKpi.ResumeLayout(false);
        pnlThongTin.ResumeLayout(false);
        pnlThongTin.PerformLayout();
        pnlNut.ResumeLayout(false);
        ResumeLayout(false);
    }

    private ErrorProvider errLoi;
    private Panel pnlDau;
    private Label lblMoTaTrang;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Panel pnlNoiDung;
    private Panel pnlKpi;
    private KpiCard kpiSanYeuThich;
    private KpiCard kpiVoucher;
    private KpiCard kpiChiTieu;
    private KpiCard kpiSoLanDat;
    private Panel pnlThongTin;
    private Panel pnlNut;
    private Button btnDoiMatKhau;
    private Button btnHuy;
    private Button btnLuu;
    private Button btnSua;
    private Label lblNhanDiaChi;
    private Label lblNhanEmail;
    private Label lblNhanDienThoai;
    private Label lblNhanHoTen;
    private Label lblNgayTao;
    private Label lblNhanNgayTao;
    private Label lblMaKH;
    private Label lblNhanMa;
    private TextBox txtDiaChi;
    private TextBox txtEmail;
    private TextBox txtSoDienThoai;
    private TextBox txtHoTen;
}
