using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Controls;

partial class KpiCard
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
        pnlThe = new RoundedPanel();
        pnlMau = new Panel();
        picBieuTuong = new IconBox();
        lblTieuDe = new Label();
        lblGiaTri = new Label();
        lblPhuDe = new Label();
        pnlThe.SuspendLayout();
        SuspendLayout();

        // pnlThe
        pnlThe.BackColor = GiaoDien.BeMat;
        pnlThe.BanKinh = 14;
        pnlThe.DoDayVien = 1;
        pnlThe.MauVien = GiaoDien.Vien;
        pnlThe.Controls.Add(pnlMau);
        pnlThe.Controls.Add(picBieuTuong);
        pnlThe.Controls.Add(lblTieuDe);
        pnlThe.Controls.Add(lblGiaTri);
        pnlThe.Controls.Add(lblPhuDe);
        pnlThe.Dock = DockStyle.Fill;
        pnlThe.Location = new System.Drawing.Point(0, 0);
        pnlThe.Name = "pnlThe";
        pnlThe.Size = new System.Drawing.Size(250, 120);
        pnlThe.TabIndex = 0;

        // pnlMau
        pnlMau.BackColor = GiaoDien.Chinh;
        pnlMau.Dock = DockStyle.Left;
        pnlMau.Location = new System.Drawing.Point(0, 0);
        pnlMau.Name = "pnlMau";
        pnlMau.Size = new System.Drawing.Size(6, 120);
        pnlMau.TabIndex = 0;

        // picBieuTuong
        picBieuTuong.Location = new System.Drawing.Point(20, 20);
        picBieuTuong.MauBieuTuong = GiaoDien.Chinh;
        picBieuTuong.Name = "picBieuTuong";
        picBieuTuong.Size = new System.Drawing.Size(38, 38);
        picBieuTuong.TabIndex = 1;
        picBieuTuong.TenBieuTuong = "chart";

        // lblTieuDe
        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.ChuNho;
        lblTieuDe.ForeColor = GiaoDien.ChuPhu;
        lblTieuDe.Location = new System.Drawing.Point(70, 20);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(90, 15);
        lblTieuDe.TabIndex = 2;
        lblTieuDe.Text = "Tiêu đề";

        // lblGiaTri
        lblGiaTri.AutoSize = true;
        lblGiaTri.Font = new System.Drawing.Font(GiaoDien.TenFont, 19F, System.Drawing.FontStyle.Bold);
        lblGiaTri.ForeColor = GiaoDien.Chu;
        lblGiaTri.Location = new System.Drawing.Point(68, 40);
        lblGiaTri.Name = "lblGiaTri";
        lblGiaTri.Size = new System.Drawing.Size(80, 35);
        lblGiaTri.TabIndex = 3;
        lblGiaTri.Text = "0";

        // lblPhuDe
        lblPhuDe.AutoSize = true;
        lblPhuDe.Font = GiaoDien.ChuNho;
        lblPhuDe.ForeColor = GiaoDien.ChuPhu;
        lblPhuDe.Location = new System.Drawing.Point(20, 84);
        lblPhuDe.Name = "lblPhuDe";
        lblPhuDe.Size = new System.Drawing.Size(60, 15);
        lblPhuDe.TabIndex = 4;
        lblPhuDe.Text = "Mô tả";

        // KpiCard
        Controls.Add(pnlThe);
        Name = "KpiCard";
        Size = new System.Drawing.Size(250, 120);
        pnlThe.ResumeLayout(false);
        ResumeLayout(false);
    }

    private RoundedPanel pnlThe;
    private Panel pnlMau;
    private IconBox picBieuTuong;
    private Label lblTieuDe;
    private Label lblGiaTri;
    private Label lblPhuDe;
}
