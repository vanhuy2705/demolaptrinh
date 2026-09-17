# Bản khắc phục giao diện – 17/09/2026

## Lỗi quan sát được

1. **frmDatSanKhachHang / frmDatSanNhanVien / frmDatSanAdmin:** vùng `pnlTrai` rộng 440px + `pnlPhai` rộng khoảng 728px, nhưng khi form được nhúng vào `pnlNoiDung` của màn hình chính chỉ còn khoảng 670–1020px. Hai cột vì vậy bị ép/chồng/cắt.
2. **Lỗi runtime ở form Đặt sân:** `ComboBox.SelectedValue` trong một số thời điểm binding là object `SportFieldBooking.Core.Entities.San`; code cũ gọi `Convert.ToInt32()` khiến phát sinh `Unable to cast object of type '...San' to type 'System.IConvertible'`.
3. **Admin/Nhân viên/Khách hàng:** `GiaoDien.ApDung()` từng ép mọi Form `MinimumSize = 1024x640`; khi Form con được nhúng vào vùng nhỏ hơn, nội dung không thể co theo vùng chứa.
4. **Ba màn hình chính:** `MinimumSize = 1100x700` không phù hợp với một số độ phân giải/mức scale; sidebar/header chiếm diện tích làm vùng nội dung quá hẹp.
5. **Header:** vùng người dùng được Anchor Right nhưng không chừa diện tích cho `pnlChuDe` ở bên phải, nên tên người dùng/vai trò có thể đè vào khu vực nút chủ đề.
6. **DataGridView:** `AutoSizeColumnsMode = Fill` cho lưới có rất nhiều cột làm mỗi cột quá hẹp, tiêu đề bị cắt và dữ liệu khó đọc.
7. **appsettings.json:** bản cũ chứa comment `/* ... */` trong JSON; JSON chuẩn không cho phép comment. Đồng thời chuỗi mặc định dùng `Data Source=.` không khớp instance SQL Server Express thể hiện trên máy là `MSSQL16.SQLEXPRESS`.
8. **Nền giao diện:** vùng trống khá phẳng; bản này bổ sung họa tiết vector thể thao rất nhẹ dưới nền, không ảnh hưởng thao tác.

## Cách khắc phục

- Thêm `ResponsiveLayout.cs` dùng chung.
- Form đặt sân: rộng thì 2 cột; hẹp thì tự xếp dọc và cho cuộn dọc.
- Các màn hình chính: giảm kích thước tối thiểu xuống 900x600 và căn lại vùng thông tin người dùng.
- Lưới nhiều cột: chuyển sang độ rộng dễ đọc + cuộn ngang; lưới ít cột vẫn Fill.
- `LayGiaTriComboBox()` đọc khóa từ `SelectedValue` và fallback qua property của `SelectedItem`, không ép object entity sang `IConvertible`.
- Chuỗi kết nối mặc định: `Data Source=.\SQLEXPRESS;Initial Catalog=QLSanTheThao;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True`.
- `appsettings.json` được đưa về JSON chuẩn.
- Bổ sung nền vector nhẹ cho vùng nội dung.

## Database

Ảnh thư mục SQL Server cho thấy cặp file `QLSanTheThao.mdf` và `QLSanTheThao_log.ldf`. `QLSanTheThao_log.ldf` là **transaction log của database QLSanTheThao**, không phải tên database cần đặt vào `Initial Catalog`.
