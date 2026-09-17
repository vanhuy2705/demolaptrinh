/*
  Quản lý cho thuê sân thể thao — NÂNG CẤP CSDL HIỆN CÓ LÊN BẢN v2
  =================================================================
  Dành cho ai ĐÃ chạy 01_TaoCSDL.sql + 02_DuLieuMau.sql và MUỐN GIỮ DỮ LIỆU.
  (Nếu chấp nhận tạo lại từ đầu: chạy 01_TaoCSDL_v2.sql + 02_DuLieuMau_v2.sql.)

  Đặc điểm:
   • KHÔNG xóa bảng nào, KHÔNG thêm/bớt cột nào của 11 bảng nghiệp vụ gốc
     => ứng dụng C# hiện tại chạy y như cũ.
   • Chạy lại nhiều lần an toàn (idempotent): mọi CREATE đều có kiểm tra tồn tại.
   • Bổ sung: 4 bảng (VAI_TRO, QUYEN, VAI_TRO_QUYEN, NHAT_KY_HOAT_DONG),
     5 function, 8 view, 7 procedure, seed ma trận quyền, index phụ,
     và VÁ LỖI tài khoản Admin/NhanVien thiếu hồ sơ NHAN_VIEN.
   • Các ràng buộc CHECK mới của bản v2 (độ dài tên đăng nhập, % voucher…)
     cố tình KHÔNG thêm vào đây, vì dữ liệu cũ có thể không thỏa.
*/

USE QLSanTheThao;
GO

SET NOCOUNT ON;
GO

PRINT N'== Bắt đầu nâng cấp CSDL lên v2 ==';
GO

/* ====================== A. BỐN BẢNG MỚI ====================== */
IF OBJECT_ID(N'VAI_TRO', N'U') IS NULL
BEGIN
/* 10. MỚI — PHÂN QUYỀN ĐỘNG TRONG CSDL (RBAC)                           */
/*     Seed đúng ma trận của PhanQuyenService (C#) để sau này có thể      */
/*     cho Admin tự cấp quyền mà không cần biên dịch lại ứng dụng.        */
/* ===================================================================== */
CREATE TABLE VAI_TRO (
    MaVaiTro  NVARCHAR(20)  PRIMARY KEY,          -- 'Admin' | 'NhanVien' | 'KhachHang'
    TenVaiTro NVARCHAR(50)  NOT NULL,
    MoTa      NVARCHAR(255) NULL,
    LaMacDinh BIT           NOT NULL DEFAULT 0
);
END
GO

IF OBJECT_ID(N'QUYEN', N'U') IS NULL
BEGIN
CREATE TABLE QUYEN (
    MaQuyen   NVARCHAR(40)  PRIMARY KEY,          -- khớp hằng số MaQuyen trong HangSo.cs
    TenQuyen  NVARCHAR(100) NOT NULL,
    NhomQuyen NVARCHAR(50)  NOT NULL,             -- TaiKhoan, NhanVien, San, DatSan, HoaDon…
    MoTa      NVARCHAR(255) NULL
);
END
GO

IF OBJECT_ID(N'VAI_TRO_QUYEN', N'U') IS NULL
BEGIN
CREATE TABLE VAI_TRO_QUYEN (
    MaVaiTro NVARCHAR(20) NOT NULL,
    MaQuyen  NVARCHAR(40) NOT NULL,
    CONSTRAINT PK_VAI_TRO_QUYEN PRIMARY KEY (MaVaiTro, MaQuyen),
    CONSTRAINT FK_VTQ_VAITRO FOREIGN KEY (MaVaiTro) REFERENCES VAI_TRO(MaVaiTro) ON DELETE CASCADE,
    CONSTRAINT FK_VTQ_QUYEN  FOREIGN KEY (MaQuyen)  REFERENCES QUYEN(MaQuyen)    ON DELETE CASCADE
);
END
GO

IF OBJECT_ID(N'NHAT_KY_HOAT_DONG', N'U') IS NULL
BEGIN
/* ===================================================================== */
/* 11. MỚI — NHẬT KÝ HOẠT ĐỘNG (audit trail)                             */
/*     Hiện ứng dụng chỉ ghi MaNguoiTao/MaNguoiLap trên từng bản ghi.     */
/*     Bảng này cho biết AI làm GÌ, LÚC NÀO, KẾT QUẢ ra sao (kể cả lỗi).  */
/* ===================================================================== */
CREATE TABLE NHAT_KY_HOAT_DONG (
    MaNhatKy   BIGINT IDENTITY(1,1) PRIMARY KEY,
    MaTK       INT           NULL,                -- NULL: hệ thống / chưa đăng nhập
    TenDangNhap NVARCHAR(50) NULL,                -- lưu dự phòng để đọc log kể cả khi tài khoản bị xóa
    VaiTro     NVARCHAR(20)  NULL,
    HoatDong   NVARCHAR(50)  NOT NULL,            -- DangNhap, ThemDatSan, HuyHoaDon, DoiMatKhau…
    BangDuLieu NVARCHAR(50)  NULL,                -- DAT_SAN, HOA_DON, TAIKHOAN…
    MaDuLieu   NVARCHAR(30)  NULL,                -- mã bản ghi liên quan
    NoiDung    NVARCHAR(500) NULL,
    KetQua     NVARCHAR(20)  NOT NULL DEFAULT N'ThanhCong'
               CHECK (KetQua IN (N'ThanhCong', N'ThatBai', N'CanhBao')),
    ThoiGian   DATETIME      NOT NULL DEFAULT GETDATE(),
    MayTram    NVARCHAR(100) NULL,                -- tên máy / IP người thao tác
    CONSTRAINT FK_NHATKY_TAIKHOAN FOREIGN KEY (MaTK) REFERENCES TAIKHOAN(MaTK) ON DELETE SET NULL
);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_NHATKY_ThoiGian' AND object_id = OBJECT_ID(N'NHAT_KY_HOAT_DONG'))
    CREATE INDEX IX_NHATKY_ThoiGian ON NHAT_KY_HOAT_DONG(ThoiGian DESC);
    CREATE INDEX IX_NHATKY_TaiKhoan ON NHAT_KY_HOAT_DONG(MaTK, ThoiGian DESC);
    CREATE INDEX IX_NHATKY_HoatDong ON NHAT_KY_HOAT_DONG(HoatDong, ThoiGian DESC);
