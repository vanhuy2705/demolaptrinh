/*
  Quản lý cho thuê sân thể thao — TẬP LỆNH TẠO CSDL BẢN ĐẦY ĐỦ (v2)
  ===================================================================
  SQL Server 2016+ | Windows Authentication | Database: QLSanTheThao

  Chạy lần lượt:  01_TaoCSDL_v2.sql  ->  02_DuLieuMau_v2.sql
  (Hai file 01/02 cũ vẫn còn trong repo nếu muốn dùng bản tối giản.)

  ⚠ Tập lệnh này DROP DATABASE QLSanTheThao nếu đã tồn tại. Sao lưu trước khi chạy.

  ---------------------------------------------------------------------
  TƯƠNG THÍCH NGƯỢC VỚI ỨNG DỤNG C# HIỆN TẠI
  ---------------------------------------------------------------------
  • Giữ NGUYÊN tên bảng, tên cột, kiểu dữ liệu, giá trị CHECK của 11 bảng gốc
    => ứng dụng WinForms chạy được ngay, không phải sửa một dòng code nào.
  • Mọi thứ "đầy đủ hơn" đều là BỔ SUNG (additive):
      + 4 bảng mới : VAI_TRO, QUYEN, VAI_TRO_QUYEN, NHAT_KY_HOAT_DONG
      + 8 view      : tra cứu nhanh, báo cáo, đối soát
      + 5 function  : tính tiền theo block, đọc tham số, kiểm tra cuối tuần
      + 7 procedure : bảo trì trạng thái, kiểm tra trùng lịch, báo cáo, ghi nhật ký
      + index bổ sung cho các truy vấn nặng (lịch theo ngày, hóa đơn theo trạng thái…)
      + ràng buộc CHECK chặt hơn ở những chỗ ứng dụng vốn tự kiểm tra bằng code

  ---------------------------------------------------------------------
  VÌ SAO ADMIN KHÔNG CÓ BẢNG RIÊNG?
  ---------------------------------------------------------------------
  Mô hình phân quyền là RBAC 1 bảng định danh + cột vai trò:
      TAIKHOAN(VaiTro IN ('Admin','NhanVien','KhachHang'))
          ├── NHAN_VIEN  (MaTK -> TAIKHOAN)   : hồ sơ nhân sự của Admin VÀ Nhân viên
          └── KHACH_HANG (MaTK -> TAIKHOAN)   : hồ sơ khách hàng
  Admin = "nhân viên có toàn quyền", không phải một loại thực thể khác, nên:
      • Không nhân đôi cột mật khẩu / trạng thái khóa.
      • DAT_SAN.MaNguoiTao và HOA_DON.MaNguoiLap trỏ về TAIKHOAN
        => truy vết được cả Admin lẫn Nhân viên (nếu trỏ về NHAN_VIEN sẽ khó hơn).
      • Bản v2 sửa luôn điểm lệch của dữ liệu mẫu cũ: tài khoản admin
        nay CÓ hồ sơ NHAN_VIEN (xem 02_DuLieuMau_v2.sql), và có view v_NguoiDung
        để nhìn toàn bộ người dùng ở một nơi.
  Quyền hạn vẫn do PhanQuyenService (C#) quyết định; ba bảng VAI_TRO/QUYEN/
  VAI_TRO_QUYEN được seed sẵn đúng ma trận đó để sau này có thể chuyển sang
  phân quyền động trong CSDL mà không phải đổi schema.
*/

IF DB_ID(N'QLSanTheThao') IS NOT NULL
BEGIN
    ALTER DATABASE QLSanTheThao SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QLSanTheThao;
END
GO

CREATE DATABASE QLSanTheThao COLLATE Vietnamese_CI_AS;
GO

USE QLSanTheThao;
GO

SET NOCOUNT ON;
GO

