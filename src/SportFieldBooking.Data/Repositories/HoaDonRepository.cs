using System.Data;
using Microsoft.Data.SqlClient;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Data.Repositories;

/// <summary>Repository bảng HOA_DON.</summary>
public class HoaDonRepository : BaseRepository, IHoaDonRepository
{
    private const string SqlSelect = @"
        SELECT hd.MaHD, hd.MaDat, hd.NgayLap, hd.TienGoc, hd.LoaiGiamGia, hd.TienGiam, hd.TongTien,
               hd.PhuongThucThanhToan, hd.TrangThai, hd.MaNguoiLap, hd.MaVoucher, ISNULL(hd.GhiChu, '') AS GhiChu,
               ds.NgayDat, ds.GioBatDau, ds.GioKetThuc,
               ISNULL(kh.HoTen, '') AS TenKH, ISNULL(kh.SDT, '') AS SDT, ISNULL(s.TenSan, '') AS TenSan,
               ISNULL(v.MaCode, '') AS MaCode
        FROM HOA_DON hd
        LEFT JOIN DAT_SAN ds ON ds.MaDat = hd.MaDat
        LEFT JOIN KHACH_HANG kh ON kh.MaKH = ds.MaKH
        LEFT JOIN SAN s ON s.MaSan = ds.MaSan
        OUTER APPLY (SELECT TOP (1) v.MaCode FROM SU_DUNG_VOUCHER sd JOIN VOUCHER v ON v.MaVoucher = sd.MaVoucher
                     WHERE sd.MaDat = hd.MaDat) v";

    private static HoaDon AnhXa(DataRow dong) => new()
    {
        MaHD = dong.SoNguyen("MaHD"),
        MaDat = dong.SoNguyen("MaDat"),
        NgayLap = dong.NgayGio("NgayLap"),
        TienGoc = dong.SoThapPhan("TienGoc"),
        LoaiGiamGia = dong.Chuoi("LoaiGiamGia"),
        TienGiam = dong.SoThapPhan("TienGiam"),
        TongTien = dong.SoThapPhan("TongTien"),
        PhuongThucThanhToan = dong.Chuoi("PhuongThucThanhToan"),
        TrangThai = dong.Chuoi("TrangThai"),
        MaNguoiLap = dong.SoNguyenCoTheNull("MaNguoiLap"),
        MaVoucher = dong.SoNguyenCoTheNull("MaVoucher"),
        GhiChu = dong.Chuoi("GhiChu"),
        NgayDat = dong.NgayGio("NgayDat"),
        GioBatDau = dong.Gio("GioBatDau"),
        GioKetThuc = dong.Gio("GioKetThuc"),
        TenKH = dong.Chuoi("TenKH"),
        SDT = dong.Chuoi("SDT"),
        TenSan = dong.Chuoi("TenSan"),
        MaCode = dong.Chuoi("MaCode")
    };

    public List<HoaDon> LayTatCa(string tuKhoa = "") =>
        DanhSach($@"{SqlSelect}
                    WHERE (@TuKhoa IS NULL OR kh.HoTen LIKE @TuKhoa OR kh.SDT LIKE @TuKhoa OR s.TenSan LIKE @TuKhoa)
                    ORDER BY hd.NgayLap DESC",
            AnhXa, ThamSoTimKiem("@TuKhoa", tuKhoa));

    public List<HoaDon> LayTheoKhoang(DateTime tuNgay, DateTime denNgay, string trangThai = null) =>
        DanhSach($@"{SqlSelect}
                    WHERE CAST(hd.NgayLap AS DATE) BETWEEN @TuNgay AND @DenNgay
                      AND (@TrangThai IS NULL OR hd.TrangThai = @TrangThai)
                    ORDER BY hd.NgayLap DESC",
            AnhXa,
            ThamSo("@TuNgay", tuNgay.Date),
            ThamSo("@DenNgay", denNgay.Date),
            ThamSo("@TrangThai", (object)trangThai ?? DBNull.Value));

    public List<HoaDon> LayTheoKhachHang(int maKH) =>
        DanhSach($@"{SqlSelect} WHERE ds.MaKH = @MaKH ORDER BY hd.NgayLap DESC",
            AnhXa, ThamSo("@MaKH", maKH));