GO

/* ================== B. FUNCTION / VIEW / PROCEDURE ================== */
IF OBJECT_ID(N'fn_LayThamSo', N'FN') IS NOT NULL DROP FUNCTION fn_LayThamSo;
GO

/* 12. FUNCTION TIỆN ÍCH                                                 */
/* ===================================================================== */

/* Đọc một tham số cấu hình, có giá trị mặc định. */
CREATE FUNCTION fn_LayThamSo (@TenThamSo NVARCHAR(50), @MacDinh NVARCHAR(200))
RETURNS NVARCHAR(200)
AS
BEGIN
    DECLARE @GiaTri NVARCHAR(200);
    SELECT @GiaTri = GiaTri FROM THAM_SO WHERE TenThamSo = @TenThamSo;
    RETURN ISNULL(NULLIF(LTRIM(RTRIM(@GiaTri)), N''), @MacDinh);
END
GO

IF OBJECT_ID(N'fn_LaCuoiTuan', N'FN') IS NOT NULL DROP FUNCTION fn_LaCuoiTuan;
GO

/* Thứ Bảy / Chủ Nhật? — không phụ thuộc SET DATEFIRST.
   Mốc 2000-01-03 là một Thứ Hai => số ngày chênh lệch % 7: 0=Thứ Hai … 5=Thứ Bảy, 6=Chủ Nhật. */
CREATE FUNCTION fn_LaCuoiTuan (@Ngay DATE)
RETURNS BIT
AS
BEGIN
    IF @Ngay IS NULL RETURN 0;
    RETURN CASE WHEN (DATEDIFF(DAY, '20000103', @Ngay) % 7) IN (5, 6) THEN 1 ELSE 0 END;
END
GO

IF OBJECT_ID(N'fn_SoBlock', N'FN') IS NOT NULL DROP FUNCTION fn_SoBlock;
GO

/* Số block làm tròn lên: 70 phút / block 30 phút = 3 block. */
CREATE FUNCTION fn_SoBlock (@GioBatDau TIME(0), @GioKetThuc TIME(0), @PhutBlock INT)
RETURNS INT
AS
BEGIN
    DECLARE @soPhut INT = DATEDIFF(MINUTE, @GioBatDau, @GioKetThuc);
    IF @soPhut <= 0 RETURN 0;
    IF ISNULL(@PhutBlock, 0) <= 0 SET @PhutBlock = 30;
    RETURN CAST(CEILING(CAST(@soPhut AS FLOAT) / @PhutBlock) AS INT);
END
GO

IF OBJECT_ID(N'fn_TienSan', N'FN') IS NOT NULL DROP FUNCTION fn_TienSan;
GO

/* Tiền sân = đơn giá x số block x (phút block / 60), làm tròn đến đồng.
   Khớp công thức TinhTienService.TinhTienTheoBlock của C#.
   Lưu ý nhỏ: SQL ROUND() làm tròn ra xa 0, còn Math.Round() mặc định của C# làm tròn
   ngân hàng (chẵn) — chỉ lệch nhau đúng 1 đồng ở trường hợp x.5 rất hiếm. */
CREATE FUNCTION fn_TienSan (@MaSan INT, @GioBatDau TIME(0), @GioKetThuc TIME(0))
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @donGia DECIMAL(18,2) = (SELECT DonGia FROM SAN WHERE MaSan = @MaSan);
    DECLARE @phutBlock INT = TRY_CAST(dbo.fn_LayThamSo(N'ThoiLuongBlockPhut', N'30') AS INT);
    DECLARE @soBlock INT = dbo.fn_SoBlock(@GioBatDau, @GioKetThuc, @phutBlock);
    IF ISNULL(@donGia, 0) <= 0 OR @soBlock <= 0 RETURN 0;
    RETURN CAST(ROUND(@donGia * @soBlock * @phutBlock / 60.0, 0) AS DECIMAL(18,2));
END
GO

IF OBJECT_ID(N'fn_SoBookingTrungLich', N'FN') IS NOT NULL DROP FUNCTION fn_SoBookingTrungLich;
GO

/* Đếm số booking chồng lấn khung giờ của một sân (bỏ qua booking đã hủy). */
CREATE FUNCTION fn_SoBookingTrungLich (@MaSan INT, @NgayDat DATE,
                                      @GioBatDau TIME(0), @GioKetThuc TIME(0),
                                      @MaDatLoaiTru INT)
RETURNS INT
AS
BEGIN
    RETURN (SELECT COUNT(1)
            FROM DAT_SAN
            WHERE MaSan = @MaSan
              AND NgayDat = @NgayDat
              AND TrangThai <> N'DaHuy'
              AND (@MaDatLoaiTru IS NULL OR MaDat <> @MaDatLoaiTru)
              AND GioBatDau < @GioKetThuc
              AND GioKetThuc > @GioBatDau);
END
GO

IF OBJECT_ID(N'v_NguoiDung', N'V') IS NOT NULL DROP VIEW v_NguoiDung;
GO

/* ===================================================================== */
/* 13. VIEW                                                              */
/* ===================================================================== */

