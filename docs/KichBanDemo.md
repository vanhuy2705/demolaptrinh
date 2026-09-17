# KỊCH BẢN DEMO — Quản lý cho thuê sân thể thao V1.0

**Chuẩn bị trước khi demo**
1. Chạy `database\01_TaoCSDL.sql` → `database\02_DuLieuMau.sql` (hoặc `03_ResetDuLieu.sql` nếu muốn bắt đầu lại).
2. Cấu hình chuỗi kết nối trong `src\SportFieldBooking.WinForms\appsettings.json`.
3. Build solution (`dotnet build SportFieldBooking.slnx`), chạy ứng dụng.
4. Tài khoản: `admin/admin123` · `nhanvien/nv123456` · `khach1/kh123456`.

**Ngày giả định cho demo:** chọn **Thứ Bảy tuần này** (bật giảm cuối tuần 10%).
Sân A1 thuộc loại *Sân bóng đá 5 người*, đơn giá **400.000 đ/giờ**.

---

## PHẦN A — QUẢN TRỊ VIÊN (đăng nhập `admin / admin123`)

### A1. Tạo tài khoản cho nhân viên (3 phút)
1. Menu **Tài khoản** → **Thêm**.
   - Tên đăng nhập: `nhanvien2` · Họ tên: `Lê Hoàng Nam` · Vai trò: `Nhân viên`
   - Mật khẩu: `nv123456` (nhập lại để xác nhận).
2. Nhấn **Lưu** → thông báo thành công (mật khẩu được băm PBKDF2, không lưu dạng thô).
3. Menu **Nhân viên** → **Thêm**: chọn tài khoản `nhanvien2`, nhập SDT `0909000111`,
   chức vụ `Lễ tân`, ngày vào làm = hôm nay → **Lưu**.

### A2. Danh mục sân
1. Menu **Loại sân** → thêm mới (nếu trống): `Sân bóng đá 5 người`, `Sân cầu lông`…
2. Menu **Quản lý sân** → **Thêm**:
   - Tên sân: `Sân A1` · Loại: `Sân bóng đá 5 người` · **Đơn giá: 400000** · Trạng thái: `Trống`.
3. **Lưu** → sân xuất hiện trong lưới. Thử **Thêm** lại đúng tên `Sân A1` → hệ thống báo *trùng tên*.
4. Chọn sân → **Đổi trạng thái** → `Bảo trì` → xác nhận → cột Trạng thái đổi màu.
   Chuyển lại `Trống` để tiếp tục demo.

### A3. Tạo khách hàng
1. Menu **Khách hàng** → **Thêm**: `Nguyễn Văn An` · SDT `0987654321` · Email `an.nv@gmail.com`.
2. **Lưu**. Thử thêm khách khác trùng SDT `0987654321` → báo lỗi trùng số điện thoại.

### A4. Đặt sân Thứ Bảy 17:00–18:00 (giảm cuối tuần 10%)
1. Menu **Đặt sân**.
   - Khách hàng: `Nguyễn Văn An` · Sân: `Sân A1` · Ngày: **Thứ Bảy**
   - Giờ bắt đầu `17:00` · Giờ kết thúc `18:00`
2. Bảng tính tiền hiển thị ngay:
   - Đơn giá `400.000 đ/giờ` · Thời lượng `1 giờ (2 block x 30 phút)`
   - Tiền sân `400.000 đ` · Ưu đãi **Giảm giá cuối tuần** · Giảm `40.000 đ` · **Tổng 360.000 đ**
3. Nhấn **Xác nhận đặt sân** → thành công, lịch bên phải xuất hiện booking vừa tạo.

### A5. Áp dụng voucher 20% (phải thắng giảm cuối tuần 10%)
1. Nhập mã `GIAM20` vào ô *Mã voucher* → bảng tính tiền cập nhật ngay:
   - Ưu đãi **Voucher GIAM20** · Giảm `80.000 đ` · **Tổng 320.000 đ**
   - (Giảm cuối tuần 10% **không** được cộng thêm — chỉ áp dụng 01 ưu đãi.)
2. Thử nhập mã `HETHAN` → báo *voucher đã hết hạn*; nhập mã sai `ABCXYZ` → báo *không tồn tại*.
3. Để trống mã voucher trước khi sang bước tiếp theo (khung giờ tiếp theo chỉ để kiểm tra trùng lịch).

