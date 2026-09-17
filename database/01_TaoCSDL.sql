/*
  Quản lý cho thuê sân thể thao - Tập lệnh tạo CSDL (SQL Server 2016+)
  ---------------------------------------------------------------------
  Chạy lần lượt: 01_TaoCSDL.sql  ->  02_DuLieuMau.sql
  Mặc định: Windows Authentication, database QLSanTheThao.
  Nếu dùng SQL Authentication, sửa chuỗi kết nối trong
  src/SportFieldBooking.WinForms/appsettings.json.

  Chú thích thiết kế:
   - 11 bảng: TAIKHOAN, NHAN_VIEN, LOAI_SAN, SAN, KHACH_HANG, DAT_SAN,
              HOA_DON, VOUCHER, SU_DUNG_VOUCHER, KHUYEN_MAI, THAM_SO.
   - Đặc tả gốc chỉ liệt kê 10 bảng (thiếu NHAN_VIEN) nhưng chức năng
     "Quản lý nhân viên" bắt buộc phải có bảng này -> đã bổ sung.
   - KHACH_HANG.MaTK cho phép khách đăng nhập vào cổng khách hàng.
   - Mọi giá thời gian trong ngày (GioBatDau/GioKetThuc) lưu kiểu TIME.
*/

IF DB_ID(N'QLSanTheThao') IS NOT NULL
BEGIN
    ALTER DATABASE QLSanTheThao SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QLSanTheThao;
END
GO

CREATE DATABASE QLSanTheThao
    COLLATE Vietnamese_CI_AS;
GO

USE QLSanTheThao;
GO

/* ============================ BẢNG TÀI KHOẢN ============================ */
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
    NgayTao     DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

/* ============================ BẢNG NHÂN VIÊN ============================ */
CREATE TABLE NHAN_VIEN (
    MaNV        INT IDENTITY(1,1) PRIMARY KEY,
    MaTK        INT           NULL,
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

/* ============================ LOẠI SÂN & SÂN ============================ */
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
    DonGia      DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0),   -- đồng/giờ, cố định
    TrangThai   NVARCHAR(20)  NOT NULL DEFAULT N'Trong'
                CHECK (TrangThai IN (N'Trong', N'DangThue', N'BaoTri')),
    MoTa        NVARCHAR(255) NULL,
    CONSTRAINT FK_SAN_LOAISAN FOREIGN KEY (MaLoaiSan)
        REFERENCES LOAI_SAN(MaLoaiSan) ON DELETE CASCADE
);
GO

CREATE INDEX IX_SAN_Loai ON SAN(MaLoaiSan);
GO

/* ============================ KHÁCH HÀNG ================================ */
CREATE TABLE KHACH_HANG (
    MaKH        INT IDENTITY(1,1) PRIMARY KEY,
    HoTen       NVARCHAR(100) NOT NULL,
    SDT         NVARCHAR(15)  NOT NULL UNIQUE,
    Email       NVARCHAR(100) NULL,
    DiaChi      NVARCHAR(200) NULL,
    MaTK        INT           NULL,                 -- null: khách vãng lai
    NgayTao     DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_KHACHHANG_TAIKHOAN FOREIGN KEY (MaTK)
        REFERENCES TAIKHOAN(MaTK) ON DELETE SET NULL
);
GO

/* ============================ ĐẶT SÂN ================================== */
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
    MaNguoiTao  INT           NULL,
    CONSTRAINT CK_DAT_SAN_Gio CHECK (GioKetThuc > GioBatDau),
    CONSTRAINT FK_DAT_SAN_KHACHHANG FOREIGN KEY (MaKH) REFERENCES KHACH_HANG(MaKH),
    CONSTRAINT FK_DAT_SAN_SAN FOREIGN KEY (MaSan) REFERENCES SAN(MaSan),
    CONSTRAINT FK_DAT_SAN_NGUOITAO FOREIGN KEY (MaNguoiTao) REFERENCES TAIKHOAN(MaTK)
);
GO

CREATE INDEX IX_DAT_SAN_Ngay ON DAT_SAN(NgayDat, MaSan);
CREATE INDEX IX_DAT_SAN_KH ON DAT_SAN(MaKH);
GO

