using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Business.Services;

/// <summary>Nghiệp vụ đặt sân: kiểm tra hợp lệ, chống trùng lịch, tính tiền, hủy booking.</summary>
public class DatSanService
{
    private readonly IDatSanRepository _datSanRepo;
    private readonly ISanRepository _sanRepo;
    private readonly IKhachHangRepository _khachHangRepo;
    private readonly IThamSoRepository _thamSoRepo;
    private readonly TinhTienService _tinhTienService;

    public DatSanService()
        : this(new Data.Repositories.DatSanRepository(), new Data.Repositories.SanRepository(),
               new Data.Repositories.KhachHangRepository(),
               new Data.Repositories.ThamSoRepository(),
               new TinhTienService())
    {
    }

    public DatSanService(IDatSanRepository datSanRepo, ISanRepository sanRepo,
        IKhachHangRepository khachHangRepo, IThamSoRepository thamSoRepo, TinhTienService tinhTienService)
    {
        _datSanRepo = datSanRepo;
        _sanRepo = sanRepo;
        _khachHangRepo = khachHangRepo;
        _thamSoRepo = thamSoRepo;
        _tinhTienService = tinhTienService;
    }

    public List<DatSan> LayTatCa(string tuKhoa = "") => _datSanRepo.LayTatCa(tuKhoa);

    public List<DatSan> LayTheoNgay(DateTime ngay, int? maSan = null) => _datSanRepo.LayTheoNgay(ngay, maSan);

    public List<DatSan> LayTheoKhoang(DateTime tuNgay, DateTime denNgay, int? maSan = null, string trangThai = null) =>
        _datSanRepo.LayTheoKhoang(tuNgay, denNgay, maSan, trangThai);

    public List<DatSan> LayTheoKhachHang(int maKH) => _datSanRepo.LayTheoKhachHang(maKH);

    public List<DatSan> LaySapDienRa(int soLuong) => _datSanRepo.LaySapDienRa(soLuong);

    public DatSan LayTheoMa(int maDat) => _datSanRepo.LayTheoMa(maDat);

    /// <summary>Trả về danh sách booking trùng giờ (rỗng = không trùng).</summary>
    public List<DatSan> LayTrungLich(int maSan, DateTime ngay, TimeSpan gioBatDau, TimeSpan gioKetThuc, int? maDatLoaiTru = null) =>
        _datSanRepo.LayTrungLich(maSan, ngay.Date, gioBatDau, gioKetThuc, maDatLoaiTru);

    /// <summary>Thông báo chi tiết khi trùng lịch.</summary>
    public string TaoThongBaoTrungLich(List<DatSan> danhSachTrung)
    {
        if (danhSachTrung == null || danhSachTrung.Count == 0) return "";
        string chiTiet = string.Join("\n", danhSachTrung.Select(d =>
            $"   • {d.TenSan}: {d.GioBatDau:hh\\:mm} - {d.GioKetThuc:hh\\:mm} ({TrangThaiDatSan.TenHienThi(d.TrangThai)} - {d.TenKH})"));
        return $"Sân đã được đặt trong khoảng thời gian này:\n{chiTiet}\nVui lòng chọn khung giờ hoặc sân khác.";
    }

    private int LaySoNgayDatTruoc()
    {
        try
        {
            string gt = _thamSoRepo.GiaTri(ThamSoKeys.SoNgayDatTruoc, "30");
            return int.TryParse(gt, out int n) && n > 0 ? n : 30;
        }
        catch { return 30; }
    }

    private int LayThoiGianHuyToiDaGio()
    {
        try
        {
            string gt = _thamSoRepo.GiaTri(ThamSoKeys.ThoiGianHuyToiDaGio, "24");
            return int.TryParse(gt, out int h) && h >= 0 ? h : 24;
        }
        catch { return 24; }
    }

