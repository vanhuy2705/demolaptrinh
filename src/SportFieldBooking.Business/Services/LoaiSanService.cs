using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

/// <summary>Nghiệp vụ quản lý loại sân.</summary>
public class LoaiSanService
{
    private readonly ILoaiSanRepository _loaiSanRepo;

    public LoaiSanService() : this(new Data.Repositories.LoaiSanRepository())
    {
    }

    public LoaiSanService(ILoaiSanRepository loaiSanRepo) => _loaiSanRepo = loaiSanRepo;

    public List<LoaiSan> LayTatCa(string tuKhoa = "") => _loaiSanRepo.LayTatCa(tuKhoa);

    public LoaiSan LayTheoMa(int maLoaiSan) => _loaiSanRepo.LayTheoMa(maLoaiSan);

    public KetQua<LoaiSan> Them(LoaiSan loaiSan)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.LoaiSanThem))
                return KetQua<LoaiSan>.Loi("Bạn không có quyền thêm loại sân.");
            if (loaiSan == null || string.IsNullOrWhiteSpace(loaiSan.TenLoaiSan))
                return KetQua<LoaiSan>.Loi("Vui lòng nhập tên loại sân.");
            if (_loaiSanRepo.TonTaiTen(loaiSan.TenLoaiSan.Trim()))
                return KetQua<LoaiSan>.Loi("Tên loại sân đã tồn tại.");

            loaiSan.TenLoaiSan = loaiSan.TenLoaiSan.Trim();
            loaiSan.MaLoaiSan = _loaiSanRepo.Them(loaiSan);
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "ThemLoaiSan", "LOAI_SAN", loaiSan.MaLoaiSan.ToString(), $"Thêm loại sân {loaiSan.TenLoaiSan}"); } catch { }
            return KetQua<LoaiSan>.Tot(loaiSan, "Thêm loại sân thành công.");
        }
        catch (Exception ex)
        {
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "ThemLoaiSan", "LOAI_SAN", null, $"Lỗi thêm loại sân: {ex.Message}", KetQuaNhatKy.ThatBai); } catch { }
            return KetQua<LoaiSan>.Loi("Không thể thêm loại sân: " + ex.Message);
        }
    }

    public KetQua CapNhat(LoaiSan loaiSan)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.LoaiSanSua))
                return KetQua.Loi("Bạn không có quyền sửa loại sân.");
            if (loaiSan == null || loaiSan.MaLoaiSan <= 0) return KetQua.Loi("Loại sân không hợp lệ.");
            if (string.IsNullOrWhiteSpace(loaiSan.TenLoaiSan)) return KetQua.Loi("Vui lòng nhập tên loại sân.");
            if (_loaiSanRepo.TonTaiTen(loaiSan.TenLoaiSan.Trim(), loaiSan.MaLoaiSan))
                return KetQua.Loi("Tên loại sân đã tồn tại.");

            _loaiSanRepo.CapNhat(loaiSan);
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "SuaLoaiSan", "LOAI_SAN", loaiSan.MaLoaiSan.ToString(), $"Sửa loại sân #{loaiSan.MaLoaiSan}"); } catch { }
            return KetQua.Tot("Cập nhật loại sân thành công.");
        }
        catch (Exception ex)
        {
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "SuaLoaiSan", "LOAI_SAN", loaiSan?.MaLoaiSan.ToString(), $"Lỗi sửa: {ex.Message}", KetQuaNhatKy.ThatBai); } catch { }
            return KetQua.Loi("Không thể cập nhật loại sân: " + ex.Message);
        }
    }

    public KetQua Xoa(int maLoaiSan)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.LoaiSanXoa))
                return KetQua.Loi("Bạn không có quyền xóa loại sân.");

            int soSan = _loaiSanRepo.DemSoSanSuDung(maLoaiSan);
            if (soSan > 0)
                return KetQua.Loi($"Không thể xóa: đang có {soSan} sân thuộc loại này.");

            _loaiSanRepo.Xoa(maLoaiSan);
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "XoaLoaiSan", "LOAI_SAN", maLoaiSan.ToString(), $"Xóa loại sân #{maLoaiSan}"); } catch { }
            return KetQua.Tot("Xóa loại sân thành công.");
        }
        catch (Exception ex)
        {
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "XoaLoaiSan", "LOAI_SAN", maLoaiSan.ToString(), $"Lỗi xóa: {ex.Message}", KetQuaNhatKy.ThatBai); } catch { }
            return KetQua.Loi("Không thể xóa loại sân: " + ex.Message);
        }
    }
}
