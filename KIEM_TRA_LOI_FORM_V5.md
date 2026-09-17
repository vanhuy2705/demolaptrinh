# V5 - Kiểm tra và khắc phục giao diện

## Mục tiêu
- Không để control bị che/lấp khi đổi kích thước cửa sổ hoặc DPI.
- Font dùng thống nhất Segoe UI, cỡ vừa phải, tiêu đề rõ ràng.
- Sidebar/header của Admin, Nhân viên, Khách hàng co giãn đồng bộ.
- Form đặt sân chuyển 2 cột -> 1 cột khi hẹp.
- Form quản lý chuyển danh sách + chi tiết thành bố cục dọc khi hẹp.
- DataGridView nhiều cột dùng cuộn ngang thay vì ép chữ.
- Dashboard/thống kê tự xếp dọc khi vùng hiển thị nhỏ.
- Form đăng nhập tự căn giữa và co giãn khung nhập.
- Nội dung dài có thanh cuộn, không bị mất nút cuối form.

## Các Form đã bao phủ
Admin: Dashboard, đặt sân, lịch đặt sân, hóa đơn, voucher, khuyến mãi, sân, loại sân, khách hàng, nhân viên, tài khoản, thống kê, cấu hình.
Nhân viên: Dashboard, đặt sân, lịch đặt sân, hóa đơn, thống kê và các form dùng chung.
Khách hàng: Trang chủ, đặt sân, lịch sử đặt sân, hóa đơn, voucher, thông tin cá nhân.
Dùng chung: đăng nhập, đăng ký, đổi mật khẩu, chi tiết đặt sân, chi tiết hóa đơn, thông báo, xác nhận thanh toán.

## Lưu ý kiểm thử
Môi trường này không có .NET SDK/Windows Forms runtime nên không thể chạy GUI thực tế tại đây. Khi mở bằng Visual Studio trên Windows: Clean Solution -> Rebuild Solution -> chạy thử ở 1920x1080, 1366x768 và khoảng 900x600; đồng thời thử Windows Scale 125%/150%.
