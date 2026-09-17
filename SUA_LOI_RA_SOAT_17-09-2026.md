# SỬA LỖI ĐỢT RÀ SOÁT TOÀN APP — 17/09/2026

Rà soát toàn bộ solution (Core / Business / Data / WinForms / SQL / Kiểm thử),
phát hiện và sửa **8 lỗi chức năng** + **12 điểm gia cố chống crash**.
Biên dịch sạch (0 warning, 0 error), kiểm thử **73/73 đạt** (thêm nhóm K: 11 test mới).

---

## I. Lỗi chức năng (8)

### 1. Form Hóa đơn (Admin/Nhân viên) và Voucher crash ngay khi mở — NullReferenceException
- **Nguyên nhân:** `dgv.Columns["..."].Visible = false` chạy khi lưới chưa có dữ liệu
  (`Columns` rỗng → `null.Visible`).
- **Sửa:** dùng `Luoi.AnCot(...)` (hỗ trợ null) trong
  `frmHoaDonAdmin.cs`, `frmHoaDonNhanVien.cs`, `frmVoucher.cs`.

### 2. Nhập "1,000,000" ở form Voucher/Khuyến mãi → kiểm tra báo ĐẠT rồi crash khi Lưu
- **Nguyên nhân:** `TroGiup.SaiTien` tự loại bỏ dấu phân cách để kiểm tra, nhưng `btnLuu`
  lại `decimal.Parse` chuỗi gốc → `FormatException` với số có dấu phân cách.
- **Sửa:**
  - `frmVoucher.HopLe` / `frmKhuyenMai.HopLe` trả ra giá trị đã đọc (`out`), `btnLuu`
    dùng trực tiếp, không Parse lại.
  - `TroGiup.SaiTien` thêm tham số `choPhepBangKhong`.
  - "Đơn tối thiểu" voucher được để trống/0 (trước đây bắt nhập > 0); "Số lượng"
    kiểm tra ≥ 1 ngay ở giao diện (khớp service).

### 3. Voucher còn hạn đến hôm nay nhưng bị ẩn khỏi danh sách "còn hạn"
- **Nguyên nhân:** `VoucherRepository.LayTatCa` so sánh cột DATE với `GETDATE()` có giờ
  (vd: hạn 17/09 nhưng 17/09 15:00 đã bị coi hết hạn).
- **Sửa:** dùng `CAST(GETDATE() AS DATE)` (2 vị trí trong câu lệnh).

### 4. Không thể lập hóa đơn khi khách không có voucher
- **Nguyên nhân:** `frmNhapLieu` từ chối chuỗi rỗng, trong khi ô voucher ghi
  "để trống nếu không có" (mặc định `""`) → bấm Đồng ý không qua, bấm Hủy thì thoát.
- **Sửa:** `frmNhapLieu.NhapChuoi` thêm tham số `batBuocNhap` (mặc định giữ như cũ);
  ô voucher (2 form hóa đơn) và 6 ô "lý do hủy" truyền `batBuocNhap: false`.
- Kèm theo: ô "đặt lại mật khẩu" (`frmQuanLyTaiKhoan`) chuyển sang chế độ ẩn ký tự.

### 5. Khách hàng không bao giờ sửa được Thông tin cá nhân
- **Nguyên nhân:** `frmThongTinCaNhan` đòi quyền `KH_SUA` (quyền quản lý, khách không có),
  dù tầng nghiệp vụ cho phép khách tự sửa hồ sơ của mình (có kiểm tra `MaKH` trùng khớp).
- **Sửa:** form chỉ yêu cầu quyền `KH_XEM`; chặn sửa hồ sơ người khác vẫn do
  `KhachHangService` đảm nhiệm (đã có test K.7).

### 6. Khách hàng không in được hóa đơn từ màn hình chi tiết
- **Nguyên nhân:** ma trận `QuyenKhachHang` thiếu `HD_IN` → nút "In" luôn báo thiếu quyền.
- **Sửa:** thêm `MaQuyen.HdIn` vào `QuyenKhachHang`; seed `HD_IN` cho vai trò
  `KhachHang` trong `database/01_TaoCSDL_v2.sql`; vá idempotent (mục D2) +
  cập nhật seed trong `database/04_NangCapCSDL_v2.sql` cho CSDL đã có.

