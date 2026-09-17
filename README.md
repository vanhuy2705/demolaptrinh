# QUẢN LÝ CHO THUÊ SÂN THỂ THAO — Phiên bản 1.0

Ứng dụng Windows Forms (.NET 10) quản lý cho thuê sân thể thao với 3 vai trò:
**Quản trị viên (Admin)**, **Nhân viên**, **Khách hàng (cổng khách hàng)**.

- Kiến trúc: 4 lớp `Core / Data / Business / WinForms` (Entities → Repository → Service → Form)
- Truy cập dữ liệu: **ADO.NET** thuần (`Microsoft.Data.SqlClient`), tài khoản SQL gom tại `Data/Helpers/DbHelper.cs`
- Giao diện: WinForms tự vẽ (không dùng theme ngoài) + **ScottPlot.WinForms** cho biểu đồ (bản quyền MIT)
- Ngôn ngữ: tiếng Việt có dấu, mã nguồn tiếng Việt không dấu (đồng nhất toàn dự án)

```
SportFieldBooking/
├── SportFieldBooking.sln        -- solution Visual Studio (định dạng .sln cổ điển)
├── Directory.Build.props        -- thuộc tính chung + đổi phiên bản .NET tại đây
├── Directory.Packages.props     -- quản lý phiên bản NuGet tập trung (CPM)
├── .editorconfig                -- quy ước định dạng mã nguồn
├── ChayUngDung.bat              -- build + chạy nhanh bằng CLI (không cần mở VS)
├── database/
│   ├── 01_TaoCSDL.sql          -- tạo DB QLSanTheThao (11 bảng, ràng buộc, index)
│   ├── 02_DuLieuMau.sql        -- tài khoản, sân, khách, voucher, tham số demo
│   ├── 03_ResetDuLieu.sql      -- xóa dữ liệu nghiệp vụ để demo lại từ đầu
│   └── ChayCSDL.bat            -- chạy nhanh 2 script trên bằng sqlcmd
├── docs/
│   ├── HuongDanVisualStudio.md -- mở/chạy/sửa lỗi trong Visual Studio
│   └── KichBanDemo.md          -- kịch bản demo chi tiết từng bước
└── src/
    ├── SportFieldBooking.Core/      -- Entities, Enums/Hằng số, Phiên làm việc, PasswordHasher
    ├── SportFieldBooking.Data/      -- DbHelper, BaseRepository, 12 Repository + Interface
    ├── SportFieldBooking.Business/  -- 14 Service + KetQua + ServiceFactory
    └── SportFieldBooking.WinForms/  -- 35 Form, 4 Control tự vẽ, Helpers, app.manifest, Program.cs
```

## 1. Yêu cầu môi trường

| Thành phần | Phiên bản |
|---|---|
| Windows | 10/11 (WinForms) |
| Visual Studio | **2026** (khuyến nghị, .NET 10) · 2022 (đổi sang .NET 8) · 2019 (đổi sang .NET 6) |
| Workload | **.NET desktop development** |
| .NET SDK | **.NET 10** (`net10.0`, `net10.0-windows`) — có thể hạ cấp ở `Directory.Build.props` |
| SQL Server | 2016 trở lên (Express đủ dùng) |
| NuGet | `Microsoft.Data.SqlClient` 7.0.3, `ScottPlot.WinForms` 5.1.59 |

> Chi tiết từng bước (mở solution, đặt startup project, sửa lỗi thường gặp):
> xem **`docs/HuongDanVisualStudio.md`**.

## 2. Cài đặt & chạy

1. **Tạo CSDL** — chạy `database\ChayCSDL.bat` (dùng `sqlcmd`, mặc định server `localhost`),
   hoặc mở SQL Server Management Studio chạy lần lượt:
   ```
   database\01_TaoCSDL.sql
   database\02_DuLieuMau.sql
   ```
2. **Sửa chuỗi kết nối** trong `src/SportFieldBooking.WinForms/appsettings.json`:
   ```json
   "DefaultConnection": "Data Source=.\\SQLEXPRESS;Initial Catalog=QLSanTheThao;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
   ```
   (SQL Authentication: `User ID=sa;Password=...` thay cho `Integrated Security=True`)
3. **Build & chạy**:
   ```bat
   dotnet build SportFieldBooking.sln
   dotnet run --project src\SportFieldBooking.WinForms
   ```
   (Có sẵn **`ChayUngDung.bat`** ở thư mục gốc: double-click để build + chạy luôn.)
   Hoặc mở `SportFieldBooking.sln` bằng Visual Studio và nhấn **F5**.