### A6. Kiểm tra xung đột lịch (17:30–18:30 → bị từ chối)
1. Chọn lại khung giờ **17:30 – 18:30** trên cùng sân A1, cùng ngày Thứ Bảy.
2. Hệ thống hiện cảnh báo đỏ ngay dưới form: *"Sân A1 đã được đặt … 17:00–18:00 bởi Nguyễn Văn An"*
   và **nút Xác nhận đặt sân bị vô hiệu hóa**.
3. Đổi sang **18:00–19:00** → cảnh báo biến mất, tiền tính lại bình thường.

### A7. Xác nhận booking → Lập hóa đơn → Thanh toán → In
1. Trở lại khung giờ **17:00–18:00** (đã đặt ở A4), chọn dòng booking trong lưới → **Lập hóa đơn**.
2. Hộp thoại **Chi tiết hóa đơn** hiện ra: Tiền sân `400.000`, ưu đãi `Giảm giá cuối tuần`,
   giảm `40.000`, **Tổng thanh toán `360.000 đ`**, trạng thái `Chưa thanh toán`.
3. Nhấn **Thanh toán** → hộp thoại `frmXacNhanThanhToan`:
   - Phương thức: `Tiền mặt` · Tiền khách đưa: `400000` → hiển thị **tiền thừa 40.000 đ** → **Xác nhận**.
4. Thông báo thanh toán thành công. Trạng thái hóa đơn → `Đã thanh toán`;
   booking → `Hoàn thành`; sân A1 → `Trống` (tất cả trong cùng một giao dịch).
5. Mở lại hóa đơn (menu **Hóa đơn** → chọn → **Chi tiết**) → nhấn **In hóa đơn** →
   `PrintPreviewDialog` hiện phiếu thu đầy đủ (tên trung tâm, địa chỉ, điện thoại từ THAM_SO) → có thể in thật.

### A8. Xem thống kê
1. Menu **Thống kê & báo cáo** → chọn **Tháng này** (hoặc khoảng ngày tuần này).
2. Kiểm chứng: KPI doanh thu/booking/hóa đơn chưa thu; biểu đồ đường *doanh thu theo ngày*;
   biểu đồ tròn *cơ cấu tiền giảm* (thấy nhánh “Giảm giá cuối tuần”);
   biểu đồ cột *top sân*; lưới chi tiết từng ngày.
3. Menu **Tổng quan** → KPI ngày + lịch sân hôm nay.

### A9. Cấu hình hệ thống (tùy chọn)
1. Menu **Cấu hình** → sửa `PhanTramGiamCuoiTuan` = `15` → **Lưu cấu hình**.
2. Quay lại màn hình Đặt sân, đặt lại khung giờ cuối tuần → tiền giảm tính theo 15%.
3. Thử nhập `150` vào `% giảm cuối tuần` → hệ thống chặn (0–100).

---

## PHẦN B — KIỂM TRA PHÂN QUYỀN (nhân viên)

### B1. Đăng xuất & đăng nhập nhân viên
1. Tại `frmAdminMain` → **Đăng xuất** → xác nhận → về `frmDangNhap`.
2. Đăng nhập `nv123456`? Không — dùng **nhân viên có sẵn**: `nhanvien / nv123456`
   (hoặc `nhanvien2 / nv123456` vừa tạo ở A1).
3. `frmNhanVienMain` mở ra: **không có** menu Tài khoản / Nhân viên / Cấu hình.

### B2. Nhân viên thực hiện nghiệp vụ
1. **Đặt sân** (menu Đặt sân) → đặt `Sân B1` Thứ Bảy `19:00–20:30`
   (1,5 giờ = 3 block → `900.000 đ`, nếu có khuyến mãi/giảm cuối tuần sẽ tự áp dụng).
2. **Lịch đặt sân** → lọc “Hôm nay/7 ngày”, mở chi tiết một booking, sửa giờ → **Lưu**.
3. **Hóa đơn** → chọn booking chưa thanh toán → **Lập hóa đơn** → nhập mã `GIAM50K`
   (đơn ≥ 300k) → xem tiền giảm `50.000` → **Thanh toán** (chuyển khoản) → **In hóa đơn**.
