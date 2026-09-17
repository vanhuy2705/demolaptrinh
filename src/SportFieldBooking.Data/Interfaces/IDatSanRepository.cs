namespace SportFieldBooking.Data.Interfaces;

/// <summary>Truy xuất dữ liệu bảng DAT_SAN.</summary>
public interface IDatSanRepository
{
    List<DatSan> LayTatCa(string tuKhoa = "");
    List<DatSan> LayTheoNgay(DateTime ngay, int? maSan = null);
    List<DatSan> LayTheoKhoang(DateTime tuNgay, DateTime denNgay, int? maSan = null, string trangThai = null);
    List<DatSan> LayTheoKhachHang(int maKH);
    List<DatSan> LaySapDienRa(int soLuong);
    DatSan LayTheoMa(int maDat);
    /// <summary>Tìm các booking cùng sân, cùng ngày bị trùng khoảng giờ (bỏ qua booking đã hủy).</summary>
    /// <param name="khoaBang">
    /// Đặt true khi gọi ngay trước lúc ghi (đặt/sửa sân) bên trong một giao dịch:
    /// câu SELECT sẽ dùng WITH (UPDLOCK, HOLDLOCK) để khoá dải bản ghi tới khi commit,
    /// nhờ đó hai yêu cầu đặt cùng khung giờ không thể cùng lọt qua bước kiểm tra.
    /// </param>
    List<DatSan> LayTrungLich(int maSan, DateTime ngay, TimeSpan gioBatDau, TimeSpan gioKetThuc,
        int? maDatLoaiTru = null, bool khoaBang = false);
    int Them(DatSan datSan);
    int CapNhat(DatSan datSan);
    int CapNhatTrangThai(int maDat, string trangThai);
    int Xoa(int maDat);
    int DemTheoTrangThai(string trangThai, DateTime? ngay = null);
}
