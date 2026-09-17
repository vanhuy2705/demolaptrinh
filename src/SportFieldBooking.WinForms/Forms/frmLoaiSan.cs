using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Quản lý loại sân (Admin: CRUD; Nhân viên/Khách hàng: chỉ xem).</summary>
public partial class frmLoaiSan : BaseForm
{
    private enum CheDo { Xem, Them, Sua }
    private CheDo _cheDo = CheDo.Xem;

    public frmLoaiSan()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.LoaiSanXem;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvLoaiSan);
        Luoi.DatTieuDe(dgvLoaiSan,
            ("MaLoaiSan", "Mã"),
            ("TenLoaiSan", "Tên loại sân"),
            ("MoTa", "Mô tả"),
            ("SoLuongSan", "Số sân"));
        Luoi.DatDoRong(dgvLoaiSan, "MaLoaiSan", 70);
        Luoi.DatDoRong(dgvLoaiSan, "SoLuongSan", 90);
        AnCot();
    }

    protected override void TaiDuLieu() => TimKiem();

    protected override void CapNhatTrangThaiNut()
    {
        bool dangSua = _cheDo != CheDo.Xem;
        bool coQuyenQuanLy = PhanQuyenService.CoQuyen(MaQuyen.LoaiSanThem);

        btnThem.Enabled = coQuyenQuanLy && !dangSua;
        btnSua.Enabled = coQuyenQuanLy && !dangSua && Luoi.LayDongDangChon<LoaiSan>(dgvLoaiSan) != null;
        btnXoa.Enabled = btnSua.Enabled && PhanQuyenService.CoQuyen(MaQuyen.LoaiSanXoa);
        btnLuu.Enabled = dangSua;
        btnHuy.Enabled = dangSua;
        btnLamMoi.Enabled = !dangSua;

        bool choPhepNhap = (dangSua || coQuyenQuanLy) && PhanQuyenService.CoQuyen(MaQuyen.LoaiSanThem);
        txtTenLoaiSan.ReadOnly = !dangSua;
        txtMoTa.ReadOnly = !dangSua;
        dgvLoaiSan.Enabled = !dangSua;
        btnThem.Visible = choPhepNhap || !coQuyenQuanLy;
        btnSua.Visible = choPhepNhap || !coQuyenQuanLy;
        btnXoa.Visible = choPhepNhap || !coQuyenQuanLy;
    }

    private void AnCot() { }

    private void TimKiem()
    {
        ThucHien(() =>
        {
            Luoi.GanDuLieu(dgvLoaiSan, ServiceFactory.LoaiSan.LayTatCa(txtTimKiem.Text.Trim()));
            HienThiChiTiet();
            CapNhatTrangThaiNut();
        }, "Không thể tải danh sách loại sân");
    }

    private void HienThiChiTiet()
    {
        LoaiSan dangChon = Luoi.LayDongDangChon<LoaiSan>(dgvLoaiSan);
        if (dangChon == null)
        {
            txtTenLoaiSan.Clear();
            txtMoTa.Clear();
            lblMaLoaiSan.Text = "—";
            return;
        }
        lblMaLoaiSan.Text = dangChon.MaLoaiSan.ToString();
        txtTenLoaiSan.Text = dangChon.TenLoaiSan;
        txtMoTa.Text = dangChon.MoTa;
    }

    private void btnThem_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.LoaiSanThem)) return;
        _cheDo = CheDo.Them;
        lblMaLoaiSan.Text = "Mới";
        txtTenLoaiSan.Clear();
        txtMoTa.Clear();
        txtTenLoaiSan.Select();
        CapNhatTrangThaiNut();
    }

    private void btnSua_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.LoaiSanSua)) return;
        if (Luoi.LayDongDangChon<LoaiSan>(dgvLoaiSan) == null)
        {
            CanhBao("Vui lòng chọn một loại sân trong danh sách.", "Chưa chọn dữ liệu");
            return;
        }
        _cheDo = CheDo.Sua;
        txtTenLoaiSan.Select();
        CapNhatTrangThaiNut();
    }

    private void btnXoa_Click(object sender, EventArgs e)
    {
        LoaiSan dangChon = Luoi.LayDongDangChon<LoaiSan>(dgvLoaiSan);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.LoaiSanXoa)) return;
        if (!XacNhan($"Xóa loại sân \"{dangChon.TenLoaiSan}\"?", "Xác nhận xóa")) return;

        ThucHien(ServiceFactory.LoaiSan.Xoa(dangChon.MaLoaiSan));
        _cheDo = CheDo.Xem;
        TimKiem();
    }

    private void btnLuu_Click(object sender, EventArgs e)
    {
        if (!HopLe()) return;

        var loaiSan = new LoaiSan
        {
            TenLoaiSan = txtTenLoaiSan.Text.Trim(),
            MoTa = txtMoTa.Text.Trim()
        };

        bool thanhCong;
        if (_cheDo == CheDo.Them)
        {
            KetQua<LoaiSan> ketQua = ServiceFactory.LoaiSan.Them(loaiSan);
            thanhCong = ThucHien(ketQua);
        }
        else
        {
            LoaiSan dangChon = Luoi.LayDongDangChon<LoaiSan>(dgvLoaiSan);
            if (dangChon == null) return;
            loaiSan.MaLoaiSan = dangChon.MaLoaiSan;
            thanhCong = ThucHien(ServiceFactory.LoaiSan.CapNhat(loaiSan));
        }

        if (!thanhCong) return;
        _cheDo = CheDo.Xem;
        TimKiem();
    }

    private bool HopLe()
    {
        errLoi.Clear();
        return !TroGiup.Rong(txtTenLoaiSan, "tên loại sân", errLoi);
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

    private void dgvLoaiSan_SelectionChanged(object sender, EventArgs e)
    {
        if (_cheDo != CheDo.Xem) return;
        HienThiChiTiet();
        CapNhatTrangThaiNut();
    }
}
