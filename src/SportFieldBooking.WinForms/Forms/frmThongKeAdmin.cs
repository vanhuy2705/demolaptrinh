using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>
/// Thống kê &amp; báo cáo: doanh thu theo ngày, cơ cấu giảm giá, top sân, tổng quan kỳ.
/// </summary>
public partial class frmThongKeAdmin : BaseForm
{
    private TongQuan _tongQuan = new();
    private List<DoanhThuNgay> _doanhThuNgay = new();
    private List<ThongKeGiamGia> _thongKeGiamGia = new();
    private List<ThongKeSan> _thongKeSan = new();

    public frmThongKeAdmin()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.ThongKeToanBo;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvChiTietNgay);
        Luoi.DatTieuDe(dgvChiTietNgay,
            ("Ngay", "Ngày"),
            ("SoBooking", "Số booking"),
            ("TienGoc", "Tiền sân"),
            ("TienGiam", "Giảm giá"),
            ("DoanhThu", "Doanh thu"));
        Luoi.DatDoRong(dgvChiTietNgay, "Ngay", 90);
        Luoi.DatDoRong(dgvChiTietNgay, "SoBooking", 90);
        Luoi.DatDinhDangNgay(dgvChiTietNgay, "dd/MM/yyyy", "Ngay");
        Luoi.DatDinhDangTien(dgvChiTietNgay, "TienGoc", "TienGiam", "DoanhThu");

        Luoi.Dang(dgvTopSan);
        Luoi.DatTieuDe(dgvTopSan,
            ("TenSan", "Sân"),
            ("SoLuotThue", "Số lượt thuê"),
            ("DoanhThu", "Doanh thu"));
        Luoi.DatDoRong(dgvTopSan, "SoLuotThue", 100);
        Luoi.DatDinhDangTien(dgvTopSan, "DoanhThu");

        Luoi.Dang(dgvGiamGia);
        Luoi.DatTieuDe(dgvGiamGia,
            ("LoaiGiamGia", "Loại giảm"),
            ("SoLuong", "Số lượt"),
            ("TongTienGiam", "Tổng tiền giảm"));
        Luoi.DatDinhDangTien(dgvGiamGia, "TongTienGiam");
        dgvGiamGia.CellFormatting += dgvGiamGia_CellFormatting;

        dtpTuNgay.Value = DateTime.Today.AddDays(-29);
        dtpDenNgay.Value = DateTime.Today;
        cboKieuBieuDo.Items.AddRange(new object[] { "Doanh thu theo ngày", "Số booking theo ngày" });
        cboKieuBieuDo.SelectedIndex = 0;
    }

    protected override void TaiDuLieu() => TaiDuLieuThongKe();

    private void TaiDuLieuThongKe()
    {
        ThucHien(() =>
        {
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;

            _tongQuan = ServiceFactory.ThongKe.LayTongQuan(tuNgay, denNgay);
            _doanhThuNgay = ServiceFactory.ThongKe.DoanhThuTheoNgay(tuNgay, denNgay);
            _thongKeGiamGia = ServiceFactory.ThongKe.ThongKeTheoLoaiGiam(tuNgay, denNgay);
            _thongKeSan = ServiceFactory.ThongKe.ThongKeTheoSan(tuNgay, denNgay, 5);

            HienThiKpi();
            Luoi.GanDuLieu(dgvChiTietNgay, _doanhThuNgay);
            Luoi.GanDuLieu(dgvTopSan, _thongKeSan);
            Luoi.GanDuLieu(dgvGiamGia, _thongKeGiamGia);
            VeBieuDo();
        }, "Không thể tải dữ liệu thống kê");
    }

    private void HienThiKpi()
    {
        kpiDoanhThu.DatNoiDung("Doanh thu kỳ", TroGiup.Tien(_tongQuan.DoanhThuTrongKy),
            $"Hôm nay: {TroGiup.Tien(_tongQuan.DoanhThuHomNay)}");
        kpiBooking.DatNoiDung("Booking trong kỳ", _tongQuan.BookingTrongKy.ToString(),
            $"Hôm nay: {_tongQuan.BookingHomNay}");
        kpiChuaThanhToan.DatNoiDung("Hóa đơn chưa thu", _tongQuan.HoaDonChuaThanhToan.ToString(),
            $"{_tongQuan.TongKhachHang} khách hàng");
        kpiGiamGia.DatNoiDung("Tổng tiền giảm", TroGiup.Tien(_tongQuan.TongTienGiam),
            $"Sân trống: {_tongQuan.SanTrong}/{_tongQuan.TongSoSan}");
    }

    private void VeBieuDo()
    {
        List<string> nhanNgay = _doanhThuNgay.Select(d => d.Ngay.ToString("dd/MM")).ToList();

        if (cboKieuBieuDo.SelectedIndex == 0)
            BieuDo.VeDuong(plotDuong, nhanNgay, _doanhThuNgay.Select(d => (double)d.DoanhThu).ToList(),
                "Doanh thu", "#0F766E", "Doanh thu theo ngày");
        else
            BieuDo.VeCot(plotDuong, nhanNgay, _doanhThuNgay.Select(d => (double)d.SoBooking).ToList(),
                "Số booking", "#0EA5E9", "Số booking theo ngày", "Booking");

        BieuDo.VeTron(plotTron,
            _thongKeGiamGia.Select(g => LoaiGiamGia.TenHienThi(g.LoaiGiamGia)).ToList(),
            _thongKeGiamGia.Select(g => (double)g.TongTienGiam).ToList(),
            "Cơ cấu tiền giảm giá");

        BieuDo.VeCot(plotTopSan,
            _thongKeSan.Select(s => s.TenSan).ToList(),
            _thongKeSan.Select(s => (double)s.DoanhThu).ToList(),
            "Doanh thu", "#16A34A", "Top sân theo doanh thu", "VNĐ");
    }

    private void btnHomNay_Click(object sender, EventArgs e)
    {
        dtpTuNgay.Value = DateTime.Today;
        dtpDenNgay.Value = DateTime.Today;
        TaiDuLieuThongKe();
    }

    private void btnBayNgay_Click(object sender, EventArgs e)
    {
        dtpTuNgay.Value = DateTime.Today.AddDays(-6);
        dtpDenNgay.Value = DateTime.Today;
        TaiDuLieuThongKe();
    }

    private void btnThangNay_Click(object sender, EventArgs e)
    {
        DateTime homNay = DateTime.Today;
        dtpTuNgay.Value = new DateTime(homNay.Year, homNay.Month, 1);
        dtpDenNgay.Value = homNay;
        TaiDuLieuThongKe();
    }

    private void btnLamMoi_Click(object sender, EventArgs e) => TaiDuLieuThongKe();

    private void btnXem_Click(object sender, EventArgs e) => TaiDuLieuThongKe();

    private void cboKieuBieuDo_SelectedIndexChanged(object sender, EventArgs e) => VeBieuDo();

    /// <summary>Đổi mã loại giảm (CuoiTuan/Voucher/...) sang tiếng Việt có dấu.</summary>
    private void dgvGiamGia_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || dgvGiamGia.Columns[e.ColumnIndex].Name != "LoaiGiamGia") return;
        if (e.Value is not string ma) return;
        e.Value = LoaiGiamGia.TenHienThi(ma);
        e.FormattingApplied = true;
    }
}
