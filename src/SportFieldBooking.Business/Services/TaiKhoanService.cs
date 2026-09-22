using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Core.Security;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

/// <summary>Nghiệp vụ quản lý tài khoản: thêm/sửa/khóa/mở/phân quyền.</summary>
public class TaiKhoanService
{
    private readonly ITaiKhoanRepository _taiKhoanRepo;
    private readonly INhanVienRepository _nhanVienRepo;

    public TaiKhoanService() : this(new Data.Repositories.TaiKhoanRepository(), new Data.Repositories.NhanVienRepository())
    {
    }

    public TaiKhoanService(ITaiKhoanRepository taiKhoanRepo, INhanVienRepository nhanVienRepo)
    {
        _taiKhoanRepo = taiKhoanRepo;
        _nhanVienRepo = nhanVienRepo;
    }

    public List<TaiKhoan> LayTatCa(string tuKhoa = "") => _taiKhoanRepo.LayTatCa(tuKhoa);

    public TaiKhoan LayTheoMa(int maTK) => _taiKhoanRepo.LayTheoMa(maTK);

    public KetQua<TaiKhoan> Them(TaiKhoan taiKhoan, string matKhau)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.TkThem))
                return KetQua<TaiKhoan>.Loi("Bạn không có quyền tạo tài khoản.");
            if (taiKhoan == null) return KetQua<TaiKhoan>.Loi("Dữ liệu tài khoản không hợp lệ.");
            if (string.IsNullOrWhiteSpace(taiKhoan.TenDangNhap))
                return KetQua<TaiKhoan>.Loi("Vui lòng nhập tên đăng nhập.");
            if (_taiKhoanRepo.TonTaiTenDangNhap(taiKhoan.TenDangNhap.Trim()))
                return KetQua<TaiKhoan>.Loi("Tên đăng nhập đã tồn tại.");
            if (string.IsNullOrWhiteSpace(matKhau) || matKhau.Length < 6)
                return KetQua<TaiKhoan>.Loi("Mật khẩu phải có ít nhất 6 ký tự.");
            if (taiKhoan.VaiTro != VaiTro.Admin && taiKhoan.VaiTro != VaiTro.NhanVien && taiKhoan.VaiTro != VaiTro.KhachHang)
                return KetQua<TaiKhoan>.Loi("Vai trò không hợp lệ.");
            if (string.IsNullOrWhiteSpace(taiKhoan.HoTen))
                return KetQua<TaiKhoan>.Loi("Vui lòng nhập họ tên.");

            taiKhoan.TenDangNhap = taiKhoan.TenDangNhap.Trim();
            taiKhoan.MatKhau = PasswordHasher.MaHoa(matKhau);
            taiKhoan.MaTK = _taiKhoanRepo.Them(taiKhoan);
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "ThemTaiKhoan", "TAIKHOAN", taiKhoan.MaTK.ToString(), $"Thêm TK {taiKhoan.TenDangNhap} ({taiKhoan.VaiTro})"); } catch { }
            return KetQua<TaiKhoan>.Tot(taiKhoan, "Thêm tài khoản thành công.");
        }
        catch (Exception ex)
        {
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "ThemTaiKhoan", "TAIKHOAN", null, $"Lỗi thêm TK: {ex.Message}", KetQuaNhatKy.ThatBai); } catch { }
            return KetQua<TaiKhoan>.Loi("Không thể thêm tài khoản: " + ex.Message);
        }
    }

    public KetQua CapNhat(TaiKhoan taiKhoan)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.TkSua))
                return KetQua.Loi("Bạn không có quyền sửa tài khoản.");
            if (taiKhoan == null || taiKhoan.MaTK <= 0) return KetQua.Loi("Tài khoản không hợp lệ.");
            if (string.IsNullOrWhiteSpace(taiKhoan.HoTen)) return KetQua.Loi("Vui lòng nhập họ tên.");
            if (_taiKhoanRepo.TonTaiTenDangNhap(taiKhoan.TenDangNhap.Trim(), taiKhoan.MaTK))
                return KetQua.Loi("Tên đăng nhập đã được dùng bởi tài khoản khác.");

            if (taiKhoan.MaTK == PhienLamViec.MaTK && taiKhoan.VaiTro != PhienLamViec.VaiTro)
                return KetQua.Loi("Bạn không được thay đổi vai trò của chính mình.");

            taiKhoan.MatKhau = "";
            _taiKhoanRepo.CapNhat(taiKhoan);
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "SuaTaiKhoan", "TAIKHOAN", taiKhoan.MaTK.ToString(), $"Sửa TK #{taiKhoan.MaTK}"); } catch { }
            return KetQua.Tot("Cập nhật tài khoản thành công.");
        }
        catch (Exception ex)
        {
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "SuaTaiKhoan", "TAIKHOAN", taiKhoan?.MaTK.ToString(), $"Lỗi sửa TK: {ex.Message}", KetQuaNhatKy.ThatBai); } catch { }
            return KetQua.Loi("Không thể cập nhật tài khoản: " + ex.Message);
        }
    }

    public KetQua DoiTrangThai(int maTK, string trangThai)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.TkKhoa))
                return KetQua.Loi("Bạn không có quyền khóa/mở tài khoản.");
            if (maTK == PhienLamViec.MaTK)
                return KetQua.Loi("Không thể khóa tài khoản đang đăng nhập.");
            if (trangThai != TrangThaiTaiKhoan.HoatDong && trangThai != TrangThaiTaiKhoan.BiKhoa)
                return KetQua.Loi("Trạng thái không hợp lệ.");

            _taiKhoanRepo.DoiTrangThai(maTK, trangThai);
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "KhoaMoTaiKhoan", "TAIKHOAN", maTK.ToString(), $"Đổi trạng thái TK #{maTK} -> {trangThai}"); } catch { }
            return KetQua.Tot(trangThai == TrangThaiTaiKhoan.BiKhoa ? "Đã khóa tài khoản." : "Đã mở khóa tài khoản.");
        }
        catch (Exception ex)
        {
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "KhoaMoTaiKhoan", "TAIKHOAN", maTK.ToString(), $"Lỗi khóa/mở: {ex.Message}", KetQuaNhatKy.ThatBai); } catch { }
            return KetQua.Loi("Không thể đổi trạng thái tài khoản: " + ex.Message);
        }
    }

    public KetQua DatLaiMatKhau(int maTK, string matKhauMoi)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.TkSua))
                return KetQua.Loi("Bạn không có quyền đặt lại mật khẩu.");
            if (string.IsNullOrWhiteSpace(matKhauMoi) || matKhauMoi.Length < 6)
                return KetQua.Loi("Mật khẩu mới phải có ít nhất 6 ký tự.");

            _taiKhoanRepo.DoiMatKhau(maTK, PasswordHasher.MaHoa(matKhauMoi));
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "DatLaiMatKhau", "TAIKHOAN", maTK.ToString(), $"Đặt lại MK TK #{maTK}"); } catch { }
            return KetQua.Tot("Đặt lại mật khẩu thành công.");
        }
        catch (Exception ex)
        {
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "DatLaiMatKhau", "TAIKHOAN", maTK.ToString(), $"Lỗi đặt lại MK: {ex.Message}", KetQuaNhatKy.ThatBai); } catch { }
            return KetQua.Loi("Không thể đặt lại mật khẩu: " + ex.Message);
        }
    }

    public KetQua Xoa(int maTK)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.TkXoa))
                return KetQua.Loi("Bạn không có quyền xóa tài khoản.");
            if (maTK == PhienLamViec.MaTK)
                return KetQua.Loi("Không thể xóa tài khoản đang đăng nhập.");

            TaiKhoan taiKhoan = _taiKhoanRepo.LayTheoMa(maTK);
            if (taiKhoan == null) return KetQua.Loi("Tài khoản không tồn tại.");

            NhanVien nhanVien = _nhanVienRepo.LayTheoMaTK(maTK);
            if (nhanVien != null)
                return KetQua.Loi("Tài khoản này đang gắn với hồ sơ nhân viên, hãy xóa hồ sơ nhân viên trước.");

            _taiKhoanRepo.Xoa(maTK);
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "XoaTaiKhoan", "TAIKHOAN", maTK.ToString(), $"Xóa TK #{maTK}"); } catch { }
            return KetQua.Tot("Xóa tài khoản thành công.");
        }
        catch (Exception ex)
        {
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "XoaTaiKhoan", "TAIKHOAN", maTK.ToString(), $"Lỗi xóa TK: {ex.Message}", KetQuaNhatKy.ThatBai); } catch { }
            return KetQua.Loi("Không thể xóa tài khoản: " + ex.Message);
        }
    }

    public int DemTheoVaiTro(string vaiTro) => _taiKhoanRepo.DemTheoVaiTro(vaiTro);
}
