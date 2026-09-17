using System.Data;

namespace SportFieldBooking.Data.Helpers;

/// <summary>Hàm đọc dữ liệu an toàn từ DataRow (xử lý DBNull gọn gàng).</summary>
public static class DataRowExtensions
{
    public static bool CoCot(this DataRow dong, string tenCot) =>
        dong.Table.Columns.Contains(tenCot);

    public static string Chuoi(this DataRow dong, string tenCot) =>
        dong.CoCot(tenCot) && dong[tenCot] != DBNull.Value ? Convert.ToString(dong[tenCot]) : "";

    public static int SoNguyen(this DataRow dong, string tenCot) =>
        dong.CoCot(tenCot) && dong[tenCot] != DBNull.Value ? Convert.ToInt32(dong[tenCot]) : 0;

    public static decimal SoThapPhan(this DataRow dong, string tenCot) =>
        dong.CoCot(tenCot) && dong[tenCot] != DBNull.Value ? Convert.ToDecimal(dong[tenCot]) : 0m;

    public static bool Bit(this DataRow dong, string tenCot) =>
        dong.CoCot(tenCot) && dong[tenCot] != DBNull.Value && Convert.ToBoolean(dong[tenCot]);

    public static DateTime NgayGio(this DataRow dong, string tenCot) =>
        dong.CoCot(tenCot) && dong[tenCot] != DBNull.Value ? Convert.ToDateTime(dong[tenCot]) : DateTime.MinValue;

    public static TimeSpan Gio(this DataRow dong, string tenCot)
    {
        if (!dong.CoCot(tenCot) || dong[tenCot] == DBNull.Value) return TimeSpan.Zero;
        if (dong[tenCot] is TimeSpan ts) return ts;
        // Dữ liệu lạ (chuỗi sai định dạng...): trả 0 giờ thay vì ném FormatException.
        return TimeSpan.TryParse(Convert.ToString(dong[tenCot]), out TimeSpan gio) ? gio : TimeSpan.Zero;
    }

    public static int? SoNguyenCoTheNull(this DataRow dong, string tenCot) =>
        dong.CoCot(tenCot) && dong[tenCot] != DBNull.Value ? Convert.ToInt32(dong[tenCot]) : null;

    public static DateTime? NgayGioCoTheNull(this DataRow dong, string tenCot) =>
        dong.CoCot(tenCot) && dong[tenCot] != DBNull.Value ? Convert.ToDateTime(dong[tenCot]) : null;
}
