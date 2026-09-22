using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

public class NhatKyService
{
    private readonly INhatKyHoatDongRepository _repo;

    public NhatKyService() : this(new Data.Repositories.NhatKyHoatDongRepository()) { }

    public NhatKyService(INhatKyHoatDongRepository repo) => _repo = repo;

    public List<NhatKyHoatDong> LayTatCa(string tuKhoa = "", string hoatDong = null, string ketQua = null,
        DateTime? tuNgay = null, DateTime? denNgay = null, int? maTK = null) =>
        _repo.LayTatCa(tuKhoa, hoatDong, ketQua, tuNgay, denNgay, maTK);

    public List<NhatKyHoatDong> LayTheoTaiKhoan(int maTK, int soLuong = 50) =>
        _repo.LayTheoTaiKhoan(maTK, soLuong);

    public NhatKyHoatDong LayTheoMa(long ma) => _repo.LayTheoMa(ma);

    public int DemTong() => _repo.DemTong();

    public void Ghi(int? maTK, string hoatDong, string bangDuLieu = null, string maDuLieu = null,
        string noiDung = null, string ketQua = KetQuaNhatKy.ThanhCong, string mayTram = null)
    {
        try
        {
            var tk = maTK ?? PhienLamViec.MaTK;
            var entry = new NhatKyHoatDong
            {
                MaTK = tk > 0 ? tk : null,
                TenDangNhap = tk > 0 ? PhienLamViec.TenDangNhap : "HeThong",
                VaiTro = PhienLamViec.VaiTro ?? "",
                HoatDong = hoatDong ?? "",
                BangDuLieu = bangDuLieu ?? "",
                MaDuLieu = maDuLieu ?? "",
                NoiDung = noiDung ?? "",
                KetQua = ketQua ?? KetQuaNhatKy.ThanhCong,
                ThoiGian = DateTime.Now,
                MayTram = mayTram ?? Environment.MachineName
            };
            _repo.Them(entry);
        }
        catch
        {
            // Không để lỗi ghi log làm hỏng nghiệp vụ chính
        }
    }

    public KetQua Xoa(long maNhatKy)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.NhatKyXoa))
                return KetQua.Loi("Bạn không có quyền xóa nhật ký.");

            _repo.Xoa(maNhatKy);
            return KetQua.Tot("Đã xóa bản ghi nhật ký.");
        }
        catch (Exception ex) { return KetQua.Loi("Không thể xóa: " + ex.Message); }
    }

    public KetQua XoaCu(DateTime truocNgay)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.NhatKyXoa))
                return KetQua.Loi("Bạn không có quyền xóa nhật ký.");

            int soLuong = _repo.XoaCu(truocNgay);
            return KetQua.Tot($"Đã xóa {soLuong} bản ghi cũ.");
        }
        catch (Exception ex) { return KetQua.Loi("Không thể xóa: " + ex.Message); }
    }
}
