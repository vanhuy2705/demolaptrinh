namespace SportFieldBooking.Data.Interfaces;

/// <summary>Truy xuất dữ liệu bảng TAIKHOAN.</summary>
public interface ITaiKhoanRepository
{
    TaiKhoan LayTheoMa(int maTK);
    TaiKhoan LayTheoTenDangNhap(string tenDangNhap);
    List<TaiKhoan> LayTatCa(string tuKhoa = "");
    List<TaiKhoan> LayTheoVaiTro(string vaiTro);
    bool TonTaiTenDangNhap(string tenDangNhap, int? maTKLoaiTru = null);
    int Them(TaiKhoan taiKhoan);
    int CapNhat(TaiKhoan taiKhoan);
    int DoiMatKhau(int maTK, string matKhauDaBam);
    int DoiTrangThai(int maTK, string trangThai);
    int Xoa(int maTK);
    int DemTheoVaiTro(string vaiTro);
}
