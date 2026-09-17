using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Trang tổng quan của Nhân viên: KPI, biểu đồ doanh thu, tình trạng sân, lịch hôm nay.</summary>
public partial class frmDashboardNhanVien : BaseForm
{
    public frmDashboardNhanVien()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.ThongKeNghiepVu;

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
        Luoi.DatDoRong(dgvHomNay, "MaDat", 60);
        Luoi.DatDoRong(dgvHomNay, "GioBatDau", 90);
        Luoi.DatDoRong(dgvHomNay, "GioKetThuc", 90);
        Luoi.DatDoRong(dgvHomNay, "SDT", 110);
        Luoi.DatDinhDangTien(dgvHomNay, "TienSan");
        Luoi.HienThiTrangThai(dgvHomNay, "TrangThai", TrangThaiDatSan.TenHienThi);
        Luoi.ToMauTrangThai(dgvHomNay, "TrangThai");
    }

    protected override Task TaiDuLieuAsync() => TaiTongQuanAsync();

    private async Task TaiTongQuanAsync()
    {
        DateTime homNay = DateTime.Today;

        BatDauBan();
        try
        {
            var dulieu = await ChayNenAsync(() =>
            {
                ServiceFactory.DatSan.CapNhatBookingDangSuDung();
                return (
                    tongQuan: ServiceFactory.ThongKe.LayTongQuan(homNay.AddDays(-29), homNay),
                    doanhThu: ServiceFactory.ThongKe.DoanhThuTheoNgay(homNay.AddDays(-6), homNay),
                    topSan: ServiceFactory.ThongKe.ThongKeTheoSan(homNay.AddDays(-29), homNay, 5),
                    lichHomNay: ServiceFactory.DatSan.LayTheoNgay(homNay));
            });

            TongQuan tongQuan = dulieu.tongQuan;
            kpiDoanhThu.DatNoiDung("Doanh thu hôm nay", TroGiup.Tien(tongQuan.DoanhThuHomNay),
                $"30 ngày: {TroGiup.Tien(tongQuan.DoanhThuTrongKy)}");
            kpiBooking.DatNoiDung("Booking hôm nay", tongQuan.BookingHomNay.ToString(),
                $"30 ngày: {tongQuan.BookingTrongKy}");
            kpiChuaThanhToan.DatNoiDung("Hóa đơn chưa thu", tongQuan.HoaDonChuaThanhToan.ToString(),
                $"{tongQuan.TongKhachHang} khách hàng");
            kpiSan.DatNoiDung("Tình trạng sân", $"{tongQuan.SanTrong}/{tongQuan.TongSoSan} trống",
                $"Đang thuê: {tongQuan.SanDangThue}  |  Bảo trì: {tongQuan.SanBaoTri}");

            BieuDo.VeDuong(plotDoanhThu,
                dulieu.doanhThu.Select(d => d.Ngay.ToString("dd/MM")).ToList(),
                dulieu.doanhThu.Select(d => (double)d.DoanhThu).ToList(),
                "Doanh thu", "#0F766E", "Doanh thu 7 ngày gần nhất");

            BieuDo.VeTron(plotSan,
                new List<string> { "Sân trống", "Đang thuê", "Bảo trì" },
                new List<double> { tongQuan.SanTrong, tongQuan.SanDangThue, tongQuan.SanBaoTri },
                "Tình trạng sân");

            BieuDo.VeCot(plotTopSan,
                dulieu.topSan.Select(x => x.TenSan).ToList(),
                dulieu.topSan.Select(x => (double)x.DoanhThu).ToList(),
                "Doanh thu", "#16A34A", "Top sân 30 ngày", "VNĐ");

            Luoi.GanDuLieu(dgvHomNay, dulieu.lichHomNay);
        }
        catch (Exception ex) { BaoLoi("Không thể tải dữ liệu tổng quan", ex); }
        finally { KetThucBan(); }
    }

    private void btnLamMoi_Click(object sender, EventArgs e) => _ = TaiTongQuanAsync();
}
