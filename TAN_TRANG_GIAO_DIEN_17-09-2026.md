# Bản tân trang giao diện – 17/09/2026

Đại tu phần nhìn trên nền kiến trúc sẵn có, **không đụng nghiệp vụ / phân quyền / SQL**.
Mọi thay đổi tập trung ở tầng hiển thị nên đồng bộ tự động trên cả 35 form.

## 1. Bảng màu mới (2 chủ đề)

| | Tối (mặc định) | Sáng |
|---|---|---|
| Nền ứng dụng | `#0A0E14` đêm sâu ánh lam | `#F6F7FB` sứ nhạt ánh lam |
| Nền thẻ / lưới | `#141A22` | `#FFFFFF` |
| Viền | `#222B36` | `#E6E9F2` |
| Thanh bên | `#06090D` | `#12162B` navy chàm |
| **Màu chủ đạo** | `#6366F1` chàm rực | `#4F46E5` chàm |
| Chủ đạo đậm / nhạt | `#4F46E5` / `#1D2140` | `#4338CA` / `#EEF0FF` |
| Điểm nhấn | `#F59E0B` hổ phách | `#F59E0B` |

Chuyển từ xanh azure cũ sang **họ chàm (indigo)** cho cảm giác hiện đại, sang hơn;
nền tối sâu hơn giúp thẻ và biểu đồ nổi khối. Màu trạng thái (xanh lá / hổ phách / đỏ)
giữ ngữ nghĩa, chỉ tinh chỉnh độ rực cho hợp nền mới.

## 2. Chữ

- Thêm cấp độ: `ChuVua` (Semibold cho nút/nhãn), `SoLon` (20pt Bold cho số KPI).
- Nhỉnh kích thước tiêu đề (`TieuDe` 17.5, `TieuDeLon` 23) để phân tầng rõ hơn.

## 3. Control tự vẽ

- **Nút (`DangNutChinh/Phu/NguyHiem`)**: bo tròn 10px bằng `Region`, cao 40px,
  thêm màu `MouseDown`, đệm ngang 14px → cảm giác nút hiện đại, bấm "đã tay".
- **`RoundedPanel` (thẻ KPI, khung nhập)**: nền **gradient dọc** sáng→tối rất nhẹ
  + **ánh sáng viền trong** ở mép trên → thẻ có chiều sâu, không còn phẳng.
- **`SidebarButton`**: mục đang chọn là **viên bo tròn** nền gradient chàm + viền sáng,
  icon nằm trong **chip bo tròn nhuộm màu chủ đạo**, vạch chỉ báo trái bo tròn;
  hover là viên nền nhạt → thanh bên mềm mại, rõ trạng thái.
- **`KpiCard`**: số liệu dùng `SoLon` to và đậm hơn.

## 4. Màn đăng nhập

- Panel thương hiệu bên trái được `GiaoDien.TrangTriDoc()` vẽ:
  **gradient dọc** + **quầng sáng màu chủ đạo** lệch góc trên-trái + vài nét chéo mờ
  gợi chuyển động thể thao → hết cảm giác khối màu phẳng.

## 5. An toàn & kiểm chứng

- Không đổi tên property/public API nào đang dùng → Designer không vỡ.
- `Region` của nút tự cập nhật khi resize (hook `Resize`), không bị méo khi dàn trang.
- `dotnet build SportFieldBooking.sln` → **0 error**.
- `tests/SportFieldBooking.KiemThu` → **42 đạt / 0 lỗi / 42**.

> Xem trực quan: `git pull` → mở Visual Studio → F5. Bấm nút **Chủ đề** ở góc phải
> đầu trang để chuyển Sáng/Tối và ngắm cả hai bảng màu mới.
