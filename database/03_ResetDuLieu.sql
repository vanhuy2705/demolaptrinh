/*
  Tiện ích: xóa toàn bộ dữ liệu nghiệp vụ, giữ lại cấu trúc + tài khoản + danh mục.
  Dùng trước khi demo lại từ đầu (chạy xong ứng dụng sẽ trống booking/hóa đơn).
*/
USE QLSanTheThao;
GO

SET NOCOUNT ON;

DELETE FROM SU_DUNG_VOUCHER;
DELETE FROM HOA_DON;
DELETE FROM DAT_SAN;
DELETE FROM KHACH_HANG WHERE MaTK IS NULL;      -- giữ khách có tài khoản đăng nhập

UPDATE VOUCHER SET SoLuongDaDung = 0;
UPDATE SAN SET TrangThai = N'Trong' WHERE TrangThai = N'DangThue';

DBCC CHECKIDENT ('DAT_SAN', RESEED, 0);
DBCC CHECKIDENT ('HOA_DON', RESEED, 0);
DBCC CHECKIDENT ('SU_DUNG_VOUCHER', RESEED, 0);
GO

PRINT N'== Đã reset dữ liệu nghiệp vụ (giữ tài khoản, sân, voucher, tham số) ==';
GO

/* ---- Các câu truy vấn kiểm tra nhanh trong quá trình demo ----
SELECT * FROM DAT_SAN;
SELECT * FROM HOA_DON;
SELECT * FROM SU_DUNG_VOUCHER;
SELECT TenThamSo, GiaTri FROM THAM_SO;
SELECT v.MaCode, v.SoLuong, v.SoLuongDaDung FROM VOUCHER v;
*/
