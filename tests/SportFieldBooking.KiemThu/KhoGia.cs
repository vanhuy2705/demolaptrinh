using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.KiemThu;

/// <summary>
/// Các repository giả lưu dữ liệu trong bộ nhớ.
/// Mục đích: kiểm thử toàn bộ quy tắc nghiệp vụ ở tầng Business
/// (tính tiền, block thời gian, ưu tiên giảm giá, trùng lịch, phân quyền)
/// mà KHÔNG cần SQL Server. Các phương thức không dùng đến sẽ ném NotSupportedException.
/// </summary>
public class KhoSanGia : ISanRepository
{
    public List<San> DuLieu { get; } = new();

    public List<San> LayTatCa(string tuKhoa = "", int? maLoaiSan = null, string trangThai = null) => DuLieu;
    public San LayTheoMa(int maSan) => DuLieu.FirstOrDefault(s => s.MaSan == maSan);
    public bool TonTaiTen(string tenSan, int? maSanLoaiTru = null) =>
        DuLieu.Any(s => s.TenSan == tenSan && (maSanLoaiTru == null || s.MaSan != maSanLoaiTru));
    public int Them(San san) { san.MaSan = DuLieu.Count == 0 ? 1 : DuLieu.Max(s => s.MaSan) + 1; DuLieu.Add(san); return san.MaSan; }
    public int CapNhat(San san) => 1;
    public int CapNhatTrangThai(int maSan, string trangThai)
    {
        San san = LayTheoMa(maSan);
        if (san != null) san.TrangThai = trangThai;
        return 1;
    }
    public int Xoa(int maSan) => DuLieu.RemoveAll(s => s.MaSan == maSan);
    public int DemDatSan(int maSan) => 0;
}

public class KhoThamSoGia : IThamSoRepository
{
    public List<ThamSo> DuLieu { get; } = new();

    public List<ThamSo> LayTatCa() => DuLieu;
    public ThamSo LayTheoTen(string tenThamSo) => DuLieu.FirstOrDefault(t => t.TenThamSo == tenThamSo);
    public string GiaTri(string tenThamSo, string macDinh = "") => LayTheoTen(tenThamSo)?.GiaTri ?? macDinh;
    public int CapNhat(string tenThamSo, string giaTri)
    {
        ThamSo ts = LayTheoTen(tenThamSo);
        if (ts != null) ts.GiaTri = giaTri;
        return 1;
    }
    public int Them(ThamSo thamSo) { DuLieu.Add(thamSo); return 1; }
}

public class KhoVoucherGia : IVoucherRepository
{
    public List<Voucher> DuLieu { get; } = new();

    public List<Voucher> LayTatCa(string tuKhoa = "", bool? chiConHan = null) => DuLieu;
    public Voucher LayTheoMa(int maVoucher) => DuLieu.FirstOrDefault(v => v.MaVoucher == maVoucher);
    public Voucher LayTheoMaCode(string maCode) =>
        DuLieu.FirstOrDefault(v => string.Equals(v.MaCode, maCode?.Trim(), StringComparison.OrdinalIgnoreCase));
    public bool TonTaiMaCode(string maCode, int? maVoucherLoaiTru = null) => LayTheoMaCode(maCode) != null;
    public int Them(Voucher voucher) { voucher.MaVoucher = DuLieu.Count + 1; DuLieu.Add(voucher); return voucher.MaVoucher; }
    public int CapNhat(Voucher voucher) => 1;
    public int Xoa(int maVoucher) => DuLieu.RemoveAll(v => v.MaVoucher == maVoucher);
    public int TangSoLuongDaDung(int maVoucher, int soLuong = 1)
    {
        Voucher v = LayTheoMa(maVoucher);
        if (v != null) v.SoLuongDaDung += soLuong;
        return 1;
    }
    public int GiamSoLuongDaDung(int maVoucher, int soLuong = 1)
    {
        Voucher v = LayTheoMa(maVoucher);
        if (v != null) v.SoLuongDaDung = Math.Max(0, v.SoLuongDaDung - soLuong);
        return 1;
    }
}

public class KhoSuDungVoucherGia : ISuDungVoucherRepository
{
    public List<SuDungVoucher> DuLieu { get; } = new();

    public int Them(SuDungVoucher suDung) { DuLieu.Add(suDung); return 1; }
    public List<SuDungVoucher> LayTheoKhachHang(int maKH) => DuLieu.Where(d => d.MaKH == maKH).ToList();
    public List<SuDungVoucher> LayTheoMaDat(int maDat) => DuLieu.Where(d => d.MaDat == maDat).ToList();
    public bool DaDungChoBooking(int maVoucher, int maDat) => DuLieu.Any(d => d.MaVoucher == maVoucher && d.MaDat == maDat);
    public int XoaTheoMaDat(int maDat) => DuLieu.RemoveAll(d => d.MaDat == maDat);
}

public class KhoKhuyenMaiGia : IKhuyenMaiRepository
{
    public List<KhuyenMai> DuLieu { get; } = new();

