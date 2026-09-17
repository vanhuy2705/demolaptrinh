using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>
/// Chi tiết hóa đơn: xem số tiền, thanh toán (tiền mặt/chuyển khoản), in hóa đơn, hủy hóa đơn.
/// Khách hàng chỉ xem được hóa đơn của mình và không có nút thu tiền.
/// </summary>
public partial class frmChiTietHoaDon : BaseForm
{
    private readonly int _maHD;
    private readonly bool _coQuyenThuTien;
    private HoaDon _hoaDon;

    public frmChiTietHoaDon(int maHD, bool coQuyenThuTien = true)
    {
        InitializeComponent();
        _maHD = maHD;
        _coQuyenThuTien = coQuyenThuTien;
    }

    protected override void TaiDuLieu()
    {
        _hoaDon = ServiceFactory.HoaDon.LayTheoMa(_maHD);
        if (_hoaDon == null)
        {
            CanhBao("Không tìm thấy hóa đơn #" + _maHD + ".");
            DialogResult = DialogResult.Abort;
            Close();
            return;
        }

        if (PhienLamViec.LaKhachHang && PhienLamViec.MaKH != null)
        {
            DatSan datSan = ServiceFactory.DatSan.LayTheoMa(_hoaDon.MaDat);
            if (datSan != null && datSan.MaKH != PhienLamViec.MaKH)
            {
                VoHieuHoa("Đây không phải hóa đơn của bạn.\nBạn chỉ được xem hóa đơn của chính mình.");
                return;
            }
        }

        HienThiThongTin();
        CapNhatTrangThaiNut();
    }

    private void HienThiThongTin()
    {
        lblMaHoaDon.Text = "#" + _hoaDon.MaHD;
        lblNgayLap.Text = _hoaDon.NgayLap.ToString("dd/MM/yyyy HH:mm");
        lblKhachHang.Text = $"{_hoaDon.TenKH}  |  {_hoaDon.SDT}";
        lblSan.Text = _hoaDon.TenSan;
        lblThoiGian.Text = $"{_hoaDon.NgayDat:dd/MM/yyyy}   {_hoaDon.GioBatDau:hh\\:mm} - {_hoaDon.GioKetThuc:hh\\:mm}";
        lblTienGoc.Text = TroGiup.Tien(_hoaDon.TienGoc);
        lblUuDai.Text = $"{LoaiGiamGia.TenHienThi(_hoaDon.LoaiGiamGia)}" +
                        (string.IsNullOrWhiteSpace(_hoaDon.MaCode) ? "" : $" ({_hoaDon.MaCode})");
        lblTienGiam.Text = "-" + TroGiup.Tien(_hoaDon.TienGiam);
        lblTongTien.Text = TroGiup.Tien(_hoaDon.TongTien);
        lblPhuongThuc.Text = PhuongThucThanhToan.TenHienThi(_hoaDon.PhuongThucThanhToan);
        lblTrangThai.Text = TrangThaiHoaDon.TenHienThi(_hoaDon.TrangThai);
        lblTrangThai.ForeColor = GiaoDien.MauTrangThai(_hoaDon.TrangThai);
        txtGhiChu.Text = _hoaDon.GhiChu;
    }

    protected override void CapNhatTrangThaiNut()
    {
        if (_hoaDon == null) return;
        bool daThanhToan = _hoaDon.TrangThai == TrangThaiHoaDon.DaThanhToan;
        bool daHuy = _hoaDon.TrangThai == TrangThaiHoaDon.DaHuy;

        btnThanhToan.Enabled = _coQuyenThuTien && !daThanhToan && !daHuy
            && PhanQuyenService.CoQuyen(MaQuyen.HdThanhToan);
        btnHuyHoaDon.Enabled = _coQuyenThuTien && !daThanhToan && !daHuy
            && PhanQuyenService.CoQuyen(MaQuyen.HdXoa);
        btnInHoaDon.Enabled = PhanQuyenService.CoQuyen(MaQuyen.HdIn);
        txtGhiChu.ReadOnly = !(_coQuyenThuTien && PhanQuyenService.CoQuyen(MaQuyen.HdSua));
    }

    private void btnThanhToan_Click(object sender, EventArgs e)
    {
        if (_hoaDon == null) return;
        if (!CoQuyen(MaQuyen.HdThanhToan)) return;

        using var xacNhan = new frmXacNhanThanhToan(_hoaDon);
        if (xacNhan.ShowDialog(this) != DialogResult.OK) return;

        if (!ThucHien(ServiceFactory.HoaDon.ThanhToan(_hoaDon.MaHD, xacNhan.PhuongThuc),
                $"Đã thanh toán hóa đơn #{_hoaDon.MaHD}.")) return;

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnInHoaDon_Click(object sender, EventArgs e)
    {
        if (_hoaDon == null) return;
        if (!CoQuyen(MaQuyen.HdIn)) return;

        ThucHien(() => InHoaDon.XemTruoc(_hoaDon, LayThongTinCuaHang()), "Không thể xem trước hóa đơn");
    }

    private static InHoaDon.ThongTinCuaHang LayThongTinCuaHang() => new()
    {
        TenTrungTam = ServiceFactory.CauHinh.LayGiaTri(ThamSoKeys.TenTrungTam, "TRUNG TÂM THỂ THAO"),
        DiaChi = ServiceFactory.CauHinh.LayGiaTri(ThamSoKeys.DiaChi, ""),
        DienThoai = ServiceFactory.CauHinh.LayGiaTri(ThamSoKeys.DienThoai, ""),
        LoiChao = ServiceFactory.CauHinh.LayGiaTri(ThamSoKeys.LoiChaoHoaDon, "Cảm ơn quý khách, hẹn gặp lại!")
    };

    private void btnHuyHoaDon_Click(object sender, EventArgs e)
    {
        if (_hoaDon == null) return;
        if (!CoQuyen(MaQuyen.HdXoa)) return;
        if (!XacNhan($"Hủy hóa đơn #{_hoaDon.MaHD}?", "Xác nhận hủy hóa đơn")) return;

        string lyDo = frmNhapLieu.NhapChuoi("Hủy hóa đơn", "Lý do hủy:", "Hủy hóa đơn", false, this);
        if (lyDo == null) return;

        if (!ThucHien(ServiceFactory.HoaDon.HuyHoaDon(_hoaDon.MaHD, lyDo))) return;

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnDong_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
