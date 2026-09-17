# Bản khắc phục chồng chéo layout – 17/09/2026

## Hiện tượng

Nhiều màn hình bị **các thành phần đè lên nhau / bị cắt mất cạnh**, nặng nhất ở:
thanh bộ lọc (`pnlBoLoc`), thanh công cụ danh sách (`pnlThanhCongCu`),
chân trang nút (`pnlChan`), khối nút form chi tiết (`pnlNut`), vùng tạo hóa đơn
(`pnlTaoHoaDon`) và 4 thẻ KPI ở trang chủ khách hàng.

Đo tĩnh toàn bộ 41 file Designer: **61 lỗi layout thật trên 13 form.**

## Ba nguyên nhân gốc

1. **`GiaoDien.DangNutChinh/Phu/NguyHiem()` ép mọi `Button` về `Height = 38` và
   `MinimumSize = 96x34`**, nhưng Designer lại vẽ panel chứa theo nút cao 34px,
   rộng 66–90px. Hệ quả:
   - Nút bị panel **cắt mất cạnh dưới** (ví dụ `btnLamMoi` cao 38px trong `pnlBoLoc`
     chỉ chừa 36px → thiếu 18px).
   - Nút hẹp bị **nới lên 96px rồi đè sang nút bên cạnh**
     (`btnHomNay` đè `btnLamMoi` ở `frmLichDatSan*`, `btnDong` đè `btnHuyHoaDon`
     ở `frmChiTietHoaDon`).

2. **Toạ độ Designer đặt cho form rộng 1200px.** Khi form bị nhúng vào `pnlNoiDung`
   của màn hình chính (chỉ còn ~670–1020px) mọi control **giữ nguyên toạ độ cũ**
   → tràn ra ngoài vùng nhìn thấy và chồng lấn nhau.

3. **`Tim()` tìm control ĐỆ QUY theo tên.** Khi một Form con đã được nhúng vào
   `pnlNoiDung`, các hàm resize của form cha có thể "nhặt nhầm" panel trùng tên
   của form con (VD `ContentResize` đổi chiều cao `pnlDau` của form con) → vỡ bố cục.
   Ngoài ra nhánh xếp layout cho `frmTrangChuKhachHang` nằm trong `ApDungMain`
   nên **không bao giờ chạy** (đây là form con nhúng, không phải màn hình chính)
   → 4 thẻ KPI đứng yên ở toạ độ 4×280px = 1168px và chồng lên nhau.

## Cách khắc phục

Bổ sung **engine tự động dàn trang** trong `Helpers/ResponsiveLayout.cs`, chạy ở
`OnLoad` sau khi mọi style đã áp xong, và chạy lại mỗi khi bề rộng form/panel đổi:

- `XepLaiHang(panel)` — với các panel công cụ (`pnlBoLoc, pnlChan, pnlThanhCongCu,
  pnlTaoHoaDon, pnlNut, pnlHanhDong`):
  - Gom control **cùng hàng** theo tâm dọc (dung sai 16px), canh giữa hàng.
  - Control nào bị nới kích thước mà **đè lên ô nhập cùng dải dọc** thì tự tách
    xuống hàng mới.
  - Nhóm **neo phải** (`Anchor = Right`) luôn bám mép phải, cách nhau ≥ 8px.
  - Nhóm trái giữ toạ độ thiết kế, chỉ đẩy sang phải khi bị chồng; không đủ chỗ
    thì bật cuộn ngang.
  - **Nới chiều cao panel** cho vừa nội dung thật → không nút nào bị cắt.
- `VuaKhopNoiDung(panel)` — nới chiều cao / bật cuộn cho panel thường để nội dung
  không bị cắt (bỏ qua panel có con `Dock = Fill` để tránh giật layout).
- `ConTrucTiep(cha, ten)` — thay `Tim()` đệ quy bằng tìm **con trực tiếp**
  (`Controls.Find(ten, false)`) ở các hàm resize của form, dứt điểm việc nhặt nhầm
  panel của form con nhúng.
- Chuyển nhánh xếp layout **`frmTrangChuKhachHang`** ra khỏi `ApDungMain` để nó
  thực sự chạy (4 thẻ KPI giờ tự xếp 4 cột / 2×2 theo bề rộng).
- `TuDongDanTrang` chạy **đệ quy trong → ngoài** và **gắn hook `Resize`** cho từng
  panel công cụ, nên khi `ManagementResize`/`BookingResize` thu hẹp panel cha thì
  các hàng bên trong tự xếp lại theo.
- Có **khoá chống tái nhập** (`_dangXep`) và **lưu toạ độ/kích thước gốc** trong
  `Tag` để panel co lại được khi form rộng ra, không phình vĩnh viễn, không lặp vô hạn.

Sửa trực tiếp Designer:
- `frmDoiMatKhau.Designer.cs` — dời 2 nút Xác nhận/Hủy xuống `y = 262` (trước `218`
  đè lên ô `txtXacNhan`), tăng `ClientSize` cao `360 → 400` cho vừa.

## Phạm vi ảnh hưởng

- Chỉ đụng **bố cục/giao diện**; **không** thay đổi nghiệp vụ, phân quyền, SQL.
- `ResponsiveLayout.cs`: 644 → 974 dòng. `frmDoiMatKhau.Designer.cs`: 3 toạ độ.
- Build sạch (0 error). Bộ kiểm thử nghiệp vụ: **42 đạt / 0 lỗi / 42**.

## Kiểm chứng

```
dotnet build SportFieldBooking.sln
dotnet run --project tests/SportFieldBooking.KiemThu   # 42 đạt / 0 lỗi
```

> Lưu ý: môi trường build là Linux (`EnableWindowsTargeting`), chỉ biên dịch được
> chứ không chạy giao diện. Cần mở `SportFieldBooking.sln` trên Windows/Visual Studio
> và **F5** để xem kết quả trực quan, kéo co giãn cửa sổ để kiểm tra responsive.