/* Một nơi để nhìn MỌI người dùng: tài khoản + hồ sơ (Admin, Nhân viên, Khách). */
CREATE VIEW v_NguoiDung AS
SELECT
    tk.MaTK,
    tk.TenDangNhap,
    tk.HoTen,
    tk.VaiTro,
    tk.TrangThai                 AS TrangThaiTaiKhoan,
    tk.NgayTao,
    nv.MaNV,
    nv.ChucVu,
    nv.SDT                       AS SDTNhanVien,
    nv.Email                     AS EmailNhanVien,
    nv.NgayVaoLam,
    nv.TrangThai                 AS TrangThaiNhanSu,
    kh.MaKH,
    kh.SDT                       AS SDTKhachHang,
    kh.Email                     AS EmailKhachHang,
    kh.DiaChi,
    CASE
        WHEN tk.VaiTro IN (N'Admin', N'NhanVien') AND nv.MaNV IS NULL
            THEN N'Thiếu hồ sơ nhân viên'
        WHEN tk.VaiTro = N'KhachHang' AND kh.MaKH IS NULL
            THEN N'Thiếu hồ sơ khách hàng'
        ELSE N'Đầy đủ'
    END                          AS TinhTrangHoSo
FROM TAIKHOAN tk
     LEFT JOIN NHAN_VIEN  nv ON nv.MaTK = tk.MaTK
     LEFT JOIN KHACH_HANG kh ON kh.MaTK = tk.MaTK;
GO

IF OBJECT_ID(N'v_LichDatSan', N'V') IS NOT NULL DROP VIEW v_LichDatSan;
GO

/* Lịch đặt sân đầy đủ thông tin hiển thị. */
CREATE VIEW v_LichDatSan AS
SELECT
    ds.MaDat, ds.NgayDat, ds.GioBatDau, ds.GioKetThuc,
    DATEDIFF(MINUTE, ds.GioBatDau, ds.GioKetThuc) AS SoPhut,
    ds.TienSan, ds.TrangThai, ds.GhiChu, ds.NgayTao,
    s.MaSan, s.TenSan, s.DonGia, ls.TenLoaiSan,
    kh.MaKH, kh.HoTen AS TenKH, kh.SDT,
    tk.HoTen AS NguoiTao, tk.VaiTro AS VaiTroNguoiTao,
    hd.MaHD, hd.TrangThai AS TrangThaiHoaDon,
    dbo.fn_LaCuoiTuan(ds.NgayDat) AS LaCuoiTuan
FROM DAT_SAN ds
     INNER JOIN SAN s        ON s.MaSan = ds.MaSan
     INNER JOIN LOAI_SAN ls  ON ls.MaLoaiSan = s.MaLoaiSan
     INNER JOIN KHACH_HANG kh ON kh.MaKH = ds.MaKH
     LEFT JOIN TAIKHOAN tk  ON tk.MaTK = ds.MaNguoiTao
     LEFT JOIN HOA_DON hd   ON hd.MaDat = ds.MaDat;
GO

IF OBJECT_ID(N'v_HoaDonChiTiet', N'V') IS NOT NULL DROP VIEW v_HoaDonChiTiet;
GO

/* Hóa đơn chi tiết (kèm ai lập, sân nào, khách nào, voucher nào). */
CREATE VIEW v_HoaDonChiTiet AS
SELECT
    hd.MaHD, hd.MaDat, hd.NgayLap, hd.TienGoc, hd.LoaiGiamGia, hd.TienGiam,
    hd.TongTien, hd.PhuongThucThanhToan, hd.TrangThai, hd.GhiChu,
    ds.NgayDat, ds.GioBatDau, ds.GioKetThuc,
    s.TenSan, ls.TenLoaiSan,
    kh.MaKH, kh.HoTen AS TenKH, kh.SDT,
    nv.MaTK AS MaTKLap, nv.HoTen AS NguoiLap, nv.VaiTro AS VaiTroNguoiLap,
    v.MaCode, v.TenVoucher
FROM HOA_DON hd
     INNER JOIN DAT_SAN ds   ON ds.MaDat = hd.MaDat
     INNER JOIN SAN s        ON s.MaSan = ds.MaSan
     INNER JOIN LOAI_SAN ls  ON ls.MaLoaiSan = s.MaLoaiSan
     INNER JOIN KHACH_HANG kh ON kh.MaKH = ds.MaKH
     LEFT JOIN TAIKHOAN nv   ON nv.MaTK = hd.MaNguoiLap
     LEFT JOIN VOUCHER v     ON v.MaVoucher = hd.MaVoucher;
GO

IF OBJECT_ID(N'v_DoanhThuTheoNgay', N'V') IS NOT NULL DROP VIEW v_DoanhThuTheoNgay;
GO

/* Doanh thu theo ngày (chỉ tính hóa đơn chưa hủy). */
CREATE VIEW v_DoanhThuTheoNgay AS
SELECT
    CAST(hd.NgayLap AS DATE)                          AS Ngay,
    COUNT(DISTINCT hd.MaHD)                           AS SoHoaDon,
    SUM(CASE WHEN hd.TrangThai = N'DaThanhToan' THEN 1 ELSE 0 END) AS SoHoaDonDaThu,
    SUM(hd.TienGoc)                                   AS TienGoc,
    SUM(hd.TienGiam)                                  AS TienGiam,
    SUM(CASE WHEN hd.TrangThai <> N'DaHuy' THEN hd.TongTien ELSE 0 END) AS DoanhThu,
    SUM(CASE WHEN hd.TrangThai = N'DaThanhToan'   THEN hd.TongTien ELSE 0 END) AS DaThu,
    SUM(CASE WHEN hd.TrangThai = N'ChuaThanhToan' THEN hd.TongTien ELSE 0 END) AS ConNo
FROM HOA_DON hd
GROUP BY CAST(hd.NgayLap AS DATE);
GO

