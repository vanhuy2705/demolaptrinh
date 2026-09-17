using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>
/// Chi tiết một booking: xem/sửa thông tin, tính lại tiền, hủy booking.
/// Khách hàng chỉ được mở booking của chính mình.
/// </summary>
public partial class frmChiTietDatSan : BaseForm
{
    private readonly int _maDat;
    private readonly bool _coQuyenQuanLy;
    private DatSan _datSan;

    public frmChiTietDatSan(int maDat, bool coQuyenQuanLy = false)
    {
        InitializeComponent();
        _maDat = maDat;
        _coQuyenQuanLy = coQuyenQuanLy;
    }

    protected override void ThietLapGiaoDien()
    {
        dtpGioBatDau.Format = DateTimePickerFormat.Custom;
        dtpGioBatDau.CustomFormat = "HH:mm";
        dtpGioBatDau.ShowUpDown = true;
        dtpGioKetThuc.Format = DateTimePickerFormat.Custom;
        dtpGioKetThuc.CustomFormat = "HH:mm";
        dtpGioKetThuc.ShowUpDown = true;
    }

    protected override async Task TaiDuLieuAsync()
    {
        BatDauBan();
        try
        {
            _datSan = await ChayNenAsync(() => ServiceFactory.DatSan.LayTheoMa(_maDat));
        }
        catch (Exception ex) { BaoLoi("Không thể tải chi tiết booking", ex); KetThucBan(); return; }
        KetThucBan();

        if (_datSan == null)
        {
            CanhBao("Không tìm thấy booking #" + _maDat + ".");
            DialogResult = DialogResult.Abort;
            Close();
            return;
        }

        if (PhienLamViec.LaKhachHang && PhienLamViec.MaKH != _datSan.MaKH)
        {
            VoHieuHoa("Đây không phải booking của bạn.\nBạn chỉ được xem lịch đặt của chính mình.");
            return;
        }

        HienThiThongTin();
        CapNhatTrangThaiNut();
    }

    private void HienThiThongTin()
    {
        lblMaDat.Text = "#" + _datSan.MaDat;
        lblKhachHang.Text = $"{_datSan.TenKH}  |  {_datSan.SDT}";
        lblSan.Text = $"{_datSan.TenSan} ({_datSan.TenLoaiSan})";
        lblTrangThai.Text = TrangThaiDatSan.TenHienThi(_datSan.TrangThai);
        lblTrangThai.ForeColor = GiaoDien.MauTrangThai(_datSan.TrangThai);

        dtpNgayDat.Value = _datSan.NgayDat;
        TroGiup.DatGio(dtpGioBatDau, _datSan.GioBatDau);
        TroGiup.DatGio(dtpGioKetThuc, _datSan.GioKetThuc);
        txtGhiChu.Text = _datSan.GhiChu;

        lblDonGia.Text = TroGiup.Tien(_datSan.DonGia) + "/giờ";
        lblTienSan.Text = TroGiup.Tien(_datSan.TienSan);
    }

    protected override void CapNhatTrangThaiNut()
    {
        if (_datSan == null) return;
        bool daKhoa = _datSan.TrangThai == TrangThaiDatSan.DaHuy || _datSan.TrangThai == TrangThaiDatSan.HoanThanh;

        btnLuu.Enabled = _coQuyenQuanLy && !daKhoa && PhanQuyenService.CoQuyen(MaQuyen.DatSanSua);
        btnHuyBooking.Enabled = !daKhoa && (_coQuyenQuanLy
            ? PhanQuyenService.CoQuyen(MaQuyen.DatSanHuy)
            : PhanQuyenService.CoQuyen(MaQuyen.DatSanHuy));

        dtpNgayDat.Enabled = btnLuu.Enabled;
        dtpGioBatDau.Enabled = btnLuu.Enabled;
        dtpGioKetThuc.Enabled = btnLuu.Enabled;
        txtGhiChu.ReadOnly = !btnLuu.Enabled;
    }

    private async void btnLuu_Click(object sender, EventArgs e)
    {
        if (_datSan == null) return;
        if (!CoQuyen(MaQuyen.DatSanSua)) return;

        _datSan.NgayDat = dtpNgayDat.Value.Date;
        _datSan.GioBatDau = TroGiup.LayGio(dtpGioBatDau);
        _datSan.GioKetThuc = TroGiup.LayGio(dtpGioKetThuc);
        _datSan.GhiChu = txtGhiChu.Text.Trim();

        if (!await ThucHienAsync(() => ServiceFactory.DatSan.CapNhatDatSan(_datSan))) return;

        DialogResult = DialogResult.OK;
        Close();
    }

    private async void btnHuyBooking_Click(object sender, EventArgs e)
    {
        if (_datSan == null) return;
        if (!CoQuyen(MaQuyen.DatSanHuy)) return;

        string lyDo = frmNhapLieu.NhapChuoi("Hủy booking #" + _datSan.MaDat,
            "Lý do hủy (không bắt buộc):", "Khách hủy", false, this, batBuocNhap: false);
        if (lyDo == null) return;

        if (!await ThucHienAsync(() => ServiceFactory.DatSan.HuyDatSan(_datSan.MaDat, lyDo))) return;

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnDong_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void ThayDoiThoiGian(object sender, EventArgs e) => CapNhatTienTamTinh();

    private async void CapNhatTienTamTinh()
    {
        if (_datSan == null) return;

        int maSan = _datSan.MaSan;
        DateTime ngay = dtpNgayDat.Value.Date;
        TimeSpan gioBatDau = TroGiup.LayGio(dtpGioBatDau), gioKetThuc = TroGiup.LayGio(dtpGioKetThuc);

        KetQua<ChiTietTien> ketQua = await ChayNenAsync(() =>
            ServiceFactory.TinhTien.TinhTien(maSan, ngay, gioBatDau, gioKetThuc, ""));
        if (IsDisposed) return;

        lblTienTamTinh.Text = ketQua.ThanhCong
            ? TroGiup.Tien(ketQua.DuLieu.TienGoc) + $" ({ketQua.DuLieu.SoGio:0.##} giờ)"
            : ketQua.ThongBao;
    }
}
