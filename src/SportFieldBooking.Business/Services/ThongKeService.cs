using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

/// <summary>Nghiệp vụ thống kê: Admin xem toàn bộ, Nhân viên xem nghiệp vụ, Khách hàng xem lịch sử cá nhân.</summary>
public class ThongKeService
{
    private readonly IThongKeRepository _thongKeRepo;
    private readonly IDatSanRepository _datSanRepo;
    private readonly IHoaDonRepository _hoaDonRepo;
    private readonly ISuDungVoucherRepository _suDungRepo;

    public ThongKeService()
        : this(new Data.Repositories.ThongKeRepository(), new Data.Repositories.DatSanRepository(),
               new Data.Repositories.HoaDonRepository(), new Data.Repositories.SuDungVoucherRepository())
    {
    }

    public ThongKeService(IThongKeRepository thongKeRepo, IDatSanRepository datSanRepo,
        IHoaDonRepository hoaDonRepo, ISuDungVoucherRepository suDungRepo)
    {
        _thongKeRepo = thongKeRepo;
        _datSanRepo = datSanRepo;
        _hoaDonRepo = hoaDonRepo;
        _suDungRepo = suDungRepo;
    }

    private static bool CoQuyenXemHeThong =>
        PhanQuyenService.CoQuyen(MaQuyen.ThongKeToanBo) || PhanQuyenService.CoQuyen(MaQuyen.ThongKeNghiepVu);

    public TongQuan LayTongQuan(DateTime tuNgay, DateTime denNgay)
    {
        if (!CoQuyenXemHeThong) return new TongQuan();
        return _thongKeRepo.LayTongQuan(tuNgay, denNgay);
    }

    public List<DoanhThuNgay> DoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
    {
        if (!CoQuyenXemHeThong) return new List<DoanhThuNgay>();
        return _thongKeRepo.DoanhThuTheoNgay(tuNgay, denNgay);
    }

    public List<DoanhThuNgay> DoanhThuTheoThang(int nam)
    {
        if (!CoQuyenXemHeThong) return new List<DoanhThuNgay>();
        return _thongKeRepo.DoanhThuTheoThang(nam);
    }

    public List<ThongKeGiamGia> ThongKeTheoLoaiGiam(DateTime tuNgay, DateTime denNgay)
    {
        if (!CoQuyenXemHeThong) return new List<ThongKeGiamGia>();
        return _thongKeRepo.ThongKeTheoLoaiGiam(tuNgay, denNgay);
    }

    public List<ThongKeSan> ThongKeTheoSan(DateTime tuNgay, DateTime denNgay, int soLuongTop = 5)
    {
        if (!CoQuyenXemHeThong) return new List<ThongKeSan>();
        return _thongKeRepo.ThongKeTheoSan(tuNgay, denNgay, soLuongTop);
    }

    /// <summary>Thống kê lịch sử của chính khách hàng đang đăng nhập.</summary>
    public ThongKeCaNhan LayThongKeCaNhan(int maKH)
    {
        var ketQua = new ThongKeCaNhan();
        if (maKH <= 0) return ketQua;
        try
        {
            List<DatSan> danhSach = _datSanRepo.LayTheoKhachHang(maKH);
            ketQua.SoLanDat = danhSach.Count;
            ketQua.SoLanHoanThanh = danhSach.Count(d => d.TrangThai == TrangThaiDatSan.HoanThanh);
            ketQua.SoLanHuy = danhSach.Count(d => d.TrangThai == TrangThaiDatSan.DaHuy);
            ketQua.TongChiTieu = _hoaDonRepo.LayTheoKhachHang(maKH)
                .Where(h => h.TrangThai == TrangThaiHoaDon.DaThanhToan)
                .Sum(h => h.TongTien);
            ketQua.SoLanDungVoucher = _suDungRepo.LayTheoKhachHang(maKH).Count;
            ketQua.SanYeuThich = danhSach
                .Where(d => d.TrangThai != TrangThaiDatSan.DaHuy)
                .GroupBy(d => d.TenSan)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "Chưa có";
        }
        catch
        {
            // Thống kê cá nhân chỉ phục vụ hiển thị: lỗi thì trả về số liệu rỗng.
        }
        return ketQua;
    }
}
