using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmNhatKyHoatDong
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
        btnLamMoi = new Button();
        pnlNoiDung = new Panel();
        pnlBoLoc = new Panel();
        btnXoaCu = new Button();
        btnXoa = new Button();
        btnTim = new Button();
        cboKetQua = new ComboBox();
        lblNhanKetQua = new Label();
        cboHoatDong = new ComboBox();
        lblNhanHoatDong = new Label();
        dtpDenNgay = new DateTimePicker();
        lblNhanDenNgay = new Label();
        dtpTuNgay = new DateTimePicker();
        lblNhanTuNgay = new Label();
        txtTimKiem = new TextBox();
        lblNhanTimKiem = new Label();
        dgvNhatKy = new DataGridView();
        pnlChan = new Panel();
        lblThongKe = new Label();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlBoLoc.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvNhatKy).BeginInit();
        pnlChan.SuspendLayout();
        SuspendLayout();

        pnlDau.BackColor = GiaoDien.ThanhBen;
        pnlDau.Controls.Add(lblMoTaTrang);
        pnlDau.Controls.Add(lblTieuDe);
        pnlDau.Controls.Add(picBieuTuong);
        pnlDau.Controls.Add(btnLamMoi);
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
        picBieuTuong.TenBieuTuong = "note";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(76, 14);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(220, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Nhật ký hoạt động";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(380, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Theo dõi mọi thao tác quan trọng: ai làm gì, khi nào, kết quả ra sao";

        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLamMoi.Location = new System.Drawing.Point(1060, 18);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(110, 38);
        btnLamMoi.TabIndex = 3;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Controls.Add(pnlChan);
        pnlNoiDung.Controls.Add(pnlBoLoc);
        pnlNoiDung.Controls.Add(dgvNhatKy);
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(0, 78);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(16);
        pnlNoiDung.Size = new System.Drawing.Size(1200, 642);
        pnlNoiDung.TabIndex = 1;

        pnlBoLoc.BackColor = GiaoDien.BeMat;
        pnlBoLoc.Controls.Add(btnXoaCu);
        pnlBoLoc.Controls.Add(btnXoa);
        pnlBoLoc.Controls.Add(btnTim);
        pnlBoLoc.Controls.Add(cboKetQua);
        pnlBoLoc.Controls.Add(lblNhanKetQua);
        pnlBoLoc.Controls.Add(cboHoatDong);
        pnlBoLoc.Controls.Add(lblNhanHoatDong);
        pnlBoLoc.Controls.Add(dtpDenNgay);
        pnlBoLoc.Controls.Add(lblNhanDenNgay);
        pnlBoLoc.Controls.Add(dtpTuNgay);
        pnlBoLoc.Controls.Add(lblNhanTuNgay);
        pnlBoLoc.Controls.Add(txtTimKiem);
        pnlBoLoc.Controls.Add(lblNhanTimKiem);
        pnlBoLoc.Dock = DockStyle.Top;
        pnlBoLoc.Location = new System.Drawing.Point(16, 16);
        pnlBoLoc.Name = "pnlBoLoc";
        pnlBoLoc.Padding = new Padding(12);
        pnlBoLoc.Size = new System.Drawing.Size(1168, 90);
        pnlBoLoc.TabIndex = 0;

        lblNhanTimKiem.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTimKiem);
        lblNhanTimKiem.Location = new System.Drawing.Point(12, 12);
        lblNhanTimKiem.Name = "lblNhanTimKiem";
        lblNhanTimKiem.Text = "Từ khóa";

        txtTimKiem.BackColor = GiaoDien.ONhap;
        txtTimKiem.BorderStyle = BorderStyle.FixedSingle;
        txtTimKiem.Location = new System.Drawing.Point(12, 32);
        txtTimKiem.Name = "txtTimKiem";
        txtTimKiem.Size = new System.Drawing.Size(200, 25);
        txtTimKiem.TabIndex = 0;
        txtTimKiem.KeyDown += txtTimKiem_KeyDown;

        lblNhanTuNgay.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTuNgay);
        lblNhanTuNgay.Location = new System.Drawing.Point(230, 12);
        lblNhanTuNgay.Name = "lblNhanTuNgay";
        lblNhanTuNgay.Text = "Từ ngày";

        dtpTuNgay.Format = DateTimePickerFormat.Short;
        dtpTuNgay.Location = new System.Drawing.Point(230, 32);
        dtpTuNgay.Name = "dtpTuNgay";
        dtpTuNgay.Size = new System.Drawing.Size(130, 25);
        dtpTuNgay.TabIndex = 1;

        lblNhanDenNgay.AutoSize = true;
        GiaoDien.DangNhan(lblNhanDenNgay);
        lblNhanDenNgay.Location = new System.Drawing.Point(375, 12);
        lblNhanDenNgay.Name = "lblNhanDenNgay";
        lblNhanDenNgay.Text = "Đến ngày";

        dtpDenNgay.Format = DateTimePickerFormat.Short;
        dtpDenNgay.Location = new System.Drawing.Point(375, 32);
        dtpDenNgay.Name = "dtpDenNgay";
        dtpDenNgay.Size = new System.Drawing.Size(130, 25);
        dtpDenNgay.TabIndex = 2;

        lblNhanHoatDong.AutoSize = true;
        GiaoDien.DangNhan(lblNhanHoatDong);
        lblNhanHoatDong.Location = new System.Drawing.Point(520, 12);
        lblNhanHoatDong.Name = "lblNhanHoatDong";
        lblNhanHoatDong.Text = "Hành động";

        cboHoatDong.DropDownStyle = ComboBoxStyle.DropDownList;
        cboHoatDong.Location = new System.Drawing.Point(520, 32);
        cboHoatDong.Name = "cboHoatDong";
        cboHoatDong.Size = new System.Drawing.Size(150, 25);
        cboHoatDong.TabIndex = 3;

        lblNhanKetQua.AutoSize = true;
        GiaoDien.DangNhan(lblNhanKetQua);
        lblNhanKetQua.Location = new System.Drawing.Point(685, 12);
        lblNhanKetQua.Name = "lblNhanKetQua";
        lblNhanKetQua.Text = "Kết quả";

        cboKetQua.DropDownStyle = ComboBoxStyle.DropDownList;
        cboKetQua.Location = new System.Drawing.Point(685, 32);
        cboKetQua.Name = "cboKetQua";
        cboKetQua.Size = new System.Drawing.Size(130, 25);
        cboKetQua.TabIndex = 4;

        GiaoDien.DangNutChinh(btnTim);
        btnTim.Location = new System.Drawing.Point(830, 28);
        btnTim.Name = "btnTim";
        btnTim.Size = new System.Drawing.Size(90, 34);
        btnTim.TabIndex = 5;
        btnTim.Text = "Tìm";
        btnTim.UseVisualStyleBackColor = false;
        btnTim.Click += btnTim_Click;

        GiaoDien.DangNutNguyHiem(btnXoa);
        btnXoa.Location = new System.Drawing.Point(930, 28);
        btnXoa.Name = "btnXoa";
        btnXoa.Size = new System.Drawing.Size(90, 34);
        btnXoa.TabIndex = 6;
        btnXoa.Text = "Xóa";
        btnXoa.UseVisualStyleBackColor = false;
        btnXoa.Click += btnXoa_Click;

        GiaoDien.DangNutPhu(btnXoaCu);
        btnXoaCu.Location = new System.Drawing.Point(1030, 28);
        btnXoaCu.Name = "btnXoaCu";
        btnXoaCu.Size = new System.Drawing.Size(110, 34);
        btnXoaCu.TabIndex = 7;
        btnXoaCu.Text = "Xóa cũ";
        btnXoaCu.UseVisualStyleBackColor = false;
        btnXoaCu.Click += btnXoaCu_Click;

        dgvNhatKy.Dock = DockStyle.Fill;
        dgvNhatKy.Location = new System.Drawing.Point(16, 106);
        dgvNhatKy.Name = "dgvNhatKy";
        dgvNhatKy.Size = new System.Drawing.Size(1168, 480);
        dgvNhatKy.TabIndex = 1;

        pnlChan.BackColor = GiaoDien.BeMat;
        pnlChan.Controls.Add(lblThongKe);
        pnlChan.Dock = DockStyle.Bottom;
        pnlChan.Location = new System.Drawing.Point(16, 586);
        pnlChan.Name = "pnlChan";
        pnlChan.Size = new System.Drawing.Size(1168, 40);
        pnlChan.TabIndex = 2;

        lblThongKe.AutoSize = true;
        lblThongKe.Font = GiaoDien.ChuNho;
        lblThongKe.ForeColor = GiaoDien.ChuPhu;
        lblThongKe.Location = new System.Drawing.Point(12, 12);
        lblThongKe.Name = "lblThongKe";
        lblThongKe.Size = new System.Drawing.Size(50, 15);
        lblThongKe.TabIndex = 0;
        lblThongKe.Text = "Tổng: 0";

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmNhatKyHoatDong";
        Text = "Nhật ký hoạt động";
        pnlDau.ResumeLayout(false);
        pnlDau.PerformLayout();
        pnlNoiDung.ResumeLayout(false);
        pnlBoLoc.ResumeLayout(false);
        pnlBoLoc.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvNhatKy).EndInit();
        pnlChan.ResumeLayout(false);
        pnlChan.PerformLayout();
        ResumeLayout(false);
    }

    private Panel pnlDau;
    private Label lblMoTaTrang;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Button btnLamMoi;
    private Panel pnlNoiDung;
    private Panel pnlBoLoc;
    private TextBox txtTimKiem;
    private Label lblNhanTimKiem;
    private DateTimePicker dtpTuNgay;
    private Label lblNhanTuNgay;
    private DateTimePicker dtpDenNgay;
    private Label lblNhanDenNgay;
    private ComboBox cboHoatDong;
    private Label lblNhanHoatDong;
    private ComboBox cboKetQua;
    private Label lblNhanKetQua;
    private Button btnTim;
    private Button btnXoa;
    private Button btnXoaCu;
    private DataGridView dgvNhatKy;
    private Panel pnlChan;
    private Label lblThongKe;
}
