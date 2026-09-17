namespace SportFieldBooking.Core.Common;

/// <summary>
/// Các hằng số dùng chung toàn hệ thống (vai trò, trạng thái, tham số cấu hình, mã quyền).
/// Chỉ chứa hằng số/biến tĩnh - không chứa truy vấn dữ liệu hay nghiệp vụ.
/// </summary>
public static class VaiTro
{
    public const string Admin = "Admin";
    public const string NhanVien = "NhanVien";
    public const string KhachHang = "KhachHang";

    public static readonly string[] TatCa = { Admin, NhanVien, KhachHang };

    public static string TenHienThi(string vaiTro) => vaiTro switch
    {
        Admin => "Quản trị viên",
        NhanVien => "Nhân viên",
        KhachHang => "Khách hàng",
        _ => vaiTro ?? ""
    };
}

public static class TrangThaiSan
{
    public const string Trong = "Trong";
    public const string DangThue = "DangThue";
    public const string BaoTri = "BaoTri";

    public static readonly string[] TatCa = { Trong, DangThue, BaoTri };

    public static string TenHienThi(string trangThai) => trangThai switch
    {
        Trong => "Trống",
        DangThue => "Đang thuê",
        BaoTri => "Bảo trì",
        _ => trangThai ?? ""
    };
}

public static class TrangThaiDatSan
{
    public const string DaDat = "DaDat";
    public const string DangSuDung = "DangSuDung";
    public const string HoanThanh = "HoanThanh";
    public const string DaHuy = "DaHuy";

    public static readonly string[] TatCa = { DaDat, DangSuDung, HoanThanh, DaHuy };

    public static string TenHienThi(string trangThai) => trangThai switch
    {
        DaDat => "Đã đặt",
        DangSuDung => "Đang sử dụng",
        HoanThanh => "Hoàn thành",
        DaHuy => "Đã hủy",
        _ => trangThai ?? ""
    };
}

public static class TrangThaiHoaDon
{
    public const string ChuaThanhToan = "ChuaThanhToan";
    public const string DaThanhToan = "DaThanhToan";
    public const string DaHuy = "DaHuy";

    public static readonly string[] TatCa = { ChuaThanhToan, DaThanhToan, DaHuy };

    public static string TenHienThi(string trangThai) => trangThai switch
    {
        ChuaThanhToan => "Chưa thanh toán",
        DaThanhToan => "Đã thanh toán",
        DaHuy => "Đã hủy",
        _ => trangThai ?? ""
    };
}

public static class PhuongThucThanhToan
{
    public const string TienMat = "TienMat";
    public const string ChuyenKhoan = "ChuyenKhoan";

    public static readonly string[] TatCa = { TienMat, ChuyenKhoan };

    public static string TenHienThi(string phuongThuc) =>
        phuongThuc == ChuyenKhoan ? "Chuyển khoản" : "Tiền mặt";
}

/// <summary>Loại giảm giá của voucher: theo phần trăm hoặc số tiền cố định.</summary>
public static class LoaiGiam
{
    public const string PhanTram = "PhanTram";
    public const string SoTien = "SoTien";

    public static string TenHienThi(string loai) => loai == SoTien ? "Số tiền" : "Phần trăm";
}

/// <summary>Loại ưu đãi thực tế được áp dụng cho một hóa đơn (chỉ áp dụng tối đa 1 loại).</summary>
public static class LoaiGiamGia
{
    public const string Khong = "Khong";
    public const string Voucher = "Voucher";
    public const string KhuyenMai = "KhuyenMai";
    public const string CuoiTuan = "CuoiTuan";

    public static string TenHienThi(string loai) => loai switch
    {
        Voucher => "Voucher",
        KhuyenMai => "Khuyến mãi đặc biệt",
        CuoiTuan => "Giảm giá cuối tuần",
        _ => "Không giảm"
    };
}

public static class TrangThaiVoucher
{
    public const string HoatDong = "HoatDong";
    public const string TamNgung = "TamNgung";

    public static readonly string[] TatCa = { HoatDong, TamNgung };

    public static string TenHienThi(string trangThai) =>
        trangThai == HoatDong ? "Hoạt động" : "Tạm ngưng";
}

