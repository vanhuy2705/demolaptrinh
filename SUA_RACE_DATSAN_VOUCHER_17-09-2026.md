# Sửa 2 lỗi nghiệp vụ nặng: đặt trùng sân đồng thời & voucher bị mất — 17/09/2026

Hai lỗi này không nhìn thấy bằng mắt khi dùng một mình, nhưng ảnh hưởng trực tiếp
đến **tiền và dữ liệu thật** khi có nhiều người dùng cùng lúc.

---

## LỖI A — Hai người đặt cùng sân, cùng giờ: cả hai đều thành công

### Nguyên nhân
`DatSanService.TaoDatSan()` làm 2 bước rời nhau, không có giao dịch:

```csharp
List<DatSan> trung = LayTrungLich(maSan, ngayDat, gioBatDau, gioKetThuc);   // 1) ĐỌC
if (trung.Count > 0) return KetQua<DatSan>.Loi(...);
...
datSan.MaDat = _datSanRepo.Them(datSan);                                    // 2) GHI
```

Giữa bước 1 và bước 2 không có gì ngăn người khác ghi. Hai lễ tân (hoặc khách
đặt trên web cùng nhân viên đặt tại quầy) bấm cùng lúc → cả hai đều đọc thấy
lịch trống → cả hai đều ghi → **sân bị đặt chồng 2 booking**.

Tầng CSDL cũng không chặn: `DAT_SAN` chỉ có `CHECK (GioKetThuc > GioBatDau)`;
`sp_KiemTraTrungLich` có trong SQL nhưng **C# chưa gọi lần nào**. Đây là lỗi
kiểu TOCTOU (time-of-check to time-of-use) kinh điển.

### Cách sửa
1. `IDatSanRepository.LayTrungLich(...)` thêm tham số `bool khoaBang = false`.
2. `DatSanRepository`: khi `khoaBang = true`, câu SELECT dùng
   `FROM DAT_SAN ds WITH (UPDLOCK, HOLDLOCK)` → khoá dải bản ghi của sân/ngày đó
   tới khi giao dịch kết thúc, nên yêu cầu thứ hai phải **xếp hàng chờ**.
3. `DatSanService.TaoDatSan()` và `CapNhatDatSan()`: đưa bước kiểm tra trùng +
   bước ghi vào **cùng một giao dịch** (`DbHelper.ChayGiaoDich`), và bước kiểm
   tra bên trong giao dịch luôn dùng bản đọc có khoá:

```csharp
KetQua<DatSan> ketQua = null;
Data.Helpers.DbHelper.ChayGiaoDich(() =>
{
    List<DatSan> trungKhoa = _datSanRepo.LayTrungLich(maSan, ngayDat.Date,
        gioBatDau, gioKetThuc, null, khoaBang: true);
    if (trungKhoa.Count > 0)
    {
        ketQua = KetQua<DatSan>.Loi(TaoThongBaoTrungLich(trungKhoa));
        return;                 // không ghi gì; giao dịch commit rỗng, khoá được nhả
    }
    datSan.MaDat = _datSanRepo.Them(datSan);
    ketQua = KetQua<DatSan>.Tot(datSan, $"Đặt sân thành công (mã #{datSan.MaDat}).");
});
return ketQua ?? KetQua<DatSan>.Loi("Không thể đặt sân.");
```

Bước kiểm tra không khoá ở đầu hàm vẫn giữ nguyên để báo lỗi nhanh cho người dùng
trong phần lớn trường hợp; bản đọc có khoá mới là chốt chặn cuối cùng.

`CapNhatDatSan` cũng được bọc y vậy (truyền `maDatLoaiTru` để không tự coi mình
là trùng) — sửa giờ/sân cũng có thể gây trùng như đặt mới.

---

## LỖI B — Voucher chọn lúc đặt sân bị vứt bỏ

### Nguyên nhân
- `DAT_SAN` **không có cột lưu voucher**.
- `TaoDatSan` tính tiền có voucher nhưng chỉ lưu giá gốc:
  `datSan.TienSan = ketQuaTien.DuLieu.TienGoc;`
- Trong khi đó màn hình đặt sân hiển thị `lblTongTien` = **giá đã giảm**
  (`frmDatSanKhachHang.cs:122`) → *số khách nhìn thấy khác số hệ thống lưu*.
- Voucher chỉ được ghi nhận ở `HoaDonService.ThanhToan()`
  (`TangSoLuongDaDung`, dòng 157) → tới bước lập hóa đơn, `LapHoaDon(maDat, maVoucher)`
  nhận `maVoucher` từ ô nhập trên form, mà form đã `txtMaVoucher.Clear()` sau khi đặt
  → **khách mất ưu đãi, nhân viên phải hỏi lại mã**.

### Cách sửa
1. **CSDL**: thêm cột `DAT_SAN.MaVoucher INT NULL` + khoá ngoại `FK_DAT_SAN_VOUCHER`
   + index `IX_DAT_SAN_Voucher`.
   * Cài mới: đã thêm sẵn vào `01_TaoCSDL_v2.sql`.
   * CSDL đang chạy: chạy `database/05_BoSungVoucherDatSan_v3.sql` (idempotent,
     có hồi tố dữ liệu cũ từ `HOA_DON` và `SU_DUNG_VOUCHER`).
   * Khoá ngoại đặt sau khối tạo bảng `VOUCHER` vì `DAT_SAN` được tạo trước.