4. **Khách hàng** → thêm/sửa khách hàng (nhân viên được quyền).
5. **Thông tin sân** → mở `frmSan`: nhân viên **xem được** nhưng các nút Thêm/Sửa/Xóa
   bị vô hiệu hóa (không có quyền `SAN_THEM/SUA/XOA`); nút **Đổi trạng thái** vẫn bật.
6. **Thống kê** → mở `frmThongKeNhanVien` (thống kê nghiệp vụ).

> Nếu cố tình mở chức năng bị chặn (ví dụ gọi form quản lý tài khoản), hệ thống
> không chỉ ẩn nút mà còn kiểm tra `PhanQuyenService` và hiện thông báo
> *“Bạn không có quyền sử dụng chức năng quản lý tài khoản”*.

---

## PHẦN C — CỔNG KHÁCH HÀNG (đăng nhập `khach1 / kh123456`)

1. Đăng xuất → đăng nhập `khach1 / kh123456` → `frmKhachHangMain`.
2. **Trang chủ**: lời chào, KPI cá nhân (số lần đặt, tổng chi tiêu, voucher đã dùng, sân yêu thích),
   lịch sắp tới, voucher đang có thể dùng.
3. Nhấn **Đặt sân ngay** → `frmDatSanKhachHang`:
   - Chọn `Sân A1`, ngày mai (không cho chọn ngày quá khứ), `17:00–18:00`.
   - Bảng tính tiền hiển thị: tiền sân, ưu đãi (cuối tuần nếu đúng T7/CN), giảm, tổng.
   - Nhấn **Voucher của tôi** để xem mã còn hạn, nhập `GIAM20` → tổng tiền giảm 20%.
   - Chọn một khung giờ đã có người đặt → hiện cảnh báo trùng lịch, không cho đặt.
   - **Xác nhận đặt sân** → thành công.
4. **Lịch sử đặt sân** → thấy booking vừa tạo; mở **Chi tiết**; thử **Hủy booking** trên một
   booking khác (trạng thái Đã đặt) → trạng thái chuyển `Đã hủy`.
5. **Hóa đơn của tôi** → xem danh sách, **Chi tiết**, **In hóa đơn** (không có nút thanh toán).
6. **Thông tin cá nhân** → xem thống kê cá nhân, nhấn **Cập nhật** để sửa SDT/email/địa chỉ →
   **Lưu**; nhấn **Đổi mật khẩu** để đổi (cần mật khẩu cũ đúng).

---

## PHẦN D — CÁC TÌNH HUỐNG KIỂM TRA NHANH (QA)

| Tình huống | Kết quả mong đợi |
|---|---|
| Đăng nhập sai mật khẩu / sai tên đăng nhập | Thông báo “Tên đăng nhập hoặc mật khẩu không đúng”, không vào hệ thống |
| Đăng nhập tài khoản bị khóa | Báo “tài khoản đang bị khóa, liên hệ quản trị viên” |
| Đặt sân giờ kết thúc ≤ giờ bắt đầu | Chặn, báo lỗi tại ô giờ kết thúc |
| Đặt sân ngày đã qua hoặc giờ bắt đầu nằm trong quá khứ | Chặn: “Không thể đặt sân cho ngày đã qua” / “Giờ bắt đầu không được nằm trong quá khứ” |
| Đặt trùng lịch | Cảnh báo + vô hiệu hóa nút xác nhận |
| 17:00–18:10 | 70 phút → 3 block → **1,5 giờ** |
| Voucher + giảm cuối tuần cùng lúc | Chỉ ưu đãi cao hơn được áp dụng (không cộng dồn) |
| Voucher hết hạn / hết số lượng / chưa đạt đơn tối thiểu | Thông báo tương ứng, không áp dụng |
| Thuê dưới 30 phút hoặc quá 12 giờ | Chặn, nêu rõ giới hạn thời lượng |
| Thanh toán tiền mặt thiếu tiền khách đưa | Chặn thanh toán, hiện “Thiếu tiền …”, yêu cầu nhập đủ |
| Hủy hóa đơn đã thanh toán | Chặn (chỉ hủy hóa đơn chưa thanh toán) |
| Xóa loại sân còn sân | Chặn, báo còn N sân thuộc loại này |
| Khách mở booking/hóa đơn của người khác | Chặn: “Đây không phải booking của bạn” |
| Mất kết nối CSDL khi mở form | Thông báo thân thiện, không treo ứng dụng |
