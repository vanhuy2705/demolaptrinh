# HƯỚNG DẪN MỞ & CHẠY BẰNG VISUAL STUDIO

## 1. Yêu cầu

| Thành phần | Ghi chú |
|---|---|
| **Visual Studio** | 2026 (bản 18.x) cho .NET 10 · 2022 (17.8+) cho .NET 8 · 2019 (16.11) cho .NET 6 |
| Workload | **.NET desktop development** (bắt buộc để có WinForms designer) |
| SQL Server | 2016 trở lên (Express đủ dùng) + SSMS |

> **Nếu Visual Studio của bạn cũ hơn yêu cầu .NET 10**: mở file `Directory.Build.props`
> ở thư mục gốc và đổi **một dòng** ` <PhienBanNet>10.0</PhienBanNet> ` thành `8.0`
> (VS 2022) hoặc `6.0` (VS 2019), rồi **Build ▸ Clean Solution** và build lại.
> Không cần sửa từng project.

## 2. Các bước chạy

1. **Mở solution**: double-click `SportFieldBooking.sln`
   (solution dạng `.sln` cổ điển — mọi bản Visual Studio đều đọc được).
2. **Đặt project khởi động**: chuột phải `SportFieldBooking.WinForms` ▸ **Set as Startup Project**.
3. **Khôi phục NuGet** (tự động; nếu lỗi: **Tools ▸ NuGet Package Manager ▸ Restore**).
   Phiên bản gói được quản lý **tập trung** trong `Directory.Packages.props`
   (Visual Studio hiển thị tại *Manage NuGet Packages for Solution*):
   `Microsoft.Data.SqlClient 7.0.3`, `ScottPlot.WinForms 5.1.59` (giấy phép MIT).
4. **Tạo CSDL**: chạy **`database\ChayCSDL.bat`** (mặc định server `localhost`,
   Windows Authentication; dùng SQL Auth: `ChayCSDL.bat .\SQLEXPRESS -U sa -P matkhau`),
   hoặc mở SSMS ▸ **File ▸ Open ▸ File** chạy lần lượt
   `database\01_TaoCSDL.sql` rồi `database\02_DuLieuMau.sql`.
5. **Sửa chuỗi kết nối** trong `src\SportFieldBooking.WinForms\appsettings.json`
   (file này được copy tự động vào thư mục `bin\Debug\net10.0-windows\`, vì vậy
   sửa file nguồn rồi build lại là đủ — **không** cần sửa trong thư mục bin):
   ```json
   "DefaultConnection": "Data Source=.\\SQLEXPRESS;Initial Catalog=QLSanTheThao;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
   ```
6. **Chạy**: nhấn **F5** (Debug) hoặc **Ctrl+F5** (không debug).
7. Đăng nhập: `admin / admin123` · `nhanvien / nv123456` · `khach1 / kh123456`.

## 3. Thiết kế form trong Visual Studio

- Mỗi form gồm **2 file**: `frmXxx.cs` (logic) và `frmXxx.Designer.cs` (thiết kế).
- Để mở designer: chuột phải file `frmXxx.cs` ▸ **View Designer** (Shift+F7).
- `frmXxx.Designer.cs` **chỉ chứa mã thiết kế** (control, `InitializeComponent`, Dock/Anchor,
  màu, font). Không được đưa SQL/CRUD/tính tiền/phân quyền vào file này.
- Các control tự vẽ (`RoundedPanel`, `IconBox`, `SidebarButton`, `KpiCard`) nằm trong
  `Controls/`, có thuộc tính hiển thị trong cửa sổ **Properties** (nhóm *Giao dien*)
  nên có thể kéo thả và chỉnh trực tiếp trên designer.
- Muốn chỉnh nhanh bằng tay: chuột phải form ▸ **View Code** — designer sẽ hiện cảnh báo
  nếu bạn mở designer khi form đang lỗi build, hãy build trước rồi mở lại.

## 4. Xử lý lỗi thường gặp

| Hiện tượng | Cách xử lý |
|---|---|
| *“The project targets .NET 10.0 which is not installed”* | Cài .NET 10 SDK, hoặc đổi `PhienBanNet` trong `Directory.Build.props` xuống 8.0/6.0 |
| *“A project with an Output Type of WinExe cannot be started directly”* | Chuột phải project WinForms ▸ **Set as Startup Project** |
| Lỗi `NU1701` (OpenTK/SkiaSharp) | Cảnh báo vô hại do ScottPlot kéo theo; đã được ẩn trong `Directory.Build.props` |
| *“Login failed for user”* / không kết nối được | Sai `Data Source` hoặc `Integrated Security`; thử `User ID=sa;Password=...;TrustServerCertificate=True` |
| *“Cannot open database QLSanTheThao”* | Chưa chạy `01_TaoCSDL.sql`, hoặc sai tên DB trong connection string |
| Form bị mờ trên màn hình 4K | Đã bật `PerMonitorV2` qua `app.manifest`; đăng xuất Windows rồi đăng nhập lại nếu vẫn mờ |
| Designer báo lỗi không tải được | Build solution trước (**Ctrl+Shift+B**), sau đó đóng/mở lại designer |
| Muốn demo lại từ đầu | Chạy `database\03_ResetDuLieu.sql` rồi F5 lại |

## 5. Chạy nhanh không cần mở Visual Studio

Double-click **`ChayUngDung.bat`** ở thư mục gốc (build Debug rồi chạy ứng dụng).
Tạo/xóa CSDL cũng có sẵn `database\ChayCSDL.bat` và `database\03_ResetDuLieu.sql`.

## 6. Build & đóng gói

- **Debug**: `bin\Debug\net10.0-windows\SportFieldBooking.WinForms.exe`
- **Release**: **Build ▸ Configuration Manager** ▸ *Release* ▸ build lại
  → `bin\Release\net10.0-windows\`
- **Chạy không cần cài Visual Studio**: publish
  (chuột phải project ▸ **Publish** ▸ *Folder* ▸ *self-contained* nếu máy đích chưa có .NET).

## 7. Đổi chủ đề giao diện

Mặc định ứng dụng chạy **chủ đề Tối** (nền `#282828`, điểm nhấn tím `#A080E0`). Toàn bộ màu nằm trong
`src/SportFieldBooking.WinForms/Helpers/GiaoDien.cs` (2 bảng màu `MauSang` / `MauToi`).

- Chuyển chủ đề: mở `Program.cs` → sửa `GiaoDien.ChuyenTheme(toi: false);` để dùng chủ đề Sáng.
- Thêm/sửa màu: đổi trong `MauToi` (hoặc `MauSang`), mọi form, lưới, KPI, biểu đồ sẽ ăn theo
  vì tất cả đều đọc qua property `GiaoDien.Xxx` thay vì hard-code màu.
- Đổi chủ đề lúc chạy: nút **Chủ đề sáng/tối** trên thanh trên cùng của 3 màn hình chính.
- Xem nhanh bảng màu, bố cục và toàn bộ icon: mở file `docs/XemTruocGiaoDien.html` bằng trình duyệt.
- Icon: `Helpers/VeBieuTuong.cs` (lưới 24x24, nét 1.75, bo tròn). Thêm icon mới = thêm một `case "ten":` dùng
  các hàm `b.Line / b.Poly / b.Round / b.Circle / b.Arc / b.Dot / b.Star` với toạ độ theo lưới 24.
