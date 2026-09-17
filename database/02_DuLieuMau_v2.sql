/*
  Quản lý cho thuê sân thể thao — DỮ LIỆU MẪU BẢN ĐẦY ĐỦ (v2)
  ===========================================================
  Chạy SAU 01_TaoCSDL_v2.sql (cùng database QLSanTheThao).

  ---------------------------------------------------------------------
  TÀI KHOẢN DEMO
  ---------------------------------------------------------------------
     admin     / admin123    -> Quản trị viên (toàn quyền) — ĐÃ CÓ hồ sơ NHAN_VIEN
     quanly    / admin123    -> Quản trị viên thứ hai (dự phòng)
     nhanvien  / nv123456    -> Nhân viên lễ tân
     nhanvien2 / nv123456    -> Nhân viên ca tối
     khach1    / kh123456    -> Khách hàng Nguyễn Văn An
     khach2    / kh123456    -> Khách hàng Lê Hoàng Nam
  Mật khẩu lưu dạng băm PBKDF2 (iterations.salt.hash). Ứng dụng vẫn chấp nhận
  mật khẩu thô nếu cột MatKhau sai định dạng (cơ chế dự phòng trong PasswordHasher),
  nên có thể UPDATE tay thành 'admin123' khi cần thử nhanh.

  ---------------------------------------------------------------------
  KHÁC BIỆT SO VỚI 02_DuLieuMau.sql (bản cũ)
  ---------------------------------------------------------------------
   • Admin có dòng NHAN_VIEN  -> không còn "admin vô hình" trong màn hình
     Quản lý nhân viên, và PhienLamViec.MaNV khác NULL khi đăng nhập admin.
   • Ngày tháng sinh ĐỘNG theo GETDATE(): dữ liệu mẫu luôn hợp lệ dù chạy
     vào bất kỳ ngày nào (bản cũ hard-code năm 2026 nên dễ hết hạn voucher).
   • Sinh ~38 ngày booking bằng vòng lặp (quá khứ 30 ngày -> tương lai 7 ngày),
     tiền sân tính đúng công thức ứng dụng qua fn_TienSan().
   • Hóa đơn sinh kèm: giảm cuối tuần 10%, voucher GIAM20 20% (ưu tiên cao hơn),
     3 hóa đơn mới nhất để ChuaThanhToan phục vụ demo thu tiền.
   • Có dữ liệu SU_DUNG_VOUCHER + cập nhật SoLuongDaDung, và NHAT_KY_HOAT_DONG.
*/

USE QLSanTheThao;
GO

SET NOCOUNT ON;
GO

/* ============================== 1. TÀI KHOẢN ============================== */
INSERT INTO TAIKHOAN (TenDangNhap, MatKhau, HoTen, VaiTro, TrangThai) VALUES
(N'admin',
 N'100000.yGnbndaG9aF5wmkGfJVbiw==.2kfLmCa+dnHyly5rG3T5vzl+n0Il/lkK+frdnpsw4PQ=',   /* admin123 */
 N'Nguyễn Quốc Huy', N'Admin', N'HoatDong'),
(N'quanly',
 N'100000.yGnbndaG9aF5wmkGfJVbiw==.2kfLmCa+dnHyly5rG3T5vzl+n0Il/lkK+frdnpsw4PQ=',   /* admin123 */
 N'Đặng Thùy Linh', N'Admin', N'HoatDong'),
(N'nhanvien',
 N'100000.KjxMMIrSfCf3HhzrkEs5DQ==.g9LsgqJwv+J82byKmZOYcj9eTNiGLH0a1xt2kVWzA7E=',   /* nv123456 */
 N'Trần Thị Thu Ngân', N'NhanVien', N'HoatDong'),
(N'nhanvien2',
 N'100000.KjxMMIrSfCf3HhzrkEs5DQ==.g9LsgqJwv+J82byKmZOYcj9eTNiGLH0a1xt2kVWzA7E=',   /* nv123456 */
 N'Lê Văn Đạt', N'NhanVien', N'HoatDong'),
(N'khach1',
 N'100000.0jYIlIIiMUPpZnJWq81pVw==.kHsGrbXy1vuYRzF5IlgNReUPzYApUsZojT1WlpXippY=',   /* kh123456 */
 N'Nguyễn Văn An', N'KhachHang', N'HoatDong'),
