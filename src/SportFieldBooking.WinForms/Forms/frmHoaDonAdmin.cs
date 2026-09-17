using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>
/// Quản lý hóa đơn: lập hóa đơn từ booking chưa thanh toán, thu tiền, in, hủy hóa đơn.
/// </summary>
public partial class frmHoaDonAdmin : BaseForm
{
    private List<HoaDon> _danhSach = new();

    public frmHoaDonAdmin()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.HdXemTatCa;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvHoaDon);
        Luoi.DatTieuDe(dgvHoaDon,
            ("MaHD", "Mã HĐ"),
            ("NgayLap", "Ngày lập"),
            ("TenKH", "Khách hàng"),
            ("TenSan", "Sân"),
            ("NgayDat", "Ngày đá"),
            ("TienGoc", "Tiền sân"),
            ("TienGiam", "Giảm"),
            ("TongTien", "Tổng tiền"),
            ("LoaiGiamGia", "Ưu đãi"),
            ("TrangThai", "Trạng thái"));
        Luoi.DatDoRong(dgvHoaDon, "MaHD", 66);
        Luoi.DatDoRong(dgvHoaDon, "NgayLap", 130);
        Luoi.DatDoRong(dgvHoaDon, "NgayDat", 100);
        Luoi.DatDinhDangTien(dgvHoaDon, "TienGoc", "TienGiam", "TongTien");
        Luoi.DatDinhDangNgay(dgvHoaDon, "dd/MM/yyyy HH:mm", "NgayLap");
        Luoi.DatDinhDangNgay(dgvHoaDon, "dd/MM/yyyy", "NgayDat");
        dgvHoaDon.Columns["LoaiGiamGia"].Visible = false;
        Luoi.HienThiTrangThai(dgvHoaDon, "TrangThai", TrangThaiHoaDon.TenHienThi);
        Luoi.ToMauTrangThai(dgvHoaDon, "TrangThai");

        dtpTuNgay.Value = DateTime.Today.AddDays(-7);
        dtpDenNgay.Value = DateTime.Today;
        txtTimKiem.PlaceholderText = "Tìm theo tên khách, mã voucher, mã hóa đơn...";
    }

    protected override async Task TaiDuLieuAsync()
    {
        await NapBookingChuaLapAsync();
        await TimKiemAsync();
    }

    private async Task NapBookingChuaLapAsync()
    {
        BatDauBan();
        try
        {
            List<DatSan> chuaThanhToan = await ChayNenAsync(() => ServiceFactory.DatSan.LayTheoKhoang(
                    DateTime.Today.AddDays(-7), DateTime.Today, null, null)
                .Where(d => d.TrangThai == TrangThaiDatSan.DaDat || d.TrangThai == TrangThaiDatSan.DangSuDung)
                .Where(d => ServiceFactory.HoaDon.LayTheoMaDat(d.MaDat) == null)
                .ToList());

            cboBookingChuaLap.Items.Clear();
            foreach (DatSan d in chuaThanhToan)
                cboBookingChuaLap.Items.Add(new MucBooking(d.MaDat,
                    $"#{d.MaDat} | {d.TenSan} | {d.NgayDat:dd/MM} {d.GioBatDau:hh\\:mm}-{d.GioKetThuc:hh\\:mm} | {d.TenKH}"));

            cboBookingChuaLap.DisplayMember = nameof(MucBooking.TieuDe);
            cboBookingChuaLap.ValueMember = nameof(MucBooking.MaDat);
            cboBookingChuaLap.Enabled = chuaThanhToan.Count > 0;
            if (chuaThanhToan.Count == 0) cboBookingChuaLap.Items.Add("(Không có booking chờ lập hóa đơn)");
            cboBookingChuaLap.SelectedIndex = 0;
        }
        catch (Exception ex) { BaoLoi("Không thể tải booking chờ lập hóa đơn", ex); }
        finally { KetThucBan(); }
    }

    private sealed class MucBooking
    {
        public MucBooking(int maDat, string tieuDe)
        {
            MaDat = maDat;
            TieuDe = tieuDe;
        }
        public int MaDat { get; }
        public string TieuDe { get; }
    }

    protected override void CapNhatTrangThaiNut()
    {
        HoaDon dangChon = Luoi.LayDongDangChon<HoaDon>(dgvHoaDon);
        btnChiTiet.Enabled = dangChon != null;
        btnThanhToan.Enabled = dangChon != null && dangChon.TrangThai == TrangThaiHoaDon.ChuaThanhToan
            && PhanQuyenService.CoQuyen(MaQuyen.HdThanhToan);
        btnIn.Enabled = dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.HdIn);
        btnXoa.Enabled = dangChon != null && dangChon.TrangThai != TrangThaiHoaDon.DaThanhToan
            && PhanQuyenService.CoQuyen(MaQuyen.HdXoa);
        btnLapHoaDon.Enabled = cboBookingChuaLap.SelectedItem is MucBooking
            && PhanQuyenService.CoQuyen(MaQuyen.HdLap);
    }

    private async Task TimKiemAsync()
    {
        string trangThai = cboTrangThai.SelectedIndex switch
        {
            1 => TrangThaiHoaDon.ChuaThanhToan,
            2 => TrangThaiHoaDon.DaThanhToan,
            3 => TrangThaiHoaDon.DaHuy,
            _ => null
        };
        DateTime tuNgay = dtpTuNgay.Value.Date, denNgay = dtpDenNgay.Value.Date;
        string tuKhoa = txtTimKiem.Text.Trim().ToLowerInvariant();

        BatDauBan();
        try
        {
            _danhSach = await ChayNenAsync(() =>
                ServiceFactory.HoaDon.LayTheoKhoang(tuNgay, denNgay, trangThai));

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                _danhSach = _danhSach.Where(h =>
                    (h.TenKH ?? "").ToLowerInvariant().Contains(tuKhoa) ||
                    (h.MaCode ?? "").ToLowerInvariant().Contains(tuKhoa) ||
                    h.MaHD.ToString().Contains(tuKhoa)).ToList();
            }

            Luoi.GanDuLieu(dgvHoaDon, _danhSach);
            ThongKeNhanh();
            CapNhatTrangThaiNut();
        }
        catch (Exception ex) { BaoLoi("Không thể tải danh sách hóa đơn", ex); }
        finally { KetThucBan(); }
    }

    private void ThongKeNhanh()
    {
        decimal tongTatCa = _danhSach.Sum(h => h.TongTien);
        decimal daThu = _danhSach.Where(h => h.TrangThai == TrangThaiHoaDon.DaThanhToan).Sum(h => h.TongTien);
        decimal chuaThu = _danhSach.Where(h => h.TrangThai == TrangThaiHoaDon.ChuaThanhToan).Sum(h => h.TongTien);
        decimal tongGiam = _danhSach.Sum(h => h.TienGiam);
        lblThongKe.Text = $"Tổng: {TroGiup.Tien(tongTatCa)}  |  Đã thu: {TroGiup.Tien(daThu)}  |  " +
                          $"Chưa thu: {TroGiup.Tien(chuaThu)}  |  Giảm giá: {TroGiup.Tien(tongGiam)}";
    }

    private async void btnLapHoaDon_Click(object sender, EventArgs e)
    {
        if (cboBookingChuaLap.SelectedItem is not MucBooking muc) return;
        if (!CoQuyen(MaQuyen.HdLap)) return;

        string maVoucher = frmNhapLieu.NhapChuoi("Lập hóa đơn", "Mã voucher (để trống nếu không có):", "", false, this);
        if (maVoucher == null) return;

        int maDat = muc.MaDat;
        string voucher = maVoucher.Trim();
        var ketQua = await ChayNenAsync(() => ServiceFactory.HoaDon.LapHoaDon(maDat, voucher));
        if (IsDisposed) return;
        if (!ThucHien(ketQua)) return;

        using var chiTiet = new frmChiTietHoaDon(ketQua.DuLieu.MaHD, coQuyenThuTien: true);
        chiTiet.ShowDialog(this);
        _ = TaiDuLieuAsync();
    }

    private void btnChiTiet_Click(object sender, EventArgs e)
    {
        int maHD = Luoi.LayMaDangChon(dgvHoaDon, "MaHD");
        if (maHD <= 0) return;
        using var chiTiet = new frmChiTietHoaDon(maHD, coQuyenThuTien: true);
        if (chiTiet.ShowDialog(this) == DialogResult.OK) _ = TaiDuLieuAsync();
    }

    private void btnThanhToan_Click(object sender, EventArgs e)
    {
        HoaDon dangChon = Luoi.LayDongDangChon<HoaDon>(dgvHoaDon);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.HdThanhToan)) return;

        using var chiTiet = new frmChiTietHoaDon(dangChon.MaHD, coQuyenThuTien: true);
        if (chiTiet.ShowDialog(this) == DialogResult.OK) _ = TaiDuLieuAsync();
    }

    private async void btnIn_Click(object sender, EventArgs e)
    {
        HoaDon dangChon = Luoi.LayDongDangChon<HoaDon>(dgvHoaDon);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.HdIn)) return;

        var thongTin = await ChayNenAsync(() => LayThongTinCuaHang());
        if (IsDisposed) return;
        ThucHien(() => InHoaDon.XemTruoc(dangChon, thongTin), "Không thể xem trước hóa đơn");
    }

    private static InHoaDon.ThongTinCuaHang LayThongTinCuaHang() => new()
    {
        TenTrungTam = ServiceFactory.CauHinh.LayGiaTri(ThamSoKeys.TenTrungTam, "TRUNG TÂM THỂ THAO"),
        DiaChi = ServiceFactory.CauHinh.LayGiaTri(ThamSoKeys.DiaChi, ""),
        DienThoai = ServiceFactory.CauHinh.LayGiaTri(ThamSoKeys.DienThoai, ""),
        LoiChao = ServiceFactory.CauHinh.LayGiaTri(ThamSoKeys.LoiChaoHoaDon, "Cảm ơn quý khách, hẹn gặp lại!")
    };

    private async void btnXoa_Click(object sender, EventArgs e)
    {
        HoaDon dangChon = Luoi.LayDongDangChon<HoaDon>(dgvHoaDon);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.HdXoa)) return;
        if (!XacNhan($"Hủy hóa đơn #{dangChon.MaHD}?", "Xác nhận hủy hóa đơn")) return;

        await ThucHienAsync(() => ServiceFactory.HoaDon.HuyHoaDon(dangChon.MaHD, "Hủy từ danh sách hóa đơn"));
        _ = TaiDuLieuAsync();
    }

    private void btnLamMoi_Click(object sender, EventArgs e)
    {
        txtTimKiem.Clear();
        _ = TaiDuLieuAsync();
    }

    private void btnTim_Click(object sender, EventArgs e) => _ = TimKiemAsync();

    private void LocThayDoi(object sender, EventArgs e)
    {
        if (IsHandleCreated) _ = TimKiemAsync();
    }

    private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) _ = TimKiemAsync();
    }

    private void dgvHoaDon_SelectionChanged(object sender, EventArgs e) => CapNhatTrangThaiNut();

    private void cboBookingChuaLap_SelectedIndexChanged(object sender, EventArgs e) => CapNhatTrangThaiNut();
}
