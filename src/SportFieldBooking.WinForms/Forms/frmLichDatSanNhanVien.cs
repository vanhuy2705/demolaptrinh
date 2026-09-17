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

        List<San> danhSachSan = ServiceFactory.San.LayTatCa();
        cboLocSan.Items.Clear();
        cboLocSan.Items.Add("Tất cả sân");
        foreach (San san in danhSachSan)
            cboLocSan.Items.Add(new SanLoc(san.MaSan, san.TenSan));
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

    protected override void TaiDuLieu() => TimKiem();

    protected override void CapNhatTrangThaiNut()
    {
        bool coChon = Luoi.LayDongDangChon<DatSan>(dgvLich) != null;
        btnXemChiTiet.Enabled = coChon;
        btnHuyBooking.Enabled = coChon && PhanQuyenService.CoQuyen(MaQuyen.DatSanHuy);
        btnLapHoaDon.Enabled = coChon && PhanQuyenService.CoQuyen(MaQuyen.HdLap);
    }

    private void TimKiem()
    {
        ThucHien(() =>
        {
            ServiceFactory.DatSan.CapNhatBookingDangSuDung();

            int? maSan = cboLocSan.SelectedIndex > 0 ? ((SanLoc)cboLocSan.SelectedItem).MaSan : null;
            string trangThai = cboLocTrangThai.SelectedIndex switch
            {
                1 => TrangThaiDatSan.DaDat,
                2 => TrangThaiDatSan.DangSuDung,
                3 => TrangThaiDatSan.HoanThanh,
                4 => TrangThaiDatSan.DaHuy,
                _ => null
            };

            _danhSach = ServiceFactory.DatSan.LayTheoKhoang(dtpTuNgay.Value.Date, dtpDenNgay.Value.Date, maSan, trangThai);

            if (!string.IsNullOrWhiteSpace(txtTimKiem.Text))
            {
                string tuKhoa = txtTimKiem.Text.Trim().ToLowerInvariant();
                _danhSach = _danhSach.Where(d =>
                    (d.TenKH ?? "").ToLowerInvariant().Contains(tuKhoa) ||
                    (d.SDT ?? "").Contains(tuKhoa) ||
                    (d.TenSan ?? "").ToLowerInvariant().Contains(tuKhoa)).ToList();
            }

            Luoi.GanDuLieu(dgvLich, _danhSach);
            ThongKeNhanh();
            CapNhatTrangThaiNut();
        }, "Không thể tải lịch đặt sân");
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
        TimKiem();
    }

    private void btnLamMoi_Click(object sender, EventArgs e)
    {
        txtTimKiem.Clear();
        TimKiem();
    }

    private void btnXemChiTiet_Click(object sender, EventArgs e)
    {
        int maDat = Luoi.LayMaDangChon(dgvLich, "MaDat");
        if (maDat <= 0) return;
        using var chiTiet = new frmChiTietDatSan(maDat, coQuyenQuanLy: true);
        if (chiTiet.ShowDialog(this) == DialogResult.OK) TimKiem();
    }

    private void btnHuyBooking_Click(object sender, EventArgs e)
    {
        DatSan dangChon = Luoi.LayDongDangChon<DatSan>(dgvLich);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.DatSanHuy)) return;

        string lyDo = frmNhapLieu.NhapChuoi("Hủy booking", "Lý do hủy:", "Khách hủy", false, this);
        if (lyDo == null) return;

        ThucHien(ServiceFactory.DatSan.HuyDatSan(dangChon.MaDat, lyDo));
        TimKiem();
    }

    private void btnLapHoaDon_Click(object sender, EventArgs e)
    {
        DatSan dangChon = Luoi.LayDongDangChon<DatSan>(dgvLich);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.HdLap)) return;

        var ketQua = ServiceFactory.HoaDon.LapHoaDon(dangChon.MaDat);
        if (!ThucHien(ketQua)) return;

        using var chiTietHoaDon = new frmChiTietHoaDon(ketQua.DuLieu.MaHD, coQuyenThuTien: true);
        chiTietHoaDon.ShowDialog(this);
        TimKiem();
    }

    private void btnTim_Click(object sender, EventArgs e) => TimKiem();

    private void LocThayDoi(object sender, EventArgs e)
    {
        if (IsHandleCreated) TimKiem();
    }

    private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) TimKiem();
    }

    private void dgvLich_SelectionChanged(object sender, EventArgs e) => CapNhatTrangThaiNut();
}
