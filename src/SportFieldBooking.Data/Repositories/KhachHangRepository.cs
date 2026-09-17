using System.Data;
using Microsoft.Data.SqlClient;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Data.Repositories;

/// <summary>Repository bảng KHACH_HANG.</summary>
public class KhachHangRepository : BaseRepository, IKhachHangRepository
{
    private const string SqlSelect = @"
        SELECT kh.MaKH, kh.HoTen, kh.SDT, kh.Email, kh.DiaChi, kh.MaTK, kh.NgayTao,
               ISNULL(tk.TenDangNhap, '') AS TenDangNhap
        FROM KHACH_HANG kh LEFT JOIN TAIKHOAN tk ON tk.MaTK = kh.MaTK";

    private static KhachHang AnhXa(DataRow dong) => new()
    {
        MaKH = dong.SoNguyen("MaKH"),
        HoTen = dong.Chuoi("HoTen"),
        SDT = dong.Chuoi("SDT"),
        Email = dong.Chuoi("Email"),
        DiaChi = dong.Chuoi("DiaChi"),
        MaTK = dong.SoNguyenCoTheNull("MaTK"),
        NgayTao = dong.NgayGio("NgayTao"),
        TenDangNhap = dong.Chuoi("TenDangNhap")
    };

    public List<KhachHang> LayTatCa(string tuKhoa = "") =>
        DanhSach($@"{SqlSelect}
                    WHERE (@TuKhoa IS NULL OR kh.HoTen LIKE @TuKhoa OR kh.SDT LIKE @TuKhoa OR kh.Email LIKE @TuKhoa)
                    ORDER BY kh.HoTen",
            AnhXa, ThamSoTimKiem("@TuKhoa", tuKhoa));

    public KhachHang LayTheoMa(int maKH) =>
        MotHoacNull($"{SqlSelect} WHERE kh.MaKH = @Ma", AnhXa, ThamSo("@Ma", maKH));

    public KhachHang LayTheoSDT(string sdt) =>
        MotHoacNull($"{SqlSelect} WHERE kh.SDT = @SDT", AnhXa, ThamSo("@SDT", sdt));

    public KhachHang LayTheoMaTK(int maTK) =>
        MotHoacNull($"{SqlSelect} WHERE kh.MaTK = @MaTK", AnhXa, ThamSo("@MaTK", maTK));

    public bool TonTaiSDT(string sdt, int? maKHLoaiTru = null)
    {
        object ketQua = GiaTriDon(
            "SELECT COUNT(1) FROM KHACH_HANG WHERE SDT = @SDT AND (@MaLoaiTru IS NULL OR MaKH <> @MaLoaiTru)",
            ThamSo("@SDT", sdt), ThamSo("@MaLoaiTru", (object)maKHLoaiTru ?? DBNull.Value));
        return Convert.ToInt32(ketQua) > 0;
    }

    public int Them(KhachHang khachHang) =>
        ThemTraVeMa(@"INSERT INTO KHACH_HANG (HoTen, SDT, Email, DiaChi, MaTK, NgayTao)
                      VALUES (@HoTen, @SDT, @Email, @DiaChi, @MaTK, GETDATE());
                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
            ThamSo("@HoTen", khachHang.HoTen),
            ThamSo("@SDT", khachHang.SDT),
            ThamSo("@Email", khachHang.Email),
            ThamSo("@DiaChi", khachHang.DiaChi),
            ThamSo("@MaTK", (object)khachHang.MaTK ?? DBNull.Value));

    public int CapNhat(KhachHang khachHang) =>
        ThucThi(@"UPDATE KHACH_HANG SET HoTen = @HoTen, SDT = @SDT, Email = @Email, DiaChi = @DiaChi
                  WHERE MaKH = @Ma",
            ThamSo("@HoTen", khachHang.HoTen),
            ThamSo("@SDT", khachHang.SDT),
            ThamSo("@Email", khachHang.Email),
            ThamSo("@DiaChi", khachHang.DiaChi),
            ThamSo("@Ma", khachHang.MaKH));

    public int Xoa(int maKH) =>
        ThucThi("DELETE FROM KHACH_HANG WHERE MaKH = @Ma", ThamSo("@Ma", maKH));

    public int GanTaiKhoan(int maKH, int maTK) =>
        ThucThi("UPDATE KHACH_HANG SET MaTK = @MaTK WHERE MaKH = @Ma",
            ThamSo("@MaTK", maTK), ThamSo("@Ma", maKH));
}
