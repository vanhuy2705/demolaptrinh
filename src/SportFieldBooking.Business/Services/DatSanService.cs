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
    private readonly TinhTienService _tinhTienService;

    public DatSanService()
        : this(new Data.Repositories.DatSanRepository(), new Data.Repositories.SanRepository(),
               new Data.Repositories.KhachHangRepository(), new TinhTienService())
    {
    }

    public DatSanService(IDatSanRepository datSanRepo, ISanRepository sanRepo,
        IKhachHangRepository khachHangRepo, TinhTienService tinhTienService)
    {
        _datSanRepo = datSanRepo;
        _sanRepo = sanRepo;
        _khachHangRepo = khachHangRepo;
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
                // Giữ lại voucher đã kiểm tra hợp lệ. Trước đây thông tin này bị vứt bỏ:
                // booking chỉ lưu giá gốc nên số tiền báo cho khách (đã giảm) khác số
                // được lưu, và sang bước lập hóa đơn phải chọn lại voucher từ đầu.
                MaVoucher = ketQuaTien.DuLieu.VoucherDuocDung?.MaVoucher,
                MaVoucherCode = ketQuaTien.DuLieu.VoucherDuocDung?.MaCode ?? ""
            };

            // CHỐNG ĐẶT TRÙNG ĐỒNG THỜI: kiểm tra lịch trống và ghi booking phải nằm trong
            // CÙNG một giao dịch, trong đó câu kiểm tra khoá dải bản ghi (UPDLOCK+HOLDLOCK).
            // Để rời nhau như trước thì hai người bấm "Đặt sân" cùng lúc đều thấy trống
            // và cả hai cùng ghi thành công.
            KetQua<DatSan> ketQua = null;
            Data.Helpers.DbHelper.ChayGiaoDich(() =>
            {
                List<DatSan> trungKhoa = _datSanRepo.LayTrungLich(maSan, ngayDat.Date,
                    gioBatDau, gioKetThuc, null, khoaBang: true);
                if (trungKhoa.Count > 0)
                {
                    ketQua = KetQua<DatSan>.Loi(TaoThongBaoTrungLich(trungKhoa));
                    return;                 // không ghi gì; giao dịch commit rỗng, khoá được nhả
                }

                datSan.MaDat = _datSanRepo.Them(datSan);
                ketQua = KetQua<DatSan>.Tot(datSan, $"Đặt sân thành công (mã #{datSan.MaDat}).");
            });

            return ketQua ?? KetQua<DatSan>.Loi("Không thể đặt sân.");
        }
        catch (Exception ex)
        {
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

            // Voucher: ưu tiên mã vừa nhập; không nhập thì GIỮ voucher đang lưu trên booking
            // (màn hình chi tiết không có ô nhập voucher, không được làm khách mất ưu đãi).
            datSan.MaVoucher = ketQuaTien.DuLieu.VoucherDuocDung?.MaVoucher ?? datSan.MaVoucher ?? cu.MaVoucher;
            datSan.MaVoucherCode = ketQuaTien.DuLieu.VoucherDuocDung?.MaCode ?? datSan.MaVoucherCode ?? cu.MaVoucherCode;

            // Giống lúc đặt mới: kiểm tra trùng + ghi trong một giao dịch có khoá.
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

            return ketQua ?? KetQua<DatSan>.Loi("Không thể cập nhật booking.");
        }
        catch (Exception ex)
        {
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

            _datSanRepo.CapNhatTrangThai(maDat, TrangThaiDatSan.DaHuy);
            return KetQua.Tot("Đã hủy booking #" + maDat + ".");
        }
        catch (Exception ex)
        {
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
            if (ngayDat.Date == DateTime.Now.Date && gioBatDau < DateTime.Now.TimeOfDay)
                return KetQua.Loi("Giờ bắt đầu không được nằm trong quá khứ.");
        }
        return KetQua.Tot();
    }
}