IF OBJECT_ID(N'v_ThongKeTheoSan', N'V') IS NOT NULL DROP VIEW v_ThongKeTheoSan;
GO

/* Doanh thu theo sân. */
CREATE VIEW v_ThongKeTheoSan AS
SELECT
    s.MaSan, s.TenSan, ls.TenLoaiSan, s.DonGia, s.TrangThai AS TrangThaiSan,
    COUNT(CASE WHEN ds.TrangThai <> N'DaHuy' THEN 1 END)     AS SoLuotDat,
    SUM(CASE WHEN ds.TrangThai = N'HoanThanh'
             THEN DATEDIFF(MINUTE, ds.GioBatDau, ds.GioKetThuc) / 60.0 ELSE 0 END) AS SoGioDaChoi,
    SUM(CASE WHEN ds.TrangThai <> N'DaHuy' THEN ds.TienSan ELSE 0 END) AS TienSan,
    SUM(CASE WHEN hd.TrangThai = N'DaThanhToan' THEN hd.TongTien ELSE 0 END) AS DoanhThuDaThu
FROM SAN s
     INNER JOIN LOAI_SAN ls ON ls.MaLoaiSan = s.MaLoaiSan
     LEFT JOIN DAT_SAN ds   ON ds.MaSan = s.MaSan
     LEFT JOIN HOA_DON hd   ON hd.MaDat = ds.MaDat
GROUP BY s.MaSan, s.TenSan, ls.TenLoaiSan, s.DonGia, s.TrangThai;
GO

IF OBJECT_ID(N'v_KhachHangThanThiet', N'V') IS NOT NULL DROP VIEW v_KhachHangThanThiet;
GO

/* Xếp hạng khách hàng theo tổng chi tiêu. */
CREATE VIEW v_KhachHangThanThiet AS
SELECT
    kh.MaKH, kh.HoTen, kh.SDT, kh.Email, kh.NgayTao,
    COUNT(CASE WHEN ds.TrangThai <> N'DaHuy' THEN 1 END) AS SoLanDat,
    COUNT(CASE WHEN ds.TrangThai = N'HoanThanh' THEN 1 END) AS SoLanHoanThanh,
    COUNT(CASE WHEN ds.TrangThai = N'DaHuy' THEN 1 END) AS SoLanHuy,
    ISNULL(SUM(CASE WHEN hd.TrangThai = N'DaThanhToan' THEN hd.TongTien END), 0) AS TongChiTieu,
    MAX(hd.NgayLap) AS LanGiaoDichCuoi
FROM KHACH_HANG kh
     LEFT JOIN DAT_SAN ds ON ds.MaKH = kh.MaKH
     LEFT JOIN HOA_DON hd ON hd.MaDat = ds.MaDat
GROUP BY kh.MaKH, kh.HoTen, kh.SDT, kh.Email, kh.NgayTao;
GO

IF OBJECT_ID(N'v_VoucherConHan', N'V') IS NOT NULL DROP VIEW v_VoucherConHan;
GO

/* Voucher còn hiệu lực, còn lượt dùng. */
CREATE VIEW v_VoucherConHan AS
SELECT
    v.*,
    (v.SoLuong - v.SoLuongDaDung) AS SoLuotConLai,
    CASE
        WHEN v.TrangThai <> N'HoatDong'                        THEN N'Tạm ngưng'
        WHEN CAST(GETDATE() AS DATE) < v.NgayBatDau            THEN N'Chưa bắt đầu'
        WHEN CAST(GETDATE() AS DATE) > v.NgayKetThuc           THEN N'Hết hạn'
        WHEN v.SoLuongDaDung >= v.SoLuong                      THEN N'Hết lượt'
        ELSE N'Còn dùng được'
    END AS TinhTrang
FROM VOUCHER v;
GO

IF OBJECT_ID(N'v_MaTranQuyen', N'V') IS NOT NULL DROP VIEW v_MaTranQuyen;
GO

/* Ma trận quyền dạng dễ đọc (mỗi vai trò một dòng liệt kê quyền). */
CREATE VIEW v_MaTranQuyen AS
SELECT
    vt.MaVaiTro, vt.TenVaiTro,
    COUNT(vtq.MaQuyen)                                    AS SoQuyen,
    (SELECT COUNT(1) FROM QUYEN)                          AS TongSoQuyen,
    STUFF((SELECT N', ' + q.NhomQuyen + N':' + q.MaQuyen
           FROM VAI_TRO_QUYEN vtq INNER JOIN QUYEN q ON q.MaQuyen = vtq.MaQuyen
           WHERE vtq.MaVaiTro = vt.MaVaiTro
           ORDER BY q.NhomQuyen, q.MaQuyen
           FOR XML PATH(N''), TYPE).value(N'.', N'NVARCHAR(MAX)'), 1, 2, N'') AS DanhSachQuyen
FROM VAI_TRO vt;
GO

IF OBJECT_ID(N'sp_CapNhatTrangThaiBooking', N'P') IS NOT NULL DROP PROCEDURE sp_CapNhatTrangThaiBooking;
GO

/* ===================================================================== */
/* 14. STORED PROCEDURE                                                  */
/* ===================================================================== */

/* Bảo trì trạng thái booking: DaDat -> DangSuDung (đang diễn ra).
   @TuDongHoanThanh = 1 thì đóng luôn các lượt đã qua giờ kết thúc
   (ứng dụng hiện KHÔNG tự đóng, nên mặc định = 0 để hành vi khớp nhau). */
