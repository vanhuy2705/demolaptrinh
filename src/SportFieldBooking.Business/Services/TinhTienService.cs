using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

/// <summary>
/// Quy tắc tính tiền:
/// - Giá thuê cố định theo sân/loại sân (không chia sáng/chiều/tối).
/// - Thời gian vượt giờ tính theo block (mặc định 30 phút): 17:00-18:10 =&gt; 70 phút =&gt; 3 block =&gt; 1,5 giờ.
/// - Một booking/hóa đơn chỉ áp dụng tối đa một loại giảm giá.
/// - Ưu tiên: voucher hợp lệ &gt; khuyến mãi dịp đặc biệt &gt; giảm cuối tuần &gt; giá gốc.
/// </summary>
public class TinhTienService
{
    private readonly ISanRepository _sanRepo;
    private readonly IThamSoRepository _thamSoRepo;
    private readonly VoucherService _voucherService;
    private readonly KhuyenMaiService _khuyenMaiService;

    public TinhTienService()
        : this(new Data.Repositories.SanRepository(), new Data.Repositories.ThamSoRepository(),
               new VoucherService(), new KhuyenMaiService())
    {
    }

    public TinhTienService(ISanRepository sanRepo, IThamSoRepository thamSoRepo,
        VoucherService voucherService, KhuyenMaiService khuyenMaiService)
    {
        _sanRepo = sanRepo;
        _thamSoRepo = thamSoRepo;
        _voucherService = voucherService;
        _khuyenMaiService = khuyenMaiService;
    }

    /// <summary>Số phút của một block tính tiền (cấu hình THAM_SO, mặc định 30).</summary>
    public int LayThoiLuongBlockPhut()
    {
        string giaTri = _thamSoRepo.GiaTri(ThamSoKeys.ThoiLuongBlockPhut, "30");
        return int.TryParse(giaTri, out int soPhut) && soPhut > 0 ? soPhut : 30;
    }

    /// <summary>Mức giảm cuối tuần (%).</summary>
    public decimal LayPhanTramGiamCuoiTuan()
    {
        string giaTri = _thamSoRepo.GiaTri(ThamSoKeys.PhanTramGiamCuoiTuan, "10");
        return decimal.TryParse(giaTri, out decimal phanTram) ? Math.Max(0, Math.Min(100, phanTram)) : 10m;
    }

    /// <summary>Số block làm tròn lên: 70 phút / 30 = 3 block.</summary>
    public static int TinhSoBlock(TimeSpan gioBatDau, TimeSpan gioKetThuc, int thoiLuongBlockPhut)
    {
        double soPhut = (gioKetThuc - gioBatDau).TotalMinutes;
        if (soPhut <= 0) return 0;
        if (thoiLuongBlockPhut <= 0) thoiLuongBlockPhut = 30;
        return (int)Math.Ceiling(soPhut / thoiLuongBlockPhut);
    }

    /// <summary>Tiền sân = đơn giá x số block x (phút block / 60).</summary>
    public static decimal TinhTienTheoBlock(decimal donGia, int soBlock, int thoiLuongBlockPhut)
    {
        if (soBlock <= 0 || donGia <= 0) return 0m;
        return Math.Round(donGia * soBlock * thoiLuongBlockPhut / 60m, 0);
    }

