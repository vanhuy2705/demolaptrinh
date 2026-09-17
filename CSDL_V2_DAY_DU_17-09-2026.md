# CSDL bản đầy đủ (v2) + trả lời câu hỏi "vì sao Admin không có bảng riêng" — 17/09/2026

## 1. Trả lời ngắn gọn

Admin **không** có bảng riêng là **chủ ý thiết kế**, theo mô hình RBAC
"một bảng định danh + cột vai trò":

```
TAIKHOAN (MaTK, TenDangNhap, MatKhau, HoTen, VaiTro, TrangThai, NgayTao)
   VaiTro CHECK IN ('Admin','NhanVien','KhachHang')
      ├── NHAN_VIEN  (MaTK → TAIKHOAN)   ← hồ sơ nhân sự của CẢ Admin và Nhân viên
      └── KHACH_HANG (MaTK → TAIKHOAN)   ← hồ sơ khách hàng
```

Lý do:

- Admin bản chất là *nhân viên có toàn quyền*, không phải một loại thực thể khác
  → tách bảng sẽ nhân đôi cột mật khẩu / trạng thái khóa / họ tên.
- `PhanQuyenService.CoQuyen()` (C#) xử lý Admin bằng một nhánh riêng:
  `if (vaiTro == VaiTro.Admin) return TatCaQuyen.Contains(maQuyen);` → Admin có mọi quyền,
  nên CSDL không cần bảng quyền vẫn chạy đúng.
- `NhanVienService.Them(...)` nhận `vaiTro = Admin` và tạo **TAIKHOAN + NHAN_VIEN trong một
  giao dịch** → Admin tạo từ ứng dụng luôn có hồ sơ nhân viên.
- Truy vết `DAT_SAN.MaNguoiTao` và `HOA_DON.MaNguoiLap` trỏ về **TAIKHOAN**,
  nên ghi được cả Admin lẫn Nhân viên. Nếu trỏ về `NHAN_VIEN` thì tài khoản admin
  (vốn không có `MaNV` trong dữ liệu mẫu cũ) sẽ không truy vết được.

**Điểm lệch thật sự (đã sửa trong bản v2):** dữ liệu mẫu cũ tạo tài khoản `admin`
nhưng **không** tạo dòng `NHAN_VIEN`, khiến:

- `PhienLamViec.MaNV = NULL` khi đăng nhập bằng admin;
- admin "vô hình" trong màn hình **Quản lý nhân viên** (không sửa được SĐT/Email/Chức vụ,
  không đổi vai trò/khóa từ đó);
- dữ liệu không nhất quán: admin *seed* thì thiếu hồ sơ, admin *tạo từ app* thì có.

## 2. Ba tập lệnh mới trong `database/`

| Tập lệnh | Dùng khi nào | Nội dung |
|---|---|---|
| `01_TaoCSDL_v2.sql` | Tạo mới từ đầu (⚠ **DROP DATABASE** nếu đã có) | 15 bảng + 5 function + 8 view + 7 procedure + seed 3 vai trò / 56 quyền / ma trận quyền |
| `02_DuLieuMau_v2.sql` | Chạy sau `01_..._v2.sql` | 7 tài khoản, 5 nhân viên (**admin có hồ sơ**), 5 loại sân, 10 sân, 8 khách, 6 voucher, 3 khuyến mãi, 10 tham số, ~200 booking sinh động theo `GETDATE()`, hóa đơn + giảm giá + voucher, 10 dòng nhật ký |
| `04_NangCapCSDL_v2.sql` | **Đã có dữ liệu, không muốn mất** | Chỉ bổ sung, không xóa gì; chạy lại nhiều lần an toàn (idempotent); tự vá hồ sơ `NHAN_VIEN` còn thiếu cho Admin/NhanVien |

Hai tập lệnh cũ `01_TaoCSDL.sql` / `02_DuLieuMau.sql` được **giữ nguyên** để đối chiếu.

### Tương thích ngược với ứng dụng C#

- Giữ nguyên **tên bảng, tên cột, kiểu dữ liệu, giá trị CHECK** của 11 bảng gốc
  → ứng dụng WinForms chạy được ngay, **không phải sửa một dòng code nào**.
- Không thêm cột nào vào bảng cũ (chỉ thêm CHECK/index), nên các câu
  `INSERT INTO ... (danh sách cột)` trong Repository vẫn hợp lệ.
- Index unique trên `Email` lọc cả `NULL` **và chuỗi rỗng**
  (`WHERE Email IS NOT NULL AND LTRIM(RTRIM(Email)) <> N''`), vì ứng dụng lưu `''`
  khi khách không khai email — nếu không lọc sẽ không tạo được khách thứ hai.
- `THONGTKE_TOANBO` / `THONGTKE_NGHIEPVU` / `THONGTKE_CANHAN` giữ nguyên **lỗi gõ**
  của hằng số C# để mã quyền trong DB khớp tuyệt đối với `MaQuyen`.

## 3. Những gì "đầy đủ hơn" so với bản cũ

**Bảng mới**

- `VAI_TRO`, `QUYEN`, `VAI_TRO_QUYEN` — ma trận phân quyền nằm trong CSDL,
  seed đúng 100% theo `PhanQuyenService.cs` (Admin: tất cả; Nhân viên: 23 quyền;
  Khách hàng: 13 quyền). Sau này có thể cho Admin tự cấp quyền mà không cần build lại app.
- `NHAT_KY_HOAT_DONG` — audit trail: ai (`MaTK` + `TenDangNhap` lưu dự phòng),
  vai trò, hành động, bảng/bản ghi bị tác động, nội dung, kết quả
  (`ThanhCong`/`ThatBai`/`CanhBao`), thời gian, máy trạm.

**View (8)** — `v_NguoiDung` (mọi người dùng một nơi, có cột `TinhTrangHoSo`
tự phát hiện tài khoản thiếu hồ sơ), `v_LichDatSan`, `v_HoaDonChiTiet`,
`v_DoanhThuTheoNgay`, `v_ThongKeTheoSan`, `v_KhachHangThanThiet`,
`v_VoucherConHan`, `v_MaTranQuyen`.

**Function (5)** — `fn_LayThamSo`, `fn_LaCuoiTuan` (không phụ thuộc `SET DATEFIRST`),
`fn_SoBlock`, `fn_TienSan` (khớp công thức `TinhTienService.TinhTienTheoBlock`),
`fn_SoBookingTrungLich`.

**Procedure (7)** — `sp_CapNhatTrangThaiBooking` (mặc định **không** tự đóng lượt
đã qua giờ, để hành vi giống ứng dụng; bật bằng `@TuDongHoanThanh = 1`),
`sp_KiemTraTrungLich`, `sp_LayTongQuan`, `sp_DoanhThuTheoNgay`, `sp_ThongKeTheoSan`,
`sp_GhiNhatKy`, `sp_LichTrongCuaSan`.

**Ràng buộc & index** — `UNIQUE` trên `NHAN_VIEN.MaTK` / `KHACH_HANG.MaTK` (1 tài khoản
↔ 1 hồ sơ), CHECK độ dài tên đăng nhập ≥ 4 và họ tên ≥ 2 (đúng rule ứng dụng đang kiểm),
CHECK % voucher 0–100, CHECK tiền ≥ 0; index cho trạng thái booking/hóa đơn,
index lọc theo ngày hết hạn voucher/khuyến mãi.

**Dữ liệu mẫu** — ngày tháng sinh **động** theo `GETDATE()` nên voucher/khuyến mãi
không bao giờ "hết hạn" theo thời gian thực; booking trải 30 ngày quá khứ → 7 ngày tương lai,
tiền sân tính bằng chính `fn_TienSan()`; hóa đơn áp dụng đúng thứ tự ưu tiên giảm giá
của ứng dụng (Voucher > Cuối tuần), 3 hóa đơn mới nhất để `ChuaThanhToan` cho demo thu tiền.

## 4. Kiểm chứng

- Cả ba tập lệnh được parse bằng `sqlfluff` dialect `tsql`: **0 lỗi cú pháp**.
  (Hai lỗi thật đã bắt được nhờ bước này: `GO` lọt vào giữa `BEGIN...END`,
  và `CREATE VIEW/PROCEDURE` phải là lệnh đầu tiên của batch.)
- Script nâng cấp `04` được sinh tự động từ `01_TaoCSDL_v2.sql`
  (generator: `analysis/gen04.py`) để hai bản không lệch nhau.
- Ứng dụng C#: `dotnet build` **Build succeeded**, test **42 đạt / 0 lỗi**
  (không đổi code C# trong lần này).

## 5. Cách chạy

```text
Máy chưa có CSDL      : 01_TaoCSDL_v2.sql  →  02_DuLieuMau_v2.sql
Máy đã có dữ liệu thật: 04_NangCapCSDL_v2.sql   (không mất dữ liệu)
```

Đăng nhập thử sau khi seed: `admin/admin123`, `quanly/admin123`,
`nhanvien/nv123456`, `nhanvien2/nv123456`, `khach1/kh123456`, `khach2/kh123456`.

## 6. Nếu muốn nối C# vào các object mới

Ba việc nhỏ, đều **không bắt buộc**:

1. Đọc quyền từ `VAI_TRO_QUYEN` thay cho ma trận hard-code
   (thêm `QuyenRepository` + cache trong `PhanQuyenService`).
2. Gọi `sp_GhiNhatKy` ở đầu/cuối các thao tác ghi trong tầng Business,
   rồi thêm màn hình "Nhật ký hoạt động" cho Admin (đã có sẵn quyền `NHATKY_XEM` trong DB —
   muốn dùng trong app thì thêm hằng số tương ứng vào `MaQuyen`).
3. Thay các truy vấn thống kê tự ghép trong `ThongKeRepository` bằng
   `sp_LayTongQuan` / `sp_DoanhThuTheoNgay` / `sp_ThongKeTheoSan`.
