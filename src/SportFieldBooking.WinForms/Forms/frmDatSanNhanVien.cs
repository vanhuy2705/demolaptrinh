using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>
/// Đặt sân (Nhân viên): chọn khách/sân/ngày/giờ, kiểm tra trùng lịch, xem bảng tính tiền
/// (tiền gốc - ưu đãi - tổng tiền) trước khi xác nhận, có thể áp dụng voucher.
/// </summary>
public partial class frmDatSanNhanVien : BaseForm
{
    private List<KhachHang> _danhSachKhachHang = new();
    private List<San> _danhSachSan = new();
    private ChiTietTien _ketQuaTien;
    private int _phienTinhTien;

    public frmDatSanNhanVien()
    {
        InitializeComponent();
    }

    /// <summary>Mở sẵn form với một khách hàng đã chọn (dùng khi chuyển từ màn hình khách hàng).</summary>
    public frmDatSanNhanVien(int maKHDuocChon) : this() => _maKHDuocChon = maKHDuocChon;

    private readonly int _maKHDuocChon;

    protected override string MaQuyenYeuCau => MaQuyen.DatSanThem;

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

    protected override async Task TaiDuLieuAsync()
    {
        BatDauBan();
        try
        {
            var dulieu = await ChayNenAsync(() => (
                khachHang: ServiceFactory.KhachHang.LayTatCa(),
                san: ServiceFactory.San.LayTatCa()));

            _danhSachKhachHang = dulieu.khachHang;
            _danhSachSan = dulieu.san;
            TroGiup.GanComboBox(cboKhachHang, _danhSachKhachHang, "HoTen", "MaKH");
            TroGiup.GanComboBox(cboSan, _danhSachSan, "TenSan", "MaSan");
            if (_maKHDuocChon > 0) cboKhachHang.SelectedValue = _maKHDuocChon;
        }
        catch (Exception ex) { BaoLoi("Không thể tải danh sách khách hàng / sân", ex); }
        finally { KetThucBan(); }

        await TaiLichTrongNgayAsync();
        await TinhTienAsync();
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



    private async Task TaiLichTrongNgayAsync()
    {
        int maSan = TroGiup.LayGiaTriComboBox(cboSan);
        if (maSan <= 0) return;
        DateTime ngay = dtpNgayDat.Value.Date;

        BatDauBan();
        try
        {
            var lich = await ChayNenAsync(() => ServiceFactory.DatSan.LayTheoNgay(ngay, maSan));
            Luoi.GanDuLieu(dgvLichTrongNgay, lich);
            CapNhatTrangThaiNut();
        }
        catch (Exception ex) { BaoLoi("Không thể tải lịch sân", ex); }
        finally { KetThucBan(); }
    }

    /// <summary>Gọi tầng nghiệp vụ tính tiền (block 30 phút + ưu tiên giảm giá) ở luồng nền.</summary>
    private async Task TinhTienAsync()
    {
        errLoi.Clear();
        int maSan = TroGiup.LayGiaTriComboBox(cboSan);
        if (maSan <= 0)
        {
            XoaBangTien();
            return;
        }

        int maKH = TroGiup.LayGiaTriComboBox(cboKhachHang);
        DateTime ngay = dtpNgayDat.Value.Date;
        TimeSpan gioBatDau = TroGiup.LayGio(dtpGioBatDau), gioKetThuc = TroGiup.LayGio(dtpGioKetThuc);
        string maVoucher = txtMaVoucher.Text.Trim();
        int phien = ++_phienTinhTien;

        KetQua<ChiTietTien> ketQua;
        List<DatSan> trung;
        try
        {
            (ketQua, trung) = await ChayNenAsync(() => (
                ServiceFactory.TinhTien.TinhTien(maSan, ngay, gioBatDau, gioKetThuc, maVoucher, maKH),
                ServiceFactory.DatSan.LayTrungLich(maSan, ngay, gioBatDau, gioKetThuc)));
        }
        catch (Exception ex) { BaoLoi("Không thể tính tiền", ex); return; }

        if (phien != _phienTinhTien || IsDisposed) return; // đã có yêu cầu mới hơn

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

        HienThiCanhBaoTrung(trung);
    }

    /// <summary>Cảnh báo xung đột lịch ngay khi chọn khung giờ.</summary>
    private void HienThiCanhBaoTrung(List<DatSan> trung)
    {
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

    private void XoaBangTien()
    {
        lblDonGia.Text = "—";
        lblSoGio.Text = "—";
        lblTienGoc.Text = "0 đ";
        lblUuDai.Text = "—";
        lblTienGiam.Text = "0 đ";
        lblTongTien.Text = "0 đ";
    }


    private async void btnDatSan_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.DatSanThem)) return;
        // Luôn tính lại tiền ngay trước khi đặt: tránh đặt sân với giá cũ hiển thị từ
        // trước khi đổi ngày/giờ/sân/voucher mà quên bấm "Tính tiền".
        await TinhTienAsync();
        if (!HopLe()) return;

