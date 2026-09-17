using System.Text.RegularExpressions;
using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Core.Security;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

/// <summary>Nghiệp vụ quản lý nhân viên (kèm tạo tài khoản đăng nhập).</summary>
public class NhanVienService
{
    private readonly INhanVienRepository _nhanVienRepo;
    private readonly ITaiKhoanRepository _taiKhoanRepo;

    public NhanVienService() : this(new Data.Repositories.NhanVienRepository(), new Data.Repositories.TaiKhoanRepository())
    {
    }

    public NhanVienService(INhanVienRepository nhanVienRepo, ITaiKhoanRepository taiKhoanRepo)
    {
        _nhanVienRepo = nhanVienRepo;
        _taiKhoanRepo = taiKhoanRepo;
    }

    private static readonly Regex MauSDT = new(@"^0\d{9,10}$", RegexOptions.Compiled);
    private static readonly Regex MauEmail = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public List<NhanVien> LayTatCa(string tuKhoa = "") => _nhanVienRepo.LayTatCa(tuKhoa);

    public NhanVien LayTheoMa(int maNV) => _nhanVienRepo.LayTheoMa(maNV);

    /// <summary>
    /// Thêm nhân viên kèm tài khoản đăng nhập (vai trò do Admin chọn).
    /// Nhân viên không thể tự tạo tài khoản cho mình (kiểm tra quyền ở đây).
    /// </summary>
    public KetQua<NhanVien> Them(NhanVien nhanVien, string tenDangNhap, string matKhau, string vaiTro)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.NvThem))
                return KetQua<NhanVien>.Loi("Bạn không có quyền thêm nhân viên.");

            KetQua kiemTra = KiemTraDuLieu(nhanVien);
            if (!kiemTra.ThanhCong) return KetQua<NhanVien>.Loi(kiemTra.ThongBao);

            if (vaiTro != VaiTro.NhanVien && vaiTro != VaiTro.Admin)
                return KetQua<NhanVien>.Loi("Vai trò nhân viên không hợp lệ.");
            if (string.IsNullOrWhiteSpace(tenDangNhap) || tenDangNhap.Trim().Length < 4)
                return KetQua<NhanVien>.Loi("Tên đăng nhập phải có ít nhất 4 ký tự.");
            if (_taiKhoanRepo.TonTaiTenDangNhap(tenDangNhap.Trim()))
                return KetQua<NhanVien>.Loi("Tên đăng nhập đã tồn tại.");
            if (string.IsNullOrWhiteSpace(matKhau) || matKhau.Length < 6)
                return KetQua<NhanVien>.Loi("Mật khẩu phải có ít nhất 6 ký tự.");
            if (_nhanVienRepo.TonTaiSDT(nhanVien.SDT.Trim()))
                return KetQua<NhanVien>.Loi("Số điện thoại đã được dùng cho nhân viên khác.");

            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = tenDangNhap.Trim(),
                MatKhau = PasswordHasher.MaHoa(matKhau),
                HoTen = nhanVien.HoTen.Trim(),
                VaiTro = vaiTro,
                TrangThai = TrangThaiTaiKhoan.HoatDong
            };

            Data.Helpers.DbHelper.ChayGiaoDich(() =>
            {
                taiKhoan.MaTK = _taiKhoanRepo.Them(taiKhoan);
                nhanVien.MaTK = taiKhoan.MaTK;
                nhanVien.TrangThai = TrangThaiTaiKhoan.HoatDong;
                nhanVien.MaNV = _nhanVienRepo.Them(nhanVien);
            });

            return KetQua<NhanVien>.Tot(nhanVien, "Thêm nhân viên và tài khoản thành công.");
        }
        catch (Exception ex)
        {
            return KetQua<NhanVien>.Loi("Không thể thêm nhân viên: " + ex.Message);
        }
    }

    public KetQua CapNhat(NhanVien nhanVien)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.NvSua))
                return KetQua.Loi("Bạn không có quyền sửa nhân viên.");

            KetQua kiemTra = KiemTraDuLieu(nhanVien);
            if (!kiemTra.ThanhCong) return kiemTra;
            if (_nhanVienRepo.TonTaiSDT(nhanVien.SDT.Trim(), nhanVien.MaNV))
                return KetQua.Loi("Số điện thoại đã được dùng cho nhân viên khác.");

            _nhanVienRepo.CapNhat(nhanVien);
            return KetQua.Tot("Cập nhật nhân viên thành công.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể cập nhật nhân viên: " + ex.Message);
        }
    }

    /// <summary>Nhân viên không được tự nâng quyền: chỉ Admin mới đổi được vai trò tài khoản.</summary>
    public KetQua DoiVaiTro(int maNV, string vaiTroMoi)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.TkPhanQuyen))
                return KetQua.Loi("Chỉ quản trị viên mới được phân quyền.");

            NhanVien nhanVien = _nhanVienRepo.LayTheoMa(maNV);
            if (nhanVien?.MaTK == null) return KetQua.Loi("Nhân viên chưa có tài khoản đăng nhập.");
            if (vaiTroMoi != VaiTro.Admin && vaiTroMoi != VaiTro.NhanVien)
                return KetQua.Loi("Vai trò không hợp lệ.");

            TaiKhoan taiKhoan = _taiKhoanRepo.LayTheoMa(nhanVien.MaTK.Value);
            if (taiKhoan == null) return KetQua.Loi("Không tìm thấy tài khoản của nhân viên.");
            if (taiKhoan.MaTK == PhienLamViec.MaTK && vaiTroMoi != PhienLamViec.VaiTro)
                return KetQua.Loi("Bạn không được thay đổi vai trò của chính mình.");

            taiKhoan.VaiTro = vaiTroMoi;
            taiKhoan.MatKhau = "";   // giữ nguyên mật khẩu
            _taiKhoanRepo.CapNhat(taiKhoan);
            return KetQua.Tot("Phân quyền thành công.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể phân quyền: " + ex.Message);
        }
    }

    public KetQua DoiTrangThai(int maNV, string trangThai)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.NvSua))
                return KetQua.Loi("Bạn không có quyền đổi trạng thái nhân viên.");

            _nhanVienRepo.DoiTrangThai(maNV, trangThai);
            if (trangThai == TrangThaiTaiKhoan.BiKhoa)
            {
                NhanVien nhanVien = _nhanVienRepo.LayTheoMa(maNV);
                if (nhanVien?.MaTK != null) _taiKhoanRepo.DoiTrangThai(nhanVien.MaTK.Value, TrangThaiTaiKhoan.BiKhoa);
            }
            return KetQua.Tot("Cập nhật trạng thái thành công.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể đổi trạng thái: " + ex.Message);
        }
    }

    public KetQua Xoa(int maNV)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.NvXoa))
                return KetQua.Loi("Bạn không có quyền xóa nhân viên.");

            NhanVien nhanVien = _nhanVienRepo.LayTheoMa(maNV);
            if (nhanVien == null) return KetQua.Loi("Nhân viên không tồn tại.");

            Data.Helpers.DbHelper.ChayGiaoDich(() =>
            {
                _nhanVienRepo.Xoa(maNV);
                if (nhanVien.MaTK != null) _taiKhoanRepo.Xoa(nhanVien.MaTK.Value);
            });

            return KetQua.Tot("Xóa nhân viên và tài khoản tương ứng thành công.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể xóa nhân viên: " + ex.Message);
        }
    }

    private static KetQua KiemTraDuLieu(NhanVien nhanVien)
    {
        if (nhanVien == null) return KetQua.Loi("Dữ liệu nhân viên không hợp lệ.");
        if (string.IsNullOrWhiteSpace(nhanVien.HoTen)) return KetQua.Loi("Vui lòng nhập họ tên nhân viên.");
        if (string.IsNullOrWhiteSpace(nhanVien.SDT) || !MauSDT.IsMatch(nhanVien.SDT.Trim()))
            return KetQua.Loi("Số điện thoại không hợp lệ (bắt đầu bằng 0, 10-11 chữ số).");
        if (!string.IsNullOrWhiteSpace(nhanVien.Email) && !MauEmail.IsMatch(nhanVien.Email.Trim()))
            return KetQua.Loi("Email không đúng định dạng.");
        return KetQua.Tot();
    }
}