    public HoaDon LayTheoMa(int maHD) =>
        MotHoacNull($"{SqlSelect} WHERE hd.MaHD = @Ma", AnhXa, ThamSo("@Ma", maHD));

    public HoaDon LayTheoMaDat(int maDat) =>
        MotHoacNull($"{SqlSelect} WHERE hd.MaDat = @Ma", AnhXa, ThamSo("@Ma", maDat));

    public int Them(HoaDon hoaDon) =>
        ThemTraVeMa(@"INSERT INTO HOA_DON (MaDat, NgayLap, TienGoc, LoaiGiamGia, TienGiam, TongTien,
                      PhuongThucThanhToan, TrangThai, MaNguoiLap, MaVoucher, GhiChu)
                      VALUES (@MaDat, GETDATE(), @TienGoc, @LoaiGiamGia, @TienGiam, @TongTien,
                              @PhuongThuc, @TrangThai, @MaNguoiLap, @MaVoucher, @GhiChu);
                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
            ThamSo("@MaDat", hoaDon.MaDat),
            ThamSo("@TienGoc", hoaDon.TienGoc),
            ThamSo("@LoaiGiamGia", hoaDon.LoaiGiamGia),
            ThamSo("@TienGiam", hoaDon.TienGiam),
            ThamSo("@TongTien", hoaDon.TongTien),
            ThamSo("@PhuongThuc", hoaDon.PhuongThucThanhToan),
            ThamSo("@TrangThai", hoaDon.TrangThai),
            ThamSo("@MaNguoiLap", (object)hoaDon.MaNguoiLap ?? DBNull.Value),
            ThamSo("@MaVoucher", (object)hoaDon.MaVoucher ?? DBNull.Value),
            ThamSo("@GhiChu", hoaDon.GhiChu ?? ""));

    public int CapNhat(HoaDon hoaDon) =>
        ThucThi(@"UPDATE HOA_DON SET TienGoc = @TienGoc, LoaiGiamGia = @LoaiGiamGia, TienGiam = @TienGiam,
                  TongTien = @TongTien, PhuongThucThanhToan = @PhuongThuc, TrangThai = @TrangThai,
                  MaNguoiLap = @MaNguoiLap, MaVoucher = @MaVoucher, GhiChu = @GhiChu
                  WHERE MaHD = @Ma",
            ThamSo("@TienGoc", hoaDon.TienGoc),
            ThamSo("@LoaiGiamGia", hoaDon.LoaiGiamGia),
            ThamSo("@TienGiam", hoaDon.TienGiam),
            ThamSo("@TongTien", hoaDon.TongTien),
            ThamSo("@PhuongThuc", hoaDon.PhuongThucThanhToan),
            ThamSo("@TrangThai", hoaDon.TrangThai),
            ThamSo("@MaNguoiLap", (object)hoaDon.MaNguoiLap ?? DBNull.Value),
            ThamSo("@MaVoucher", (object)hoaDon.MaVoucher ?? DBNull.Value),
            ThamSo("@GhiChu", hoaDon.GhiChu ?? ""),
            ThamSo("@Ma", hoaDon.MaHD));

    public int CapNhatTrangThai(int maHD, string trangThai) =>
        ThucThi("UPDATE HOA_DON SET TrangThai = @TrangThai WHERE MaHD = @Ma",
            ThamSo("@TrangThai", trangThai), ThamSo("@Ma", maHD));

    public int Xoa(int maHD) =>
        ThucThi("DELETE FROM HOA_DON WHERE MaHD = @Ma", ThamSo("@Ma", maHD));

    public decimal TongDoanhThu(DateTime tuNgay, DateTime denNgay) =>
        Convert.ToDecimal(GiaTriDon(
            @"SELECT ISNULL(SUM(TongTien), 0) FROM HOA_DON
              WHERE TrangThai = 'DaThanhToan' AND CAST(NgayLap AS DATE) BETWEEN @TuNgay AND @DenNgay",
            ThamSo("@TuNgay", tuNgay.Date), ThamSo("@DenNgay", denNgay.Date)));
}