/* ===================================================================== */
/* 1. BẢNG TÀI KHOẢN — định danh duy nhất cho MỌI loại người dùng        */
/* ===================================================================== */
CREATE TABLE TAIKHOAN (
    MaTK        INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap NVARCHAR(50)  NOT NULL UNIQUE,
    MatKhau     NVARCHAR(255) NOT NULL,          -- PBKDF2: iterations.salt.hash
    HoTen       NVARCHAR(100) NOT NULL,
    VaiTro      NVARCHAR(20)  NOT NULL
                CHECK (VaiTro IN (N'Admin', N'NhanVien', N'KhachHang')),
    TrangThai   NVARCHAR(20)  NOT NULL
                CONSTRAINT DF_TAIKHOAN_TrangThai DEFAULT N'HoatDong'
                CHECK (TrangThai IN (N'HoatDong', N'BiKhoa')),
    NgayTao     DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT CK_TAIKHOAN_TenDangNhap CHECK (LEN(LTRIM(RTRIM(TenDangNhap))) >= 4),
    CONSTRAINT CK_TAIKHOAN_HoTen       CHECK (LEN(LTRIM(RTRIM(HoTen))) >= 2)
);
GO

CREATE INDEX IX_TAIKHOAN_VaiTro ON TAIKHOAN(VaiTro, TrangThai);
GO

/* ===================================================================== */
/* 2. BẢNG NHÂN VIÊN — hồ sơ nhân sự của Admin VÀ Nhân viên              */
/* ===================================================================== */
CREATE TABLE NHAN_VIEN (
    MaNV        INT IDENTITY(1,1) PRIMARY KEY,
    MaTK        INT           NULL UNIQUE,       -- 1 tài khoản <-> tối đa 1 hồ sơ nhân viên
    HoTen       NVARCHAR(100) NOT NULL,
    SDT         NVARCHAR(15)  NOT NULL UNIQUE,
    Email       NVARCHAR(100) NULL,
    DiaChi      NVARCHAR(200) NULL,
    ChucVu      NVARCHAR(50)  NOT NULL DEFAULT N'Nhân viên',
    NgayVaoLam  DATE          NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    TrangThai   NVARCHAR(20)  NOT NULL DEFAULT N'HoatDong'
                CHECK (TrangThai IN (N'HoatDong', N'DaNghi')),
    CONSTRAINT FK_NHANVIEN_TAIKHOAN FOREIGN KEY (MaTK)
        REFERENCES TAIKHOAN(MaTK) ON DELETE SET NULL
);
GO

/* Ứng dụng có thể lưu Email = '' (khách/nhân viên không khai email) nên chỉ ràng buộc
   duy nhất với email thực sự có nội dung — nếu không sẽ không tạo được hồ sơ thứ hai. */
CREATE UNIQUE INDEX UX_NHANVIEN_Email ON NHAN_VIEN(Email)
    WHERE Email IS NOT NULL AND LTRIM(RTRIM(Email)) <> N'';
CREATE INDEX IX_NHANVIEN_TrangThai ON NHAN_VIEN(TrangThai);
GO

/* ===================================================================== */
/* 3. LOẠI SÂN & SÂN                                                     */
/* ===================================================================== */
CREATE TABLE LOAI_SAN (
    MaLoaiSan   INT IDENTITY(1,1) PRIMARY KEY,
    TenLoaiSan  NVARCHAR(50) NOT NULL UNIQUE,
    MoTa        NVARCHAR(255) NULL
);
GO

CREATE TABLE SAN (
    MaSan       INT IDENTITY(1,1) PRIMARY KEY,
    TenSan      NVARCHAR(100) NOT NULL UNIQUE,
    MaLoaiSan   INT           NOT NULL,
    DonGia      DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0),   -- đồng/giờ, giá cố định
    TrangThai   NVARCHAR(20)  NOT NULL DEFAULT N'Trong'
                CHECK (TrangThai IN (N'Trong', N'DangThue', N'BaoTri')),
    MoTa        NVARCHAR(255) NULL,
    CONSTRAINT FK_SAN_LOAISAN FOREIGN KEY (MaLoaiSan)
        REFERENCES LOAI_SAN(MaLoaiSan) ON DELETE CASCADE
);
GO

CREATE INDEX IX_SAN_Loai ON SAN(MaLoaiSan);
CREATE INDEX IX_SAN_TrangThai ON SAN(TrangThai) WHERE TrangThai <> N'BaoTri';
GO

