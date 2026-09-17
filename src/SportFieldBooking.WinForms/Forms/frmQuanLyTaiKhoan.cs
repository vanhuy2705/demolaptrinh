using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Quản lý tài khoản: CRUD, khóa/mở khóa, đổi mật khẩu, phân quyền (chỉ Admin).</summary>
public partial class frmQuanLyTaiKhoan : BaseForm
{
    private enum CheDo { Xem, Them, Sua }
    private CheDo _cheDo = CheDo.Xem;

    public frmQuanLyTaiKhoan()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.TkXem;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvTaiKhoan);
        Luoi.DatTieuDe(dgvTaiKhoan,
            ("MaTK", "Mã"),
            ("TenDangNhap", "Tên đăng nhập"),
            ("HoTen", "Họ tên"),
            ("VaiTro", "Vai trò"),
            ("TrangThai", "Trạng thái"),
            ("NgayTao", "Ngày tạo"));
        Luoi.DatDoRong(dgvTaiKhoan, "MaTK", 60);
        Luoi.DatDoRong(dgvTaiKhoan, "VaiTro", 110);
        Luoi.DatDoRong(dgvTaiKhoan, "TrangThai", 110);
        Luoi.DatDinhDangNgay(dgvTaiKhoan, "dd/MM/yyyy HH:mm", "NgayTao");
        Luoi.HienThiTrangThai(dgvTaiKhoan, "VaiTro", VaiTro.TenHienThi);
        Luoi.HienThiTrangThai(dgvTaiKhoan, "TrangThai", TrangThaiTaiKhoan.TenHienThi);
        Luoi.ToMauTrangThai(dgvTaiKhoan, "TrangThai");

        cboVaiTro.Items.Clear();
        cboVaiTro.Items.AddRange(new object[] { "Quản trị viên", "Nhân viên", "Khách hàng" });
        cboVaiTro.SelectedIndex = 0;

        cboTrangThai.Items.Clear();
        cboTrangThai.Items.AddRange(new object[] { "Hoạt động", "Bị khóa" });
        cboTrangThai.SelectedIndex = 0;

        cboLocVaiTro.Items.Clear();
        cboLocVaiTro.Items.AddRange(new object[] { "Tất cả", "Quản trị viên", "Nhân viên", "Khách hàng" });
        cboLocVaiTro.SelectedIndex = 0;
    }

    private static string LayMaVaiTro(string tenHienThi) => tenHienThi switch
    {
        "Quản trị viên" => VaiTro.Admin,
        "Nhân viên" => VaiTro.NhanVien,
        "Khách hàng" => VaiTro.KhachHang,
        _ => null
    };

    private static string LayMaTrangThai(string tenHienThi) =>
        tenHienThi == "Bị khóa" ? TrangThaiTaiKhoan.BiKhoa : TrangThaiTaiKhoan.HoatDong;

    protected override Task TaiDuLieuAsync() => TimKiemAsync();

    protected override void CapNhatTrangThaiNut()
    {
        bool dangSua = _cheDo != CheDo.Xem;
        TaiKhoan dangChon = Luoi.LayDongDangChon<TaiKhoan>(dgvTaiKhoan);
        bool chinhMinh = dangChon != null && dangChon.MaTK == PhienLamViec.MaTK;

        btnThem.Enabled = !dangSua && PhanQuyenService.CoQuyen(MaQuyen.TkThem);
        btnSua.Enabled = !dangSua && dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.TkSua);
        btnXoa.Enabled = !dangSua && dangChon != null && !chinhMinh && PhanQuyenService.CoQuyen(MaQuyen.TkXoa);
        btnKhoaMo.Enabled = !dangSua && dangChon != null && !chinhMinh && PhanQuyenService.CoQuyen(MaQuyen.TkKhoa);
        btnDatLaiMatKhau.Enabled = !dangSua && dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.TkSua);
        btnLuu.Enabled = dangSua;
        btnHuy.Enabled = dangSua;
        btnLamMoi.Enabled = !dangSua;

        txtTenDangNhap.ReadOnly = !dangSua;
        txtMatKhau.ReadOnly = !dangSua || _cheDo == CheDo.Sua;
        txtHoTen.ReadOnly = !dangSua;
        cboVaiTro.Enabled = dangSua;
        cboTrangThai.Enabled = dangSua;
        dgvTaiKhoan.Enabled = !dangSua;

        lblGhiChuMatKhau.Visible = _cheDo == CheDo.Sua;
        btnKhoaMo.Text = dangChon != null && dangChon.TrangThai == TrangThaiTaiKhoan.BiKhoa ? "Mở khóa" : "Khóa TK";
    }

    private async Task TimKiemAsync()
    {
        string tuKhoa = txtTimKiem.Text.Trim();
        string vaiTro = LayMaVaiTro(cboLocVaiTro.Text);
        BatDauBan();
        try
        {
            List<TaiKhoan> danhSach = await ChayNenAsync(() => ServiceFactory.TaiKhoan.LayTatCa(tuKhoa));
            if (vaiTro != null) danhSach = danhSach.Where(tk => tk.VaiTro == vaiTro).ToList();

            Luoi.GanDuLieu(dgvTaiKhoan, danhSach);
            HienThiChiTiet();
            CapNhatTrangThaiNut();
        }
        catch (Exception ex) { BaoLoi("Không thể tải danh sách tài khoản", ex); }
        finally { KetThucBan(); }
    }

    private void HienThiChiTiet()
    {
        TaiKhoan dangChon = Luoi.LayDongDangChon<TaiKhoan>(dgvTaiKhoan);
        if (dangChon == null)
        {
            lblMaTK.Text = "—";
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            txtHoTen.Clear();
            return;
        }
        lblMaTK.Text = dangChon.MaTK.ToString();
        txtTenDangNhap.Text = dangChon.TenDangNhap;
        txtHoTen.Text = dangChon.HoTen;
        txtMatKhau.Clear();
        cboVaiTro.SelectedIndex = dangChon.VaiTro switch
        {
            VaiTro.NhanVien => 1,
            VaiTro.KhachHang => 2,
            _ => 0
        };
        cboTrangThai.SelectedIndex = dangChon.TrangThai == TrangThaiTaiKhoan.BiKhoa ? 1 : 0;
    }

    private void btnThem_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.TkThem)) return;
        _cheDo = CheDo.Them;
        lblMaTK.Text = "Mới";
        txtTenDangNhap.Clear();
        txtMatKhau.Clear();
        txtHoTen.Clear();
        txtTenDangNhap.Select();
        CapNhatTrangThaiNut();
    }

    private void btnSua_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.TkSua)) return;
        if (Luoi.LayDongDangChon<TaiKhoan>(dgvTaiKhoan) == null)
        {
            CanhBao("Vui lòng chọn một tài khoản trong danh sách.", "Chưa chọn dữ liệu");
            return;
        }
        _cheDo = CheDo.Sua;
        txtHoTen.Select();
        CapNhatTrangThaiNut();
    }

    private async void btnXoa_Click(object sender, EventArgs e)
    {
        TaiKhoan dangChon = Luoi.LayDongDangChon<TaiKhoan>(dgvTaiKhoan);
        if (dangChon == null || !CoQuyen(MaQuyen.TkXoa)) return;
        if (!XacNhan($"Xóa tài khoản \"{dangChon.TenDangNhap}\"?", "Xác nhận xóa")) return;

        await ThucHienAsync(() => ServiceFactory.TaiKhoan.Xoa(dangChon.MaTK));
        _cheDo = CheDo.Xem;
        _ = TimKiemAsync();
    }

    private async void btnLuu_Click(object sender, EventArgs e)
    {
        if (!HopLe()) return;

        var taiKhoan = new TaiKhoan
        {
            TenDangNhap = txtTenDangNhap.Text.Trim(),
            HoTen = txtHoTen.Text.Trim(),
            VaiTro = LayMaVaiTro(cboVaiTro.Text) ?? VaiTro.KhachHang,
            TrangThai = LayMaTrangThai(cboTrangThai.Text)
        };

        bool thanhCong;
        if (_cheDo == CheDo.Them)
        {
            string matKhau = txtMatKhau.Text;
            thanhCong = await ThucHienAsync(() => ServiceFactory.TaiKhoan.Them(taiKhoan, matKhau));
        }
        else
        {
            TaiKhoan dangChon = Luoi.LayDongDangChon<TaiKhoan>(dgvTaiKhoan);
            if (dangChon == null) return;
            taiKhoan.MaTK = dangChon.MaTK;
            thanhCong = await ThucHienAsync(() => ServiceFactory.TaiKhoan.CapNhat(taiKhoan));
        }

        if (!thanhCong) return;
        _cheDo = CheDo.Xem;
        _ = TimKiemAsync();
    }

    private bool HopLe()
    {
        errLoi.Clear();
        bool loi = TroGiup.Rong(txtTenDangNhap, "tên đăng nhập", errLoi);
        loi |= TroGiup.Rong(txtHoTen, "họ tên", errLoi);
        if (_cheDo == CheDo.Them)
            loi |= TroGiup.Sai(txtMatKhau.Text.Length < 6, txtMatKhau,
                "Mật khẩu phải có ít nhất 6 ký tự.", errLoi);
        return !loi;
    }

    private async void btnKhoaMo_Click(object sender, EventArgs e)
    {
        TaiKhoan dangChon = Luoi.LayDongDangChon<TaiKhoan>(dgvTaiKhoan);
        if (dangChon == null || !CoQuyen(MaQuyen.TkKhoa)) return;

        string trangThaiMoi = dangChon.TrangThai == TrangThaiTaiKhoan.BiKhoa
            ? TrangThaiTaiKhoan.HoatDong
            : TrangThaiTaiKhoan.BiKhoa;

        await ThucHienAsync(() => ServiceFactory.TaiKhoan.DoiTrangThai(dangChon.MaTK, trangThaiMoi));
        _ = TimKiemAsync();
    }

    private async void btnDatLaiMatKhau_Click(object sender, EventArgs e)
    {
        TaiKhoan dangChon = Luoi.LayDongDangChon<TaiKhoan>(dgvTaiKhoan);
        if (dangChon == null || !CoQuyen(MaQuyen.TkSua)) return;

        string matKhauMoi = frmNhapLieu.NhapChuoi("Đặt lại mật khẩu",
            $"Mật khẩu mới cho tài khoản \"{dangChon.TenDangNhap}\" (ít nhất 6 ký tự):",
            "123456", cheDoMatKhau: true, chuSoHuu: this);
        if (string.IsNullOrWhiteSpace(matKhauMoi)) return;

        await ThucHienAsync(() => ServiceFactory.TaiKhoan.DatLaiMatKhau(dangChon.MaTK, matKhauMoi.Trim()));
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
        cboLocVaiTro.SelectedIndex = 0;
        _cheDo = CheDo.Xem;
        _ = TimKiemAsync();
    }

    private void btnTim_Click(object sender, EventArgs e) => _ = TimKiemAsync();

    private void cboLocVaiTro_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_cheDo == CheDo.Xem) _ = TimKiemAsync();
    }

    private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) _ = TimKiemAsync();
    }

    private void dgvTaiKhoan_SelectionChanged(object sender, EventArgs e)
    {
        if (_cheDo != CheDo.Xem) return;
        HienThiChiTiet();
        CapNhatTrangThaiNut();
    }
}
