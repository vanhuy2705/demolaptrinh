# Sửa chèn ở màn Thống kê (pnlDuoi 3 cột) — 17/09/2026

Nối tiếp vòng "sửa giao diện bị chèn". Sau khi rà lại toàn bộ 35 file Designer
bằng script (`analysis/layout_audit_v6.py`, `analysis/sim_dancot.py`) thì còn
đúng **một lỗi chèn thật** chưa được xử lý.

## Hiện tượng
`frmThongKeAdmin` và `frmThongKeNhanVien`, vùng `pnlDuoi` có 3 cột:

| Cột | Nội dung | Toạ độ thiết kế |
|---|---|---|
| 1 | `dgvChiTietNgay` + `lblTieuDeChiTiet` | 0 → 420 |
| 2 | `pnlTopSan` (lưới + biểu đồ top sân) | 436 → 856 |
| 3 | `dgvGiamGia` + `lblTieuDeGiamGia` | 880 → 1168 |

Panel thiết kế rộng 1168px nhưng vùng nội dung thật chỉ 1008px. Cột 1 và 2 neo
`Anchor=Left` nên đứng yên, còn cột 3 neo `Anchor=Right` nên bị WinForms kéo về
trái 160px (880 → 720). Kết quả: **lưới "Giảm giá nhiều nhất" đè lên khối
"Top sân" 136px**, che mất một phần dữ liệu.

## Cách sửa
1. `frmThongKeAdmin.Designer.cs` + `frmThongKeNhanVien.Designer.cs`:
   đổi `dgvGiamGia` và `lblTieuDeGiamGia` từ `Anchor=Right` sang `Anchor=Left`
   → không còn bị kéo trôi, vị trí hoàn toàn do engine điều khiển.
2. `ResponsiveLayout.DashboardResize()`: gọi thêm `DanCot(bottom)` cho `pnlDuoi`.
   `DanCot()` gom các control thành cột theo chồng lấn ngang, rồi chia lại bề rộng
   theo **đúng tỉ lệ thiết kế**, giữ khe hở tối thiểu 14px, chặn không cho tràn
   cạnh phải. Có **cổng chặn**: chỉ can thiệp khi thật sự có chồng lấn, tràn cạnh
   phải, hoặc màn hình rộng mà các cột không giãn ra (thừa khoảng trống) — nên
   không đụng vào bố cục đang đúng.

## Vì sao không làm thành pass tổng quát cho mọi panel
Đã thử. Bản mô phỏng cho thấy luật tổng quát đụng **22 panel**, trong đó có các
dialog cố định (`frmChiTietDatSan`, `frmXacNhanThanhToan`, `frmThongBao`...) và các
panel nhập liệu — thêm điều kiện theo chiều dọc thì vẫn đụng **18 panel** và có nguy
cơ dời cả ô nhập của `frmThongTinCaNhan`, `frmVoucher`, `frmQuanLy*` sang vị trí sai.
→ Thu hẹp phạm vi: chỉ chạy cho `pnlDuoi` trong `DashboardResize`.

## Kiểm chứng (mô phỏng đúng thuật toán, đo trên toạ độ thật)
```
frmThongKeAdmin/pnlDuoi & frmThongKeNhanVien/pnlDuoi
  w= 848 -> 0..301 | 317..618 | 642..848      ok (khong lan, khong tran)
  w=1008 -> 0..360 | 376..736 | 760..1008     ok
  w=1368 -> 0..494 | 510..1004 | 1028..1368   ok
  w=1600 -> 0..581 | 597..1178 | 1202..1600   ok
```
Cột 1 luôn chứa `dgvChiTietNgay` + `lblTieuDeChiTiet`, cột 3 luôn chứa
`dgvGiamGia` + `lblTieuDeGiamGia` (tiêu đề thẳng hàng với lưới của nó).

Rà lại 46 panel khác: không panel nào chồng lấn hay tràn.

* `dotnet build` → **0 error / 0 warning**
* Kiểm thử nghiệp vụ → **42 đạt / 0 lỗi**