### Cách 2 — chạy từ Visual Studio (khuyến nghị)

1. Mở **`SportFieldBooking.sln`** (định dạng `.sln` cổ điển, mọi bản VS đều đọc được).
2. Chuột phải **`SportFieldBooking.WinForms`** ▸ *Set as Startup Project*.
3. Tạo CSDL bằng 2 script trong `database/` (xem phần trên).
4. Sửa chuỗi kết nối trong `src\SportFieldBooking.WinForms\appsettings.json`
   (file được copy tự động vào thư mục output khi build).
5. Nhấn **F5**.

**Nếu Visual Studio của bạn không hỗ trợ .NET 10** (VS 2022/2019):
mở `Directory.Build.props` và đổi một dòng duy nhất, rồi *Build ▸ Clean Solution* và build lại:

```xml
<PhienBanNet>10.0</PhienBanNet>   <!-- VS 2026 -->
<PhienBanNet>8.0</PhienBanNet>    <!-- VS 2022 -->
<PhienBanNet>6.0</PhienBanNet>    <!-- VS 2019 -->
```

### Tài khoản demo (tạo bởi `02_DuLieuMau.sql`)

| Tên đăng nhập | Mật khẩu | Vai trò | Mở |
|---|---|---|---|
| `admin` | `admin123` | Quản trị viên | `frmAdminMain` — toàn quyền |
| `nhanvien` | `nv123456` | Nhân viên | `frmNhanVienMain` — nghiệp vụ, không có tài khoản/nhân viên/cấu hình |
| `khach1` | `kh123456` | Khách hàng | `frmKhachHangMain` — chỉ dữ liệu của chính mình |

> Mật khẩu được băm PBKDF2-SHA256 (`100000.salt.hash`). Ứng dụng vẫn chấp nhận
> mật khẩu dạng thô nếu dữ liệu seed nhập tay (`PasswordHasher` có cơ chế dự phòng).

## 3. Nguyên tắc thiết kế (bắt buộc trong đặc tả, đã tuân thủ)

| Yêu cầu | Cách thực hiện |
|---|---|
| Mỗi form tách `Form.cs` + `Form.Designer.cs` | 35/35 form đều tách đôi |
| `Designer.cs` chỉ chứa thiết kế | Chỉ khai báo control, `InitializeComponent`, Dock/Anchor, màu, font, icon. **Không** có SQL/CRUD/tính tiền/kiểm tra quyền |
| `Form.cs` chỉ chứa logic | Constructor + event handler + gọi Service; **không** dựng control |
| Truy cập CSDL chỉ ở tầng Data | Mọi `SqlConnection/SqlCommand` nằm trong `Data/Helpers/DbHelper.cs` + `BaseRepository`; Service/Form không chứa chuỗi SQL |
| Nghiệp vụ chỉ ở tầng Business | Giá tiền, giảm giá, trùng lịch, phân quyền… nằm trong `Business/Services/*` |
| Phân quyền khi **mở chức năng** | `BaseForm.CoQuyenMoForm()` + `frm*Main.DuocMo(maQuyen, tên)` chặn và thông báo; từng nút còn gọi `CoQuyen(maQuyen)` → tự disable |
| Validate dữ liệu | Trống (`TroGiup.Rong`), định dạng (điện thoại/email/số tiền/số nguyên), trùng (tên sân, SDT, mã voucher, tên đăng nhập) |
| Try/catch thân thiện | Mọi thao tác đi qua `BaseForm.ThucHien(...)` → hiển thị `frmThongBao` custom, không bao giờ show stack trace |
| Enable/Disable nút | `CapNhatTrangThaiNut()` trên mỗi form theo chế độ Xem/Thêm/Sửa + quyền |
| DataGridView đẹp + tìm kiếm | `Helpers/Luoi.cs`: định dạng tiền/ngày, ẩn cột kỹ thuật, tô màu trạng thái, autosize; thanh công cụ tìm kiếm trên các màn hình danh sách |

### Giao diện & chủ đề màu

Ứng dụng dùng **chủ đề Tối "Slate + Azure"** làm mặc định, đồng bộ trên 35 form, 4 control tự vẽ,
40 icon vector và biểu đồ ScottPlot. Bảng màu đạt độ tương phản WCAG AA; toàn bộ màu tập trung tại
`WinForms/Helpers/GiaoDien.cs` (2 bảng màu `MauSang` / `MauToi`).

