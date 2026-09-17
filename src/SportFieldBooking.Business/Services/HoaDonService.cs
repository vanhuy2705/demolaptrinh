using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

/// <summary>
/// Nghiệp vụ hóa đơn: lập hóa đơn, tính lại tiền, thanh toán (cập nhật hóa đơn,
/// trạng thái booking, trạng thái sân và lượt sử dụng voucher trong một giao dịch).
/// </summary>
public class HoaDonService
{
    private readonly IHoaDonRepository _hoaDonRepo;
    private readonly IDatSanRepository _datSanRepo;
    private readonly ISanRepository _sanRepo;
    private readonly IVoucherRepository _voucherRepo;
    private readonly ISuDungVoucherRepository _suDungRepo;
    private readonly TinhTienService _tinhTienService;

    public HoaDonService()
        : this(new Data.Repositories.HoaDonRepository(), new Data.Repositories.DatSanRepository(),
               new Data.Repositories.SanRepository(), new Data.Repositories.VoucherRepository(),
               new Data.Repositories.SuDungVoucherRepository(), new TinhTienService())
    {
    }

    public HoaDonService(IHoaDonRepository hoaDonRepo, IDatSanRepository datSanRepo, ISanRepository sanRepo,
        IVoucherRepository voucherRepo, ISuDungVoucherRepository suDungRepo, TinhTienService tinhTienService)
    {
        _hoaDonRepo = hoaDonRepo;
        _datSanRepo = datSanRepo;
        _sanRepo = sanRepo;
        _voucherRepo = voucherRepo;
        _suDungRepo = suDungRepo;
        _tinhTienService = tinhTienService;
    }

    public List<HoaDon> LayTatCa(string tuKhoa = "") => _hoaDonRepo.LayTatCa(tuKhoa);

    public List<HoaDon> LayTheoKhoang(DateTime tuNgay, DateTime denNgay, string trangThai = null) =>
        _hoaDonRepo.LayTheoKhoang(tuNgay, denNgay, trangThai);

    public List<HoaDon> LayTheoKhachHang(int maKH) => _hoaDonRepo.LayTheoKhachHang(maKH);

    public HoaDon LayTheoMa(int maHD) => _hoaDonRepo.LayTheoMa(maHD);

    public HoaDon LayTheoMaDat(int maDat) => _hoaDonRepo.LayTheoMaDat(maDat);