    public List<KhuyenMai> LayTatCa(string tuKhoa = "") => DuLieu;
    public KhuyenMai LayTheoMa(int maKM) => DuLieu.FirstOrDefault(k => k.MaKM == maKM);

    /// <summary>Mirror đúng điều kiện SQL: còn hiệu lực, đúng trạng thái, và (không áp dụng cuối tuần HOẶC khớp cờ cuối tuần).</summary>
    public List<KhuyenMai> LayDangApDung(DateTime ngay, bool laCuoiTuan) => DuLieu
        .Where(k => k.TrangThai == TrangThaiVoucher.HoatDong
                    && ngay.Date >= k.NgayBatDau.Date && ngay.Date <= k.NgayKetThuc.Date
                    && (!k.ApDungCuoiTuan || k.ApDungCuoiTuan == laCuoiTuan))
        .OrderByDescending(k => k.PhanTramGiam)
        .ToList();

    public bool TonTaiTen(string tenKM, int? maKMLoaiTru = null) =>
        DuLieu.Any(k => k.TenKM == tenKM && (maKMLoaiTru == null || k.MaKM != maKMLoaiTru));
    public int Them(KhuyenMai khuyenMai) { khuyenMai.MaKM = DuLieu.Count + 1; DuLieu.Add(khuyenMai); return khuyenMai.MaKM; }
    public int CapNhat(KhuyenMai khuyenMai) => 1;
    public int Xoa(int maKM) => DuLieu.RemoveAll(k => k.MaKM == maKM);
}

public class KhoDatSanGia : IDatSanRepository
{
    public List<DatSan> DuLieu { get; } = new();

    public List<DatSan> LayTatCa(string tuKhoa = "") => DuLieu;
    public List<DatSan> LayTheoNgay(DateTime ngay, int? maSan = null) => DuLieu;
    public List<DatSan> LayTheoKhoang(DateTime tuNgay, DateTime denNgay, int? maSan = null, string trangThai = null) => DuLieu;
    public List<DatSan> LayTheoKhachHang(int maKH) => DuLieu.Where(d => d.MaKH == maKH).ToList();
    public List<DatSan> LaySapDienRa(int soLuong) => DuLieu.Take(soLuong).ToList();
    public DatSan LayTheoMa(int maDat) => DuLieu.FirstOrDefault(d => d.MaDat == maDat);

    /// <summary>Trùng lịch = cùng sân, cùng ngày, giao nhau về khoảng giờ, bỏ qua booking đã hủy.</summary>
    public List<DatSan> LayTrungLich(int maSan, DateTime ngay, TimeSpan gioBatDau, TimeSpan gioKetThuc, int? maDatLoaiTru = null) =>
        DuLieu.Where(d => d.MaSan == maSan
                          && d.NgayDat.Date == ngay.Date
                          && d.TrangThai != TrangThaiDatSan.DaHuy
                          && d.GioBatDau < gioKetThuc
                          && d.GioKetThuc > gioBatDau
                          && (maDatLoaiTru == null || d.MaDat != maDatLoaiTru))
              .ToList();

    public int Them(DatSan datSan) { datSan.MaDat = DuLieu.Count + 1; DuLieu.Add(datSan); return datSan.MaDat; }
    public int CapNhat(DatSan datSan) => 1;
    public int CapNhatTrangThai(int maDat, string trangThai)
    {
        DatSan d = LayTheoMa(maDat);
        if (d != null) d.TrangThai = trangThai;
        return 1;
    }
    public int Xoa(int maDat) => DuLieu.RemoveAll(d => d.MaDat == maDat);
    public int DemTheoTrangThai(string trangThai, DateTime? ngay = null) => DuLieu.Count(d => d.TrangThai == trangThai);
}

public class KhoKhachHangGia : IKhachHangRepository
{
    public List<KhachHang> DuLieu { get; } = new();

    public List<KhachHang> LayTatCa(string tuKhoa = "") => DuLieu;
    public KhachHang LayTheoMa(int maKH) => DuLieu.FirstOrDefault(k => k.MaKH == maKH);
    public KhachHang LayTheoSDT(string sdt) => DuLieu.FirstOrDefault(k => k.SDT == sdt);
    public KhachHang LayTheoMaTK(int maTK) => DuLieu.FirstOrDefault(k => k.MaTK == maTK);
    public bool TonTaiSDT(string sdt, int? maKHLoaiTru = null) =>
        DuLieu.Any(k => k.SDT == sdt && (maKHLoaiTru == null || k.MaKH != maKHLoaiTru));
    public int Them(KhachHang khachHang) { khachHang.MaKH = DuLieu.Count + 1; DuLieu.Add(khachHang); return khachHang.MaKH; }
    public int CapNhat(KhachHang khachHang) => 1;
    public int Xoa(int maKH) => DuLieu.RemoveAll(k => k.MaKH == maKH);
    public int GanTaiKhoan(int maKH, int maTK)
    {
        KhachHang k = LayTheoMa(maKH);
        if (k != null) k.MaTK = maTK;
        return 1;
    }
}
