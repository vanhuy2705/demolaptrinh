using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmHoaDonCuaToi
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
        pnlChan = new Panel();
        lblThongKe = new Label();
        btnIn = new Button();
        btnChiTiet = new Button();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlHoaDon.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHoaDon).BeginInit();
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
        picBieuTuong.TenBieuTuong = "hoadon";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(76, 14);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(220, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Hóa đơn của tôi";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(400, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Xem chi tiết và in lại hóa đơn của bạn";

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
        pnlHoaDon.Controls.Add(pnlChan);
        pnlHoaDon.Dock = DockStyle.Fill;
        pnlHoaDon.Location = new System.Drawing.Point(16, 16);
        pnlHoaDon.Name = "pnlHoaDon";
        pnlHoaDon.Size = new System.Drawing.Size(1168, 610);
        pnlHoaDon.TabIndex = 0;

        pnlBoLoc.Controls.Add(cboTrangThai);
        pnlBoLoc.Controls.Add(btnLamMoi);
        pnlBoLoc.Dock = DockStyle.Top;
        pnlBoLoc.Location = new System.Drawing.Point(0, 0);
        pnlBoLoc.Name = "pnlBoLoc";
        pnlBoLoc.Padding = new Padding(14, 12, 14, 12);
        pnlBoLoc.Size = new System.Drawing.Size(1168, 60);
        pnlBoLoc.TabIndex = 0;

        cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTrangThai.Font = GiaoDien.ChuThuong;
        cboTrangThai.Location = new System.Drawing.Point(14, 15);
        cboTrangThai.Name = "cboTrangThai";
        cboTrangThai.Size = new System.Drawing.Size(180, 25);
        cboTrangThai.TabIndex = 0;
        cboTrangThai.SelectedIndexChanged += cboTrangThai_SelectedIndexChanged;

        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Location = new System.Drawing.Point(210, 13);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(110, 34);
        btnLamMoi.TabIndex = 1;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        dgvHoaDon.Dock = DockStyle.Fill;
        dgvHoaDon.Location = new System.Drawing.Point(0, 60);
        dgvHoaDon.Name = "dgvHoaDon";
        dgvHoaDon.Size = new System.Drawing.Size(1168, 498);
        dgvHoaDon.TabIndex = 1;
        dgvHoaDon.SelectionChanged += dgvHoaDon_SelectionChanged;

        pnlChan.BackColor = GiaoDien.BeMat;
        pnlChan.Controls.Add(lblThongKe);
        pnlChan.Controls.Add(btnIn);
        pnlChan.Controls.Add(btnChiTiet);
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
        lblThongKe.Size = new System.Drawing.Size(600, 20);
        lblThongKe.TabIndex = 0;
        lblThongKe.Text = "Tổng 0 hóa đơn";

        GiaoDien.DangNutPhu(btnChiTiet);
        btnChiTiet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnChiTiet.Location = new System.Drawing.Point(864, 7);
        btnChiTiet.Name = "btnChiTiet";
        btnChiTiet.Size = new System.Drawing.Size(120, 38);
        btnChiTiet.TabIndex = 1;
        btnChiTiet.Text = "Chi tiết";
        btnChiTiet.UseVisualStyleBackColor = false;
        btnChiTiet.Click += btnChiTiet_Click;

        GiaoDien.DangNutChinh(btnIn);
        btnIn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnIn.Location = new System.Drawing.Point(994, 7);
        btnIn.Name = "btnIn";
        btnIn.Size = new System.Drawing.Size(120, 38);
        btnIn.TabIndex = 2;
        btnIn.Text = "In hóa đơn";
        btnIn.UseVisualStyleBackColor = false;
        btnIn.Click += btnIn_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmHoaDonCuaToi";
        Text = "Hóa đơn của tôi";
        pnlDau.ResumeLayout(false);
        pnlNoiDung.ResumeLayout(false);
        pnlHoaDon.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvHoaDon).EndInit();
        pnlBoLoc.ResumeLayout(false);
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
    private Panel pnlChan;
    private Label lblThongKe;
    private Button btnIn;
    private Button btnChiTiet;
}
