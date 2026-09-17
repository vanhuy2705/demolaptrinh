namespace SportFieldBooking.Data.Interfaces;

/// <summary>Truy xuất dữ liệu bảng SU_DUNG_VOUCHER.</summary>
public interface ISuDungVoucherRepository
{
    int Them(SuDungVoucher suDung);
    List<SuDungVoucher> LayTheoKhachHang(int maKH);
    List<SuDungVoucher> LayTheoMaDat(int maDat);
    bool DaDungChoBooking(int maVoucher, int maDat);
    int XoaTheoMaDat(int maDat);
}
