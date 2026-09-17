using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmNhapLieu
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
        lblTieuDe = new Label();
        picBieuTuong = new IconBox();
        pnlThan = new Panel();
        btnHuy = new Button();
        btnDongY = new Button();
        txtGiaTri = new TextBox();
        lblNhanLoi = new Label();
        pnlDau.SuspendLayout();
        pnlThan.SuspendLayout();
        SuspendLayout();

        pnlDau.BackColor = GiaoDien.ThanhBen;
        pnlDau.Controls.Add(lblTieuDe);
        pnlDau.Controls.Add(picBieuTuong);
        pnlDau.Dock = DockStyle.Top;
        pnlDau.Location = new System.Drawing.Point(0, 0);
        pnlDau.Name = "pnlDau";
        pnlDau.Size = new System.Drawing.Size(440, 78);
        pnlDau.TabIndex = 0;

        picBieuTuong.Location = new System.Drawing.Point(20, 17);
        picBieuTuong.MauBieuTuong = GiaoDien.Chinh;
        picBieuTuong.Name = "picBieuTuong";
        picBieuTuong.Size = new System.Drawing.Size(42, 42);
        picBieuTuong.TabIndex = 1;
        picBieuTuong.TenBieuTuong = "edit";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.ChuLon;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(74, 26);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(140, 25);
        lblTieuDe.TabIndex = 0;
        lblTieuDe.Text = "Nhập dữ liệu";

        pnlThan.BackColor = GiaoDien.BeMat;
        pnlThan.Controls.Add(btnHuy);
        pnlThan.Controls.Add(btnDongY);
        pnlThan.Controls.Add(txtGiaTri);
        pnlThan.Controls.Add(lblNhanLoi);
        pnlThan.Dock = DockStyle.Fill;
        pnlThan.Location = new System.Drawing.Point(0, 78);
        pnlThan.Name = "pnlThan";
        pnlThan.Size = new System.Drawing.Size(440, 124);
        pnlThan.TabIndex = 1;

        lblNhanLoi.AutoSize = true;
        GiaoDien.DangNhan(lblNhanLoi);
        lblNhanLoi.Location = new System.Drawing.Point(28, 24);
        lblNhanLoi.Name = "lblNhanLoi";
        lblNhanLoi.TabIndex = 0;
        lblNhanLoi.Text = "Giá trị";

        txtGiaTri.BackColor = GiaoDien.ManHinhNen;
        txtGiaTri.BorderStyle = BorderStyle.FixedSingle;
        txtGiaTri.Font = GiaoDien.ChuThuong;
        txtGiaTri.Location = new System.Drawing.Point(28, 44);
        txtGiaTri.Name = "txtGiaTri";
        txtGiaTri.Size = new System.Drawing.Size(384, 25);
        txtGiaTri.TabIndex = 0;
        txtGiaTri.KeyDown += txtGiaTri_KeyDown;

        GiaoDien.DangNutChinh(btnDongY);
        btnDongY.Location = new System.Drawing.Point(180, 80);
        btnDongY.Name = "btnDongY";
        btnDongY.Size = new System.Drawing.Size(110, 36);
        btnDongY.TabIndex = 1;
        btnDongY.Text = "Đồng ý";
        btnDongY.UseVisualStyleBackColor = false;
        btnDongY.Click += btnDongY_Click;

        GiaoDien.DangNutPhu(btnHuy);
        btnHuy.Location = new System.Drawing.Point(300, 80);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new System.Drawing.Size(112, 36);
        btnHuy.TabIndex = 2;
        btnHuy.Text = "Hủy bỏ";
        btnHuy.UseVisualStyleBackColor = false;
        btnHuy.Click += btnHuy_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(440, 202);
        Controls.Add(pnlThan);
        Controls.Add(pnlDau);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmNhapLieu";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Nhập dữ liệu";
        pnlDau.ResumeLayout(false);
        pnlThan.ResumeLayout(false);
        pnlThan.PerformLayout();
        ResumeLayout(false);
    }

    private Panel pnlDau;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Panel pnlThan;
    private Button btnHuy;
    private Button btnDongY;
    private TextBox txtGiaTri;
    private Label lblNhanLoi;
}