CREATE PROCEDURE sp_CapNhatTrangThaiBooking
    @TuDongHoanThanh BIT = 0,
    @SoBanGhi INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @homNay DATE = CAST(GETDATE() AS DATE);
    DECLARE @gio TIME(0) = CAST(GETDATE() AS TIME(0));

    UPDATE DAT_SAN SET TrangThai = N'DangSuDung'
    WHERE TrangThai = N'DaDat' AND NgayDat = @homNay
      AND @gio >= GioBatDau AND @gio < GioKetThuc;

    IF @TuDongHoanThanh = 1
        UPDATE DAT_SAN SET TrangThai = N'HoanThanh'
        WHERE TrangThai IN (N'DaDat', N'DangSuDung')
          AND (NgayDat < @homNay OR (NgayDat = @homNay AND GioKetThuc <= @gio));

    /* Sân đang thuê nhưng không còn lượt nào diễn ra -> trả về Trống.
       Không đụng sân Bảo trì. */
    UPDATE s SET TrangThai = N'Trong'
    FROM SAN s
    WHERE s.TrangThai = N'DangThue'
      AND NOT EXISTS (SELECT 1 FROM DAT_SAN ds
                      WHERE ds.MaSan = s.MaSan AND ds.TrangThai = N'DangSuDung');

    /* Lượt sắp tới -> sân chuyển sang Đang thuê (để giao diện phản ánh đúng). */
    UPDATE s SET TrangThai = N'DangThue'
    FROM SAN s
    WHERE s.TrangThai = N'Trong'
      AND EXISTS (SELECT 1 FROM DAT_SAN ds
                  WHERE ds.MaSan = s.MaSan AND ds.TrangThai = N'DangSuDung');

    SET @SoBanGhi = @@ROWCOUNT;
END
GO

IF OBJECT_ID(N'sp_KiemTraTrungLich', N'P') IS NOT NULL DROP PROCEDURE sp_KiemTraTrungLich;
GO

/* Kiểm tra trùng lịch trước khi đặt (trả về danh sách booking chồng lấn). */
CREATE PROCEDURE sp_KiemTraTrungLich
    @MaSan INT,
    @NgayDat DATE,
    @GioBatDau TIME(0),
    @GioKetThuc TIME(0),
    @MaDatLoaiTru INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ds.MaDat, ds.NgayDat, ds.GioBatDau, ds.GioKetThuc, ds.TrangThai,
           kh.HoTen AS TenKH, kh.SDT, s.TenSan
    FROM DAT_SAN ds
         INNER JOIN KHACH_HANG kh ON kh.MaKH = ds.MaKH
         INNER JOIN SAN s ON s.MaSan = ds.MaSan
    WHERE ds.MaSan = @MaSan
      AND ds.NgayDat = @NgayDat
      AND ds.TrangThai <> N'DaHuy'
      AND (@MaDatLoaiTru IS NULL OR ds.MaDat <> @MaDatLoaiTru)
      AND ds.GioBatDau < @GioKetThuc
      AND ds.GioKetThuc > @GioBatDau
    ORDER BY ds.GioBatDau;
END
GO

IF OBJECT_ID(N'sp_LayTongQuan', N'P') IS NOT NULL DROP PROCEDURE sp_LayTongQuan;
GO

/* Bảng tổng quan cho dashboard. */
CREATE PROCEDURE sp_LayTongQuan
    @TuNgay DATE = NULL,
    @DenNgay DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET @TuNgay  = ISNULL(@TuNgay, DATEADD(DAY, -29, CAST(GETDATE() AS DATE)));
    SET @DenNgay = ISNULL(@DenNgay, CAST(GETDATE() AS DATE));

    SELECT
        CAST(GETDATE() AS DATE)                                            AS HomNay,
        (SELECT ISNULL(SUM(TongTien), 0) FROM HOA_DON
          WHERE TrangThai = N'DaThanhToan' AND CAST(NgayLap AS DATE) = CAST(GETDATE() AS DATE)) AS DoanhThuHomNay,
        (SELECT COUNT(1) FROM DAT_SAN WHERE NgayDat = CAST(GETDATE() AS DATE) AND TrangThai <> N'DaHuy') AS BookingHomNay,
        (SELECT ISNULL(SUM(TongTien), 0) FROM HOA_DON
          WHERE TrangThai = N'DaThanhToan'
            AND CAST(NgayLap AS DATE) BETWEEN @TuNgay AND @DenNgay)        AS DoanhThuTrongKy,
        (SELECT COUNT(1) FROM DAT_SAN
          WHERE NgayDat BETWEEN @TuNgay AND @DenNgay AND TrangThai <> N'DaHuy') AS BookingTrongKy,
        (SELECT COUNT(1) FROM HOA_DON WHERE TrangThai = N'ChuaThanhToan')   AS HoaDonChuaThanhToan,
        (SELECT COUNT(1) FROM KHACH_HANG)                                   AS TongKhachHang,
        (SELECT COUNT(1) FROM SAN)                                          AS TongSoSan,
        (SELECT COUNT(1) FROM SAN WHERE TrangThai = N'Trong')               AS SanTrong,
        (SELECT COUNT(1) FROM SAN WHERE TrangThai = N'DangThue')            AS SanDangThue,
        (SELECT COUNT(1) FROM SAN WHERE TrangThai = N'BaoTri')              AS SanBaoTri;
END
GO

IF OBJECT_ID(N'sp_DoanhThuTheoNgay', N'P') IS NOT NULL DROP PROCEDURE sp_DoanhThuTheoNgay;
GO

/* Doanh thu theo ngày trong kỳ. */
CREATE PROCEDURE sp_DoanhThuTheoNgay
    @TuNgay DATE,
    @DenNgay DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Ngay, SoHoaDon, SoHoaDonDaThu, TienGoc, TienGiam, DoanhThu, DaThu, ConNo
    FROM v_DoanhThuTheoNgay
    WHERE Ngay BETWEEN @TuNgay AND @DenNgay
    ORDER BY Ngay;
END
GO

