# Khắc phục: bản vừa rồi build xong nhưng **chạy không được**

Ngày: 17/09/2026 · Commit trước: `c940cdf` (sửa race đặt trùng sân + voucher bị mất)

## 1. Chuyện gì đã xảy ra

Bản `c940cdf` thêm cột `DAT_SAN.MaVoucher` để voucher đi theo lượt đặt sân.
Phần **code C# build sạch** (0 lỗi / 0 cảnh báo, thử cả net10.0 lẫn net8.0),
nhưng **mọi câu truy vấn bảng `DAT_SAN` đều SELECT thêm cột mới**:

```sql
SELECT ... ds.MaNguoiTao, ds.MaVoucher, ... ISNULL(v.MaCode,'') AS MaVoucherCode
FROM DAT_SAN ds
LEFT JOIN VOUCHER v ON v.MaVoucher = ds.MaVoucher
```

Nếu CSDL trên máy bạn **chưa chạy `database/05_BoSungVoucherDatSan_v3.sql`**
thì cột đó chưa tồn tại → SQL Server báo:

```
Invalid column name 'MaVoucher'.
```

→ mở màn Đăng nhập được, nhưng vào màn Đặt sân / Hóa đơn / Thống kê là lỗi.
Cảm giác đúng như "build không chạy được nữa".

**Lỗi thiết kế thuộc về mình:** bắt người dùng nhớ chạy thêm một file SQL,
trong khi chỉ cần `git pull` rồi chạy. Đã sửa để không bao giờ tái diễn.

## 2. Đã sửa (3 lớp phòng vệ)

### Lớp 1 — Ứng dụng TỰ nâng cấp CSDL lúc khởi động
File mới `src/SportFieldBooking.Data/Helpers/NangCapCSDL.cs`, gọi từ `Program.Main`:

- Kiểm tra `COL_LENGTH('dbo.DAT_SAN','MaVoucher')`, thiếu thì `ALTER TABLE ... ADD MaVoucher INT NULL`,
  thêm khoá ngoại `FK_DAT_SAN_VOUCHER`, index `IX_DAT_SAN_Voucher`.
- Hồi tố dữ liệu cũ: booking đã có hóa đơn / lịch sử dùng voucher thì chép `MaVoucher` ngược về `DAT_SAN`.
- **Idempotent** — chạy lại bao nhiêu lần cũng được; **chỉ THÊM**, không xoá hay đổi kiểu dữ liệu.
- Mọi lỗi đều được bắt lại, không làm sập ứng dụng.

Nghĩa là: `git pull` → chạy app → app tự vá CSDL. Không cần mở SSMS.

### Lớp 2 — Repository tự thích ứng khi không có cột
`DatSanRepository` kiểm tra cột có tồn tại không (1 lần/phiên, có nhớ kết quả)
rồi mới lắp câu SQL (`LapSql`):

| Trạng thái CSDL | SELECT / INSERT / UPDATE |
|---|---|
| Đã có `MaVoucher` (v3) | đầy đủ voucher như thiết kế |
| Chưa có (DB cũ, không nâng cấp được) | bỏ hẳn cột + JOIN voucher ra khỏi SQL → **app vẫn chạy bình thường** |

Khi ở trạng thái thứ 2, tính năng "voucher gắn với booking" tạm ngưng
(hóa đơn vẫn tra được voucher qua `SU_DUNG_VOUCHER` như bản cũ), phần còn lại không ảnh hưởng.

### Lớp 3 — Báo lỗi rõ ràng, đúng nguyên nhân
- Không tự nâng cấp được (thường do tài khoản SQL thiếu quyền `ALTER`) → hiện hộp thoại:
  *"Chương trình vẫn chạy bình thường, nhưng voucher gắn với lượt đặt sân sẽ không được lưu.
  Nhờ quản trị chạy `database/05_BoSungVoucherDatSan_v3.sql`"* — kèm lỗi SQL cụ thể.
- Sửa thông báo lỗi kết nối cũ: trước ghi *"chạy database/01_TaoCSDL.sql"* (file cũ),
  nay chỉ đúng `01_TaoCSDL_v2.sql` / `04_NangCapCSDL_v2.sql` + tài khoản demo.

### Ngoài ra — `database/ChayCSDL.bat`
- Phân biệt được 2 loại lỗi: thiếu `sqlcmd` trong PATH vs script SQL chạy thất bại
  (trước đây gộp chung một câu "Không chạy được sqlcmd" → gây hiểu nhầm).
- Thêm chế độ **nâng cấp giữ dữ liệu**: `ChayCSDL.bat .\SQLEXPRESS nangcap` (chạy 04 rồi 05).
- Cảnh báo rõ chế độ tạo mới sẽ DROP CSDL cũ; in tài khoản demo khi tạo xong.

## 3. Kiểm chứng

| Hạng mục | Kết quả |
|---|---|
| `dotnet build` (net10.0 — VS2026) | Build succeeded, 0 lỗi / 0 cảnh báo |
| `dotnet build -p:PhienBanNet=8.0` (VS2022) | Build succeeded, 0 lỗi / 0 cảnh báo |
| Bộ kiểm thử nghiệp vụ | **62 đạt / 0 lỗi / 62** (56 cũ + 6 mới) |

6 kiểm thử mới (nhóm J — *SQL thích ứng lược đồ*):
CSDL v3 có đủ cột/JOIN; CSDL cũ không còn nhắc tới `MaVoucher` ở bất kỳ đâu;
không sót chỗ giữ `{…}`; danh sách cột hợp lệ (không thừa dấu phẩy trước `FROM`) ở cả 2 trạng thái;
khoá chống trùng `WITH (UPDLOCK, HOLDLOCK)` vẫn gắn được ở cả 2 trạng thái.

## 4. Bạn cần làm gì

**Cách 1 (khuyên dùng):** `git pull` rồi chạy app như thường — app tự nâng cấp CSDL.

**Cách 2 (không pull được code mới):** mở SSMS, chạy `database/05_BoSungVoucherDatSan_v3.sql`
trên CSDL `QLSanTheThao`. Xong là bản hiện tại chạy lại bình thường.

**Cách 3:** `database\ChayCSDL.bat .\SQLEXPRESS nangcap` (giữ nguyên dữ liệu).

## 5. File thay đổi

```
M  database/ChayCSDL.bat
A  src/SportFieldBooking.Data/Helpers/NangCapCSDL.cs
M  src/SportFieldBooking.Data/Repositories/DatSanRepository.cs
M  src/SportFieldBooking.WinForms/Program.cs
M  tests/SportFieldBooking.KiemThu/Program.cs
A  SUA_LOI_APP_KHONG_CHAY_17-09-2026.md
```