        int maKH = TroGiup.LayGiaTriComboBox(cboKhachHang);
        int maSan = TroGiup.LayGiaTriComboBox(cboSan);
        DateTime ngay = dtpNgayDat.Value.Date;
        TimeSpan gioBatDau = TroGiup.LayGio(dtpGioBatDau), gioKetThuc = TroGiup.LayGio(dtpGioKetThuc);
        string ghiChu = txtGhiChu.Text.Trim(), maVoucher = txtMaVoucher.Text.Trim();

        KetQua<DatSan> ketQua = await ChayNenAsync(() => ServiceFactory.DatSan.TaoDatSan(
            maKH, maSan, ngay, gioBatDau, gioKetThuc, ghiChu, maVoucher));
        if (IsDisposed) return;

        if (!ThucHien(ketQua)) return;

        txtGhiChu.Clear();
        txtMaVoucher.Clear();
        _ = TaiLichTrongNgayAsync();
        _ = TinhTienAsync();
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

    private void btnTinhTien_Click(object sender, EventArgs e) => _ = TinhTienAsync();

    private void btnLamMoi_Click(object sender, EventArgs e)
    {
        txtGhiChu.Clear();
        txtMaVoucher.Clear();
        _ = TaiDuLieuAsync();
    }

    private void btnXemChiTiet_Click(object sender, EventArgs e)
    {
        int maDat = Luoi.LayMaDangChon(dgvLichTrongNgay, "MaDat");
        if (maDat <= 0) return;
        using var chiTiet = new frmChiTietDatSan(maDat, coQuyenQuanLy: true);
        chiTiet.ShowDialog(this);
        _ = TaiLichTrongNgayAsync();
    }

    private async void btnHuyBooking_Click(object sender, EventArgs e)
    {
        DatSan dangChon = Luoi.LayDongDangChon<DatSan>(dgvLichTrongNgay);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.DatSanHuy)) return;

        string lyDo = frmNhapLieu.NhapChuoi("Hủy booking", "Lý do hủy (không bắt buộc):", "Khách hủy", false, this, batBuocNhap: false);
        if (lyDo == null) return;

        await ThucHienAsync(() => ServiceFactory.DatSan.HuyDatSan(dangChon.MaDat, lyDo));
        _ = TaiLichTrongNgayAsync();
        _ = TinhTienAsync();
    }

    private async void btnLapHoaDon_Click(object sender, EventArgs e)
    {
        DatSan dangChon = Luoi.LayDongDangChon<DatSan>(dgvLichTrongNgay);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.HdLap)) return;

        int maDat = dangChon.MaDat;
        string maVoucher = txtMaVoucher.Text.Trim();

        KetQua<HoaDon> ketQua = await ChayNenAsync(() => ServiceFactory.HoaDon.LapHoaDon(maDat, maVoucher));
        if (IsDisposed) return;
        if (!ThucHien(ketQua)) return;

        using var chiTietHoaDon = new frmChiTietHoaDon(ketQua.DuLieu.MaHD, coQuyenThuTien: true);
        chiTietHoaDon.ShowDialog(this);
        _ = TaiLichTrongNgayAsync();
    }

    private void cboSan_SelectedIndexChanged(object sender, EventArgs e)
    {
        San sanDangChon = cboSan.SelectedItem as San;
        lblThongTinSan.Text = sanDangChon == null
            ? "—"
            : $"{sanDangChon.TenLoaiSan}  |  {TrangThaiSan.TenHienThi(sanDangChon.TrangThai)}";

        if (!IsHandleCreated) return;
        _ = TaiLichTrongNgayAsync();
        _ = TinhTienAsync();
    }

    private void cboKhachHang_SelectedIndexChanged(object sender, EventArgs e)
    {
        KhachHang khach = cboKhachHang.SelectedItem as KhachHang;
        lblThongTinKhach.Text = khach == null ? "—" : $"{khach.SDT}  |  {khach.Email}";
        if (IsHandleCreated) _ = TinhTienAsync();
    }

    private void ThayDoiThoiGian(object sender, EventArgs e)
    {
        if (!IsHandleCreated) return;
        _ = TinhTienAsync();
    }

    private void txtMaVoucher_TextChanged(object sender, EventArgs e)
    {
        if (IsHandleCreated) _ = TinhTienAsync();
    }

    private void dgvLichTrongNgay_SelectionChanged(object sender, EventArgs e) => CapNhatTrangThaiNut();
}
