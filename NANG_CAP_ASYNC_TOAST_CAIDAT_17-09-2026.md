# Bản nâng cấp: Async + Toast + Nhớ cài đặt – 17/09/2026

Triển khai 3 hạng mục đã chốt: **#1 bất đồng bộ hoá**, **#2 toast không chặn**,
**#5 nhớ thiết lập người dùng**. Không đổi nghiệp vụ / phân quyền / cấu trúc CSDL.

## 1. Bất đồng bộ hoá (#1)

**Vấn đề:** mọi truy vấn ADO.NET chạy **đồng bộ trên thread giao diện** → bấm
Tìm / Lưu / mở màn nặng là cửa sổ **đứng hình** tới khi SQL trả về.

**Nền tảng mới (`Data/Helpers/DbHelper.cs`):**
- `[ThreadStatic]` → **`AsyncLocal<T>`** cho kết nối/giao dịch bao quanh.
  Lý do: với `async/await`, continuation có thể chạy trên thread khác nên
  `[ThreadStatic]` sẽ **mất giao dịch**; `AsyncLocal` chảy đúng qua các điểm `await`.
- Thêm bản async: `TruyVanAsync`, `ThucThiAsync`, `GiaTriDonAsync`,
  `ThucThiTraVeMaAsync`, `ChayGiaoDichAsync` (transaction commit/rollback giữ nguyên ngữ nghĩa).

**Tầng giao diện (`Base/BaseForm.cs`):**
- `OnLoad` nay gọi hook **`TaiDuLieuAsync()`** (mặc định vẫn chạy `TaiDuLieu()` đồng bộ
  → các form chưa chuyển đổi **giữ nguyên hành vi**, không hồi quy).
- Thêm `ChayNenAsync<T>(Func<T>)` đẩy hàm nghiệp vụ (không đụng UI) xuống thread nền.
- Thêm `ThucHienAsync(Func<KetQua>)`: chạy nền → hiển thị kết quả trên luồng UI.
- Thêm **lớp phủ "Đang tải…"** mờ (`BatDauBan/KetThucBan`) chặn bấm lặp trong lúc chờ.

**Form đã chuyển đổi làm mẫu:**
- `frmDashboardAdmin`: toàn bộ 4 nhóm truy vấn (tổng quan, doanh thu 7 ngày,
  top sân, lịch hôm nay) chạy nền rồi mới gán KPI/biểu đồ/lưới → mở màn hình **mượt**.
- `frmDangNhap`: xác thực chạy nền, nút chuyển "Đang đăng nhập…".

> Các form còn lại chuyển dần theo đúng mẫu: override `TaiDuLieuAsync`,
> bọc phần truy vấn bằng `ChayNenAsync`, gán UI sau `await`.

## 2. Toast không chặn (#2)

- Control mới `Controls/Toast.cs`: hộp **bo tròn trượt mờ ở góc dưới-phải**,
  tự biến mất sau ~2,6s, **không cướp focus**, không hiện trong Alt-Tab,
  nhiều toast tự xếp chồng lên nhau.
- `BaseForm.ThanhCong(...)` và `ThongTin(...)` nay dùng **toast** thay vì hộp thoại modal
  → thao tác liền mạch, không phải bấm "OK" mỗi lần lưu thành công.
- **Cảnh báo / lỗi / xác nhận vẫn modal** (`frmThongBao`) vì cần người dùng chú ý.

## 3. Nhớ thiết lập người dùng (#5)

Helper mới `Helpers/CaiDatNguoiDung.cs`, lưu JSON vào
`%AppData%\SportFieldBooking\caidat.json` (mọi lỗi đọc/ghi đều được nuốt để không hỏng khởi động):

| Thiết lập | Nơi áp |
|---|---|
| Chủ đề Sáng/Tối | `Program` đọc khi khởi động; `GiaoDien.DoiChuDe()` ghi lại khi đổi |
| Tài khoản đăng nhập gần nhất | `frmDangNhap` điền sẵn + con trỏ nhảy ô mật khẩu; ghi khi đăng nhập thành công |
| Kích thước / trạng thái cửa sổ chính | `BaseMainForm` khôi phục khi mở, ghi lại khi đóng |

## Kiểm chứng

- `dotnet build SportFieldBooking.sln` → **0 error**
- `tests/SportFieldBooking.KiemThu` → **42 đạt / 0 lỗi / 42**

> Xem trực quan: `git pull` → F5. Thử: lưu một thao tác (thấy **toast** lướt qua, không modal),
> mở **Tổng quan** (không còn đứng hình), đổi **Chủ đề** rồi tắt mở lại app (vẫn giữ chủ đề),
> đăng nhập lại (tên đăng nhập đã điền sẵn).