/* ===================================================================== */
/* 4. KHÁCH HÀNG                                                         */
/* ===================================================================== */
CREATE TABLE KHACH_HANG (
    MaKH        INT IDENTITY(1,1) PRIMARY KEY,
    HoTen       NVARCHAR(100) NOT NULL,
    SDT         NVARCHAR(15)  NOT NULL UNIQUE,
    Email       NVARCHAR(100) NULL,
    DiaChi      NVARCHAR(200) NULL,
    MaTK        INT           NULL UNIQUE,        -- NULL: khách vãng lai (đặt qua quầy)
    NgayTao     DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_KHACHHANG_TAIKHOAN FOREIGN KEY (MaTK)
        REFERENCES TAIKHOAN(MaTK) ON DELETE SET NULL
);
GO

CREATE INDEX IX_KHACHHANG_HoTen ON KHACH_HANG(HoTen);
CREATE UNIQUE INDEX UX_KHACHHANG_Email ON KHACH_HANG(Email)
    WHERE Email IS NOT NULL AND LTRIM(RTRIM(Email)) <> N'';
GO

/* ===================================================================== */
/* 5. ĐẶT SÂN (booking)                                                  */
/* ===================================================================== */
CREATE TABLE DAT_SAN (
    MaDat       INT IDENTITY(1,1) PRIMARY KEY,
    MaKH        INT           NOT NULL,
    MaSan       INT           NOT NULL,
    NgayDat     DATE          NOT NULL,
    GioBatDau   TIME(0)       NOT NULL,
    GioKetThuc  TIME(0)       NOT NULL,
    TienSan     DECIMAL(18,2) NOT NULL DEFAULT 0,
    TrangThai   NVARCHAR(20)  NOT NULL DEFAULT N'DaDat'
                CHECK (TrangThai IN (N'DaDat', N'DangSuDung', N'HoanThanh', N'DaHuy')),
    GhiChu      NVARCHAR(255) NULL,
    NgayTao     DATETIME      NOT NULL DEFAULT GETDATE(),
    MaNguoiTao  INT           NULL,               -- FK -> TAIKHOAN: Admin hoặc Nhân viên
    MaVoucher   INT           NULL,               -- voucher khách chọn lúc đặt; FK khai báo ở mục 6
                                                  -- (bảng VOUCHER được tạo sau DAT_SAN)
    CONSTRAINT CK_DAT_SAN_Gio      CHECK (GioKetThuc > GioBatDau),
    CONSTRAINT CK_DAT_SAN_Tien     CHECK (TienSan >= 0),
    CONSTRAINT CK_DAT_SAN_Ngay     CHECK (NgayDat >= '2020-01-01'),
    CONSTRAINT FK_DAT_SAN_KHACHHANG FOREIGN KEY (MaKH) REFERENCES KHACH_HANG(MaKH),
    CONSTRAINT FK_DAT_SAN_SAN       FOREIGN KEY (MaSan) REFERENCES SAN(MaSan),
    CONSTRAINT FK_DAT_SAN_NGUOITAO  FOREIGN KEY (MaNguoiTao) REFERENCES TAIKHOAN(MaTK)
);
GO

CREATE INDEX IX_DAT_SAN_Ngay      ON DAT_SAN(NgayDat, MaSan);
CREATE INDEX IX_DAT_SAN_KH        ON DAT_SAN(MaKH);
CREATE INDEX IX_DAT_SAN_TrangThai ON DAT_SAN(TrangThai, NgayDat);
CREATE INDEX IX_DAT_SAN_NguoiTao  ON DAT_SAN(MaNguoiTao);
CREATE INDEX IX_DAT_SAN_Voucher   ON DAT_SAN(MaVoucher);
GO

