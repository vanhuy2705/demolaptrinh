using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>
/// Cổng khách hàng: tự đặt sân, xem khung giờ trống của sân, áp dụng voucher và xem bảng tính tiền.
/// </summary>
public partial class frmDatSanKhachHang : BaseForm
{
    private ChiTietTien _ketQuaTien;

    public frmDatSanKhachHang()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.DatSanThem;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvLichTrongNgay);
        Luoi.DatTieuDe(dgvLichTrongNgay,
            ("GioBatDau", "Bắt đầu"),
            ("GioKetThuc", "Kết thúc"),
            ("TenSan", "Sân"),
            ("TrangThai", "Trạng thái"));
        Luoi.DatDoRong(dgvLichTrongNgay, "GioBatDau", 100);
        Luoi.DatDoRong(dgvLichTrongNgay, "GioKetThuc", 100);
        Luoi.HienThiTrangThai(dgvLichTrongNgay, "TrangThai", TrangThaiDatSan.TenHienThi);

        dtpNgayDat.MinDate = DateTime.Today;
        dtpNgayDat.Value = DateTime.Today;
        dtpGioBatDau.Format = DateTimePickerFormat.Custom;
        dtpGioBatDau.CustomFormat = "HH:mm";
        dtpGioBatDau.ShowUpDown = true;
        dtpGioKetThuc.Format = DateTimePickerFormat.Custom;
        dtpGioKetThuc.CustomFormat = "HH:mm";
        dtpGioKetThuc.ShowUpDown = true;
        TroGiup.DatGio(dtpGioBatDau, new TimeSpan(17, 0, 0));
        TroGiup.DatGio(dtpGioKetThuc, new TimeSpan(18, 0, 0));

        List<San> danhSachSan = ServiceFactory.San.LayTatCa("", null, TrangThaiSan.Trong);
        TroGiup.GanComboBox(cboSan, danhSachSan, "TenSan", "MaSan");
        if (danhSachSan.Count == 0)
            TroGiup.GanComboBox(cboSan, ServiceFactory.San.LayTatCa(), "TenSan", "MaSan");
    }

    protected override void TaiDuLieu() => TinhTien();

    protected override void CapNhatTrangThaiNut()
    {
        btnDatSan.Enabled = PhienLamViec.MaKH != null && PhanQuyenService.CoQuyen(MaQuyen.DatSanThem);
        btnLamMoi.Enabled = true;
    }

    private void TinhTien()
    {
        errLoi.Clear();
        int maSan = TroGiup.LayGiaTriComboBox(cboSan);
        if (maSan <= 0)
        {
            XoaBangTien();
            return;
        }

        San sanDangChon = cboSan.SelectedItem as San;
        lblThongTinSan.Text = sanDangChon == null
            ? "—"
            : $"{sanDangChon.TenLoaiSan}  |  {TroGiup.Tien(sanDangChon.DonGia)}/giờ";

        KetQua<ChiTietTien> ketQua = ServiceFactory.TinhTien.TinhTien(maSan, dtpNgayDat.Value.Date,
            TroGiup.LayGio(dtpGioBatDau), TroGiup.LayGio(dtpGioKetThuc), txtMaVoucher.Text.Trim(),
            PhienLamViec.MaKH);

        if (!ketQua.ThanhCong)
        {
            _ketQuaTien = null;
            lblTrangThaiUuDai.Text = ketQua.ThongBao;
            lblTrangThaiUuDai.ForeColor = GiaoDien.NguyHiem;
            XoaBangTien();
            return;
        }

        _ketQuaTien = ketQua.DuLieu;
        lblSoGio.Text = $"{ketQua.DuLieu.SoGio:0.##} giờ ({ketQua.DuLieu.SoBlock} block x {ketQua.DuLieu.ThoiLuongBlockPhut} phút)";
        lblTienGoc.Text = TroGiup.Tien(ketQua.DuLieu.TienGoc);
        lblUuDai.Text = ketQua.DuLieu.TenUuDai;
        lblTienGiam.Text = "-" + TroGiup.Tien(ketQua.DuLieu.TienGiam);
        lblTongTien.Text = TroGiup.Tien(ketQua.DuLieu.TongTien);
        lblTrangThaiUuDai.Text = ketQua.ThongBao;
        lblTrangThaiUuDai.ForeColor = ketQua.DuLieu.TienGiam > 0 ? GiaoDien.ThanhCong : GiaoDien.ChuPhu;

        KiemTraTrungLich();
    }

    private void XoaBangTien()
    {
        lblSoGio.Text = "—";
        lblTienGoc.Text = "0 đ";
        lblUuDai.Text = "—";
        lblTienGiam.Text = "0 đ";
        lblTongTien.Text = "0 đ";
    }

    private void KiemTraTrungLich()
    {
        int maSan = TroGiup.LayGiaTriComboBox(cboSan);
        if (maSan <= 0) return;

        List<DatSan> trung = ServiceFactory.DatSan.LayTrungLich(maSan, dtpNgayDat.Value.Date,
            TroGiup.LayGio(dtpGioBatDau), TroGiup.LayGio(dtpGioKetThuc));

        lblCanhBaoTrung.Visible = trung.Count > 0;
        if (trung.Count > 0)
            lblCanhBaoTrung.Text = "⚠ " + ServiceFactory.DatSan.TaoThongBaoTrungLich(trung).Replace("\n", " ");
        CapNhatTrangThaiNut();
    }

    private void TaiLichTrongNgay()
    {
        ThucHien(() =>
        {
            int maSan = TroGiup.LayGiaTriComboBox(cboSan);
            if (maSan <= 0) return;
            Luoi.GanDuLieu(dgvLichTrongNgay,
                ServiceFactory.DatSan.LayTheoNgay(dtpNgayDat.Value.Date, maSan)
                    .Where(d => d.TrangThai != TrangThaiDatSan.DaHuy)
                    .ToList());
        }, "Không thể tải lịch sân");
    }

    private void btnDatSan_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.DatSanThem)) return;
        if (PhienLamViec.MaKH == null)
        {
            CanhBao("Tài khoản của bạn chưa gắn với hồ sơ khách hàng, vui lòng liên hệ quầy để được hỗ trợ.",
                "Không thể đặt sân");
            return;
        }
        if (_ketQuaTien == null)
        {
            CanhBao("Khung giờ chưa hợp lệ, vui lòng kiểm tra lại giờ bắt đầu / kết thúc.", "Chưa thể đặt sân");
            return;
        }

        KetQua<DatSan> ketQua = ServiceFactory.DatSan.TaoDatSan(PhienLamViec.MaKH.Value,
            TroGiup.LayGiaTriComboBox(cboSan), dtpNgayDat.Value.Date,
            TroGiup.LayGio(dtpGioBatDau), TroGiup.LayGio(dtpGioKetThuc),
            txtGhiChu.Text.Trim(), txtMaVoucher.Text.Trim());

        if (!ThucHien(ketQua, "Đặt sân thành công! Vui lòng đến quầy thanh toán trước giờ chơi.")) return;

        txtGhiChu.Clear();
        txtMaVoucher.Clear();
        TaiLichTrongNgay();
        TinhTien();
    }

    private void btnLamMoi_Click(object sender, EventArgs e)
    {
        txtGhiChu.Clear();
        txtMaVoucher.Clear();
        TaiLichTrongNgay();
        TinhTien();
    }

    private void cboSan_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!IsHandleCreated) return;
        TaiLichTrongNgay();
        TinhTien();
    }

    private void ThayDoiThoiGian(object sender, EventArgs e)
    {
        if (!IsHandleCreated) return;
        TinhTien();
    }

    private void txtMaVoucher_TextChanged(object sender, EventArgs e)
    {
        if (IsHandleCreated) TinhTien();
    }

    private void btnVoucherCuaToi_Click(object sender, EventArgs e)
    {
        using var voucherCuaToi = new frmVoucherCuaToi();
        voucherCuaToi.ShowDialog(this);
    }
}
