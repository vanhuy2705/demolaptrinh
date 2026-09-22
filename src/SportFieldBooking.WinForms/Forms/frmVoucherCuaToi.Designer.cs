using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmVoucherCuaToi
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
        pnlVoucher = new Panel();
        dgvVoucher = new DataGridView();
        pnlBoLoc = new Panel();
        btnLamMoi = new Button();
        btnTim = new Button();
        txtTimKiem = new TextBox();
        lblTieuDeVoucher = new Label();
        pnlLichSu = new Panel();
        dgvLichSu = new DataGridView();
        pnlChan = new Panel();
        lblThongKe = new Label();
        lblTieuDeLichSu = new Label();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlVoucher.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvVoucher).BeginInit();
        pnlBoLoc.SuspendLayout();
        pnlLichSu.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLichSu).BeginInit();
        pnlChan.SuspendLayout();
        SuspendLayout();

        pnlDau.BackColor = GiaoDien.ThanhBen;
        pnlDau.Controls.Add(lblMoTaTrang);
        pnlDau.Controls.Add(lblTieuDe);
        pnlDau.Controls.Add(picBieuTuong);
        pnlDau.Dock = DockStyle.Top;
        pnlDau.Location = new System.Drawing.Point(0, 0);
        pnlDau.Name = "pnlDau";
        pnlDau.Size = new System.Drawing.Size(860, 78);
        pnlDau.TabIndex = 0;

        picBieuTuong.Location = new System.Drawing.Point(20, 16);
        picBieuTuong.MauBieuTuong = GiaoDien.Chinh;
        picBieuTuong.Name = "picBieuTuong";
        picBieuTuong.Size = new System.Drawing.Size(44, 44);
        picBieuTuong.TabIndex = 0;
        picBieuTuong.TenBieuTuong = "voucher";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(76, 14);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(200, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Voucher của tôi";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(420, 15);
        lblMoTaTrang.Text = "Danh sách mã giảm giá còn hạn và lịch sử đã sử dụng của bạn";

        pnlNoiDung.Controls.Add(pnlLichSu);
        pnlNoiDung.Controls.Add(pnlVoucher);
        pnlNoiDung.Controls.Add(pnlChan);
        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(0, 78);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(16);
        pnlNoiDung.Size = new System.Drawing.Size(860, 542);
        pnlNoiDung.TabIndex = 1;

        pnlVoucher.BackColor = GiaoDien.BeMat;
        pnlVoucher.Controls.Add(dgvVoucher);
        pnlVoucher.Controls.Add(pnlBoLoc);
        pnlVoucher.Controls.Add(lblTieuDeVoucher);
        pnlVoucher.Dock = DockStyle.Fill;
        pnlVoucher.Location = new System.Drawing.Point(16, 16);
        pnlVoucher.Name = "pnlVoucher";
        pnlVoucher.Padding = new Padding(14, 12, 14, 12);
        pnlVoucher.Size = new System.Drawing.Size(828, 262);
        pnlVoucher.TabIndex = 0;

        lblTieuDeVoucher.AutoSize = false;
        lblTieuDeVoucher.Dock = DockStyle.Top;
        lblTieuDeVoucher.Font = GiaoDien.ChuLon;
        lblTieuDeVoucher.ForeColor = GiaoDien.Chu;
        lblTieuDeVoucher.Location = new System.Drawing.Point(14, 12);
        lblTieuDeVoucher.Name = "lblTieuDeVoucher";
        lblTieuDeVoucher.Size = new System.Drawing.Size(800, 30);
        lblTieuDeVoucher.TabIndex = 0;
        lblTieuDeVoucher.Text = "Voucher có thể sử dụng";
        lblTieuDeVoucher.TextAlign = ContentAlignment.MiddleLeft;

        pnlBoLoc.BackColor = GiaoDien.BeMat;
        pnlBoLoc.Controls.Add(btnLamMoi);
        pnlBoLoc.Controls.Add(btnTim);
        pnlBoLoc.Controls.Add(txtTimKiem);
        pnlBoLoc.Dock = DockStyle.Top;
        pnlBoLoc.Location = new System.Drawing.Point(14, 42);
        pnlBoLoc.Name = "pnlBoLoc";
        pnlBoLoc.Padding = new Padding(0, 4, 0, 6);
        pnlBoLoc.Size = new System.Drawing.Size(800, 40);
        pnlBoLoc.TabIndex = 1;
        pnlBoLoc.AutoScroll = true;

        txtTimKiem.BackColor = GiaoDien.ManHinhNen;
        txtTimKiem.BorderStyle = BorderStyle.FixedSingle;
        txtTimKiem.Font = GiaoDien.ChuThuong;
        txtTimKiem.Location = new System.Drawing.Point(0, 5);
        txtTimKiem.Name = "txtTimKiem";
        txtTimKiem.Size = new System.Drawing.Size(260, 25);
        txtTimKiem.TabIndex = 0;
        txtTimKiem.KeyDown += txtTimKiem_KeyDown;

        GiaoDien.DangNutChinh(btnTim);
        btnTim.Location = new System.Drawing.Point(270, 1);
        btnTim.Name = "btnTim";
        btnTim.Size = new System.Drawing.Size(90, 34);
        btnTim.TabIndex = 1;
        btnTim.Text = "Tìm";
        btnTim.UseVisualStyleBackColor = false;
        btnTim.Click += btnTim_Click;

        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Location = new System.Drawing.Point(370, 1);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(110, 34);
        btnLamMoi.TabIndex = 2;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        dgvVoucher.Dock = DockStyle.Fill;
        dgvVoucher.Location = new System.Drawing.Point(14, 44);
        dgvVoucher.Name = "dgvVoucher";
        dgvVoucher.Size = new System.Drawing.Size(800, 206);
        dgvVoucher.TabIndex = 2;

        pnlLichSu.BackColor = GiaoDien.BeMat;
        pnlLichSu.Controls.Add(dgvLichSu);
        pnlLichSu.Controls.Add(lblTieuDeLichSu);
        pnlLichSu.Dock = DockStyle.Bottom;
        pnlLichSu.Location = new System.Drawing.Point(16, 278);
        pnlLichSu.Name = "pnlLichSu";
        pnlLichSu.Padding = new Padding(14, 12, 14, 12);
        pnlLichSu.Size = new System.Drawing.Size(828, 216);
        pnlLichSu.TabIndex = 1;

        lblTieuDeLichSu.AutoSize = true;
        lblTieuDeLichSu.Font = GiaoDien.ChuLon;
        lblTieuDeLichSu.ForeColor = GiaoDien.Chu;
        lblTieuDeLichSu.Location = new System.Drawing.Point(14, 12);
        lblTieuDeLichSu.Name = "lblTieuDeLichSu";
        lblTieuDeLichSu.Size = new System.Drawing.Size(200, 25);
        lblTieuDeLichSu.TabIndex = 0;
        lblTieuDeLichSu.Text = "Lịch sử đã dùng";

        dgvLichSu.Dock = DockStyle.Fill;
        dgvLichSu.Location = new System.Drawing.Point(14, 44);
        dgvLichSu.Name = "dgvLichSu";
        dgvLichSu.Size = new System.Drawing.Size(800, 160);
        dgvLichSu.TabIndex = 1;

        pnlChan.BackColor = GiaoDien.ManHinhNen;
        pnlChan.Controls.Add(lblThongKe);
        pnlChan.Dock = DockStyle.Bottom;
        pnlChan.Location = new System.Drawing.Point(16, 494);
        pnlChan.Name = "pnlChan";
        pnlChan.Padding = new Padding(14, 0, 14, 0);
        pnlChan.Size = new System.Drawing.Size(828, 48);
        pnlChan.TabIndex = 2;

        lblThongKe.AutoSize = true;
        lblThongKe.Font = GiaoDien.ChuNho;
        lblThongKe.ForeColor = GiaoDien.ChuPhu;
        lblThongKe.Location = new System.Drawing.Point(14, 15);
        lblThongKe.Name = "lblThongKe";
        lblThongKe.Size = new System.Drawing.Size(260, 15);
        lblThongKe.TabIndex = 0;
        lblThongKe.Text = "0 voucher đang có thể sử dụng";

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(860, 620);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmVoucherCuaToi";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Voucher của tôi";
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlVoucher.ResumeLayout(false);
        pnlVoucher.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvVoucher).EndInit();
        pnlBoLoc.ResumeLayout(false);
        pnlLichSu.ResumeLayout(false);
        pnlLichSu.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLichSu).EndInit();
        pnlChan.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlDau;
    private Label lblMoTaTrang;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Panel pnlNoiDung;
    private Panel pnlVoucher;
    private DataGridView dgvVoucher;
    private Panel pnlBoLoc;
    private Button btnLamMoi;
    private Button btnTim;
    private TextBox txtTimKiem;
    private Label lblTieuDeVoucher;
    private Panel pnlLichSu;
    private DataGridView dgvLichSu;
    private Label lblTieuDeLichSu;
    private Panel pnlChan;
    private Label lblThongKe;
}
