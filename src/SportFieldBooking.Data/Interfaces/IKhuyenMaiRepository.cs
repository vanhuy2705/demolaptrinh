namespace SportFieldBooking.Data.Interfaces;

/// <summary>Truy xuất dữ liệu bảng KHUYEN_MAI.</summary>
public interface IKhuyenMaiRepository
{
    List<KhuyenMai> LayTatCa(string tuKhoa = "");
    KhuyenMai LayTheoMa(int maKM);
    /// <summary>Các chương trình còn hiệu lực tại ngày đặt (có xét cờ cuối tuần).</summary>
    List<KhuyenMai> LayDangApDung(DateTime ngay, bool laCuoiTuan);
    bool TonTaiTen(string tenKM, int? maKMLoaiTru = null);
    int Them(KhuyenMai khuyenMai);
    int CapNhat(KhuyenMai khuyenMai);
    int Xoa(int maKM);
}
