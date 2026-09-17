# Sửa giao diện "bị chèn" — 17/09/2026

## Hiện tượng
Một số form vẫn bị chèn nội dung dù đã qua các vòng responsive trước:

1. **Thẻ KPI / biểu đồ tràn ra ngoài, thẻ cuối bị cắt** ở `frmDashboardAdmin`,
   `frmDashboardNhanVien`, `frmThongKeAdmin`, `frmThongKeNhanVien`,
   `frmThongTinCaNhan` (và một phần `frmTrangChuKhachHang`).
2. **`frmQuanLyNhanVien`: khối nút (`pnlNut`) đè lên 3 trường cuối**
   (`txtMatKhau`, `cboVaiTro`, `cboTrangThai`) — không cuộn tới được.

## Nguyên nhân gốc

### 1) Toạ độ thiết kế của nhóm thẻ rộng hơn vùng nội dung thật
4 thẻ KPI được đặt ở x = 0 / 296 / 592 / 888, mỗi thẻ 280px → chiếm **1168px**.
Nhưng khi form con được nhúng vào màn hình chính thì vùng nội dung thật chỉ còn:

```
1280 (ClientSize) − 240 (sidebar) − 32 (padding) = 1008px
```

`DashboardResize()` chỉ resize **panel chứa** (`kpi.Width = w`) chứ không sắp lại
các thẻ bên trong → thẻ thứ 4 (bắt đầu ở 888, rộng 280 → kết thúc 1168) bị cắt mất
160px. Chỉ riêng `frmTrangChuKhachHang` có đoạn code xử lý 4 thẻ, các form còn lại
thì không có gì cả; `frmThongTinCaNhan` không có bất kỳ handler resize nào → nặng nhất.

### 2) `VuaKhopNoiDung()` không trừ phần chiều cao đã bị chiếm bởi control Dock
Panel `pnlPhai` của `frmQuanLyNhanVien` cao 610px, trong đó `pnlNut` (Dock=Bottom)
chiếm 180px → vùng thật sự khả dụng chỉ **430px**, nhưng nội dung cần **541px**.
Hàm lại so sánh `541 > 610` → kết luận "vừa" → **không bật AutoScroll**
→ 3 trường cuối nằm lọt xuống dưới `pnlNut` và không có cách nào kéo ra.

Không thể sửa bằng cách "nén khoảng cách dọc": mỗi hàng là nhãn (17px) + ô nhập (30px)
= tối thiểu 49px, mà 430 / 11 hàng = 39px → nén là sẽ chồng nhãn lên ô nhập.
Cũng không thể hạ `pnlNut`: 7 nút xếp 3 hàng đã chiếm đúng 168/180px.
→ **AutoScroll là phương án đúng duy nhất**, chỉ cần tính lại cho đúng chiều cao.

## Cách sửa (`Helpers/ResponsiveLayout.cs`)

### FIX 1 — `VuaKhopNoiDung()` trừ chiều cao các control Dock Top/Bottom
```csharp
int chiemCho = 0;
foreach (Control c in pnl.Controls)
    if (c.Dock == DockStyle.Top || c.Dock == DockStyle.Bottom) chiemCho += c.Height;

int khaDung = pnl.ClientSize.Height - chiemCho;
...
if (canCao > khaDung) { pnl.AutoScroll = true; ... }
```
Áp dụng cho **mọi panel** của mọi form, không chỉ `frmQuanLyNhanVien`.

### FIX 2 — Thêm `SapThe(Panel)`: lớp xếp thẻ tổng quát
Chạy trong `TuDongDanTrang()` nên tự áp dụng cho **mọi panel có ≥ 2 thẻ**
(`KpiCard`, `FormsPlot`) ở **mọi form**, kể cả form thêm sau này — không phụ thuộc tên form.

* Chốt số đo thiết kế của từng thẻ một lần (`ConditionalWeakTable`, không đụng `Tag`
  vì `Tag` đang được nghiệp vụ dùng cho DataGridView/sidebar).
* Tính **số cột tối đa** sao cho mỗi thẻ vẫn đạt bề rộng tối thiểu (180px với KPI,
  280px với biểu đồ).
* Đủ chỗ → **1 hàng, chia theo tỉ lệ thiết kế** (biểu đồ 740/428 vẫn ~2:1 chứ không bị chia đều).
  Nếu chia theo tỉ lệ làm một thẻ nào đó lọt dưới mức tối thiểu → quay về **chia đều**
  để tổng bề rộng không bao giờ vượt panel.
* Không đủ chỗ → **tự xuống hàng**, chia đều, và **nới chiều cao panel**
  (chỉ nới, không bóp) để hàng dưới không bị cắt.
* Chặn cứng cạnh phải: không thẻ nào vẽ vượt `ClientSize.Width − Padding.Right`.
* Panel tự xếp lại mỗi khi form đổi kích thước (đi theo `FormDoiKichThuoc`).

### FIX 3 — Dọn 6 cảnh báo `CS8632`
Thêm `#nullable enable annotations` cho `BaseForm.cs`, `CaiDatNguoiDung.cs`,
`GiaoDien.cs`, `Luoi.cs`. Build giờ **0 error / 0 warning**.

## Kết quả kiểm chứng
Vì không chạy được WinForms trên môi trường Linux, đã mô phỏng lại đúng thuật toán
bằng script (`analysis/sim_layout.py`) trên toạ độ thật lấy từ các file Designer:

| Nhóm thẻ | w=848 | w=1008 | w=1368 |
|---|---|---|---|
| `frmDashboardAdmin/pnlKpi` (4 thẻ) | 1 hàng, cạnh phải 848 ✔ | 1008 ✔ | 1368 ✔ |
| `frmDashboardAdmin/pnlBieuDo` (2 biểu đồ) | 848 ✔ | 1008 ✔ | 1368 ✔ |
| `frmDashboardNhanVien` (KPI + biểu đồ) | ✔ | ✔ | ✔ |
| `frmThongKeAdmin` / `frmThongKeNhanVien` | ✔ | ✔ | ✔ |
| `frmThongTinCaNhan/pnlKpi` | ✔ | ✔ | ✔ |
| `frmTrangChuKhachHang/pnlKpi` | ✔ | ✔ | ✔ |

Không nhóm nào tràn; `frmQuanLyNhanVien/pnlPhai` nay được bật AutoScroll
(nội dung 541px > khả dụng 430px).

* `dotnet build SportFieldBooking.sln` → **Build succeeded, 0 warning**
* Bộ kiểm thử `tests/SportFieldBooking.KiemThu` → **42 đạt / 0 lỗi**
