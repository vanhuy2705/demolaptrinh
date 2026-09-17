using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>
/// Đặt sân (Admin): chọn khách/sân/ngày/giờ, kiểm tra trùng lịch, xem bảng tính tiền
/// (tiền gốc - ưu đãi - tổng tiền) trước khi xác nhận, có thể áp dụng voucher.
/// </summary>
public partial class frmDatSanAdmin : BaseForm
{
    private List<KhachHang> _danhSachKhachHang = new();
    private List<San> _danhSachSan = new();
    private ChiTietTien _ketQuaTien;

    public frmDatSanAdmin()
    {
        InitializeComponent();
    }

    /// <summary>Mở sẵn form với một khách hàng đã chọn (dùng khi chuyển từ màn hình khách hàng).</summary>
    public frmDatSanAdmin(int maKHDuocChon) : this() => _maKHDuocChon = maKHDuocChon;

    private readonly int _maKHDuocChon;

    protected override string MaQuyenYeuCau => MaQuyen.DatSanXemTatCa;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvLichTrongNgay);
        Luoi.DatTieuDe(dgvLichTrongNgay,
            ("MaDat", "Mã"),
            ("TenKH", "Khách hàng"),
            ("GioBatDau", "Bắt đầu"),
            ("GioKetThuc", "Kết thúc"),
            ("TienSan", "Tiền sân"),
            ("TrangThai", "Trạng thái"));
        Luoi.DatDoRong(dgvLichTrongNgay, "MaDat", 60);
        Luoi.DatDoRong(dgvLichTrongNgay, "GioBatDau", 100);
        Luoi.DatDoRong(dgvLichTrongNgay, "GioKetThuc", 100);
        Luoi.DatDinhDangTien(dgvLichTrongNgay, "TienSan");
        Luoi.HienThiTrangThai(dgvLichTrongNgay, "TrangThai", TrangThaiDatSan.TenHienThi);
        Luoi.ToMauTrangThai(dgvLichTrongNgay, "TrangThai");

        dtpNgayDat.Value = DateTime.Today;
        dtpGioBatDau.Format = DateTimePickerFormat.Custom;
        dtpGioBatDau.CustomFormat = "HH:mm";
        dtpGioBatDau.ShowUpDown = true;
        dtpGioKetThuc.Format = DateTimePickerFormat.Custom;
        dtpGioKetThuc.CustomFormat = "HH:mm";
        dtpGioKetThuc.ShowUpDown = true;
        TroGiup.DatGio(dtpGioBatDau, new TimeSpan(17, 0, 0));
        TroGiup.DatGio(dtpGioKetThuc, new TimeSpan(18, 0, 0));
    }

    protected override void TaiDuLieu()
    {
        NapDanhSachKhachHang();
        NapDanhSachSan();
        if (_maKHDuocChon > 0) cboKhachHang.SelectedValue = _maKHDuocChon;
        TaiLichTrongNgay();
        TinhTien();
    }

    private void NapDanhSachKhachHang()
    {
        _danhSachKhachHang = ServiceFactory.KhachHang.LayTatCa();
        TroGiup.GanComboBox(cboKhachHang, _danhSachKhachHang, "HoTen", "MaKH");
    }

    private void NapDanhSachSan()
    {
        _danhSachSan = ServiceFactory.San.LayTatCa();
        TroGiup.GanComboBox(cboSan, _danhSachSan, "TenSan", "MaSan");
    }

    protected override void CapNhatTrangThaiNut()
    {
        btnDatSan.Enabled = PhanQuyenService.CoQuyen(MaQuyen.DatSanThem);
        btnTinhTien.Enabled = true;
        btnLamMoi.Enabled = true;
        btnXemChiTiet.Enabled = Luoi.LayDongDangChon<DatSan>(dgvLichTrongNgay) != null;
        btnHuyBooking.Enabled = btnXemChiTiet.Enabled && PhanQuyenService.CoQuyen(MaQuyen.DatSanHuy);
        btnLapHoaDon.Enabled = btnXemChiTiet.Enabled && PhanQuyenService.CoQuyen(MaQuyen.HdLap);
    }

    private void TaiLichTrongNgay()
    {
        ThucHien(() =>
        {
            int maSan = TroGiup.LayGiaTriComboBox(cboSan);
            if (maSan <= 0) return;
            Luoi.GanDuLieu(dgvLichTrongNgay, ServiceFactory.DatSan.LayTheoNgay(dtpNgayDat.Value.Date, maSan));
            CapNhatTrangThaiNut();
        }, "Không thể tải lịch sân");
    }

    /// <summary>Gọi tầng nghiệp vụ tính tiền (block 30 phút + ưu tiên giảm giá).</summary>
    private void TinhTien()
    {
        errLoi.Clear();
        int maSan = TroGiup.LayGiaTriComboBox(cboSan);
        if (maSan <= 0)
        {
            XoaBangTien();
            return;
        }

        KetQua<ChiTietTien> ketQua = ServiceFactory.TinhTien.TinhTien(maSan, dtpNgayDat.Value.Date,
            TroGiup.LayGio(dtpGioBatDau), TroGiup.LayGio(dtpGioKetThuc), txtMaVoucher.Text.Trim(),
            TroGiup.LayGiaTriComboBox(cboKhachHang));

        if (!ketQua.ThanhCong)
        {
            _ketQuaTien = null;
            lblTrangThaiUuDai.Text = ketQua.ThongBao;
            lblTrangThaiUuDai.ForeColor = GiaoDien.NguyHiem;
            XoaBangTien();
            return;
        }

        _ketQuaTien = ketQua.DuLieu;
        lblDonGia.Text = TroGiup.Tien(ketQua.DuLieu.DonGia) + "/giờ";
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
        lblDonGia.Text = "—";
        lblSoGio.Text = "—";
        lblTienGoc.Text = "0 đ";
        lblUuDai.Text = "—";
        lblTienGiam.Text = "0 đ";
        lblTongTien.Text = "0 đ";
    }

    /// <summary>Cảnh báo xung đột lịch ngay khi chọn khung giờ.</summary>
    private void KiemTraTrungLich()
    {
        int maSan = TroGiup.LayGiaTriComboBox(cboSan);
        if (maSan <= 0) return;

        List<DatSan> trung = ServiceFactory.DatSan.LayTrungLich(maSan, dtpNgayDat.Value.Date,
            TroGiup.LayGio(dtpGioBatDau), TroGiup.LayGio(dtpGioKetThuc));

        if (trung.Count > 0)
        {
            lblCanhBaoTrung.Text = "⚠ " + ServiceFactory.DatSan.TaoThongBaoTrungLich(trung).Replace("\n", " ");
            lblCanhBaoTrung.ForeColor = GiaoDien.NguyHiem;
            lblCanhBaoTrung.Visible = true;
            btnDatSan.Enabled = false;
        }
        else
        {
            lblCanhBaoTrung.Visible = false;
            CapNhatTrangThaiNut();
        }
    }

    private void btnDatSan_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.DatSanThem)) return;
        if (!HopLe()) return;

        KetQua<DatSan> ketQua = ServiceFactory.DatSan.TaoDatSan(
            TroGiup.LayGiaTriComboBox(cboKhachHang),
            TroGiup.LayGiaTriComboBox(cboSan),
            dtpNgayDat.Value.Date,
            TroGiup.LayGio(dtpGioBatDau),
            TroGiup.LayGio(dtpGioKetThuc),
            txtGhiChu.Text.Trim(),
            txtMaVoucher.Text.Trim());

        if (!ThucHien(ketQua)) return;

        txtGhiChu.Clear();
        txtMaVoucher.Clear();
        TaiLichTrongNgay();
        TinhTien();
    }

    private bool HopLe()
    {
        errLoi.Clear();
        bool loi = TroGiup.Sai(TroGiup.LayGiaTriComboBox(cboKhachHang) <= 0, cboKhachHang,
            "Vui lòng chọn khách hàng.", errLoi);
        loi |= TroGiup.Sai(TroGiup.LayGiaTriComboBox(cboSan) <= 0, cboSan,
            "Vui lòng chọn sân.", errLoi);
        loi |= TroGiup.Sai(_ketQuaTien == null, dtpGioKetThuc,
            "Khung giờ chưa hợp lệ hoặc chưa tính được tiền, vui lòng kiểm tra lại.", errLoi);
        return !loi;
    }

    private void btnTinhTien_Click(object sender, EventArgs e) => TinhTien();

    private void btnLamMoi_Click(object sender, EventArgs e)
    {
        txtGhiChu.Clear();
        txtMaVoucher.Clear();
        TaiDuLieu();
    }

    private void btnXemChiTiet_Click(object sender, EventArgs e)
    {
        int maDat = Luoi.LayMaDangChon(dgvLichTrongNgay, "MaDat");
        if (maDat <= 0) return;
        using var chiTiet = new frmChiTietDatSan(maDat, coQuyenQuanLy: true);
        chiTiet.ShowDialog(this);
        TaiLichTrongNgay();
    }

    private void btnHuyBooking_Click(object sender, EventArgs e)
    {
        DatSan dangChon = Luoi.LayDongDangChon<DatSan>(dgvLichTrongNgay);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.DatSanHuy)) return;

        string lyDo = frmNhapLieu.NhapChuoi("Hủy booking", "Lý do hủy (không bắt buộc):", "Khách hủy", false, this);
        if (lyDo == null) return;

        ThucHien(ServiceFactory.DatSan.HuyDatSan(dangChon.MaDat, lyDo));
        TaiLichTrongNgay();
        TinhTien();
    }

    private void btnLapHoaDon_Click(object sender, EventArgs e)
    {
        DatSan dangChon = Luoi.LayDongDangChon<DatSan>(dgvLichTrongNgay);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.HdLap)) return;

        KetQua<HoaDon> ketQua = ServiceFactory.HoaDon.LapHoaDon(dangChon.MaDat, txtMaVoucher.Text.Trim());
        if (!ThucHien(ketQua)) return;

        using var chiTietHoaDon = new frmChiTietHoaDon(ketQua.DuLieu.MaHD, coQuyenThuTien: true);
        chiTietHoaDon.ShowDialog(this);
        TaiLichTrongNgay();
    }

    private void cboSan_SelectedIndexChanged(object sender, EventArgs e)
    {
        San sanDangChon = cboSan.SelectedItem as San;
        lblThongTinSan.Text = sanDangChon == null
            ? "—"
            : $"{sanDangChon.TenLoaiSan}  |  {TrangThaiSan.TenHienThi(sanDangChon.TrangThai)}";

        if (!IsHandleCreated) return;
        TaiLichTrongNgay();
        TinhTien();
    }

    private void cboKhachHang_SelectedIndexChanged(object sender, EventArgs e)
    {
        KhachHang khach = cboKhachHang.SelectedItem as KhachHang;
        lblThongTinKhach.Text = khach == null ? "—" : $"{khach.SDT}  |  {khach.Email}";
        if (IsHandleCreated) TinhTien();
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

    private void dgvLichTrongNgay_SelectionChanged(object sender, EventArgs e) => CapNhatTrangThaiNut();
}
