using System.Reflection;
using SportFieldBooking.Core.Common;

namespace SportFieldBooking.Business.Services;

/// <summary>
/// Ma trận phân quyền tập trung. Mọi Form đều hỏi service này trước khi mở chức năng
/// (không chỉ ẩn nút), và mọi thao tác nhạy cảm cũng được kiểm tra lại ở tầng nghiệp vụ.
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
        MaQuyen.HdXemCuaToi,
        MaQuyen.VoucherXem, MaQuyen.VoucherSuDung,
        MaQuyen.KmXem,
        MaQuyen.ThongKeCaNhan
    };

    /// <summary>Kiểm tra một vai trò có quyền thực hiện chức năng hay không.</summary>
    public static bool CoQuyen(string vaiTro, string maQuyen)
    {
        if (string.IsNullOrWhiteSpace(vaiTro) || string.IsNullOrWhiteSpace(maQuyen)) return false;
        if (vaiTro == VaiTro.Admin) return TatCaQuyen.Contains(maQuyen);
        if (vaiTro == VaiTro.NhanVien) return QuyenNhanVien.Contains(maQuyen);
        if (vaiTro == VaiTro.KhachHang) return QuyenKhachHang.Contains(maQuyen);
        return false;
    }

    /// <summary>Kiểm tra quyền của người dùng đang đăng nhập.</summary>
    public static bool CoQuyen(string maQuyen) => CoQuyen(PhienLamViec.VaiTro, maQuyen);

    /// <summary>Danh sách toàn bộ mã quyền trong hệ thống.</summary>
    public static IReadOnlyList<string> LayTatCaQuyen() => TatCaQuyen;

    /// <summary>Tên hiển thị của một mã quyền (dùng cho báo lỗi).</summary>
    public static string TenChucNang(string maQuyen)
    {
        string tienTo = maQuyen.Split('_')[0];
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
            _ => "chức năng này"
        };
    }
}
