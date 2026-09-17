namespace SportFieldBooking.Data.Interfaces;

/// <summary>Truy xuất dữ liệu bảng KHACH_HANG.</summary>
public interface IKhachHangRepository
{
    List<KhachHang> LayTatCa(string tuKhoa = "");
    KhachHang LayTheoMa(int maKH);
    KhachHang LayTheoSDT(string sdt);
    KhachHang LayTheoMaTK(int maTK);
    bool TonTaiSDT(string sdt, int? maKHLoaiTru = null);
    int Them(KhachHang khachHang);
    int CapNhat(KhachHang khachHang);
    int Xoa(int maKH);
    int GanTaiKhoan(int maKH, int maTK);
}
