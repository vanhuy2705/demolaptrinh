using SportFieldBooking.Core.Entities;

namespace SportFieldBooking.Data.Interfaces;

public interface INhatKyHoatDongRepository
{
    List<NhatKyHoatDong> LayTatCa(string tuKhoa = "", string hoatDong = null, string ketQua = null,
        DateTime? tuNgay = null, DateTime? denNgay = null, int? maTK = null);
    List<NhatKyHoatDong> LayTheoTaiKhoan(int maTK, int soLuong = 50);
    NhatKyHoatDong LayTheoMa(long maNhatKy);
    long Them(NhatKyHoatDong nhatKy);
    int Xoa(long maNhatKy);
    int XoaCu(DateTime truocNgay);
    int DemTong();
}
