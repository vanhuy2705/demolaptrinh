namespace SportFieldBooking.Data.Interfaces;

/// <summary>Truy xuất dữ liệu bảng VOUCHER.</summary>
public interface IVoucherRepository
{
    List<Voucher> LayTatCa(string tuKhoa = "", bool? chiConHan = null);
    Voucher LayTheoMa(int maVoucher);
    Voucher LayTheoMaCode(string maCode);
    bool TonTaiMaCode(string maCode, int? maVoucherLoaiTru = null);
    int Them(Voucher voucher);
    int CapNhat(Voucher voucher);
    int Xoa(int maVoucher);
    int TangSoLuongDaDung(int maVoucher, int soLuong = 1);
    int GiamSoLuongDaDung(int maVoucher, int soLuong = 1);
}
