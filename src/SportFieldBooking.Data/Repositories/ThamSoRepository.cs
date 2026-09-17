using System.Data;
using Microsoft.Data.SqlClient;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Data.Repositories;

/// <summary>Repository bảng THAM_SO (cấu hình hệ thống).</summary>
public class ThamSoRepository : BaseRepository, IThamSoRepository
{
    private static ThamSo AnhXa(DataRow dong) => new()
    {
        TenThamSo = dong.Chuoi("TenThamSo"),
        GiaTri = dong.Chuoi("GiaTri"),
        MoTa = dong.Chuoi("MoTa")
    };

    public List<ThamSo> LayTatCa() =>
        DanhSach("SELECT TenThamSo, GiaTri, ISNULL(MoTa, '') AS MoTa FROM THAM_SO ORDER BY TenThamSo", AnhXa);

    public ThamSo LayTheoTen(string tenThamSo) =>
        MotHoacNull("SELECT TenThamSo, GiaTri, ISNULL(MoTa, '') AS MoTa FROM THAM_SO WHERE TenThamSo = @Ten",
            AnhXa, ThamSo("@Ten", tenThamSo));

    public string GiaTri(string tenThamSo, string macDinh = "")
    {
        object ketQua = GiaTriDon("SELECT GiaTri FROM THAM_SO WHERE TenThamSo = @Ten", ThamSo("@Ten", tenThamSo));
        return ketQua == null || ketQua == DBNull.Value ? macDinh : Convert.ToString(ketQua);
    }

    public int CapNhat(string tenThamSo, string giaTri) =>
        ThucThi("UPDATE THAM_SO SET GiaTri = @GiaTri WHERE TenThamSo = @Ten",
            ThamSo("@GiaTri", giaTri ?? ""), ThamSo("@Ten", tenThamSo));

    public int Them(ThamSo thamSo) =>
        ThucThi(@"INSERT INTO THAM_SO (TenThamSo, GiaTri, MoTa) VALUES (@Ten, @GiaTri, @MoTa)",
            ThamSo("@Ten", thamSo.TenThamSo), ThamSo("@GiaTri", thamSo.GiaTri ?? ""), ThamSo("@MoTa", thamSo.MoTa ?? ""));
}
