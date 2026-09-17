using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

/// <summary>Nghiệp vụ quản lý sân và trạng thái sân.</summary>
public class SanService
{
    private readonly ISanRepository _sanRepo;
    private readonly IDatSanRepository _datSanRepo;

    public SanService() : this(new Data.Repositories.SanRepository(), new Data.Repositories.DatSanRepository())
    {
    }

    public SanService(ISanRepository sanRepo, IDatSanRepository datSanRepo)
    {
        _sanRepo = sanRepo;
        _datSanRepo = datSanRepo;
    }

    public List<San> LayTatCa(string tuKhoa = "", int? maLoaiSan = null, string trangThai = null) =>
        _sanRepo.LayTatCa(tuKhoa, maLoaiSan, trangThai);

    public San LayTheoMa(int maSan) => _sanRepo.LayTheoMa(maSan);

    public KetQua<San> Them(San san)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.SanThem))
                return KetQua<San>.Loi("Bạn không có quyền thêm sân.");

            KetQua hopLe = KiemTraDuLieu(san);
            if (!hopLe.ThanhCong) return KetQua<San>.Loi(hopLe.ThongBao);
            if (_sanRepo.TonTaiTen(san.TenSan.Trim()))
                return KetQua<San>.Loi("Tên sân đã tồn tại.");

            san.TenSan = san.TenSan.Trim();
            san.MaSan = _sanRepo.Them(san);
            return KetQua<San>.Tot(san, "Thêm sân thành công.");
        }
        catch (Exception ex)
        {
            return KetQua<San>.Loi("Không thể thêm sân: " + ex.Message);
        }
    }

    public KetQua CapNhat(San san)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.SanSua))
                return KetQua.Loi("Bạn không có quyền sửa sân.");

            KetQua hopLe = KiemTraDuLieu(san);
            if (!hopLe.ThanhCong) return hopLe;
            if (san.MaSan <= 0) return KetQua.Loi("Sân không hợp lệ.");
            if (_sanRepo.TonTaiTen(san.TenSan.Trim(), san.MaSan))
                return KetQua.Loi("Tên sân đã tồn tại.");

            _sanRepo.CapNhat(san);
            return KetQua.Tot("Cập nhật sân thành công.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể cập nhật sân: " + ex.Message);
        }
    }

    /// <summary>Đổi trạng thái nghiệp vụ: Trống / Đang thuê / Bảo trì.</summary>
    public KetQua DoiTrangThai(int maSan, string trangThai)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.SanDoiTrangThai))
                return KetQua.Loi("Bạn không có quyền đổi trạng thái sân.");
            if (!TrangThaiSan.TatCa.Contains(trangThai))
                return KetQua.Loi("Trạng thái sân không hợp lệ.");

            San san = _sanRepo.LayTheoMa(maSan);
            if (san == null) return KetQua.Loi("Sân không tồn tại.");

            _sanRepo.CapNhatTrangThai(maSan, trangThai);
            return KetQua.Tot($"Sân \"{san.TenSan}\" đã chuyển sang trạng thái {TrangThaiSan.TenHienThi(trangThai)}.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể đổi trạng thái sân: " + ex.Message);
        }
    }

    public KetQua Xoa(int maSan)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.SanXoa))
                return KetQua.Loi("Bạn không có quyền xóa sân.");

            int soDat = _sanRepo.DemDatSan(maSan);
            if (soDat > 0)
                return KetQua.Loi($"Không thể xóa: sân đã có {soDat} lượt đặt trong lịch sử.");

            _sanRepo.Xoa(maSan);
            return KetQua.Tot("Xóa sân thành công.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể xóa sân: " + ex.Message);
        }
    }

    /// <summary>Sân đang được thuê tại thời điểm hiện tại (có booking Đang sử dụng trong khung giờ).</summary>
    public List<San> LaySanDangThueThucTe(DateTime ngay, TimeSpan gioHienTai) =>
        _datSanRepo.LayTheoNgay(ngay)
            .Where(d => d.TrangThai == TrangThaiDatSan.DangSuDung
                        && d.GioBatDau <= gioHienTai && d.GioKetThuc > gioHienTai)
            .Select(d => new San { MaSan = d.MaSan, TenSan = d.TenSan, TrangThai = TrangThaiSan.DangThue })
            .ToList();

    private static KetQua KiemTraDuLieu(San san)
    {
        if (san == null) return KetQua.Loi("Dữ liệu sân không hợp lệ.");
        if (string.IsNullOrWhiteSpace(san.TenSan)) return KetQua.Loi("Vui lòng nhập tên sân.");
        if (san.MaLoaiSan <= 0) return KetQua.Loi("Vui lòng chọn loại sân.");
        if (san.DonGia <= 0) return KetQua.Loi("Đơn giá phải lớn hơn 0.");
        if (san.DonGia > 100_000_000) return KetQua.Loi("Đơn giá không hợp lệ (quá lớn).");
        return KetQua.Tot();
    }
}
