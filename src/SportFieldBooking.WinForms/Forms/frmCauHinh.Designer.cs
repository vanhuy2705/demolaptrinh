using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmCauHinh
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
        pnlChinh = new Panel();
        dgvThamSo = new DataGridView();
        pnlNut = new Panel();
        btnLamMoi = new Button();
        btnMacDinh = new Button();
        btnLuu = new Button();
        lblGhiChu = new Label();
        ((System.ComponentModel.ISupportInitialize)errLoi).BeginInit();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlChinh.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvThamSo).BeginInit();
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
        picBieuTuong.TenBieuTuong = "cauhinh";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(76, 14);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(200, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Cấu hình hệ thống";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(420, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Phần trăm giảm cuối tuần, thời lượng block, giờ hoạt động, thông tin trung tâm";

        pnlNoiDung.Controls.Add(pnlChinh);
        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(0, 78);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(16);
        pnlNoiDung.Size = new System.Drawing.Size(1200, 642);
        pnlNoiDung.TabIndex = 1;

        pnlChinh.BackColor = GiaoDien.BeMat;
        pnlChinh.Controls.Add(lblGhiChu);
        pnlChinh.Controls.Add(dgvThamSo);
        pnlChinh.Controls.Add(pnlNut);
        pnlChinh.Dock = DockStyle.Fill;
        pnlChinh.Location = new System.Drawing.Point(16, 16);
        pnlChinh.Name = "pnlChinh";
        pnlChinh.Size = new System.Drawing.Size(1168, 610);
        pnlChinh.TabIndex = 0;

        dgvThamSo.Dock = DockStyle.Fill;
        dgvThamSo.Location = new System.Drawing.Point(0, 0);
        dgvThamSo.Name = "dgvThamSo";
        dgvThamSo.Size = new System.Drawing.Size(1168, 470);
        dgvThamSo.TabIndex = 0;

        lblGhiChu.BackColor = GiaoDien.ManHinhNen;
        lblGhiChu.Dock = DockStyle.Bottom;
        lblGhiChu.Font = GiaoDien.ChuNho;
        lblGhiChu.ForeColor = GiaoDien.ChuPhu;
        lblGhiChu.Location = new System.Drawing.Point(0, 470);
        lblGhiChu.Name = "lblGhiChu";
        lblGhiChu.Padding = new Padding(14, 10, 14, 10);
        lblGhiChu.Size = new System.Drawing.Size(1168, 80);
        lblGhiChu.TabIndex = 1;
        lblGhiChu.Text = "Hướng dẫn:" + Environment.NewLine +
            "- PhanTramGiamCuoiTuan: % giảm tự động cho booking vào Thứ Bảy / Chủ Nhật (0 - 100)." + Environment.NewLine +
            "- ThoiLuongBlockPhut: thời lượng một block tính tiền, mặc định 30 phút (17:00 - 18:10 = 1.5 giờ)." + Environment.NewLine +
            "- GioMoCua / GioDongCua: khung giờ hoạt động, định dạng HH:mm. Voucher luôn được ưu tiên hơn khuyến mãi và giảm cuối tuần.";

        pnlNut.BackColor = GiaoDien.BeMat;
        pnlNut.Controls.Add(btnLuu);
        pnlNut.Controls.Add(btnMacDinh);
        pnlNut.Controls.Add(btnLamMoi);
        pnlNut.Dock = DockStyle.Bottom;
        pnlNut.Location = new System.Drawing.Point(0, 550);
        pnlNut.Name = "pnlNut";
        pnlNut.Padding = new Padding(14, 10, 14, 10);
        pnlNut.Size = new System.Drawing.Size(1168, 60);
        pnlNut.TabIndex = 2;

        GiaoDien.DangNutChinh(btnLuu);
        btnLuu.Location = new System.Drawing.Point(824, 11);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new System.Drawing.Size(120, 38);
        btnLuu.TabIndex = 0;
        btnLuu.Text = "Lưu cấu hình";
        btnLuu.UseVisualStyleBackColor = false;
        btnLuu.Click += btnLuu_Click;

        GiaoDien.DangNutPhu(btnMacDinh);
        btnMacDinh.Location = new System.Drawing.Point(950, 11);
        btnMacDinh.Name = "btnMacDinh";
        btnMacDinh.Size = new System.Drawing.Size(90, 38);
        btnMacDinh.TabIndex = 1;
        btnMacDinh.Text = "Mặc định";
        btnMacDinh.UseVisualStyleBackColor = false;
        btnMacDinh.Click += btnMacDinh_Click;

        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Location = new System.Drawing.Point(1046, 11);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(100, 38);
        btnLamMoi.TabIndex = 2;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmCauHinh";
        Text = "Cấu hình hệ thống";
        ((System.ComponentModel.ISupportInitialize)errLoi).EndInit();
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlChinh.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvThamSo).EndInit();
        pnlNut.ResumeLayout(false);
        ResumeLayout(false);
    }

    private ErrorProvider errLoi;
    private Panel pnlDau;
    private Label lblMoTaTrang;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Panel pnlNoiDung;
    private Panel pnlChinh;
    private DataGridView dgvThamSo;
    private Label lblGhiChu;
    private Panel pnlNut;
    private Button btnLamMoi;
    private Button btnMacDinh;
    private Button btnLuu;
}
