using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Thông tin cá nhân: khách hàng tự xem và cập nhật hồ sơ của mình.</summary>
public partial class frmThongTinCaNhan : BaseForm
{
    private KhachHang _khachHang;
    private ThongKeCaNhan _thongKe;
    private bool _dangSua;

    public frmThongTinCaNhan()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.KhXem;

    protected override async Task TaiDuLieuAsync()
    {
        if (PhienLamViec.MaKH == null)
        {
            VoHieuHoa("Tài khoản của bạn chưa được gắn với hồ sơ khách hàng.\nVui lòng liên hệ quầy để được hỗ trợ.");
            return;
        }

        int maKH = PhienLamViec.MaKH.Value;
        BatDauBan();
        try
        {
            var dulieu = await ChayNenAsync(() =>
            {
                KhachHang kh = ServiceFactory.KhachHang.LayTheoMa(maKH);
                return (khachHang: kh, thongKe: kh == null ? null : ServiceFactory.ThongKe.LayThongKeCaNhan(kh.MaKH));
            });

            _khachHang = dulieu.khachHang;
            _thongKe = dulieu.thongKe;
            if (_khachHang == null)
            {
                VoHieuHoa("Không tìm thấy hồ sơ khách hàng của bạn.");
                return;
            }

            HienThiThongTin();
            CapNhatTrangThaiNut();
        }
        catch (Exception ex) { BaoLoi("Không thể tải thông tin cá nhân", ex); }
        finally { KetThucBan(); }
    }

    private void HienThiThongTin()
    {
        lblMaKH.Text = _khachHang.MaKH.ToString();
        lblNgayTao.Text = _khachHang.NgayTao.ToString("dd/MM/yyyy HH:mm");
        txtHoTen.Text = _khachHang.HoTen;
        txtSoDienThoai.Text = _khachHang.SDT;
        txtEmail.Text = _khachHang.Email;
        txtDiaChi.Text = _khachHang.DiaChi;

        ThongKeCaNhan thongKe = _thongKe;
        kpiSoLanDat.DatNoiDung("Số lần đặt", thongKe.SoLanDat.ToString(), $"Hoàn thành: {thongKe.SoLanHoanThanh}");
        kpiChiTieu.DatNoiDung("Tổng chi tiêu", TroGiup.Tien(thongKe.TongChiTieu), $"Hủy: {thongKe.SoLanHuy} lần");
        kpiVoucher.DatNoiDung("Lần dùng voucher", thongKe.SoLanDungVoucher.ToString(), "");
        kpiSanYeuThich.DatNoiDung("Sân yêu thích", thongKe.SanYeuThich, "");
    }

    protected override void CapNhatTrangThaiNut()
    {
        bool coQuyenSua = _khachHang != null && PhanQuyenService.CoQuyen(MaQuyen.KhSua);
        btnSua.Enabled = coQuyenSua && !_dangSua;
        btnLuu.Enabled = coQuyenSua && _dangSua;
        btnHuy.Enabled = _dangSua;
        btnDoiMatKhau.Enabled = PhanQuyenService.CoQuyen(MaQuyen.TkDoiMatKhau);

        txtHoTen.ReadOnly = !_dangSua;
        txtSoDienThoai.ReadOnly = !_dangSua;
        txtEmail.ReadOnly = !_dangSua;
        txtDiaChi.ReadOnly = !_dangSua;
    }

    private void btnSua_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.KhSua)) return;
        _dangSua = true;
        txtHoTen.Select();
        CapNhatTrangThaiNut();
    }

    private async void btnLuu_Click(object sender, EventArgs e)
    {
        if (_khachHang == null || !CoQuyen(MaQuyen.KhSua)) return;

        errLoi.Clear();
        bool loi = TroGiup.Rong(txtHoTen, "họ tên", errLoi);
        loi |= TroGiup.SaiDienThoai(txtSoDienThoai, errLoi);
        loi |= TroGiup.SaiEmail(txtEmail, errLoi);
        if (loi) return;

        _khachHang.HoTen = txtHoTen.Text.Trim();
        _khachHang.SDT = txtSoDienThoai.Text.Trim();
        _khachHang.Email = txtEmail.Text.Trim();
        _khachHang.DiaChi = txtDiaChi.Text.Trim();

        if (!await ThucHienAsync(() => ServiceFactory.KhachHang.CapNhat(_khachHang, laTuChinhSua: true))) return;

        _dangSua = false;
        _ = TaiDuLieuAsync();
    }

    private void btnHuy_Click(object sender, EventArgs e)
    {
        _dangSua = false;
        errLoi.Clear();
        if (_khachHang != null) HienThiThongTin();
        CapNhatTrangThaiNut();
    }

    private void btnDoiMatKhau_Click(object sender, EventArgs e)
    {
        using var doiMatKhau = new frmDoiMatKhau();
        doiMatKhau.ShowDialog(this);
    }
}
