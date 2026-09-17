using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Quản lý hồ sơ nhân viên kèm tài khoản đăng nhập (chỉ Admin).</summary>
public partial class frmQuanLyNhanVien : BaseForm
{
    private enum CheDo { Xem, Them, Sua }
    private CheDo _cheDo = CheDo.Xem;

    public frmQuanLyNhanVien()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.NvXem;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvNhanVien);
        Luoi.DatTieuDe(dgvNhanVien,
            ("MaNV", "Mã"),
            ("HoTen", "Họ tên"),
            ("SDT", "Số điện thoại"),
            ("ChucVu", "Chức vụ"),
            ("TenDangNhap", "Tên đăng nhập"),
            ("VaiTro", "Quyền"),
            ("TrangThai", "Trạng thái"));
        Luoi.DatDoRong(dgvNhanVien, "MaNV", 55);
        Luoi.DatDoRong(dgvNhanVien, "SDT", 120);
        Luoi.DatDoRong(dgvNhanVien, "VaiTro", 110);
        Luoi.DatDoRong(dgvNhanVien, "TrangThai", 100);
        Luoi.HienThiTrangThai(dgvNhanVien, "VaiTro", VaiTro.TenHienThi);
        Luoi.HienThiTrangThai(dgvNhanVien, "TrangThai", TrangThaiNhanVien.TenHienThi);
        Luoi.ToMauTrangThai(dgvNhanVien, "TrangThai");

        cboVaiTro.Items.Clear();
        cboVaiTro.Items.AddRange(new object[] { "Nhân viên", "Quản trị viên" });
        cboVaiTro.SelectedIndex = 0;

        cboTrangThai.Items.Clear();
        cboTrangThai.Items.AddRange(new object[] { "Hoạt động", "Ngừng làm việc" });
        cboTrangThai.SelectedIndex = 0;
    }

    protected override Task TaiDuLieuAsync() => TimKiemAsync();

    protected override void CapNhatTrangThaiNut()
    {
        bool dangSua = _cheDo != CheDo.Xem;
        NhanVien dangChon = Luoi.LayDongDangChon<NhanVien>(dgvNhanVien);

        btnThem.Enabled = !dangSua && PhanQuyenService.CoQuyen(MaQuyen.NvThem);
        btnSua.Enabled = !dangSua && dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.NvSua);
        btnXoa.Enabled = !dangSua && dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.NvXoa);
        btnPhanQuyen.Enabled = !dangSua && dangChon != null && dangChon.MaTK != null
            && PhanQuyenService.CoQuyen(MaQuyen.TkPhanQuyen);
        btnKhoaMo.Enabled = !dangSua && dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.NvSua);
        btnLuu.Enabled = dangSua;
        btnHuy.Enabled = dangSua;
        btnLamMoi.Enabled = !dangSua;

        txtHoTen.ReadOnly = !dangSua;
        txtSoDienThoai.ReadOnly = !dangSua;
        txtEmail.ReadOnly = !dangSua;
        txtDiaChi.ReadOnly = !dangSua;
        txtChucVu.ReadOnly = !dangSua;
        txtTenDangNhap.ReadOnly = !dangSua || _cheDo == CheDo.Sua;
        txtMatKhau.ReadOnly = !dangSua || _cheDo == CheDo.Sua;
        dtpNgayVaoLam.Enabled = dangSua;
        cboTrangThai.Enabled = dangSua;
        cboVaiTro.Enabled = _cheDo == CheDo.Them;
        dgvNhanVien.Enabled = !dangSua;

        btnKhoaMo.Text = dangChon != null && dangChon.TrangThai == TrangThaiNhanVien.DaNghi
            ? "Đi làm lại" : "Cho nghỉ";
    }

    private async Task TimKiemAsync()
    {
        string tuKhoa = txtTimKiem.Text.Trim();
        BatDauBan();
        try
        {
            var danhSach = await ChayNenAsync(() => ServiceFactory.NhanVien.LayTatCa(tuKhoa));
            Luoi.GanDuLieu(dgvNhanVien, danhSach);
            HienThiChiTiet();
            CapNhatTrangThaiNut();
        }
        catch (Exception ex) { BaoLoi("Không thể tải danh sách nhân viên", ex); }
        finally { KetThucBan(); }
    }

    private void HienThiChiTiet()
    {
        NhanVien dangChon = Luoi.LayDongDangChon<NhanVien>(dgvNhanVien);
        if (dangChon == null)
        {
            lblMaNV.Text = "—";
            txtHoTen.Clear();
            txtSoDienThoai.Clear();
            txtEmail.Clear();
            txtDiaChi.Clear();
            txtChucVu.Clear();
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            return;
        }
        lblMaNV.Text = dangChon.MaNV.ToString();
        txtHoTen.Text = dangChon.HoTen;
        txtSoDienThoai.Text = dangChon.SDT;
        txtEmail.Text = dangChon.Email;
        txtDiaChi.Text = dangChon.DiaChi;
        txtChucVu.Text = dangChon.ChucVu;
        txtTenDangNhap.Text = dangChon.TenDangNhap;
        txtMatKhau.Clear();
        dtpNgayVaoLam.Value = dangChon.NgayVaoLam ?? DateTime.Today;
        cboTrangThai.SelectedIndex = dangChon.TrangThai == TrangThaiNhanVien.DaNghi ? 1 : 0;
        cboVaiTro.SelectedIndex = dangChon.VaiTro == VaiTro.Admin ? 1 : 0;
    }

    private void btnThem_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.NvThem)) return;
        _cheDo = CheDo.Them;
        lblMaNV.Text = "Mới";
        XoaTrong();
        txtHoTen.Select();
        CapNhatTrangThaiNut();
    }

    private void XoaTrong()
    {
        txtHoTen.Clear();
        txtSoDienThoai.Clear();
        txtEmail.Clear();
        txtDiaChi.Clear();
        txtChucVu.Clear();
        txtTenDangNhap.Clear();
        txtMatKhau.Clear();
        dtpNgayVaoLam.Value = DateTime.Today;
        cboTrangThai.SelectedIndex = 0;
    }

    private void btnSua_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.NvSua)) return;
        if (Luoi.LayDongDangChon<NhanVien>(dgvNhanVien) == null)
        {
            CanhBao("Vui lòng chọn một nhân viên trong danh sách.", "Chưa chọn dữ liệu");
            return;
        }
        _cheDo = CheDo.Sua;
        txtHoTen.Select();
        CapNhatTrangThaiNut();
    }

    private async void btnXoa_Click(object sender, EventArgs e)
    {
        NhanVien dangChon = Luoi.LayDongDangChon<NhanVien>(dgvNhanVien);
        if (dangChon == null || !CoQuyen(MaQuyen.NvXoa)) return;
        if (!XacNhan($"Xóa nhân viên \"{dangChon.HoTen}\" (kèm tài khoản đăng nhập)?", "Xác nhận xóa")) return;

        await ThucHienAsync(() => ServiceFactory.NhanVien.Xoa(dangChon.MaNV));
        _cheDo = CheDo.Xem;
        _ = TimKiemAsync();
    }

    private async void btnLuu_Click(object sender, EventArgs e)
    {
        if (!HopLe()) return;

        var nhanVien = new NhanVien
        {
            HoTen = txtHoTen.Text.Trim(),
            SDT = txtSoDienThoai.Text.Trim(),
            Email = txtEmail.Text.Trim(),
            DiaChi = txtDiaChi.Text.Trim(),
            ChucVu = txtChucVu.Text.Trim(),
            NgayVaoLam = dtpNgayVaoLam.Value.Date,
            TrangThai = cboTrangThai.SelectedIndex == 1 ? TrangThaiNhanVien.DaNghi : TrangThaiNhanVien.HoatDong
        };

        bool thanhCong;
        if (_cheDo == CheDo.Them)
        {
            string vaiTro = cboVaiTro.SelectedIndex == 1 ? VaiTro.Admin : VaiTro.NhanVien;
            string tenDangNhap = txtTenDangNhap.Text.Trim(), matKhau = txtMatKhau.Text;
            thanhCong = await ThucHienAsync(() => ServiceFactory.NhanVien.Them(nhanVien, tenDangNhap, matKhau, vaiTro));
        }
        else
        {
            NhanVien dangChon = Luoi.LayDongDangChon<NhanVien>(dgvNhanVien);
            if (dangChon == null) return;
            nhanVien.MaNV = dangChon.MaNV;
            nhanVien.MaTK = dangChon.MaTK;
            thanhCong = await ThucHienAsync(() => ServiceFactory.NhanVien.CapNhat(nhanVien));
        }

        if (!thanhCong) return;
        _cheDo = CheDo.Xem;
        _ = TimKiemAsync();
    }

    private bool HopLe()
    {
        errLoi.Clear();
        bool loi = TroGiup.Rong(txtHoTen, "họ tên", errLoi);
        loi |= TroGiup.SaiDienThoai(txtSoDienThoai, errLoi);
        loi |= TroGiup.SaiEmail(txtEmail, errLoi);
        if (_cheDo == CheDo.Them)
        {
            loi |= TroGiup.Sai(txtTenDangNhap.Text.Trim().Length < 4, txtTenDangNhap,
                "Tên đăng nhập phải có ít nhất 4 ký tự.", errLoi);
            loi |= TroGiup.Sai(txtMatKhau.Text.Length < 6, txtMatKhau,
                "Mật khẩu phải có ít nhất 6 ký tự.", errLoi);
        }
        return !loi;
    }

    /// <summary>Nhân viên không được tự nâng quyền: chức năng này chỉ dành cho Admin.</summary>
    private async void btnPhanQuyen_Click(object sender, EventArgs e)
    {
        NhanVien dangChon = Luoi.LayDongDangChon<NhanVien>(dgvNhanVien);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.TkPhanQuyen)) return;

        string vaiTroMoi = cboVaiTro.SelectedIndex == 1 ? VaiTro.Admin : VaiTro.NhanVien;
        if (!XacNhan($"Đặt quyền \"{VaiTro.TenHienThi(vaiTroMoi)}\" cho {dangChon.HoTen}?", "Phân quyền")) return;

        await ThucHienAsync(() => ServiceFactory.NhanVien.DoiVaiTro(dangChon.MaNV, vaiTroMoi));
        _ = TimKiemAsync();
    }

    private async void btnKhoaMo_Click(object sender, EventArgs e)
    {
        NhanVien dangChon = Luoi.LayDongDangChon<NhanVien>(dgvNhanVien);
        if (dangChon == null || !CoQuyen(MaQuyen.NvSua)) return;

        string trangThaiMoi = dangChon.TrangThai == TrangThaiNhanVien.DaNghi
            ? TrangThaiNhanVien.HoatDong
            : TrangThaiNhanVien.DaNghi;

        await ThucHienAsync(() => ServiceFactory.NhanVien.DoiTrangThai(dangChon.MaNV, trangThaiMoi));
        _ = TimKiemAsync();
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
        _ = TimKiemAsync();
    }

    private void btnTim_Click(object sender, EventArgs e) => _ = TimKiemAsync();

    private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) _ = TimKiemAsync();
    }

    private void dgvNhanVien_SelectionChanged(object sender, EventArgs e)
    {
        if (_cheDo != CheDo.Xem) return;
        HienThiChiTiet();
        CapNhatTrangThaiNut();
    }
}
