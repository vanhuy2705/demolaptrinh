using SportFieldBooking.Business.Common;
using SportFieldBooking.Business.Services;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Repositories;

namespace SportFieldBooking.KiemThu;

/// <summary>
/// Bộ kiểm thử quy tắc nghiệp vụ (chạy bằng kho dữ liệu giả, KHÔNG cần SQL Server).
/// Chạy:  dotnet run --project tests/SportFieldBooking.KiemThu
/// </summary>
internal static class Program
{
    private const int MaSanA1 = 1;
    private static int _dat;
    private static int _thatBai;

    private static DateTime _ngayThuong;   // Thứ Hai tới (ngày thường)
    private static DateTime _ngayThuBay;   // Thứ Bảy tới (cuối tuần)

    private static KhoKhuyenMaiGia _khoKhuyenMai;
    private static KhoDatSanGia _khoDatSan;
    private static KhoVoucherGia _khoVoucher;
    private static KhoHoaDonGia _khoHoaDon;
    private static TinhTienService _tinhTien;
    private static DatSanService _datSan;
    private static HoaDonService _hoaDon;
    private static CauHinhService _cauHinh;
    private static VoucherService _voucher;

    private static int Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        TaoDuLieu();

        Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   KIỂM THỬ QUY TẮC NGHIỆP VỤ - QUẢN LÝ CHO THUÊ SÂN THỂ THAO     ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");

        Nhom("A. Tính thời lượng theo block 30 phút");
        Kiem("60 phút (17:00-18:00) = 2 block", TinhTienService.TinhSoBlock(TS(17, 0), TS(18, 0), 30) == 2);
        Kiem("70 phút (17:00-18:10) = 3 block", TinhTienService.TinhSoBlock(TS(17, 0), TS(18, 10), 30) == 3);
        Kiem("45 phút (17:00-17:45) = 2 block", TinhTienService.TinhSoBlock(TS(17, 0), TS(17, 45), 30) == 2);
        Kiem("90 phút = 3 block", TinhTienService.TinhSoBlock(TS(17, 0), TS(18, 30), 30) == 3);
        Kiem("Tiền block: 400.000 x 3 x 30/60 = 600.000",
            TinhTienService.TinhTienTheoBlock(400000m, 3, 30) == 600000m);

        Nhom("B. Giá gốc & giảm cuối tuần (tắt khuyến mãi)");
        BatKhuyenMai(false);
        KetQua<ChiTietTien> thuong = Tinh(_ngayThuong, TS(17, 0), TS(18, 0));
        Kiem("Ngày thường 1 giờ = 400.000 đ (không giảm)",
            thuong.ThanhCong && thuong.DuLieu.TongTien == 400000m && thuong.DuLieu.LoaiGiamGia == LoaiGiamGia.Khong);

        KetQua<ChiTietTien> cuoiTuan = Tinh(_ngayThuBay, TS(17, 0), TS(18, 0));
        Kiem("Thứ Bảy 1 giờ = 360.000 đ (giảm cuối tuần 10%)",
            cuoiTuan.ThanhCong && cuoiTuan.DuLieu.TongTien == 360000m && cuoiTuan.DuLieu.LoaiGiamGia == LoaiGiamGia.CuoiTuan);

        KetQua<ChiTietTien> coVoucher = Tinh(_ngayThuBay, TS(17, 0), TS(18, 0), "GIAM20");
        Kiem("Thứ Bảy + GIAM20 = 320.000 đ (voucher thắng cuối tuần)",
            coVoucher.ThanhCong && coVoucher.DuLieu.TongTien == 320000m && coVoucher.DuLieu.LoaiGiamGia == LoaiGiamGia.Voucher);

        KetQua<ChiTietTien> khongCongDon = Tinh(_ngayThuBay, TS(17, 0), TS(18, 0), "GIAM20");
        Kiem("KHÔNG cộng dồn: chỉ giảm 80.000 đ (không phải 80.000 + 40.000)",
            khongCongDon.ThanhCong && khongCongDon.DuLieu.TienGiam == 80000m);

