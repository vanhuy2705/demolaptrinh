namespace SportFieldBooking.Data.Interfaces;

/// <summary>Truy xuất dữ liệu bảng SAN.</summary>
public interface ISanRepository
{
    List<San> LayTatCa(string tuKhoa = "", int? maLoaiSan = null, string trangThai = null);
    San LayTheoMa(int maSan);
    bool TonTaiTen(string tenSan, int? maSanLoaiTru = null);
    int Them(San san);
    int CapNhat(San san);
    int CapNhatTrangThai(int maSan, string trangThai);
    int Xoa(int maSan);
    int DemDatSan(int maSan);
}
