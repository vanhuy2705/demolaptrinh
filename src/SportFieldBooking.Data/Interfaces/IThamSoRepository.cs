namespace SportFieldBooking.Data.Interfaces;

/// <summary>Truy xuất dữ liệu bảng THAM_SO (cấu hình hệ thống).</summary>
public interface IThamSoRepository
{
    List<ThamSo> LayTatCa();
    ThamSo LayTheoTen(string tenThamSo);
    string GiaTri(string tenThamSo, string macDinh = "");
    int CapNhat(string tenThamSo, string giaTri);
    int Them(ThamSo thamSo);
}
