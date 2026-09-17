# Phủ bất đồng bộ toàn bộ form — 17/09/2026

Vòng nâng cấp thứ 5: hoàn tất mục **#1 Bất đồng bộ hoá** — không còn truy vấn cơ sở dữ liệu nào
chạy trên luồng giao diện (UI thread) ở bất kỳ form nào.

## 1. Phạm vi

| Nhóm | Form | Trạng thái |
|---|---|---|
| Đã async từ vòng 4 | `frmDashboardAdmin`, `frmDangNhap` | giữ nguyên |
| Danh mục CRUD | `frmLoaiSan`, `frmKhuyenMai`, `frmQuanLyKhachHang`, `frmVoucher`, `frmSan`, `frmQuanLyNhanVien`, `frmQuanLyTaiKhoan`, `frmCauHinh` | ✅ chuyển |
| Nghiệp vụ | `frmLichDatSanAdmin`, `frmLichDatSanNhanVien`, `frmHoaDonAdmin`, `frmHoaDonNhanVien`, `frmDatSanAdmin`, `frmDatSanNhanVien`, `frmDatSanKhachHang`, `frmChiTietDatSan`, `frmChiTietHoaDon` | ✅ chuyển |
| Phía khách hàng | `frmTrangChuKhachHang`, `frmLichSuDatSan`, `frmHoaDonCuaToi`, `frmVoucherCuaToi`, `frmThongTinCaNhan` | ✅ chuyển |
| Báo cáo | `frmThongKeAdmin`, `frmThongKeNhanVien`, `frmDashboardNhanVien` | ✅ chuyển |
| Hộp thoại | `frmDangKy`, `frmDoiMatKhau` | ✅ chuyển (dùng `Task.Run` vì không kế thừa `BaseForm`) |

Kết quả: **26/26 form** có `TaiDuLieuAsync`, **0** form còn `protected override void TaiDuLieu()`.

## 2. Mẫu chuyển đổi thống nhất

```csharp
private async Task TimKiemAsync()
{
    string tuKhoa = txtTimKiem.Text.Trim();      // 1. đọc UI TRƯỚC (trên luồng UI)

    BatDauBan();                                 // 2. phủ lớp "Đang tải…"
    try
    {
        var danhSach = await ChayNenAsync(() =>  // 3. truy vấn chạy nền
            ServiceFactory.LoaiSan.LayTatCa(tuKhoa));

        Luoi.GanDuLieu(dgvLoaiSan, danhSach);    // 4. gán UI sau khi await (đã về luồng UI)
        HienThiChiTiet();
        CapNhatTrangThaiNut();
    }
    catch (Exception ex) { BaoLoi("Không thể tải danh sách loại sân", ex); }
    finally { KetThucBan(); }
}
```

- Hook tải dữ liệu: `protected override Task TaiDuLieuAsync() => TimKiemAsync();`
- Nút bấm/handler đồng bộ gọi kiểu *fire-and-forget*: `_ = TimKiemAsync();`
- Thao tác ghi (Thêm/Sửa/Xóa/Thanh toán/Hủy/Lập hóa đơn):
  `await ThucHienAsync(() => ServiceFactory.X.Y(...))` hoặc
  `KetQua<T> kq = await ChayNenAsync(() => ...)` rồi mới `ThucHien(kq)`.

## 3. Ba lỗi kinh điển đã chặn từ trước

1. **Đọc control trong luồng nền** — mọi giá trị UI (`txt…`, `cbo…`, `dtp…`) đều được đọc ra biến
   cục bộ *trước* khi vào `ChayNenAsync`. Đã rà soát tự động toàn bộ 35 form: 0 vi phạm.
2. **Kết quả về sai thứ tự** — `TinhTienAsync()` bắn ra mỗi lần gõ mã voucher / đổi giờ, nên có
   bộ đếm phiên `_phienTinhTien`: kết quả cũ hơn bị bỏ qua, cộng kiểm tra `IsDisposed` trước khi gán UI.
3. **Lớp phủ tắt sớm khi hàm async lồng nhau** — `BatDauBan/KetThucBan` trong `BaseForm` nay
   đếm số lượt (`_soLuotBan`), chỉ gỡ lớp phủ khi lượt cuối kết thúc.

## 4. Thay đổi hạ tầng

- `Base/BaseForm.cs`: lớp phủ "Đang tải…" đếm lồng nhau.
- `Program.cs`: thêm lưới an toàn exception toàn cục
  (`Application.ThreadException`, `AppDomain.UnhandledException`, `TaskScheduler.UnobservedTaskException`)
  — lỗi phát sinh từ handler `async void` giờ hiện hộp thoại "Lỗi không mong muốn"
  thay vì làm ứng dụng thoát đột ngột.
- Nạp combo cũng xuống nền: danh sách sân (`frmLichDatSan*`), loại sân (`frmSan`),
  sân trống (`frmDatSanKhachHang`), khách hàng + sân (`frmDatSan*`),
  thông tin cửa hàng khi in hóa đơn (`frmChiTietHoaDon`, `frmHoaDon*`).

## 5. Kiểm chứng

- `dotnet build SportFieldBooking.sln` → **Build succeeded**, 0 error, không phát sinh warning mới.
- Bộ kiểm thử `SportFieldBooking.KiemThu` → **42 đạt / 0 lỗi**.
- Rà soát tĩnh tự động: không còn lời gọi `ServiceFactory.*` đồng bộ nào nằm ngoài luồng nền
  (ngoại lệ duy nhất: `TaoThongBaoTrungLich` — chỉ ghép chuỗi, không chạm CSDL).

## 6. Còn giữ đồng bộ (có chủ đích)

- `InHoaDon.XemTruoc(...)` — dựng cửa sổ xem trước, bắt buộc chạy trên luồng UI.
- Hộp thoại xác nhận `XacNhan/CanhBao/BaoLoi` — vẫn modal để chặn thao tác sai.
