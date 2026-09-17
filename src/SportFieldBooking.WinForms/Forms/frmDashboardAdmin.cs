using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Trang tổng quan của Quản trị viên: KPI, biểu đồ doanh thu, tình trạng sân, lịch hôm nay.</summary>
public partial class frmDashboardAdmin : BaseForm
{
    public frmDashboardAdmin()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.ThongKeToanBo;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvHomNay);
        Luoi.DatTieuDe(dgvHomNay,
            ("MaDat", "Mã"),
            ("GioBatDau", "Bắt đầu"),
            ("GioKetThuc", "Kết thúc"),
            ("TenSan", "Sân"),
            ("TenKH", "Khách hàng"),
            ("SDT", "Số điện thoại"),
            ("TienSan", "Tiền sân"),
            ("TrangThai", "Trạng thái"));
        Luoi.AnCot(dgvHomNay, "MaKH", "MaSan", "MaNguoiTao", "NgayTao", "NgayDat");
        Luoi.DatDoRong(dgvHomNay, "MaDat", 60);
        Luoi.DatDoRong(dgvHomNay, "GioBatDau", 90);
        Luoi.DatDoRong(dgvHomNay, "GioKetThuc", 90);
        Luoi.DatDoRong(dgvHomNay, "SDT", 110);
        Luoi.DatDinhDangTien(dgvHomNay, "TienSan");
        Luoi.HienThiTrangThai(dgvHomNay, "TrangThai", TrangThaiDatSan.TenHienThi);
        Luoi.ToMauTrangThai(dgvHomNay, "TrangThai");
    }

    protected override void TaiDuLieu() => TaiTongQuan();

    private void TaiTongQuan()
    {
        ThucHien(() =>
        {
            DateTime homNay = DateTime.Today;
            TongQuan tongQuan = ServiceFactory.ThongKe.LayTongQuan(homNay.AddDays(-29), homNay);

            kpiDoanhThu.DatNoiDung("Doanh thu hôm nay", TroGiup.Tien(tongQuan.DoanhThuHomNay),
                $"30 ngày: {TroGiup.Tien(tongQuan.DoanhThuTrongKy)}");
            kpiBooking.DatNoiDung("Booking hôm nay", tongQuan.BookingHomNay.ToString(),
                $"30 ngày: {tongQuan.BookingTrongKy}");
            kpiChuaThanhToan.DatNoiDung("Hóa đơn chưa thu", tongQuan.HoaDonChuaThanhToan.ToString(),
                $"{tongQuan.TongKhachHang} khách hàng");
            kpiSan.DatNoiDung("Tình trạng sân", $"{tongQuan.SanTrong}/{tongQuan.TongSoSan} trống",
                $"Đang thuê: {tongQuan.SanDangThue}  |  Bảo trì: {tongQuan.SanBaoTri}");

            List<DoanhThuNgay> doanhThu = ServiceFactory.ThongKe.DoanhThuTheoNgay(homNay.AddDays(-6), homNay);
            BieuDo.VeDuong(plotDoanhThu,
                doanhThu.Select(d => d.Ngay.ToString("dd/MM")).ToList(),
                doanhThu.Select(d => (double)d.DoanhThu).ToList(),
                "Doanh thu", "#0F766E", "Doanh thu 7 ngày gần nhất");

            BieuDo.VeTron(plotSan,
                new List<string> { "Sân trống", "Đang thuê", "Bảo trì" },
                new List<double> { tongQuan.SanTrong, tongQuan.SanDangThue, tongQuan.SanBaoTri },
                "Tình trạng sân");

            List<ThongKeSan> topSan = ServiceFactory.ThongKe.ThongKeTheoSan(homNay.AddDays(-29), homNay, 5);
            BieuDo.VeCot(plotTopSan,
                topSan.Select(s => s.TenSan).ToList(),
                topSan.Select(s => (double)s.DoanhThu).ToList(),
                "Doanh thu", "#16A34A", "Top sân 30 ngày", "VNĐ");

            ServiceFactory.DatSan.CapNhatBookingDangSuDung();
            Luoi.GanDuLieu(dgvHomNay, ServiceFactory.DatSan.LayTheoNgay(homNay));
        }, "Không thể tải dữ liệu tổng quan");
    }

    private void btnLamMoi_Click(object sender, EventArgs e) => TaiTongQuan();
}