        KetQua<ChiTietTien> gioLe = Tinh(_ngayThuong, TS(17, 0), TS(18, 10));
        Kiem("17:00-18:10 = 1,5 giờ = 600.000 đ",
            gioLe.ThanhCong && gioLe.DuLieu.SoGio == 1.5m && gioLe.DuLieu.TongTien == 600000m);

        Nhom("C. Khuyến mãi (ưu tiên sau voucher, trước cuối tuần)");
        BatKhuyenMai(true);

        KetQua<ChiTietTien> km = Tinh(_ngayThuong, TS(17, 0), TS(18, 0));
        Kiem("Ngày thường: KM 15% (áp dụng mọi ngày) = 340.000 đ",
            km.ThanhCong && km.DuLieu.LoaiGiamGia == LoaiGiamGia.KhuyenMai
            && km.DuLieu.TongTien == 340000m && km.DuLieu.TenUuDai.Contains("15"));

        KetQua<ChiTietTien> kmCuoiTuan = Tinh(_ngayThuBay, TS(17, 0), TS(18, 0));
        Kiem("Thứ Bảy: KM 25% dành riêng cuối tuần thắng KM 15% = 300.000 đ",
            kmCuoiTuan.ThanhCong && kmCuoiTuan.DuLieu.LoaiGiamGia == LoaiGiamGia.KhuyenMai
            && kmCuoiTuan.DuLieu.TongTien == 300000m);

        KetQua<ChiTietTien> kmVoiVoucher = Tinh(_ngayThuong, TS(17, 0), TS(18, 0), "GIAM20");
        Kiem("Voucher thắng cả khuyến mãi lẫn cuối tuần (400.000 - 20% = 320.000)",
            kmVoiVoucher.ThanhCong && kmVoiVoucher.DuLieu.LoaiGiamGia == LoaiGiamGia.Voucher
            && kmVoiVoucher.DuLieu.TongTien == 320000m);

        Nhom("D. Kiểm tra điều kiện voucher");
        Kiem("Voucher hết hạn bị từ chối", !Tinh(_ngayThuong, TS(17, 0), TS(18, 0), "HETHAN").ThanhCong);
        Kiem("Voucher hết lượt bị từ chối", !Tinh(_ngayThuong, TS(17, 0), TS(18, 0), "HETLUOT").ThanhCong);
        Kiem("Voucher không tồn tại bị từ chối", !Tinh(_ngayThuong, TS(17, 0), TS(18, 0), "KHONGTONTAI").ThanhCong);

        KetQua<ChiTietTien> duoiToiThieu = Tinh(_ngayThuong, TS(17, 0), TS(17, 30), "GIAM50K"); // 200.000 đ
        Kiem("Đơn 200.000 < tối thiểu 300.000 => từ chối GIAM50K", !duoiToiThieu.ThanhCong);

        KetQua<ChiTietTien> duToiThieu = Tinh(_ngayThuong, TS(17, 0), TS(18, 0), "GIAM50K");    // 400.000 đ
        Kiem("Đơn 400.000 đủ điều kiện => GIAM50K còn 350.000 đ",
            duToiThieu.ThanhCong && duToiThieu.DuLieu.TongTien == 350000m);

        KetQua<Voucher> hopLe = _voucher.KiemTraVoucher("giam20", 400000m);
        Kiem("Mã voucher không phân biệt hoa thường", hopLe.ThanhCong);

        Nhom("E. Chống trùng lịch");
        List<DatSan> trung = _datSan.LayTrungLich(MaSanA1, _ngayThuong, TS(17, 30), TS(18, 30));
        Kiem("17:30-18:30 trùng booking 17:00-18:00", trung.Count == 1);

