namespace SportFieldBooking.Data.Interfaces;

/// <summary>Truy xuất dữ liệu bảng LOAI_SAN.</summary>
public interface ILoaiSanRepository
{
    List<LoaiSan> LayTatCa(string tuKhoa = "");
    LoaiSan LayTheoMa(int maLoaiSan);
    bool TonTaiTen(string tenLoaiSan, int? maLoaiTru = null);
    int Them(LoaiSan loaiSan);
    int CapNhat(LoaiSan loaiSan);
    int Xoa(int maLoaiSan);
    int DemSoSanSuDung(int maLoaiSan);
}
