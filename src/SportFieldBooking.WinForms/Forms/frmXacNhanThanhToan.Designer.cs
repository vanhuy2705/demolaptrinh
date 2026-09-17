using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

partial class frmXacNhanThanhToan
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
        lblNhanTienThua = new Label();
        lblTienThua = new Label();
        txtSoTienKhachDua = new TextBox();
        lblNhanTienKhachDua = new Label();
        radChuyenKhoan = new RadioButton();
        radTienMat = new RadioButton();
        lblNhanPhuongThuc = new Label();
        lblTongTien = new Label();
        lblNhanTongTien = new Label();
        lblGiamGia = new Label();
        lblNhanGiamGia = new Label();
        lblTienGoc = new Label();
        lblNhanTienGoc = new Label();
        lblThoiGian = new Label();
        lblNhanThoiGian = new Label();
        lblSan = new Label();
        lblNhanSan = new Label();
        lblKhachHang = new Label();
        lblNhanKhachHang = new Label();
        lblMaHoaDon = new Label();
        pnlChan = new Panel();
        btnHuy = new Button();
        btnXacNhan = new Button();
        pnlDau.SuspendLayout();
        pnlThan.SuspendLayout();
        pnlChan.SuspendLayout();
        SuspendLayout();

        // pnlDau
        pnlDau.BackColor = GiaoDien.ThanhBen;
        pnlDau.Controls.Add(lblTieuDe);
        pnlDau.Controls.Add(picBieuTuong);
        pnlDau.Dock = DockStyle.Top;
        pnlDau.Location = new System.Drawing.Point(0, 0);
        pnlDau.Name = "pnlDau";
        pnlDau.Size = new System.Drawing.Size(520, 84);
        pnlDau.TabIndex = 0;

        // picBieuTuong
        picBieuTuong.Location = new System.Drawing.Point(24, 18);
        picBieuTuong.MauBieuTuong = GiaoDien.Chinh;
        picBieuTuong.Name = "picBieuTuong";
        picBieuTuong.Size = new System.Drawing.Size(46, 46);
        picBieuTuong.TabIndex = 1;
        picBieuTuong.TenBieuTuong = "cash";

        // lblTieuDe
        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = GiaoDien.TieuDe;
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Location = new System.Drawing.Point(84, 26);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new System.Drawing.Size(200, 31);
        lblTieuDe.TabIndex = 0;
        lblTieuDe.Text = "Xác nhận thanh toán";

        // pnlThan
        pnlThan.BackColor = GiaoDien.BeMat;
        pnlThan.Controls.Add(lblNhanTienThua);
        pnlThan.Controls.Add(lblTienThua);
        pnlThan.Controls.Add(txtSoTienKhachDua);
        pnlThan.Controls.Add(lblNhanTienKhachDua);
        pnlThan.Controls.Add(radChuyenKhoan);
        pnlThan.Controls.Add(radTienMat);
        pnlThan.Controls.Add(lblNhanPhuongThuc);
        pnlThan.Controls.Add(lblTongTien);
        pnlThan.Controls.Add(lblNhanTongTien);
        pnlThan.Controls.Add(lblGiamGia);
        pnlThan.Controls.Add(lblNhanGiamGia);
        pnlThan.Controls.Add(lblTienGoc);
        pnlThan.Controls.Add(lblNhanTienGoc);
        pnlThan.Controls.Add(lblThoiGian);
        pnlThan.Controls.Add(lblNhanThoiGian);
        pnlThan.Controls.Add(lblSan);
        pnlThan.Controls.Add(lblNhanSan);
        pnlThan.Controls.Add(lblKhachHang);
        pnlThan.Controls.Add(lblNhanKhachHang);
        pnlThan.Controls.Add(lblMaHoaDon);
        pnlThan.Dock = DockStyle.Fill;
        pnlThan.Location = new System.Drawing.Point(0, 84);
        pnlThan.Name = "pnlThan";
        pnlThan.Size = new System.Drawing.Size(520, 336);
        pnlThan.TabIndex = 1;

        // lblMaHoaDon
        lblMaHoaDon.AutoSize = true;
        lblMaHoaDon.Font = GiaoDien.ChuDam;
        lblMaHoaDon.ForeColor = GiaoDien.ChinhDam;
        lblMaHoaDon.Location = new System.Drawing.Point(32, 22);
        lblMaHoaDon.Name = "lblMaHoaDon";
        lblMaHoaDon.Size = new System.Drawing.Size(90, 17);
        lblMaHoaDon.TabIndex = 0;
        lblMaHoaDon.Text = "Hóa đơn #";

        // lblNhanKhachHang
        lblNhanKhachHang.AutoSize = true;
        GiaoDien.DangNhan(lblNhanKhachHang);
        lblNhanKhachHang.Location = new System.Drawing.Point(32, 58);
        lblNhanKhachHang.Name = "lblNhanKhachHang";
        lblNhanKhachHang.TabIndex = 1;
        lblNhanKhachHang.Text = "Khách hàng";

        // lblKhachHang
        lblKhachHang.AutoSize = true;
        lblKhachHang.Font = GiaoDien.ChuThuong;
        lblKhachHang.ForeColor = GiaoDien.Chu;
        lblKhachHang.Location = new System.Drawing.Point(180, 56);
        lblKhachHang.Name = "lblKhachHang";
        lblKhachHang.Size = new System.Drawing.Size(80, 17);
        lblKhachHang.TabIndex = 2;
        lblKhachHang.Text = "Khách hàng";

        // lblNhanSan
        lblNhanSan.AutoSize = true;
        GiaoDien.DangNhan(lblNhanSan);
        lblNhanSan.Location = new System.Drawing.Point(32, 84);
        lblNhanSan.Name = "lblNhanSan";
        lblNhanSan.TabIndex = 3;
        lblNhanSan.Text = "Sân";

        // lblSan
        lblSan.AutoSize = true;
        lblSan.Font = GiaoDien.ChuThuong;
        lblSan.ForeColor = GiaoDien.Chu;
        lblSan.Location = new System.Drawing.Point(180, 82);
        lblSan.Name = "lblSan";
        lblSan.Size = new System.Drawing.Size(40, 17);
        lblSan.TabIndex = 4;
        lblSan.Text = "Sân";

        // lblNhanThoiGian
        lblNhanThoiGian.AutoSize = true;
        GiaoDien.DangNhan(lblNhanThoiGian);
        lblNhanThoiGian.Location = new System.Drawing.Point(32, 110);
        lblNhanThoiGian.Name = "lblNhanThoiGian";
        lblNhanThoiGian.TabIndex = 5;
        lblNhanThoiGian.Text = "Thời gian";

        // lblThoiGian
        lblThoiGian.AutoSize = true;
        lblThoiGian.Font = GiaoDien.ChuThuong;
        lblThoiGian.ForeColor = GiaoDien.Chu;
        lblThoiGian.Location = new System.Drawing.Point(180, 108);
        lblThoiGian.Name = "lblThoiGian";
        lblThoiGian.Size = new System.Drawing.Size(70, 17);
        lblThoiGian.TabIndex = 6;
        lblThoiGian.Text = "Thời gian";

        // lblNhanTienGoc
        lblNhanTienGoc.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTienGoc);
        lblNhanTienGoc.Location = new System.Drawing.Point(32, 148);
        lblNhanTienGoc.Name = "lblNhanTienGoc";
        lblNhanTienGoc.TabIndex = 7;
        lblNhanTienGoc.Text = "Tiền sân";

        // lblTienGoc
        lblTienGoc.AutoSize = true;
        lblTienGoc.Font = GiaoDien.ChuThuong;
        lblTienGoc.ForeColor = GiaoDien.Chu;
        lblTienGoc.Location = new System.Drawing.Point(180, 146);
        lblTienGoc.Name = "lblTienGoc";
        lblTienGoc.Size = new System.Drawing.Size(60, 17);
        lblTienGoc.TabIndex = 8;
        lblTienGoc.Text = "0 đ";

        // lblNhanGiamGia
        lblNhanGiamGia.AutoSize = true;
        GiaoDien.DangNhan(lblNhanGiamGia);
        lblNhanGiamGia.Location = new System.Drawing.Point(32, 174);
        lblNhanGiamGia.Name = "lblNhanGiamGia";
        lblNhanGiamGia.TabIndex = 9;
        lblNhanGiamGia.Text = "Giảm giá";

        // lblGiamGia
        lblGiamGia.AutoSize = true;
        lblGiamGia.Font = GiaoDien.ChuThuong;
        lblGiamGia.ForeColor = GiaoDien.ThanhCong;
        lblGiamGia.Location = new System.Drawing.Point(180, 172);
        lblGiamGia.Name = "lblGiamGia";
        lblGiamGia.Size = new System.Drawing.Size(60, 17);
        lblGiamGia.TabIndex = 10;
        lblGiamGia.Text = "0 đ";

        // lblNhanTongTien
        lblNhanTongTien.AutoSize = true;
        lblNhanTongTien.Font = GiaoDien.ChuDam;
        lblNhanTongTien.ForeColor = GiaoDien.Chu;
        lblNhanTongTien.Location = new System.Drawing.Point(32, 208);
        lblNhanTongTien.Name = "lblNhanTongTien";
        lblNhanTongTien.Size = new System.Drawing.Size(90, 17);
        lblNhanTongTien.TabIndex = 11;
        lblNhanTongTien.Text = "Tổng thanh toán";

        // lblTongTien
        lblTongTien.AutoSize = true;
        lblTongTien.Font = GiaoDien.TieuDe;
        lblTongTien.ForeColor = GiaoDien.ChinhDam;
        lblTongTien.Location = new System.Drawing.Point(180, 198);
        lblTongTien.Name = "lblTongTien";
        lblTongTien.Size = new System.Drawing.Size(80, 31);
        lblTongTien.TabIndex = 12;
        lblTongTien.Text = "0 đ";

        // lblNhanPhuongThuc
        lblNhanPhuongThuc.AutoSize = true;
        GiaoDien.DangNhan(lblNhanPhuongThuc);
        lblNhanPhuongThuc.Location = new System.Drawing.Point(32, 250);
        lblNhanPhuongThuc.Name = "lblNhanPhuongThuc";
        lblNhanPhuongThuc.TabIndex = 13;
        lblNhanPhuongThuc.Text = "Phương thức thanh toán";

        // radTienMat
        radTienMat.AutoSize = true;
        radTienMat.Checked = true;
        radTienMat.Font = GiaoDien.ChuThuong;
        radTienMat.Location = new System.Drawing.Point(180, 246);
        radTienMat.Name = "radTienMat";
        radTienMat.Size = new System.Drawing.Size(90, 21);
        radTienMat.TabIndex = 0;
        radTienMat.TabStop = true;
        radTienMat.Text = "Tiền mặt";
        radTienMat.UseVisualStyleBackColor = true;
        radTienMat.CheckedChanged += radTienMat_CheckedChanged;

        // radChuyenKhoan
        radChuyenKhoan.AutoSize = true;
        radChuyenKhoan.Font = GiaoDien.ChuThuong;
        radChuyenKhoan.Location = new System.Drawing.Point(290, 246);
        radChuyenKhoan.Name = "radChuyenKhoan";
        radChuyenKhoan.Size = new System.Drawing.Size(115, 21);
        radChuyenKhoan.TabIndex = 1;
        radChuyenKhoan.Text = "Chuyển khoản";
        radChuyenKhoan.UseVisualStyleBackColor = true;

        // lblNhanTienKhachDua
        lblNhanTienKhachDua.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTienKhachDua);
        lblNhanTienKhachDua.Location = new System.Drawing.Point(32, 284);
        lblNhanTienKhachDua.Name = "lblNhanTienKhachDua";
        lblNhanTienKhachDua.TabIndex = 15;
        lblNhanTienKhachDua.Text = "Tiền khách đưa";

        // txtSoTienKhachDua
        txtSoTienKhachDua.BackColor = GiaoDien.ManHinhNen;
        txtSoTienKhachDua.BorderStyle = BorderStyle.FixedSingle;
        txtSoTienKhachDua.Font = GiaoDien.ChuDam;
        txtSoTienKhachDua.Location = new System.Drawing.Point(180, 280);
        txtSoTienKhachDua.Name = "txtSoTienKhachDua";
        txtSoTienKhachDua.Size = new System.Drawing.Size(180, 25);
        txtSoTienKhachDua.TabIndex = 2;
        txtSoTienKhachDua.TextAlign = HorizontalAlignment.Right;
        txtSoTienKhachDua.TextChanged += txtSoTienKhachDua_TextChanged;

        // lblTienThua
        lblTienThua.AutoSize = true;
        lblTienThua.Font = GiaoDien.ChuDam;
        lblTienThua.ForeColor = GiaoDien.ThanhCong;
        lblTienThua.Location = new System.Drawing.Point(372, 284);
        lblTienThua.Name = "lblTienThua";
        lblTienThua.Size = new System.Drawing.Size(60, 17);
        lblTienThua.TabIndex = 17;
        lblTienThua.Text = "0 đ";

        // lblNhanTienThua
        lblNhanTienThua.AutoSize = true;
        GiaoDien.DangNhan(lblNhanTienThua);
        lblNhanTienThua.Location = new System.Drawing.Point(372, 266);
        lblNhanTienThua.Name = "lblNhanTienThua";
        lblNhanTienThua.TabIndex = 18;
        lblNhanTienThua.Text = "Tiền thừa";

        // pnlChan
        pnlChan.BackColor = GiaoDien.ManHinhNen;
        pnlChan.Controls.Add(btnHuy);
        pnlChan.Controls.Add(btnXacNhan);
        pnlChan.Dock = DockStyle.Bottom;
        pnlChan.Location = new System.Drawing.Point(0, 420);
        pnlChan.Name = "pnlChan";
        pnlChan.Size = new System.Drawing.Size(520, 64);
        pnlChan.TabIndex = 2;

        // btnXacNhan
        btnXacNhan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        GiaoDien.DangNutChinh(btnXacNhan);
        btnXacNhan.Location = new System.Drawing.Point(300, 13);
        btnXacNhan.Name = "btnXacNhan";
        btnXacNhan.Size = new System.Drawing.Size(180, 40);
        btnXacNhan.TabIndex = 0;
        btnXacNhan.Text = "Xác nhận thanh toán";
        btnXacNhan.UseVisualStyleBackColor = false;
        btnXacNhan.Click += btnXacNhan_Click;

        // btnHuy
        btnHuy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        GiaoDien.DangNutPhu(btnHuy);
        btnHuy.Location = new System.Drawing.Point(204, 13);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new System.Drawing.Size(86, 40);
        btnHuy.TabIndex = 1;
        btnHuy.Text = "Hủy";
        btnHuy.UseVisualStyleBackColor = false;
        btnHuy.Click += btnHuy_Click;

        // frmXacNhanThanhToan
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(520, 484);
        Controls.Add(pnlThan);
        Controls.Add(pnlChan);
        Controls.Add(pnlDau);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmXacNhanThanhToan";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Xác nhận thanh toán";
        Load += frmXacNhanThanhToan_Load;
        pnlDau.ResumeLayout(false);
        pnlThan.ResumeLayout(false);
        pnlThan.PerformLayout();
        pnlChan.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlDau;
    private Label lblTieuDe;
    private IconBox picBieuTuong;
    private Panel pnlThan;
    private Label lblNhanTienThua;
    private Label lblTienThua;
    private TextBox txtSoTienKhachDua;
    private Label lblNhanTienKhachDua;
    private RadioButton radChuyenKhoan;
    private RadioButton radTienMat;
    private Label lblNhanPhuongThuc;
    private Label lblTongTien;
    private Label lblNhanTongTien;
    private Label lblGiamGia;
    private Label lblNhanGiamGia;
    private Label lblTienGoc;
    private Label lblNhanTienGoc;
    private Label lblThoiGian;
    private Label lblNhanThoiGian;
    private Label lblSan;
    private Label lblNhanSan;
    private Label lblKhachHang;
    private Label lblNhanKhachHang;
    private Label lblMaHoaDon;
    private Panel pnlChan;
    private Button btnHuy;
    private Button btnXacNhan;
}
