using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmHoaDonAdmin
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
        lblMoTaTrang = new Label();
        lblTieuDe = new Label();
        picBieuTuong = new IconBox();
        pnlNoiDung = new Panel();
        pnlHoaDon = new Panel();
        dgvHoaDon = new DataGridView();
        pnlBoLoc = new Panel();
        cboTrangThai = new ComboBox();
        btnLamMoi = new Button();
        btnTim = new Button();
        txtTimKiem = new TextBox();
        lblNhanDenNgay = new Label();
        lblNhanTuNgay = new Label();
        dtpDenNgay = new DateTimePicker();
        dtpTuNgay = new DateTimePicker();
        pnlTaoHoaDon = new Panel();
        btnLapHoaDon = new Button();
        cboBookingChuaLap = new ComboBox();
        lblNhanBooking = new Label();
        pnlChan = new Panel();
        lblThongKe = new Label();
        btnXoa = new Button();
        btnIn = new Button();
        btnThanhToan = new Button();
        btnChiTiet = new Button();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlHoaDon.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHoaDon).BeginInit();
        pnlBoLoc.SuspendLayout();
        pnlTaoHoaDon.SuspendLayout();
        pnlChan.SuspendLayout();
        SuspendLayout();

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
        picBieuTuong.TenBieuTuong = "hoadon";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(76, 14);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(200, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Quản lý hóa đơn";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(420, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Lập hóa đơn từ booking, thu tiền, in hóa đơn và theo dõi công nợ";

        pnlNoiDung.Controls.Add(pnlHoaDon);
        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(0, 78);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(16);
        pnlNoiDung.Size = new System.Drawing.Size(1200, 642);
        pnlNoiDung.TabIndex = 1;

        pnlHoaDon.BackColor = GiaoDien.BeMat;
        pnlHoaDon.Controls.Add(dgvHoaDon);
        pnlHoaDon.Controls.Add(pnlBoLoc);
        pnlHoaDon.Controls.Add(pnlTaoHoaDon);
        pnlHoaDon.Controls.Add(pnlChan);
        pnlHoaDon.Dock = DockStyle.Fill;
        pnlHoaDon.Location = new System.Drawing.Point(16, 16);
        pnlHoaDon.Name = "pnlHoaDon";
        pnlHoaDon.Size = new System.Drawing.Size(1168, 610);
        pnlHoaDon.TabIndex = 0;

        pnlTaoHoaDon.BackColor = GiaoDien.ManHinhNen;
        pnlTaoHoaDon.Controls.Add(btnLapHoaDon);
        pnlTaoHoaDon.Controls.Add(cboBookingChuaLap);
        pnlTaoHoaDon.Controls.Add(lblNhanBooking);
        pnlTaoHoaDon.Dock = DockStyle.Top;
        pnlTaoHoaDon.Location = new System.Drawing.Point(0, 0);
        pnlTaoHoaDon.Name = "pnlTaoHoaDon";
        pnlTaoHoaDon.Padding = new Padding(14, 12, 14, 12);
        pnlTaoHoaDon.Size = new System.Drawing.Size(1168, 62);
        pnlTaoHoaDon.TabIndex = 0;

        lblNhanBooking.AutoSize = true;
        GiaoDien.DangNhan(lblNhanBooking);
        lblNhanBooking.Location = new System.Drawing.Point(14, 18);
        lblNhanBooking.Name = "lblNhanBooking";
        lblNhanBooking.TabIndex = 0;
        lblNhanBooking.Text = "Booking chờ lập hóa đơn";

        cboBookingChuaLap.DropDownStyle = ComboBoxStyle.DropDownList;
        cboBookingChuaLap.Font = GiaoDien.ChuThuong;
        cboBookingChuaLap.Location = new System.Drawing.Point(200, 16);
        cboBookingChuaLap.Name = "cboBookingChuaLap";
        cboBookingChuaLap.Size = new System.Drawing.Size(480, 25);
        cboBookingChuaLap.TabIndex = 0;
        cboBookingChuaLap.SelectedIndexChanged += cboBookingChuaLap_SelectedIndexChanged;

        GiaoDien.DangNutChinh(btnLapHoaDon);
        btnLapHoaDon.Location = new System.Drawing.Point(694, 12);
        btnLapHoaDon.Name = "btnLapHoaDon";
        btnLapHoaDon.Size = new System.Drawing.Size(150, 34);
        btnLapHoaDon.TabIndex = 1;
        btnLapHoaDon.Text = "Lập hóa đơn";
        btnLapHoaDon.UseVisualStyleBackColor = false;
        btnLapHoaDon.Click += btnLapHoaDon_Click;

        pnlBoLoc.Controls.Add(cboTrangThai);
        pnlBoLoc.Controls.Add(btnLamMoi);
        pnlBoLoc.Controls.Add(btnTim);
        pnlBoLoc.Controls.Add(txtTimKiem);
        pnlBoLoc.Controls.Add(lblNhanDenNgay);
        pnlBoLoc.Controls.Add(lblNhanTuNgay);
        pnlBoLoc.Controls.Add(dtpDenNgay);
        pnlBoLoc.Controls.Add(dtpTuNgay);
        pnlBoLoc.Dock = DockStyle.Top;
        pnlBoLoc.Location = new System.Drawing.Point(0, 62);
        pnlBoLoc.Name = "pnlBoLoc";
        pnlBoLoc.Padding = new Padding(14, 10, 14, 10);
        pnlBoLoc.Size = new System.Drawing.Size(1168, 56);
        pnlBoLoc.TabIndex = 1;

        lblNhanTuNgay.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTuNgay);
        lblNhanTuNgay.Location = new System.Drawing.Point(14, 14);
        lblNhanTuNgay.Name = "lblNhanTuNgay";
        lblNhanTuNgay.TabIndex = 0;
        lblNhanTuNgay.Text = "Từ ngày";

        dtpTuNgay.Font = GiaoDien.ChuThuong;
        dtpTuNgay.Format = DateTimePickerFormat.Short;
        dtpTuNgay.Location = new System.Drawing.Point(14, 30);
        dtpTuNgay.Name = "dtpTuNgay";
        dtpTuNgay.Size = new System.Drawing.Size(140, 25);
        dtpTuNgay.TabIndex = 0;
        dtpTuNgay.ValueChanged += LocThayDoi;

        lblNhanDenNgay.AutoSize = true;
        GiaoDien.DangNhan(lblNhanDenNgay);
        lblNhanDenNgay.Location = new System.Drawing.Point(166, 14);
        lblNhanDenNgay.Name = "lblNhanDenNgay";
        lblNhanDenNgay.TabIndex = 2;
        lblNhanDenNgay.Text = "Đến ngày";

        dtpDenNgay.Font = GiaoDien.ChuThuong;
        dtpDenNgay.Format = DateTimePickerFormat.Short;
        dtpDenNgay.Location = new System.Drawing.Point(166, 30);
        dtpDenNgay.Name = "dtpDenNgay";
        dtpDenNgay.Size = new System.Drawing.Size(140, 25);
        dtpDenNgay.TabIndex = 1;
        dtpDenNgay.ValueChanged += LocThayDoi;

        cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTrangThai.Font = GiaoDien.ChuThuong;
        cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Chưa thanh toán", "Đã thanh toán", "Đã hủy" });
        cboTrangThai.Location = new System.Drawing.Point(320, 29);
        cboTrangThai.Name = "cboTrangThai";
        cboTrangThai.Size = new System.Drawing.Size(170, 25);
        cboTrangThai.TabIndex = 2;
        cboTrangThai.SelectedIndexChanged += LocThayDoi;

        txtTimKiem.BackColor = GiaoDien.ManHinhNen;
        txtTimKiem.BorderStyle = BorderStyle.FixedSingle;
        txtTimKiem.Font = GiaoDien.ChuThuong;
        txtTimKiem.Location = new System.Drawing.Point(504, 30);
        txtTimKiem.Name = "txtTimKiem";
        txtTimKiem.Size = new System.Drawing.Size(300, 25);
        txtTimKiem.TabIndex = 3;
        txtTimKiem.KeyDown += txtTimKiem_KeyDown;

        GiaoDien.DangNutChinh(btnTim);
        btnTim.Location = new System.Drawing.Point(814, 26);
        btnTim.Name = "btnTim";
        btnTim.Size = new System.Drawing.Size(90, 34);
        btnTim.TabIndex = 4;
        btnTim.Text = "Lọc";
        btnTim.UseVisualStyleBackColor = false;
        btnTim.Click += btnTim_Click;

        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Location = new System.Drawing.Point(914, 26);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(90, 34);
        btnLamMoi.TabIndex = 5;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        dgvHoaDon.Dock = DockStyle.Fill;
        dgvHoaDon.Location = new System.Drawing.Point(0, 118);
        dgvHoaDon.Name = "dgvHoaDon";
        dgvHoaDon.Size = new System.Drawing.Size(1168, 440);
        dgvHoaDon.TabIndex = 2;
        dgvHoaDon.SelectionChanged += dgvHoaDon_SelectionChanged;

        pnlChan.BackColor = GiaoDien.BeMat;
        pnlChan.Controls.Add(lblThongKe);
        pnlChan.Controls.Add(btnXoa);
        pnlChan.Controls.Add(btnIn);
        pnlChan.Controls.Add(btnThanhToan);
        pnlChan.Controls.Add(btnChiTiet);
        pnlChan.Dock = DockStyle.Bottom;
        pnlChan.Location = new System.Drawing.Point(0, 558);
        pnlChan.Name = "pnlChan";
        pnlChan.Padding = new Padding(14, 6, 14, 6);
        pnlChan.Size = new System.Drawing.Size(1168, 52);
        pnlChan.TabIndex = 3;

        lblThongKe.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
        lblThongKe.Font = GiaoDien.ChuNho;
        lblThongKe.ForeColor = GiaoDien.ChuPhu;
        lblThongKe.Location = new System.Drawing.Point(14, 18);
        lblThongKe.Size = new System.Drawing.Size(480, 20);
        lblThongKe.TabIndex = 0;
        lblThongKe.Text = "Tổng: 0 đ";

        GiaoDien.DangNutPhu(btnChiTiet);
        btnChiTiet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnChiTiet.Location = new System.Drawing.Point(600, 7);
        btnChiTiet.Name = "btnChiTiet";
        btnChiTiet.Size = new System.Drawing.Size(110, 38);
        btnChiTiet.TabIndex = 1;
        btnChiTiet.Text = "Chi tiết";
        btnChiTiet.UseVisualStyleBackColor = false;
        btnChiTiet.Click += btnChiTiet_Click;

        GiaoDien.DangNutChinh(btnThanhToan);
        btnThanhToan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnThanhToan.Location = new System.Drawing.Point(720, 7);
        btnThanhToan.Name = "btnThanhToan";
        btnThanhToan.Size = new System.Drawing.Size(120, 38);
        btnThanhToan.TabIndex = 2;
        btnThanhToan.Text = "Thanh toán";
        btnThanhToan.UseVisualStyleBackColor = false;
        btnThanhToan.Click += btnThanhToan_Click;

        GiaoDien.DangNutPhu(btnIn);
        btnIn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnIn.Location = new System.Drawing.Point(850, 7);
        btnIn.Name = "btnIn";
        btnIn.Size = new System.Drawing.Size(110, 38);
        btnIn.TabIndex = 3;
        btnIn.Text = "In hóa đơn";
        btnIn.UseVisualStyleBackColor = false;
        btnIn.Click += btnIn_Click;

        GiaoDien.DangNutNguyHiem(btnXoa);
        btnXoa.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnXoa.Location = new System.Drawing.Point(970, 7);
        btnXoa.Name = "btnXoa";
        btnXoa.Size = new System.Drawing.Size(80, 38);
        btnXoa.TabIndex = 4;
        btnXoa.Text = "Xóa";
        btnXoa.UseVisualStyleBackColor = false;
        btnXoa.Click += btnXoa_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmHoaDonAdmin";
        Text = "Quản lý hóa đơn";
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlHoaDon.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvHoaDon).EndInit();
        pnlBoLoc.ResumeLayout(false);
        pnlBoLoc.PerformLayout();
        pnlTaoHoaDon.ResumeLayout(false);
        pnlTaoHoaDon.PerformLayout();
        pnlChan.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlDau;
    private Label lblMoTaTrang;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Panel pnlNoiDung;
    private Panel pnlHoaDon;
    private DataGridView dgvHoaDon;
    private Panel pnlBoLoc;
    private ComboBox cboTrangThai;
    private Button btnLamMoi;
    private Button btnTim;
    private TextBox txtTimKiem;
    private Label lblNhanDenNgay;
    private Label lblNhanTuNgay;
    private DateTimePicker dtpDenNgay;
    private DateTimePicker dtpTuNgay;
    private Panel pnlTaoHoaDon;
    private Button btnLapHoaDon;
    private ComboBox cboBookingChuaLap;
    private Label lblNhanBooking;
    private Panel pnlChan;
    private Label lblThongKe;
    private Button btnXoa;
    private Button btnIn;
    private Button btnThanhToan;
    private Button btnChiTiet;
}
