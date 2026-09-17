using SportFieldBooking.Core.Common;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>
/// Màn hình chính của Nhân viên: menu chức năng được cấp quyền, không có quản lý
/// tài khoản/nhân viên/cấu hình (kiểm tra quyền khi mở, không chỉ ẩn nút).
/// </summary>
public partial class frmNhanVienMain : BaseMainForm
{
    public frmNhanVienMain()
    {
        InitializeComponent();
    }

    protected override void ThietLapGiaoDien()
    {
        CapNhatNutChuDe();
        lblTenNguoiDung.Text = PhienLamViec.HoTen ?? PhienLamViec.TenDangNhap;
        lblVaiTro.Text = VaiTro.TenHienThi(PhienLamViec.VaiTro);
        MoFormCon(btnTongQuan);
    }

    private void MoFormCon(SidebarButton nutMenu)
    {
        string tieuDe;
        Form formCon;

        switch (nutMenu.Name)
        {
            case nameof(btnTongQuan):
                if (!DuocMo(MaQuyen.ThongKeNghiepVu, "thống kê")) return;
                tieuDe = "Tổng quan ca làm việc";
                formCon = new frmDashboardNhanVien();
                break;
            case nameof(btnDatSan):
                if (!DuocMo(MaQuyen.DatSanThem, "đặt sân")) return;
                tieuDe = "Đặt sân";
                formCon = new frmDatSanNhanVien();
                break;
            case nameof(btnLichDatSan):
                if (!DuocMo(MaQuyen.LichXemTatCa, "lịch đặt sân")) return;
                tieuDe = "Lịch đặt sân";
                formCon = new frmLichDatSanNhanVien();
                break;
            case nameof(btnHoaDon):
                if (!DuocMo(MaQuyen.HdXemTatCa, "hóa đơn")) return;
                tieuDe = "Hóa đơn &amp; thanh toán";
                formCon = new frmHoaDonNhanVien();
                break;
            case nameof(btnKhachHang):
                if (!DuocMo(MaQuyen.KhXem, "thông tin khách hàng")) return;
                tieuDe = "Khách hàng";
                formCon = new frmQuanLyKhachHang();
                break;
            case nameof(btnSan):
                if (!DuocMo(MaQuyen.SanXem, "quản lý sân")) return;
                tieuDe = "Thông tin sân";
                formCon = new frmSan();
                break;
            case nameof(btnLoaiSan):
                if (!DuocMo(MaQuyen.LoaiSanXem, "loại sân")) return;
                tieuDe = "Loại sân";
                formCon = new frmLoaiSan();
                break;
            case nameof(btnThongKe):
                if (!DuocMo(MaQuyen.ThongKeNghiepVu, "thống kê")) return;
                tieuDe = "Thống kê nghiệp vụ";
                formCon = new frmThongKeNhanVien();
                break;
            default:
                return;
        }

        DanhDauMenuDangChon(nutMenu, btnTongQuan, btnDatSan, btnLichDatSan, btnHoaDon,
            btnKhachHang, btnSan, btnLoaiSan, btnThongKe);

        tieuDe = tieuDe.Replace("&amp;", "&");
        lblTieuDeTrang.Text = tieuDe;
        Text = tieuDe + " - Quản lý cho thuê sân thể thao";
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

    private void frmNhanVienMain_FormClosing(object sender, FormClosingEventArgs e)
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
