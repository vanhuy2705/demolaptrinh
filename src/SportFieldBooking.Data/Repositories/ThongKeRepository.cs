using System.Data;
using Microsoft.Data.SqlClient;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Data.Repositories;

/// <summary>Các truy vấn tổng hợp (Dashboard + Thống kê).</summary>
public class ThongKeRepository : BaseRepository, IThongKeRepository
{
    public TongQuan LayTongQuan(DateTime tuNgay, DateTime denNgay)
    {
        DataTable bang = TruyVan(@"
            SELECT
              (SELECT COUNT(1) FROM SAN) AS TongSoSan,
              (SELECT COUNT(1) FROM SAN WHERE TrangThai = 'Trong') AS SanTrong,
              (SELECT COUNT(1) FROM SAN WHERE TrangThai = 'DangThue') AS SanDangThue,
              (SELECT COUNT(1) FROM SAN WHERE TrangThai = 'BaoTri') AS SanBaoTri,
              (SELECT COUNT(1) FROM DAT_SAN WHERE NgayDat = CAST(GETDATE() AS DATE) AND TrangThai <> 'DaHuy') AS BookingHomNay,
              (SELECT COUNT(1) FROM DAT_SAN WHERE NgayDat BETWEEN @TuNgay AND @DenNgay AND TrangThai <> 'DaHuy') AS BookingTrongKy,
              (SELECT COUNT(1) FROM HOA_DON WHERE TrangThai = 'ChuaThanhToan') AS HoaDonChuaThanhToan,
              (SELECT COUNT(1) FROM KHACH_HANG) AS TongKhachHang,
              (SELECT ISNULL(SUM(TongTien), 0) FROM HOA_DON WHERE TrangThai = 'DaThanhToan'
                 AND CAST(NgayLap AS DATE) = CAST(GETDATE() AS DATE)) AS DoanhThuHomNay,
              (SELECT ISNULL(SUM(TongTien), 0) FROM HOA_DON WHERE TrangThai = 'DaThanhToan'
                 AND CAST(NgayLap AS DATE) BETWEEN @TuNgay AND @DenNgay) AS DoanhThuTrongKy,
              (SELECT ISNULL(SUM(TienGiam), 0) FROM HOA_DON WHERE TrangThai = 'DaThanhToan'
                 AND CAST(NgayLap AS DATE) BETWEEN @TuNgay AND @DenNgay) AS TongTienGiam",
            ThamSo("@TuNgay", tuNgay.Date), ThamSo("@DenNgay", denNgay.Date));

        DataRow dong = bang.Rows[0];
        return new TongQuan
        {
            TongSoSan = dong.SoNguyen("TongSoSan"),
            SanTrong = dong.SoNguyen("SanTrong"),
            SanDangThue = dong.SoNguyen("SanDangThue"),
            SanBaoTri = dong.SoNguyen("SanBaoTri"),
            BookingHomNay = dong.SoNguyen("BookingHomNay"),
            BookingTrongKy = dong.SoNguyen("BookingTrongKy"),
            HoaDonChuaThanhToan = dong.SoNguyen("HoaDonChuaThanhToan"),
            TongKhachHang = dong.SoNguyen("TongKhachHang"),
            DoanhThuHomNay = dong.SoThapPhan("DoanhThuHomNay"),
            DoanhThuTrongKy = dong.SoThapPhan("DoanhThuTrongKy"),
            TongTienGiam = dong.SoThapPhan("TongTienGiam")
        };
    }

