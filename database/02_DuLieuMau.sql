/*
  Quản lý cho thuê sân thể thao - Dữ liệu mẫu (chạy SAU 01_TaoCSDL.sql)
  ----------------------------------------------------------------------
  Tài khoản demo (mật khẩu đã băm PBKDF2, định dạng iterations.salt.hash):
     admin    / admin123   -> Quản trị viên (toàn quyền)
     nhanvien / nv123456   -> Nhân viên
     khach1   / kh123456   -> Khách hàng (Nguyễn Văn An)
  Ứng dụng vẫn chấp nhận mật khẩu dạng thô nếu cột MatKhau không đúng định dạng
  (cơ chế dự phòng trong PasswordHasher), nên có thể sửa tay thành 'admin123'.

  Dữ liệu phục vụ đúng kịch bản demo:
     - Sân A1 (Sân bóng đá 5 người) giá 400.000 đ/giờ
     - Voucher GIAM20: giảm 20% (thắng giảm cuối tuần 10% theo độ ưu tiên)
     - THAM_SO: PhanTramGiamCuoiTuan = 10, ThoiLuongBlockPhut = 30
*/

USE QLSanTheThao;
GO

SET NOCOUNT ON;
GO

/* ------------------------------ TÀI KHOẢN ------------------------------ */
INSERT INTO TAIKHOAN (TenDangNhap, MatKhau, HoTen, VaiTro, TrangThai) VALUES
(N'admin',
 N'100000.yGnbndaG9aF5wmkGfJVbiw==.2kfLmCa+dnHyly5rG3T5vzl+n0Il/lkK+frdnpsw4PQ=',
 N'Quản Trị Viên', N'Admin', N'HoatDong'),
(N'nhanvien',
 N'100000.KjxMMIrSfCf3HhzrkEs5DQ==.g9LsgqJwv+J82byKmZOYcj9eTNiGLH0a1xt2kVWzA7E=',
 N'Trần Thị Thu Ngân', N'NhanVien', N'HoatDong'),
(N'khach1',
 N'100000.0jYIlIIiMUPpZnJWq81pVw==.kHsGrbXy1vuYRzF5IlgNReUPzYApUsZojT1WlpXippY=',
 N'Nguyễn Văn An', N'KhachHang', N'HoatDong');
GO

/* ------------------------------ NHÂN VIÊN ------------------------------ */
INSERT INTO NHAN_VIEN (MaTK, HoTen, SDT, Email, DiaChi, ChucVu, NgayVaoLam, TrangThai)
VALUES ((SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'nhanvien'),
        N'Trần Thị Thu Ngân', N'0905123456', N'ngan.tt@santhethao.vn',
        N'12 Nguyễn Trãi, Hà Nội', N'Nhân viên lễ tân', '2025-01-15', N'HoatDong');
GO

/* ---------------------------- LOẠI SÂN & SÂN --------------------------- */
INSERT INTO LOAI_SAN (TenLoaiSan, MoTa) VALUES
(N'Sân bóng đá 5 người', N'Sân cỏ nhân tạo 5v5, kích thước 40x20m'),
(N'Sân bóng đá 7 người', N'Sân cỏ nhân tạo 7v7, kích thước 55x35m'),
(N'Sân cầu lông',       N'Sân thảm PVC trong nhà, tiêu chuẩn thi đấu'),
(N'Sân bóng rổ',        N'Sân gỗ/nhựa trong nhà, tiêu chuẩn FIBA');
GO

