using System.Reflection;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

/// <summary>
/// Ma trận phân quyền tập trung. Mọi Form đều hỏi service này trước khi mở chức năng
/// (không chỉ ẩn nút), và mọi thao tác nhạy cảm cũng được kiểm tra lại ở tầng nghiệp vụ.
/// Hỗ trợ đọc quyền động từ CSDL (VAI_TRO_QUYEN) nếu CSDL đã nâng cấp v2, fallback về hard-code.
/// </summary>
public static class PhanQuyenService
{
    private static readonly string[] TatCaQuyen = typeof(MaQuyen)
        .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
        .Where(f => f.IsLiteral && f.FieldType == typeof(string))
        .Select(f => (string)f.GetRawConstantValue())
        .ToArray();

    /// <summary>Quyền của Nhân viên theo ma trận chức năng (mục 6 của đặc tả).</summary>
    private static readonly HashSet<string> QuyenNhanVien = new()
    {
        MaQuyen.TkDoiMatKhau,
        MaQuyen.LoaiSanXem,
        MaQuyen.SanXem, MaQuyen.SanDoiTrangThai,
        MaQuyen.KhXem, MaQuyen.KhXemTatCa, MaQuyen.KhThem, MaQuyen.KhSua,
        MaQuyen.DatSanXemTatCa, MaQuyen.DatSanThem, MaQuyen.DatSanSua, MaQuyen.DatSanHuy,
        MaQuyen.LichXemTatCa,
        MaQuyen.HdXemTatCa, MaQuyen.HdLap, MaQuyen.HdSua, MaQuyen.HdThanhToan, MaQuyen.HdIn,
        MaQuyen.VoucherXem, MaQuyen.VoucherApDung,
        MaQuyen.KmXem, MaQuyen.KmApDung,
        MaQuyen.ThongKeNghiepVu
    };

    /// <summary>Quyền của Khách hàng (chỉ dữ liệu của chính mình).</summary>
    private static readonly HashSet<string> QuyenKhachHang = new()
    {
        MaQuyen.TkDoiMatKhau,
        MaQuyen.LoaiSanXem,
        MaQuyen.SanXem,
        MaQuyen.KhXem,
        MaQuyen.DatSanXemCuaToi, MaQuyen.DatSanThem, MaQuyen.DatSanHuy,
        MaQuyen.LichXemCuaToi,
        MaQuyen.HdXemCuaToi, MaQuyen.HdIn,
        MaQuyen.VoucherXem, MaQuyen.VoucherSuDung,
        MaQuyen.KmXem,
        MaQuyen.ThongKeCaNhan
    };

    // Cache quyền động từ CSDL
    private static Dictionary<string, HashSet<string>> _cacheQuyenDb;
    private static DateTime _cacheThoiGian = DateTime.MinValue;
    private static readonly TimeSpan _cacheHan = TimeSpan.FromMinutes(5);
    private static readonly object _cacheKhoa = new();

    private static void NapCacheNeuCan()
    {
        if (_cacheQuyenDb != null && DateTime.Now - _cacheThoiGian < _cacheHan) return;
        lock (_cacheKhoa)
        {
            if (_cacheQuyenDb != null && DateTime.Now - _cacheThoiGian < _cacheHan) return;
            try
            {
                IQuyenRepository repo = new Data.Repositories.QuyenRepository();
                var maTran = repo.LayMaTran();
                if (maTran.Count == 0) return; // CSDL chưa có bảng quyền -> dùng hard-code

                var dict = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
                foreach (var item in maTran)
                {
                    if (!dict.TryGetValue(item.MaVaiTro, out var set))
                    {
                        set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        dict[item.MaVaiTro] = set;
                    }
                    set.Add(item.MaQuyen);
                }
                _cacheQuyenDb = dict;
                _cacheThoiGian = DateTime.Now;
            }
            catch
            {
                // Nếu lỗi (CSDL cũ chưa có bảng): giữ cache null để dùng hard-code
            }
        }
    }

    public static void XoaCache() { lock (_cacheKhoa) { _cacheQuyenDb = null; _cacheThoiGian = DateTime.MinValue; } }

    /// <summary>Kiểm tra một vai trò có quyền thực hiện chức năng hay không.</summary>
    public static bool CoQuyen(string vaiTro, string maQuyen)
    {
        if (string.IsNullOrWhiteSpace(vaiTro) || string.IsNullOrWhiteSpace(maQuyen)) return false;

        NapCacheNeuCan();

        if (_cacheQuyenDb != null)
        {
            if (vaiTro == VaiTro.Admin)
            {
                // Admin: nếu trong DB có quyền thì kiểm tra, nếu không thì vẫn full quyền fallback
                if (_cacheQuyenDb.TryGetValue(VaiTro.Admin, out var adminSet) && adminSet.Count > 0)
                    return adminSet.Contains(maQuyen);
                return TatCaQuyen.Contains(maQuyen);
            }

            if (_cacheQuyenDb.TryGetValue(vaiTro, out var set))
                return set.Contains(maQuyen);
            // Nếu vai trò không có trong DB, fallback về hard-code
        }

        if (vaiTro == VaiTro.Admin) return TatCaQuyen.Contains(maQuyen);
        if (vaiTro == VaiTro.NhanVien) return QuyenNhanVien.Contains(maQuyen);
        if (vaiTro == VaiTro.KhachHang) return QuyenKhachHang.Contains(maQuyen);
        return false;
    }

    /// <summary>Kiểm tra quyền của người dùng đang đăng nhập.</summary>
    public static bool CoQuyen(string maQuyen) => CoQuyen(PhienLamViec.VaiTro, maQuyen);

    /// <summary>Danh sách toàn bộ mã quyền trong hệ thống.</summary>
    public static IReadOnlyList<string> LayTatCaQuyen() => TatCaQuyen;

    /// <summary>Lấy quyền theo vai trò (ưu tiên DB, fallback hard-code).</summary>
    public static List<string> LayQuyenTheoVaiTro(string vaiTro)
    {
        NapCacheNeuCan();
        if (_cacheQuyenDb != null && _cacheQuyenDb.TryGetValue(vaiTro, out var set))
            return set.ToList();
        if (vaiTro == VaiTro.Admin) return TatCaQuyen.ToList();
        if (vaiTro == VaiTro.NhanVien) return QuyenNhanVien.ToList();
        if (vaiTro == VaiTro.KhachHang) return QuyenKhachHang.ToList();
        return new List<string>();
    }

    /// <summary>Tên hiển thị của một mã quyền (dùng cho báo lỗi).</summary>
    public static string TenChucNang(string maQuyen)
    {
        string tienTo = maQuyen.Contains('_') ? maQuyen.Split('_')[0] : maQuyen;
        return tienTo switch
        {
            "TK" => "quản lý tài khoản",
            "NV" => "quản lý nhân viên",
            "LOAISAN" => "quản lý loại sân",
            "SAN" => "quản lý sân",
            "KH" => "quản lý khách hàng",
            "DATSAN" => "đặt sân",
            "LICH" => "lịch đặt sân",
            "HD" => "hóa đơn",
            "VOUCHER" => "voucher",
            "KM" => "khuyến mãi",
            "CAUHINH" => "cấu hình hệ thống",
            "THONGTKE" => "thống kê",
            "NHATKY" => "nhật ký hoạt động",
            _ => "chức năng này"
        };
    }
}
