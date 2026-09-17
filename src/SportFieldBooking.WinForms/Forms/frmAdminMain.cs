using SportFieldBooking.Core.Common;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Controls;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>
/// Màn hình chính của Quản trị viên: menu chức năng bên trái, nội dung mở bên trong khung.
/// Mỗi chức năng được kiểm tra quyền trước khi mở.
/// </summary>
public partial class frmAdminMain : BaseMainForm
{
    public frmAdminMain()
    {
        InitializeComponent();
    }

    protected override void ThietLapGiaoDien()
    {
        CapNhatNutChuDe();
        lblTenNguoiDung.Text = PhienLamViec.HoTen ?? PhienLamViec.TenDangNhap;
        lblVaiTro.Text = VaiTro.TenHienThi(PhienLamViec.VaiTro);
        MoTrangChu();
    }

    private void MoTrangChu() => MoFormCon(btnTongQuan);

    /// <summary>Mở form con nếu người dùng có quyền, đồng thời cập nhật tiêu đề trang.</summary>
    private void MoFormCon(SidebarButton nutMenu)
    {
        string tieuDe;
        Form formCon;

        switch (nutMenu.Name)
        {
            case nameof(btnTongQuan):
                if (!DuocMo(MaQuyen.ThongKeToanBo, "thống kê")) return;
                tieuDe = "Tổng quan hệ thống";
                formCon = new frmDashboardAdmin();
                break;
            case nameof(btnLoaiSan):
                if (!DuocMo(MaQuyen.SanXem, "loại sân / sân")) return;
                tieuDe = "Quản lý loại sân";
                formCon = new frmLoaiSan();
                break;
            case nameof(btnSan):
                if (!DuocMo(MaQuyen.SanXem, "quản lý sân")) return;
                tieuDe = "Quản lý sân";
                formCon = new frmSan();
                break;
            case nameof(btnKhachHang):
                if (!DuocMo(MaQuyen.KhXemTatCa, "quản lý khách hàng")) return;
                tieuDe = "Quản lý khách hàng";
                formCon = new frmQuanLyKhachHang();
                break;
            case nameof(btnDatSan):
                if (!DuocMo(MaQuyen.DatSanXemTatCa, "đặt sân")) return;
                tieuDe = "Đặt sân";
                formCon = new frmDatSanAdmin();
                break;
            case nameof(btnLichDatSan):
                if (!DuocMo(MaQuyen.LichXemTatCa, "lịch đặt sân")) return;
                tieuDe = "Lịch đặt sân";
                formCon = new frmLichDatSanAdmin();
                break;
            case nameof(btnHoaDon):
                if (!DuocMo(MaQuyen.HdXemTatCa, "hóa đơn")) return;
                tieuDe = "Quản lý hóa đơn";
                formCon = new frmHoaDonAdmin();
                break;
            case nameof(btnVoucher):
                if (!DuocMo(MaQuyen.VoucherXem, "voucher")) return;
                tieuDe = "Quản lý voucher";
                formCon = new frmVoucher();
                break;
            case nameof(btnKhuyenMai):
                if (!DuocMo(MaQuyen.KmXem, "khuyến mãi")) return;
                tieuDe = "Chương trình khuyến mãi";
                formCon = new frmKhuyenMai();
                break;
            case nameof(btnTaiKhoan):
                if (!DuocMo(MaQuyen.TkXem, "quản lý tài khoản")) return;
                tieuDe = "Quản lý tài khoản";
                formCon = new frmQuanLyTaiKhoan();
                break;
            case nameof(btnNhanVien):
                if (!DuocMo(MaQuyen.NvXem, "quản lý nhân viên")) return;
                tieuDe = "Quản lý nhân viên";
                formCon = new frmQuanLyNhanVien();
                break;
            case nameof(btnThongKe):
                if (!DuocMo(MaQuyen.ThongKeToanBo, "thống kê")) return;
                tieuDe = "Thống kê & báo cáo";
                formCon = new frmThongKeAdmin();
                break;
            case nameof(btnCauHinh):
                if (!DuocMo(MaQuyen.CauHinhXem, "cấu hình hệ thống")) return;
                tieuDe = "Cấu hình hệ thống";
                formCon = new frmCauHinh();
                break;
            default:
                return;
        }

        DanhDauMenuDangChon(nutMenu, btnTongQuan, btnLoaiSan, btnSan, btnKhachHang, btnDatSan,
            btnLichDatSan, btnHoaDon, btnVoucher, btnKhuyenMai, btnTaiKhoan, btnNhanVien,
            btnThongKe, btnCauHinh);

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

    private void frmAdminMain_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            if (!XacNhan("Bạn có chắc muốn thoát chương trình?", "Thoát chương trình"))
            {
                e.Cancel = true;
                return;
            }
            DialogResult = DialogResult.Cancel;
        }
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
