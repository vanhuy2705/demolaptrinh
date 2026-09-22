using System.Data;
using Microsoft.Data.SqlClient;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Data.Repositories;

public class NhatKyHoatDongRepository : BaseRepository, INhatKyHoatDongRepository
{
    private static NhatKyHoatDong AnhXa(DataRow dong) => new()
    {
        MaNhatKy = dong.SoNguyen64("MaNhatKy"),
        MaTK = dong.SoNguyenCoTheNull("MaTK"),
        TenDangNhap = dong.Chuoi("TenDangNhap"),
        VaiTro = dong.Chuoi("VaiTro"),
        HoatDong = dong.Chuoi("HoatDong"),
        BangDuLieu = dong.Chuoi("BangDuLieu"),
        MaDuLieu = dong.Chuoi("MaDuLieu"),
        NoiDung = dong.Chuoi("NoiDung"),
        KetQua = dong.Chuoi("KetQua"),
        ThoiGian = dong.NgayGio("ThoiGian"),
        MayTram = dong.Chuoi("MayTram"),
        HoTen = dong.Chuoi("HoTen")
    };

    public List<NhatKyHoatDong> LayTatCa(string tuKhoa = "", string hoatDong = null, string ketQua = null,
        DateTime? tuNgay = null, DateTime? denNgay = null, int? maTK = null)
    {
        string sql = @"
            SELECT nk.*, ISNULL(tk.HoTen, '') AS HoTen
            FROM NHAT_KY_HOAT_DONG nk
            LEFT JOIN TAIKHOAN tk ON tk.MaTK = nk.MaTK
            WHERE (@TuKhoa IS NULL OR @TuKhoa = '' 
                OR nk.TenDangNhap LIKE @TuKhoa 
                OR nk.NoiDung LIKE @TuKhoa 
                OR nk.HoatDong LIKE @TuKhoa
                OR nk.BangDuLieu LIKE @TuKhoa)
              AND (@HoatDong IS NULL OR nk.HoatDong = @HoatDong)
              AND (@KetQua IS NULL OR nk.KetQua = @KetQua)
              AND (@TuNgay IS NULL OR nk.ThoiGian >= @TuNgay)
              AND (@DenNgay IS NULL OR nk.ThoiGian <= @DenNgay)
              AND (@MaTK IS NULL OR nk.MaTK = @MaTK)
            ORDER BY nk.ThoiGian DESC";

        DateTime? denNgayCuoi = denNgay?.Date.AddDays(1).AddTicks(-1);

        return DanhSach(sql, AnhXa,
            ThamSoTimKiem("@TuKhoa", tuKhoa),
            ThamSo("@HoatDong", (object)hoatDong ?? DBNull.Value),
            ThamSo("@KetQua", (object)ketQua ?? DBNull.Value),
            ThamSo("@TuNgay", (object)tuNgay?.Date ?? DBNull.Value),
            ThamSo("@DenNgay", (object)denNgayCuoi ?? DBNull.Value),
            ThamSo("@MaTK", (object)maTK ?? DBNull.Value));
    }

    public List<NhatKyHoatDong> LayTheoTaiKhoan(int maTK, int soLuong = 50) =>
        DanhSach(@"
            SELECT TOP (@SoLuong) nk.*, ISNULL(tk.HoTen, '') AS HoTen
            FROM NHAT_KY_HOAT_DONG nk
            LEFT JOIN TAIKHOAN tk ON tk.MaTK = nk.MaTK
            WHERE nk.MaTK = @MaTK
            ORDER BY nk.ThoiGian DESC",
            AnhXa,
            ThamSo("@MaTK", maTK),
            ThamSo("@SoLuong", soLuong));

    public NhatKyHoatDong LayTheoMa(long maNhatKy) =>
        MotHoacNull(@"
            SELECT nk.*, ISNULL(tk.HoTen, '') AS HoTen
            FROM NHAT_KY_HOAT_DONG nk
            LEFT JOIN TAIKHOAN tk ON tk.MaTK = nk.MaTK
            WHERE nk.MaNhatKy = @Ma",
            AnhXa, ThamSo("@Ma", maNhatKy));

    public long Them(NhatKyHoatDong nhatKy)
    {
        try
        {
            object ketQua = GiaTriDon(@"
                INSERT INTO NHAT_KY_HOAT_DONG (MaTK, TenDangNhap, VaiTro, HoatDong, BangDuLieu, MaDuLieu, NoiDung, KetQua, ThoiGian, MayTram)
                VALUES (@MaTK, @TenDangNhap, @VaiTro, @HoatDong, @BangDuLieu, @MaDuLieu, @NoiDung, @KetQua, @ThoiGian, @MayTram);
                SELECT CAST(SCOPE_IDENTITY() AS BIGINT);",
                ThamSo("@MaTK", (object)nhatKy.MaTK ?? DBNull.Value),
                ThamSo("@TenDangNhap", nhatKy.TenDangNhap ?? ""),
                ThamSo("@VaiTro", nhatKy.VaiTro ?? ""),
                ThamSo("@HoatDong", nhatKy.HoatDong ?? ""),
                ThamSo("@BangDuLieu", (object)nhatKy.BangDuLieu ?? DBNull.Value),
                ThamSo("@MaDuLieu", (object)nhatKy.MaDuLieu ?? DBNull.Value),
                ThamSo("@NoiDung", (object)nhatKy.NoiDung ?? DBNull.Value),
                ThamSo("@KetQua", nhatKy.KetQua ?? "ThanhCong"),
                ThamSo("@ThoiGian", nhatKy.ThoiGian == default ? DateTime.Now : nhatKy.ThoiGian),
                ThamSo("@MayTram", (object)nhatKy.MayTram ?? DBNull.Value));

            return ketQua == null || ketQua == DBNull.Value ? 0 : Convert.ToInt64(ketQua);
        }
        catch
        {
            // Nếu bảng chưa tồn tại (CSDL cũ chưa nâng cấp v2) thì bỏ qua, không làm hỏng nghiệp vụ chính
            return 0;
        }
    }

    public int Xoa(long maNhatKy) =>
        ThucThi("DELETE FROM NHAT_KY_HOAT_DONG WHERE MaNhatKy = @Ma", ThamSo("@Ma", maNhatKy));

    public int XoaCu(DateTime truocNgay) =>
        ThucThi("DELETE FROM NHAT_KY_HOAT_DONG WHERE ThoiGian < @Ngay", ThamSo("@Ngay", truocNgay));

    public int DemTong()
    {
        try
        {
            object kq = GiaTriDon("SELECT COUNT(1) FROM NHAT_KY_HOAT_DONG");
            return kq == null || kq == DBNull.Value ? 0 : Convert.ToInt32(kq);
        }
        catch { return 0; }
    }
}