| Vai trò | Chủ đề Tối | Chủ đề Sáng | Dùng cho |
|---|---|---|---|
| `ManHinhNen` | `#11161D` | `#F5F7FA` | Nền cửa sổ |
| `BeMat` | `#1A222C` | `#FFFFFF` | Thẻ KPI, lưới, panel nội dung |
| `Vien` | `#2A3541` | `#E4E9F0` | Đường viền |
| `ONhap` / `ONhapVien` | `#141A22` / `#33404F` | `#FFFFFF` / `#CBD5E1` | Ô nhập liệu |
| `ThanhBen` | `#0C1016` | `#0F172A` | Thanh bên, đầu trang |
| `ThanhBenSang` / `ThanhBenChon` | `#1B2430` / `#16324E` | `#1E293B` / `#2F6FED` | Hover / mục đang chọn |
| `Chinh` / `ChinhDam` / `ChinhNhat` | `#2F6FED` / `#245BD0` / `#16233A` | `#2F6FED` / `#1D4ED8` / `#E8F0FE` | Nút chính, tiêu điểm, nhãn |
| `DiemNhan` | `#F5A524` | `#F59E0B` | Điểm nhấn, huy hiệu |
| `Chu` / `ChuPhu` / `ChuTrenNenDam` | `#E7EDF5` / `#8B9AAD` / `#F3F7FC` | `#0F172A` / `#5A6B82` / `#FFFFFF` | Chữ chính / phụ / trên nền tối |
| `ThanhCong` / `CanhBao` / `NguyHiem` / `ThongTin` | `#34D399` / `#FBBF24` / `#F87171` / `#60A5FA` | `#16A34A` / `#D97706` / `#DC2626` / `#2563EB` | Trạng thái sân, booking, hóa đơn |

- `GiaoDien.ApDung(form)` dọn màu **đệ quy** cho toàn bộ control con (ô nhập, nhãn, lưới…), nhưng
  **giữ nguyên** màu cấu trúc (thanh bên, đầu trang, màu trạng thái) nên không vỡ layout.
- **Đổi chủ đề ngay khi đang chạy**: nút "Chủ đề sáng/tối" trên thanh trên cùng của cả 3 màn hình chính
  (Admin / Nhân viên / Khách hàng) → `GiaoDien.DoiChuDe()` gọi `LamMoiTatCa()` để dọn lại màu
  cho mọi cửa sổ đang mở (kể cả form con nhúng, lưới dữ liệu, thẻ KPI) mà không cần khởi động lại.
- `Helpers/BieuDo.cs` lấy màu biểu đồ ScottPlot trực tiếp từ `GiaoDien` (nền, lưới, chữ, bảng màu dữ liệu)
  nên biểu đồ luôn đồng bộ chủ đề, không bị trắng trên nền tối.

### Bộ icon vector

`WinForms/Helpers/VeBieuTuong.cs` — **40 icon** vẽ bằng GDI+ trên lưới thiết kế **24&times;24**
(phong cách Lucide/Feather): nét vẽ đồng nhất `1.75/24`, đầu nét bo tròn, `AntiAlias` +
`PixelOffsetMode.HighQuality` nên sắc nét ở mọi DPI, **không cần font icon hay file ảnh**.

- Dùng qua control `IconBox` (thuộc tính `TenBieuTuong`, `MauBieuTuong` — chỉnh được trong Designer)
  hoặc `VeBieuTuong.TaoAnh(ten, size, mau)` để gắn icon vào Button.
- Tên icon: `home, dashboard, thongke, bieudo, chart, calendar, lich, clock, san, sanbong, loaisan, ball,
  building, user, users, taikhoan, nhanvien, hoadon, voucher, khuyenmai, cash, cauhinh, dangxuat, key, khoa,
  search, plus, edit, trash, save, print, refresh, check, back, filter, eye, note, canhbao, phone, star,
  percent, tag, moon, sun, chude` (có alias tiếng Việt/Anh: `them/sua/xoa/luu/in/timkiem/dienthoai/warning/lock/logout/settings/gift/ticket/invoice/field`).
- Xem trước toàn bộ: mở `docs/XemTruocGiaoDien.html`.

## 4. Quy tắc nghiệp vụ chính

**Giá thuê**
- Giá cố định theo từng sân (`SAN.DonGia`, đồng/giờ), **không** chia khung giờ sáng/chiều/tối.
- Thời lượng tính tiền = số **block**, mặc định 30 phút/block (`THAM_SO.ThoiLuongBlockPhut`).
  Làm tròn lên: `17:00–18:10` = 70 phút = 3 block = **1,5 giờ**.
