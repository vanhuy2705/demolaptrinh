using System.Globalization;
using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

/// <summary>Nghiệp vụ tham số cấu hình hệ thống (bảng THAM_SO).</summary>
public class CauHinhService
{
    private readonly IThamSoRepository _thamSoRepo;

    public CauHinhService() : this(new Data.Repositories.ThamSoRepository())
    {
    }

    public CauHinhService(IThamSoRepository thamSoRepo) => _thamSoRepo = thamSoRepo;

    public List<ThamSo> LayTatCa() => _thamSoRepo.LayTatCa();

    public string LayGiaTri(string tenThamSo, string macDinh = "") => _thamSoRepo.GiaTri(tenThamSo, macDinh);

    /// <summary>Mức giảm cuối tuần (%).</summary>
    public decimal LayPhanTramGiamCuoiTuan()
    {
        string giaTri = _thamSoRepo.GiaTri(ThamSoKeys.PhanTramGiamCuoiTuan, "10");
        return decimal.TryParse(giaTri, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal phanTram)
            ? Math.Max(0, Math.Min(100, phanTram))
            : 10m;
    }

    /// <summary>Số phút của một block tính tiền (mặc định 30 phút).</summary>
    public int LayThoiLuongBlockPhut()
    {
        string giaTri = _thamSoRepo.GiaTri(ThamSoKeys.ThoiLuongBlockPhut, "30");
        return int.TryParse(giaTri, out int soPhut) && soPhut > 0 ? soPhut : 30;
    }

    public TimeSpan LayGioMoCua() => LayGio(ThamSoKeys.GioMoCua, new TimeSpan(5, 0, 0));

    public TimeSpan LayGioDongCua() => LayGio(ThamSoKeys.GioDongCua, new TimeSpan(23, 0, 0));

    public KetQua Luu(string tenThamSo, string giaTri)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.CauHinhSua))
                return KetQua.Loi("Bạn không có quyền thay đổi cấu hình hệ thống.");

            KetQua hopLe = KiemTraGiaTri(tenThamSo, giaTri);
            if (!hopLe.ThanhCong) return hopLe;

            int ketQua = _thamSoRepo.CapNhat(tenThamSo, giaTri);
            if (ketQua == 0)
                _thamSoRepo.Them(new ThamSo { TenThamSo = tenThamSo, GiaTri = giaTri });

            return KetQua.Tot("Lưu cấu hình thành công.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể lưu cấu hình: " + ex.Message);
        }
    }

    public KetQua LuuNhieu(IEnumerable<KeyValuePair<string, string>> danhSach)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.CauHinhSua))
                return KetQua.Loi("Bạn không có quyền thay đổi cấu hình hệ thống.");

            Data.Helpers.DbHelper.ChayGiaoDich(() =>
            {
                foreach (var cap in danhSach)
                {
                    KetQua hopLe = KiemTraGiaTri(cap.Key, cap.Value);
                    if (!hopLe.ThanhCong) throw new InvalidOperationException(hopLe.ThongBao);

                    if (_thamSoRepo.CapNhat(cap.Key, cap.Value) == 0)
                        _thamSoRepo.Them(new ThamSo { TenThamSo = cap.Key, GiaTri = cap.Value });
                }
            });

            return KetQua.Tot("Lưu toàn bộ cấu hình thành công.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể lưu cấu hình: " + ex.Message);
        }
    }

    private static KetQua KiemTraGiaTri(string tenThamSo, string giaTri)
    {
        if (string.IsNullOrWhiteSpace(tenThamSo)) return KetQua.Loi("Tên tham số không hợp lệ.");

        if (tenThamSo == ThamSoKeys.PhanTramGiamCuoiTuan)
        {
            if (!decimal.TryParse(giaTri, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal phanTram))
                return KetQua.Loi("Mức giảm cuối tuần phải là số.");
            if (phanTram < 0 || phanTram > 100)
                return KetQua.Loi("Mức giảm cuối tuần phải nằm trong khoảng 0-100%.");
        }

        if (tenThamSo == ThamSoKeys.ThoiLuongBlockPhut)
        {
            if (!int.TryParse(giaTri, out int soPhut) || soPhut <= 0 || soPhut > 480)
                return KetQua.Loi("Thời lượng block phải là số phút trong khoảng 1-480.");
        }

        if (tenThamSo == ThamSoKeys.GioMoCua || tenThamSo == ThamSoKeys.GioDongCua)
        {
            if (!TimeSpan.TryParse(giaTri, out _))
                return KetQua.Loi("Giờ mở/đóng cửa không đúng định dạng (vd: 05:00).");
        }

        return KetQua.Tot();
    }

    private TimeSpan LayGio(string tenThamSo, TimeSpan macDinh) =>
        TimeSpan.TryParse(_thamSoRepo.GiaTri(tenThamSo, macDinh.ToString()), out TimeSpan gio) ? gio : macDinh;
}
