using System.Data;
using Microsoft.Data.SqlClient;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Data.Repositories;

/// <summary>Repository bảng SAN.</summary>
public class SanRepository : BaseRepository, ISanRepository
{
    private const string SqlSelect = @"
        SELECT s.MaSan, s.TenSan, s.MaLoaiSan, s.DonGia, s.TrangThai, ISNULL(s.MoTa, '') AS MoTa,
               ISNULL(ls.TenLoaiSan, '') AS TenLoaiSan
        FROM SAN s LEFT JOIN LOAI_SAN ls ON ls.MaLoaiSan = s.MaLoaiSan";

    private static San AnhXa(DataRow dong) => new()
    {
        MaSan = dong.SoNguyen("MaSan"),
        TenSan = dong.Chuoi("TenSan"),
        MaLoaiSan = dong.SoNguyen("MaLoaiSan"),
        DonGia = dong.SoThapPhan("DonGia"),
        TrangThai = dong.Chuoi("TrangThai"),
        MoTa = dong.Chuoi("MoTa"),
        TenLoaiSan = dong.Chuoi("TenLoaiSan")
    };

    public List<San> LayTatCa(string tuKhoa = "", int? maLoaiSan = null, string trangThai = null) =>
        DanhSach($@"{SqlSelect}
                    WHERE (@TuKhoa IS NULL OR s.TenSan LIKE @TuKhoa OR ls.TenLoaiSan LIKE @TuKhoa)
                      AND (@MaLoai IS NULL OR s.MaLoaiSan = @MaLoai)
                      AND (@TrangThai IS NULL OR s.TrangThai = @TrangThai)
                    ORDER BY s.TenSan",
            AnhXa,
            ThamSoTimKiem("@TuKhoa", tuKhoa),
            ThamSo("@MaLoai", (object)maLoaiSan ?? DBNull.Value),
            ThamSo("@TrangThai", (object)trangThai ?? DBNull.Value));

    public San LayTheoMa(int maSan) =>
        MotHoacNull($"{SqlSelect} WHERE s.MaSan = @Ma", AnhXa, ThamSo("@Ma", maSan));

    public bool TonTaiTen(string tenSan, int? maSanLoaiTru = null)
    {
        object ketQua = GiaTriDon(
            "SELECT COUNT(1) FROM SAN WHERE TenSan = @Ten AND (@MaLoaiTru IS NULL OR MaSan <> @MaLoaiTru)",
            ThamSo("@Ten", tenSan), ThamSo("@MaLoaiTru", (object)maSanLoaiTru ?? DBNull.Value));
        return Convert.ToInt32(ketQua) > 0;
    }

    public int Them(San san) =>
        ThemTraVeMa(@"INSERT INTO SAN (TenSan, MaLoaiSan, DonGia, TrangThai, MoTa)
                      VALUES (@Ten, @MaLoai, @DonGia, @TrangThai, @MoTa);
                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
            ThamSo("@Ten", san.TenSan),
            ThamSo("@MaLoai", san.MaLoaiSan),
            ThamSo("@DonGia", san.DonGia),
            ThamSo("@TrangThai", san.TrangThai),
            ThamSo("@MoTa", san.MoTa));

    public int CapNhat(San san) =>
        ThucThi(@"UPDATE SAN SET TenSan = @Ten, MaLoaiSan = @MaLoai, DonGia = @DonGia,
                  TrangThai = @TrangThai, MoTa = @MoTa WHERE MaSan = @Ma",
            ThamSo("@Ten", san.TenSan),
            ThamSo("@MaLoai", san.MaLoaiSan),
            ThamSo("@DonGia", san.DonGia),
            ThamSo("@TrangThai", san.TrangThai),
            ThamSo("@MoTa", san.MoTa),
            ThamSo("@Ma", san.MaSan));

    public int CapNhatTrangThai(int maSan, string trangThai) =>
        ThucThi("UPDATE SAN SET TrangThai = @TrangThai WHERE MaSan = @Ma",
            ThamSo("@TrangThai", trangThai), ThamSo("@Ma", maSan));

    public int Xoa(int maSan) =>
        ThucThi("DELETE FROM SAN WHERE MaSan = @Ma", ThamSo("@Ma", maSan));

    public int DemDatSan(int maSan) =>
        Convert.ToInt32(GiaTriDon("SELECT COUNT(1) FROM DAT_SAN WHERE MaSan = @Ma", ThamSo("@Ma", maSan)));
}
