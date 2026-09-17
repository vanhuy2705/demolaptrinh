using ScottPlot.WinForms;
using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmThongKeAdmin
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
        pnlBoLoc = new Panel();
        cboKieuBieuDo = new ComboBox();
        btnLamMoi = new Button();
        btnThangNay = new Button();
        btnBayNgay = new Button();
        btnHomNay = new Button();
        btnXem = new Button();
        lblNhanDenNgay = new Label();
        lblNhanTuNgay = new Label();
        dtpDenNgay = new DateTimePicker();
        dtpTuNgay = new DateTimePicker();
        pnlKpi = new Panel();
        kpiGiamGia = new KpiCard();
        kpiChuaThanhToan = new KpiCard();
        kpiBooking = new KpiCard();
        kpiDoanhThu = new KpiCard();
        pnlBieuDo = new Panel();
        plotTron = new FormsPlot();
        plotDuong = new FormsPlot();
        pnlDuoi = new Panel();
        dgvGiamGia = new DataGridView();
        lblTieuDeGiamGia = new Label();
        pnlTopSan = new Panel();
        lblTieuDeChiTiet = new Label();
        dgvChiTietNgay = new DataGridView();
        plotTopSan = new FormsPlot();
        dgvTopSan = new DataGridView();
        lblTieuDeTopSan = new Label();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlBoLoc.SuspendLayout();
        pnlKpi.SuspendLayout();
        pnlBieuDo.SuspendLayout();
        pnlDuoi.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvGiamGia).BeginInit();
        pnlTopSan.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvChiTietNgay).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvTopSan).BeginInit();
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
        picBieuTuong.TenBieuTuong = "thongke";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(76, 14);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(200, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Thống kê & báo cáo";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(400, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Doanh thu, cơ cấu giảm giá, top sân theo khoảng thời gian";

        pnlNoiDung.Controls.Add(pnlDuoi);
        pnlNoiDung.Controls.Add(pnlBieuDo);
        pnlNoiDung.Controls.Add(pnlKpi);
        pnlNoiDung.Controls.Add(pnlBoLoc);
        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(0, 78);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(16);
        pnlNoiDung.Size = new System.Drawing.Size(1200, 642);
        pnlNoiDung.TabIndex = 1;

        pnlBoLoc.BackColor = GiaoDien.BeMat;
        pnlBoLoc.Controls.Add(cboKieuBieuDo);
        pnlBoLoc.Controls.Add(btnLamMoi);
        pnlBoLoc.Controls.Add(btnThangNay);
        pnlBoLoc.Controls.Add(btnBayNgay);
        pnlBoLoc.Controls.Add(btnHomNay);
        pnlBoLoc.Controls.Add(btnXem);
        pnlBoLoc.Controls.Add(lblNhanDenNgay);
        pnlBoLoc.Controls.Add(lblNhanTuNgay);
        pnlBoLoc.Controls.Add(dtpDenNgay);
        pnlBoLoc.Controls.Add(dtpTuNgay);
        pnlBoLoc.Dock = DockStyle.Top;
        pnlBoLoc.Location = new System.Drawing.Point(16, 16);
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

        GiaoDien.DangNutChinh(btnXem);
        btnXem.Location = new System.Drawing.Point(320, 30);
        btnXem.Name = "btnXem";
        btnXem.Size = new System.Drawing.Size(90, 34);
        btnXem.TabIndex = 2;
        btnXem.Text = "Xem";
        btnXem.UseVisualStyleBackColor = false;
        btnXem.Click += btnXem_Click;

        GiaoDien.DangNutPhu(btnHomNay);
        btnHomNay.Location = new System.Drawing.Point(420, 30);
        btnHomNay.Name = "btnHomNay";
        btnHomNay.Size = new System.Drawing.Size(90, 34);
        btnHomNay.TabIndex = 3;
        btnHomNay.Text = "Hôm nay";
        btnHomNay.UseVisualStyleBackColor = false;
        btnHomNay.Click += btnHomNay_Click;

        GiaoDien.DangNutPhu(btnBayNgay);
        btnBayNgay.Location = new System.Drawing.Point(520, 30);
        btnBayNgay.Name = "btnBayNgay";
        btnBayNgay.Size = new System.Drawing.Size(90, 34);
        btnBayNgay.TabIndex = 4;
        btnBayNgay.Text = "7 ngày";
        btnBayNgay.UseVisualStyleBackColor = false;
        btnBayNgay.Click += btnBayNgay_Click;

        GiaoDien.DangNutPhu(btnThangNay);
        btnThangNay.Location = new System.Drawing.Point(620, 30);
        btnThangNay.Name = "btnThangNay";
        btnThangNay.Size = new System.Drawing.Size(100, 34);
        btnThangNay.TabIndex = 5;
        btnThangNay.Text = "Tháng này";
        btnThangNay.UseVisualStyleBackColor = false;
        btnThangNay.Click += btnThangNay_Click;

        cboKieuBieuDo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboKieuBieuDo.Font = GiaoDien.ChuThuong;
        cboKieuBieuDo.Location = new System.Drawing.Point(730, 33);
        cboKieuBieuDo.Name = "cboKieuBieuDo";
        cboKieuBieuDo.Size = new System.Drawing.Size(220, 25);
        cboKieuBieuDo.TabIndex = 6;
        cboKieuBieuDo.SelectedIndexChanged += cboKieuBieuDo_SelectedIndexChanged;

        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Location = new System.Drawing.Point(960, 30);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(100, 34);
        btnLamMoi.TabIndex = 7;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        pnlKpi.BackColor = GiaoDien.BeMat;
        pnlKpi.Controls.Add(kpiGiamGia);
        pnlKpi.Controls.Add(kpiChuaThanhToan);
        pnlKpi.Controls.Add(kpiBooking);
        pnlKpi.Controls.Add(kpiDoanhThu);
        pnlKpi.Dock = DockStyle.Top;
        pnlKpi.Location = new System.Drawing.Point(16, 80);
        pnlKpi.Name = "pnlKpi";
        pnlKpi.Padding = new Padding(0, 0, 0, 12);
        pnlKpi.Size = new System.Drawing.Size(1168, 122);
        pnlKpi.TabIndex = 1;

        kpiDoanhThu.GiaTri = "0 đ";
        kpiDoanhThu.Location = new System.Drawing.Point(0, 0);
        kpiDoanhThu.MauNhan = GiaoDien.Chinh;
        kpiDoanhThu.Name = "kpiDoanhThu";
        kpiDoanhThu.PhuDe = "Hôm nay: 0 đ";
        kpiDoanhThu.Size = new System.Drawing.Size(280, 110);
        kpiDoanhThu.TabIndex = 0;
        kpiDoanhThu.TenBieuTuong = "hoadon";
        kpiDoanhThu.TieuDe = "Doanh thu kỳ";

        kpiBooking.GiaTri = "0";
        kpiBooking.Location = new System.Drawing.Point(296, 0);
        kpiBooking.MauNhan = GiaoDien.ThanhCong;
        kpiBooking.Name = "kpiBooking";
        kpiBooking.PhuDe = "Hôm nay: 0";
        kpiBooking.Size = new System.Drawing.Size(280, 110);
        kpiBooking.TabIndex = 1;
        kpiBooking.TenBieuTuong = "calendar";
        kpiBooking.TieuDe = "Booking trong kỳ";

        kpiChuaThanhToan.GiaTri = "0";
        kpiChuaThanhToan.Location = new System.Drawing.Point(592, 0);
        kpiChuaThanhToan.MauNhan = GiaoDien.CanhBao;
        kpiChuaThanhToan.Name = "kpiChuaThanhToan";
        kpiChuaThanhToan.PhuDe = "0 khách hàng";
        kpiChuaThanhToan.Size = new System.Drawing.Size(280, 110);
        kpiChuaThanhToan.TabIndex = 2;
        kpiChuaThanhToan.TenBieuTuong = "canhbao";
        kpiChuaThanhToan.TieuDe = "Hóa đơn chưa thu";

        kpiGiamGia.GiaTri = "0 đ";
        kpiGiamGia.Location = new System.Drawing.Point(888, 0);
        kpiGiamGia.MauNhan = GiaoDien.ChinhDam;
        kpiGiamGia.Name = "kpiGiamGia";
        kpiGiamGia.PhuDe = "Sân trống: 0/0";
        kpiGiamGia.Size = new System.Drawing.Size(280, 110);
        kpiGiamGia.TabIndex = 3;
        kpiGiamGia.TenBieuTuong = "voucher";
        kpiGiamGia.TieuDe = "Tổng tiền giảm";

        pnlBieuDo.BackColor = GiaoDien.BeMat;
        pnlBieuDo.Controls.Add(plotTron);
        pnlBieuDo.Controls.Add(plotDuong);
        pnlBieuDo.Dock = DockStyle.Top;
        pnlBieuDo.Location = new System.Drawing.Point(16, 202);
        pnlBieuDo.Name = "pnlBieuDo";
        pnlBieuDo.Padding = new Padding(0, 0, 0, 12);
        pnlBieuDo.Size = new System.Drawing.Size(1168, 262);
        pnlBieuDo.TabIndex = 2;

        plotDuong.BackColor = GiaoDien.BeMat;
        plotDuong.Dock = DockStyle.Left;
        plotDuong.Location = new System.Drawing.Point(0, 0);
        plotDuong.Name = "plotDuong";
        plotDuong.Size = new System.Drawing.Size(740, 250);
        plotDuong.TabIndex = 0;

        plotTron.BackColor = GiaoDien.BeMat;
        plotTron.Dock = DockStyle.Fill;
        plotTron.Location = new System.Drawing.Point(740, 0);
        plotTron.Name = "plotTron";
        plotTron.Size = new System.Drawing.Size(428, 250);
        plotTron.TabIndex = 1;

        pnlDuoi.BackColor = GiaoDien.BeMat;
        pnlDuoi.Controls.Add(dgvGiamGia);
        pnlDuoi.Controls.Add(lblTieuDeGiamGia);
        pnlDuoi.Controls.Add(pnlTopSan);
        pnlDuoi.Controls.Add(dgvChiTietNgay);
        pnlDuoi.Controls.Add(lblTieuDeChiTiet);
        pnlDuoi.Dock = DockStyle.Fill;
        pnlDuoi.Location = new System.Drawing.Point(16, 464);
        pnlDuoi.Name = "pnlDuoi";
        pnlDuoi.Size = new System.Drawing.Size(1168, 162);
        pnlDuoi.TabIndex = 3;

        lblTieuDeChiTiet.AutoSize = true;
        lblTieuDeChiTiet.Font = GiaoDien.ChuLon;
        lblTieuDeChiTiet.ForeColor = GiaoDien.Chu;
        lblTieuDeChiTiet.Location = new System.Drawing.Point(0, 0);
        lblTieuDeChiTiet.Name = "lblTieuDeChiTiet";
        lblTieuDeChiTiet.Size = new System.Drawing.Size(180, 25);
        lblTieuDeChiTiet.TabIndex = 0;
        lblTieuDeChiTiet.Text = "Chi tiết theo ngày";

        dgvChiTietNgay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        dgvChiTietNgay.Location = new System.Drawing.Point(0, 28);
        dgvChiTietNgay.Name = "dgvChiTietNgay";
        dgvChiTietNgay.Size = new System.Drawing.Size(420, 134);
        dgvChiTietNgay.TabIndex = 1;

        pnlTopSan.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        pnlTopSan.Controls.Add(plotTopSan);
        pnlTopSan.Controls.Add(dgvTopSan);
        pnlTopSan.Controls.Add(lblTieuDeTopSan);
        pnlTopSan.Location = new System.Drawing.Point(436, 0);
        pnlTopSan.Name = "pnlTopSan";
        pnlTopSan.Size = new System.Drawing.Size(420, 162);
        pnlTopSan.TabIndex = 2;

        lblTieuDeTopSan.AutoSize = true;
        lblTieuDeTopSan.Font = GiaoDien.ChuLon;
        lblTieuDeTopSan.ForeColor = GiaoDien.Chu;
        lblTieuDeTopSan.Location = new System.Drawing.Point(0, 0);
        lblTieuDeTopSan.Name = "lblTieuDeTopSan";
        lblTieuDeTopSan.Size = new System.Drawing.Size(140, 25);
        lblTieuDeTopSan.TabIndex = 0;
        lblTieuDeTopSan.Text = "Top sân";

        plotTopSan.BackColor = GiaoDien.BeMat;
        plotTopSan.Dock = DockStyle.Bottom;
        plotTopSan.Location = new System.Drawing.Point(0, 52);
        plotTopSan.Name = "plotTopSan";
        plotTopSan.Size = new System.Drawing.Size(420, 0);
        plotTopSan.TabIndex = 1;

        dgvTopSan.Dock = DockStyle.Fill;
        dgvTopSan.Location = new System.Drawing.Point(0, 28);
        dgvTopSan.Name = "dgvTopSan";
        dgvTopSan.Size = new System.Drawing.Size(420, 134);
        dgvTopSan.TabIndex = 2;

        lblTieuDeGiamGia.AutoSize = true;
        lblTieuDeGiamGia.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lblTieuDeGiamGia.Font = GiaoDien.ChuLon;
        lblTieuDeGiamGia.ForeColor = GiaoDien.Chu;
        lblTieuDeGiamGia.Location = new System.Drawing.Point(880, 0);
        lblTieuDeGiamGia.Name = "lblTieuDeGiamGia";
        lblTieuDeGiamGia.Size = new System.Drawing.Size(180, 25);
        lblTieuDeGiamGia.TabIndex = 3;
        lblTieuDeGiamGia.Text = "Cơ cấu giảm giá";

        dgvGiamGia.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        dgvGiamGia.Location = new System.Drawing.Point(880, 28);
        dgvGiamGia.Name = "dgvGiamGia";
        dgvGiamGia.Size = new System.Drawing.Size(288, 134);
        dgvGiamGia.TabIndex = 4;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmThongKeAdmin";
        Text = "Thống kê & báo cáo";
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlBoLoc.ResumeLayout(false);
        pnlBoLoc.PerformLayout();
        pnlKpi.ResumeLayout(false);
        pnlBieuDo.ResumeLayout(false);
        pnlDuoi.ResumeLayout(false);
        pnlDuoi.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvGiamGia).EndInit();
        pnlTopSan.ResumeLayout(false);
        pnlTopSan.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvChiTietNgay).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvTopSan).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlDau;
    private Label lblMoTaTrang;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Panel pnlNoiDung;
    private Panel pnlBoLoc;
    private ComboBox cboKieuBieuDo;
    private Button btnLamMoi;
    private Button btnThangNay;
    private Button btnBayNgay;
    private Button btnHomNay;
    private Button btnXem;
    private Label lblNhanDenNgay;
    private Label lblNhanTuNgay;
    private DateTimePicker dtpDenNgay;
    private DateTimePicker dtpTuNgay;
    private Panel pnlKpi;
    private KpiCard kpiGiamGia;
    private KpiCard kpiChuaThanhToan;
    private KpiCard kpiBooking;
    private KpiCard kpiDoanhThu;
    private Panel pnlBieuDo;
    private FormsPlot plotDuong;
    private FormsPlot plotTron;
    private Panel pnlDuoi;
    private DataGridView dgvGiamGia;
    private Label lblTieuDeGiamGia;
    private Panel pnlTopSan;
    private DataGridView dgvTopSan;
    private Label lblTieuDeTopSan;
    private DataGridView dgvChiTietNgay;
    private Label lblTieuDeChiTiet;
    private FormsPlot plotTopSan;
}
