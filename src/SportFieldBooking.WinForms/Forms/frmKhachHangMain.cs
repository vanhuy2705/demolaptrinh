using SportFieldBooking.Core.Common;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>
/// Màn hình chính của Khách hàng (cổng khách hàng): chỉ mở được dữ liệu của chính mình,
/// mọi chức năng đều kiểm tra quyền trước khi mở.
/// </summary>
public partial class frmKhachHangMain : BaseMainForm
{
    public frmKhachHangMain()
    {
        InitializeComponent();
    }

    protected override void ThietLapGiaoDien()
    {
        CapNhatNutChuDe();
        lblTenNguoiDung.Text = PhienLamViec.HoTen ?? PhienLamViec.TenDangNhap;
        lblVaiTro.Text = VaiTro.TenHienThi(PhienLamViec.VaiTro);
        MoFormCon(btnTrangChu);
    }

    private void MoFormCon(SidebarButton nutMenu)
    {
        string tieuDe;
        Form formCon;

        switch (nutMenu.Name)
        {
            case nameof(btnTrangChu):
                if (!DuocMo(MaQuyen.ThongKeCaNhan, "trang chủ")) return;
                tieuDe = "Trang chủ";
                var trangChu = new frmTrangChuKhachHang();
                trangChu.MuonDatSan += (_, _) => MoFormCon(btnDatSan);
                formCon = trangChu;
                break;
            case nameof(btnDatSan):
                if (!DuocMo(MaQuyen.DatSanThem, "đặt sân")) return;
                tieuDe = "Đặt sân";
                formCon = new frmDatSanKhachHang();
                break;
            case nameof(btnLichSuDatSan):
                if (!DuocMo(MaQuyen.DatSanXemCuaToi, "lịch sử đặt sân")) return;
                tieuDe = "Lịch sử đặt sân";
                formCon = new frmLichSuDatSan();
                break;
            case nameof(btnHoaDon):
                if (!DuocMo(MaQuyen.HdXemCuaToi, "hóa đơn của tôi")) return;
                tieuDe = "Hóa đơn của tôi";
                formCon = new frmHoaDonCuaToi();
                break;
            case nameof(btnVoucher):
                if (!DuocMo(MaQuyen.VoucherXem, "voucher")) return;
                tieuDe = "Voucher của tôi";
                formCon = new frmVoucherCuaToi();
                break;
            case nameof(btnThongTin):
                if (!DuocMo(MaQuyen.KhXem, "thông tin khách hàng")) return;
                tieuDe = "Thông tin cá nhân";
                formCon = new frmThongTinCaNhan();
                break;
            default:
                return;
        }

        DanhDauMenuDangChon(nutMenu, btnTrangChu, btnDatSan, btnLichSuDatSan,
            btnHoaDon, btnVoucher, btnThongTin);

        lblTieuDeTrang.Text = tieuDe;
        Text = tieuDe + " - Thuê sân thể thao";
        MoFormCon(formCon, pnlNoiDung);
    }


    /// <summary>Kiểm tra quyền mở chức năng (không chỉ ẩn nút): thông báo và chặn nếu không được phép.</summary>
    private bool DuocMo(string maQuyen, string tenChucNang)
    {
        if (PhanQuyenService.CoQuyen(maQuyen)) return true;
        CanhBao($"Bạn không có quyền sử dụng chức năng {tenChucNang}.\nVui lòng liên hệ quản trị viên.",
            "Không đủ quyền");
        return false;
    }

    private void NhanMenu(object sender, EventArgs e)
    {
        if (sender is SidebarButton nut) MoFormCon(nut);
    }

    private void btnDoiMatKhau_Click(object sender, EventArgs e)
    {
        using var doiMatKhau = new frmDoiMatKhau();
        doiMatKhau.ShowDialog(this);
    }

    private void btnDangXuat_Click(object sender, EventArgs e) => DangXuat();

    private void frmKhachHangMain_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (e.CloseReason != CloseReason.UserClosing) return;
        if (!XacNhan("Bạn có chắc muốn thoát chương trình?", "Thoát chương trình"))
        {
            e.Cancel = true;
            return;
        }
        DialogResult = DialogResult.Cancel;
    }

    /// <summary>Đổi chủ đề Sáng/Tối ngay khi đang chạy (áp dụng lại cho mọi cửa sổ đang mở).</summary>
    private void btnChuDe_Click(object sender, EventArgs e)
    {
        GiaoDien.DoiChuDe();
        CapNhatNutChuDe();
    }

    /// <summary>Cập nhật nhãn + icon của nút đổi chủ đề theo chủ đề hiện tại.</summary>
    private void CapNhatNutChuDe()
    {
        GiaoDien.DangNutPhu(btnChuDe);
        btnChuDe.Text = GiaoDien.LaThemeToi ? "Chủ đề sáng" : "Chủ đề tối";
        btnChuDe.Image = VeBieuTuong.TaoAnh(GiaoDien.LaThemeToi ? "moon" : "sun", 20, GiaoDien.Chu);
    }

}
