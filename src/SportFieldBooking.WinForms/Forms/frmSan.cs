using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>
/// Quản lý sân: CRUD (Admin), đổi trạng thái nghiệp vụ (Nhân viên), xem (Khách hàng).
/// </summary>
public partial class frmSan : BaseForm
{
    private enum CheDo { Xem, Them, Sua }
    private CheDo _cheDo = CheDo.Xem;
    private List<LoaiSan> _danhSachLoaiSan = new();

    public frmSan()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.SanXem;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvSan);
        Luoi.DatTieuDe(dgvSan,
            ("MaSan", "Mã"),
            ("TenSan", "Tên sân"),
            ("TenLoaiSan", "Loại sân"),
            ("DonGia", "Đơn giá/giờ"),
            ("TrangThai", "Trạng thái"),
            ("MoTa", "Mô tả"));
        Luoi.DatDoRong(dgvSan, "MaSan", 60);
        Luoi.DatDoRong(dgvSan, "DonGia", 120);
        Luoi.DatDoRong(dgvSan, "TrangThai", 110);
        Luoi.DatDinhDangTien(dgvSan, "DonGia");
        Luoi.HienThiTrangThai(dgvSan, "TrangThai", TrangThaiSan.TenHienThi);
        Luoi.ToMauTrangThai(dgvSan, "TrangThai");

        NapComboBox();
    }

    private void NapComboBox()
    {
        _danhSachLoaiSan = ServiceFactory.LoaiSan.LayTatCa();
        TroGiup.GanComboBox(cboLoaiSan, _danhSachLoaiSan, "TenLoaiSan", "MaLoaiSan");

        cboTrangThai.Items.Clear();
        cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Trống", "Đang thuê", "Bảo trì" });
        cboTrangThai.SelectedIndex = 0;

        cboTrangThaiNhap.Items.Clear();
        cboTrangThaiNhap.Items.AddRange(new object[] { "Trống", "Đang thuê", "Bảo trì" });
        cboTrangThaiNhap.SelectedIndex = 0;
    }

    private static string LayMaTrangThai(string tenHienThi) => tenHienThi switch
    {
        "Trống" => TrangThaiSan.Trong,
        "Đang thuê" => TrangThaiSan.DangThue,
        "Bảo trì" => TrangThaiSan.BaoTri,
        _ => null
    };

    protected override void TaiDuLieu() => TimKiem();

    protected override void CapNhatTrangThaiNut()
    {
        bool dangSua = _cheDo != CheDo.Xem;
        bool coQuyenQuanLy = PhanQuyenService.CoQuyen(MaQuyen.SanThem) || PhanQuyenService.CoQuyen(MaQuyen.SanSua);
        San dangChon = Luoi.LayDongDangChon<San>(dgvSan);

        btnThem.Enabled = coQuyenQuanLy && !dangSua && PhanQuyenService.CoQuyen(MaQuyen.SanThem);
        btnSua.Enabled = coQuyenQuanLy && !dangSua && dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.SanSua);
        btnXoa.Enabled = !dangSua && dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.SanXoa);
        btnLuu.Enabled = dangSua;
        btnHuy.Enabled = dangSua;
        btnDoiTrangThai.Enabled = !dangSua && dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.SanDoiTrangThai);
        btnLamMoi.Enabled = !dangSua;

        txtTenSan.ReadOnly = !dangSua;
        txtDonGia.ReadOnly = !dangSua;
        txtMoTa.ReadOnly = !dangSua;
        cboLoaiSan.Enabled = dangSua;
        cboTrangThaiNhap.Enabled = dangSua;
        dgvSan.Enabled = !dangSua;
    }

    private void TimKiem()
    {
        ThucHien(() =>
        {
            string trangThai = LayMaTrangThai(cboTrangThai.Text);
            Luoi.GanDuLieu(dgvSan, ServiceFactory.San.LayTatCa(txtTimKiem.Text.Trim(), null, trangThai));
            HienThiChiTiet();
            CapNhatTrangThaiNut();
        }, "Không thể tải danh sách sân");
    }

    private void HienThiChiTiet()
    {
        San dangChon = Luoi.LayDongDangChon<San>(dgvSan);
        if (dangChon == null)
        {
            lblMaSan.Text = "—";
            txtTenSan.Clear();
            txtDonGia.Clear();
            txtMoTa.Clear();
            return;
        }
        lblMaSan.Text = dangChon.MaSan.ToString();
        txtTenSan.Text = dangChon.TenSan;
        txtDonGia.Text = ((long)dangChon.DonGia).ToString();
        txtMoTa.Text = dangChon.MoTa;
        cboLoaiSan.SelectedValue = dangChon.MaLoaiSan;
        cboTrangThaiNhap.SelectedIndex = dangChon.TrangThai switch
        {
            TrangThaiSan.DangThue => 1,
            TrangThaiSan.BaoTri => 2,
            _ => 0
        };
    }

    private void btnThem_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.SanThem)) return;
        _cheDo = CheDo.Them;
        lblMaSan.Text = "Mới";
        txtTenSan.Clear();
        txtDonGia.Clear();
        txtMoTa.Clear();
        txtTenSan.Select();
        CapNhatTrangThaiNut();
    }

    private void btnSua_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.SanSua)) return;
        if (Luoi.LayDongDangChon<San>(dgvSan) == null)
        {
            CanhBao("Vui lòng chọn một sân trong danh sách.", "Chưa chọn dữ liệu");
            return;
        }
        _cheDo = CheDo.Sua;
        txtTenSan.Select();
        CapNhatTrangThaiNut();
    }

    private void btnXoa_Click(object sender, EventArgs e)
    {
        San dangChon = Luoi.LayDongDangChon<San>(dgvSan);
        if (dangChon == null || !CoQuyen(MaQuyen.SanXoa)) return;
        if (!XacNhan($"Xóa sân \"{dangChon.TenSan}\"?", "Xác nhận xóa")) return;

        ThucHien(ServiceFactory.San.Xoa(dangChon.MaSan));
        _cheDo = CheDo.Xem;
        TimKiem();
    }

    private void btnLuu_Click(object sender, EventArgs e)
    {
        if (!HopLe(out decimal donGia)) return;

        var san = new San
        {
            TenSan = txtTenSan.Text.Trim(),
            MaLoaiSan = TroGiup.LayGiaTriComboBox(cboLoaiSan),
            DonGia = donGia,
            TrangThai = LayMaTrangThai(cboTrangThaiNhap.Text) ?? TrangThaiSan.Trong,
            MoTa = txtMoTa.Text.Trim()
        };

        bool thanhCong;
        if (_cheDo == CheDo.Them)
        {
            thanhCong = ThucHien(ServiceFactory.San.Them(san));
        }
        else
        {
            San dangChon = Luoi.LayDongDangChon<San>(dgvSan);
            if (dangChon == null) return;
            san.MaSan = dangChon.MaSan;
            thanhCong = ThucHien(ServiceFactory.San.CapNhat(san));
        }

        if (!thanhCong) return;
        _cheDo = CheDo.Xem;
        TimKiem();
    }

    private bool HopLe(out decimal donGia)
    {
        errLoi.Clear();
        bool loi = TroGiup.Rong(txtTenSan, "tên sân", errLoi);
        loi |= TroGiup.SaiTien(txtDonGia, "Đơn giá", errLoi, out donGia);
        loi |= TroGiup.Sai(cboLoaiSan.SelectedValue == null, cboLoaiSan, "Vui lòng chọn loại sân.", errLoi);
        return !loi;
    }

    /// <summary>Đổi nhanh trạng thái nghiệp vụ của sân (Admin và Nhân viên).</summary>
    private void btnDoiTrangThai_Click(object sender, EventArgs e)
    {
        San dangChon = Luoi.LayDongDangChon<San>(dgvSan);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.SanDoiTrangThai)) return;

        string trangThaiMoi = dangChon.TrangThai switch
        {
            TrangThaiSan.Trong => TrangThaiSan.DangThue,
            TrangThaiSan.DangThue => TrangThaiSan.BaoTri,
            _ => TrangThaiSan.Trong
        };

        if (!XacNhan($"Chuyển sân \"{dangChon.TenSan}\" sang trạng thái {TrangThaiSan.TenHienThi(trangThaiMoi)}?",
                "Đổi trạng thái sân")) return;

        ThucHien(ServiceFactory.San.DoiTrangThai(dangChon.MaSan, trangThaiMoi));
        TimKiem();
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
        cboTrangThai.SelectedIndex = 0;
        _cheDo = CheDo.Xem;
        TimKiem();
    }

    private void btnTim_Click(object sender, EventArgs e) => TimKiem();

    private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_cheDo == CheDo.Xem) TimKiem();
    }

    private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) TimKiem();
    }

    private void dgvSan_SelectionChanged(object sender, EventArgs e)
    {
        if (_cheDo != CheDo.Xem) return;
        HienThiChiTiet();
        CapNhatTrangThaiNut();
    }
}
