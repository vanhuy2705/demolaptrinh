using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmPhanQuyen
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
        pnlTrai = new Panel();
        lblNhanVaiTro = new Label();
        cboVaiTro = new ComboBox();
        lblGhiChu = new Label();
        pnlPhai = new Panel();
        dgvQuyen = new DataGridView();
        pnlBoLoc = new Panel();
        btnBoChon = new Button();
        btnChonTatCa = new Button();
        btnLuu = new Button();
        lblThongKe = new Label();
        pnlDau.SuspendLayout();
        pnlNoiDung.SuspendLayout();
        pnlTrai.SuspendLayout();
        pnlPhai.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvQuyen).BeginInit();
        pnlBoLoc.SuspendLayout();
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
        picBieuTuong.TenBieuTuong = "key";

        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(76, 14);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(200, 31);
        lblTieuDe.TabIndex = 1;
        lblTieuDe.Text = "Phân quyền vai trò";

        lblMoTaTrang.AutoSize = true;
        lblMoTaTrang.Font = GiaoDien.ChuNho;
        lblMoTaTrang.ForeColor = Color.FromArgb(148, 163, 184);
        lblMoTaTrang.Location = new System.Drawing.Point(78, 48);
        lblMoTaTrang.Name = "lblMoTaTrang";
        lblMoTaTrang.Size = new System.Drawing.Size(420, 15);
        lblMoTaTrang.TabIndex = 2;
        lblMoTaTrang.Text = "Chọn vai trò và tick các quyền được phép – áp dụng ngay sau khi lưu";

        GiaoDien.DangNutPhu(btnLamMoi);
        btnLamMoi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLamMoi.Location = new System.Drawing.Point(1060, 18);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new System.Drawing.Size(110, 38);
        btnLamMoi.TabIndex = 3;
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = false;
        btnLamMoi.Click += btnLamMoi_Click;

        pnlNoiDung.Controls.Add(pnlPhai);
        pnlNoiDung.Controls.Add(pnlTrai);
        pnlNoiDung.AutoScroll = true;
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Location = new System.Drawing.Point(0, 78);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(16);
        pnlNoiDung.Size = new System.Drawing.Size(1200, 642);
        pnlNoiDung.TabIndex = 1;

        pnlTrai.BackColor = GiaoDien.BeMat;
        pnlTrai.Controls.Add(lblGhiChu);
        pnlTrai.Controls.Add(cboVaiTro);
        pnlTrai.Controls.Add(lblNhanVaiTro);
        pnlTrai.Dock = DockStyle.Left;
        pnlTrai.Location = new System.Drawing.Point(16, 16);
        pnlTrai.Name = "pnlTrai";
        pnlTrai.Padding = new Padding(20);
        pnlTrai.Size = new System.Drawing.Size(300, 610);
        pnlTrai.TabIndex = 0;

        lblNhanVaiTro.AutoSize = true;
        GiaoDien.DangNhan(lblNhanVaiTro);
        lblNhanVaiTro.Location = new System.Drawing.Point(20, 20);
        lblNhanVaiTro.Name = "lblNhanVaiTro";
        lblNhanVaiTro.Text = "Vai trò";

        cboVaiTro.DropDownStyle = ComboBoxStyle.DropDownList;
        cboVaiTro.Font = GiaoDien.ChuThuong;
        cboVaiTro.Location = new System.Drawing.Point(20, 44);
        cboVaiTro.Name = "cboVaiTro";
        cboVaiTro.Size = new System.Drawing.Size(260, 25);
        cboVaiTro.TabIndex = 0;

        lblGhiChu.AutoSize = false;
        lblGhiChu.Font = GiaoDien.ChuNho;
        lblGhiChu.ForeColor = GiaoDien.ChuPhu;
        lblGhiChu.Location = new System.Drawing.Point(20, 90);
        lblGhiChu.Name = "lblGhiChu";
        lblGhiChu.Size = new System.Drawing.Size(260, 200);
        lblGhiChu.TabIndex = 1;
        lblGhiChu.Text = "Hướng dẫn:\n- Admin: toàn quyền\n- Nhân viên: quản lý đặt sân, hóa đơn, khách hàng, sân\n- Khách hàng: chỉ xem/sửa dữ liệu của mình\n\nLưu ý: Thay đổi có hiệu lực sau 5 phút (cache) hoặc khởi động lại app.";

        pnlPhai.BackColor = GiaoDien.BeMat;
        pnlPhai.Controls.Add(dgvQuyen);
        pnlPhai.Controls.Add(pnlBoLoc);
        pnlPhai.Dock = DockStyle.Fill;
        pnlPhai.Location = new System.Drawing.Point(316, 16);
        pnlPhai.Name = "pnlPhai";
        pnlPhai.Padding = new Padding(0, 0, 0, 0);
        pnlPhai.Size = new System.Drawing.Size(868, 610);
        pnlPhai.TabIndex = 1;

        dgvQuyen.Dock = DockStyle.Fill;
        dgvQuyen.Location = new System.Drawing.Point(0, 60);
        dgvQuyen.Name = "dgvQuyen";
        dgvQuyen.Size = new System.Drawing.Size(868, 550);
        dgvQuyen.TabIndex = 1;

        pnlBoLoc.BackColor = GiaoDien.BeMat;
        pnlBoLoc.Controls.Add(lblThongKe);
        pnlBoLoc.Controls.Add(btnLuu);
        pnlBoLoc.Controls.Add(btnChonTatCa);
        pnlBoLoc.Controls.Add(btnBoChon);
        pnlBoLoc.Dock = DockStyle.Top;
        pnlBoLoc.Location = new System.Drawing.Point(0, 0);
        pnlBoLoc.Name = "pnlBoLoc";
        pnlBoLoc.Padding = new Padding(12, 10, 12, 10);
        pnlBoLoc.Size = new System.Drawing.Size(868, 60);
        pnlBoLoc.TabIndex = 0;

        lblThongKe.AutoSize = true;
        lblThongKe.Font = GiaoDien.ChuNho;
        lblThongKe.ForeColor = GiaoDien.ChuPhu;
        lblThongKe.Location = new System.Drawing.Point(12, 22);
        lblThongKe.Name = "lblThongKe";
        lblThongKe.Size = new System.Drawing.Size(100, 15);
        lblThongKe.TabIndex = 3;
        lblThongKe.Text = "0/0 quyền";

        GiaoDien.DangNutChinh(btnLuu);
        btnLuu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLuu.Location = new System.Drawing.Point(730, 12);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new System.Drawing.Size(120, 36);
        btnLuu.TabIndex = 0;
        btnLuu.Text = "Lưu phân quyền";
        btnLuu.UseVisualStyleBackColor = false;
        btnLuu.Click += btnLuu_Click;

        GiaoDien.DangNutPhu(btnChonTatCa);
        btnChonTatCa.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnChonTatCa.Location = new System.Drawing.Point(500, 12);
        btnChonTatCa.Name = "btnChonTatCa";
        btnChonTatCa.Size = new System.Drawing.Size(110, 36);
        btnChonTatCa.TabIndex = 1;
        btnChonTatCa.Text = "Chọn tất cả";
        btnChonTatCa.UseVisualStyleBackColor = false;
        btnChonTatCa.Click += btnChonTatCa_Click;

        GiaoDien.DangNutPhu(btnBoChon);
        btnBoChon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBoChon.Location = new System.Drawing.Point(620, 12);
        btnBoChon.Name = "btnBoChon";
        btnBoChon.Size = new System.Drawing.Size(100, 36);
        btnBoChon.TabIndex = 2;
        btnBoChon.Text = "Bỏ chọn";
        btnBoChon.UseVisualStyleBackColor = false;
        btnBoChon.Click += btnBoChon_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1200, 720);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Name = "frmPhanQuyen";
        Text = "Phân quyền vai trò";
        pnlDau.ResumeLayout(false);
        pnlDau.PerformLayout();
        pnlNoiDung.ResumeLayout(false);
        pnlTrai.ResumeLayout(false);
        pnlPhai.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvQuyen).EndInit();
        pnlBoLoc.ResumeLayout(false);
        pnlBoLoc.PerformLayout();
        ResumeLayout(false);
    }

    private Panel pnlDau;
    private Label lblMoTaTrang;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Button btnLamMoi;
    private Panel pnlNoiDung;
    private Panel pnlTrai;
    private Label lblNhanVaiTro;
    private ComboBox cboVaiTro;
    private Label lblGhiChu;
    private Panel pnlPhai;
    private DataGridView dgvQuyen;
    private Panel pnlBoLoc;
    private Button btnBoChon;
    private Button btnChonTatCa;
    private Button btnLuu;
    private Label lblThongKe;
}