        List<DatSan> khongTrung = _datSan.LayTrungLich(MaSanA1, _ngayThuong, TS(18, 0), TS(19, 0));
        Kiem("18:00-19:00 KHÔNG trùng (giáp giờ)", khongTrung.Count == 0);

        List<DatSan> boQuaHuy = _datSan.LayTrungLich(MaSanA1, _ngayThuong, TS(19, 30), TS(20, 30));
        Kiem("Booking đã hủy được bỏ qua", boQuaHuy.Count == 0);

        List<DatSan> sanKhac = _datSan.LayTrungLich(99, _ngayThuong, TS(17, 30), TS(18, 30));
        Kiem("Sân khác không bị ảnh hưởng", sanKhac.Count == 0);

        Kiem("Thông báo trùng lịch có tên sân + khung giờ",
            _datSan.TaoThongBaoTrungLich(trung).Contains("Sân A1")
            && _datSan.TaoThongBaoTrungLich(trung).Contains("17:00"));

        Nhom("F. Kiểm tra thời gian đặt sân");
        Kiem("Giờ kết thúc <= giờ bắt đầu => lỗi",
            !_datSan.KiemTraThoiGian(_ngayThuong, TS(18, 0), TS(18, 0)).ThanhCong);
        Kiem("Thời lượng < 30 phút => lỗi",
            !_datSan.KiemTraThoiGian(_ngayThuong, TS(17, 0), TS(17, 15)).ThanhCong);
        Kiem("Thời lượng > 12 giờ => lỗi",
            !_datSan.KiemTraThoiGian(_ngayThuong, TS(5, 0), TS(23, 0)).ThanhCong);
        Kiem("Ngày đã qua => lỗi",
            !_datSan.KiemTraThoiGian(DateTime.Today.AddDays(-1), TS(17, 0), TS(18, 0)).ThanhCong);
        Kiem("Khung giờ hợp lệ => chấp nhận",
            _datSan.KiemTraThoiGian(_ngayThuong, TS(17, 0), TS(18, 0)).ThanhCong);
        Kiem("Ngày trong quá khứ nhưng bỏ qua kiểm tra => chấp nhận",
            _datSan.KiemTraThoiGian(DateTime.Today.AddDays(-1), TS(17, 0), TS(18, 0), boQuaQuaKhu: true).ThanhCong);

        Nhom("G. Phân quyền (kiểm tra ở tầng nghiệp vụ)");
        Kiem("Admin có quyền sửa cấu hình", PhanQuyenService.CoQuyen(VaiTro.Admin, MaQuyen.CauHinhSua));
        Kiem("Nhân viên KHÔNG có quyền sửa cấu hình", !PhanQuyenService.CoQuyen(VaiTro.NhanVien, MaQuyen.CauHinhSua));
        Kiem("Nhân viên có quyền lập hóa đơn", PhanQuyenService.CoQuyen(VaiTro.NhanVien, MaQuyen.HdLap));
        Kiem("Khách hàng KHÔNG có quyền xem hóa đơn tất cả", !PhanQuyenService.CoQuyen(VaiTro.KhachHang, MaQuyen.HdXemTatCa));
        Kiem("Khách hàng có quyền xem hóa đơn của tôi", PhanQuyenService.CoQuyen(VaiTro.KhachHang, MaQuyen.HdXemCuaToi));
        Kiem("Khách hàng KHÔNG có quyền quản lý nhân viên", !PhanQuyenService.CoQuyen(VaiTro.KhachHang, MaQuyen.NvXem));

        Nhom("H. Cấu hình hệ thống (THAM_SO)");
        Kiem("Giờ mở cửa = 05:00", _cauHinh.LayGioMoCua() == new TimeSpan(5, 0, 0));
        Kiem("Giờ đóng cửa = 23:00", _cauHinh.LayGioDongCua() == new TimeSpan(23, 0, 0));
        Kiem("% giảm cuối tuần = 10", _cauHinh.LayPhanTramGiamCuoiTuan() == 10m);
        Kiem("Block tính tiền = 30 phút", _cauHinh.LayThoiLuongBlockPhut() == 30);
        Kiem("Thứ Bảy là cuối tuần", KhuyenMaiService.LaCuoiTuan(_ngayThuBay));
        Kiem("Thứ Hai không phải cuối tuần", !KhuyenMaiService.LaCuoiTuan(_ngayThuong));