    public List<DoanhThuNgay> DoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay) =>
        DanhSach(@"SELECT CAST(NgayLap AS DATE) AS Ngay, COUNT(1) AS SoBooking,
                          ISNULL(SUM(TienGoc), 0) AS TienGoc, ISNULL(SUM(TienGiam), 0) AS TienGiam,
                          ISNULL(SUM(TongTien), 0) AS DoanhThu
                   FROM HOA_DON
                   WHERE TrangThai = 'DaThanhToan' AND CAST(NgayLap AS DATE) BETWEEN @TuNgay AND @DenNgay
                   GROUP BY CAST(NgayLap AS DATE)
                   ORDER BY Ngay",
            Dong => new DoanhThuNgay
            {
                Ngay = Dong.NgayGio("Ngay"),
                SoBooking = Dong.SoNguyen("SoBooking"),
                TienGoc = Dong.SoThapPhan("TienGoc"),
                TienGiam = Dong.SoThapPhan("TienGiam"),
                DoanhThu = Dong.SoThapPhan("DoanhThu")
            },
            ThamSo("@TuNgay", tuNgay.Date), ThamSo("@DenNgay", denNgay.Date));

    public List<DoanhThuNgay> DoanhThuTheoThang(int nam) =>
        DanhSach(@"SELECT DATEFROMPARTS(YEAR(NgayLap), MONTH(NgayLap), 1) AS Ngay, COUNT(1) AS SoBooking,
                          ISNULL(SUM(TienGoc), 0) AS TienGoc, ISNULL(SUM(TienGiam), 0) AS TienGiam,
                          ISNULL(SUM(TongTien), 0) AS DoanhThu
                   FROM HOA_DON
                   WHERE TrangThai = 'DaThanhToan' AND YEAR(NgayLap) = @Nam
                   GROUP BY YEAR(NgayLap), MONTH(NgayLap)
                   ORDER BY Ngay",
            Dong => new DoanhThuNgay
            {
                Ngay = Dong.NgayGio("Ngay"),
                SoBooking = Dong.SoNguyen("SoBooking"),
                TienGoc = Dong.SoThapPhan("TienGoc"),
                TienGiam = Dong.SoThapPhan("TienGiam"),
                DoanhThu = Dong.SoThapPhan("DoanhThu")
            },
            ThamSo("@Nam", nam));

    public List<ThongKeGiamGia> ThongKeTheoLoaiGiam(DateTime tuNgay, DateTime denNgay) =>
        DanhSach(@"SELECT ISNULL(NULLIF(LoaiGiamGia, ''), 'Khong') AS LoaiGiamGia, COUNT(1) AS SoLuong,
                          ISNULL(SUM(TienGiam), 0) AS TongTienGiam
                   FROM HOA_DON
                   WHERE TrangThai = 'DaThanhToan' AND CAST(NgayLap AS DATE) BETWEEN @TuNgay AND @DenNgay
                   GROUP BY ISNULL(NULLIF(LoaiGiamGia, ''), 'Khong')",
            Dong => new ThongKeGiamGia
            {
                LoaiGiamGia = Dong.Chuoi("LoaiGiamGia"),
                SoLuong = Dong.SoNguyen("SoLuong"),
                TongTienGiam = Dong.SoThapPhan("TongTienGiam")
            },
            ThamSo("@TuNgay", tuNgay.Date), ThamSo("@DenNgay", denNgay.Date));

    public List<ThongKeSan> ThongKeTheoSan(DateTime tuNgay, DateTime denNgay, int soLuongTop = 5) =>
        DanhSach(@"SELECT TOP (@Top) s.MaSan, s.TenSan, COUNT(ds.MaDat) AS SoLuotThue,
                          ISNULL(SUM(hd.TongTien), 0) AS DoanhThu
                   FROM HOA_DON hd
                   JOIN DAT_SAN ds ON ds.MaDat = hd.MaDat
                   JOIN SAN s ON s.MaSan = ds.MaSan
                   WHERE hd.TrangThai = 'DaThanhToan' AND CAST(hd.NgayLap AS DATE) BETWEEN @TuNgay AND @DenNgay
                   GROUP BY s.MaSan, s.TenSan
                   ORDER BY DoanhThu DESC",
            Dong => new ThongKeSan
            {
                MaSan = Dong.SoNguyen("MaSan"),
                TenSan = Dong.Chuoi("TenSan"),
                SoLuotThue = Dong.SoNguyen("SoLuotThue"),
                DoanhThu = Dong.SoThapPhan("DoanhThu")
            },
            ThamSo("@Top", soLuongTop), ThamSo("@TuNgay", tuNgay.Date), ThamSo("@DenNgay", denNgay.Date));
}
