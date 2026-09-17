using System.Globalization;
using System.Text.RegularExpressions;

namespace SportFieldBooking.WinForms.Helpers;

/// <summary>Các hàm kiểm tra dữ liệu nhập và tiện ích giao diện dùng chung.</summary>
public static class TroGiup
{
    private static readonly Regex MauSoDienThoai = new(@"^0\d{9,10}$", RegexOptions.Compiled);
    private static readonly Regex MauEmail = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    /// <summary>Báo lỗi nếu ô bị bỏ trống.</summary>
    public static bool Rong(Control o, string tenTruong, ErrorProvider loi)
    {
        if (o == null) return true;
        if (!string.IsNullOrWhiteSpace(o.Text))
        {
            loi?.SetError(o, "");
            return false;
        }
        loi?.SetError(o, $"Vui lòng nhập {tenTruong}.");
        return true;
    }

    /// <summary>Báo lỗi theo điều kiện tự do.</summary>
    public static bool Sai(bool dieuKienLoi, Control o, string thongBao, ErrorProvider loi)
    {
        if (o == null) return true;
        if (!dieuKienLoi)
        {
            loi?.SetError(o, "");
            return false;
        }
        loi?.SetError(o, thongBao);
        return true;
    }

    public static bool SaiDienThoai(Control o, ErrorProvider loi, bool batBuoc = true)
    {
        string giaTri = o.Text.Trim();
        if (giaTri.Length == 0 && !batBuoc) { loi?.SetError(o, ""); return false; }
        return Sai(!MauSoDienThoai.IsMatch(giaTri), o, "Số điện thoại phải bắt đầu bằng 0 và có 10-11 chữ số.", loi);
    }

    public static bool SaiEmail(Control o, ErrorProvider loi, bool batBuoc = false)
    {
        string giaTri = o.Text.Trim();
        if (giaTri.Length == 0 && !batBuoc) { loi?.SetError(o, ""); return false; }
        return Sai(!MauEmail.IsMatch(giaTri), o, "Email không đúng định dạng (vd: ten@gmail.com).", loi);
    }

    /// <summary>Đọc số tiền từ ô nhập, báo lỗi nếu không hợp lệ.</summary>
    public static bool SaiTien(Control o, string tenTruong, ErrorProvider loi, out decimal ketQua, bool choPhepAm = false)
    {
        ketQua = 0m;
        string chuoi = o.Text.Trim().Replace(",", "").Replace(".", "").Replace("đ", "").Trim();
        if (chuoi.Length == 0)
            return Sai(true, o, $"Vui lòng nhập {tenTruong}.", loi);
        if (!decimal.TryParse(chuoi, NumberStyles.Any, CultureInfo.InvariantCulture, out ketQua))
            return Sai(true, o, $"{tenTruong} phải là số.", loi);
        if (!choPhepAm && ketQua <= 0)
            return Sai(true, o, $"{tenTruong} phải lớn hơn 0.", loi);
        loi?.SetError(o, "");
        return false;
    }

    public static bool SaiSoNguyen(Control o, string tenTruong, ErrorProvider loi, out int ketQua, int nhoNhat = 0)
    {
        ketQua = 0;
        string chuoi = o.Text.Trim().Replace(",", "").Replace(".", "");
        if (chuoi.Length == 0 || !int.TryParse(chuoi, out ketQua))
            return Sai(true, o, $"{tenTruong} phải là số nguyên.", loi);
        if (ketQua < nhoNhat)
            return Sai(true, o, $"{tenTruong} không được nhỏ hơn {nhoNhat}.", loi);
        loi?.SetError(o, "");
        return false;
    }

    public static void XoaLoi(ErrorProvider loi, params Control[] danhSach)
    {
        foreach (Control o in danhSach)
            loi?.SetError(o, "");
    }

    public static void XoaTatCaLoi(Control cha, ErrorProvider loi)
    {
        loi?.Clear();
    }

    public static string Tien(decimal soTien) => soTien.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";

    public static TimeSpan LayGio(DateTimePicker chonNgayGio) => chonNgayGio.Value.TimeOfDay;

    public static void DatGio(DateTimePicker chonNgayGio, TimeSpan gio) =>
        chonNgayGio.Value = DateTime.Today.Add(gio);

    /// <summary>Gán danh sách vào ComboBox (hiển thị/ giá trị).</summary>
    public static void GanComboBox<T>(ComboBox combo, IEnumerable<T> danhSach, string hienThi, string giaTri,
        string mucDauTien = "")
    {
        if (combo == null) return;
        combo.DataSource = null;
        combo.Items.Clear();

        var nguon = danhSach?.ToList() ?? new List<T>();
        if (!string.IsNullOrEmpty(mucDauTien))
        {
            combo.DataSource = nguon;
            combo.DisplayMember = hienThi;
            combo.ValueMember = giaTri;
            combo.SelectedIndex = -1;
            combo.Text = mucDauTien;
            return;
        }

        combo.DataSource = nguon;
        combo.DisplayMember = hienThi;
        combo.ValueMember = giaTri;
        combo.SelectedIndex = nguon.Count > 0 ? 0 : -1;
    }

    /// <summary>
    /// Lấy khóa của phần tử đang chọn. ComboBox WinForms đôi khi trả SelectedValue
    /// là chính object DataSource trong lúc binding; tuyệt đối không Convert object
    /// nghiệp vụ (ví dụ San) sang IConvertible.
    /// </summary>
    public static int LayGiaTriComboBox(ComboBox combo)
    {
        if (combo == null || combo.SelectedIndex < 0) return 0;

        object value = combo.SelectedValue;
        if (value != null && value != DBNull.Value)
        {
            if (value is int i) return i;
            if (value is long l && l >= int.MinValue && l <= int.MaxValue) return (int)l;
            if (int.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), out int parsed))
                return parsed;
        }

        // Fallback an toàn khi SelectedValue đang là object DataSource.
        object item = combo.SelectedItem;
        if (item == null) return 0;

        var property = item.GetType().GetProperty(combo.ValueMember ?? string.Empty);
        if (property == null) return 0;

        object raw = property.GetValue(item);
        return raw == null ? 0 : int.TryParse(Convert.ToString(raw, CultureInfo.InvariantCulture), out int result) ? result : 0;
    }
}