IF OBJECT_ID(N'sp_ThongKeTheoSan', N'P') IS NOT NULL DROP PROCEDURE sp_ThongKeTheoSan;
GO

/* Top sân theo doanh thu. */
CREATE PROCEDURE sp_ThongKeTheoSan
    @TuNgay DATE = NULL,
    @DenNgay DATE = NULL,
    @TopN INT = 5
AS
BEGIN
    SET NOCOUNT ON;
    SET @TuNgay  = ISNULL(@TuNgay, DATEADD(DAY, -29, CAST(GETDATE() AS DATE)));
    SET @DenNgay = ISNULL(@DenNgay, CAST(GETDATE() AS DATE));

    SELECT TOP (@TopN)
        s.MaSan, s.TenSan, ls.TenLoaiSan,
        COUNT(CASE WHEN ds.TrangThai <> N'DaHuy' THEN 1 END) AS SoLuotDat,
        ISNULL(SUM(CASE WHEN ds.TrangThai <> N'DaHuy' THEN ds.TienSan END), 0) AS TienSan,
        ISNULL(SUM(CASE WHEN hd.TrangThai = N'DaThanhToan' THEN hd.TongTien END), 0) AS DoanhThu
    FROM SAN s
         INNER JOIN LOAI_SAN ls ON ls.MaLoaiSan = s.MaLoaiSan
         LEFT JOIN DAT_SAN ds ON ds.MaSan = s.MaSan AND ds.NgayDat BETWEEN @TuNgay AND @DenNgay
         LEFT JOIN HOA_DON hd ON hd.MaDat = ds.MaDat
    GROUP BY s.MaSan, s.TenSan, ls.TenLoaiSan
    ORDER BY DoanhThu DESC, TienSan DESC;
END
GO

IF OBJECT_ID(N'sp_GhiNhatKy', N'P') IS NOT NULL DROP PROCEDURE sp_GhiNhatKy;
GO

/* Ghi nhật ký hoạt động (gọi từ tầng nghiệp vụ khi muốn bật audit). */
CREATE PROCEDURE sp_GhiNhatKy
    @MaTK INT,
    @HoatDong NVARCHAR(50),
    @BangDuLieu NVARCHAR(50) = NULL,
    @MaDuLieu NVARCHAR(30) = NULL,
    @NoiDung NVARCHAR(500) = NULL,
    @KetQua NVARCHAR(20) = N'ThanhCong',
    @MayTram NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO NHAT_KY_HOAT_DONG
        (MaTK, TenDangNhap, VaiTro, HoatDong, BangDuLieu, MaDuLieu, NoiDung, KetQua, ThoiGian, MayTram)
    SELECT @MaTK, tk.TenDangNhap, tk.VaiTro, @HoatDong, @BangDuLieu, @MaDuLieu,
           @NoiDung, @KetQua, GETDATE(), @MayTram
    FROM TAIKHOAN tk WHERE tk.MaTK = @MaTK;

    IF @@ROWCOUNT = 0
        INSERT INTO NHAT_KY_HOAT_DONG
            (MaTK, TenDangNhap, VaiTro, HoatDong, BangDuLieu, MaDuLieu, NoiDung, KetQua, ThoiGian, MayTram)
        VALUES (@MaTK, NULL, NULL, @HoatDong, @BangDuLieu, @MaDuLieu,
                @NoiDung, @KetQua, GETDATE(), @MayTram);
END
GO

IF OBJECT_ID(N'sp_LichTrongCuaSan', N'P') IS NOT NULL DROP PROCEDURE sp_LichTrongCuaSan;
GO

/* Xem lịch còn trống của một sân trong ngày (các khoảng chưa bị chiếm). */
CREATE PROCEDURE sp_LichTrongCuaSan
    @MaSan INT,
    @NgayDat DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ds.MaDat, ds.GioBatDau, ds.GioKetThuc, ds.TrangThai,
           kh.HoTen AS TenKH, ds.TienSan
    FROM DAT_SAN ds INNER JOIN KHACH_HANG kh ON kh.MaKH = ds.MaKH
    WHERE ds.MaSan = @MaSan AND ds.NgayDat = @NgayDat AND ds.TrangThai <> N'DaHuy'
    ORDER BY ds.GioBatDau;
END
GO

/* ============ C. SEED MA TRẬN QUYỀN (idempotent) ============ */
/* 15. SEED: VAI TRÒ + QUYỀN + MA TRẬN (khớp PhanQuyenService C#)        */
/* ===================================================================== */
IF NOT EXISTS (SELECT 1 FROM VAI_TRO)
INSERT INTO VAI_TRO (MaVaiTro, TenVaiTro, MoTa, LaMacDinh) VALUES
(N'Admin',     N'Quản trị viên', N'Toàn quyền hệ thống, kể cả phân quyền và cấu hình', 0),
(N'NhanVien',  N'Nhân viên',     N'Vận hành hằng ngày: đặt sân, lập hóa đơn, thu tiền', 1),
(N'KhachHang', N'Khách hàng',    N'Chỉ xem và thao tác trên dữ liệu của chính mình', 0);
GO

