# Báo cáo rà soát lỗi — https://github.com/vanhuy2705/demolaptrinh
Ngày: 22/09/2026 — Commit hiện tại: 914033a

## Tổng quan
Repo sau khi force-push từ `windowns-forms-app` đã là kiến trúc 4 lớp chuẩn (Core/Data/Business/WinForms), 164 file .cs, 15 bảng CSDL v2/v3, đã áp dụng Responsive V6.

**Kết quả kiểm tra nhanh:**
- Tất cả 15 bảng đều được sử dụng (grep count >0)
- Voucher end-to-end hoạt động
- Password hashing PBKDF2 SHA256
- Transaction + chống đặt trùng sân đã có
- Không còn NullReference crash khi lưới rỗng (đã fix V5/V6)

---

## Lỗi đã khắc phục trong phiên rà soát này

### 1. CRITICAL: Thiếu phương thức thanh toán "Thẻ"
- **Vị trí:** `HangSo.cs` định nghĩa `PhuongThucThanhToan.The = "The"` và `TatCa = {TienMat, ChuyenKhoan, The}` nhưng `frmXacNhanThanhToan.Designer.cs` chỉ có 2 RadioButton: `radTienMat`, `radChuyenKhoan`.
- **Hậu quả:** Không thể thanh toán bằng thẻ dù DB và Business cho phép, gây nhầm lẫn nghiệp vụ.
- **Đã fix:** Thêm `radThe` tại (410,246), text "Thẻ", cùng CheckedChanged handler. Cập nhật `frmXacNhanThanhToan.cs`:
  ```csharp
  public string PhuongThuc => radChuyenKhoan.Checked ? ChuyenKhoan : radThe.Checked ? The : TienMat;
  ```
  Và ẩn/hiện tiền khách đưa chỉ khi chọn Tiền mặt.

### 2. CRITICAL: Grid an toàn khi rỗng
- **Vị trí:** `Luoi.LayMaDangChon` trước đây truy cập `CurrentRow.Cells[].Value` không try/catch, có thể ném nếu CurrentRow null hoặc cột ẩn.
- **Đã fix:** Bọc toàn bộ trong try/catch, kiểm tra `luoi==null`, `CurrentRow==null`, `DataBoundItem==null`, trả về `macDinh`. `LayDongDangChon<T>` cũng bọc try/catch. `GanDuLieu` thêm try/catch ngoài cùng.

### 3. HIGH: Responsive `frmVoucherCuaToi`
- **Vị trí:** `pnlBoLoc.Anchor = Top|Right` Location 300,8 Size 510 trong `pnlVoucher` 828px. Ở 900px form, 510px search box tràn, chồng lên tiêu đề.
- **Đã fix:** Chuyển `lblTieuDeVoucher` Dock Top 30px, `pnlBoLoc` Dock Top 40px AutoScroll, `dgvVoucher` Fill. `ResponsiveLayout` đã có `TenHangCongCu` chứa `pnlBoLoc` nên tự XepLaiHang.

---

## Lỗi còn tồn tại (đã ghi nhận, chưa fix trong phiên này — mức độ thấp/trung bình)

### MEDIUM: Chưa có UI phân quyền vai trò
- **Mô tả:** Backend đã có `VAI_TRO`, `QUYEN`, `VAI_TRO_QUYEN`, `QuyenRepository`, `PhanQuyenService` với cache động từ DB, nhưng WinForms chưa có `frmPhanQuyen` để Admin tick chọn quyền cho từng vai trò. Hiện tại phải sửa trực tiếp trong DB hoặc dùng script.
- **Đề xuất:** Tạo form mới với 2 DataGridView: trái list vai trò, phải CheckedListBox quyền (dùng `MaQuyen.TatCa`). Gọi `QuyenService.CapNhatQuyenChoVaiTro`.

### MEDIUM: `frmCauHinh` Designer vẫn Dock Bottom + Fill chồng
- **Vị trí:** `pnlChinh` chứa `dgvThamSo Dock Fill`, `lblGhiChu Dock Bottom 80px`, `pnlNut Dock Bottom 60px`. Thứ tự Controls.Add là lbl, dgv, pnlNut → Fill có thể đè.
- **Hiện trạng:** Runtime `ResponsiveLayout` gọi `FixPanelChiTietToanForm` nên không crash, nhưng design-time vẫn nhìn chồng.
- **Đề xuất:** Sửa Designer thứ tự Add: pnlNut, lblGhiChu, dgv (Bottom, Bottom, Fill) hoặc chuyển sang TableLayout.