        Nhom("I. Đặt sân: voucher đi theo booking & chống đặt trùng đồng thời");
        BatKhuyenMai(false);
        PhienLamViec.DangNhap(new TaiKhoan
        {
            MaTK = 1, TenDangNhap = "admin", HoTen = "Quản trị viên", VaiTro = VaiTro.Admin
        });

        int maGiam20 = _khoVoucher.LayTheoMaCode("GIAM20").MaVoucher;
        int maHetLuot = _khoVoucher.LayTheoMaCode("HETLUOT").MaVoucher;
        int soBooking = _khoDatSan.DuLieu.Count;
        int soLanKhoa = _khoDatSan.SoLanDocKhoa;

        KetQua<DatSan> datMoi = _datSan.TaoDatSan(1, MaSanA1, _ngayThuong, TS(20, 0), TS(21, 0), "", "GIAM20");
        Kiem("Đặt sân 20:00-21:00 kèm GIAM20 => thành công", datMoi.ThanhCong);
        Kiem("Booking LƯU voucher (trước đây bị mất)",
            datMoi.ThanhCong && datMoi.DuLieu.MaVoucher == maGiam20);
        Kiem("Booking lưu giá gốc 400.000 đ, giảm giá tính ở hóa đơn",
            datMoi.ThanhCong && datMoi.DuLieu.TienSan == 400000m);
        Kiem("Bước kiểm tra trùng trước khi ghi có ĐỌC KHOÁ (chống race)",
            _khoDatSan.SoLanDocKhoa > soLanKhoa);

        KetQua<DatSan> datTrung = _datSan.TaoDatSan(2, MaSanA1, _ngayThuong, TS(20, 30), TS(21, 30));
        Kiem("Đặt chồng 20:30-21:30 cùng sân => bị từ chối", !datTrung.ThanhCong);
        Kiem("Không sinh booking mới khi trùng lịch", _khoDatSan.DuLieu.Count == soBooking + 1);

        KetQua<HoaDon> hd = _hoaDon.LapHoaDon(datMoi.DuLieu.MaDat, "");
        Kiem("Lập hóa đơn KHÔNG cần nhập lại voucher => thành công", hd.ThanhCong);
        Kiem("Hóa đơn tự dùng voucher của booking: 400.000 - 20% = 320.000 đ",
            hd.ThanhCong && hd.DuLieu.TongTien == 320000m && hd.DuLieu.LoaiGiamGia == LoaiGiamGia.Voucher);
        Kiem("Hóa đơn ghi đúng MaVoucher của booking", hd.ThanhCong && hd.DuLieu.MaVoucher == maGiam20);

        DatSan banSua = _khoDatSan.LayTheoMa(datMoi.DuLieu.MaDat);
        banSua.GioKetThuc = TS(21, 30);
        banSua.GhiChu = "Đổi giờ đá thêm 30 phút";
        KetQua<DatSan> kqSua = _datSan.CapNhatDatSan(banSua);
        Kiem("Sửa booking (không nhập voucher) => thành công", kqSua.ThanhCong);
        Kiem("Sửa booking vẫn GIỮ voucher cũ, khách không mất ưu đãi",
            kqSua.ThanhCong && _khoDatSan.LayTheoMa(banSua.MaDat).MaVoucher == maGiam20);

