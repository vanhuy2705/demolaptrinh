using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Quản lý voucher: phát hành mã, giới hạn số lượng, giá trị đơn tối thiểu, thời hạn.</summary>
public partial class frmVoucher : BaseForm
{
    private enum CheDo { Xem, Them, Sua }
    private CheDo _cheDo = CheDo.Xem;

    public frmVoucher()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.VoucherXem;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvVoucher);
        Luoi.DatTieuDe(dgvVoucher,
            ("MaCode", "Mã"),
            ("TenVoucher", "Tên voucher"),
            ("LoaiGiam", "Loại giảm"),
            ("GiaTriGiamHienThi", "Mức giảm"),
            ("GiaTriGiam", "Giá trị"),
            ("DonToiThieu", "Đơn tối thiểu"),
            ("SoLuongDaDung", "Đã dùng"),
            ("SoLuongConLai", "Còn lại"),
            ("NgayKetThuc", "Hết hạn"),
            ("TrangThai", "Trạng thái"));
        Luoi.DatDoRong(dgvVoucher, "MaCode", 110);
        Luoi.DatDoRong(dgvVoucher, "LoaiGiam", 100);
        Luoi.DatDinhDangNgay(dgvVoucher, "dd/MM/yyyy", "NgayKetThuc");
        Luoi.HienThiTrangThai(dgvVoucher, "TrangThai", TrangThaiVoucher.TenHienThi);
        Luoi.ToMauTrangThai(dgvVoucher, "TrangThai");
        Luoi.AnCot(dgvVoucher, "LoaiGiam");
        dgvVoucher.CellFormatting += dgvVoucher_CellFormatting;

        cboLoaiGiam.Items.AddRange(new object[] { "Phần trăm (%)", "Số tiền (đ)" });
        cboLoaiGiam.SelectedIndex = 0;
        cboTrangThai.Items.AddRange(new object[] { "Hoạt động", "Tạm ngưng" });
        cboTrangThai.SelectedIndex = 0;
    }

    protected override Task TaiDuLieuAsync() => TimKiemAsync();

    protected override void CapNhatTrangThaiNut()
    {
        bool dangSua = _cheDo != CheDo.Xem;
        Voucher dangChon = Luoi.LayDongDangChon<Voucher>(dgvVoucher);

        btnThem.Enabled = !dangSua && PhanQuyenService.CoQuyen(MaQuyen.VoucherThem);
        btnSua.Enabled = !dangSua && dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.VoucherSua);
        btnXoa.Enabled = !dangSua && dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.VoucherXoa);
        btnLuu.Enabled = dangSua;
        btnHuy.Enabled = dangSua;
        btnLamMoi.Enabled = !dangSua;

        txtMaCode.ReadOnly = !dangSua;
        txtTenVoucher.ReadOnly = !dangSua;
        txtGiaTriGiam.ReadOnly = !dangSua;
        txtDonToiThieu.ReadOnly = !dangSua;
        txtSoLuong.ReadOnly = !dangSua;
        txtMoTa.ReadOnly = !dangSua;
        cboLoaiGiam.Enabled = dangSua;
        cboTrangThai.Enabled = dangSua;
        dtpNgayBatDau.Enabled = dangSua;
        dtpNgayKetThuc.Enabled = dangSua;
        dgvVoucher.Enabled = !dangSua;
    }

    private async Task TimKiemAsync()
    {
        string tuKhoa = txtTimKiem.Text.Trim();
        bool chiConHan = chkChiConHan.Checked;
        BatDauBan();
        try
        {
            var danhSach = await ChayNenAsync(() => chiConHan
                ? ServiceFactory.Voucher.LayVoucherCoTheDung(tuKhoa)
                : ServiceFactory.Voucher.LayTatCa(tuKhoa));
            Luoi.GanDuLieu(dgvVoucher, danhSach);
            HienThiChiTiet();
            CapNhatTrangThaiNut();
        }
        catch (Exception ex) { BaoLoi("Không thể tải danh sách voucher", ex); }
        finally { KetThucBan(); }
    }

    private void HienThiChiTiet()
    {
        Voucher dangChon = Luoi.LayDongDangChon<Voucher>(dgvVoucher);
        if (dangChon == null)
        {
            lblMaVoucher.Text = "—";
            txtMaCode.Clear();
            txtTenVoucher.Clear();
            txtGiaTriGiam.Clear();
            txtDonToiThieu.Clear();
            txtSoLuong.Clear();
            txtMoTa.Clear();
            lblDaDung.Text = "0";
            return;
        }
        lblMaVoucher.Text = dangChon.MaVoucher.ToString();
        txtMaCode.Text = dangChon.MaCode;
        txtTenVoucher.Text = dangChon.TenVoucher;
        txtGiaTriGiam.Text = dangChon.GiaTriGiam.ToString("0");
        txtDonToiThieu.Text = dangChon.DonToiThieu.ToString("0");
        txtSoLuong.Text = dangChon.SoLuong.ToString();
        txtMoTa.Text = dangChon.MoTa;
        lblDaDung.Text = $"{dangChon.SoLuongDaDung} / {dangChon.SoLuong}";
        cboLoaiGiam.SelectedIndex = dangChon.LoaiGiam == LoaiGiam.PhanTram ? 0 : 1;
        cboTrangThai.SelectedIndex = dangChon.TrangThai == TrangThaiVoucher.HoatDong ? 0 : 1;
        dtpNgayBatDau.Value = dangChon.NgayBatDau;
        dtpNgayKetThuc.Value = dangChon.NgayKetThuc;
    }

    private void btnThem_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.VoucherThem)) return;
        _cheDo = CheDo.Them;
        lblMaVoucher.Text = "Mới";
        txtMaCode.Clear();
        txtTenVoucher.Clear();
        txtGiaTriGiam.Clear();
        txtDonToiThieu.Text = "0";
        txtSoLuong.Text = "100";
        txtMoTa.Clear();
        lblDaDung.Text = "0";
        dtpNgayBatDau.Value = DateTime.Today;
        dtpNgayKetThuc.Value = DateTime.Today.AddMonths(3);
        txtMaCode.Select();
        CapNhatTrangThaiNut();
    }

    private void btnSua_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.VoucherSua)) return;
        if (Luoi.LayDongDangChon<Voucher>(dgvVoucher) == null)
        {
            CanhBao("Vui lòng chọn một voucher trong danh sách.", "Chưa chọn dữ liệu");
            return;
        }
        _cheDo = CheDo.Sua;
        CapNhatTrangThaiNut();
    }

    private async void btnXoa_Click(object sender, EventArgs e)
    {
        Voucher dangChon = Luoi.LayDongDangChon<Voucher>(dgvVoucher);
        if (dangChon == null || !CoQuyen(MaQuyen.VoucherXoa)) return;
        if (!XacNhan($"Xóa voucher \"{dangChon.MaCode}\"?", "Xác nhận xóa")) return;

        await ThucHienAsync(() => ServiceFactory.Voucher.Xoa(dangChon.MaVoucher));
        _cheDo = CheDo.Xem;
        _ = TimKiemAsync();
    }

    private async void btnLuu_Click(object sender, EventArgs e)
    {
        if (!HopLe(out decimal giaTriGiam, out decimal donToiThieu, out int soLuong)) return;

        bool laPhanTram = cboLoaiGiam.SelectedIndex == 0;
        var voucher = new Voucher
        {
            MaCode = txtMaCode.Text.Trim().ToUpperInvariant(),
            TenVoucher = txtTenVoucher.Text.Trim(),
            LoaiGiam = laPhanTram ? LoaiGiam.PhanTram : LoaiGiam.SoTien,
            GiaTriGiam = giaTriGiam,
            DonToiThieu = donToiThieu,
            SoLuong = soLuong,
            NgayBatDau = dtpNgayBatDau.Value.Date,
            NgayKetThuc = dtpNgayKetThuc.Value.Date,
            TrangThai = cboTrangThai.SelectedIndex == 0 ? TrangThaiVoucher.HoatDong : TrangThaiVoucher.TamNgung,
            MoTa = txtMoTa.Text.Trim()
        };

        bool thanhCong;
        if (_cheDo == CheDo.Them)
        {
            thanhCong = await ThucHienAsync(() => ServiceFactory.Voucher.Them(voucher));
        }
        else
        {
            Voucher dangChon = Luoi.LayDongDangChon<Voucher>(dgvVoucher);
            if (dangChon == null) return;
            voucher.MaVoucher = dangChon.MaVoucher;
            voucher.SoLuongDaDung = dangChon.SoLuongDaDung;
            thanhCong = await ThucHienAsync(() => ServiceFactory.Voucher.CapNhat(voucher));
        }

        if (!thanhCong) return;
        _cheDo = CheDo.Xem;
        _ = TimKiemAsync();
    }

    private bool HopLe(out decimal giaTriGiam, out decimal donToiThieu, out int soLuong)
    {
        errLoi.Clear();
        bool loi = TroGiup.Rong(txtMaCode, "mã voucher", errLoi);
        loi |= TroGiup.Rong(txtTenVoucher, "tên voucher", errLoi);
        loi |= TroGiup.SaiTien(txtGiaTriGiam, "giá trị giảm", errLoi, out giaTriGiam);
        // Đơn tối thiểu được phép để trống hoặc bằng 0 (voucher không yêu cầu đơn tối thiểu).
        if (string.IsNullOrWhiteSpace(txtDonToiThieu.Text))
        {
            donToiThieu = 0m;
            errLoi.SetError(txtDonToiThieu, "");
        }
        else
        {
            loi |= TroGiup.SaiTien(txtDonToiThieu, "đơn tối thiểu", errLoi, out donToiThieu, choPhepBangKhong: true);
        }
        loi |= TroGiup.SaiSoNguyen(txtSoLuong, "số lượng", errLoi, out soLuong, nhoNhat: 1);

        if (!loi && cboLoaiGiam.SelectedIndex == 0 && giaTriGiam > 100)
        {
            errLoi.SetError(txtGiaTriGiam, "Phần trăm giảm không được vượt quá 100%.");
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

    private void chkChiConHan_CheckedChanged(object sender, EventArgs e) => _ = TimKiemAsync();

    private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) _ = TimKiemAsync();
    }

    private void dgvVoucher_SelectionChanged(object sender, EventArgs e)
    {
        if (_cheDo != CheDo.Xem) return;
        HienThiChiTiet();
        CapNhatTrangThaiNut();
    }

    /// <summary>Hiển thị mức giảm kèm đơn vị (% hoặc tiền) thay cho giá trị thô.</summary>
    private void dgvVoucher_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || dgvVoucher.Columns[e.ColumnIndex].Name != "GiaTriGiam") return;
        if (dgvVoucher.Rows[e.RowIndex].DataBoundItem is not Voucher voucher) return;

        e.Value = voucher.LoaiGiam == LoaiGiam.PhanTram
            ? voucher.GiaTriGiam.ToString("0") + " %"
            : TroGiup.Tien(voucher.GiaTriGiam);
        e.FormattingApplied = true;
    }
}
