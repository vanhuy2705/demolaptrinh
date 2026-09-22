using SportFieldBooking.Core.Entities;

namespace SportFieldBooking.Data.Interfaces;

public interface IQuyenRepository
{
    List<Quyen> LayTatCa();
    List<VaiTroDb> LayVaiTro();
    List<VaiTroQuyen> LayMaTran();
    List<string> LayQuyenTheoVaiTro(string maVaiTro);
    bool KiemTraQuyen(string maVaiTro, string maQuyen);
    void ThemQuyenVaoVaiTro(string maVaiTro, string maQuyen);
    void XoaQuyenKhoiVaiTro(string maVaiTro, string maQuyen);
    void CapNhatMaTranVaiTro(string maVaiTro, IEnumerable<string> danhSachQuyen);
}
