using System.Data;
using Microsoft.Data.SqlClient;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Data.Repositories;

/// <summary>Các truy vấn tổng hợp (Dashboard + Thống kê) — hỗ trợ cả CSDL v2 với view/procedure.</summary>
public class ThongKeRepository : BaseRepository, IThongKeRepository
{
    public TongQuan LayTongQuan(DateTime tuNgay, DateTime denNgay)
    {
        // Thử dùng stored procedure mới sp_LayTongQuan (CSDL v2)
        try
        {
            DataTable bang = TruyVan("EXEC sp_LayTongQuan @TuNgay, @DenNgay",
                ThamSo("@TuNgay", tuNgay.Date), ThamSo("@DenNgay", denNgay.Date));

            if (bang.Rows.Count > 0)
            {
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
                    TongTienGiam = 0 // sẽ tính riêng nếu cần
                };
            }
        }
        catch { /* fallback */ }

        DataTable bangCu = TruyVan(@"
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

        DataRow dongCu = bangCu.Rows[0];
        return new TongQuan
        {
            TongSoSan = dongCu.SoNguyen("TongSoSan"),
            SanTrong = dongCu.SoNguyen("SanTrong"),
            SanDangThue = dongCu.SoNguyen("SanDangThue"),
            SanBaoTri = dongCu.SoNguyen("SanBaoTri"),
            BookingHomNay = dongCu.SoNguyen("BookingHomNay"),
            BookingTrongKy = dongCu.SoNguyen("BookingTrongKy"),
            HoaDonChuaThanhToan = dongCu.SoNguyen("HoaDonChuaThanhToan"),
            TongKhachHang = dongCu.SoNguyen("TongKhachHang"),
            DoanhThuHomNay = dongCu.SoNguyen("DoanhThuHomNay"),
            DoanhThuTrongKy = dongCu.SoThapPhan("DoanhThuTrongKy"),
            TongTienGiam = dongCu.SoThapPhan("TongTienGiam")
        };
    }

    public List<DoanhThuNgay> DoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
    {
        try
        {
            // Thử dùng view mới v_DoanhThuTheoNgay
            return DanhSach(@"SELECT Ngay, ISNULL(SoHoaDon,0) AS SoBooking,
                          ISNULL(TienGoc,0) AS TienGoc, ISNULL(TienGiam,0) AS TienGiam,
                          ISNULL(DoanhThu,0) AS DoanhThu
                   FROM v_DoanhThuTheoNgay
                   WHERE Ngay BETWEEN @TuNgay AND @DenNgay
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
        }
        catch { }

        return DanhSach(@"SELECT CAST(NgayLap AS DATE) AS Ngay, COUNT(1) AS SoBooking,
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
    }

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

    public List<ThongKeSan> ThongKeTheoSan(DateTime tuNgay, DateTime denNgay, int soLuongTop = 5)
    {
        try
        {
            return DanhSach(@"SELECT TOP (@Top) s.MaSan, s.TenSan, COUNT(ds.MaDat) AS SoLuotThue,
                          ISNULL(SUM(hd.TongTien), 0) AS DoanhThu
                   FROM SAN s
                   LEFT JOIN DAT_SAN ds ON ds.MaSan = s.MaSan AND ds.NgayDat BETWEEN @TuNgay AND @DenNgay AND ds.TrangThai <> 'DaHuy'
                   LEFT JOIN HOA_DON hd ON hd.MaDat = ds.MaDat AND hd.TrangThai = 'DaThanhToan'
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
        catch
        {
            return DanhSach(@"SELECT TOP (@Top) s.MaSan, s.TenSan, COUNT(ds.MaDat) AS SoLuotThue,
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
    }
}
