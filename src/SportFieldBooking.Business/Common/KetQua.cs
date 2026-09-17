namespace SportFieldBooking.Business.Common;

/// <summary>Kết quả trả về của các hàm nghiệp vụ (thành công/thất bại + thông báo).</summary>
public class KetQua
{
    public bool ThanhCong { get; set; }
    public string ThongBao { get; set; } = "";

    public static KetQua Tot(string thongBao = "Thực hiện thành công.") =>
        new() { ThanhCong = true, ThongBao = thongBao };

    public static KetQua Loi(string thongBao) =>
        new() { ThanhCong = false, ThongBao = thongBao };
}

/// <summary>Kết quả trả về có kèm dữ liệu.</summary>
public class KetQua<T> : KetQua
{
    public T DuLieu { get; set; }

    public static KetQua<T> Tot(T duLieu, string thongBao = "Thực hiện thành công.") =>
        new() { ThanhCong = true, ThongBao = thongBao, DuLieu = duLieu };

    /// <summary>Ẩn có chủ đích phương thức cùng tên của lớp cha để trả về kiểu KetQua&lt;T&gt;.</summary>
    public new static KetQua<T> Loi(string thongBao) =>
        new() { ThanhCong = false, ThongBao = thongBao };
}
