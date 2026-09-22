using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

/// <summary>Nghiệp vụ khuyến mãi dịp đặc biệt.</summary>
public class KhuyenMaiService
{
    private readonly IKhuyenMaiRepository _khuyenMaiRepo;

    public KhuyenMaiService() : this(new Data.Repositories.KhuyenMaiRepository())
    {
    }

    public KhuyenMaiService(IKhuyenMaiRepository khuyenMaiRepo) => _khuyenMaiRepo = khuyenMaiRepo;

    public List<KhuyenMai> LayTatCa(string tuKhoa = "") => _khuyenMaiRepo.LayTatCa(tuKhoa);

    public KhuyenMai LayTheoMa(int maKM) => _khuyenMaiRepo.LayTheoMa(maKM);

    /// <summary>Chương trình khuyến mãi có phần trăm cao nhất còn hiệu lực tại ngày đặt.</summary>
    public KhuyenMai LayKhuyenMaiTotNhat(DateTime ngayDat)
    {
        bool laCuoiTuan = LaCuoiTuan(ngayDat);
        return _khuyenMaiRepo.LayDangApDung(ngayDat, laCuoiTuan).FirstOrDefault();
    }

    public static bool LaCuoiTuan(DateTime ngay) =>
        ngay.DayOfWeek == DayOfWeek.Saturday || ngay.DayOfWeek == DayOfWeek.Sunday;

    public KetQua<KhuyenMai> Them(KhuyenMai khuyenMai)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.KmThem))
                return KetQua<KhuyenMai>.Loi("Bạn không có quyền tạo khuyến mãi.");

            KetQua hopLe = KiemTraDuLieu(khuyenMai);
            if (!hopLe.ThanhCong) return KetQua<KhuyenMai>.Loi(hopLe.ThongBao);
            if (_khuyenMaiRepo.TonTaiTen(khuyenMai.TenKM.Trim()))
                return KetQua<KhuyenMai>.Loi("Tên chương trình khuyến mãi đã tồn tại.");

            khuyenMai.MaKM = _khuyenMaiRepo.Them(khuyenMai);
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "ThemKhuyenMai", "KHUYEN_MAI", khuyenMai.MaKM.ToString(), $"Thêm KM {khuyenMai.TenKM}"); } catch { }
            return KetQua<KhuyenMai>.Tot(khuyenMai, "Thêm khuyến mãi thành công.");
        }
        catch (Exception ex)
        {
            return KetQua<KhuyenMai>.Loi("Không thể thêm khuyến mãi: " + ex.Message);
        }
    }

    public KetQua CapNhat(KhuyenMai khuyenMai)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.KmSua))
                return KetQua.Loi("Bạn không có quyền sửa khuyến mãi.");

            KetQua hopLe = KiemTraDuLieu(khuyenMai);
            if (!hopLe.ThanhCong) return hopLe;
            if (khuyenMai.MaKM <= 0) return KetQua.Loi("Khuyến mãi không hợp lệ.");
            if (_khuyenMaiRepo.TonTaiTen(khuyenMai.TenKM.Trim(), khuyenMai.MaKM))
                return KetQua.Loi("Tên chương trình khuyến mãi đã tồn tại.");

            _khuyenMaiRepo.CapNhat(khuyenMai);
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "SuaKhuyenMai", "KHUYEN_MAI", khuyenMai.MaKM.ToString(), $"Sửa KM #{khuyenMai.MaKM}"); } catch { }
            return KetQua.Tot("Cập nhật khuyến mãi thành công.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể cập nhật khuyến mãi: " + ex.Message);
        }
    }

    public KetQua Xoa(int maKM)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.KmXoa))
                return KetQua.Loi("Bạn không có quyền xóa khuyến mãi.");

            _khuyenMaiRepo.Xoa(maKM);
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "XoaKhuyenMai", "KHUYEN_MAI", maKM.ToString(), $"Xóa KM #{maKM}"); } catch { }
            return KetQua.Tot("Xóa khuyến mãi thành công.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể xóa khuyến mãi: " + ex.Message);
        }
    }

    private static KetQua KiemTraDuLieu(KhuyenMai khuyenMai)
    {
        if (khuyenMai == null) return KetQua.Loi("Dữ liệu khuyến mãi không hợp lệ.");
        if (string.IsNullOrWhiteSpace(khuyenMai.TenKM)) return KetQua.Loi("Vui lòng nhập tên chương trình.");
        if (khuyenMai.PhanTramGiam <= 0) return KetQua.Loi("Phần trăm giảm phải lớn hơn 0.");
        if (khuyenMai.PhanTramGiam > 100) return KetQua.Loi("Phần trăm giảm không được vượt quá 100%.");
        if (khuyenMai.NgayKetThuc.Date < khuyenMai.NgayBatDau.Date)
            return KetQua.Loi("Ngày kết thúc phải sau ngày bắt đầu.");
        return KetQua.Tot();
    }
}
