using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

public class QuyenService
{
    private readonly IQuyenRepository _repo;

    public QuyenService() : this(new Data.Repositories.QuyenRepository()) { }
    public QuyenService(IQuyenRepository repo) => _repo = repo;

    public List<Quyen> LayTatCa() => _repo.LayTatCa();
    public List<VaiTroDb> LayVaiTro() => _repo.LayVaiTro();
    public List<VaiTroQuyen> LayMaTran() => _repo.LayMaTran();
    public List<string> LayQuyenTheoVaiTro(string vaiTro) => _repo.LayQuyenTheoVaiTro(vaiTro);

    public KetQua CapNhatMaTran(string vaiTro, IEnumerable<string> danhSachQuyen)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.TkPhanQuyen))
                return KetQua.Loi("Bạn không có quyền phân quyền.");

            if (string.IsNullOrWhiteSpace(vaiTro))
                return KetQua.Loi("Vai trò không hợp lệ.");

            _repo.CapNhatMaTranVaiTro(vaiTro, danhSachQuyen);
            PhanQuyenService.XoaCache();

            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "PhanQuyen", "VAI_TRO_QUYEN", vaiTro, $"Cập nhật quyền cho vai trò {vaiTro}"); } catch { }

            return KetQua.Tot($"Đã cập nhật {danhSachQuyen.Count()} quyền cho vai trò {vaiTro}.");
        }
        catch (Exception ex) { return KetQua.Loi("Không thể cập nhật quyền: " + ex.Message); }
    }
}