IF NOT EXISTS (SELECT 1 FROM QUYEN)
INSERT INTO QUYEN (MaQuyen, TenQuyen, NhomQuyen, MoTa) VALUES
(N'TK_XEM',            N'Xem tài khoản',            N'TaiKhoan',   N'Xem danh sách tài khoản người dùng'),
(N'TK_THEM',           N'Thêm tài khoản',           N'TaiKhoan',   N'Tạo tài khoản mới'),
(N'TK_SUA',            N'Sửa tài khoản',            N'TaiKhoan',   N'Cập nhật thông tin tài khoản'),
(N'TK_XOA',            N'Xóa tài khoản',            N'TaiKhoan',   N'Xóa tài khoản'),
(N'TK_KHOA',           N'Khóa / mở tài khoản',      N'TaiKhoan',   N'Đổi trạng thái HoatDong <-> BiKhoa'),
(N'TK_PHANQUYEN',      N'Phân quyền',               N'TaiKhoan',   N'Thay đổi vai trò / ma trận quyền'),
(N'TK_DOIMATKHAU',     N'Đổi mật khẩu của mình',    N'TaiKhoan',   N'Ai cũng được đổi mật khẩu của chính mình'),
(N'NV_XEM',            N'Xem nhân viên',            N'NhanVien',   N'Xem danh sách nhân viên'),
(N'NV_THEM',           N'Thêm nhân viên',           N'NhanVien',   N'Tạo nhân viên + tài khoản'),
(N'NV_SUA',            N'Sửa nhân viên',            N'NhanVien',   N'Cập nhật hồ sơ nhân viên'),
(N'NV_XOA',            N'Xóa nhân viên',            N'NhanVien',   N'Xóa hồ sơ nhân viên'),
(N'LOAISAN_XEM',       N'Xem loại sân',             N'San',        NULL),
(N'LOAISAN_THEM',      N'Thêm loại sân',            N'San',        NULL),
(N'LOAISAN_SUA',       N'Sửa loại sân',             N'San',        NULL),
(N'LOAISAN_XOA',       N'Xóa loại sân',             N'San',        NULL),
(N'SAN_XEM',           N'Xem sân',                  N'San',        NULL),
(N'SAN_THEM',          N'Thêm sân',                 N'San',        NULL),
(N'SAN_SUA',           N'Sửa sân',                  N'San',        NULL),
(N'SAN_XOA',           N'Xóa sân',                  N'San',        NULL),
(N'SAN_DOITRANGTHAI',  N'Đổi trạng thái sân',       N'San',        N'Trống / Đang thuê / Bảo trì'),
(N'KH_XEM',            N'Xem khách hàng',           N'KhachHang',  N'Xem hồ sơ của chính mình (khách) hoặc danh sách (nhân viên)'),
(N'KH_XEM_TATCA',      N'Xem tất cả khách hàng',    N'KhachHang',  NULL),
(N'KH_THEM',           N'Thêm khách hàng',          N'KhachHang',  NULL),
(N'KH_SUA',            N'Sửa khách hàng',           N'KhachHang',  NULL),
(N'KH_XOA',            N'Xóa khách hàng',           N'KhachHang',  NULL),
(N'DATSAN_XEM_TATCA',  N'Xem tất cả booking',       N'DatSan',     NULL),
(N'DATSAN_XEM_CUATOI', N'Xem booking của tôi',      N'DatSan',     NULL),
(N'DATSAN_THEM',       N'Thêm booking',             N'DatSan',     NULL),
(N'DATSAN_SUA',        N'Sửa booking',              N'DatSan',     NULL),
(N'DATSAN_HUY',        N'Hủy booking',              N'DatSan',     NULL),
(N'LICH_XEM_TATCA',    N'Xem lịch toàn hệ thống',   N'Lich',       NULL),
(N'LICH_XEM_CUATOI',   N'Xem lịch của tôi',         N'Lich',       NULL),
(N'HD_XEM_TATCA',      N'Xem tất cả hóa đơn',       N'HoaDon',     NULL),
(N'HD_XEM_CUATOI',     N'Xem hóa đơn của tôi',      N'HoaDon',     NULL),
(N'HD_LAP',            N'Lập hóa đơn',              N'HoaDon',     NULL),
(N'HD_SUA',            N'Sửa hóa đơn',              N'HoaDon',     NULL),
(N'HD_XOA',            N'Xóa hóa đơn',              N'HoaDon',     NULL),
(N'HD_THANHTOAN',      N'Thanh toán hóa đơn',       N'HoaDon',     NULL),
(N'HD_IN',             N'In / xem trước hóa đơn',   N'HoaDon',     NULL),
(N'VOUCHER_XEM',       N'Xem voucher',              N'Voucher',    NULL),
(N'VOUCHER_THEM',      N'Thêm voucher',             N'Voucher',    NULL),
(N'VOUCHER_SUA',       N'Sửa voucher',              N'Voucher',    NULL),
(N'VOUCHER_XOA',       N'Xóa voucher',              N'Voucher',    NULL),
(N'VOUCHER_APDUNG',    N'Áp dụng voucher (quầy)',   N'Voucher',    NULL),
(N'VOUCHER_SUDUNG',    N'Sử dụng voucher (khách)',  N'Voucher',    NULL),
(N'KM_XEM',            N'Xem khuyến mãi',           N'KhuyenMai',  NULL),
(N'KM_THEM',           N'Thêm khuyến mãi',          N'KhuyenMai',  NULL),
(N'KM_SUA',            N'Sửa khuyến mãi',           N'KhuyenMai',  NULL),
(N'KM_XOA',            N'Xóa khuyến mãi',           N'KhuyenMai',  NULL),
(N'KM_APDUNG',         N'Áp dụng khuyến mãi',       N'KhuyenMai',  NULL),
(N'CAUHINH_XEM',       N'Xem cấu hình hệ thống',    N'CauHinh',    NULL),
(N'CAUHINH_SUA',       N'Sửa cấu hình hệ thống',    N'CauHinh',    NULL),
(N'THONGTKE_TOANBO',   N'Thống kê toàn hệ thống',   N'ThongKe',    N'Giữ nguyên mã (kể cả lỗi gõ) để khớp hằng số C#'),
(N'THONGTKE_NGHIEPVU', N'Thống kê nghiệp vụ',       N'ThongKe',    NULL),
(N'THONGTKE_CANHAN',   N'Thống kê cá nhân',         N'ThongKe',    NULL),
(N'NHATKY_XEM',        N'Xem nhật ký hoạt động',    N'NhatKy',     N'Mới ở bản v2 — muốn dùng trong app thì thêm hằng số vào MaQuyen');
GO

