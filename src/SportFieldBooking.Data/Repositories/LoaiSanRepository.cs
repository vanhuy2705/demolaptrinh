using System.Data;
using Microsoft.Data.SqlClient;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Data.Repositories;

/// <summary>Repository bảng LOAI_SAN.</summary>
public class LoaiSanRepository : BaseRepository, ILoaiSanRepository
{
    private const string SqlSelect = @"
        SELECT ls.MaLoaiSan, ls.TenLoaiSan, ISNULL(ls.MoTa, '') AS MoTa,
               (SELECT COUNT(1) FROM SAN s WHERE s.MaLoaiSan = ls.MaLoaiSan) AS SoLuongSan
        FROM LOAI_SAN ls";

    private static LoaiSan AnhXa(DataRow dong) => new()
    {
        MaLoaiSan = dong.SoNguyen("MaLoaiSan"),
        TenLoaiSan = dong.Chuoi("TenLoaiSan"),
        MoTa = dong.Chuoi("MoTa"),
        SoLuongSan = dong.SoNguyen("SoLuongSan")
    };

    public List<LoaiSan> LayTatCa(string tuKhoa = "") =>
        DanhSach($@"{SqlSelect}
                    WHERE (@TuKhoa IS NULL OR ls.TenLoaiSan LIKE @TuKhoa)
                    ORDER BY ls.TenLoaiSan",
            AnhXa, ThamSoTimKiem("@TuKhoa", tuKhoa));

    public LoaiSan LayTheoMa(int maLoaiSan) =>
        MotHoacNull($"{SqlSelect} WHERE ls.MaLoaiSan = @Ma", AnhXa, ThamSo("@Ma", maLoaiSan));

    public bool TonTaiTen(string tenLoaiSan, int? maLoaiTru = null)
    {
        object ketQua = GiaTriDon(
            "SELECT COUNT(1) FROM LOAI_SAN WHERE TenLoaiSan = @Ten AND (@MaLoaiTru IS NULL OR MaLoaiSan <> @MaLoaiTru)",
            ThamSo("@Ten", tenLoaiSan), ThamSo("@MaLoaiTru", (object)maLoaiTru ?? DBNull.Value));
        return Convert.ToInt32(ketQua) > 0;
    }

    public int Them(LoaiSan loaiSan) =>
        ThemTraVeMa(@"INSERT INTO LOAI_SAN (TenLoaiSan, MoTa) VALUES (@Ten, @MoTa);
                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
            ThamSo("@Ten", loaiSan.TenLoaiSan), ThamSo("@MoTa", loaiSan.MoTa));

    public int CapNhat(LoaiSan loaiSan) =>
        ThucThi("UPDATE LOAI_SAN SET TenLoaiSan = @Ten, MoTa = @MoTa WHERE MaLoaiSan = @Ma",
            ThamSo("@Ten", loaiSan.TenLoaiSan), ThamSo("@MoTa", loaiSan.MoTa), ThamSo("@Ma", loaiSan.MaLoaiSan));

    public int Xoa(int maLoaiSan) =>
        ThucThi("DELETE FROM LOAI_SAN WHERE MaLoaiSan = @Ma", ThamSo("@Ma", maLoaiSan));

    public int DemSoSanSuDung(int maLoaiSan) =>
        Convert.ToInt32(GiaTriDon("SELECT COUNT(1) FROM SAN WHERE MaLoaiSan = @Ma", ThamSo("@Ma", maLoaiSan)));
}
