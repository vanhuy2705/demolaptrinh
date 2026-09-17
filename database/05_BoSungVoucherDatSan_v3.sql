/* =====================================================================
   05_BoSungVoucherDatSan_v3.sql
   Nâng CSDL ĐANG CHẠY lên bản v3: lưu voucher ngay trên booking.

   Chạy file này khi bạn ĐÃ có CSDL từ 01_TaoCSDL_v2.sql hoặc từ bản cũ.
   (Cài mới hoàn toàn thì KHÔNG cần - 01_TaoCSDL_v2.sql đã có sẵn.)

   Idempotent: chạy lại bao nhiêu lần cũng được, không mất dữ liệu.
   Tương thích SQL Server 2016 trở lên.

   Lý do: trước đây khách chọn voucher ở màn "Đặt sân", ứng dụng báo giá
   đã giảm, nhưng DAT_SAN không có chỗ lưu voucher -> sang bước lập hóa
   đơn phải nhập lại mã, quên là khách mất ưu đãi.
   ===================================================================== */

IF DB_ID(N'QLSanTheThao') IS NULL
    RAISERROR(N'Chưa có CSDL QLSanTheThao - hãy chạy 01_TaoCSDL_v2.sql (hoặc 04_NangCapCSDL_v2.sql) trước.', 16, 1);
GO

USE QLSanTheThao;
GO

SET NOCOUNT ON;
GO

PRINT N'== 05: Bổ sung DAT_SAN.MaVoucher ==';
GO

/* 1. Cột MaVoucher trên DAT_SAN -------------------------------------- */
IF COL_LENGTH(N'dbo.DAT_SAN', N'MaVoucher') IS NULL
BEGIN
    ALTER TABLE dbo.DAT_SAN ADD MaVoucher INT NULL;
    PRINT N'  + Đã thêm cột DAT_SAN.MaVoucher';
END
ELSE
    PRINT N'  = Cột DAT_SAN.MaVoucher đã có, bỏ qua';
GO

/* 2. Khoá ngoại (chỉ thêm khi bảng VOUCHER đã tồn tại) ---------------- */
IF OBJECT_ID(N'dbo.VOUCHER', N'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_DAT_SAN_VOUCHER')
BEGIN
    ALTER TABLE dbo.DAT_SAN
        ADD CONSTRAINT FK_DAT_SAN_VOUCHER FOREIGN KEY (MaVoucher) REFERENCES dbo.VOUCHER(MaVoucher);
    PRINT N'  + Đã thêm khoá ngoại FK_DAT_SAN_VOUCHER';
END
ELSE
    PRINT N'  = FK_DAT_SAN_VOUCHER đã có (hoặc thiếu bảng VOUCHER), bỏ qua';
GO

/* 3. Index tra cứu theo voucher --------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_DAT_SAN_Voucher' AND object_id = OBJECT_ID(N'dbo.DAT_SAN'))
BEGIN
    CREATE INDEX IX_DAT_SAN_Voucher ON dbo.DAT_SAN(MaVoucher);
    PRINT N'  + Đã tạo index IX_DAT_SAN_Voucher';
END
ELSE
    PRINT N'  = Index IX_DAT_SAN_Voucher đã có, bỏ qua';
GO

/* 4. Hồi tố dữ liệu: booking đã có hóa đơn dùng voucher thì chép ngược
      mã voucher từ HOA_DON lên DAT_SAN, để các booking cũ cũng được
      hưởng cơ chế tự động ở bước lập hóa đơn.                          */
IF COL_LENGTH(N'dbo.HOA_DON', N'MaVoucher') IS NOT NULL
BEGIN
    UPDATE ds
       SET ds.MaVoucher = hd.MaVoucher
      FROM dbo.DAT_SAN ds
      INNER JOIN dbo.HOA_DON hd ON hd.MaDat = ds.MaDat
     WHERE ds.MaVoucher IS NULL
       AND hd.MaVoucher IS NOT NULL;
    PRINT N'  + Đã hồi tố MaVoucher cho ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + N' booking cũ';
END
GO

/* 5. Đối soát: voucher đã dùng (SU_DUNG_VOUCHER) mà booking chưa ghi nhận */
UPDATE ds
   SET ds.MaVoucher = sd.MaVoucher
  FROM dbo.DAT_SAN ds
  INNER JOIN dbo.SU_DUNG_VOUCHER sd ON sd.MaDat = ds.MaDat
 WHERE ds.MaVoucher IS NULL;

IF @@ROWCOUNT > 0
    PRINT N'  + Đã đối soát thêm ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + N' booking từ SU_DUNG_VOUCHER';
ELSE
    PRINT N'  = Không còn booking nào lệch với SU_DUNG_VOUCHER';
GO

PRINT N'== 05 hoàn tất. CSDL đã ở bản v3 (có DAT_SAN.MaVoucher). ==';
GO