    /// <summary>
    /// Lập (hoặc tính lại) hóa đơn cho một booking. Chỉ áp dụng tối đa một loại giảm giá.
    /// </summary>
    public KetQua<HoaDon> LapHoaDon(int maDat, string maVoucher = "",
        string phuongThuc = PhuongThucThanhToan.TienMat, bool tinhLai = false)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.HdLap))
                return KetQua<HoaDon>.Loi("Bạn không có quyền lập hóa đơn.");

            DatSan datSan = _datSanRepo.LayTheoMa(maDat);
            if (datSan == null) return KetQua<HoaDon>.Loi("Không tìm thấy booking để lập hóa đơn.");
            if (datSan.TrangThai == TrangThaiDatSan.DaHuy)
                return KetQua<HoaDon>.Loi("Booking đã hủy, không thể lập hóa đơn.");

            HoaDon hoaDonCu = _hoaDonRepo.LayTheoMaDat(maDat);
            if (hoaDonCu != null && hoaDonCu.TrangThai == TrangThaiHoaDon.DaThanhToan && !tinhLai)
                return KetQua<HoaDon>.Tot(hoaDonCu, "Booking này đã có hóa đơn thanh toán.");

            KetQua<ChiTietTien> ketQuaTien = _tinhTienService.TinhTienChoBooking(datSan, maVoucher);
            if (!ketQuaTien.ThanhCong) return KetQua<HoaDon>.Loi(ketQuaTien.ThongBao);
            ChiTietTien tien = ketQuaTien.DuLieu;

            if (hoaDonCu != null)
            {
                hoaDonCu.TienGoc = tien.TienGoc;
                hoaDonCu.LoaiGiamGia = tien.LoaiGiamGia;
                hoaDonCu.TienGiam = tien.TienGiam;
                hoaDonCu.TongTien = tien.TongTien;
                hoaDonCu.MaVoucher = tien.VoucherDuocDung?.MaVoucher;
                hoaDonCu.PhuongThucThanhToan = phuongThuc;
                _hoaDonRepo.CapNhat(hoaDonCu);
                return KetQua<HoaDon>.Tot(hoaDonCu, "Đã cập nhật lại hóa đơn.");
            }

            var hoaDon = new HoaDon
            {
                MaDat = maDat,
                NgayLap = DateTime.Now,
                TienGoc = tien.TienGoc,
                LoaiGiamGia = tien.LoaiGiamGia,
                TienGiam = tien.TienGiam,
                TongTien = tien.TongTien,
                PhuongThucThanhToan = phuongThuc,
                TrangThai = TrangThaiHoaDon.ChuaThanhToan,
                MaNguoiLap = PhienLamViec.MaTK,
                MaVoucher = tien.VoucherDuocDung?.MaVoucher,
                GhiChu = tien.TenUuDai
            };

            hoaDon.MaHD = _hoaDonRepo.Them(hoaDon);
            return KetQua<HoaDon>.Tot(hoaDon, "Lập hóa đơn thành công.");
        }
        catch (Exception ex)
        {
            return KetQua<HoaDon>.Loi("Không thể lập hóa đơn: " + ex.Message);
        }
    }

    /// <summary>
    /// Thanh toán hóa đơn: cập nhật trạng thái hóa đơn, trạng thái booking (Hoàn thành),
    /// trạng thái sân (Trống) và lượt sử dụng voucher - tất cả trong một giao dịch.
    /// </summary>
    public KetQua<HoaDon> ThanhToan(int maHD, string phuongThuc, int? maNguoiLap = null)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.HdThanhToan))
                return KetQua<HoaDon>.Loi("Bạn không có quyền thu tiền.");
            if (phuongThuc != PhuongThucThanhToan.TienMat && phuongThuc != PhuongThucThanhToan.ChuyenKhoan)
                return KetQua<HoaDon>.Loi("Phương thức thanh toán không hợp lệ.");

            HoaDon hoaDon = _hoaDonRepo.LayTheoMa(maHD);
            if (hoaDon == null) return KetQua<HoaDon>.Loi("Hóa đơn không tồn tại.");
            if (hoaDon.TrangThai == TrangThaiHoaDon.DaThanhToan)
                return KetQua<HoaDon>.Loi("Hóa đơn này đã được thanh toán.");
            if (hoaDon.TrangThai == TrangThaiHoaDon.DaHuy)
                return KetQua<HoaDon>.Loi("Hóa đơn đã hủy, không thể thanh toán.");

            DatSan datSan = _datSanRepo.LayTheoMa(hoaDon.MaDat);
            if (datSan == null) return KetQua<HoaDon>.Loi("Không tìm thấy booking của hóa đơn này.");

            DbHelper.ChayGiaoDich(() =>
            {
                // Ghi nhận lượt sử dụng voucher (chỉ 1 lần cho mỗi booking).
                if (hoaDon.MaVoucher != null)
                {
                    Voucher voucher = _voucherRepo.LayTheoMa(hoaDon.MaVoucher.Value);
                    if (voucher == null) throw new InvalidOperationException("Voucher của hóa đơn không còn tồn tại.");

                    if (!_suDungRepo.DaDungChoBooking(voucher.MaVoucher, hoaDon.MaDat))
                    {
                        KetQua<Voucher> hopLe = new VoucherService(_voucherRepo, _suDungRepo)
                            .KiemTraVoucher(voucher.MaCode, hoaDon.TienGoc, datSan.MaKH);
                        if (!hopLe.ThanhCong)
                            throw new InvalidOperationException(hopLe.ThongBao);

                        _suDungRepo.Them(new SuDungVoucher
                        {
                            MaVoucher = voucher.MaVoucher,
                            MaDat = hoaDon.MaDat,
                            MaKH = datSan.MaKH,
                            SoTienGiam = hoaDon.TienGiam
                        });

                        if (_voucherRepo.TangSoLuongDaDung(voucher.MaVoucher) == 0)
                            throw new InvalidOperationException("Voucher đã hết lượt sử dụng.");
                    }
                }

                hoaDon.TrangThai = TrangThaiHoaDon.DaThanhToan;
                hoaDon.PhuongThucThanhToan = phuongThuc;
                hoaDon.MaNguoiLap = maNguoiLap ?? PhienLamViec.MaTK;
                _hoaDonRepo.CapNhat(hoaDon);

                _datSanRepo.CapNhatTrangThai(hoaDon.MaDat, TrangThaiDatSan.HoanThanh);
                _sanRepo.CapNhatTrangThai(datSan.MaSan, TrangThaiSan.Trong);
            });

            return KetQua<HoaDon>.Tot(hoaDon, $"Thanh toán hóa đơn #{hoaDon.MaHD} thành công.");
        }
        catch (Exception ex)
        {
            return KetQua<HoaDon>.Loi("Không thể thanh toán: " + ex.Message);
        }
    }

    /// <summary>Hủy hóa đơn: hoàn lại trạng thái booking/sân và hoàn lượt voucher (nếu đã ghi nhận).</summary>
    public KetQua HuyHoaDon(int maHD, string lyDo = "")
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.HdXoa))
                return KetQua.Loi("Bạn không có quyền hủy hóa đơn.");

            HoaDon hoaDon = _hoaDonRepo.LayTheoMa(maHD);
            if (hoaDon == null) return KetQua.Loi("Hóa đơn không tồn tại.");
            if (hoaDon.TrangThai == TrangThaiHoaDon.DaThanhToan)
                return KetQua.Loi("Hóa đơn đã thanh toán, không thể hủy trực tiếp.");

            DatSan datSan = _datSanRepo.LayTheoMa(hoaDon.MaDat);

            DbHelper.ChayGiaoDich(() =>
            {
                if (hoaDon.MaVoucher != null && _suDungRepo.DaDungChoBooking(hoaDon.MaVoucher.Value, hoaDon.MaDat))
                {
                    _suDungRepo.XoaTheoMaDat(hoaDon.MaDat);
                    _voucherRepo.GiamSoLuongDaDung(hoaDon.MaVoucher.Value);
                }

                _hoaDonRepo.CapNhatTrangThai(maHD, TrangThaiHoaDon.DaHuy);
                if (datSan != null) _datSanRepo.CapNhatTrangThai(hoaDon.MaDat, TrangThaiDatSan.DaHuy);
            });

            return KetQua.Tot("Đã hủy hóa đơn #" + maHD + ".");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể hủy hóa đơn: " + ex.Message);
        }
    }

    public KetQua Xoa(int maHD)
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.HdXoa))
                return KetQua.Loi("Bạn không có quyền xóa hóa đơn.");

            HoaDon hoaDon = _hoaDonRepo.LayTheoMa(maHD);
            if (hoaDon == null) return KetQua.Loi("Hóa đơn không tồn tại.");
            if (hoaDon.TrangThai == TrangThaiHoaDon.DaThanhToan)
                return KetQua.Loi("Không thể xóa hóa đơn đã thanh toán.");

            _hoaDonRepo.Xoa(maHD);
            return KetQua.Tot("Xóa hóa đơn thành công.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi("Không thể xóa hóa đơn: " + ex.Message);
        }
    }

    public decimal TongDoanhThu(DateTime tuNgay, DateTime denNgay) => _hoaDonRepo.TongDoanhThu(tuNgay, denNgay);
}