(N'khach2',
 N'100000.0jYIlIIiMUPpZnJWq81pVw==.kHsGrbXy1vuYRzF5IlgNReUPzYApUsZojT1WlpXippY=',   /* kh123456 */
 N'Lê Hoàng Nam', N'KhachHang', N'HoatDong'),
(N'khoa_test',
 N'100000.KjxMMIrSfCf3HhzrkEs5DQ==.g9LsgqJwv+J82byKmZOYcj9eTNiGLH0a1xt2kVWzA7E=',   /* nv123456 */
 N'Phạm Văn Thử', N'NhanVien', N'BiKhoa');   /* để demo màn hình khóa/mở tài khoản */
GO

/* ================== 2. HỒ SƠ NHÂN VIÊN (kể cả Admin) ================== */
INSERT INTO NHAN_VIEN (MaTK, HoTen, SDT, Email, DiaChi, ChucVu, NgayVaoLam, TrangThai) VALUES
((SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'admin'),
 N'Nguyễn Quốc Huy', N'0901000001', N'huy.nq@santhethao.vn',
 N'1 Đường Thể Thao, Hoàn Kiếm, Hà Nội', N'Quản trị viên', '2023-03-01', N'HoatDong'),
((SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'quanly'),
 N'Đặng Thùy Linh', N'0901000002', N'linh.dt@santhethao.vn',
 N'25 Phố Huế, Hai Bà Trưng, Hà Nội', N'Quản lý trung tâm', '2023-08-15', N'HoatDong'),
((SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'nhanvien'),
 N'Trần Thị Thu Ngân', N'0905123456', N'ngan.tt@santhethao.vn',
 N'12 Nguyễn Trãi, Thanh Xuân, Hà Nội', N'Nhân viên lễ tân', '2025-01-15', N'HoatDong'),
((SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'nhanvien2'),
 N'Lê Văn Đạt', N'0905222333', N'dat.lv@santhethao.vn',
 N'88 Cầu Giấy, Hà Nội', N'Nhân viên ca tối', '2025-06-01', N'HoatDong'),
((SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'khoa_test'),
 N'Phạm Văn Thử', N'0905444555', N'thu.pv@santhethao.vn',
 N'5 Láng Hạ, Đống Đa, Hà Nội', N'Nhân viên thử việc', '2026-02-10', N'DaNghi');
GO

/* ========================= 3. LOẠI SÂN & SÂN ========================= */
INSERT INTO LOAI_SAN (TenLoaiSan, MoTa) VALUES
(N'Sân bóng đá 5 người', N'Sân cỏ nhân tạo 5v5, kích thước 40x20m'),
(N'Sân bóng đá 7 người', N'Sân cỏ nhân tạo 7v7, kích thước 55x35m'),
(N'Sân cầu lông',        N'Sân thảm PVC trong nhà, tiêu chuẩn thi đấu'),
(N'Sân bóng rổ',         N'Sân gỗ/nhựa trong nhà, tiêu chuẩn FIBA'),
(N'Sân pickleball',      N'Sân mới, phong trào 2025-2026');
GO

