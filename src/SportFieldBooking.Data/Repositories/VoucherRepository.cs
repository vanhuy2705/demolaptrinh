using System.Data;
using Microsoft.Data.SqlClient;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Data.Repositories;

/// <summary>Repository bảng VOUCHER.</summary>
public class VoucherRepository : BaseRepository, IVoucherRepository
{
    private const string Cot = @"MaVoucher, MaCode, TenVoucher, LoaiGiam, GiaTriGiam, DonToiThieu, SoLuong,
                                 SoLuongDaDung, NgayBatDau, NgayKetThuc, TrangThai, MoTa";

    private static Voucher AnhXa(DataRow dong) => new()
    {
        MaVoucher = dong.SoNguyen("MaVoucher"),
        MaCode = dong.Chuoi("MaCode"),
        TenVoucher = dong.Chuoi("TenVoucher"),
        LoaiGiam = dong.Chuoi("LoaiGiam"),
        GiaTriGiam = dong.SoThapPhan("GiaTriGiam"),
        DonToiThieu = dong.SoThapPhan("DonToiThieu"),
        SoLuong = dong.SoNguyen("SoLuong"),
        SoLuongDaDung = dong.SoNguyen("SoLuongDaDung"),
        NgayBatDau = dong.NgayGio("NgayBatDau"),
        NgayKetThuc = dong.NgayGio("NgayKetThuc"),
        TrangThai = dong.Chuoi("TrangThai"),
        MoTa = dong.Chuoi("MoTa")
    };

    public List<Voucher> LayTatCa(string tuKhoa = "", bool? chiConHan = null) =>
        DanhSach($@"SELECT {Cot} FROM VOUCHER
                    WHERE (@TuKhoa IS NULL OR MaCode LIKE @TuKhoa OR TenVoucher LIKE @TuKhoa)
                      AND (@ChiConHan IS NULL OR (@ChiConHan = 1 AND GETDATE() BETWEEN NgayBatDau AND NgayKetThuc
                           AND SoLuongDaDung < SoLuong AND TrangThai = 'HoatDong')
                        OR (@ChiConHan = 0 AND NOT (GETDATE() BETWEEN NgayBatDau AND NgayKetThuc
                           AND SoLuongDaDung < SoLuong AND TrangThai = 'HoatDong')))
                    ORDER BY NgayKetThuc DESC",
            AnhXa,
            ThamSoTimKiem("@TuKhoa", tuKhoa),
            ThamSo("@ChiConHan", (object)chiConHan ?? DBNull.Value));

    public Voucher LayTheoMa(int maVoucher) =>
        MotHoacNull($"SELECT {Cot} FROM VOUCHER WHERE MaVoucher = @Ma", AnhXa, ThamSo("@Ma", maVoucher));

    public Voucher LayTheoMaCode(string maCode) =>
        MotHoacNull($"SELECT {Cot} FROM VOUCHER WHERE MaCode = @MaCode", AnhXa, ThamSo("@MaCode", maCode));

    public bool TonTaiMaCode(string maCode, int? maVoucherLoaiTru = null)
    {
        object ketQua = GiaTriDon(
            "SELECT COUNT(1) FROM VOUCHER WHERE MaCode = @MaCode AND (@MaLoaiTru IS NULL OR MaVoucher <> @MaLoaiTru)",
            ThamSo("@MaCode", maCode), ThamSo("@MaLoaiTru", (object)maVoucherLoaiTru ?? DBNull.Value));
        return Convert.ToInt32(ketQua) > 0;
    }

    public int Them(Voucher voucher) =>
        ThemTraVeMa(@"INSERT INTO VOUCHER (MaCode, TenVoucher, LoaiGiam, GiaTriGiam, DonToiThieu, SoLuong,
                      SoLuongDaDung, NgayBatDau, NgayKetThuc, TrangThai, MoTa)
                      VALUES (@MaCode, @Ten, @LoaiGiam, @GiaTri, @DonToiThieu, @SoLuong, 0,
                              @NgayBatDau, @NgayKetThuc, @TrangThai, @MoTa);
                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
            ThamSo("@MaCode", voucher.MaCode),
            ThamSo("@Ten", voucher.TenVoucher),
            ThamSo("@LoaiGiam", voucher.LoaiGiam),
            ThamSo("@GiaTri", voucher.GiaTriGiam),
            ThamSo("@DonToiThieu", voucher.DonToiThieu),
            ThamSo("@SoLuong", voucher.SoLuong),
            ThamSo("@NgayBatDau", voucher.NgayBatDau.Date),
            ThamSo("@NgayKetThuc", voucher.NgayKetThuc.Date),
            ThamSo("@TrangThai", voucher.TrangThai),
            ThamSo("@MoTa", voucher.MoTa ?? ""));

    public int CapNhat(Voucher voucher) =>
        ThucThi(@"UPDATE VOUCHER SET MaCode = @MaCode, TenVoucher = @Ten, LoaiGiam = @LoaiGiam,
                  GiaTriGiam = @GiaTri, DonToiThieu = @DonToiThieu, SoLuong = @SoLuong,
                  NgayBatDau = @NgayBatDau, NgayKetThuc = @NgayKetThuc, TrangThai = @TrangThai, MoTa = @MoTa
                  WHERE MaVoucher = @Ma",
            ThamSo("@MaCode", voucher.MaCode),
            ThamSo("@Ten", voucher.TenVoucher),
            ThamSo("@LoaiGiam", voucher.LoaiGiam),
            ThamSo("@GiaTri", voucher.GiaTriGiam),
            ThamSo("@DonToiThieu", voucher.DonToiThieu),
            ThamSo("@SoLuong", voucher.SoLuong),
            ThamSo("@NgayBatDau", voucher.NgayBatDau.Date),
            ThamSo("@NgayKetThuc", voucher.NgayKetThuc.Date),
            ThamSo("@TrangThai", voucher.TrangThai),
            ThamSo("@MoTa", voucher.MoTa ?? ""),
            ThamSo("@Ma", voucher.MaVoucher));

    public int Xoa(int maVoucher) =>
        ThucThi("DELETE FROM VOUCHER WHERE MaVoucher = @Ma", ThamSo("@Ma", maVoucher));

    public int TangSoLuongDaDung(int maVoucher, int soLuong = 1) =>
        ThucThi(@"UPDATE VOUCHER SET SoLuongDaDung = SoLuongDaDung + @SoLuong
                  WHERE MaVoucher = @Ma AND SoLuongDaDung + @SoLuong <= SoLuong",
            ThamSo("@SoLuong", soLuong), ThamSo("@Ma", maVoucher));

    public int GiamSoLuongDaDung(int maVoucher, int soLuong = 1) =>
        ThucThi(@"UPDATE VOUCHER SET SoLuongDaDung = CASE WHEN SoLuongDaDung - @SoLuong < 0 THEN 0 ELSE SoLuongDaDung - @SoLuong END
                  WHERE MaVoucher = @Ma",
            ThamSo("@SoLuong", soLuong), ThamSo("@Ma", maVoucher));
}