/* ===================================================================== */
/* 6. VOUCHER & LỊCH SỬ SỬ DỤNG                                          */
/* ===================================================================== */
CREATE TABLE VOUCHER (
    MaVoucher     INT IDENTITY(1,1) PRIMARY KEY,
    MaCode        NVARCHAR(30)  NOT NULL UNIQUE,
    TenVoucher    NVARCHAR(100) NOT NULL,
    LoaiGiam      NVARCHAR(20)  NOT NULL DEFAULT N'PhanTram'
                  CHECK (LoaiGiam IN (N'PhanTram', N'SoTien')),
    GiaTriGiam    DECIMAL(18,2) NOT NULL DEFAULT 0,
    DonToiThieu   DECIMAL(18,2) NOT NULL DEFAULT 0,
    SoLuong       INT           NOT NULL DEFAULT 0 CHECK (SoLuong >= 0),
    SoLuongDaDung INT           NOT NULL DEFAULT 0 CHECK (SoLuongDaDung >= 0),
    NgayBatDau    DATE          NOT NULL,
    NgayKetThuc   DATE          NOT NULL,
    TrangThai     NVARCHAR(20)  NOT NULL DEFAULT N'HoatDong'
                  CHECK (TrangThai IN (N'HoatDong', N'TamNgung')),
    MoTa          NVARCHAR(255) NULL,
    CONSTRAINT CK_VOUCHER_Ngay      CHECK (NgayKetThuc >= NgayBatDau),
    CONSTRAINT CK_VOUCHER_SoLuong   CHECK (SoLuongDaDung <= SoLuong),
    CONSTRAINT CK_VOUCHER_GiaTri    CHECK (GiaTriGiam >= 0 AND DonToiThieu >= 0),
    CONSTRAINT CK_VOUCHER_PhanTram  CHECK (LoaiGiam <> N'PhanTram' OR GiaTriGiam BETWEEN 0 AND 100)
);
GO

CREATE INDEX IX_VOUCHER_HanDung ON VOUCHER(TrangThai, NgayBatDau, NgayKetThuc);
GO

-- FK cho cột DAT_SAN.MaVoucher (đặt ở đây vì DAT_SAN được tạo trước VOUCHER).
-- NULL = booking không dùng voucher; voucher chỉ thật sự bị trừ lượt khi THANH TOÁN
-- (xem bảng SU_DUNG_VOUCHER), nên hủy booking không cần hoàn lượt.
ALTER TABLE DAT_SAN
    ADD CONSTRAINT FK_DAT_SAN_VOUCHER FOREIGN KEY (MaVoucher) REFERENCES VOUCHER(MaVoucher);
GO

CREATE TABLE SU_DUNG_VOUCHER (
    MaSuDung    INT IDENTITY(1,1) PRIMARY KEY,
    MaVoucher   INT           NOT NULL,
    MaDat       INT           NOT NULL,
    MaKH        INT           NOT NULL,
    SoTienGiam  DECIMAL(18,2) NOT NULL DEFAULT 0,
    NgaySuDung  DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT CK_SUDUNG_SoTien CHECK (SoTienGiam >= 0),
    CONSTRAINT FK_SUDUNG_VOUCHER   FOREIGN KEY (MaVoucher) REFERENCES VOUCHER(MaVoucher),
    CONSTRAINT FK_SUDUNG_DAT_SAN   FOREIGN KEY (MaDat)     REFERENCES DAT_SAN(MaDat),
    CONSTRAINT FK_SUDUNG_KHACHHANG FOREIGN KEY (MaKH)      REFERENCES KHACH_HANG(MaKH)
);
GO

CREATE UNIQUE INDEX UX_SUDUNG_VOUCHER_MaDat ON SU_DUNG_VOUCHER(MaDat);  -- 1 booking chỉ 1 voucher
CREATE INDEX IX_SUDUNG_VOUCHER_Voucher ON SU_DUNG_VOUCHER(MaVoucher);
CREATE INDEX IX_SUDUNG_VOUCHER_KH ON SU_DUNG_VOUCHER(MaKH);
GO

