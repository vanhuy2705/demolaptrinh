using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Lịch đặt sân (Nhân viên): lọc theo ngày/sân/trạng thái, cảnh báo xung đột.</summary>
public partial class frmLichDatSanNhanVien : BaseForm
{
    private List<DatSan> _danhSach = new();

    public frmLichDatSanNhanVien()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.LichXemTatCa;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvLich);
        Luoi.DatTieuDe(dgvLich,
            ("MaDat", "Mã"),
            ("NgayDat", "Ngày"),
            ("GioBatDau", "Bắt đầu"),
            ("GioKetThuc", "Kết thúc"),
            ("TenSan", "Sân"),
            ("TenKH", "Khách hàng"),
            ("SDT", "Số điện thoại"),
            ("TienSan", "Tiền sân"),
            ("TrangThai", "Trạng thái"));
        Luoi.DatDoRong(dgvLich, "MaDat", 60);
        Luoi.DatDoRong(dgvLich, "NgayDat", 100);
        Luoi.DatDoRong(dgvLich, "GioBatDau", 90);
        Luoi.DatDoRong(dgvLich, "GioKetThuc", 90);
        Luoi.DatDoRong(dgvLich, "SDT", 120);
        Luoi.DatDoRong(dgvLich, "TrangThai", 120);
        Luoi.DatDinhDangNgay(dgvLich, "dd/MM/yyyy", "NgayDat");
        Luoi.DatDinhDangTien(dgvLich, "TienSan");
        Luoi.HienThiTrangThai(dgvLich, "TrangThai", TrangThaiDatSan.TenHienThi);
        Luoi.ToMauTrangThai(dgvLich, "TrangThai");

        dtpTuNgay.Value = DateTime.Today;
        dtpDenNgay.Value = DateTime.Today.AddDays(7);

        cboLocSan.Items.Clear();
        cboLocSan.Items.Add("Tất cả sân");
        cboLocSan.DisplayMember = nameof(SanLoc.TenSan);
        cboLocSan.ValueMember = nameof(SanLoc.MaSan);
        cboLocSan.SelectedIndex = 0;

        cboLocTrangThai.Items.Clear();
        cboLocTrangThai.Items.AddRange(new object[] { "Tất cả", "Đã đặt", "Đang sử dụng", "Hoàn thành", "Đã hủy" });
        cboLocTrangThai.SelectedIndex = 0;
    }

    private sealed class SanLoc
    {
        public SanLoc(int maSan, string tenSan)
        {
            MaSan = maSan;
            TenSan = tenSan;
        }
        public int MaSan { get; }
        public string TenSan { get; }
    }

    protected override async Task TaiDuLieuAsync()
    {
        await NapDanhSachSanAsync();
        await TimKiemAsync();
    }

    private async Task NapDanhSachSanAsync()
    {
        BatDauBan();
        try
        {
            List<San> danhSachSan = await ChayNenAsync(() => ServiceFactory.San.LayTatCa());
            cboLocSan.Items.Clear();
            cboLocSan.Items.Add("Tất cả sân");
            foreach (San san in danhSachSan)
                cboLocSan.Items.Add(new SanLoc(san.MaSan, san.TenSan));
            cboLocSan.DisplayMember = nameof(SanLoc.TenSan);
            cboLocSan.ValueMember = nameof(SanLoc.MaSan);
            cboLocSan.SelectedIndex = 0;
        }
        catch (Exception ex) { BaoLoi("Không thể tải danh sách sân", ex); }
        finally { KetThucBan(); }
    }

    protected override void CapNhatTrangThaiNut()
    {
        bool coChon = Luoi.LayDongDangChon<DatSan>(dgvLich) != null;
        btnXemChiTiet.Enabled = coChon;
        btnHuyBooking.Enabled = coChon && PhanQuyenService.CoQuyen(MaQuyen.DatSanHuy);
        btnLapHoaDon.Enabled = coChon && PhanQuyenService.CoQuyen(MaQuyen.HdLap);
    }

    private async Task TimKiemAsync()
    {
        int? maSan = cboLocSan.SelectedIndex > 0 ? ((SanLoc)cboLocSan.SelectedItem).MaSan : null;
        string trangThai = cboLocTrangThai.SelectedIndex switch
        {
            1 => TrangThaiDatSan.DaDat,
            2 => TrangThaiDatSan.DangSuDung,
            3 => TrangThaiDatSan.HoanThanh,
            4 => TrangThaiDatSan.DaHuy,
            _ => null
        };
        DateTime tuNgay = dtpTuNgay.Value.Date, denNgay = dtpDenNgay.Value.Date;
        string tuKhoa = txtTimKiem.Text.Trim().ToLowerInvariant();

        BatDauBan();
        try
        {
            _danhSach = await ChayNenAsync(() =>
            {
                ServiceFactory.DatSan.CapNhatBookingDangSuDung();
                return ServiceFactory.DatSan.LayTheoKhoang(tuNgay, denNgay, maSan, trangThai);
            });

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                _danhSach = _danhSach.Where(d =>
                    (d.TenKH ?? "").ToLowerInvariant().Contains(tuKhoa) ||
                    (d.SDT ?? "").Contains(tuKhoa) ||
                    (d.TenSan ?? "").ToLowerInvariant().Contains(tuKhoa)).ToList();
            }

            Luoi.GanDuLieu(dgvLich, _danhSach);
            ThongKeNhanh();
            CapNhatTrangThaiNut();
        }
        catch (Exception ex) { BaoLoi("Không thể tải lịch đặt sân", ex); }
        finally { KetThucBan(); }
    }

    private void ThongKeNhanh()
    {
        int tong = _danhSach.Count;
        int daDat = _danhSach.Count(d => d.TrangThai == TrangThaiDatSan.DaDat);
        int dangSuDung = _danhSach.Count(d => d.TrangThai == TrangThaiDatSan.DangSuDung);
        int daHuy = _danhSach.Count(d => d.TrangThai == TrangThaiDatSan.DaHuy);
        decimal tongTien = _danhSach.Where(d => d.TrangThai != TrangThaiDatSan.DaHuy).Sum(d => d.TienSan);
        lblThongKe.Text = $"Tổng {tong} lượt  |  Đã đặt: {daDat}  |  Đang sử dụng: {dangSuDung}  |  Đã hủy: {daHuy}  |  Tiền sân: {TroGiup.Tien(tongTien)}";
    }

    private void btnHomNay_Click(object sender, EventArgs e)
    {
        dtpTuNgay.Value = DateTime.Today;
        dtpDenNgay.Value = DateTime.Today;
        _ = TimKiemAsync();
    }

    private void btnLamMoi_Click(object sender, EventArgs e)
    {
        txtTimKiem.Clear();
        _ = TimKiemAsync();
    }

    private void btnXemChiTiet_Click(object sender, EventArgs e)
    {
        int maDat = Luoi.LayMaDangChon(dgvLich, "MaDat");
        if (maDat <= 0) return;
        using var chiTiet = new frmChiTietDatSan(maDat, coQuyenQuanLy: true);
        if (chiTiet.ShowDialog(this) == DialogResult.OK) _ = TimKiemAsync();
    }

    private async void btnHuyBooking_Click(object sender, EventArgs e)
    {
        DatSan dangChon = Luoi.LayDongDangChon<DatSan>(dgvLich);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.DatSanHuy)) return;

        string lyDo = frmNhapLieu.NhapChuoi("Hủy booking", "Lý do hủy:", "Khách hủy", false, this);
        if (lyDo == null) return;

        await ThucHienAsync(() => ServiceFactory.DatSan.HuyDatSan(dangChon.MaDat, lyDo));
        _ = TimKiemAsync();
    }

    private async void btnLapHoaDon_Click(object sender, EventArgs e)
    {
        DatSan dangChon = Luoi.LayDongDangChon<DatSan>(dgvLich);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.HdLap)) return;

        int maDat = dangChon.MaDat;
        var ketQua = await ChayNenAsync(() => ServiceFactory.HoaDon.LapHoaDon(maDat));
        if (IsDisposed) return;
        if (!ThucHien(ketQua)) return;

        using var chiTietHoaDon = new frmChiTietHoaDon(ketQua.DuLieu.MaHD, coQuyenThuTien: true);
        chiTietHoaDon.ShowDialog(this);
        _ = TimKiemAsync();
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

    private void dgvLich_SelectionChanged(object sender, EventArgs e) => CapNhatTrangThaiNut();
}
