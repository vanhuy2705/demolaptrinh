using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

public partial class frmNhatKyHoatDong : BaseForm
{
    public frmNhatKyHoatDong()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.NhatKyXem;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvNhatKy);
        Luoi.DatTieuDe(dgvNhatKy,
            ("ThoiGian", "Thời gian"),
            ("TenDangNhap", "Tài khoản"),
            ("HoTen", "Họ tên"),
            ("VaiTro", "Vai trò"),
            ("HoatDong", "Hành động"),
            ("BangDuLieu", "Bảng"),
            ("MaDuLieu", "Mã DL"),
            ("NoiDung", "Nội dung"),
            ("KetQua", "Kết quả"),
            ("MayTram", "Máy trạm"));
        Luoi.DatDoRong(dgvNhatKy, "ThoiGian", 140);
        Luoi.DatDoRong(dgvNhatKy, "TenDangNhap", 110);
        Luoi.DatDoRong(dgvNhatKy, "HoatDong", 110);
        Luoi.DatDoRong(dgvNhatKy, "KetQua", 90);
        Luoi.DatDinhDangNgay(dgvNhatKy, "dd/MM/yyyy HH:mm:ss", "ThoiGian");
        Luoi.HienThiTrangThai(dgvNhatKy, "KetQua", KetQuaNhatKy.TenHienThi);
        Luoi.ToMauTrangThai(dgvNhatKy, "KetQua");

        dtpTuNgay.Value = DateTime.Today.AddDays(-7);
        dtpDenNgay.Value = DateTime.Today;

        cboHoatDong.Items.Clear();
        cboHoatDong.Items.Add("(Tất cả)");
        cboHoatDong.Items.AddRange(new object[] {
            "DangNhap", "DangXuat", "DangKy",
            "ThemDatSan", "SuaDatSan", "HuyDatSan",
            "LapHoaDon", "SuaHoaDon", "HuyHoaDon", "ThanhToan", "XoaHoaDon",
            "ThemVoucher", "SuaVoucher", "XoaVoucher",
            "ThemKhachHang", "SuaKhachHang", "XoaKhachHang",
            "ThemNhanVien", "SuaNhanVien", "XoaNhanVien", "DoiTrangThaiNhanVien",
            "ThemTaiKhoan", "SuaTaiKhoan", "KhoaTaiKhoan", "PhanQuyen",
            "SuaCauHinh", "DoiMatKhau"
        });
        cboHoatDong.SelectedIndex = 0;

        cboKetQua.Items.Clear();
        cboKetQua.Items.Add("(Tất cả)");
        cboKetQua.Items.AddRange(new object[] { "Thành công", "Thất bại", "Cảnh báo" });
        cboKetQua.SelectedIndex = 0;
    }

    protected override Task TaiDuLieuAsync() => TimKiemAsync();

    protected override void CapNhatTrangThaiNut()
    {
        var dangChon = Luoi.LayDongDangChon<NhatKyHoatDong>(dgvNhatKy);
        btnXoa.Enabled = dangChon != null && PhanQuyenService.CoQuyen(MaQuyen.NhatKyXoa);
        btnXoaCu.Enabled = PhanQuyenService.CoQuyen(MaQuyen.NhatKyXoa);
    }

    private async Task TimKiemAsync()
    {
        string tuKhoa = txtTimKiem.Text.Trim();
        string hoatDong = cboHoatDong.SelectedIndex <= 0 ? null : cboHoatDong.SelectedItem?.ToString();
        string ketQua = cboKetQua.SelectedIndex switch
        {
            1 => KetQuaNhatKy.ThanhCong,
            2 => KetQuaNhatKy.ThatBai,
            3 => KetQuaNhatKy.CanhBao,
            _ => null
        };
        DateTime? tuNgay = dtpTuNgay.Value.Date;
        DateTime? denNgay = dtpDenNgay.Value.Date;

        BatDauBan();
        try
        {
            var ds = await ChayNenAsync(() => ServiceFactory.NhatKy.LayTatCa(tuKhoa, hoatDong, ketQua, tuNgay, denNgay));
            Luoi.GanDuLieu(dgvNhatKy, ds);
            lblThongKe.Text = $"Tổng: {ds.Count} bản ghi | Từ {tuNgay:dd/MM/yyyy} đến {denNgay:dd/MM/yyyy}";
            CapNhatTrangThaiNut();
        }
        catch (Exception ex) { BaoLoi("Không thể tải nhật ký hoạt động", ex); }
        finally { KetThucBan(); }
    }

    private void btnTim_Click(object sender, EventArgs e) => _ = TimKiemAsync();

    private void btnLamMoi_Click(object sender, EventArgs e)
    {
        txtTimKiem.Clear();
        cboHoatDong.SelectedIndex = 0;
        cboKetQua.SelectedIndex = 0;
        dtpTuNgay.Value = DateTime.Today.AddDays(-7);
        dtpDenNgay.Value = DateTime.Today;
        _ = TimKiemAsync();
    }

    private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) _ = TimKiemAsync();
    }

    private async void btnXoa_Click(object sender, EventArgs e)
    {
        var dangChon = Luoi.LayDongDangChon<NhatKyHoatDong>(dgvNhatKy);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.NhatKyXoa)) return;
        if (!XacNhan($"Xóa bản ghi nhật ký #{dangChon.MaNhatKy}?", "Xác nhận xóa")) return;

        await ThucHienAsync(() => ServiceFactory.NhatKy.Xoa(dangChon.MaNhatKy));
        _ = TimKiemAsync();
    }

    private async void btnXoaCu_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.NhatKyXoa)) return;
        string soNgayStr = frmNhapLieu.NhapChuoi("Xóa nhật ký cũ", "Xóa các bản ghi trước bao nhiêu ngày? (ví dụ 30):", "30", false, this);
        if (soNgayStr == null) return;
        if (!int.TryParse(soNgayStr, out int soNgay) || soNgay <= 0)
        {
            CanhBao("Số ngày không hợp lệ.", "Lỗi");
            return;
        }
        if (!XacNhan($"Xóa tất cả nhật ký trước {DateTime.Today.AddDays(-soNgay):dd/MM/yyyy}?", "Xác nhận xóa hàng loạt")) return;

        await ThucHienAsync(() => ServiceFactory.NhatKy.XoaCu(DateTime.Today.AddDays(-soNgay)));
        _ = TimKiemAsync();
    }
}