/* ===================================================================== */
/* 7. HÓA ĐƠN                                                            */
/* ===================================================================== */
CREATE TABLE HOA_DON (
    MaHD                 INT IDENTITY(1,1) PRIMARY KEY,
    MaDat                INT           NOT NULL,
    NgayLap              DATETIME      NOT NULL DEFAULT GETDATE(),
    TienGoc              DECIMAL(18,2) NOT NULL DEFAULT 0,
    LoaiGiamGia          NVARCHAR(20)  NOT NULL DEFAULT N'Khong'
                         CHECK (LoaiGiamGia IN (N'Khong', N'Voucher', N'KhuyenMai', N'CuoiTuan')),
    TienGiam             DECIMAL(18,2) NOT NULL DEFAULT 0,
    TongTien             DECIMAL(18,2) NOT NULL DEFAULT 0,
    PhuongThucThanhToan  NVARCHAR(20)  NOT NULL DEFAULT N'TienMat'
                         CHECK (PhuongThucThanhToan IN (N'TienMat', N'ChuyenKhoan', N'The')),
    TrangThai            NVARCHAR(20)  NOT NULL DEFAULT N'ChuaThanhToan'
                         CHECK (TrangThai IN (N'ChuaThanhToan', N'DaThanhToan', N'DaHuy')),
    MaNguoiLap           INT           NULL,     -- FK -> TAIKHOAN: Admin hoặc Nhân viên
    MaVoucher            INT           NULL,
    GhiChu               NVARCHAR(255) NULL,
    CONSTRAINT CK_HOADON_Tien CHECK (TienGoc >= 0 AND TienGiam >= 0 AND TongTien >= 0),
    CONSTRAINT FK_HOADON_DAT_SAN  FOREIGN KEY (MaDat)     REFERENCES DAT_SAN(MaDat),
    CONSTRAINT FK_HOADON_NGUOILAP FOREIGN KEY (MaNguoiLap) REFERENCES TAIKHOAN(MaTK),
    CONSTRAINT FK_HOADON_VOUCHER  FOREIGN KEY (MaVoucher)  REFERENCES VOUCHER(MaVoucher)
);
GO

CREATE INDEX IX_HOADON_Ngay      ON HOA_DON(NgayLap);
CREATE INDEX IX_HOADON_TrangThai ON HOA_DON(TrangThai, NgayLap);
CREATE INDEX IX_HOADON_NguoiLap  ON HOA_DON(MaNguoiLap);
CREATE UNIQUE INDEX UX_HOADON_MaDat ON HOA_DON(MaDat);   -- 1 booking chỉ 1 hóa đơn
GO

/* ===================================================================== */
/* 8. KHUYẾN MÃI                                                         */
/* ===================================================================== */
CREATE TABLE KHUYEN_MAI (
    MaKM           INT IDENTITY(1,1) PRIMARY KEY,
    TenKM          NVARCHAR(100) NOT NULL UNIQUE,
    LoaiKhuyenMai  NVARCHAR(50)  NULL,
    PhanTramGiam   DECIMAL(5,2)  NOT NULL DEFAULT 0 CHECK (PhanTramGiam BETWEEN 0 AND 100),
    NgayBatDau     DATE          NOT NULL,
    NgayKetThuc    DATE          NOT NULL,
    ApDungCuoiTuan BIT           NOT NULL DEFAULT 0,
    TrangThai      NVARCHAR(20)  NOT NULL DEFAULT N'HoatDong'
                   CHECK (TrangThai IN (N'HoatDong', N'TamNgung')),
    MoTa           NVARCHAR(255) NULL,
    CONSTRAINT CK_KHUYENMAI_Ngay CHECK (NgayKetThuc >= NgayBatDau)
);
GO

CREATE INDEX IX_KHUYENMAI_HanDung ON KHUYEN_MAI(TrangThai, NgayBatDau, NgayKetThuc);
GO

/* ===================================================================== */
/* 9. THAM SỐ HỆ THỐNG                                                   */
/* ===================================================================== */
CREATE TABLE THAM_SO (
    TenThamSo NVARCHAR(50)  PRIMARY KEY,
    GiaTri    NVARCHAR(200) NULL,
    MoTa      NVARCHAR(255) NULL
);
GO

/* ===================================================================== */
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
GO

CREATE TABLE QUYEN (
    MaQuyen   NVARCHAR(40)  PRIMARY KEY,          -- khớp hằng số MaQuyen trong HangSo.cs
    TenQuyen  NVARCHAR(100) NOT NULL,
    NhomQuyen NVARCHAR(50)  NOT NULL,             -- TaiKhoan, NhanVien, San, DatSan, HoaDon…
    MoTa      NVARCHAR(255) NULL
);
GO

