namespace SportFieldBooking.Data.Interfaces;

/// <summary>Các truy vấn tổng hợp phục vụ Dashboard và màn hình thống kê.</summary>
public interface IThongKeRepository
{
    TongQuan LayTongQuan(DateTime tuNgay, DateTime denNgay);
    List<DoanhThuNgay> DoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay);
    List<DoanhThuNgay> DoanhThuTheoThang(int nam);
    List<ThongKeGiamGia> ThongKeTheoLoaiGiam(DateTime tuNgay, DateTime denNgay);
    List<ThongKeSan> ThongKeTheoSan(DateTime tuNgay, DateTime denNgay, int soLuongTop = 5);
}
