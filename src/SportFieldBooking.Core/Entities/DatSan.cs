namespace SportFieldBooking.Core.Entities;

/// <summary>Bảng DAT_SAN: lượt đặt sân của khách hàng.</summary>
public class DatSan
{
    public int MaDat { get; set; }
    public int MaKH { get; set; }
    public int MaSan { get; set; }
    public DateTime NgayDat { get; set; }
    public TimeSpan GioBatDau { get; set; }
    public TimeSpan GioKetThuc { get; set; }
    /// <summary>Tiền sân (chưa giảm), tính theo block 30 phút ở tầng nghiệp vụ.</summary>
    public decimal TienSan { get; set; }
    /// <summary>DaDat | DangSuDung | HoanThanh | DaHuy (xem lớp TrangThaiDatSan).</summary>
    public string TrangThai { get; set; } = Core.Common.TrangThaiDatSan.DaDat;
    public string GhiChu { get; set; } = "";
    public DateTime NgayTao { get; set; }
    public int? MaNguoiTao { get; set; }

    /// <summary>
    /// Voucher khách chọn lúc đặt sân (FK -> VOUCHER). Trước đây giá trị này bị mất:
    /// booking chỉ lưu giá gốc nên sang bước lập hóa đơn phải chọn lại voucher.
    /// </summary>
    public int? MaVoucher { get; set; }

    // --- Thuộc tính hiển thị (join từ bảng khác, không ánh xạ trực tiếp) ---
    /// <summary>Mã voucher dạng chuỗi (VD: "KM10"), để trống nếu không dùng.</summary>
    public string MaVoucherCode { get; set; } = "";
    public string TenKH { get; set; } = "";
    public string SDT { get; set; } = "";
    public string TenSan { get; set; } = "";
    public string TenLoaiSan { get; set; } = "";
    public decimal DonGia { get; set; }
}

/// <summary>Dòng dữ liệu hiển thị trên lịch đặt sân (gộp thông tin khách + sân).</summary>
public class LichDatItem
{
    public int MaDat { get; set; }
    public string TenSan { get; set; } = "";
    public string TenKH { get; set; } = "";
    public string SDT { get; set; } = "";
    public DateTime NgayDat { get; set; }
    public TimeSpan GioBatDau { get; set; }
    public TimeSpan GioKetThuc { get; set; }
    public decimal TienSan { get; set; }
    public string TrangThai { get; set; } = "";
    public decimal TongTien { get; set; }
    public string TrangThaiHoaDon { get; set; } = "";

    public string KhungGio => $"{GioBatDau:hh\\:mm} - {GioKetThuc:hh\\:mm}";
}
