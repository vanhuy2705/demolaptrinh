using System.Data;
using Microsoft.Data.SqlClient;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Data.Repositories;

/// <summary>Repository bảng SU_DUNG_VOUCHER (lịch sử dùng voucher).</summary>
public class SuDungVoucherRepository : BaseRepository, ISuDungVoucherRepository
{
    private const string SqlSelect = @"
        SELECT sd.MaSuDung, sd.MaVoucher, sd.MaDat, sd.MaKH, sd.SoTienGiam, sd.NgaySuDung,
               ISNULL(v.MaCode, '') AS MaCode, ISNULL(kh.HoTen, '') AS TenKH
        FROM SU_DUNG_VOUCHER sd
        LEFT JOIN VOUCHER v ON v.MaVoucher = sd.MaVoucher
        LEFT JOIN KHACH_HANG kh ON kh.MaKH = sd.MaKH";

    private static SuDungVoucher AnhXa(DataRow dong) => new()
    {
        MaSuDung = dong.SoNguyen("MaSuDung"),
        MaVoucher = dong.SoNguyen("MaVoucher"),
        MaDat = dong.SoNguyen("MaDat"),
        MaKH = dong.SoNguyen("MaKH"),
        SoTienGiam = dong.SoThapPhan("SoTienGiam"),
        NgaySuDung = dong.NgayGio("NgaySuDung"),
        MaCode = dong.Chuoi("MaCode"),
        TenKH = dong.Chuoi("TenKH")
    };

    public int Them(SuDungVoucher suDung) =>
        ThemTraVeMa(@"INSERT INTO SU_DUNG_VOUCHER (MaVoucher, MaDat, MaKH, SoTienGiam, NgaySuDung)
                      VALUES (@MaVoucher, @MaDat, @MaKH, @SoTienGiam, GETDATE());
                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
            ThamSo("@MaVoucher", suDung.MaVoucher),
            ThamSo("@MaDat", suDung.MaDat),
            ThamSo("@MaKH", suDung.MaKH),
            ThamSo("@SoTienGiam", suDung.SoTienGiam));

    public List<SuDungVoucher> LayTheoKhachHang(int maKH) =>
        DanhSach($"{SqlSelect} WHERE sd.MaKH = @MaKH ORDER BY sd.NgaySuDung DESC",
            AnhXa, ThamSo("@MaKH", maKH));

    public List<SuDungVoucher> LayTheoMaDat(int maDat) =>
        DanhSach($"{SqlSelect} WHERE sd.MaDat = @MaDat", AnhXa, ThamSo("@MaDat", maDat));

    public bool DaDungChoBooking(int maVoucher, int maDat)
    {
        object ketQua = GiaTriDon(
            "SELECT COUNT(1) FROM SU_DUNG_VOUCHER WHERE MaVoucher = @MaVoucher AND MaDat = @MaDat",
            ThamSo("@MaVoucher", maVoucher), ThamSo("@MaDat", maDat));
        return Convert.ToInt32(ketQua) > 0;
    }

    public int XoaTheoMaDat(int maDat) =>
        ThucThi("DELETE FROM SU_DUNG_VOUCHER WHERE MaDat = @MaDat", ThamSo("@MaDat", maDat));
}