- **Không đặt cọc.**

**Giảm giá — tối đa 01 ưu đãi mỗi booking/hóa đơn**, thứ tự ưu tiên:
```
Voucher hợp lệ  >  Khuyến mãi đặc biệt (đang trong hạn)  >  Giảm cuối tuần  >  Giá gốc
```
- Voucher được kiểm tra: mã tồn tại, còn hạn, `TrangThai = HoatDong`, còn số lượng, đạt đơn tối thiểu.
- Voucher 20% thắng giảm cuối tuần 10% (không cộng dồn).
- Loại giảm thực tế được lưu vào `HOA_DON.LoaiGiamGia` (`Khong / Voucher / KhuyenMai / CuoiTuan`).

**Đặt sân**
- Kiểm tra **xung đột thời gian** trước khi lưu (`LayTrungLich`, bỏ qua booking đã hủy) → cảnh báo rõ tên khách, sân, khung giờ trùng.
- Kiểm tra giờ mở/đóng cửa, giờ kết thúc > giờ bắt đầu, không đặt ngày đã qua (khách hàng).

**Thanh toán (giao dịch)**
`HoaDonService.ThanhToan` chạy trong `DbHelper.ChayGiaoDich` (transaction): kiểm tra lại voucher →
ghi `SU_DUNG_VOUCHER` → tăng `SoLuongDaDung` → hóa đơn `DaThanhToan` → booking `HoanThanh` → sân `Trong`.
Lỗi ở bất kỳ bước nào đều rollback toàn bộ.

**In hóa đơn**: `PrintDocument` + `PrintPreviewDialog` (`Helpers/InHoaDon.cs`), thông tin trung tâm lấy từ `THAM_SO`.

## 5. Danh sách 35 form

| # | Form | Vai trò | Chức năng |
|---|---|---|---|
| 1 | `frmThongBao` | Chung | Hộp thoại thông báo/thông tin/cảnh báo/lỗi/xác nhận (thay MessageBox) |
| 2 | `frmDangNhap` | Chung | Đăng nhập, kiểm tra tài khoản bị khóa, hiện/ẩn mật khẩu |
| 3 | `frmDangKy` | Chung | Khách tự đăng ký tài khoản + tạo hồ sơ khách hàng |
| 4 | `frmDoiMatKhau` | Chung | Đổi mật khẩu (kiểm tra mật khẩu cũ, xác nhận lại) |
| 5 | `frmNhapLieu` | Chung | Hộp thoại nhập chuỗi (dùng cho lý do hủy, mã voucher…) |
| 6 | `frmAdminMain` | Admin | Màn hình chính: sidebar 13 chức năng + khung nội dung |
| 7 | `frmNhanVienMain` | Nhân viên | Màn hình chính: 8 chức năng được cấp quyền |
| 8 | `frmKhachHangMain` | Khách hàng | Cổng khách hàng: 6 chức năng |
| 9 | `frmDashboardAdmin` | Admin | Tổng quan: KPI, doanh thu 7 ngày, tình trạng sân, lịch hôm nay |
| 10 | `frmDashboardNhanVien` | Nhân viên | Tổng quan ca làm việc |
| 11 | `frmLoaiSan` | Admin/NV | CRUD loại sân, cảnh báo xóa khi còn sân |
| 12 | `frmSan` | Admin/NV | CRUD sân + đổi trạng thái (Trống/Đang thuê/Bảo trì) |
| 13 | `frmQuanLyNhanVien` | Admin | CRUD nhân viên, gắn tài khoản, đổi chức vụ, nghỉ việc |
| 14 | `frmQuanLyTaiKhoan` | Admin | CRUD tài khoản, khóa/mở, đặt lại mật khẩu |
| 15 | `frmQuanLyKhachHang` | Admin/NV | CRUD khách hàng, tìm theo tên/SDT (SDT duy nhất) |
| 16 | `frmDatSanAdmin` | Admin | Đặt sân: chọn khách, tính tiền, chống trùng, voucher |
| 17 | `frmDatSanNhanVien` | Nhân viên | Đặt sân tại quầy |
| 18 | `frmDatSanKhachHang` | Khách hàng | Tự đặt sân, xem khung giờ đã kín |
| 19 | `frmLichDatSanAdmin` | Admin | Lịch toàn hệ thống, lọc ngày/sân/trạng thái |
| 20 | `frmLichDatSanNhanVien` | Nhân viên | Lịch ca làm việc |
| 21 | `frmLichSuDatSan` | Khách hàng | Lịch sử đặt sân của tôi + hủy booking |
| 22 | `frmChiTietDatSan` | Chung | Chi tiết booking, sửa, hủy (khách chỉ xem được của mình) |
| 23 | `frmHoaDonAdmin` | Admin | Lập hóa đơn từ booking, thu tiền, in, hủy |
| 24 | `frmHoaDonNhanVien` | Nhân viên | Lập hóa đơn + thu tiền tại quầy |
| 25 | `frmHoaDonCuaToi` | Khách hàng | Xem/in hóa đơn của tôi |
| 26 | `frmChiTietHoaDon` | Chung | Chi tiết hóa đơn, thanh toán, in, hủy |
| 27 | `frmXacNhanThanhToan` | Admin/NV | Chọn phương thức, nhập tiền khách đưa → tiền thừa |
| 28 | `frmVoucher` | Admin/NV | CRUD voucher (số lượng, đơn tối thiểu, thời hạn) |
| 29 | `frmVoucherCuaToi` | Khách hàng | Voucher còn dùng + lịch sử đã dùng |
| 30 | `frmKhuyenMai` | Admin/NV | CRUD chương trình khuyến mãi theo khoảng ngày |
| 31 | `frmCauHinh` | Admin | Tham số: % giảm cuối tuần, block phút, giờ mở cửa, thông tin trung tâm |
| 32 | `frmThongKeAdmin` | Admin | Doanh thu, cơ cấu giảm giá, top sân, chi tiết theo ngày |
| 33 | `frmThongKeNhanVien` | Nhân viên | Thống kê nghiệp vụ (không có lợi nhuận/cấu hình) |
| 34 | `frmTrangChuKhachHang` | Khách hàng | Lời chào, thống kê cá nhân, lịch sắp tới, voucher |
| 35 | `frmThongTinCaNhan` | Khách hàng | Hồ sơ + thống kê cá nhân + đổi mật khẩu |

