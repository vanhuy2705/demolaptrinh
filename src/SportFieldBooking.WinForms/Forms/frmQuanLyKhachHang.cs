using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Quản lý khách hàng: thêm/sửa/xóa, tìm theo tên hoặc số điện thoại (SDT duy nhất).</summary>
public partial class frmQuanLyKhachHang : BaseForm
{
    private enum CheDo { Xem, Them, Sua }
    private CheDo _cheDo = CheDo.Xem;

    public frmQuanLyKhachHang()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.KhXemTatCa;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvKhachHang);
        Luoi.DatTieuDe(dgvKhachHang,
            ("MaKH", "Mã"),
            ("HoTen", "Họ tên"),
            ("SDT", "Số điện thoại"),
            ("Email", "Email"),
            ("DiaChi", "Địa chỉ"),
            ("TenDangNhap", "Tài khoản"),
            ("NgayTao", "Ngày tạo"));
        Luoi.DatDoRong(dgvKhachHang, "MaKH", 60);
        Luoi.DatDoRong(dgvKhachHang, "SDT", 120);
        Luoi.DatDoRong(dgvKhachHang, "TenDangNhap", 110);
        Luoi.DatDinhDangNgay(dgvKhachHang, "dd/MM/yyyy", "NgayTao");
    }

    protected override void TaiDuLieu() => TimKiem();

    protected override void CapNhatTrangThaiNut()
    {
        bool dangSua = _cheDo != CheDo.Xem;
        KhachHang dangChon = Luoi.LayDongDangChon<KhachHang>(dgvKhachHang);

        btnThem.Enabled = !dangSua && PhanQuyenService.CoQuyen(MaQuyen.KhThem);
        btnSua.Enabled = !dangSua && dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.KhSua);
        btnXoa.Enabled = !dangSua && dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.KhXoa);
        btnLuu.Enabled = dangSua;
        btnHuy.Enabled = dangSua;
        btnLamMoi.Enabled = !dangSua;

        txtHoTen.ReadOnly = !dangSua;
        txtSoDienThoai.ReadOnly = !dangSua;
        txtEmail.ReadOnly = !dangSua;
        txtDiaChi.ReadOnly = !dangSua;
        dgvKhachHang.Enabled = !dangSua;
    }

    private void TimKiem()
    {
        ThucHien(() =>
        {
            Luoi.GanDuLieu(dgvKhachHang, ServiceFactory.KhachHang.LayTatCa(txtTimKiem.Text.Trim()));
            HienThiChiTiet();
            CapNhatTrangThaiNut();
        }, "Không thể tải danh sách khách hàng");
    }

    private void HienThiChiTiet()
    {
        KhachHang dangChon = Luoi.LayDongDangChon<KhachHang>(dgvKhachHang);
        if (dangChon == null)
        {
            lblMaKH.Text = "—";
            txtHoTen.Clear();
            txtSoDienThoai.Clear();
            txtEmail.Clear();
            txtDiaChi.Clear();
            return;
        }
        lblMaKH.Text = dangChon.MaKH.ToString();
        txtHoTen.Text = dangChon.HoTen;
        txtSoDienThoai.Text = dangChon.SDT;
        txtEmail.Text = dangChon.Email;
        txtDiaChi.Text = dangChon.DiaChi;
    }

    private void btnThem_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.KhThem)) return;
        _cheDo = CheDo.Them;
        lblMaKH.Text = "Mới";
        txtHoTen.Clear();
        txtSoDienThoai.Clear();
        txtEmail.Clear();
        txtDiaChi.Clear();
        txtHoTen.Select();
        CapNhatTrangThaiNut();
    }

    private void btnSua_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.KhSua)) return;
        if (Luoi.LayDongDangChon<KhachHang>(dgvKhachHang) == null)
        {
            CanhBao("Vui lòng chọn một khách hàng trong danh sách.", "Chưa chọn dữ liệu");
            return;
        }
        _cheDo = CheDo.Sua;
        txtHoTen.Select();
        CapNhatTrangThaiNut();
    }

    private void btnXoa_Click(object sender, EventArgs e)
    {
        KhachHang dangChon = Luoi.LayDongDangChon<KhachHang>(dgvKhachHang);
        if (dangChon == null || !CoQuyen(MaQuyen.KhXoa)) return;
        if (!XacNhan($"Xóa khách hàng \"{dangChon.HoTen}\"?", "Xác nhận xóa")) return;

        ThucHien(ServiceFactory.KhachHang.Xoa(dangChon.MaKH));
        _cheDo = CheDo.Xem;
        TimKiem();
    }

    private void btnLuu_Click(object sender, EventArgs e)
    {
        if (!HopLe()) return;

        var khachHang = new KhachHang
        {
            HoTen = txtHoTen.Text.Trim(),
            SDT = txtSoDienThoai.Text.Trim(),
            Email = txtEmail.Text.Trim(),
            DiaChi = txtDiaChi.Text.Trim()
        };

        bool thanhCong;
        if (_cheDo == CheDo.Them)
        {
            thanhCong = ThucHien(ServiceFactory.KhachHang.Them(khachHang));
        }
        else
        {
            KhachHang dangChon = Luoi.LayDongDangChon<KhachHang>(dgvKhachHang);
            if (dangChon == null) return;
            khachHang.MaKH = dangChon.MaKH;
            thanhCong = ThucHien(ServiceFactory.KhachHang.CapNhat(khachHang));
        }

        if (!thanhCong) return;
        _cheDo = CheDo.Xem;
        TimKiem();
    }

    private bool HopLe()
    {
        errLoi.Clear();
        bool loi = TroGiup.Rong(txtHoTen, "họ tên", errLoi);
        loi |= TroGiup.SaiDienThoai(txtSoDienThoai, errLoi);
        loi |= TroGiup.SaiEmail(txtEmail, errLoi);
        return !loi;
    }

    private void btnHuy_Click(object sender, EventArgs e)
    {
        _cheDo = CheDo.Xem;
        errLoi.Clear();
        HienThiChiTiet();
        CapNhatTrangThaiNut();
    }

    private void btnLamMoi_Click(object sender, EventArgs e)
    {
        txtTimKiem.Clear();
        _cheDo = CheDo.Xem;
        TimKiem();
    }

    private void btnTim_Click(object sender, EventArgs e) => TimKiem();

    private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) TimKiem();
    }

    private void dgvKhachHang_SelectionChanged(object sender, EventArgs e)
    {
        if (_cheDo != CheDo.Xem) return;
        HienThiChiTiet();
        CapNhatTrangThaiNut();
    }
}
