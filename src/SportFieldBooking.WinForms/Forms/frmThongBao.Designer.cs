using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmThongBao
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
        pnlThe = new Panel();
        pnlChan = new Panel();
        btnHuy = new Button();
        btnDongY = new Button();
        lblThongDiep = new Label();
        lblTieuDe = new Label();
        picBieuTuong = new IconBox();
        pnlThe.SuspendLayout();
        pnlChan.SuspendLayout();
        SuspendLayout();

        // pnlThe
        pnlThe.BackColor = GiaoDien.BeMat;
        pnlThe.Controls.Add(pnlChan);
        pnlThe.Controls.Add(lblThongDiep);
        pnlThe.Controls.Add(lblTieuDe);
        pnlThe.Controls.Add(picBieuTuong);
        pnlThe.Dock = DockStyle.Fill;
        pnlThe.Location = new System.Drawing.Point(0, 0);
        pnlThe.Name = "pnlThe";
        pnlThe.Size = new System.Drawing.Size(520, 260);
        pnlThe.TabIndex = 0;

        // pnlChan
        pnlChan.BackColor = GiaoDien.ManHinhNen;
        pnlChan.Controls.Add(btnHuy);
        pnlChan.Controls.Add(btnDongY);
        pnlChan.Dock = DockStyle.Bottom;
        pnlChan.Location = new System.Drawing.Point(0, 188);
        pnlChan.Name = "pnlChan";
        pnlChan.Size = new System.Drawing.Size(520, 72);
        pnlChan.TabIndex = 3;

        // btnHuy
        btnHuy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        GiaoDien.DangNutPhu(btnHuy);
        btnHuy.Location = new System.Drawing.Point(290, 17);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new System.Drawing.Size(110, 38);
        btnHuy.TabIndex = 1;
        btnHuy.Text = "Hủy";
        btnHuy.UseVisualStyleBackColor = false;
        btnHuy.Click += btnHuy_Click;

        // btnDongY
        btnDongY.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        GiaoDien.DangNutChinh(btnDongY);
        btnDongY.Location = new System.Drawing.Point(406, 17);
        btnDongY.Name = "btnDongY";
        btnDongY.Size = new System.Drawing.Size(100, 38);
        btnDongY.TabIndex = 0;
        btnDongY.Text = "Đóng";
        btnDongY.UseVisualStyleBackColor = false;
        btnDongY.Click += btnDongY_Click;

        // lblThongDiep
        lblThongDiep.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblThongDiep.Font = GiaoDien.ChuThuong;
        lblThongDiep.ForeColor = GiaoDien.Chu;
        lblThongDiep.Location = new System.Drawing.Point(96, 82);
        lblThongDiep.Name = "lblThongDiep";
        lblThongDiep.Size = new System.Drawing.Size(400, 90);
        lblThongDiep.TabIndex = 2;
        lblThongDiep.Text = "Nội dung thông báo";

        // lblTieuDe
        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.ChuLon;
        lblTieuDe.ForeColor = GiaoDien.Chu;
        lblTieuDe.Location = new System.Drawing.Point(96, 40);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(120, 25);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Tiêu đề";

        // picBieuTuong
        picBieuTuong.Location = new System.Drawing.Point(32, 40);
        picBieuTuong.MauBieuTuong = GiaoDien.ThongTin;
        picBieuTuong.Name = "picBieuTuong";
        picBieuTuong.Size = new System.Drawing.Size(48, 48);
        picBieuTuong.TabIndex = 0;
        picBieuTuong.TenBieuTuong = "note";

        // frmThongBao
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(520, 260);
        Controls.Add(pnlThe);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmThongBao";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Thông báo";
        pnlThe.ResumeLayout(false);
        pnlChan.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlThe;
    private Panel pnlChan;
    private Button btnHuy;
    private Button btnDongY;
    private Label lblThongDiep;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
}