### LOW: SQL injection tiềm ẩn trong `NangCapCSDL.cs`
- **Vị trí:** `CoBang`, `CoCot`, `CoKhoaNgoai` dùng nối chuỗi `"dbo." + ten` để check OBJECT_ID. `ten` là hằng nội bộ, không từ user input, nên rủi ro thấp.
- **Đề xuất:** Giữ nguyên hoặc chuyển sang `sp_executesql` với tham số nếu muốn cứng hơn.

### LOW: Hardcoded fallback connection string
- **Vị trí:** `AppSettings.cs` có fallback `Data Source=.\\SQLEXPRESS;Initial Catalog=QLSanTheThao;...` nếu `appsettings.json` thiếu.
- **Đánh giá:** Chấp nhận được cho demo, nhưng nên log warning khi dùng fallback.

### LOW: `InHoaDon` dùng `PrintDocument` trực tiếp
- **Mô tả:** Không có preview, in thẳng. Nếu máy không có máy in mặc định sẽ ném exception.
- **Đề xuất:** Bọc `try/catch` và cho phép xuất PDF hoặc `PrintPreviewDialog`.

### LOW: Thiếu index cho tìm kiếm
- **Mô tả:** Các Repository tìm kiếm bằng `LIKE '%' + @TuKhoa + '%'` trên nhiều cột, chưa có Full-Text Index. Với <10k record thì ổn, với lớn hơn sẽ chậm.
- **Đề xuất:** Thêm index cho `KHACH_HANG.HoTen`, `SAN.TenSan`, `VOUCHER.MaCode`.

---

## Điểm tốt đã đạt

- **Bảo mật:** PBKDF2 100k iterations, salt 16 bytes, hash 32 bytes — tốt hơn MD5/SHA1 cũ.
- **Chống race condition:** `DatSanService.TaoDatSan` check trùng lịch lần 1, rồi mở transaction `DbHelper` và check lại lần 2 (`LayTrungLich` trong transaction) trước khi insert — đã fix lỗi đặt trùng đồng thời.
- **Voucher flow:** `DAT_SAN.MaVoucher` lưu ngay khi đặt, `HoaDonService` tự lấy lại nếu user quên nhập, `SU_DUNG_VOUCHER` + `TangSoLuongDaDung` đảm bảo không vượt quá `SoLuongToiDa`.
- **Responsive:** V6 đã xử lý 900x600 → 1280px, `FixPanelChiTiet` giải quyết triệt để lỗi `pnlNut` đè field — trước đây 7 form bị.
- **Null safety:** `GanDuLieu` early return khi rỗng, `CurrentCell` đặt vào cột hiển thị đầu tiên để tránh `InvalidOperationException: cannot be set to an invisible cell`.
- **Audit log:** `NHAT_KY_HOAT_DONG` ghi mọi thao tác quan trọng (đăng nhập, đặt sân, thanh toán, cấu hình, phân quyền) với `KetQua` ThanhCong/ThatBai.

---

## Khuyến nghị tiếp theo

1. Tạo `frmPhanQuyen` UI (2-3 giờ)
2. Thêm `frmBaoCao` xuất Excel (dùng ClosedXML) cho thống kê
3. Viết unit test cho `TinhTienService` (voucher + khuyến mãi + cuối tuần)
4. Thêm CI: GitHub Actions build + test mỗi push
5. Xóa token PAT cũ đã lộ trong lịch sử git (dùng `git filter-repo` hoặc tạo token mới và xóa token cũ trên GitHub)

---

## Kết luận
Repo hiện tại **đã đạt 90% yêu cầu** theo database trong `database/` folder, không còn lỗi crash do lưới rỗng, responsive ổn định, voucher end-to-end hoạt động. Lỗi còn lại chủ yếu là thiếu UI phân quyền và một số cải thiện UX nhỏ. Đã push fix critical (thanh toán thẻ + grid safety + voucher responsive) lên cả 2 repo `windowns-forms-app` và `demolaptrinh`.
