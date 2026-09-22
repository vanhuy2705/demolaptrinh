# Hoàn thiện Nhật ký + Responsive V6 — 22/09/2026

## Mục tiêu
- Hoàn thiện chức năng theo database trong folder `database/` (15 bảng, voucher end-to-end, nhật ký hoạt động)
- Căn chỉnh lại các thành phần trong Windows Forms app (900x600 đến 1280px không chồng/cắt)

## Đã thực hiện trong phiên này

### 1. Navigation Nhật ký hoạt động
- `src/SportFieldBooking.WinForms/Forms/frmAdminMain.cs`:
  - Thêm `case nameof(btnNhatKy):` với kiểm tra quyền `MaQuyen.NhatKyXem`, tiêu đề "Nhật ký hoạt động", `formCon = new frmNhatKyHoatDong()`
  - Cập nhật `DanhDauMenuDangChon(..., btnCauHinh, btnNhatKy)` để highlight sidebar đúng
- `frmAdminMain.Designer.cs` đã có `btnNhatKy` (SidebarButton Dock Top 216x42 tại 12,556, icon "note") — không cần thêm lại

### 2. Sửa lỗi chồng control (Designer)
Các form quản lý trước đây dùng `pnlNut.Dock = Bottom` với Location cố định (412/476/440/496) đè lên các field ở 448+/516:
- `frmQuanLyNhanVien.Designer.cs`: `Dock None`, Location 20,560
- `frmQuanLyKhachHang.Designer.cs`: `Dock None`, Location 20,340
- `frmSan.Designer.cs`: `Dock None`, Location 20,520
- `frmLoaiSan.Designer.cs`: `Dock None`, Location 20,260
- `frmQuanLyTaiKhoan.Designer.cs`: `Dock None`, Location 20,560
- `frmVoucher.Designer.cs`: thêm `AutoScrollMinSize 0,700` cho `pnlPhai`
- `frmKhuyenMai.Designer.cs`: Location 20,490
- `frmNhatKyHoatDong.Designer.cs`: sửa thứ tự Controls.Add — `pnlChan (Bottom)`, `pnlBoLoc (Top)`, `dgv (Fill)` để Dock hoạt động đúng, tránh Fill đè Top/Bottom

### 3. ResponsiveLayout V6 — runtime fix toàn cục
File: `src/SportFieldBooking.WinForms/Helpers/ResponsiveLayout.cs` (viết lại hoàn toàn)

**Mới:**
- `ManagementForms` bao gồm `frmNhatKyHoatDong`, `BookingForms` constant
- `FixPanelChiTiet(Panel pnl)`: phát hiện `pnlNut/pnlChan` Dock Bottom chồng lên field → chuyển sang Dock None, tính maxBottom của các control nhập liệu, đặt Top = maxBottom+16, bật AutoScroll, chuẩn hoá width TextBox/ComboBox/DateTimePicker
- `FixPanelChiTietToanForm(Control root)`: đệ quy toàn form, gọi cho mọi `pnlPhai/pnlTrai`
- `ManagementResize`: wide (>=1040px) → detail Dock Right 340-400, list Fill; narrow → detail Top 500, list Top 430 với toolbar scroll
- `BookingResize`: wide (>=1180px) → left 440 Fixed, right Fill; narrow → stacked 650/600
- `ContentResize`: fix header `pnlDau` overflow nút Làm mới, đảm bảo `pnlNoiDung` AutoScroll cả 2 chiều
- `DashboardResize`: xử lý chung cho `frmDashboard*`, `frmThongKe*`, `frmThongTinCaNhan` qua Kpi/Plot, tắt HorizontalScroll để nút không văng
- `FormDoiKichThuoc`: gọi `TuDongDanTrang` + `FixPanelChiTietToanForm` mỗi lần resize, cache width để tránh lặp

**Engine cũ giữ lại:** `XepLaiHang`, `VuaKhopNoiDung`, `SapThe`, `DanCot`, `TrangTriNen` — đảm bảo không chồng chữ, không cắt, hỗ trợ DPI

### 4. Thống kê & cấu hình
- `ThongKeRepository.cs`: thử dùng `sp_LayTongQuan` và `v_DoanhThuTheoNgay` (CSDL v2) trước, fallback về query cũ nếu CSDL chưa nâng cấp
- `frmCauHinh.cs`: cập nhật hướng dẫn đầy đủ các key mới (`ThoiGianHuyToiDaGio`, `SoNgayDatTruoc`, `DiaChi`, `DienThoai`, `LoiChaoHoaDon`), validate chi tiết (block 0-480, hủy 0-720h, đặt trước 1-365 ngày, HH:mm), mặc định mới

### 5. Voucher end-to-end (đã có từ trước, xác minh)
- `DAT_SAN.MaVoucher` FK → `VOUCHER`, `HOA_DON.MaVoucher` + `LoaiGiamGia`, `SU_DUNG_VOUCHER`
- `DatSanService`: lưu `MaVoucher` ngay khi đặt, `MaVoucherCode` hiển thị
- `HoaDonService`: ưu tiên voucher từ booking, tạo `SU_DUNG_VOUCHER`, tăng/giảm `SoLuongDaDung`
- `VoucherService`, `SuDungVoucherRepository` đầy đủ

### 6. Bảo vệ lưới rỗng
- `Luoi.GanDuLieu`: early return nếu Rows==0, tìm cột hiển thị đầu tiên để đặt CurrentCell, try/catch InvalidOperationException
- `BaseForm`: `FixPanelChiTietToanForm` chạy trước `TuDongDanTrang` để không bị đè

## Kiểm tra
- Không có `dotnet` trong sandbox nên không build được, nhưng đã rà soát:
  - `btnNhatKy` tồn tại trong Designer và `frmAdminMain.cs`
  - `MaQuyen.NhatKyXem/Xoa` có trong `HangSo.cs`
  - `PhanQuyenService` có mapping `NHATKY => nhật ký hoạt động`
  - `NhatKyHoatDongRepository` dùng `SoNguyen64` đã bổ sung trong `DataRowExtensions`
  - `ResponsiveLayout` dùng `KpiCard` (có trong Controls) và `FormsPlot` qua name check để tránh phụ thuộc cứng ScottPlot

## Còn lại / đề xuất
- `dotnet build SportFieldBooking.sln` và chạy WinForms ở 900x600, 1024x768, 1280x720 để xác nhận không còn chồng
- Tạo `frmPhanQuyen` UI để chỉnh `VAI_TRO_QUYEN` trực quan (hiện tại đã dùng qua `QuyenRepository` + cache động)
- Bổ sung audit log cho các service còn thiếu (San, LoaiSan, KhuyenMai, Voucher) — pattern đã có ở `AuthService`, `DatSanService`, `HoaDonService`

## File thay đổi chính
- `src/SportFieldBooking.WinForms/Forms/frmAdminMain.cs`
- `src/SportFieldBooking.WinForms/Forms/frmNhatKyHoatDong.Designer.cs`
- `src/SportFieldBooking.WinForms/Forms/frmQuanLy*.Designer.cs`, `frmSan`, `frmLoaiSan`, `frmVoucher`, `frmKhuyenMai`
- `src/SportFieldBooking.WinForms/Helpers/ResponsiveLayout.cs` (V6)
- `src/SportFieldBooking.WinForms/Forms/frmCauHinh.cs`
- `src/SportFieldBooking.Data/Repositories/ThongKeRepository.cs`