    /// <summary>
    /// Tính tiền trọn gói cho một booking: tiền gốc + áp dụng đúng thứ tự ưu tiên giảm giá.
    /// </summary>
    public KetQua<ChiTietTien> TinhTien(int maSan, DateTime ngayDat, TimeSpan gioBatDau, TimeSpan gioKetThuc,
        string maVoucher = "", int? maKH = null)
    {
        try
        {
            San san = _sanRepo.LayTheoMa(maSan);
            if (san == null) return KetQua<ChiTietTien>.Loi("Không tìm thấy sân để tính tiền.");

            int phutBlock = LayThoiLuongBlockPhut();
            int soBlock = TinhSoBlock(gioBatDau, gioKetThuc, phutBlock);
            if (soBlock <= 0)
                return KetQua<ChiTietTien>.Loi("Giờ kết thúc phải sau giờ bắt đầu.");

            decimal tienGoc = TinhTienTheoBlock(san.DonGia, soBlock, phutBlock);

            var chiTiet = new ChiTietTien
            {
                MaSan = san.MaSan,
                TenSan = san.TenSan,
                DonGia = san.DonGia,
                ThoiLuongBlockPhut = phutBlock,
                SoBlock = soBlock,
                SoGio = soBlock * phutBlock / 60m,
                NgayDat = ngayDat.Date,
                GioBatDau = gioBatDau,
                GioKetThuc = gioKetThuc,
                TienGoc = tienGoc,
                TongTien = tienGoc
            };

            // 1) Voucher hợp lệ và đủ điều kiện
            if (!string.IsNullOrWhiteSpace(maVoucher))
            {
                KetQua<Voucher> kiemTraVoucher = _voucherService.KiemTraVoucher(maVoucher, tienGoc, maKH);
                if (!kiemTraVoucher.ThanhCong)
                    return KetQua<ChiTietTien>.Loi(kiemTraVoucher.ThongBao);

                chiTiet.VoucherDuocDung = kiemTraVoucher.DuLieu;
                chiTiet.LoaiGiamGia = LoaiGiamGia.Voucher;
                chiTiet.TenUuDai = $"{kiemTraVoucher.DuLieu.TenVoucher} ({kiemTraVoucher.DuLieu.MaCode})";
                chiTiet.PhanTramGiam = kiemTraVoucher.DuLieu.LoaiGiam == LoaiGiam.PhanTram
                    ? kiemTraVoucher.DuLieu.GiaTriGiam
                    : 0m;
                chiTiet.TienGiam = _voucherService.TinhTienGiam(kiemTraVoucher.DuLieu, tienGoc);
                chiTiet.TongTien = tienGoc - chiTiet.TienGiam;
                return KetQua<ChiTietTien>.Tot(chiTiet, "Đã áp dụng voucher.");
            }

            // 2) Khuyến mãi dịp đặc biệt còn hiệu lực tại ngày đặt
            KhuyenMai khuyenMai = _khuyenMaiService.LayKhuyenMaiTotNhat(ngayDat);
            if (khuyenMai != null)
            {
                chiTiet.KhuyenMaiDuocDung = khuyenMai;
                chiTiet.LoaiGiamGia = LoaiGiamGia.KhuyenMai;
                chiTiet.TenUuDai = khuyenMai.TenKM;
                chiTiet.PhanTramGiam = khuyenMai.PhanTramGiam;
                chiTiet.TienGiam = Math.Round(tienGoc * khuyenMai.PhanTramGiam / 100m, 0);
                chiTiet.TongTien = tienGoc - chiTiet.TienGiam;
                return KetQua<ChiTietTien>.Tot(chiTiet, "Đã áp dụng khuyến mãi đặc biệt.");
            }

            // 3) Giảm giá cuối tuần (Thứ Bảy, Chủ Nhật)
            if (KhuyenMaiService.LaCuoiTuan(ngayDat))
            {
                decimal phanTram = LayPhanTramGiamCuoiTuan();
                if (phanTram > 0)
                {
                    chiTiet.LoaiGiamGia = LoaiGiamGia.CuoiTuan;
                    chiTiet.TenUuDai = $"Giảm giá cuối tuần ({phanTram:0.##}%)";
                    chiTiet.PhanTramGiam = phanTram;
                    chiTiet.TienGiam = Math.Round(tienGoc * phanTram / 100m, 0);
                    chiTiet.TongTien = tienGoc - chiTiet.TienGiam;
                    return KetQua<ChiTietTien>.Tot(chiTiet, "Đã áp dụng giảm giá cuối tuần.");
                }
            }

            // 4) Giá gốc
            chiTiet.LoaiGiamGia = LoaiGiamGia.Khong;
            chiTiet.TenUuDai = "Không áp dụng ưu đãi";
            return KetQua<ChiTietTien>.Tot(chiTiet, "Tính tiền theo giá gốc.");
        }
        catch (Exception ex)
        {
            return KetQua<ChiTietTien>.Loi("Không thể tính tiền: " + ex.Message);
        }
    }

    /// <summary>Tính tiền cho một booking đã lưu (dùng khi lập/sửa hóa đơn).</summary>
    public KetQua<ChiTietTien> TinhTienChoBooking(DatSan datSan, string maVoucher = "") =>
        TinhTien(datSan?.MaSan ?? 0, datSan?.NgayDat ?? DateTime.Today,
            datSan?.GioBatDau ?? TimeSpan.Zero, datSan?.GioKetThuc ?? TimeSpan.Zero, maVoucher, datSan?.MaKH);
}