### 7. Quản lý nhân viên: lưu/khóa/mở SAI trạng thái, hiển thị sai (lỗi nặng nhất)
- **Nguyên nhân:** form + service dùng `TrangThaiTaiKhoan` (`BiKhoa`) cho cột
  `NHAN_VIEN.TrangThai` mà CHECK constraint chỉ cho `HoatDong`/`DaNghi`:
  - Lưu form / bấm Khóa-Mở → SQL Server từ chối (CHECK violation);
  - so sánh `== BiKhoa` không bao giờ đúng → nút đảo trạng thái liệt;
  - lưới hiện nhân viên "Đã nghỉ" thành "Hoạt động".
- **Sửa:**
  - Mới: `TrangThaiNhanVien` (`HoatDong`/`DaNghi` + tên hiển thị "Đã nghỉ việc")
    trong `HangSo.cs`; entity `NhanVien` đổi giá trị mặc định theo.
  - `frmQuanLyNhanVien`: lưu/so sánh/hiển thị đúng; nút đổi thành "Cho nghỉ"/"Đi làm lại".
  - `NhanVienService.DoiTrangThai`: từ chối giá trị lạ (kể cả `BiKhoa`) trước khi chạm
    CSDL; cho nghỉ → khóa tài khoản liên kết, đi làm lại → mở khóa.
  - `GiaoDien.MauTrangThai`: `DaNghi` tô màu đỏ.

### 8. Đặt sân có thể chốt với giá cũ + lưới lịch trống khi khách mới mở form
- **Nguyên nhân:** `_ketQuaTien` tính từ trước khi đổi ngày/giờ/sân/voucher;
  form khách không tải lịch ngày khi mở.
- **Sửa:** nút "Đặt sân" (Admin/Nhân viên/Khách) luôn `await TinhTienAsync()` trước khi
  chốt; form khách tải lịch ngày ngay khi mở.

---

## II. Gia cố chống crash (12)

| # | Vị trí | Nội dung |
|---|--------|----------|
| H1 | `BaseForm.OnLoad` | Áp theme trong try/catch: lỗi màu/bố cục không chặn mở form |
| H2 | `Luoi.GanDuLieu` | Đặt ô hiện tại vào cột hiển thị đầu tiên (tránh "invisible cell"), có try/catch |
| H3 | `frmDashboardAdmin` | `TaiTongQuanAsync` có try/catch + overlay bận + kiểm tra `IsDisposed` |
| H4 | `BieuDo` (3 hàm vẽ) | Cắt 2 danh sách về độ dài chung (ScottPlot ném lỗi khi lệch) |
| H5 | `DataRowExtensions.Gio` | `TryParse`, dữ liệu lạ trả `00:00` thay vì ném lỗi |
| H6 | `GiaoDien.LamMoiTatCa` | Chụp danh sách form + try/catch từng form khi đổi chủ đề |
| H7 | `Toast.Hien` | Marshal qua form còn sống, try/catch toàn phần (toast không được làm hỏng app) |
| H8 | `NhanVienRepository` | `NgayVaoLam` null → hôm nay (đúng DEFAULT của cột NOT NULL) |
| H9 | 3 form đặt sân | (đã nêu ở mục 8) |
| H10 | `VeBieuTuong` | Thêm icon `thongtin`/`info` (vòng tròn + chữ i) |
| H11 | `frmLoaiSan` | Xóa hàm `AnCot()` chết; ẩn cột `HinhAnh` sau khi tải dữ liệu |
| H12 | `frmDatSanKhachHang` | (đã nêu ở mục 8) |

---

## III. Kiểm thử mới (nhóm K, 11 test)

`tests/SportFieldBooking.KiemThu/Program.cs` + 2 kho giả mới
(`KhoNhanVienGia`, `KhoTaiKhoanGia` trong `KhoGia.cs`):

- Khách có quyền `HD_IN`; không có `KH_SUA`.
- `TrangThaiNhanVien` đúng 2 giá trị, khác `BiKhoa`, hiển thị "Đã nghỉ việc".
- Khách tự sửa được hồ sơ mình; không sửa được hồ sơ người khác; không đi đường quản lý.
- `DoiTrangThai` từ chối `BiKhoa`; cho nghỉ → khóa tài khoản; đi làm lại → mở khóa.

Kết quả đầy đủ: `docs/KetQuaKiemThu.txt` — **73 đạt / 0 lỗi / 73 tổng số**.

---

## IV. Lưu ý khi triển khai CSDL

- CSDL tạo mới: chạy `database/01_TaoCSDL_v2.sql` (đã gồm seed `HD_IN`).
- CSDL đang dùng: chạy lại `database/04_NangCapCSDL_v2.sql` (mục D2 vá quyền `HD_IN`,
  chạy nhiều lần an toàn).