INSERT INTO SAN (TenSan, MaLoaiSan, DonGia, TrangThai, MoTa) VALUES
(N'Sân A1', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân bóng đá 5 người'), 400000, N'Trong', N'Sân 5 người mặt cỏ mới, gần cổng chính'),
(N'Sân A2', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân bóng đá 5 người'), 400000, N'Trong', N'Sân 5 người, có mái che một phần'),
(N'Sân B1', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân bóng đá 7 người'), 600000, N'Trong', N'Sân 7 người, đèn chiếu sáng đầy đủ'),
(N'Sân C1', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân cầu lông'),        120000, N'Trong', N'Sân cầu lông số 1'),
(N'Sân C2', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân cầu lông'),        120000, N'BaoTri', N'Đang thay lưới + vệ sinh mặt sân'),
(N'Sân D1', (SELECT MaLoaiSan FROM LOAI_SAN WHERE TenLoaiSan = N'Sân bóng rổ'),         180000, N'Trong', N'Sân bóng rổ trong nhà');
GO

/* ------------------------------ KHÁCH HÀNG ----------------------------- */
INSERT INTO KHACH_HANG (HoTen, SDT, Email, DiaChi, MaTK) VALUES
(N'Nguyễn Văn An',   N'0987654321', N'an.nv@gmail.com',   N'25 Lê Lợi, Hà Nội',      (SELECT MaTK FROM TAIKHOAN WHERE TenDangNhap = N'khach1')),
(N'Lê Hoàng Nam',    N'0912345678', N'nam.lh@gmail.com',  N'8 Trần Hưng Đạo, Hà Nội', NULL),
(N'Phạm Minh Châu',  N'0909888777', N'chau.pm@gmail.com', N'45 Bà Triệu, Hà Nội',     NULL),
(N'Vũ Thị Hồng',     N'0977111222', N'hong.vt@gmail.com', N'3 Phố Huế, Hà Nội',       NULL);
GO

/* -------------------------------- VOUCHER ------------------------------ */
INSERT INTO VOUCHER (MaCode, TenVoucher, LoaiGiam, GiaTriGiam, DonToiThieu,
                     SoLuong, SoLuongDaDung, NgayBatDau, NgayKetThuc, TrangThai, MoTa) VALUES
(N'GIAM20',  N'Giảm 20% tiền sân',        N'PhanTram', 20,      0,       100, 0, '2026-01-01', '2026-12-31', N'HoatDong', N'Áp dụng toàn bộ sân, không giới hạn giá trị đơn'),
(N'GIAM50K', N'Giảm 50.000 cho đơn 300k', N'SoTien',   50000,   300000,  50,  0, '2026-01-01', '2026-12-31', N'HoatDong', N'Đơn tối thiểu 300.000 đ'),
(N'KHAIXUAN',N'Voucher khai xuân',        N'PhanTram', 15,      200000,  20,  0, '2026-01-01', '2026-06-30', N'HoatDong', N'Chương trình đầu năm'),
(N'HETHAN',  N'Voucher đã hết hạn',       N'PhanTram', 30,      0,       10,  0, '2025-01-01', '2025-06-30', N'HoatDong', N'Dữ liệu mẫu: dùng để thử kiểm tra hạn sử dụng');
GO

/* ------------------------------ KHUYẾN MÃI ----------------------------- */
INSERT INTO KHUYEN_MAI (TenKM, LoaiKhuyenMai, PhanTramGiam, NgayBatDau, NgayKetThuc,
                        ApDungCuoiTuan, TrangThai, MoTa) VALUES
(N'Giảm giá giờ vàng tháng 9', N'GioVang', 15, '2026-09-01', '2026-09-30', 0, N'HoatDong',
 N'Giảm 15% cho các booking trong tháng 9'),
(N'Khuyến mãi cuối tuần vàng', N'CuoiTuan', 25, '2026-09-01', '2026-09-30', 1, N'HoatDong',
 N'Giảm 25% nhưng chỉ xét cho Thứ Bảy / Chủ Nhật');
GO

/* -------------------------------- THAM SỐ ------------------------------ */
INSERT INTO THAM_SO (TenThamSo, GiaTri, MoTa) VALUES
(N'PhanTramGiamCuoiTuan', N'10',        N'% giảm tự động cho booking vào Thứ Bảy / Chủ Nhật'),
(N'ThoiLuongBlockPhut',   N'30',        N'Thời lượng 1 block tính tiền (17:00-18:10 = 1.5 giờ)'),
(N'GioMoCua',             N'05:00',     N'Giờ mở cửa'),
(N'GioDongCua',           N'23:00',     N'Giờ đóng cửa'),
(N'TenTrungTam',          N'TRUNG TÂM THỂ THAO HOÀNG GIA', N'Tên in trên hóa đơn'),
(N'DiaChi',               N'Số 1 Đường Thể Thao, Quận Hoàn Kiếm, Hà Nội', N'Địa chỉ in trên hóa đơn'),
(N'DienThoai',            N'024 3888 9999', N'Số điện thoại in trên hóa đơn'),
(N'LoiChaoHoaDon',        N'Cảm ơn quý khách, hẹn gặp lại!', N'Dòng cuối hóa đơn');
GO

PRINT N'== Đã nạp dữ liệu mẫu: 3 tài khoản, 1 nhân viên, 4 loại sân, 6 sân, 4 khách, 4 voucher, 2 khuyến mãi ==';
GO
