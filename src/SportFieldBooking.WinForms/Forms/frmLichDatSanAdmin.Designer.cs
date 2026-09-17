using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmLichDatSanAdmin
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
        pnlLich = new Panel();
        dgvLich = new DataGridView();
        pnlBoLoc = new Panel();
        cboLocTrangThai = new ComboBox();
        cboLocSan = new ComboBox();
        btnHomNay = new Button();
        btnLamMoi = new Button();
        btnTim = new Button();
        txtTimKiem = new TextBox();
        lblNhanDenNgay = new Label();
        lblNhanTuNgay = new Label();
        dtpDenNgay = new DateTimePicker();
        dtpTuNgay = new DateTimePicker();
        pnlChan = new Panel();
        lblThongKe = new Label();
        btnLapHoaDon = new Button();
        btnHuyBooking = new Button();
        btnXemChiTiet = new Button();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlLich.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLich).BeginInit();
        pnlBoLoc.SuspendLayout();
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
        picBieuTuong.TenBieuTuong = "calendar";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(76, 14);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(200, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Lịch đặt sân";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(400, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Xem toàn bộ lịch, lọc theo ngày/sân/trạng thái và xử lý booking";

        pnlNoiDung.Controls.Add(pnlLich);
        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(0, 78);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(16);
        pnlNoiDung.Size = new System.Drawing.Size(1200, 642);
        pnlNoiDung.TabIndex = 1;

        pnlLich.BackColor = GiaoDien.BeMat;
        pnlLich.Controls.Add(dgvLich);
        pnlLich.Controls.Add(pnlBoLoc);
        pnlLich.Controls.Add(pnlChan);
        pnlLich.Dock = DockStyle.Fill;
        pnlLich.Location = new System.Drawing.Point(16, 16);
        pnlLich.Name = "pnlLich";
        pnlLich.Size = new System.Drawing.Size(1168, 610);
        pnlLich.TabIndex = 0;

        pnlBoLoc.Controls.Add(cboLocTrangThai);
        pnlBoLoc.Controls.Add(cboLocSan);
        pnlBoLoc.Controls.Add(btnHomNay);
        pnlBoLoc.Controls.Add(btnLamMoi);
        pnlBoLoc.Controls.Add(btnTim);
        pnlBoLoc.Controls.Add(txtTimKiem);
        pnlBoLoc.Controls.Add(lblNhanDenNgay);
        pnlBoLoc.Controls.Add(lblNhanTuNgay);
        pnlBoLoc.Controls.Add(dtpDenNgay);
        pnlBoLoc.Controls.Add(dtpTuNgay);
        pnlBoLoc.Dock = DockStyle.Top;
        pnlBoLoc.Location = new System.Drawing.Point(0, 0);
        pnlBoLoc.Name = "pnlBoLoc";
        pnlBoLoc.Padding = new Padding(14, 12, 14, 12);
        pnlBoLoc.Size = new System.Drawing.Size(1168, 64);
        pnlBoLoc.TabIndex = 0;

        lblNhanTuNgay.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTuNgay);
        lblNhanTuNgay.Location = new System.Drawing.Point(14, 18);
        lblNhanTuNgay.Name = "lblNhanTuNgay";
        lblNhanTuNgay.TabIndex = 0;
        lblNhanTuNgay.Text = "Từ ngày";

        dtpTuNgay.Font = GiaoDien.ChuThuong;
        dtpTuNgay.Format = DateTimePickerFormat.Short;
        dtpTuNgay.Location = new System.Drawing.Point(14, 34);
        dtpTuNgay.Name = "dtpTuNgay";
        dtpTuNgay.Size = new System.Drawing.Size(140, 25);
        dtpTuNgay.TabIndex = 0;
        dtpTuNgay.ValueChanged += LocThayDoi;

        lblNhanDenNgay.AutoSize = true;
        GiaoDien.DangNhan(lblNhanDenNgay);
        lblNhanDenNgay.Location = new System.Drawing.Point(166, 18);
        lblNhanDenNgay.Name = "lblNhanDenNgay";
        lblNhanDenNgay.TabIndex = 2;
        lblNhanDenNgay.Text = "Đến ngày";

        dtpDenNgay.Font = GiaoDien.ChuThuong;
        dtpDenNgay.Format = DateTimePickerFormat.Short;
        dtpDenNgay.Location = new System.Drawing.Point(166, 34);
        dtpDenNgay.Name = "dtpDenNgay";
        dtpDenNgay.Size = new System.Drawing.Size(140, 25);
        dtpDenNgay.TabIndex = 1;
        dtpDenNgay.ValueChanged += LocThayDoi;

        cboLocSan.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLocSan.Font = GiaoDien.ChuThuong;
        cboLocSan.Location = new System.Drawing.Point(320, 33);
        cboLocSan.Name = "cboLocSan";
        cboLocSan.Size = new System.Drawing.Size(170, 25);
        cboLocSan.TabIndex = 2;
        cboLocSan.SelectedIndexChanged += LocThayDoi;

        cboLocTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLocTrangThai.Font = GiaoDien.ChuThuong;
        cboLocTrangThai.Location = new System.Drawing.Point(500, 33);
        cboLocTrangThai.Name = "cboLocTrangThai";
        cboLocTrangThai.Size = new System.Drawing.Size(150, 25);
        cboLocTrangThai.TabIndex = 3;
        cboLocTrangThai.SelectedIndexChanged += LocThayDoi;

        txtTimKiem.BackColor = GiaoDien.ManHinhNen;
        txtTimKiem.BorderStyle = BorderStyle.FixedSingle;
        txtTimKiem.Font = GiaoDien.ChuThuong;
        txtTimKiem.Location = new System.Drawing.Point(664, 34);
        txtTimKiem.Name = "txtTimKiem";
        txtTimKiem.Size = new System.Drawing.Size(230, 25);
        txtTimKiem.TabIndex = 4;
        txtTimKiem.KeyDown += txtTimKiem_KeyDown;

        GiaoDien.DangNutChinh(btnTim);
        btnTim.Location = new System.Drawing.Point(904, 30);
        btnTim.Name = "btnTim";
        btnTim.Size = new System.Drawing.Size(90, 34);
        btnTim.TabIndex = 5;
        btnTim.Text = "Lọc";
        btnTim.UseVisualStyleBackColor = false;
        btnTim.Click += btnTim_Click;

        GiaoDien.DangNutPhu(btnHomNay);
        btnHomNay.Location = new System.Drawing.Point(1000, 30);
        btnHomNay.Name = "btnHomNay";
        btnHomNay.Size = new System.Drawing.Size(80, 34);
        btnHomNay.TabIndex = 6;
        btnHomNay.Text = "Hôm nay";
        btnHomNay.UseVisualStyleBackColor = false;
        btnHomNay.Click += btnHomNay_Click;

        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Location = new System.Drawing.Point(1088, 30);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(66, 34);
        btnLamMoi.TabIndex = 7;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        dgvLich.Dock = DockStyle.Fill;
        dgvLich.Location = new System.Drawing.Point(0, 64);
        dgvLich.Name = "dgvLich";
        dgvLich.Size = new System.Drawing.Size(1168, 494);
        dgvLich.TabIndex = 1;
        dgvLich.SelectionChanged += dgvLich_SelectionChanged;

        pnlChan.BackColor = GiaoDien.BeMat;
        pnlChan.Controls.Add(lblThongKe);
        pnlChan.Controls.Add(btnLapHoaDon);
        pnlChan.Controls.Add(btnHuyBooking);
        pnlChan.Controls.Add(btnXemChiTiet);
        pnlChan.Dock = DockStyle.Bottom;
        pnlChan.Location = new System.Drawing.Point(0, 558);
        pnlChan.Name = "pnlChan";
        pnlChan.Padding = new Padding(14, 6, 14, 6);
        pnlChan.Size = new System.Drawing.Size(1168, 52);
        pnlChan.TabIndex = 2;

        lblThongKe.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
        lblThongKe.Font = GiaoDien.ChuNho;
        lblThongKe.ForeColor = GiaoDien.ChuPhu;
        lblThongKe.Location = new System.Drawing.Point(14, 18);
        lblThongKe.Name = "lblThongKe";
        lblThongKe.Size = new System.Drawing.Size(560, 20);
        lblThongKe.TabIndex = 0;
        lblThongKe.Text = "Tổng 0 lượt";

        GiaoDien.DangNutPhu(btnXemChiTiet);
        btnXemChiTiet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnXemChiTiet.Location = new System.Drawing.Point(730, 7);
        btnXemChiTiet.Name = "btnXemChiTiet";
        btnXemChiTiet.Size = new System.Drawing.Size(140, 38);
        btnXemChiTiet.TabIndex = 1;
        btnXemChiTiet.Text = "Chi tiết";
        btnXemChiTiet.UseVisualStyleBackColor = false;
        btnXemChiTiet.Click += btnXemChiTiet_Click;

        GiaoDien.DangNutNguyHiem(btnHuyBooking);
        btnHuyBooking.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnHuyBooking.Location = new System.Drawing.Point(880, 7);
        btnHuyBooking.Name = "btnHuyBooking";
        btnHuyBooking.Size = new System.Drawing.Size(130, 38);
        btnHuyBooking.TabIndex = 2;
        btnHuyBooking.Text = "Hủy booking";
        btnHuyBooking.UseVisualStyleBackColor = false;
        btnHuyBooking.Click += btnHuyBooking_Click;

        GiaoDien.DangNutChinh(btnLapHoaDon);
        btnLapHoaDon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLapHoaDon.Location = new System.Drawing.Point(1020, 7);
        btnLapHoaDon.Name = "btnLapHoaDon";
        btnLapHoaDon.Size = new System.Drawing.Size(134, 38);
        btnLapHoaDon.TabIndex = 3;
        btnLapHoaDon.Text = "Lập hóa đơn";
        btnLapHoaDon.UseVisualStyleBackColor = false;
        btnLapHoaDon.Click += btnLapHoaDon_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmLichDatSanAdmin";
        Text = "Lịch đặt sân";
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlLich.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvLich).EndInit();
        pnlBoLoc.ResumeLayout(false);
        pnlBoLoc.PerformLayout();
        pnlChan.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlDau;
    private Label lblMoTaTrang;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Panel pnlNoiDung;
    private Panel pnlLich;
    private DataGridView dgvLich;
    private Panel pnlBoLoc;
    private ComboBox cboLocTrangThai;
    private ComboBox cboLocSan;
    private Button btnHomNay;
    private Button btnLamMoi;
    private Button btnTim;
    private TextBox txtTimKiem;
    private Label lblNhanDenNgay;
    private Label lblNhanTuNgay;
    private DateTimePicker dtpDenNgay;
    private DateTimePicker dtpTuNgay;
    private Panel pnlChan;
    private Label lblThongKe;
    private Button btnLapHoaDon;
    private Button btnHuyBooking;
    private Button btnXemChiTiet;
}