/* ============================ VOUCHER ================================== */
CREATE TABLE VOUCHER (
    MaVoucher    INT IDENTITY(1,1) PRIMARY KEY,
    MaCode       NVARCHAR(30)  NOT NULL UNIQUE,
    TenVoucher   NVARCHAR(100) NOT NULL,
    LoaiGiam     NVARCHAR(20)  NOT NULL DEFAULT N'PhanTram'
                 CHECK (LoaiGiam IN (N'PhanTram', N'SoTien')),
    GiaTriGiam   DECIMAL(18,2) NOT NULL DEFAULT 0,
    DonToiThieu  DECIMAL(18,2) NOT NULL DEFAULT 0,
    SoLuong      INT           NOT NULL DEFAULT 0 CHECK (SoLuong >= 0),
    SoLuongDaDung INT          NOT NULL DEFAULT 0 CHECK (SoLuongDaDung >= 0),
    NgayBatDau   DATE          NOT NULL,
    NgayKetThuc  DATE          NOT NULL,
    TrangThai    NVARCHAR(20)  NOT NULL DEFAULT N'HoatDong'
                 CHECK (TrangThai IN (N'HoatDong', N'TamNgung')),
    MoTa         NVARCHAR(255) NULL,
    CONSTRAINT CK_VOUCHER_Ngay CHECK (NgayKetThuc >= NgayBatDau),
    CONSTRAINT CK_VOUCHER_SoLuong CHECK (SoLuongDaDung <= SoLuong)
);
GO

CREATE TABLE SU_DUNG_VOUCHER (
    MaSuDung    INT IDENTITY(1,1) PRIMARY KEY,
    MaVoucher   INT           NOT NULL,
    MaDat       INT           NOT NULL,
    MaKH        INT           NOT NULL,
    SoTienGiam  DECIMAL(18,2) NOT NULL DEFAULT 0,
    NgaySuDung  DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_SUDUNG_VOUCHER FOREIGN KEY (MaVoucher) REFERENCES VOUCHER(MaVoucher),
    CONSTRAINT FK_SUDUNG_DAT_SAN FOREIGN KEY (MaDat) REFERENCES DAT_SAN(MaDat),
    CONSTRAINT FK_SUDUNG_KHACHHANG FOREIGN KEY (MaKH) REFERENCES KHACH_HANG(MaKH)
);
GO

CREATE UNIQUE INDEX UX_SUDUNG_VOUCHER_MaDat ON SU_DUNG_VOUCHER(MaDat);
GO

/* ============================ HÓA ĐƠN ================================== */
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
    MaNguoiLap           INT           NULL,
    MaVoucher            INT           NULL,
    GhiChu               NVARCHAR(255) NULL,
    CONSTRAINT FK_HOADON_DAT_SAN FOREIGN KEY (MaDat) REFERENCES DAT_SAN(MaDat),
    CONSTRAINT FK_HOADON_NGUOILAP FOREIGN KEY (MaNguoiLap) REFERENCES TAIKHOAN(MaTK),
    CONSTRAINT FK_HOADON_VOUCHER FOREIGN KEY (MaVoucher) REFERENCES VOUCHER(MaVoucher)
);
GO

CREATE INDEX IX_HOADON_Ngay ON HOA_DON(NgayLap);
CREATE UNIQUE INDEX UX_HOADON_MaDat ON HOA_DON(MaDat);   -- 1 booking chỉ 1 hóa đơn
GO

/* ============================ KHUYẾN MÃI =============================== */
CREATE TABLE KHUYEN_MAI (
    MaKM          INT IDENTITY(1,1) PRIMARY KEY,
    TenKM         NVARCHAR(100) NOT NULL UNIQUE,
    LoaiKhuyenMai NVARCHAR(50)  NULL,
    PhanTramGiam  DECIMAL(5,2)  NOT NULL DEFAULT 0 CHECK (PhanTramGiam BETWEEN 0 AND 100),
    NgayBatDau    DATE          NOT NULL,
    NgayKetThuc   DATE          NOT NULL,
    ApDungCuoiTuan BIT          NOT NULL DEFAULT 0,
    TrangThai     NVARCHAR(20)  NOT NULL DEFAULT N'HoatDong'
                  CHECK (TrangThai IN (N'HoatDong', N'TamNgung')),
    MoTa          NVARCHAR(255) NULL,
    CONSTRAINT CK_KHUYENMAI_Ngay CHECK (NgayKetThuc >= NgayBatDau)
);
GO

/* ============================ THAM SỐ ================================== */
CREATE TABLE THAM_SO (
    TenThamSo NVARCHAR(50)  PRIMARY KEY,
    GiaTri    NVARCHAR(200) NULL,
    MoTa      NVARCHAR(255) NULL
);
GO

PRINT N'== Đã tạo xong CSDL QLSanTheThao (11 bảng) ==';
GO