> Đặc tả yêu cầu 33 form; bản giao nộp này có **35 form** (tách riêng dashboard và
> màn hình thống kê cho từng vai trò để phân quyền rõ ràng).

## 6. Cấu trúc tầng & luồng dữ liệu

```
frmXxx  ──►  ServiceFactory.<Service>  ──►  IRepository  ──►  DbHelper (SqlConnection)
  │             (Business: quyền, tính tiền,   (Data: SQL)        ▲
  │              trùng lịch, giao dịch)                           │
  └── Hiển thị qua BaseForm.ThucHien(KetQua)  ◄── KetQua<T> ─────┘
```

- `Business/Common/KetQua.cs`: `ThanhCong`, `ThongBao`, `DuLieu` — Service không bao giờ ném exception ra UI.
- `Business/Common/ServiceFactory.cs`: singleton các service, form chỉ cần `ServiceFactory.X...`.
- `Data/Helpers/DbHelper.cs`: chuỗi kết nối, `ChayGiaoDich(Action)` (transaction), `TaoLenh` tự gắn transaction hiện tại.
- `WinForms/Helpers/`: `GiaoDien` (màu/font/nút), `VeBieuTuong` (~35 icon vector), `Luoi` (DataGridView), `BieuDo` (ScottPlot), `TroGiup` (validate/format), `InHoaDon` (PrintDocument).
- `WinForms/Controls/`: `RoundedPanel`, `IconBox`, `SidebarButton`, `KpiCard` (kèm `.Designer.cs`, thuộc tính có `[DesignerSerializationVisibility]`).

## 7. Ghi chú bản quyền & bảo mật

- **ScottPlot** (MIT) — dùng tự do, không giới hạn thương mại.
- Mật khẩu: PBKDF2-SHA256, 100.000 vòng lặp, muối ngẫu nhiên 16 byte.
- Chống SQL injection: mọi câu lệnh dùng tham số hóa (`BaseRepository.ThamSo`).
- Chống nâng quyền: không cho đổi vai trò/khóa chính tài khoản đang đăng nhập; khách hàng chỉ
  truy cập dữ liệu của mình (kiểm tra ở cả UI và tầng nghiệp vụ).

---
*Hướng dẫn chạy bằng Visual Studio: `docs/HuongDanVisualStudio.md` ·
Kịch bản demo chi tiết: `docs/KichBanDemo.md`.*

## V5
Bản V5 bổ sung bộ ResponsiveLayout dùng chung cho Admin/Nhân viên/Khách hàng, bố cục dọc cho màn hình hẹp, xử lý form quản lý, đăng nhập, dashboard/thống kê, footer/filter và DataGridView nhiều cột.
