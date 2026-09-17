using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmTrangChuKhachHang
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
        lblLoiChao = new Label();
        picBieuTuong = new IconBox();
        pnlNoiDung = new Panel();
        pnlDuoi = new Panel();
        pnlVoucher = new Panel();
        dgvVoucher = new DataGridView();
        lblTieuDeVoucher = new Label();
        pnlSapToi = new Panel();
        lblThongBao = new Label();
        dgvSapToi = new DataGridView();
        lblTieuDeSapToi = new Label();
        pnlKpi = new Panel();
        kpiSanYeuThich = new KpiCard();
        kpiVoucher = new KpiCard();
        kpiChiTieu = new KpiCard();
        kpiSoLanDat = new KpiCard();
        pnlHanhDong = new Panel();
        btnDatSanNgay = new Button();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlDuoi.SuspendLayout();
        pnlVoucher.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvVoucher).BeginInit();
        pnlSapToi.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvSapToi).BeginInit();
        pnlKpi.SuspendLayout();
        pnlHanhDong.SuspendLayout();
        SuspendLayout();

        pnlDau.BackColor = GiaoDien.ThanhBen;
        pnlDau.Controls.Add(btnLamMoi);
        pnlDau.Controls.Add(lblLoiChao);
        pnlDau.Controls.Add(picBieuTuong);
        pnlDau.Dock = DockStyle.Top;
        pnlDau.Location = new System.Drawing.Point(0, 0);
        pnlDau.Name = "pnlDau";
        pnlDau.Size = new System.Drawing.Size(1200, 90);
        pnlDau.TabIndex = 0;

        picBieuTuong.Location = new System.Drawing.Point(20, 20);
        picBieuTuong.MauBieuTuong = GiaoDien.Chinh;
        picBieuTuong.Name = "picBieuTuong";
        picBieuTuong.Size = new System.Drawing.Size(48, 48);
        picBieuTuong.TabIndex = 0;
        picBieuTuong.TenBieuTuong = "sanbong";

        lblLoiChao.AutoSize = true;
        lblLoiChao.Font = GiaoDien.TieuDe;
        lblLoiChao.ForeColor = Color.White;
        lblLoiChao.Location = new System.Drawing.Point(80, 20);
        lblLoiChao.Name = "lblLoiChao";
        lblLoiChao.Size = new System.Drawing.Size(220, 31);
        lblLoiChao.TabIndex = 1;
        lblLoiChao.Text = "Xin chào, khách hàng!";

        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLamMoi.Location = new System.Drawing.Point(1060, 26);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(110, 34);
        btnLamMoi.TabIndex = 2;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        pnlNoiDung.Controls.Add(pnlDuoi);
        pnlNoiDung.Controls.Add(pnlKpi);
        pnlNoiDung.Controls.Add(pnlHanhDong);
        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(0, 90);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(16);
        pnlNoiDung.Size = new System.Drawing.Size(1200, 630);
        pnlNoiDung.TabIndex = 1;

        pnlHanhDong.BackColor = GiaoDien.BeMat;
        pnlHanhDong.Controls.Add(btnDatSanNgay);
        pnlHanhDong.Dock = DockStyle.Top;
        pnlHanhDong.Location = new System.Drawing.Point(16, 16);
        pnlHanhDong.Name = "pnlHanhDong";
        pnlHanhDong.Padding = new Padding(20, 14, 20, 14);
        pnlHanhDong.Size = new System.Drawing.Size(1168, 68);
        pnlHanhDong.TabIndex = 0;

        GiaoDien.DangNutChinh(btnDatSanNgay);
        btnDatSanNgay.Location = new System.Drawing.Point(20, 13);
        btnDatSanNgay.Name = "btnDatSanNgay";
        btnDatSanNgay.Size = new System.Drawing.Size(220, 42);
        btnDatSanNgay.TabIndex = 0;
        btnDatSanNgay.Text = "Đặt sân ngay";
        btnDatSanNgay.UseVisualStyleBackColor = false;
        btnDatSanNgay.Click += btnDatSanNgay_Click;

        pnlKpi.BackColor = GiaoDien.BeMat;
        pnlKpi.Controls.Add(kpiSanYeuThich);
        pnlKpi.Controls.Add(kpiVoucher);
        pnlKpi.Controls.Add(kpiChiTieu);
        pnlKpi.Controls.Add(kpiSoLanDat);
        pnlKpi.Dock = DockStyle.Top;
        pnlKpi.Location = new System.Drawing.Point(16, 84);
        pnlKpi.Name = "pnlKpi";
        pnlKpi.Padding = new Padding(0, 0, 0, 12);
        pnlKpi.Size = new System.Drawing.Size(1168, 122);
        pnlKpi.TabIndex = 1;

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
        kpiChiTieu.PhuDe = "Đã hủy: 0";
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
        kpiVoucher.TieuDe = "Voucher đã dùng";

        kpiSanYeuThich.GiaTri = "Chưa có";
        kpiSanYeuThich.Location = new System.Drawing.Point(888, 0);
        kpiSanYeuThich.MauNhan = GiaoDien.ChinhDam;
        kpiSanYeuThich.Name = "kpiSanYeuThich";
        kpiSanYeuThich.PhuDe = "";
        kpiSanYeuThich.Size = new System.Drawing.Size(280, 110);
        kpiSanYeuThich.TabIndex = 3;
        kpiSanYeuThich.TenBieuTuong = "sanbong";
        kpiSanYeuThich.TieuDe = "Sân yêu thích";

        pnlDuoi.BackColor = GiaoDien.BeMat;
        pnlDuoi.Controls.Add(pnlVoucher);
        pnlDuoi.Controls.Add(pnlSapToi);
        pnlDuoi.Dock = DockStyle.Top;
        pnlDuoi.Location = new System.Drawing.Point(16, 206);
        pnlDuoi.Name = "pnlDuoi";
        pnlDuoi.Padding = new Padding(0, 0, 0, 12);
        pnlDuoi.Size = new System.Drawing.Size(1168, 400);
        pnlDuoi.TabIndex = 2;

        pnlSapToi.BackColor = GiaoDien.BeMat;
        pnlSapToi.Controls.Add(lblThongBao);
        pnlSapToi.Controls.Add(dgvSapToi);
        pnlSapToi.Controls.Add(lblTieuDeSapToi);
        pnlSapToi.Dock = DockStyle.Fill;
        pnlSapToi.Location = new System.Drawing.Point(0, 0);
        pnlSapToi.Name = "pnlSapToi";
        pnlSapToi.Padding = new Padding(0, 0, 12, 0);
        pnlSapToi.Size = new System.Drawing.Size(760, 400);
        pnlSapToi.TabIndex = 0;

        lblTieuDeSapToi.AutoSize = true;
        lblTieuDeSapToi.Font = GiaoDien.ChuLon;
        lblTieuDeSapToi.ForeColor = GiaoDien.Chu;
        lblTieuDeSapToi.Location = new System.Drawing.Point(0, 0);
        lblTieuDeSapToi.Name = "lblTieuDeSapToi";
        lblTieuDeSapToi.Size = new System.Drawing.Size(200, 25);
        lblTieuDeSapToi.TabIndex = 0;
        lblTieuDeSapToi.Text = "Lịch sắp tới của bạn";

        dgvSapToi.Dock = DockStyle.Fill;
        dgvSapToi.Location = new System.Drawing.Point(0, 28);
        dgvSapToi.Name = "dgvSapToi";
        dgvSapToi.Size = new System.Drawing.Size(748, 318);
        dgvSapToi.TabIndex = 1;

        lblThongBao.AutoSize = true;
        lblThongBao.Dock = DockStyle.Bottom;
        lblThongBao.Font = GiaoDien.ChuNho;
        lblThongBao.ForeColor = GiaoDien.ChuPhu;
        lblThongBao.Location = new System.Drawing.Point(0, 346);
        lblThongBao.Name = "lblThongBao";
        lblThongBao.Size = new System.Drawing.Size(748, 54);
        lblThongBao.TabIndex = 2;
        lblThongBao.Text = "Bạn chưa có lịch đặt sân sắp tới.";

        pnlVoucher.BackColor = GiaoDien.BeMat;
        pnlVoucher.Controls.Add(dgvVoucher);
        pnlVoucher.Controls.Add(lblTieuDeVoucher);
        pnlVoucher.Dock = DockStyle.Right;
        pnlVoucher.Location = new System.Drawing.Point(760, 0);
        pnlVoucher.Name = "pnlVoucher";
        pnlVoucher.Size = new System.Drawing.Size(408, 400);
        pnlVoucher.TabIndex = 1;

        lblTieuDeVoucher.AutoSize = true;
        lblTieuDeVoucher.Font = GiaoDien.ChuLon;
        lblTieuDeVoucher.ForeColor = GiaoDien.Chu;
        lblTieuDeVoucher.Location = new System.Drawing.Point(0, 0);
        lblTieuDeVoucher.Name = "lblTieuDeVoucher";
        lblTieuDeVoucher.Size = new System.Drawing.Size(240, 25);
        lblTieuDeVoucher.TabIndex = 0;
        lblTieuDeVoucher.Text = "Voucher đang có thể dùng";

        dgvVoucher.Dock = DockStyle.Fill;
        dgvVoucher.Location = new System.Drawing.Point(0, 28);
        dgvVoucher.Name = "dgvVoucher";
        dgvVoucher.Size = new System.Drawing.Size(408, 372);
        dgvVoucher.TabIndex = 1;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1100, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmTrangChuKhachHang";
        Text = "Trang chủ";
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlDuoi.ResumeLayout(false);
        pnlVoucher.ResumeLayout(false);
        pnlVoucher.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvVoucher).EndInit();
        pnlSapToi.ResumeLayout(false);
        pnlSapToi.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvSapToi).EndInit();
        pnlKpi.ResumeLayout(false);
        pnlHanhDong.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlDau;
    private Button btnLamMoi;
    private Label lblLoiChao;
    private IconBox picBieuTuong;
    private Panel pnlNoiDung;
    private Panel pnlDuoi;
    private Panel pnlVoucher;
    private DataGridView dgvVoucher;
    private Label lblTieuDeVoucher;
    private Panel pnlSapToi;
    private Label lblThongBao;
    private DataGridView dgvSapToi;
    private Label lblTieuDeSapToi;
    private Panel pnlKpi;
    private KpiCard kpiSanYeuThich;
    private KpiCard kpiVoucher;
    private KpiCard kpiChiTieu;
    private KpiCard kpiSoLanDat;
    private Panel pnlHanhDong;
    private Button btnDatSanNgay;
}
