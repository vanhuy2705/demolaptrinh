namespace SportFieldBooking.Data.Interfaces;

/// <summary>Truy xuất dữ liệu bảng NHAN_VIEN.</summary>
public interface INhanVienRepository
{
    List<NhanVien> LayTatCa(string tuKhoa = "");
    NhanVien LayTheoMa(int maNV);
    NhanVien LayTheoMaTK(int maTK);
    bool TonTaiSDT(string sdt, int? maNVLoaiTru = null);
    int Them(NhanVien nhanVien);
    int CapNhat(NhanVien nhanVien);
    int DoiTrangThai(int maNV, string trangThai);
    int Xoa(int maNV);
}