CREATE TABLE VAI_TRO_QUYEN (
    MaVaiTro NVARCHAR(20) NOT NULL,
    MaQuyen  NVARCHAR(40) NOT NULL,
    CONSTRAINT PK_VAI_TRO_QUYEN PRIMARY KEY (MaVaiTro, MaQuyen),
    CONSTRAINT FK_VTQ_VAITRO FOREIGN KEY (MaVaiTro) REFERENCES VAI_TRO(MaVaiTro) ON DELETE CASCADE,
    CONSTRAINT FK_VTQ_QUYEN  FOREIGN KEY (MaQuyen)  REFERENCES QUYEN(MaQuyen)    ON DELETE CASCADE
);
GO

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
GO

CREATE INDEX IX_NHATKY_ThoiGian ON NHAT_KY_HOAT_DONG(ThoiGian DESC);
CREATE INDEX IX_NHATKY_TaiKhoan ON NHAT_KY_HOAT_DONG(MaTK, ThoiGian DESC);
CREATE INDEX IX_NHATKY_HoatDong ON NHAT_KY_HOAT_DONG(HoatDong, ThoiGian DESC);
GO

PRINT N'== Đã tạo 15 bảng (11 bảng nghiệp vụ + 4 bảng phân quyền/nhật ký) ==';
GO

/* ===================================================================== */
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

PRINT N'== Đã tạo 5 function + 8 view ==';
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

PRINT N'== Đã tạo 7 stored procedure ==';
GO

/* ===================================================================== */
/* 15. SEED: VAI TRÒ + QUYỀN + MA TRẬN (khớp PhanQuyenService C#)        */
/* ===================================================================== */
INSERT INTO VAI_TRO (MaVaiTro, TenVaiTro, MoTa, LaMacDinh) VALUES
(N'Admin',     N'Quản trị viên', N'Toàn quyền hệ thống, kể cả phân quyền và cấu hình', 0),
(N'NhanVien',  N'Nhân viên',     N'Vận hành hằng ngày: đặt sân, lập hóa đơn, thu tiền', 1),
(N'KhachHang', N'Khách hàng',    N'Chỉ xem và thao tác trên dữ liệu của chính mình', 0);
GO

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
SELECT N'Admin', MaQuyen FROM QUYEN;

/* Nhân viên: đúng danh sách QuyenNhanVien trong PhanQuyenService.cs */
INSERT INTO VAI_TRO_QUYEN (MaVaiTro, MaQuyen)
SELECT N'NhanVien', MaQuyen FROM QUYEN WHERE MaQuyen IN (
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
SELECT N'KhachHang', MaQuyen FROM QUYEN WHERE MaQuyen IN (
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

PRINT N'== Đã seed 3 vai trò, ' + CAST((SELECT COUNT(1) FROM QUYEN) AS NVARCHAR(10))
    + N' quyền, ' + CAST((SELECT COUNT(1) FROM VAI_TRO_QUYEN) AS NVARCHAR(10)) + N' dòng ma trận ==';
GO

/* ===================================================================== */
/* 16. KIỂM TRA SAU KHI TẠO                                              */
/* ===================================================================== */
SELECT
    (SELECT COUNT(1) FROM sys.tables WHERE type = 'U') AS SoBang,
    (SELECT COUNT(1) FROM sys.views  WHERE name LIKE 'v[_]%') AS SoView,
    (SELECT COUNT(1) FROM sys.procedures WHERE name LIKE 'sp[_]%') AS SoProcedure,
    (SELECT COUNT(1) FROM sys.objects WHERE type = 'FN' AND name LIKE 'fn[_]%') AS SoFunction,
    (SELECT COUNT(1) FROM QUYEN) AS SoQuyen,
    (SELECT COUNT(1) FROM VAI_TRO_QUYEN) AS SoDongMaTran;
GO

PRINT N'== HOÀN TẤT 01_TaoCSDL_v2.sql — chạy tiếp 02_DuLieuMau_v2.sql ==';
GO