INSERT INTO SAN (TenSan, MaLoaiSan, DonGia, TrangThai, MoTa) VALUES
(N'Sân A1', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân bóng đá 5 người'), 400000, N'Trong',  N'Sân 5 người mặt cỏ mới, gần cổng chính'),
(N'Sân A2', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân bóng đá 5 người'), 400000, N'Trong',  N'Sân 5 người, có mái che một phần'),
(N'Sân A3', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân bóng đá 5 người'), 380000, N'Trong',  N'Sân 5 người khu phụ, giá mềm hơn'),
(N'Sân B1', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân bóng đá 7 người'), 600000, N'Trong',  N'Sân 7 người, đèn chiếu sáng đầy đủ'),
(N'Sân B2', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân bóng đá 7 người'), 600000, N'BaoTri',N'Đang thay cỏ nhân tạo'),
(N'Sân C1', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân cầu lông'),        120000, N'Trong',  N'Sân cầu lông số 1'),
(N'Sân C2', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân cầu lông'),        120000, N'Trong',  N'Sân cầu lông số 2'),
(N'Sân C3', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân cầu lông'),        110000, N'Trong',  N'Sân cầu lông số 3 (khối học sinh)'),
(N'Sân D1', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân bóng rổ'),         180000, N'Trong',  N'Sân bóng rổ trong nhà'),
(N'Sân E1', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân pickleball'),      150000, N'Trong',  N'Sân pickleball mới khai trương');
GO

/* ============================ 4. KHÁCH HÀNG ============================ */
INSERT INTO KHACH_HANG (HoTen, SDT, Email, DiaChi, MaTK) VALUES
(N'Nguyễn Văn An',  N'0987654321', N'an.nv@gmail.com',   N'25 Lê Lợi, Hà Nội',        (SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'khach1')),
(N'Lê Hoàng Nam',   N'0912345678', N'nam.lh@gmail.com',  N'8 Trần Hưng Đạo, Hà Nội',  (SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'khach2')),
(N'Phạm Minh Châu', N'0909888777', N'chau.pm@gmail.com', N'45 Bà Triệu, Hà Nội',      NULL),
(N'Vũ Thị Hồng',    N'0977111222', N'hong.vt@gmail.com', N'3 Phố Huế, Hà Nội',        NULL),
(N'Hoàng Anh Tuấn', N'0933222111', N'tuan.ha@gmail.com', N'72 Xuân Thủy, Hà Nội',     NULL),
(N'Bùi Khánh Ly',   N'0944555666', N'ly.bk@gmail.com',   N'9 Kim Mã, Hà Nội',         NULL),
(N'Đỗ Mạnh Cường',  N'0966777888', N'cuong.dm@gmail.com',N'31 Giải Phóng, Hà Nội',    NULL),
(N'Ngô Thu Hà',     N'0988999000', N'ha.nt@gmail.com',   N'16 Xã Đàn, Hà Nội',        NULL);
GO

/* ============================== 5. VOUCHER ==============================
   Ngày hiệu lực tính ĐỘNG theo hôm nay để dữ liệu mẫu không bao giờ "hết hạn". */
INSERT INTO VOUCHER (MaCode, TenVoucher, LoaiGiam, GiaTriGiam, DonToiThieu,
                     SoLuong, SoLuongDaDung, NgayBatDau, NgayKetThuc, TrangThai, MoTa) VALUES
(N'GIAM20',   N'Giảm 20% tiền sân',         N'PhanTram', 20,    0,      200, 0,
 CAST(GETDATE() AS DATE), DATEADD(DAY, 180, CAST(GETDATE() AS DATE)), N'HoatDong',
 N'Áp dụng toàn bộ sân, không giới hạn giá trị đơn'),
(N'GIAM50K',  N'Giảm 50.000 cho đơn 300k',  N'SoTien',   50000, 300000, 50,  0,
 CAST(GETDATE() AS DATE), DATEADD(DAY, 120, CAST(GETDATE() AS DATE)), N'HoatDong',
 N'Đơn tối thiểu 300.000 đ'),
(N'KHAIXUAN', N'Voucher khai xuân',         N'PhanTram', 15,    200000, 30,  0,
 DATEADD(DAY, -60, CAST(GETDATE() AS DATE)), DATEADD(DAY, 60, CAST(GETDATE() AS DATE)), N'HoatDong',
 N'Chương trình đầu năm'),
(N'THANHVIEN',N'Ưu đãi khách thân thiết',   N'PhanTram', 10,    0,      100, 0,
 CAST(GETDATE() AS DATE), DATEADD(DAY, 365, CAST(GETDATE() AS DATE)), N'HoatDong',
 N'Dành cho khách đặt từ 5 lượt trở lên'),
(N'HETHAN',   N'Voucher đã hết hạn',        N'PhanTram', 30,    0,      10,  0,
 DATEADD(DAY, -400, CAST(GETDATE() AS DATE)), DATEADD(DAY, -300, CAST(GETDATE() AS DATE)), N'HoatDong',
 N'Dữ liệu mẫu: dùng để thử nghiệm kiểm tra hạn sử dụng'),
(N'TAMNGUNG', N'Voucher đang tạm ngưng',    N'SoTien',   100000, 500000, 20, 0,
 DATEADD(DAY, -30, CAST(GETDATE() AS DATE)), DATEADD(DAY, 90, CAST(GETDATE() AS DATE)), N'TamNgung',
 N'Data mẫu cho trạng thái TamNgung');
GO

/* ============================ 6. KHUYẾN MÃI ============================ */
INSERT INTO KHUYEN_MAI (TenKM, LoaiKhuyenMai, PhanTramGiam, NgayBatDau, NgayKetThuc,
                        ApDungCuoiTuan, TrangThai, MoTa) VALUES
(N'Giảm giá giờ vàng tháng này', N'GioVang',  15,
 CAST(GETDATE() AS DATE), DATEADD(DAY, 30, CAST(GETDATE() AS DATE)), 0, N'HoatDong',
 N'Giảm 15% cho các booking trong kỳ'),
(N'Khuyến mãi cuối tuần vàng',   N'CuoiTuan', 25,
 DATEADD(DAY, -10, CAST(GETDATE() AS DATE)), DATEADD(DAY, 45, CAST(GETDATE() AS DATE)), 1, N'HoatDong',
 N'Giảm 25% nhưng chỉ xét Thứ Bảy / Chủ Nhật'),
(N'Chương trình đã kết thúc',    N'MuaHe',    20,
 DATEADD(DAY, -200, CAST(GETDATE() AS DATE)), DATEADD(DAY, -120, CAST(GETDATE() AS DATE)), 0, N'TamNgung',
 N'Dữ liệu mẫu cho khuyến mãi hết hiệu lực');
GO

/* ============================== 7. THAM SỐ ============================== */
INSERT INTO THAM_SO (TenThamSo, GiaTri, MoTa) VALUES
(N'PhanTramGiamCuoiTuan', N'10',       N'% giảm tự động cho booking vào Thứ Bảy / Chủ Nhật'),
(N'ThoiLuongBlockPhut',   N'30',       N'Thời lượng 1 block tính tiền (17:00-18:10 = 1.5 giờ)'),
(N'GioMoCua',             N'05:00',    N'Giờ mở cửa'),
(N'GioDongCua',           N'23:00',    N'Giờ đóng cửa'),
(N'TenTrungTam',          N'TRUNG TÂM THỂ THAO HOÀNG GIA', N'Tên in trên hóa đơn'),
(N'DiaChi',               N'Số 1 Đường Thể Thao, Quận Hoàn Kiếm, Hà Nội', N'Địa chỉ in trên hóa đơn'),
(N'DienThoai',            N'024 3888 9999', N'Số điện thoại in trên hóa đơn'),
(N'LoiChaoHoaDon',        N'Cảm ơn quý khách, hẹn gặp lại!', N'Dòng cuối hóa đơn'),
(N'ThoiGianHuyToiDaGio',  N'24',       N'Số giờ tối thiểu trước giờ chơi để khách được tự hủy'),
(N'SoNgayDatTruoc',       N'30',       N'Cho phép đặt trước tối đa bao nhiêu ngày');
GO

/* ======================================================================= */
/* 8. SINH BOOKING: 30 ngày quá khứ -> hôm nay -> 7 ngày tương lai          */
/*    Tiền sân tính bằng fn_TienSan (đúng công thức ứng dụng).              */
/* ======================================================================= */
DECLARE @homNay   DATE = CAST(GETDATE() AS DATE);
DECLARE @gioBayGio TIME(0) = CAST(GETDATE() AS TIME(0));
DECLARE @ngay     DATE = DATEADD(DAY, -30, @homNay);
DECLARE @ngayCuoi DATE = DATEADD(DAY, 7, @homNay);

DECLARE @khach TABLE (stt INT IDENTITY(1,1), MaKH INT);
INSERT INTO @khach (MaKH) SELECT MaKH FROM KHACH_HANG ORDER BY MaKH;

DECLARE @cauHinhLich TABLE (stt INT IDENTITY(1,1), TenSan NVARCHAR(100),
                            GioBatDau TIME(0), GioKetThuc TIME(0));
INSERT INTO @cauHinhLich (TenSan, GioBatDau, GioKetThuc) VALUES
(N'Sân A1', '17:00', '19:00'),
(N'Sân A2', '18:00', '20:00'),
(N'Sân B1', '19:00', '21:00'),
(N'Sân C1', '06:00', '08:00'),
(N'Sân C3', '17:30', '19:00'),
(N'Sân D1', '18:00', '19:30'),
(N'Sân E1', '19:00', '20:30');

DECLARE @soKhach INT = (SELECT COUNT(1) FROM @khach);
DECLARE @soKhung INT = (SELECT COUNT(1) FROM @cauHinhLich);
DECLARE @dem INT = 0, @i INT, @maSan INT, @maKH INT;
DECLARE @bd TIME(0), @kt TIME(0), @tien DECIMAL(18,2), @trangThai NVARCHAR(20);
DECLARE @maAdmin INT = (SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'admin');
DECLARE @maNV    INT = (SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'nhanvien');

WHILE @ngay <= @ngayCuoi
BEGIN
    SET @i = 1;
    WHILE @i <= @soKhung
    BEGIN
        /* Bỏ bớt một số khung để lịch không "kín đặc" như dữ liệu giả. */
        IF ((DATEPART(DAY, @ngay) + @i) % 4) <> 0
        BEGIN
            SET @maSan = (SELECT MaSan FROM SAN WHERE TenSan = (SELECT TenSan FROM @cauHinhLich WHERE stt = @i));

            IF @maSan IS NOT NULL
               AND (SELECT TrangThai FROM SAN WHERE MaSan = @maSan) <> N'BaoTri'
            BEGIN
                SELECT @bd = GioBatDau, @kt = GioKetThuc FROM @cauHinhLich WHERE stt = @i;
                SET @maKH = (SELECT MaKH FROM @khach WHERE stt = (@dem % @soKhach) + 1);
                SET @tien = dbo.fn_TienSan(@maSan, @bd, @kt);

                SET @trangThai = CASE
                    WHEN @ngay <  @homNay THEN N'HoanThanh'
                    WHEN @ngay >  @homNay THEN N'DaDat'
                    WHEN @gioBayGio >= @kt THEN N'HoanThanh'
                    WHEN @gioBayGio >= @bd THEN N'DangSuDung'
                    ELSE N'DaDat' END;

                /* Khoảng 1/12 lượt đã qua bị hủy — đủ để demo bộ lọc "Đã hủy". */
                IF @trangThai = N'HoanThanh' AND (@dem % 12) = 5 SET @trangThai = N'DaHuy';

                INSERT INTO DAT_SAN
                    (MaKH, MaSan, NgayDat, GioBatDau, GioKetThuc, TienSan,
                     TrangThai, GhiChu, NgayTao, MaNguoiTao)
                VALUES
                    (@maKH, @maSan, @ngay, @bd, @kt, @tien, @trangThai,
                     CASE WHEN @dem % 9 = 0 THEN N'Khách quen, ưu tiên khung giờ đẹp' ELSE NULL END,
                     DATEADD(HOUR, -24, CAST(@ngay AS DATETIME) + CAST(@bd AS DATETIME)),
                     CASE WHEN @dem % 3 = 0 THEN @maAdmin ELSE @maNV END);

                SET @dem = @dem + 1;
            END
        END
        SET @i = @i + 1;
    END
    SET @ngay = DATEADD(DAY, 1, @ngay);
END

PRINT N'== Đã sinh ' + CAST(@dem AS NVARCHAR(10)) + N' booking ==';
GO

/* ======================================================================= */
/* 9. HÓA ĐƠN cho các booking đã hoàn thành                                */
/*    Ưu tiên giảm giá: Voucher > Cuối tuần (giống TinhTienService).        */
/* ======================================================================= */
DECLARE @giamCuoiTuan DECIMAL(5,2) =
    ISNULL(TRY_CAST(dbo.fn_LayThamSo(N'PhanTramGiamCuoiTuan', N'10') AS DECIMAL(5,2)), 10);
DECLARE @voucherGiam20 INT = (SELECT MaVoucher FROM VOUCHER WHERE MaCode = N'GIAM20');
DECLARE @maAdmin INT = (SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'admin');
DECLARE @maNV    INT = (SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'nhanvien');
DECLARE @maNV2   INT = (SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'nhanvien2');

INSERT INTO HOA_DON
    (MaDat, NgayLap, TienGoc, LoaiGiamGia, TienGiam, TongTien,
     PhuongThucThanhToan, TrangThai, MaNguoiLap, MaVoucher, GhiChu)
SELECT
    ds.MaDat,
    DATEADD(MINUTE, 5, CAST(ds.NgayDat AS DATETIME) + CAST(ds.GioKetThuc AS DATETIME)),
    ds.TienSan,
    x.LoaiGiam,
    x.TienGiam,
    ds.TienSan - x.TienGiam,
    CASE ds.MaDat % 3 WHEN 0 THEN N'TienMat' WHEN 1 THEN N'ChuyenKhoan' ELSE N'The' END,
    N'DaThanhToan',
    CASE ds.MaDat % 3 WHEN 0 THEN @maAdmin WHEN 1 THEN @maNV ELSE @maNV2 END,
    CASE WHEN x.LoaiGiam = N'Voucher' THEN @voucherGiam20 ELSE NULL END,
    CASE WHEN x.LoaiGiam = N'Voucher' THEN N'Áp dụng mã GIAM20' ELSE NULL END
FROM DAT_SAN ds
CROSS APPLY (SELECT
    LoaiGiam = CASE
                   WHEN ds.MaDat % 5 = 0                 THEN N'Voucher'
                   WHEN dbo.fn_LaCuoiTuan(ds.NgayDat) = 1 THEN N'CuoiTuan'
                   ELSE N'Khong' END,
    TienGiam = CASE
                   WHEN ds.MaDat % 5 = 0
                       THEN CAST(ROUND(ds.TienSan * 20.0 / 100, 0) AS DECIMAL(18,2))
                   WHEN dbo.fn_LaCuoiTuan(ds.NgayDat) = 1
                       THEN CAST(ROUND(ds.TienSan * @giamCuoiTuan / 100, 0) AS DECIMAL(18,2))
                   ELSE CAST(0 AS DECIMAL(18,2)) END) x
WHERE ds.TrangThai = N'HoanThanh';

/* 3 hóa đơn mới nhất: chưa thanh toán — phục vụ demo nút "Thanh toán". */
UPDATE hd
SET hd.TrangThai = N'ChuaThanhToan', hd.PhuongThucThanhToan = N'TienMat'
FROM HOA_DON hd
WHERE hd.MaHD IN (SELECT TOP (3) MaHD FROM HOA_DON ORDER BY NgayLap DESC);

PRINT N'== Đã lập ' + CAST((SELECT COUNT(1) FROM HOA_DON) AS NVARCHAR(10)) + N' hóa đơn ==';
GO

/* ================== 10. LỊCH SỬ DÙNG VOUCHER + TỒN KHO ================== */
INSERT INTO SU_DUNG_VOUCHER (MaVoucher, MaDat, MaKH, SoTienGiam, NgaySuDung)
SELECT hd.MaVoucher, hd.MaDat, ds.MaKH, hd.TienGiam, hd.NgayLap
FROM HOA_DON hd
     INNER JOIN DAT_SAN ds ON ds.MaDat = hd.MaDat
WHERE hd.LoaiGiamGia = N'Voucher' AND hd.MaVoucher IS NOT NULL;
GO

UPDATE v
SET v.SoLuongDaDung = (SELECT COUNT(1) FROM SU_DUNG_VOUCHER sdv WHERE sdv.MaVoucher = v.MaVoucher)
FROM VOUCHER v;
GO

/* ========================= 11. NHẬT KÝ HOẠT ĐỘNG ========================= */
DECLARE @maAdmin INT = (SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'admin');
DECLARE @maNV    INT = (SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'nhanvien');
DECLARE @maKhach INT = (SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'khach1');

INSERT INTO NHAT_KY_HOAT_DONG
    (MaTK, TenDangNhap, VaiTro, HoatDong, BangDuLieu, MaDuLieu, NoiDung, KetQua, ThoiGian, MayTram)
SELECT tk.MaTK, tk.TenDangNhap, tk.VaiTro, n.HoatDong, n.BangDuLieu, n.MaDuLieu,
       n.NoiDung, n.KetQua, DATEADD(MINUTE, n.LePhut, GETDATE()), n.MayTram
FROM (VALUES
    (@maAdmin, N'DangNhap',     N'TAIKHOAN',  NULL, N'Đăng nhập hệ thống',                 N'ThanhCong', -45,  N'MAY-ADMIN-01'),
    (@maNV,    N'DangNhap',     N'TAIKHOAN',  NULL, N'Đăng nhập hệ thống',                 N'ThanhCong', -40,  N'QUAY-LETAN-02'),
    (@maNV,    N'ThemDatSan',   N'DAT_SAN',   N'1', N'Đặt sân cho khách Nguyễn Văn An',    N'ThanhCong', -38,  N'QUAY-LETAN-02'),
    (@maNV,    N'LapHoaDon',    N'HOA_DON',   N'1', N'Lập hóa đơn từ booking #1',          N'ThanhCong', -30,  N'QUAY-LETAN-02'),
    (@maKhach, N'ThemDatSan',   N'DAT_SAN',   N'2', N'Khách tự đặt sân qua cổng khách hàng',N'ThanhCong', -25, N'WEB-KHACH'),
    (@maNV,    N'ThanhToan',    N'HOA_DON',   N'1', N'Thu tiền mặt',                       N'ThanhCong', -20,  N'QUAY-LETAN-02'),
    (@maAdmin, N'DoiTrangThaiSan', N'SAN',    N'5', N'Chuyển Sân B2 sang Bảo trì',          N'ThanhCong', -15,  N'MAY-ADMIN-01'),
    (@maNV,    N'ThemDatSan',   N'DAT_SAN',   N'3', N'Trùng lịch với booking khác',         N'ThatBai',  -12,  N'QUAY-LETAN-02'),
    (@maAdmin, N'DoiMatKhau',   N'TAIKHOAN',  NULL, N'Đổi mật khẩu quản trị viên',          N'ThanhCong', -8,   N'MAY-ADMIN-01'),
    (@maAdmin, N'XoaVoucher',   N'VOUCHER',   NULL, N'Ngưng voucher sắp hết lượt',          N'CanhBao',  -5,   N'MAY-ADMIN-01')
) AS n (MaTK, HoatDong, BangDuLieu, MaDuLieu, NoiDung, KetQua, LePhut, MayTram)
INNER JOIN TAIKHOAN tk ON tk.MaTK = n.MaTK;
GO

/* ============ 12. ĐỒNG BỘ TRẠNG THÁI (giống ứng dụng lúc mở form) ============ */
DECLARE @soBanGhi INT = 0;
EXEC dbo.sp_CapNhatTrangThaiBooking @TuDongHoanThanh = 0, @SoBanGhi = @soBanGhi OUTPUT;
GO

/* ============================ 13. KIỂM TRA ============================ */
SELECT N'Tài khoản'  AS Nhom, COUNT(1) AS SoDong FROM TAIKHOAN
UNION ALL SELECT N'Nhân viên (gồm Admin)', COUNT(1) FROM NHAN_VIEN
UNION ALL SELECT N'Khách hàng',            COUNT(1) FROM KHACH_HANG
UNION ALL SELECT N'Loại sân',              COUNT(1) FROM LOAI_SAN
UNION ALL SELECT N'Sân',                   COUNT(1) FROM SAN
UNION ALL SELECT N'Booking',               COUNT(1) FROM DAT_SAN
UNION ALL SELECT N'Hóa đơn',               COUNT(1) FROM HOA_DON
UNION ALL SELECT N'Lượt dùng voucher',     COUNT(1) FROM SU_DUNG_VOUCHER
UNION ALL SELECT N'Voucher',               COUNT(1) FROM VOUCHER
UNION ALL SELECT N'Khuyến mãi',            COUNT(1) FROM KHUYEN_MAI
UNION ALL SELECT N'Tham số',               COUNT(1) FROM THAM_SO
UNION ALL SELECT N'Quyền',                 COUNT(1) FROM QUYEN
UNION ALL SELECT N'Ma trận vai trò-quyền', COUNT(1) FROM VAI_TRO_QUYEN
UNION ALL SELECT N'Nhật ký hoạt động',     COUNT(1) FROM NHAT_KY_HOAT_DONG;
GO

/* Hồ sơ người dùng: phải KHÔNG còn dòng "Thiếu hồ sơ nhân viên" cho Admin. */
SELECT MaTK, TenDangNhap, HoTen, VaiTro, TrangThaiTaiKhoan, MaNV, ChucVu, TinhTrangHoSo
FROM v_NguoiDung
ORDER BY CASE VaiTro WHEN N'Admin' THEN 1 WHEN N'NhanVien' THEN 2 ELSE 3 END, TenDangNhap;
GO

/* Doanh thu 7 ngày gần nhất (nguồn cho biểu đồ dashboard). */
SELECT TOP (7) Ngay, SoHoaDon, DoanhThu, DaThu, ConNo
FROM v_DoanhThuTheoNgay
ORDER BY Ngay DESC;
GO

PRINT N'== HOÀN TẤT 02_DuLieuMau_v2.sql ==';
PRINT N'   Đăng nhập thử: admin/admin123 | nhanvien/nv123456 | khach1/kh123456';
GO
