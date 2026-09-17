# Kiểm tra lỗi Form – Admin / Nhân viên / Khách hàng

## Lỗi chung có thể xuất hiện ở cả Admin và Nhân viên

| Nhóm lỗi | Nguyên nhân | Khắc phục trong bản này |
|---|---|---|
| Form con rộng hơn vùng nội dung | Form thiết kế 1200px nhưng được nhúng vào vùng chỉ ~670–1020px | `ResponsiveLayout`: Form đặt sân xếp dọc khi hẹp; các Form khác dùng vùng nội dung Dock/Scroll |
| Chồng header người dùng và nút chủ đề | Label Anchor Right nhưng không trừ vùng `pnlChuDe` | Tính lại vị trí theo kích thước header |
| Chữ/cột DataGridView quá nhỏ | `AutoSizeColumnsMode.Fill` trên 10–20 cột | Lưới nhiều cột dùng width đọc được + cuộn ngang |
| Không co được cửa sổ | `MinimumSize` 1024x640 / 1100x700 | Bỏ minimum chung; màn hình chính 900x600 |
| Cắt nội dung theo chiều dọc | Chiều cao màn hình thấp | `AutoScroll` ở vùng nội dung và menu |
| ComboBox gây crash | `SelectedValue` tạm thời là entity `San` nhưng bị `Convert.ToInt32` | Đọc key an toàn, fallback từ `SelectedItem` |
| Khó quan sát khi vùng trống | Nền phẳng | Họa tiết vector thể thao rất nhẹ, không che control |
| SQL không vào đúng instance | Chuỗi dùng `Data Source=.` trong khi máy có SQLEXPRESS | Mặc định `Data Source=.\\SQLEXPRESS;Initial Catalog=QLSanTheThao` |
| appsettings không phải JSON chuẩn | Có comment `/*...*/` bên trong JSON | Đã loại comment, JSON hợp lệ |

## Các Form đặt sân

- Màn hình rộng: 2 cột `pnlTrai` + `pnlPhai`.
- Màn hình vừa/hẹp: tự chuyển thành 1 cột theo chiều dọc.
- Khi xếp dọc, vùng nội dung cho phép cuộn; không còn ép hai panel vào cùng một hàng.
- Điều này áp dụng chung cho `frmDatSanAdmin`, `frmDatSanNhanVien`, `frmDatSanKhachHang`.

## Kiểm tra sau khi mở project

1. Mở `SportFieldBooking.sln`.
2. Chọn **Build > Clean Solution**.
3. Chọn **Build > Rebuild Solution**.
4. Kiểm tra SQL Server instance `MSSQL16.SQLEXPRESS` đang chạy.
5. Nếu chưa có database, chạy `database/01_TaoCSDL.sql`, sau đó `database/02_DuLieuMau.sql`.
6. Chạy ứng dụng và thử resize cửa sổ chính từ toàn màn hình xuống khoảng 900x600.
7. Vào Đặt sân ở Admin, Nhân viên và Khách hàng; kiểm tra bố cục tự chuyển sang dọc khi vùng nội dung hẹp.
8. Thử chọn sân trong ComboBox; lỗi `San -> IConvertible` không còn do helper mới xử lý binding an toàn.