2. **Entity** `DatSan`: thêm `MaVoucher` (int?) và `MaVoucherCode` (join để hiển thị).
3. **Repository**: `SqlSelect` + `LaySapDienRa` có `LEFT JOIN VOUCHER`, `AnhXa`,
   `Them`, `CapNhat` đều đọc/ghi `MaVoucher`.
4. **`DatSanService.TaoDatSan`**: lưu voucher đã kiểm tra hợp lệ vào booking.
5. **`DatSanService.CapNhatDatSan`**: ưu tiên mã vừa nhập; **không nhập thì giữ
   voucher đang lưu** — màn hình chi tiết booking không có ô nhập voucher nên
   không được làm khách mất ưu đãi.
6. **`HoaDonService.LapHoaDon`**: người lập không nhập mã thì tự lấy mã voucher
   của booking. Nếu voucher đó đã hết hạn/hết lượt vào lúc thu tiền thì **lập hóa
   đơn theo giá hiện hành chứ không chặn nghiệp vụ thu tiền** (có thử lại lần 2
   không voucher).

Vẫn giữ đúng quy tắc cũ: **voucher chỉ bị trừ lượt khi THANH TOÁN**
(`SU_DUNG_VOUCHER` + `TangSoLuongDaDung` trong giao dịch), nên hủy booking không
cần hoàn lượt.

---

## Thay đổi kèm theo

### `DbHelper.GiaoDichWrapper` — điểm nối cho kiểm thử
Bộ kiểm thử chạy bằng kho dữ liệu giả trong bộ nhớ, không có SQL Server, nên
không mở được giao dịch thật. Thêm 2 thuộc tính tĩnh (mặc định `null`):

```csharp
public static Action<Action> GiaoDichWrapper { get; set; }
public static Func<Func<Task>, Task> GiaoDichWrapperAsync { get; set; }
```

`ChayGiaoDich`/`ChayGiaoDichAsync` dùng wrapper nếu được gán. **Ứng dụng thật
không gán** → hành vi không đổi. Nhờ vậy kiểm thử chạy được các nghiệp vụ có
giao dịch (đặt sân, lập hóa đơn) mà trước đây không test nổi.

### `ChayCSDL.bat`
Trước vẫn chạy `01_TaoCSDL.sql` + `02_DuLieuMau.sql` (bản CŨ) dù repo đã có bản v2.
Đã đổi sang `01_TaoCSDL_v2.sql` + `02_DuLieuMau_v2.sql` và in hướng dẫn chạy
`04_…` + `05_…` cho CSDL đang có dữ liệu.

---

## Kiểm chứng

Bộ kiểm thử nghiệp vụ tăng từ **42 → 56** (thêm nhóm I, 14 kiểm tra mới):

```
── I. Đặt sân: voucher đi theo booking & chống đặt trùng đồng thời
  [ĐẠT] Đặt sân 20:00-21:00 kèm GIAM20 => thành công
  [ĐẠT] Booking LƯU voucher (trước đây bị mất)
  [ĐẠT] Booking lưu giá gốc 400.000 đ, giảm giá tính ở hóa đơn
  [ĐẠT] Bước kiểm tra trùng trước khi ghi có ĐỌC KHOÁ (chống race)
  [ĐẠT] Đặt chồng 20:30-21:30 cùng sân => bị từ chối
  [ĐẠT] Không sinh booking mới khi trùng lịch
  [ĐẠT] Lập hóa đơn KHÔNG cần nhập lại voucher => thành công
  [ĐẠT] Hóa đơn tự dùng voucher của booking: 400.000 - 20% = 320.000 đ
  [ĐẠT] Hóa đơn ghi đúng MaVoucher của booking
  [ĐẠT] Sửa booking (không nhập voucher) => thành công
  [ĐẠT] Sửa booking vẫn GIỮ voucher cũ, khách không mất ưu đãi
  [ĐẠT] Đặt sân 22:00-23:00 không voucher => thành công
  [ĐẠT] Voucher của booking đã hết lượt => vẫn lập được hóa đơn (không chặn thu tiền)
  [ĐẠT] ...và tính theo giá hiện hành 400.000 đ, không giảm

  KẾT QUẢ: 56 đạt / 0 lỗi / 56 tổng số
```

Kiểm tra "có ĐỌC KHOÁ" khẳng định được nhờ `KhoDatSanGia.SoLanDocKhoa` đếm số lần
nghiệp vụ yêu cầu `khoaBang: true` — nếu sau này ai bỏ khoá trong `DatSanService`
thì kiểm thử đỏ ngay.

* `dotnet build SportFieldBooking.sln --no-incremental` → **0 error / 0 warning**

> Lưu ý: race condition thật chỉ xảy ra trên SQL Server, nên phần khoá
> (UPDLOCK/HOLDLOCK) được kiểm chứng bằng review + kiểm thử khẳng định đường code
> có đi qua đọc khoá, chứ không mô phỏng được 2 luồng song song trong bộ kiểm thử.

## Cách áp dụng lên máy đang chạy

```bat
:: CSDL đang có dữ liệu:
sqlcmd -S .\SQLEXPRESS -E -C -i database\05_BoSungVoucherDatSan_v3.sql

:: rồi build lại ứng dụng
dotnet build SportFieldBooking.sln
```

Booking cũ không có voucher vẫn chạy bình thường (cột cho phép NULL).
