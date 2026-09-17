namespace SportFieldBooking.Data.Interfaces;

/// <summary>Truy xuất dữ liệu bảng HOA_DON.</summary>
public interface IHoaDonRepository
{
    List<HoaDon> LayTatCa(string tuKhoa = "");
    List<HoaDon> LayTheoKhoang(DateTime tuNgay, DateTime denNgay, string trangThai = null);
    List<HoaDon> LayTheoKhachHang(int maKH);
    HoaDon LayTheoMa(int maHD);
    HoaDon LayTheoMaDat(int maDat);
    int Them(HoaDon hoaDon);
    int CapNhat(HoaDon hoaDon);
    int CapNhatTrangThai(int maHD, string trangThai);
    int Xoa(int maHD);
    decimal TongDoanhThu(DateTime tuNgay, DateTime denNgay);
}