        // Voucher hết lượt vào lúc thu tiền: không được chặn nghiệp vụ lập hóa đơn.
        KetQua<DatSan> datHetLuot = _datSan.TaoDatSan(2, MaSanA1, _ngayThuong, TS(22, 0), TS(23, 0));
        Kiem("Đặt sân 22:00-23:00 không voucher => thành công", datHetLuot.ThanhCong);
        _khoDatSan.LayTheoMa(datHetLuot.DuLieu.MaDat).MaVoucher = maHetLuot;   // voucher sau đó hết lượt
        KetQua<HoaDon> hdHetLuot = _hoaDon.LapHoaDon(datHetLuot.DuLieu.MaDat, "");
        Kiem("Voucher của booking đã hết lượt => vẫn lập được hóa đơn (không chặn thu tiền)",
            hdHetLuot.ThanhCong);
        Kiem("...và tính theo giá hiện hành 400.000 đ, không giảm",
            hdHetLuot.ThanhCong && hdHetLuot.DuLieu.TongTien == 400000m
            && hdHetLuot.DuLieu.LoaiGiamGia == LoaiGiamGia.Khong);

        // --- J. SQL tự thích ứng khi CSDL chưa được nâng cấp ---
        Nhom("J. SQL thích ứng lược đồ (CSDL cũ chưa có cột MaVoucher vẫn chạy)");
        const string khung = @"
        SELECT ds.MaDat, ds.MaNguoiTao{COT_VOUCHER},
               ISNULL(s.DonGia, 0) AS DonGia{COT_MA_CODE}
        FROM DAT_SAN ds
        LEFT JOIN SAN s ON s.MaSan = ds.MaSan{JOIN_VOUCHER}";

        string sqlMoi = DatSanRepository.LapSql(khung, coVoucher: true);
        string sqlCu = DatSanRepository.LapSql(khung, coVoucher: false);

        Kiem("CSDL v3: SELECT có ds.MaVoucher + JOIN VOUCHER + MaVoucherCode",
            sqlMoi.Contains("ds.MaVoucher") && sqlMoi.Contains("LEFT JOIN VOUCHER v")
            && sqlMoi.Contains("AS MaVoucherCode"));

        Kiem("CSDL cũ: SELECT hoàn toàn không nhắc tới MaVoucher/VOUCHER",
            !sqlCu.Contains("MaVoucher") && !sqlCu.Contains("VOUCHER"));

        Kiem("Cả 2 trạng thái: không còn chỗ giữ {…} chưa được thay",
            !sqlMoi.Contains("{") && !sqlMoi.Contains("}") && !sqlCu.Contains("{") && !sqlCu.Contains("}"));

        Kiem("CSDL cũ: danh sách cột vẫn hợp lệ (không thừa dấu phẩy trước FROM)",
            sqlCu.Contains("AS DonGia\n        FROM DAT_SAN ds") && !sqlCu.Contains(",\n        FROM"));

        Kiem("CSDL v3: danh sách cột vẫn hợp lệ (không thừa dấu phẩy trước FROM)",
            sqlMoi.Contains("AS MaVoucherCode\n        FROM DAT_SAN ds") && !sqlMoi.Contains(",\n        FROM"));

        Kiem("Khoá chống trùng WITH (UPDLOCK, HOLDLOCK) vẫn gắn được ở cả 2 trạng thái",
            sqlMoi.Replace("FROM DAT_SAN ds", "FROM DAT_SAN ds WITH (UPDLOCK, HOLDLOCK)").Contains("UPDLOCK")
            && sqlCu.Replace("FROM DAT_SAN ds", "FROM DAT_SAN ds WITH (UPDLOCK, HOLDLOCK)").Contains("UPDLOCK"));

