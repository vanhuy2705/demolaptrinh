using System.Data;
using Microsoft.Data.SqlClient;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Data.Repositories;

/// <summary>Repository bảng KHUYEN_MAI.</summary>
public class KhuyenMaiRepository : BaseRepository, IKhuyenMaiRepository
{
    private const string Cot = @"MaKM, TenKM, LoaiKhuyenMai, PhanTramGiam, NgayBatDau, NgayKetThuc,
                                 ApDungCuoiTuan, TrangThai, MoTa";

    private static KhuyenMai AnhXa(DataRow dong) => new()
    {
        MaKM = dong.SoNguyen("MaKM"),
        TenKM = dong.Chuoi("TenKM"),
        LoaiKhuyenMai = dong.Chuoi("LoaiKhuyenMai"),
        PhanTramGiam = dong.SoThapPhan("PhanTramGiam"),
        NgayBatDau = dong.NgayGio("NgayBatDau"),
        NgayKetThuc = dong.NgayGio("NgayKetThuc"),
        ApDungCuoiTuan = dong.Bit("ApDungCuoiTuan"),
        TrangThai = dong.Chuoi("TrangThai"),
        MoTa = dong.Chuoi("MoTa")
    };

    public List<KhuyenMai> LayTatCa(string tuKhoa = "") =>
        DanhSach($@"SELECT {Cot} FROM KHUYEN_MAI
                    WHERE (@TuKhoa IS NULL OR TenKM LIKE @TuKhoa OR LoaiKhuyenMai LIKE @TuKhoa)
                    ORDER BY NgayBatDau DESC",
            AnhXa, ThamSoTimKiem("@TuKhoa", tuKhoa));

    public KhuyenMai LayTheoMa(int maKM) =>
        MotHoacNull($"SELECT {Cot} FROM KHUYEN_MAI WHERE MaKM = @Ma", AnhXa, ThamSo("@Ma", maKM));

    public List<KhuyenMai> LayDangApDung(DateTime ngay, bool laCuoiTuan) =>
        DanhSach($@"SELECT {Cot} FROM KHUYEN_MAI
                    WHERE TrangThai = 'HoatDong'
                      AND @Ngay BETWEEN NgayBatDau AND NgayKetThuc
                      AND (ApDungCuoiTuan = 0 OR ApDungCuoiTuan = @LaCuoiTuan)
                    ORDER BY PhanTramGiam DESC",
            AnhXa, ThamSo("@Ngay", ngay.Date), ThamSo("@LaCuoiTuan", laCuoiTuan));

    public bool TonTaiTen(string tenKM, int? maKMLoaiTru = null)
    {
        object ketQua = GiaTriDon(
            "SELECT COUNT(1) FROM KHUYEN_MAI WHERE TenKM = @Ten AND (@MaLoaiTru IS NULL OR MaKM <> @MaLoaiTru)",
            ThamSo("@Ten", tenKM), ThamSo("@MaLoaiTru", (object)maKMLoaiTru ?? DBNull.Value));
        return Convert.ToInt32(ketQua) > 0;
    }

    public int Them(KhuyenMai khuyenMai) =>
        ThemTraVeMa(@"INSERT INTO KHUYEN_MAI (TenKM, LoaiKhuyenMai, PhanTramGiam, NgayBatDau, NgayKetThuc,
                      ApDungCuoiTuan, TrangThai, MoTa)
                      VALUES (@Ten, @Loai, @PhanTram, @NgayBatDau, @NgayKetThuc, @CuoiTuan, @TrangThai, @MoTa);
                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
            ThamSo("@Ten", khuyenMai.TenKM),
            ThamSo("@Loai", khuyenMai.LoaiKhuyenMai),
            ThamSo("@PhanTram", khuyenMai.PhanTramGiam),
            ThamSo("@NgayBatDau", khuyenMai.NgayBatDau.Date),
            ThamSo("@NgayKetThuc", khuyenMai.NgayKetThuc.Date),
            ThamSo("@CuoiTuan", khuyenMai.ApDungCuoiTuan),
            ThamSo("@TrangThai", khuyenMai.TrangThai),
            ThamSo("@MoTa", khuyenMai.MoTa ?? ""));

    public int CapNhat(KhuyenMai khuyenMai) =>
        ThucThi(@"UPDATE KHUYEN_MAI SET TenKM = @Ten, LoaiKhuyenMai = @Loai, PhanTramGiam = @PhanTram,
                  NgayBatDau = @NgayBatDau, NgayKetThuc = @NgayKetThuc, ApDungCuoiTuan = @CuoiTuan,
                  TrangThai = @TrangThai, MoTa = @MoTa
                  WHERE MaKM = @Ma",
            ThamSo("@Ten", khuyenMai.TenKM),
            ThamSo("@Loai", khuyenMai.LoaiKhuyenMai),
            ThamSo("@PhanTram", khuyenMai.PhanTramGiam),
            ThamSo("@NgayBatDau", khuyenMai.NgayBatDau.Date),
            ThamSo("@NgayKetThuc", khuyenMai.NgayKetThuc.Date),
            ThamSo("@CuoiTuan", khuyenMai.ApDungCuoiTuan),
            ThamSo("@TrangThai", khuyenMai.TrangThai),
            ThamSo("@MoTa", khuyenMai.MoTa ?? ""),
            ThamSo("@Ma", khuyenMai.MaKM));

    public int Xoa(int maKM) =>
        ThucThi("DELETE FROM KHUYEN_MAI WHERE MaKM = @Ma", ThamSo("@Ma", maKM));
}
