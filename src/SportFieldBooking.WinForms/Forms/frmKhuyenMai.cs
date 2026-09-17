using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Quản lý chương trình khuyến mãi đặc biệt (áp dụng tự động theo khoảng ngày).</summary>
public partial class frmKhuyenMai : BaseForm
{
    private enum CheDo { Xem, Them, Sua }
    private CheDo _cheDo = CheDo.Xem;

    public frmKhuyenMai()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.KmXem;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvKhuyenMai);
        Luoi.DatTieuDe(dgvKhuyenMai,
            ("MaKM", "Mã"),
            ("TenKM", "Tên chương trình"),
            ("LoaiKhuyenMai", "Loại"),
            ("PhanTramGiam", "% giảm"),
            ("NgayBatDau", "Từ ngày"),
            ("NgayKetThuc", "Đến ngày"),
            ("ApDungCuoiTuan", "Chỉ cuối tuần"),
            ("TrangThai", "Trạng thái"));
        Luoi.DatDoRong(dgvKhuyenMai, "MaKM", 60);
        Luoi.DatDoRong(dgvKhuyenMai, "PhanTramGiam", 80);
        Luoi.DatDinhDangNgay(dgvKhuyenMai, "dd/MM/yyyy", "NgayBatDau", "NgayKetThuc");
        Luoi.HienThiTrangThai(dgvKhuyenMai, "TrangThai", TrangThaiVoucher.TenHienThi);
        Luoi.ToMauTrangThai(dgvKhuyenMai, "TrangThai");

        cboTrangThai.Items.AddRange(new object[] { "Hoạt động", "Tạm ngưng" });
        cboTrangThai.SelectedIndex = 0;
    }

    protected override Task TaiDuLieuAsync() => TimKiemAsync();

    protected override void CapNhatTrangThaiNut()
    {
        bool dangSua = _cheDo != CheDo.Xem;
        KhuyenMai dangChon = Luoi.LayDongDangChon<KhuyenMai>(dgvKhuyenMai);

        btnThem.Enabled = !dangSua && PhanQuyenService.CoQuyen(MaQuyen.KmThem);
        btnSua.Enabled = !dangSua && dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.KmSua);
        btnXoa.Enabled = !dangSua && dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.KmXoa);
        btnLuu.Enabled = dangSua;
        btnHuy.Enabled = dangSua;
        btnLamMoi.Enabled = !dangSua;

        txtTenKM.ReadOnly = !dangSua;
        txtLoaiKhuyenMai.ReadOnly = !dangSua;
        txtPhanTramGiam.ReadOnly = !dangSua;
        txtMoTa.ReadOnly = !dangSua;
        chkApDungCuoiTuan.Enabled = dangSua;
        cboTrangThai.Enabled = dangSua;
        dtpNgayBatDau.Enabled = dangSua;
        dtpNgayKetThuc.Enabled = dangSua;
        dgvKhuyenMai.Enabled = !dangSua;
    }

    private async Task TimKiemAsync()
    {
        string tuKhoa = txtTimKiem.Text.Trim();
        BatDauBan();
        try
        {
            var danhSach = await ChayNenAsync(() => ServiceFactory.KhuyenMai.LayTatCa(tuKhoa));
            Luoi.GanDuLieu(dgvKhuyenMai, danhSach);
            HienThiChiTiet();
            CapNhatTrangThaiNut();
        }
        catch (Exception ex) { BaoLoi("Không thể tải danh sách khuyến mãi", ex); }
        finally { KetThucBan(); }
    }

    private void HienThiChiTiet()
    {
        KhuyenMai dangChon = Luoi.LayDongDangChon<KhuyenMai>(dgvKhuyenMai);
        if (dangChon == null)
        {
            lblMaKM.Text = "—";
            txtTenKM.Clear();
            txtLoaiKhuyenMai.Clear();
            txtPhanTramGiam.Clear();
            txtMoTa.Clear();
            return;
        }
        lblMaKM.Text = dangChon.MaKM.ToString();
        txtTenKM.Text = dangChon.TenKM;
        txtLoaiKhuyenMai.Text = dangChon.LoaiKhuyenMai;
        txtPhanTramGiam.Text = dangChon.PhanTramGiam.ToString("0.##");
        txtMoTa.Text = dangChon.MoTa;
        chkApDungCuoiTuan.Checked = dangChon.ApDungCuoiTuan;
        cboTrangThai.SelectedIndex = dangChon.TrangThai == TrangThaiVoucher.HoatDong ? 0 : 1;
        dtpNgayBatDau.Value = dangChon.NgayBatDau;
        dtpNgayKetThuc.Value = dangChon.NgayKetThuc;
    }

    private void btnThem_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.KmThem)) return;
        _cheDo = CheDo.Them;
        lblMaKM.Text = "Mới";
        txtTenKM.Clear();
        txtLoaiKhuyenMai.Clear();
        txtPhanTramGiam.Clear();
        txtMoTa.Clear();
        dtpNgayBatDau.Value = DateTime.Today;
        dtpNgayKetThuc.Value = DateTime.Today.AddDays(30);
        txtTenKM.Select();
        CapNhatTrangThaiNut();
    }

    private void btnSua_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.KmSua)) return;
        if (Luoi.LayDongDangChon<KhuyenMai>(dgvKhuyenMai) == null)
        {
            CanhBao("Vui lòng chọn một chương trình trong danh sách.", "Chưa chọn dữ liệu");
            return;
        }
        _cheDo = CheDo.Sua;
        CapNhatTrangThaiNut();
    }

    private async void btnXoa_Click(object sender, EventArgs e)
    {
        KhuyenMai dangChon = Luoi.LayDongDangChon<KhuyenMai>(dgvKhuyenMai);
        if (dangChon == null || !CoQuyen(MaQuyen.KmXoa)) return;
        if (!XacNhan($"Xóa chương trình \"{dangChon.TenKM}\"?", "Xác nhận xóa")) return;

        await ThucHienAsync(() => ServiceFactory.KhuyenMai.Xoa(dangChon.MaKM));
        _cheDo = CheDo.Xem;
        _ = TimKiemAsync();
    }

    private async void btnLuu_Click(object sender, EventArgs e)
    {
        if (!HopLe()) return;

        var khuyenMai = new KhuyenMai
        {
            TenKM = txtTenKM.Text.Trim(),
            LoaiKhuyenMai = txtLoaiKhuyenMai.Text.Trim(),
            PhanTramGiam = decimal.Parse(txtPhanTramGiam.Text.Trim()),
            NgayBatDau = dtpNgayBatDau.Value.Date,
            NgayKetThuc = dtpNgayKetThuc.Value.Date,
            ApDungCuoiTuan = chkApDungCuoiTuan.Checked,
            TrangThai = cboTrangThai.SelectedIndex == 0 ? TrangThaiVoucher.HoatDong : TrangThaiVoucher.TamNgung,
            MoTa = txtMoTa.Text.Trim()
        };

        bool thanhCong;
        if (_cheDo == CheDo.Them)
        {
            thanhCong = await ThucHienAsync(() => ServiceFactory.KhuyenMai.Them(khuyenMai));
        }
        else
        {
            KhuyenMai dangChon = Luoi.LayDongDangChon<KhuyenMai>(dgvKhuyenMai);
            if (dangChon == null) return;
            khuyenMai.MaKM = dangChon.MaKM;
            thanhCong = await ThucHienAsync(() => ServiceFactory.KhuyenMai.CapNhat(khuyenMai));
        }

        if (!thanhCong) return;
        _cheDo = CheDo.Xem;
        _ = TimKiemAsync();
    }

    private bool HopLe()
    {
        errLoi.Clear();
        bool loi = TroGiup.Rong(txtTenKM, "tên chương trình", errLoi);
        loi |= TroGiup.SaiTien(txtPhanTramGiam, "phần trăm giảm", errLoi, out decimal phanTramGiam);

        if (!loi && phanTramGiam > 100)
        {
            errLoi.SetError(txtPhanTramGiam, "Phần trăm giảm không được vượt quá 100%.");
            loi = true;
        }
        if (!loi && dtpNgayKetThuc.Value.Date < dtpNgayBatDau.Value.Date)
        {
            errLoi.SetError(dtpNgayKetThuc, "Ngày kết thúc phải sau ngày bắt đầu.");
            loi = true;
        }
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
        _ = TimKiemAsync();
    }

    private void btnTim_Click(object sender, EventArgs e) => _ = TimKiemAsync();

    private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) _ = TimKiemAsync();
    }

    private void dgvKhuyenMai_SelectionChanged(object sender, EventArgs e)
    {
        if (_cheDo != CheDo.Xem) return;
        HienThiChiTiet();
        CapNhatTrangThaiNut();
    }
}
