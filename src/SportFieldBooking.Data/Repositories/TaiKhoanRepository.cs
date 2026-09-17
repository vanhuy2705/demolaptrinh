using System.Data;
using Microsoft.Data.SqlClient;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Data.Repositories;

/// <summary>Repository bảng TAIKHOAN - chỉ chứa SQL và ánh xạ dữ liệu.</summary>
public class TaiKhoanRepository : BaseRepository, ITaiKhoanRepository
{
    private const string Cot = "MaTK, TenDangNhap, MatKhau, HoTen, VaiTro, TrangThai, NgayTao";

    private static TaiKhoan AnhXa(DataRow dong) => new()
    {
        MaTK = dong.SoNguyen("MaTK"),
        TenDangNhap = dong.Chuoi("TenDangNhap"),
        MatKhau = dong.Chuoi("MatKhau"),
        HoTen = dong.Chuoi("HoTen"),
        VaiTro = dong.Chuoi("VaiTro"),
        TrangThai = dong.Chuoi("TrangThai"),
        NgayTao = dong.NgayGio("NgayTao")
    };

    public TaiKhoan LayTheoMa(int maTK) =>
        MotHoacNull($"SELECT {Cot} FROM TAIKHOAN WHERE MaTK = @MaTK", AnhXa, ThamSo("@MaTK", maTK));

    public TaiKhoan LayTheoTenDangNhap(string tenDangNhap) =>
        MotHoacNull($"SELECT {Cot} FROM TAIKHOAN WHERE TenDangNhap = @Ten", AnhXa, ThamSo("@Ten", tenDangNhap));

    public List<TaiKhoan> LayTatCa(string tuKhoa = "") =>
        DanhSach($@"SELECT {Cot} FROM TAIKHOAN
                    WHERE (@TuKhoa IS NULL OR TenDangNhap LIKE @TuKhoa OR HoTen LIKE @TuKhoa)
                    ORDER BY CASE VaiTro WHEN 'Admin' THEN 1 WHEN 'NhanVien' THEN 2 ELSE 3 END, TenDangNhap",
            AnhXa, ThamSoTimKiem("@TuKhoa", tuKhoa));

    public List<TaiKhoan> LayTheoVaiTro(string vaiTro) =>
        DanhSach($"SELECT {Cot} FROM TAIKHOAN WHERE VaiTro = @VaiTro ORDER BY TenDangNhap",
            AnhXa, ThamSo("@VaiTro", vaiTro));

    public bool TonTaiTenDangNhap(string tenDangNhap, int? maTKLoaiTru = null)
    {
        object ketQua = GiaTriDon(
            "SELECT COUNT(1) FROM TAIKHOAN WHERE TenDangNhap = @Ten AND (@MaLoaiTru IS NULL OR MaTK <> @MaLoaiTru)",
            ThamSo("@Ten", tenDangNhap), ThamSo("@MaLoaiTru", (object)maTKLoaiTru ?? DBNull.Value));
        return Convert.ToInt32(ketQua) > 0;
    }

    public int Them(TaiKhoan taiKhoan) =>
        ThemTraVeMa(@"INSERT INTO TAIKHOAN (TenDangNhap, MatKhau, HoTen, VaiTro, TrangThai, NgayTao)
                      VALUES (@TenDangNhap, @MatKhau, @HoTen, @VaiTro, @TrangThai, GETDATE());
                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
            ThamSo("@TenDangNhap", taiKhoan.TenDangNhap),
            ThamSo("@MatKhau", taiKhoan.MatKhau),
            ThamSo("@HoTen", taiKhoan.HoTen),
            ThamSo("@VaiTro", taiKhoan.VaiTro),
            ThamSo("@TrangThai", taiKhoan.TrangThai));

    public int CapNhat(TaiKhoan taiKhoan) =>
        ThucThi(@"UPDATE TAIKHOAN
                  SET TenDangNhap = @TenDangNhap,
                      MatKhau = CASE WHEN ISNULL(@MatKhau, '') = '' THEN MatKhau ELSE @MatKhau END,
                      HoTen = @HoTen,
                      VaiTro = @VaiTro,
                      TrangThai = @TrangThai
                  WHERE MaTK = @MaTK",
            ThamSo("@TenDangNhap", taiKhoan.TenDangNhap),
            ThamSo("@MatKhau", taiKhoan.MatKhau ?? ""),
            ThamSo("@HoTen", taiKhoan.HoTen),
            ThamSo("@VaiTro", taiKhoan.VaiTro),
            ThamSo("@TrangThai", taiKhoan.TrangThai),
            ThamSo("@MaTK", taiKhoan.MaTK));

    public int DoiMatKhau(int maTK, string matKhauDaBam) =>
        ThucThi("UPDATE TAIKHOAN SET MatKhau = @MatKhau WHERE MaTK = @MaTK",
            ThamSo("@MatKhau", matKhauDaBam), ThamSo("@MaTK", maTK));

    public int DoiTrangThai(int maTK, string trangThai) =>
        ThucThi("UPDATE TAIKHOAN SET TrangThai = @TrangThai WHERE MaTK = @MaTK",
            ThamSo("@TrangThai", trangThai), ThamSo("@MaTK", maTK));

    public int Xoa(int maTK) =>
        ThucThi("DELETE FROM TAIKHOAN WHERE MaTK = @MaTK", ThamSo("@MaTK", maTK));

    public int DemTheoVaiTro(string vaiTro) =>
        Convert.ToInt32(GiaTriDon("SELECT COUNT(1) FROM TAIKHOAN WHERE VaiTro = @VaiTro", ThamSo("@VaiTro", vaiTro)));
}