/* Admin: tất cả quyền. */
INSERT INTO VAI_TRO_QUYEN (MaVaiTro, MaQuyen)
SELECT N'Admin', q.MaQuyen FROM QUYEN q
WHERE NOT EXISTS (SELECT 1 FROM VAI_TRO_QUYEN x
                  WHERE x.MaVaiTro = N'Admin' AND x.MaQuyen = q.MaQuyen);

/* Nhân viên: đúng danh sách QuyenNhanVien trong PhanQuyenService.cs */
INSERT INTO VAI_TRO_QUYEN (MaVaiTro, MaQuyen)
SELECT N'NhanVien', q.MaQuyen FROM QUYEN q
WHERE NOT EXISTS (SELECT 1 FROM VAI_TRO_QUYEN x
                  WHERE x.MaVaiTro = N'NhanVien' AND x.MaQuyen = q.MaQuyen)
  AND q.MaQuyen IN (
    N'TK_DOIMATKHAU',
    N'LOAISAN_XEM',
    N'SAN_XEM', N'SAN_DOITRANGTHAI',
    N'KH_XEM', N'KH_XEM_TATCA', N'KH_THEM', N'KH_SUA',
    N'DATSAN_XEM_TATCA', N'DATSAN_THEM', N'DATSAN_SUA', N'DATSAN_HUY',
    N'LICH_XEM_TATCA',
    N'HD_XEM_TATCA', N'HD_LAP', N'HD_SUA', N'HD_THANHTOAN', N'HD_IN',
    N'VOUCHER_XEM', N'VOUCHER_APDUNG',
    N'KM_XEM', N'KM_APDUNG',
    N'THONGTKE_NGHIEPVU');

/* Khách hàng: đúng danh sách QuyenKhachHang. */
INSERT INTO VAI_TRO_QUYEN (MaVaiTro, MaQuyen)
SELECT N'KhachHang', q.MaQuyen FROM QUYEN q
WHERE NOT EXISTS (SELECT 1 FROM VAI_TRO_QUYEN x
                  WHERE x.MaVaiTro = N'KhachHang' AND x.MaQuyen = q.MaQuyen)
  AND q.MaQuyen IN (
    N'TK_DOIMATKHAU',
    N'LOAISAN_XEM',
    N'SAN_XEM',
    N'KH_XEM',
    N'DATSAN_XEM_CUATOI', N'DATSAN_THEM', N'DATSAN_HUY',
    N'LICH_XEM_CUATOI',
    N'HD_XEM_CUATOI',
    N'VOUCHER_XEM', N'VOUCHER_SUDUNG',
    N'KM_XEM',
    N'THONGTKE_CANHAN');
GO

GO
GO

/* ============ D. VÁ LỖI: tài khoản Admin/NhanVien thiếu hồ sơ NHAN_VIEN ============
   Lỗi của dữ liệu mẫu cũ: tài khoản 'admin' không có dòng NHAN_VIEN, khiến admin
   "vô hình" trong màn hình Quản lý nhân viên và PhienLamViec.MaNV = NULL.
   SĐT sinh tự động theo MaTK để không đụng ràng buộc UNIQUE. */
INSERT INTO NHAN_VIEN (MaTK, HoTen, SDT, Email, DiaChi, ChucVu, NgayVaoLam, TrangThai)
SELECT tk.MaTK,
       tk.HoTen,
       N'09' + RIGHT(N'00000000' + CAST(tk.MaTK AS NVARCHAR(10)), 8),
       NULL, NULL,
       CASE WHEN tk.VaiTro = N'Admin' THEN N'Quản trị viên' ELSE N'Nhân viên' END,
       CAST(tk.NgayTao AS DATE),
       N'HoatDong'
FROM TAIKHOAN tk
WHERE tk.VaiTro IN (N'Admin', N'NhanVien')
  AND NOT EXISTS (SELECT 1 FROM NHAN_VIEN nv WHERE nv.MaTK = tk.MaTK)
  /* nếu SĐT tự sinh tình cờ trùng một hồ sơ đã có thì bỏ qua, tránh lỗi UNIQUE;
     tài khoản đó sẽ hiện ra ở truy vấn kiểm tra bên dưới với cột TinhTrangHoSo
     = 'Thiếu hồ sơ nhân viên' để bạn tự bổ sung tay. */
  AND NOT EXISTS (SELECT 1 FROM NHAN_VIEN nv2
                  WHERE nv2.SDT = N'09' + RIGHT(N'00000000' + CAST(tk.MaTK AS NVARCHAR(10)), 8));

PRINT N'== Đã bổ sung hồ sơ nhân viên cho ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + N' tài khoản ==';
GO

/* ====================== E. KIỂM TRA ====================== */
SELECT MaTK, TenDangNhap, HoTen, VaiTro, TrangThaiTaiKhoan, MaNV, ChucVu, TinhTrangHoSo
FROM v_NguoiDung
ORDER BY CASE VaiTro WHEN N'Admin' THEN 1 WHEN N'NhanVien' THEN 2 ELSE 3 END, TenDangNhap;
GO

SELECT vt.MaVaiTro, vt.TenVaiTro, COUNT(vtq.MaQuyen) AS SoQuyen
FROM VAI_TRO vt LEFT JOIN VAI_TRO_QUYEN vtq ON vtq.MaVaiTro = vt.MaVaiTro
GROUP BY vt.MaVaiTro, vt.TenVaiTro;
GO

PRINT N'== HOÀN TẤT nâng cấp v2 (dữ liệu cũ được giữ nguyên) ==';
GO
