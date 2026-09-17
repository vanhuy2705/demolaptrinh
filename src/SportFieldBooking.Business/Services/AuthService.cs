using System.Text.RegularExpressions;
using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Core.Security;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

/// <summary>Nghiệp vụ đăng nhập, đăng ký, đổi mật khẩu.</summary>
public class AuthService
{
    private readonly ITaiKhoanRepository _taiKhoanRepo;
    private readonly INhanVienRepository _nhanVienRepo;
    private readonly IKhachHangRepository _khachHangRepo;

    public AuthService() : this(new Data.Repositories.TaiKhoanRepository(),
                                new Data.Repositories.NhanVienRepository(),
                                new Data.Repositories.KhachHangRepository())
    {
    }

    public AuthService(ITaiKhoanRepository taiKhoanRepo, INhanVienRepository nhanVienRepo, IKhachHangRepository khachHangRepo)
    {
        _taiKhoanRepo = taiKhoanRepo;
        _nhanVienRepo = nhanVienRepo;
        _khachHangRepo = khachHangRepo;
    }

    private static readonly Regex MauTenDangNhap = new(@"^[A-Za-z0-9_]{4,30}$", RegexOptions.Compiled);
    private static readonly Regex MauSDT = new(@"^0\d{9,10}$", RegexOptions.Compiled);
    private static readonly Regex MauEmail = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public KetQua<TaiKhoan> DangNhap(string tenDangNhap, string matKhau)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(matKhau))
                return KetQua<TaiKhoan>.Loi("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.");

            TaiKhoan taiKhoan = _taiKhoanRepo.LayTheoTenDangNhap(tenDangNhap.Trim());
            if (taiKhoan == null)
                return KetQua<TaiKhoan>.Loi("Tên đăng nhập hoặc mật khẩu không đúng.");

            if (taiKhoan.TrangThai == TrangThaiTaiKhoan.BiKhoa)
                return KetQua<TaiKhoan>.Loi("Tài khoản của bạn đang bị khóa. Vui lòng liên hệ quản trị viên.");

            if (!PasswordHasher.KiemTra(matKhau, taiKhoan.MatKhau))
                return KetQua<TaiKhoan>.Loi("Tên đăng nhập hoặc mật khẩu không đúng.");

            // Nâng cấp mật khẩu đang lưu dạng thô (dữ liệu seed nhập tay) sang dạng băm.
            if (!PasswordHasher.LaMatKhauDaBam(taiKhoan.MatKhau))
                _taiKhoanRepo.DoiMatKhau(taiKhoan.MaTK, PasswordHasher.MaHoa(matKhau));

            NhanVien nhanVien = taiKhoan.VaiTro == VaiTro.Admin || taiKhoan.VaiTro == VaiTro.NhanVien
                ? _nhanVienRepo.LayTheoMaTK(taiKhoan.MaTK)
                : null;
            KhachHang khachHang = taiKhoan.VaiTro == VaiTro.KhachHang
                ? _khachHangRepo.LayTheoMaTK(taiKhoan.MaTK)
                : null;

            PhienLamViec.DangNhap(taiKhoan, nhanVien?.MaNV, khachHang?.MaKH);
            return KetQua<TaiKhoan>.Tot(taiKhoan, $"Xin chào {taiKhoan.HoTen}!");
        }
        catch (Exception ex)
        {
            return KetQua<TaiKhoan>.Loi("Không thể đăng nhập: " + ex.Message);
        }
    }

    public KetQua<TaiKhoan> DangKy(string tenDangNhap, string matKhau, string xacNhanMatKhau,
        string hoTen, string sdt, string email, string diaChi)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap) || !MauTenDangNhap.IsMatch(tenDangNhap.Trim()))
                return KetQua<TaiKhoan>.Loi("Tên đăng nhập phải gồm 4-30 ký tự chữ, số hoặc dấu gạch dưới.");
            if (string.IsNullOrWhiteSpace(matKhau) || matKhau.Length < 6)
                return KetQua<TaiKhoan>.Loi("Mật khẩu phải có ít nhất 6 ký tự.");
            if (matKhau != xacNhanMatKhau)
                return KetQua<TaiKhoan>.Loi("Mật khẩu xác nhận không khớp.");
            if (string.IsNullOrWhiteSpace(hoTen) || hoTen.Trim().Length < 2)
                return KetQua<TaiKhoan>.Loi("Vui lòng nhập họ tên (ít nhất 2 ký tự).");
            if (string.IsNullOrWhiteSpace(sdt) || !MauSDT.IsMatch(sdt.Trim()))
                return KetQua<TaiKhoan>.Loi("Số điện thoại không hợp lệ (phải bắt đầu bằng 0 và có 10-11 chữ số).");
            if (!string.IsNullOrWhiteSpace(email) && !MauEmail.IsMatch(email.Trim()))
                return KetQua<TaiKhoan>.Loi("Email không đúng định dạng.");
            if (_taiKhoanRepo.TonTaiTenDangNhap(tenDangNhap.Trim()))
                return KetQua<TaiKhoan>.Loi("Tên đăng nhập đã tồn tại, vui lòng chọn tên khác.");
            if (_khachHangRepo.TonTaiSDT(sdt.Trim()))
                return KetQua<TaiKhoan>.Loi("Số điện thoại này đã được đăng ký.");

            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = tenDangNhap.Trim(),
                MatKhau = PasswordHasher.MaHoa(matKhau),
                HoTen = hoTen.Trim(),
                VaiTro = VaiTro.KhachHang,
                TrangThai = TrangThaiTaiKhoan.HoatDong
            };

            var khachHang = new KhachHang
            {
                HoTen = hoTen.Trim(),
                SDT = sdt.Trim(),
                Email = email?.Trim() ?? "",
                DiaChi = diaChi?.Trim() ?? ""
            };

            Data.Helpers.DbHelper.ChayGiaoDich(() =>
            {
                taiKhoan.MaTK = _taiKhoanRepo.Them(taiKhoan);
                khachHang.MaTK = taiKhoan.MaTK;
                khachHang.MaKH = _khachHangRepo.Them(khachHang);
            });

            return KetQua<TaiKhoan>.Tot(taiKhoan, "Đăng ký thành công, bạn có thể đăng nhập ngay.");
        }
        catch (Exception ex)
        {
            return KetQua<TaiKhoan>.Loi("Không thể đăng ký: " + ex.Message);
        }
    }

    public KetQua DoiMatKhau(int maTK, string matKhauCu, string matKhauMoi, string xacNhanMatKhauMoi)
    {
        try
        {
            TaiKhoan taiKhoan = _taiKhoanRepo.LayTheoMa(maTK);
            if (taiKhoan == null) return KetQua.Loi("Không tìm thấy tài khoản.");
            if (!PasswordHasher.KiemTra(matKhauCu ?? "", taiKhoan.MatKhau))
                return KetQua.Loi("Mật khẩu hiện tại không đúng.");
            if (string.IsNullOrWhiteSpace(matKhauMoi) || matKhauMoi.Length < 6)
                return KetQua.Loi("Mật khẩu mới phải có ít nhất 6 ký tự.");
            if (matKhauMoi != xacNhanMatKhauMoi)
                return KetQua.Loi("Mật khẩu xác nhận không khớp.");

            _taiKhoanRepo.DoiMatKhau(maTK, PasswordHasher.MaHoa(matKhauMoi));
            return KetQua.Tot("Đổi mật khẩu thành công.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể đổi mật khẩu: " + ex.Message);
        }
    }

    public void DangXuat() => PhienLamViec.DangXuat();
}