/// <summary>Khóa của bảng THAM_SO (cấu hình hệ thống).</summary>
public static class ThamSoKeys
{
    public const string PhanTramGiamCuoiTuan = "PhanTramGiamCuoiTuan";
    public const string ThoiLuongBlockPhut = "ThoiLuongBlockPhut";
    public const string GioMoCua = "GioMoCua";
    public const string GioDongCua = "GioDongCua";
    public const string TenTrungTam = "TenTrungTam";
    public const string DiaChi = "DiaChi";
    public const string DienThoai = "DienThoai";
    public const string LoiChaoHoaDon = "LoiChaoHoaDon";
}

/// <summary>
/// Danh sách mã quyền trong hệ thống. Phân quyền được kiểm tra ở tầng Service
/// (PhanQuyenService) chứ không chỉ ẩn nút trên giao diện.
/// </summary>
public static class MaQuyen
{
    public const string TkXem = "TK_XEM";
    public const string TkThem = "TK_THEM";
    public const string TkSua = "TK_SUA";
    public const string TkXoa = "TK_XOA";
    public const string TkKhoa = "TK_KHOA";
    public const string TkPhanQuyen = "TK_PHANQUYEN";
    public const string TkDoiMatKhau = "TK_DOIMATKHAU";

    public const string NvXem = "NV_XEM";
    public const string NvThem = "NV_THEM";
    public const string NvSua = "NV_SUA";
    public const string NvXoa = "NV_XOA";

    public const string LoaiSanXem = "LOAISAN_XEM";
    public const string LoaiSanThem = "LOAISAN_THEM";
    public const string LoaiSanSua = "LOAISAN_SUA";
    public const string LoaiSanXoa = "LOAISAN_XOA";

    public const string SanXem = "SAN_XEM";
    public const string SanThem = "SAN_THEM";
    public const string SanSua = "SAN_SUA";
    public const string SanXoa = "SAN_XOA";
    public const string SanDoiTrangThai = "SAN_DOITRANGTHAI";

    public const string KhXem = "KH_XEM";
    public const string KhXemTatCa = "KH_XEM_TATCA";
    public const string KhThem = "KH_THEM";
    public const string KhSua = "KH_SUA";
    public const string KhXoa = "KH_XOA";

    public const string DatSanXemTatCa = "DATSAN_XEM_TATCA";
    public const string DatSanXemCuaToi = "DATSAN_XEM_CUATOI";
    public const string DatSanThem = "DATSAN_THEM";
    public const string DatSanSua = "DATSAN_SUA";
    public const string DatSanHuy = "DATSAN_HUY";

    public const string LichXemTatCa = "LICH_XEM_TATCA";
    public const string LichXemCuaToi = "LICH_XEM_CUATOI";

    public const string HdXemTatCa = "HD_XEM_TATCA";
    public const string HdXemCuaToi = "HD_XEM_CUATOI";
    public const string HdLap = "HD_LAP";
    public const string HdSua = "HD_SUA";
    public const string HdXoa = "HD_XOA";
    public const string HdThanhToan = "HD_THANHTOAN";
    public const string HdIn = "HD_IN";

    public const string VoucherXem = "VOUCHER_XEM";
    public const string VoucherThem = "VOUCHER_THEM";
    public const string VoucherSua = "VOUCHER_SUA";
    public const string VoucherXoa = "VOUCHER_XOA";
    public const string VoucherApDung = "VOUCHER_APDUNG";
    public const string VoucherSuDung = "VOUCHER_SUDUNG";

    public const string KmXem = "KM_XEM";
    public const string KmThem = "KM_THEM";
    public const string KmSua = "KM_SUA";
    public const string KmXoa = "KM_XOA";
    public const string KmApDung = "KM_APDUNG";

    public const string CauHinhXem = "CAUHINH_XEM";
    public const string CauHinhSua = "CAUHINH_SUA";

    public const string ThongKeToanBo = "THONGTKE_TOANBO";
    public const string ThongKeNghiepVu = "THONGTKE_NGHIEPVU";
    public const string ThongKeCaNhan = "THONGTKE_CANHAN";
}