        // --- Tổng kết ---
        Console.WriteLine();
        Console.WriteLine(new string('─', 66));
        Console.WriteLine($"  KẾT QUẢ: {_dat} đạt / {_thatBai} lỗi / {_dat + _thatBai} tổng số");
        Console.WriteLine(new string('─', 66));
        return _thatBai == 0 ? 0 : 1;
    }

    // ---------- Thiết lập ----------
    private static void TaoDuLieu()
    {
        _ngayThuong = NgayTiepTheo(DayOfWeek.Monday);
        _ngayThuBay = NgayTiepTheo(DayOfWeek.Saturday);

        var khoSan = new KhoSanGia();
        khoSan.Them(new San
        {
            MaSan = MaSanA1,
            TenSan = "Sân A1",
            MaLoaiSan = 1,
            DonGia = 400000m,
            TrangThai = TrangThaiSan.Trong
        });

        var khoThamSo = new KhoThamSoGia();
        khoThamSo.Them(new ThamSo { TenThamSo = ThamSoKeys.ThoiLuongBlockPhut, GiaTri = "30" });
        khoThamSo.Them(new ThamSo { TenThamSo = ThamSoKeys.PhanTramGiamCuoiTuan, GiaTri = "10" });
        khoThamSo.Them(new ThamSo { TenThamSo = ThamSoKeys.GioMoCua, GiaTri = "05:00" });
        khoThamSo.Them(new ThamSo { TenThamSo = ThamSoKeys.GioDongCua, GiaTri = "23:00" });

        var khoSuDung = new KhoSuDungVoucherGia();
        var khoVoucher = new KhoVoucherGia();
        khoVoucher.Them(Mau("GIAM20", "Giảm 20%", LoaiGiam.PhanTram, 20m, 0m, 50, -30, 30));
        khoVoucher.Them(Mau("GIAM50K", "Giảm 50.000", LoaiGiam.SoTien, 50000m, 300000m, 20, -30, 30));
        khoVoucher.Them(Mau("HETHAN", "Voucher hết hạn", LoaiGiam.PhanTram, 30m, 0m, 10, -60, -1));
        Voucher hetLuot = Mau("HETLUOT", "Voucher hết lượt", LoaiGiam.PhanTram, 10m, 0m, 5, -30, 30);
        hetLuot.SoLuongDaDung = 5;
        khoVoucher.Them(hetLuot);

        _khoKhuyenMai = new KhoKhuyenMaiGia();
        var khoKhuyenMai = _khoKhuyenMai;
        khoKhuyenMai.Them(new KhuyenMai
        {
            MaKM = 1,
            TenKM = "Khai trương giảm 15%",
            PhanTramGiam = 15m,
            NgayBatDau = DateTime.Today.AddDays(-5),
            NgayKetThuc = DateTime.Today.AddDays(30),
            ApDungCuoiTuan = false,          // 0 = áp dụng mọi ngày (kể cả cuối tuần)
            TrangThai = TrangThaiVoucher.HoatDong
        });
        khoKhuyenMai.Them(new KhuyenMai
        {
            MaKM = 2,
            TenKM = "Cuối tuần vui vẻ giảm 25%",
            PhanTramGiam = 25m,
            NgayBatDau = DateTime.Today.AddDays(-5),
            NgayKetThuc = DateTime.Today.AddDays(30),
            ApDungCuoiTuan = true,           // 1 = chỉ áp dụng Thứ Bảy / Chủ Nhật
            TrangThai = TrangThaiVoucher.HoatDong
        });

        var khoDatSan = new KhoDatSanGia();
        khoDatSan.Them(new DatSan
        {
            MaDat = 1, MaKH = 1, MaSan = MaSanA1, NgayDat = _ngayThuong,
            GioBatDau = TS(17, 0), GioKetThuc = TS(18, 0),
            TienSan = 400000m, TrangThai = TrangThaiDatSan.DaDat,
            TenKH = "Nguyễn Văn A", TenSan = "Sân A1"
        });
        khoDatSan.Them(new DatSan
        {
            MaDat = 2, MaKH = 2, MaSan = MaSanA1, NgayDat = _ngayThuong,
            GioBatDau = TS(19, 0), GioKetThuc = TS(20, 0),
            TienSan = 400000m, TrangThai = TrangThaiDatSan.DaHuy,   // đã hủy: không tính trùng
            TenKH = "Trần Thị B", TenSan = "Sân A1"
        });

        var khoKhachHang = new KhoKhachHangGia();
        khoKhachHang.Them(new KhachHang { MaKH = 1, HoTen = "Nguyễn Văn A", SDT = "0900000001" });
        khoKhachHang.Them(new KhachHang { MaKH = 2, HoTen = "Trần Thị B", SDT = "0900000002" });

        _khoVoucher = khoVoucher;
        _khoDatSan = khoDatSan;
        _khoHoaDon = new KhoHoaDonGia();

        // Bộ kiểm thử chạy hoàn toàn bằng kho dữ liệu giả trong bộ nhớ, không có
        // SQL Server: thay cơ chế giao dịch bằng cách chạy thẳng khối lệnh.
        // (DbHelper.GiaoDichWrapper là điểm nối dành riêng cho kiểm thử.)
        DbHelper.GiaoDichWrapper = thucHien => thucHien();
        DbHelper.GiaoDichWrapperAsync = thucHien => thucHien();

        _voucher = new VoucherService(khoVoucher, khoSuDung);
        var khuyenMaiService = new KhuyenMaiService(khoKhuyenMai);
        _tinhTien = new TinhTienService(khoSan, khoThamSo, _voucher, khuyenMaiService);
        _datSan = new DatSanService(khoDatSan, khoSan, khoKhachHang, _tinhTien);
        _hoaDon = new HoaDonService(_khoHoaDon, khoDatSan, khoSan, khoVoucher, khoSuDung, _tinhTien);
        _cauHinh = new CauHinhService(khoThamSo);
    }

    /// <summary>Bật/tắt toàn bộ chương trình khuyến mãi để tách biệt các nhóm kiểm thử.</summary>
    private static void BatKhuyenMai(bool bat)
    {
        foreach (KhuyenMai km in _khoKhuyenMai.DuLieu)
            km.TrangThai = bat ? TrangThaiVoucher.HoatDong : TrangThaiVoucher.TamNgung;
    }

    private static Voucher Mau(string ma, string ten, string loai, decimal giaTri, decimal toiThieu,
        int soLuong, int ngayBatDau, int ngayKetThuc) => new()
        {
            MaCode = ma,
            TenVoucher = ten,
            LoaiGiam = loai,
            GiaTriGiam = giaTri,
            DonToiThieu = toiThieu,
            SoLuong = soLuong,
            SoLuongDaDung = 0,
            NgayBatDau = DateTime.Today.AddDays(ngayBatDau),
            NgayKetThuc = DateTime.Today.AddDays(ngayKetThuc),
            TrangThai = TrangThaiVoucher.HoatDong
        };

    // ---------- Tiện ích ----------
    private static TimeSpan TS(int gio, int phut) => new(gio, phut, 0);

    private static DateTime NgayTiepTheo(DayOfWeek thu)
    {
        int chenh = ((int)thu - (int)DateTime.Today.DayOfWeek + 7) % 7;
        if (chenh == 0) chenh = 7;              // luôn lấy ngày trong tương lai
        return DateTime.Today.AddDays(chenh);
    }

    private static KetQua<ChiTietTien> Tinh(DateTime ngay, TimeSpan tu, TimeSpan den, string maVoucher = "") =>
        _tinhTien.TinhTien(MaSanA1, ngay, tu, den, maVoucher, maKH: 1);

    private static void Nhom(string ten)
    {
        Console.WriteLine();
        Console.WriteLine($"── {ten} " + new string('─', Math.Max(0, 60 - ten.Length)));
    }

    private static void Kiem(string ten, bool dieuKien)
    {
        if (dieuKien)
        {
            _dat++;
            Console.WriteLine($"  [ĐẠT] {ten}");
        }
        else
        {
            _thatBai++;
            Console.WriteLine($"  [LỖI] {ten}");
        }
    }
}
