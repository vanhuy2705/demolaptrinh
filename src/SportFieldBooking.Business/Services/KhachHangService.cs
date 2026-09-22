using System.Text.RegularExpressions;
using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

/// <summary>Nghiệp vụ quản lý khách hàng (SDT duy nhất).</summary>
public class KhachHangService
{
    private readonly IKhachHangRepository _khachHangRepo;

    public KhachHangService() : this(new Data.Repositories.KhachHangRepository())
    {
    }

    public KhachHangService(IKhachHangRepository khachHangRepo) => _khachHangRepo = khachHangRepo;

    private static readonly Regex MauSDT = new(@"^0\d{9,10}$", RegexOptions.Compiled);
    private static readonly Regex MauEmail = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public List<KhachHang> LayTatCa(string tuKhoa = "") => _khachHangRepo.LayTatCa(tuKhoa);

    public KhachHang LayTheoMa(int maKH) => _khachHangRepo.LayTheoMa(maKH);

    public KhachHang LayTheoSDT(string sdt) => _khachHangRepo.LayTheoSDT(sdt);

    public KhachHang LayTheoMaTK(int maTK) => _khachHangRepo.LayTheoMaTK(maTK);

    public KetQua<KhachHang> Them(KhachHang khachHang)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.KhThem))
                return KetQua<KhachHang>.Loi("Bạn không có quyền thêm khách hàng.");

            KetQua hopLe = KiemTraDuLieu(khachHang);
            if (!hopLe.ThanhCong) return KetQua<KhachHang>.Loi(hopLe.ThongBao);
            if (_khachHangRepo.TonTaiSDT(khachHang.SDT.Trim()))
                return KetQua<KhachHang>.Loi("Số điện thoại đã tồn tại trong hệ thống.");

            khachHang.HoTen = khachHang.HoTen.Trim();
            khachHang.SDT = khachHang.SDT.Trim();
            khachHang.MaKH = _khachHangRepo.Them(khachHang);
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "ThemKhachHang", "KHACH_HANG", khachHang.MaKH.ToString(), $"Thêm KH {khachHang.HoTen}"); } catch { }
            return KetQua<KhachHang>.Tot(khachHang, "Thêm khách hàng thành công.");
        }
        catch (Exception ex)
        {
            return KetQua<KhachHang>.Loi("Không thể thêm khách hàng: " + ex.Message);
        }
    }

    public KetQua CapNhat(KhachHang khachHang, bool laTuChinhSua = false)
    {
        try
        {
            // Khách hàng tự sửa thông tin của mình thì không cần quyền quản lý khách hàng.
            if (!laTuChinhSua && !PhanQuyenService.CoQuyen(MaQuyen.KhSua))
                return KetQua.Loi("Bạn không có quyền sửa thông tin khách hàng.");
            if (laTuChinhSua && PhienLamViec.MaKH != null && khachHang.MaKH != PhienLamViec.MaKH)
                return KetQua.Loi("Bạn chỉ được sửa thông tin của chính mình.");

            KetQua hopLe = KiemTraDuLieu(khachHang);
            if (!hopLe.ThanhCong) return hopLe;
            if (khachHang.MaKH <= 0) return KetQua.Loi("Khách hàng không hợp lệ.");
            if (_khachHangRepo.TonTaiSDT(khachHang.SDT.Trim(), khachHang.MaKH))
                return KetQua.Loi("Số điện thoại đã được dùng bởi khách hàng khác.");

            _khachHangRepo.CapNhat(khachHang);
            return KetQua.Tot("Cập nhật thông tin khách hàng thành công.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể cập nhật khách hàng: " + ex.Message);
        }
    }

    public KetQua Xoa(int maKH)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.KhXoa))
                return KetQua.Loi("Bạn không có quyền xóa khách hàng.");

            _khachHangRepo.Xoa(maKH);
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "XoaKhachHang", "KHACH_HANG", maKH.ToString(), $"Xóa KH #{maKH}"); } catch { }
            return KetQua.Tot("Xóa khách hàng thành công.");
        }
        catch (Exception ex)
        {
            string thongBao = ex.Message.Contains("FK__DAT_SAN") || ex.Message.Contains("REFERENCE")
                ? "Không thể xóa: khách hàng đã có lịch đặt sân."
                : "Không thể xóa khách hàng: " + ex.Message;
            return KetQua.Loi(thongBao);
        }
    }

    private static KetQua KiemTraDuLieu(KhachHang khachHang)
    {
        if (khachHang == null) return KetQua.Loi("Dữ liệu khách hàng không hợp lệ.");
        if (string.IsNullOrWhiteSpace(khachHang.HoTen)) return KetQua.Loi("Vui lòng nhập họ tên khách hàng.");
        if (string.IsNullOrWhiteSpace(khachHang.SDT) || !MauSDT.IsMatch(khachHang.SDT.Trim()))
            return KetQua.Loi("Số điện thoại không hợp lệ (bắt đầu bằng 0, 10-11 chữ số).");
        if (!string.IsNullOrWhiteSpace(khachHang.Email) && !MauEmail.IsMatch(khachHang.Email.Trim()))
            return KetQua.Loi("Email không đúng định dạng.");
        return KetQua.Tot();
    }
}