    public KetQua<DatSan> TaoDatSan(int maKH, int maSan, DateTime ngayDat, TimeSpan gioBatDau, TimeSpan gioKetThuc,
        string ghiChu = "", string maVoucher = "")
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.DatSanThem))
                return KetQua<DatSan>.Loi("Bạn không có quyền tạo đặt sân.");

            KhachHang khachHang = _khachHangRepo.LayTheoMa(maKH);
            if (khachHang == null) return KetQua<DatSan>.Loi("Vui lòng chọn khách hàng hợp lệ.");

            San san = _sanRepo.LayTheoMa(maSan);
            if (san == null) return KetQua<DatSan>.Loi("Vui lòng chọn sân hợp lệ.");
            if (san.TrangThai == TrangThaiSan.BaoTri)
                return KetQua<DatSan>.Loi($"Sân \"{san.TenSan}\" đang bảo trì, không thể đặt.");

            KetQua kiemTraThoiGian = KiemTraThoiGian(ngayDat, gioBatDau, gioKetThuc);
            if (!kiemTraThoiGian.ThanhCong) return KetQua<DatSan>.Loi(kiemTraThoiGian.ThongBao);

            List<DatSan> trung = LayTrungLich(maSan, ngayDat, gioBatDau, gioKetThuc);
            if (trung.Count > 0) return KetQua<DatSan>.Loi(TaoThongBaoTrungLich(trung));

            KetQua<ChiTietTien> ketQuaTien = _tinhTienService.TinhTien(maSan, ngayDat, gioBatDau, gioKetThuc, maVoucher, maKH);
            if (!ketQuaTien.ThanhCong) return KetQua<DatSan>.Loi(ketQuaTien.ThongBao);

            var datSan = new DatSan
            {
                MaKH = maKH,
                MaSan = maSan,
                NgayDat = ngayDat.Date,
                GioBatDau = gioBatDau,
                GioKetThuc = gioKetThuc,
                TienSan = ketQuaTien.DuLieu.TienGoc,
                TrangThai = TrangThaiDatSan.DaDat,
                GhiChu = ghiChu ?? "",
                MaNguoiTao = PhienLamViec.MaTK,
                MaVoucher = ketQuaTien.DuLieu.VoucherDuocDung?.MaVoucher,
                MaVoucherCode = ketQuaTien.DuLieu.VoucherDuocDung?.MaCode ?? ""
            };

            KetQua<DatSan> ketQua = null;
            Data.Helpers.DbHelper.ChayGiaoDich(() =>
            {
                List<DatSan> trungKhoa = _datSanRepo.LayTrungLich(maSan, ngayDat.Date,
                    gioBatDau, gioKetThuc, null, khoaBang: true);
                if (trungKhoa.Count > 0)
                {
                    ketQua = KetQua<DatSan>.Loi(TaoThongBaoTrungLich(trungKhoa));
                    return;
                }

                datSan.MaDat = _datSanRepo.Them(datSan);
                ketQua = KetQua<DatSan>.Tot(datSan, $"Đặt sân thành công (mã #{datSan.MaDat}).");
            });

            if (ketQua != null && ketQua.ThanhCong)
            {
                try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "ThemDatSan", "DAT_SAN", datSan.MaDat.ToString(), $"Đặt sân {san.TenSan} cho {khachHang.HoTen} ngày {ngayDat:dd/MM} {gioBatDau:hh\\:mm}-{gioKetThuc:hh\\:mm}"); } catch { }
            }

            return ketQua ?? KetQua<DatSan>.Loi("Không thể đặt sân.");
        }
        catch (Exception ex)
        {
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "ThemDatSan", "DAT_SAN", null, $"Lỗi đặt sân: {ex.Message}", KetQuaNhatKy.ThatBai); } catch { }
            return KetQua<DatSan>.Loi("Không thể đặt sân: " + ex.Message);
        }
    }

    public KetQua<DatSan> CapNhatDatSan(DatSan datSan, string maVoucher = "")
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.DatSanSua))
                return KetQua<DatSan>.Loi("Bạn không có quyền sửa đặt sân.");
            if (datSan == null || datSan.MaDat <= 0) return KetQua<DatSan>.Loi("Booking không hợp lệ.");

            DatSan cu = _datSanRepo.LayTheoMa(datSan.MaDat);
            if (cu == null) return KetQua<DatSan>.Loi("Booking không tồn tại.");
            if (cu.TrangThai == TrangThaiDatSan.DaHuy)
                return KetQua<DatSan>.Loi("Booking đã hủy, không thể chỉnh sửa.");
            if (cu.TrangThai == TrangThaiDatSan.HoanThanh)
                return KetQua<DatSan>.Loi("Booking đã hoàn thành, không thể chỉnh sửa.");

            San san = _sanRepo.LayTheoMa(datSan.MaSan);
            if (san == null) return KetQua<DatSan>.Loi("Vui lòng chọn sân hợp lệ.");
            if (san.TrangThai == TrangThaiSan.BaoTri && datSan.MaSan != cu.MaSan)
                return KetQua<DatSan>.Loi($"Sân \"{san.TenSan}\" đang bảo trì.");

            KetQua kiemTraThoiGian = KiemTraThoiGian(datSan.NgayDat, datSan.GioBatDau, datSan.GioKetThuc, boQuaQuaKhu: true);
            if (!kiemTraThoiGian.ThanhCong) return KetQua<DatSan>.Loi(kiemTraThoiGian.ThongBao);

            List<DatSan> trung = LayTrungLich(datSan.MaSan, datSan.NgayDat, datSan.GioBatDau, datSan.GioKetThuc, datSan.MaDat);
            if (trung.Count > 0) return KetQua<DatSan>.Loi(TaoThongBaoTrungLich(trung));

            KetQua<ChiTietTien> ketQuaTien = _tinhTienService.TinhTien(datSan.MaSan, datSan.NgayDat,
                datSan.GioBatDau, datSan.GioKetThuc, maVoucher, datSan.MaKH);
            if (!ketQuaTien.ThanhCong) return KetQua<DatSan>.Loi(ketQuaTien.ThongBao);

            datSan.TienSan = ketQuaTien.DuLieu.TienGoc;
            datSan.MaVoucher = ketQuaTien.DuLieu.VoucherDuocDung?.MaVoucher ?? datSan.MaVoucher ?? cu.MaVoucher;
            datSan.MaVoucherCode = ketQuaTien.DuLieu.VoucherDuocDung?.MaCode ?? datSan.MaVoucherCode ?? cu.MaVoucherCode;

            KetQua<DatSan> ketQua = null;
            Data.Helpers.DbHelper.ChayGiaoDich(() =>
            {
                List<DatSan> trungKhoa = _datSanRepo.LayTrungLich(datSan.MaSan, datSan.NgayDat.Date,
                    datSan.GioBatDau, datSan.GioKetThuc, datSan.MaDat, khoaBang: true);
                if (trungKhoa.Count > 0)
                {
                    ketQua = KetQua<DatSan>.Loi(TaoThongBaoTrungLich(trungKhoa));
                    return;
                }

                _datSanRepo.CapNhat(datSan);
                ketQua = KetQua<DatSan>.Tot(datSan, "Cập nhật booking thành công.");
            });

            if (ketQua != null && ketQua.ThanhCong)
            {
                try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "SuaDatSan", "DAT_SAN", datSan.MaDat.ToString(), $"Sửa booking #{datSan.MaDat}"); } catch { }
            }

            return ketQua ?? KetQua<DatSan>.Loi("Không thể cập nhật booking.");
        }
        catch (Exception ex)
        {
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "SuaDatSan", "DAT_SAN", datSan?.MaDat.ToString(), $"Lỗi sửa: {ex.Message}", KetQuaNhatKy.ThatBai); } catch { }
            return KetQua<DatSan>.Loi("Không thể cập nhật booking: " + ex.Message);
        }
    }

    public KetQua HuyDatSan(int maDat, string lyDo = "")
    {
        try
        {
            if (!PhanQuyenService.CoQuyen(MaQuyen.DatSanHuy))
                return KetQua.Loi("Bạn không có quyền hủy booking.");

            DatSan datSan = _datSanRepo.LayTheoMa(maDat);
            if (datSan == null) return KetQua.Loi("Booking không tồn tại.");
            if (datSan.TrangThai == TrangThaiDatSan.DaHuy) return KetQua.Loi("Booking này đã được hủy trước đó.");
            if (datSan.TrangThai == TrangThaiDatSan.HoanThanh) return KetQua.Loi("Booking đã hoàn thành, không thể hủy.");
            if (PhienLamViec.LaKhachHang && PhienLamViec.MaKH != datSan.MaKH)
                return KetQua.Loi("Bạn chỉ có thể hủy booking của chính mình.");

            // Kiểm tra thời gian hủy tối đa (khách hàng)
            if (PhienLamViec.LaKhachHang)
            {
                int gioToiDa = LayThoiGianHuyToiDaGio();
                if (gioToiDa > 0)
                {
                    DateTime thoiGianBatDau = datSan.NgayDat.Date + datSan.GioBatDau;
                    double gioConLai = (thoiGianBatDau - DateTime.Now).TotalHours;
                    if (gioConLai < gioToiDa)
                        return KetQua.Loi($"Bạn chỉ được hủy trước {gioToiDa} giờ so với giờ bắt đầu. Còn lại {Math.Max(0, (int)gioConLai)} giờ.");
                }
            }

            _datSanRepo.CapNhatTrangThai(maDat, TrangThaiDatSan.DaHuy);
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "HuyDatSan", "DAT_SAN", maDat.ToString(), $"Hủy booking #{maDat}: {lyDo}"); } catch { }
            return KetQua.Tot("Đã hủy booking #" + maDat + ".");
        }
        catch (Exception ex)
        {
            try { ServiceFactory.NhatKy.Ghi(PhienLamViec.MaTK, "HuyDatSan", "DAT_SAN", maDat.ToString(), $"Lỗi hủy: {ex.Message}", KetQuaNhatKy.ThatBai); } catch { }
            return KetQua.Loi("Không thể hủy booking: " + ex.Message);
        }
    }

    /// <summary>Chuyển các booking đến giờ sang trạng thái Đang sử dụng (dùng khi tải Dashboard/Lịch).</summary>
    public int CapNhatBookingDangSuDung()
    {
        try
        {
            DateTime homNay = DateTime.Now.Date;
            TimeSpan bayGio = DateTime.Now.TimeOfDay;
            int soLuong = 0;
            foreach (DatSan dat in _datSanRepo.LayTheoNgay(homNay))
            {
                if (dat.TrangThai != TrangThaiDatSan.DaDat) continue;
                if (bayGio >= dat.GioBatDau && bayGio < dat.GioKetThuc)
                {
                    _datSanRepo.CapNhatTrangThai(dat.MaDat, TrangThaiDatSan.DangSuDung);
                    soLuong++;
                }
            }
            return soLuong;
        }
        catch
        {
            return 0;
        }
    }

    public KetQua KiemTraThoiGian(DateTime ngayDat, TimeSpan gioBatDau, TimeSpan gioKetThuc, bool boQuaQuaKhu = false)
    {
        if (gioKetThuc <= gioBatDau)
            return KetQua.Loi("Giờ kết thúc phải sau giờ bắt đầu.");
        if ((gioKetThuc - gioBatDau).TotalMinutes < 30)
            return KetQua.Loi("Thời lượng tối thiểu của một lượt thuê là 30 phút.");
        if ((gioKetThuc - gioBatDau).TotalHours > 12)
            return KetQua.Loi("Thời lượng thuê không được vượt quá 12 giờ.");
        if (gioBatDau < TimeSpan.Zero || gioKetThuc > new TimeSpan(1, 0, 0, 0))
            return KetQua.Loi("Khung giờ không hợp lệ.");

        if (!boQuaQuaKhu)
        {
            if (ngayDat.Date < DateTime.Now.Date)
                return KetQua.Loi("Không thể đặt sân cho ngày đã qua.");

            // Kiểm tra số ngày đặt trước tối đa
            int soNgayToiDa = LaySoNgayDatTruoc();
            if ((ngayDat.Date - DateTime.Today).TotalDays > soNgayToiDa)
                return KetQua.Loi($"Chỉ được đặt trước tối đa {soNgayToiDa} ngày.");

            if (ngayDat.Date == DateTime.Now.Date && gioBatDau < DateTime.Now.TimeOfDay)
                return KetQua.Loi("Giờ bắt đầu không được nằm trong quá khứ.");

            // Kiểm tra giờ mở/đóng cửa
            try
            {
                string gioMoStr = _thamSoRepo.GiaTri(ThamSoKeys.GioMoCua, "05:00");
                string gioDongStr = _thamSoRepo.GiaTri(ThamSoKeys.GioDongCua, "23:00");
                if (TimeSpan.TryParse(gioMoStr, out var gioMo) && TimeSpan.TryParse(gioDongStr, out var gioDong))
                {
                    if (gioBatDau < gioMo || gioKetThuc > gioDong)
                        return KetQua.Loi($"Trung tâm chỉ mở cửa từ {gioMo:hh\\:mm} đến {gioDong:hh\\:mm}.");
                }
            }
            catch { }
        }
        return KetQua.Tot();
    }
}
