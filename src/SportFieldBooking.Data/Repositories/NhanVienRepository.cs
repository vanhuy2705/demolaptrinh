using System.Data;
using Microsoft.Data.SqlClient;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Data.Repositories;

/// <summary>Repository bảng NHAN_VIEN (join TAIKHOAN để lấy tên đăng nhập).</summary>
public class NhanVienRepository : BaseRepository, INhanVienRepository
{
    private const string SqlSelect = @"
        SELECT nv.MaNV, nv.MaTK, nv.HoTen, nv.SDT, nv.Email, nv.DiaChi, nv.ChucVu, nv.NgayVaoLam, nv.TrangThai,
               ISNULL(tk.TenDangNhap, '') AS TenDangNhap, ISNULL(tk.VaiTro, '') AS VaiTro
        FROM NHAN_VIEN nv
        LEFT JOIN TAIKHOAN tk ON tk.MaTK = nv.MaTK";

    private static NhanVien AnhXa(DataRow dong) => new()
    {
        MaNV = dong.SoNguyen("MaNV"),
        MaTK = dong.SoNguyenCoTheNull("MaTK"),
        HoTen = dong.Chuoi("HoTen"),
        SDT = dong.Chuoi("SDT"),
        Email = dong.Chuoi("Email"),
        DiaChi = dong.Chuoi("DiaChi"),
        ChucVu = dong.Chuoi("ChucVu"),
        NgayVaoLam = dong.NgayGioCoTheNull("NgayVaoLam"),
        TrangThai = dong.Chuoi("TrangThai"),
        TenDangNhap = dong.Chuoi("TenDangNhap"),
        VaiTro = dong.Chuoi("VaiTro")
    };

    public List<NhanVien> LayTatCa(string tuKhoa = "") =>
        DanhSach($@"{SqlSelect}
                    WHERE (@TuKhoa IS NULL OR nv.HoTen LIKE @TuKhoa OR nv.SDT LIKE @TuKhoa OR nv.ChucVu LIKE @TuKhoa)
                    ORDER BY nv.HoTen",
            AnhXa, ThamSoTimKiem("@TuKhoa", tuKhoa));

    public NhanVien LayTheoMa(int maNV) =>
        MotHoacNull($"{SqlSelect} WHERE nv.MaNV = @MaNV", AnhXa, ThamSo("@MaNV", maNV));

    public NhanVien LayTheoMaTK(int maTK) =>
        MotHoacNull($"{SqlSelect} WHERE nv.MaTK = @MaTK", AnhXa, ThamSo("@MaTK", maTK));

    public bool TonTaiSDT(string sdt, int? maNVLoaiTru = null)
    {
        object ketQua = GiaTriDon(
            "SELECT COUNT(1) FROM NHAN_VIEN WHERE SDT = @SDT AND (@MaLoaiTru IS NULL OR MaNV <> @MaLoaiTru)",
            ThamSo("@SDT", sdt), ThamSo("@MaLoaiTru", (object)maNVLoaiTru ?? DBNull.Value));
        return Convert.ToInt32(ketQua) > 0;
    }

    public int Them(NhanVien nhanVien) =>
        ThemTraVeMa(@"INSERT INTO NHAN_VIEN (MaTK, HoTen, SDT, Email, DiaChi, ChucVu, NgayVaoLam, TrangThai)
                      VALUES (@MaTK, @HoTen, @SDT, @Email, @DiaChi, @ChucVu, @NgayVaoLam, @TrangThai);
                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
            ThamSo("@MaTK", (object)nhanVien.MaTK ?? DBNull.Value),
            ThamSo("@HoTen", nhanVien.HoTen),
            ThamSo("@SDT", nhanVien.SDT),
            ThamSo("@Email", nhanVien.Email),
            ThamSo("@DiaChi", nhanVien.DiaChi),
            ThamSo("@ChucVu", nhanVien.ChucVu),
            ThamSo("@NgayVaoLam", (object)nhanVien.NgayVaoLam ?? DBNull.Value),
            ThamSo("@TrangThai", nhanVien.TrangThai));

    public int CapNhat(NhanVien nhanVien) =>
        ThucThi(@"UPDATE NHAN_VIEN
                  SET MaTK = @MaTK, HoTen = @HoTen, SDT = @SDT, Email = @Email, DiaChi = @DiaChi,
                      ChucVu = @ChucVu, NgayVaoLam = @NgayVaoLam, TrangThai = @TrangThai
                  WHERE MaNV = @MaNV",
            ThamSo("@MaTK", (object)nhanVien.MaTK ?? DBNull.Value),
            ThamSo("@HoTen", nhanVien.HoTen),
            ThamSo("@SDT", nhanVien.SDT),
            ThamSo("@Email", nhanVien.Email),
            ThamSo("@DiaChi", nhanVien.DiaChi),
            ThamSo("@ChucVu", nhanVien.ChucVu),
            ThamSo("@NgayVaoLam", (object)nhanVien.NgayVaoLam ?? DBNull.Value),
            ThamSo("@TrangThai", nhanVien.TrangThai),
            ThamSo("@MaNV", nhanVien.MaNV));

    public int DoiTrangThai(int maNV, string trangThai) =>
        ThucThi("UPDATE NHAN_VIEN SET TrangThai = @TrangThai WHERE MaNV = @MaNV",
            ThamSo("@TrangThai", trangThai), ThamSo("@MaNV", maNV));

    public int Xoa(int maNV) =>
        ThucThi("DELETE FROM NHAN_VIEN WHERE MaNV = @MaNV", ThamSo("@MaNV", maNV));
}
