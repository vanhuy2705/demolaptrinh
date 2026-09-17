# BÁO CÁO THIẾT KẾ — QUẢN LÝ CHO THUÊ SÂN THỂ THAO

**Phiên bản:** 1.0  •  **Nền tảng:** .NET 10 / Windows Forms (C#)  •  **CSDL:** SQL Server  •  **Truy cập dữ liệu:** ADO.NET

---

## 1. Giới thiệu

### 1.1 Tên đề tài
Xây dựng phần mềm **Quản lý cho thuê sân thể thao** phục vụ 3 nhóm người dùng: Quản trị viên, Nhân viên và Khách hàng.

### 1.2 Mục tiêu
- Quản lý danh mục loại sân, sân và trạng thái sân theo thời gian thực.
- Đặt sân, kiểm tra trùng lịch, xác nhận, hủy booking.
- Tính tiền tự động theo block thời gian, áp dụng tối đa một ưu đãi, thanh toán và in hóa đơn.
- Quản lý khách hàng, nhân viên, tài khoản, voucher, chương trình khuyến mãi.
- Thống kê doanh thu, cơ cấu giảm giá, top sân theo vai trò được phép xem.

### 1.3 Công nghệ sử dụng

| Thành phần | Lựa chọn | Lý do |
|---|---|---|
| Ngôn ngữ / nền tảng | C# · .NET 10 · Windows Forms | Theo yêu cầu đề tài; triển khai desktop nhanh |
| Truy cập CSDL | **ADO.NET** (`SqlConnection`, `SqlCommand`, transaction) | Kiểm soát trực tiếp câu lệnh, dễ tối ưu và minh bạch |
| Biểu đồ | **ScottPlot 5** (MIT) | Nhẹ, không phụ thuộc license, API vẽ rõ ràng |
| Mật khẩu | **PBKDF2** (iterations.salt.hash, Base64) | Không lưu plaintext; có fallback tương thích dữ liệu cũ |
| Cấu hình | `appsettings.json` (Microsoft.Extensions.Configuration) | Đổi chuỗi kết nối không cần biên dịch lại |

---

## 2. Kiến trúc phần mềm

### 2.1 Bốn lớp (4-tier on desktop)

```
┌──────────────────────────────────────────────────────────────┐
│  SportFieldBooking.WinForms      (Giao diện - Presentation)  │
│  35 Form (Form.cs = logic | Form.Designer.cs = thiết kế)     │
│  Controls: SidebarButton, KpiCard, IconBox, RoundedPanel      │
│  Helpers: GiaoDien, VeBieuTuong, Luoi, BieuDo, InHoaDon,      │
│           TroGiup                                             │
└───────────────────────────┬──────────────────────────────────┘
                            │ gọi Service (không chứa SQL)
┌───────────────────────────▼──────────────────────────────────┐
│  SportFieldBooking.Business     (Nghiệp vụ - Business)       │
│  14 Service: TinhTien, DatSan, HoaDon, Voucher, KhuyenMai,    │
│  PhanQuyen, Auth, TaiKhoan, NhanVien, San, LoaiSan,          │
│  KhachHang, CauHinh, ThongKe  +  KetQua<T>, ChiTietTien      │
└───────────────────────────┬──────────────────────────────────┘
                            │ gọi Repository (interface)
┌───────────────────────────▼──────────────────────────────────┐
│  SportFieldBooking.Data         (Truy cập dữ liệu - DAL)     │
│  12 Repository + BaseRepository + DbHelper (SqlConnection)    │
│  → Đây là nơi DUY NHẤT xuất hiện SqlConnection/SqlCommand     │
└───────────────────────────┬──────────────────────────────────┘
                            │ entity
┌───────────────────────────▼──────────────────────────────────┐
│  SportFieldBooking.Core         (Thực thể & hằng số)         │
│  13 Entity, HangSo (VaiTro/TrangThai/MaQuyen/ThamSoKeys),    │
│  PhienLamViec, PasswordHasher                                │
└──────────────────────────────────────────────────────────────┘
```

### 2.2 Tuân thủ nguyên tắc đặc tả

| Nguyên tắc | Hiện thực |
|---|---|
| Tách `Form.cs` / `Form.Designer.cs` | **35/35 form** đều tách đôi |
| `Designer.cs` chỉ thiết kế | Chỉ khai báo control, `InitializeComponent`, Dock/Anchor, màu/font/icon. Kiểm tra tự động bằng grep: **0** file Designer chứa SQL/CRUD/tính tiền/kiểm tra quyền |
| `Form.cs` chỉ logic | Constructor + event handler + gọi Service, không dựng control |
| SQL chỉ ở tầng Data | Mọi `SqlConnection/SqlCommand` nằm trong `Data/Helpers/DbHelper.cs` và `BaseRepository`; Service/Form không chứa chuỗi SQL |
| Nghiệp vụ ở tầng Business | Giá tiền, giảm giá, trùng lịch, phân quyền nằm trong `Business/Services/*` |
| Phân quyền khi **mở chức năng** | `BaseForm.CoQuyenMoForm()` + `DuocMo(maQuyen, tên)` trong 3 shell; từng nút còn gọi `CoQuyen(...)` để tự disable |

---

## 3. Cơ sở dữ liệu (11 bảng)

| # | Bảng | Mô tả | Cột chính |
|---|---|---|---|
| 1 | `TAIKHOAN` | Tài khoản đăng nhập | `MaTK`, `TenDangNhap`, `MatKhau`, `HoTen`, `VaiTro`, `TrangThai` |
| 2 | `NHAN_VIEN` | Hồ sơ nhân viên (liên kết `MaTK`) | `MaNV`, `MaTK`, `HoTen`, `SDT`, `ChucVu`, `NgayVaoLam`, `TrangThai` |
| 3 | `LOAI_SAN` | Loại sân (bóng đá, cầu lông…) | `MaLoaiSan`, `TenLoaiSan`, `MoTa` |
| 4 | `SAN` | Sân cụ thể + đơn giá | `MaSan`, `TenSan`, `MaLoaiSan`, `DonGia`, `TrangThai`, `MoTa` |
| 5 | `KHACH_HANG` | Khách hàng (có thể liên kết `MaTK`) | `MaKH`, `HoTen`, `SDT`, `Email`, `MaTK`, `NgayTao` |
| 6 | `DAT_SAN` | Booking | `MaDat`, `MaKH`, `MaSan`, `NgayDat`, `GioBatDau`, `GioKetThuc`, `TienSan`, `TrangThai`, `GhiChu` |
| 7 | `VOUCHER` | Mã giảm giá | `MaVoucher`, `MaCode`, `LoaiGiam`, `GiaTriGiam`, `DonToiThieu`, `SoLuong`, `SoLuongDaDung`, `NgayBatDau`, `NgayKetThuc`, `TrangThai` |
| 8 | `SU_DUNG_VOUCHER` | Lịch sử dùng voucher | `MaSuDung`, `MaVoucher`, `MaDat`, `MaKH`, `SoTienGiam`, `NgaySuDung` |
| 9 | `HOA_DON` | Hóa đơn | `MaHD`, `MaDat`, `NgayLap`, `TienGoc`, `LoaiGiamGia`, `TienGiam`, `TongTien`, `PhuongThucThanhToan`, `TrangThai`, `MaVoucher` |
| 10 | `KHUYEN_MAI` | Chương trình khuyến mãi theo khoảng ngày | `MaKM`, `TenKM`, `LoaiKhuyenMai`, `PhanTramGiam`, `NgayBatDau`, `NgayKetThuc`, `ApDungCuoiTuan`, `TrangThai` |
| 11 | `THAM_SO` | Cấu hình hệ thống | `TenThamSo`, `GiaTri`, `MoTa` |

**Ghi chú thiết kế**
- Bổ sung `NHAN_VIEN` và cột `KHACH_HANG.MaTK` so với danh sách 10 bảng của đặc tả — bắt buộc để hiện thực tính năng quản lý nhân viên và cổng tự phục vụ của khách hàng.
- `VOUCHER` được tạo **trước** `HOA_DON` trong `01_TaoCSDL.sql` vì `HOA_DON` có khóa ngoại tới `VOUCHER`.
- Script đi kèm: `01_TaoCSDL.sql` (tạo CSDL + bảng), `02_DuLieuMau.sql` (dữ liệu demo), `03_ResetDuLieu.sql` (xóa dữ liệu, giữ cấu trúc).

---

## 4. Phân quyền (55 mã quyền)

`PhanQuyenService.CoQuyen(vaiTro, maQuyen)` là **chốt chặn duy nhất**; UI chỉ phản ánh kết quả.

| Vai trò | Tập quyền |
|---|---|
| **Admin** | Toàn bộ 55 quyền (`TatCaQuyen`) |
| **Nhân viên** | Đổi mật khẩu; xem loại sân; xem + đổi trạng thái sân; khách hàng (xem tất cả, thêm, sửa); đặt sân (xem tất cả, thêm, sửa, hủy); lịch (xem tất cả); hóa đơn (xem tất cả, lập, sửa, thanh toán, in); voucher (xem, áp dụng); khuyến mãi (xem, áp dụng); thống kê nghiệp vụ |
| **Khách hàng** | Đổi mật khẩu; xem loại sân/sân; hồ sơ của mình; đặt sân (xem của tôi, thêm, hủy); lịch của tôi; hóa đơn của tôi; voucher (xem, sử dụng); khuyến mãi (xem); thống kê cá nhân |

Kiểm tra trong code (không chỉ ẩn nút):
```csharp
private bool DuocMo(string maQuyen, string tenChucNang)
{
    if (PhanQuyenService.CoQuyen(maQuyen)) return true;
    CanhBao($"Bạn không có quyền sử dụng chức năng {tenChucNang}.", "Không đủ quyền");
    return false;
}
```
Ngoài ra mỗi Service cũng tự kiểm tra quyền trước khi ghi dữ liệu (ví dụ `CauHinhService.Luu` → `MaQuyen.CauHinhSua`), nên gọi API trực tiếp cũng bị chặn.

---

## 5. Quy tắc nghiệp vụ

### 5.1 Giá thuê
- Giá **cố định theo từng sân** (`SAN.DonGia`, đồng/giờ), không chia khung giờ sáng/chiều/tối.
- Thời lượng tính tiền = số **block**, mặc định 30 phút/block (`THAM_SO.ThoiLuongBlockPhut`), làm tròn **lên**.
- **Không đặt cọc.**

### 5.2 Giảm giá — tối đa 01 ưu đãi/booking
```
Voucher hợp lệ  >  Khuyến mãi đặc biệt  >  Giảm cuối tuần  >  Giá gốc
```
- Voucher được kiểm tra: tồn tại, còn hạn, `TrangThai = HoatDong`, còn số lượng, đạt đơn tối thiểu.
- Không cộng dồn: voucher 20% thắng giảm cuối tuần 10%.
- Loại giảm được lưu vào `HOA_DON.LoaiGiamGia` (`Khong / Voucher / KhuyenMai / CuoiTuan`).

### 5.3 Ví dụ tính tiền (dữ liệu demo: sân A1 = 400.000 đ/giờ, giảm cuối tuần 10%)

| Tình huống | Thời lượng | Tiền sân | Ưu đãi | Thành tiền |
|---|---|---|---|---|
| T7 17:00–18:00, không voucher | 1,0 giờ | 400.000 đ | Cuối tuần −10% | **360.000 đ** |
| T7 17:00–18:00, dùng `GIAM20` | 1,0 giờ | 400.000 đ | Voucher −20% (thắng cuối tuần) | **320.000 đ** |
| 17:00–18:10 (70 phút → 3 block) | 1,5 giờ | 600.000 đ | — | **600.000 đ** |

### 5.4 Đặt sân
- Kiểm tra **xung đột thời gian** trước khi lưu (`LayTrungLich`, bỏ qua booking đã hủy) → thông báo rõ tên khách, sân, khung giờ trùng.
- Kiểm tra giờ mở/đóng cửa, giờ kết thúc > giờ bắt đầu, không đặt ngày đã qua (với khách hàng).

### 5.5 Thanh toán (transaction)
`HoaDonService.ThanhToan` chạy trong `DbHelper.ChayGiaoDich`:
```
kiểm tra lại voucher → ghi SU_DUNG_VOUCHER → tăng SoLuongDaDung
   → hóa đơn DaThanhToan → booking HoanThanh → sân Trống
```
Lỗi ở bất kỳ bước nào ⇒ **rollback toàn bộ**.

### 5.6 In hóa đơn
`PrintDocument` + `PrintPreviewDialog` (`Helpers/InHoaDon.cs`); thông tin trung tâm (tên, địa chỉ, điện thoại, lời chào) đọc từ `THAM_SO`.

---

## 6. Danh mục 35 màn hình

| Nhóm | Form |
|---|---|
| **Chung (5)** | `frmThongBao`, `frmDangNhap`, `frmDangKy`, `frmDoiMatKhau`, `frmNhapLieu` |
| **Shell (3)** | `frmAdminMain`, `frmNhanVienMain`, `frmKhachHangMain` |
| **Quản trị (10)** | `frmDashboardAdmin`, `frmQuanLyNhanVien`, `frmQuanLyTaiKhoan`, `frmCauHinh`, `frmThongKeAdmin`, `frmDatSanAdmin`, `frmLichDatSanAdmin`, `frmHoaDonAdmin`, `frmVoucher`, `frmKhuyenMai` |
| **Nhân viên (5)** | `frmDashboardNhanVien`, `frmDatSanNhanVien`, `frmLichDatSanNhanVien`, `frmHoaDonNhanVien`, `frmThongKeNhanVien` |
| **Khách hàng (5)** | `frmTrangChuKhachHang`, `frmDatSanKhachHang`, `frmLichSuDatSan`, `frmHoaDonCuaToi`, `frmVoucherCuaToi`, `frmThongTinCaNhan` |
| **Dùng chung nghiệp vụ (7)** | `frmLoaiSan`, `frmSan`, `frmQuanLyKhachHang`, `frmChiTietDatSan`, `frmChiTietHoaDon`, `frmXacNhanThanhToan`, `frmVoucher` |

> Đặc tả yêu cầu 33 form; bản giao nộp có **35 form** do tách riêng dashboard và thống kê cho từng vai trò để phân quyền rõ ràng.

---

## 7. Thiết kế giao diện

### 7.1 Triết lý
- Mọi màu sắc đi qua **một điểm duy nhất**: `Helpers/GiaoDien.cs` (2 bảng màu `MauSang` / `MauToi`).
- `GiaoDien.ApDung(form)` dọn màu **đệ quy** toàn bộ control con nhưng **giữ nguyên** màu cấu trúc (thanh bên, đầu trang, màu trạng thái) nên không vỡ layout.
- **Đổi chủ đề ngay khi chạy** bằng nút “Chủ đề sáng/tối” trên thanh trên cùng của 3 màn hình chính → `GiaoDien.DoiChuDe()` → `LamMoiTatCa()` dọn lại màu cho mọi cửa sổ đang mở (kể cả form con nhúng, lưới, thẻ KPI).

### 7.2 Bảng màu (đạt độ tương phản WCAG AA)

| Vai trò | Chủ đề Tối | Chủ đề Sáng |
|---|---|---|
| Nền cửa sổ `ManHinhNen` | `#11161D` | `#F5F7FA` |
| Thẻ, lưới `BeMat` | `#1A222C` | `#FFFFFF` |
| Viền `Vien` | `#2A3541` | `#E4E9F0` |
| Ô nhập `ONhap` | `#141A22` | `#FFFFFF` |
| Thanh bên `ThanhBen` | `#0C1016` | `#0F172A` |
| Đang chọn `ThanhBenChon` | `#16324E` | `#2F6FED` |
| Chủ đạo `Chinh` | `#2F6FED` | `#2F6FED` |
| Điểm nhấn `DiemNhan` | `#F5A524` | `#F59E0B` |
| Chữ `Chu` / `ChuPhu` | `#E7EDF5` / `#8B9AAD` | `#0F172A` / `#5A6B82` |
| Thành công / Cảnh báo / Nguy hiểm | `#34D399` / `#FBBF24` / `#F87171` | `#16A34A` / `#D97706` / `#DC2626` |

### 7.3 Bộ icon (42 icon vector)
- `Helpers/VeBieuTuong.cs` vẽ bằng GDI+ trên **lưới thiết kế 24×24**, nét `1.75/24`, đầu nét bo tròn, `AntiAlias` + `PixelOffsetMode.HighQuality` (phong cách Lucide/Feather).
- **Không cần font icon hay file ảnh** ⇒ sắc nét mọi DPI, không lỗi thiếu font.
- Dùng qua control `IconBox` (thuộc tính `TenBieuTuong`, `MauBieuTuong` — chỉnh được trong Designer) hoặc `VeBieuTuong.TaoAnh(ten, size, mau)`.
- Kiểm chứng: tất cả tên icon mà 35 form đang dùng đều có bản vẽ thật (**0** icon rơi vào nhánh mặc định).

### 7.4 Lưới dữ liệu & biểu đồ
- `Helpers/Luoi.cs`: định dạng tiền/ngày, ẩn cột kỹ thuật, tô màu trạng thái, autosize, dòng xen kẽ.
- `Helpers/BieuDo.cs`: ScottPlot lấy màu **trực tiếp từ `GiaoDien`** ⇒ biểu đồ luôn đồng bộ chủ đề (không bị nền trắng khi giao diện tối).

### 7.5 Thông báo & xác nhận
`frmThongBao` thay thế `MessageBox` (thông tin / cảnh báo / lỗi / xác nhận), đồng bộ chủ đề.

---

## 8. Kiểm soát lỗi & validation

| Cơ chế | Hiện thực |
|---|---|
| Validate dữ liệu | `Helpers/TroGiup.cs`: `Rong`, `SaiDienThoai`, `SaiEmail`, `SaiTien`, `SaiSoNguyen`; kiểm tra trùng (tên sân, SDT, mã voucher, tên đăng nhập) |
| Try/catch thân thiện | Mọi thao tác đi qua `BaseForm.ThucHien(...)` → hiển thị `frmThongBao`, **không bao giờ** show stack trace |
| Enable/Disable nút | `CapNhatTrangThaiNut()` theo chế độ Xem/Thêm/Sửa + quyền |
| Tham số hóa SQL | Toàn bộ câu lệnh dùng `SqlParameter` ⇒ chống SQL injection |
| Giao dịch | Thanh toán chạy trong transaction, rollback khi lỗi |

---

## 9. Cài đặt & chạy

1. **SQL Server** (Express trở lên) → chạy lần lượt:
   - `database/01_TaoCSDL.sql` (tạo CSDL `QLSanTheThao` + 11 bảng)
   - `database/02_DuLieuMau.sql` (dữ liệu demo)
   - Hoặc double-click `database/ChayCSDL.bat` (gọi `sqlcmd`; thêm `-U <user> -P <pass>` nếu không dùng Windows Auth).
2. Sửa chuỗi kết nối trong `src/SportFieldBooking.WinForms/appsettings.json`.
3. Mở `SportFieldBooking.sln` bằng Visual Studio → **F5**.
   - VS 2022 trở lên: nếu báo thiếu .NET 10, đổi một dòng trong `Directory.Build.props`: `<PhienBanNet>8.0</PhienBanNet>`.
   - Không cần Visual Studio: double-click `ChayUngDung.bat`.

**Tài khoản demo**

| Vai trò | Tên đăng nhập | Mật khẩu |
|---|---|---|
| Quản trị viên | `admin` | `admin123` |
| Nhân viên | `nhanvien` | `nv123456` |
| Khách hàng | `khach1` | `kh123456` |

Dữ liệu demo: sân A1 400.000 đ/giờ; voucher `GIAM20` (20%), `GIAM50K` (50k cho đơn ≥300k), `HETHAN` (hết hạn để test); `PhanTramGiamCuoiTuan = 10`; `ThoiLuongBlockPhut = 30`.

---

## 10. Kịch bản demo (tóm tắt — chi tiết xem `docs/KichBanDemo.md`)

**Phần A — Quản trị viên:** tạo nhân viên → đăng nhập bằng tài khoản vừa tạo → tạo loại sân + sân A1 (400k/h) → tạo khách hàng.
**Phần B — Nhân viên:** đặt sân T7 17:00–18:00 (hệ thống tự giảm cuối tuần 10% → 360.000 đ) → áp dụng `GIAM20` (20% → 320.000 đ, **thắng** giảm cuối tuần) → thử đặt 17:30–18:30 ⇒ **từ chối do trùng lịch** → xác nhận → lập hóa đơn → thanh toán (tiền khách đưa → tiền thừa) → in hóa đơn.
**Phần C — Khách hàng:** tự đăng ký → đặt sân → xem lịch sử → xem hóa đơn của tôi → xem voucher của tôi.

---

## 11. Kiểm chứng

| Hạng mục | Kết quả |
|---|---|
| Build Debug | **0 lỗi / 0 cảnh báo** |
| Build Release | **0 lỗi / 0 cảnh báo** |
| Giả lập Visual Studio 2022 (`-p:PhienBanNet=8.0`) | **0 lỗi / 0 cảnh báo** |
| Designer chứa SQL/nghiệp vụ | **0** file |
| Icon không có bản vẽ (rơi vào mặc định) | **0** tên |
| Đối chiếu script SQL ↔ mã: bảng, cột, dữ liệu mẫu | Khớp toàn bộ (11 bảng / 55 cột, mọi `INSERT` đúng cột) |
| Khóa ngoại | `VOUCHER` tạo trước `HOA_DON` (đúng thứ tự phụ thuộc) |

---

## 12. Hạn chế & hướng phát triển

| Hạn chế | Hướng phát triển |
|---|---|
| Chạy trên Windows (WinForms) | Phiên bản web (ASP.NET Core / Blazor) dùng chung tầng Business |
| Chưa có kiểm thử tự động | Thêm project xUnit test cho `TinhTienService`, `DatSanService` (đã tách biệt khỏi UI) |
| Thông báo trong ứng dụng | Email/SMS xác nhận booking |
| Báo cáo dạng lưới | Xuất Excel/PDF trực tiếp từ lưới và biểu đồ |
| Một chi nhánh | Mở rộng nhiều cơ sở, phân ca nhân viên |
