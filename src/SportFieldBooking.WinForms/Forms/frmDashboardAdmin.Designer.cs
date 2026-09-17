using ScottPlot.WinForms;
using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmDashboardAdmin
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
        btnLamMoi = new Button();
        lblMoTaTrang = new Label();
        lblTieuDe = new Label();
        picBieuTuong = new IconBox();
        pnlNoiDung = new Panel();
        pnlKpi = new Panel();
        kpiSan = new KpiCard();
        kpiChuaThanhToan = new KpiCard();
        kpiBooking = new KpiCard();
        kpiDoanhThu = new KpiCard();
        pnlBieuDo = new Panel();
        plotTopSan = new FormsPlot();
        plotSan = new FormsPlot();
        plotDoanhThu = new FormsPlot();
        pnlDuoi = new Panel();
        dgvHomNay = new DataGridView();
        lblTieuDeHomNay = new Label();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlKpi.SuspendLayout();
        pnlBieuDo.SuspendLayout();
        pnlDuoi.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHomNay).BeginInit();
        SuspendLayout();

        pnlDau.BackColor = GiaoDien.ThanhBen;
        pnlDau.Controls.Add(btnLamMoi);
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
        lblTieuDe.Text = "Tổng quan hệ thống";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(360, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Số liệu tổng hợp, doanh thu gần đây và lịch sân hôm nay";

        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLamMoi.Location = new System.Drawing.Point(1054, 22);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(120, 34);
        btnLamMoi.TabIndex = 3;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Controls.Add(pnlDuoi);
        pnlNoiDung.Controls.Add(pnlBieuDo);
        pnlNoiDung.Controls.Add(pnlKpi);
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(0, 78);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(16);
        pnlNoiDung.Size = new System.Drawing.Size(1200, 642);
        pnlNoiDung.TabIndex = 1;

        pnlKpi.BackColor = GiaoDien.BeMat;
        pnlKpi.Controls.Add(kpiSan);
        pnlKpi.Controls.Add(kpiChuaThanhToan);
        pnlKpi.Controls.Add(kpiBooking);
        pnlKpi.Controls.Add(kpiDoanhThu);
        pnlKpi.Dock = DockStyle.Top;
        pnlKpi.Location = new System.Drawing.Point(16, 16);
        pnlKpi.Name = "pnlKpi";
        pnlKpi.Padding = new Padding(0, 0, 0, 12);
        pnlKpi.Size = new System.Drawing.Size(1168, 122);
        pnlKpi.TabIndex = 0;

        kpiDoanhThu.GiaTri = "0 đ";
        kpiDoanhThu.Location = new System.Drawing.Point(0, 0);
        kpiDoanhThu.MauNhan = GiaoDien.Chinh;
        kpiDoanhThu.Name = "kpiDoanhThu";
        kpiDoanhThu.PhuDe = "30 ngày: 0 đ";
        kpiDoanhThu.Size = new System.Drawing.Size(280, 110);
        kpiDoanhThu.TabIndex = 0;
        kpiDoanhThu.TenBieuTuong = "hoadon";
        kpiDoanhThu.TieuDe = "Doanh thu hôm nay";

        kpiBooking.GiaTri = "0";
        kpiBooking.Location = new System.Drawing.Point(296, 0);
        kpiBooking.MauNhan = GiaoDien.ThanhCong;
        kpiBooking.Name = "kpiBooking";
        kpiBooking.PhuDe = "30 ngày: 0";
        kpiBooking.Size = new System.Drawing.Size(280, 110);
        kpiBooking.TabIndex = 1;
        kpiBooking.TenBieuTuong = "calendar";
        kpiBooking.TieuDe = "Booking hôm nay";

        kpiChuaThanhToan.GiaTri = "0";
        kpiChuaThanhToan.Location = new System.Drawing.Point(592, 0);
        kpiChuaThanhToan.MauNhan = GiaoDien.CanhBao;
        kpiChuaThanhToan.Name = "kpiChuaThanhToan";
        kpiChuaThanhToan.PhuDe = "0 khách hàng";
        kpiChuaThanhToan.Size = new System.Drawing.Size(280, 110);
        kpiChuaThanhToan.TabIndex = 2;
        kpiChuaThanhToan.TenBieuTuong = "canhbao";
        kpiChuaThanhToan.TieuDe = "Hóa đơn chưa thu";

        kpiSan.GiaTri = "0/0";
        kpiSan.Location = new System.Drawing.Point(888, 0);
        kpiSan.MauNhan = GiaoDien.ChinhDam;
        kpiSan.Name = "kpiSan";
        kpiSan.PhuDe = "Đang thuê: 0";
        kpiSan.Size = new System.Drawing.Size(280, 110);
        kpiSan.TabIndex = 3;
        kpiSan.TenBieuTuong = "sanbong";
        kpiSan.TieuDe = "Tình trạng sân";

        pnlBieuDo.BackColor = GiaoDien.BeMat;
        pnlBieuDo.Controls.Add(plotTopSan);
        pnlBieuDo.Controls.Add(plotSan);
        pnlBieuDo.Controls.Add(plotDoanhThu);
        pnlBieuDo.Dock = DockStyle.Top;
        pnlBieuDo.Location = new System.Drawing.Point(16, 138);
        pnlBieuDo.Name = "pnlBieuDo";
        pnlBieuDo.Padding = new Padding(0, 0, 0, 12);
        pnlBieuDo.Size = new System.Drawing.Size(1168, 262);
        pnlBieuDo.TabIndex = 1;

        plotDoanhThu.BackColor = GiaoDien.BeMat;
        plotDoanhThu.Dock = DockStyle.Left;
        plotDoanhThu.Location = new System.Drawing.Point(0, 0);
        plotDoanhThu.Name = "plotDoanhThu";
        plotDoanhThu.Size = new System.Drawing.Size(560, 250);
        plotDoanhThu.TabIndex = 0;

        plotSan.BackColor = GiaoDien.BeMat;
        plotSan.Dock = DockStyle.Left;
        plotSan.Location = new System.Drawing.Point(560, 0);
        plotSan.Name = "plotSan";
        plotSan.Size = new System.Drawing.Size(300, 250);
        plotSan.TabIndex = 1;

        plotTopSan.BackColor = GiaoDien.BeMat;
        plotTopSan.Dock = DockStyle.Fill;
        plotTopSan.Location = new System.Drawing.Point(860, 0);
        plotTopSan.Name = "plotTopSan";
        plotTopSan.Size = new System.Drawing.Size(308, 250);
        plotTopSan.TabIndex = 2;

        pnlDuoi.BackColor = GiaoDien.BeMat;
        pnlDuoi.Controls.Add(dgvHomNay);
        pnlDuoi.Controls.Add(lblTieuDeHomNay);
        pnlDuoi.Dock = DockStyle.Top;
        pnlDuoi.Location = new System.Drawing.Point(16, 400);
        pnlDuoi.Name = "pnlDuoi";
        pnlDuoi.Padding = new Padding(0, 8, 0, 8);
        pnlDuoi.Size = new System.Drawing.Size(1168, 252);
        pnlDuoi.TabIndex = 2;

        lblTieuDeHomNay.AutoSize = true;
        lblTieuDeHomNay.Font = GiaoDien.ChuLon;
        lblTieuDeHomNay.ForeColor = GiaoDien.Chu;
        lblTieuDeHomNay.Location = new System.Drawing.Point(0, 8);
        lblTieuDeHomNay.Name = "lblTieuDeHomNay";
        lblTieuDeHomNay.Size = new System.Drawing.Size(200, 25);
        lblTieuDeHomNay.TabIndex = 0;
        lblTieuDeHomNay.Text = "Lịch sân hôm nay";

        dgvHomNay.Dock = DockStyle.Fill;
        dgvHomNay.Location = new System.Drawing.Point(0, 44);
        dgvHomNay.Name = "dgvHomNay";
        dgvHomNay.Size = new System.Drawing.Size(1168, 200);
        dgvHomNay.TabIndex = 1;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmDashboardAdmin";
        Text = "Tổng quan hệ thống";
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlKpi.ResumeLayout(false);
        pnlBieuDo.ResumeLayout(false);
        pnlDuoi.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvHomNay).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlDau;
    private Button btnLamMoi;
    private Label lblMoTaTrang;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Panel pnlNoiDung;
    private Panel pnlKpi;
    private KpiCard kpiSan;
    private KpiCard kpiChuaThanhToan;
    private KpiCard kpiBooking;
    private KpiCard kpiDoanhThu;
    private Panel pnlBieuDo;
    private FormsPlot plotTopSan;
    private FormsPlot plotSan;
    private FormsPlot plotDoanhThu;
    private Panel pnlDuoi;
    private DataGridView dgvHomNay;
    private Label lblTieuDeHomNay;
}
