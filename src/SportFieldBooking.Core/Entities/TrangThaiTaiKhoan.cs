namespace SportFieldBooking.Core.Entities;

/// <summary>Trạng thái của tài khoản đăng nhập.</summary>
public static class TrangThaiTaiKhoan
{
    public const string HoatDong = "HoatDong";
    public const string BiKhoa = "BiKhoa";

    public static readonly string[] TatCa = { HoatDong, BiKhoa };

    public static string TenHienThi(string trangThai) =>
        trangThai == BiKhoa ? "Bị khóa" : "Hoạt động";
}
