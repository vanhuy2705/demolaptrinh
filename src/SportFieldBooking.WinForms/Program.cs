using SportFieldBooking.Core.Common;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.WinForms.Forms;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.SetDefaultFont(new Font(GiaoDien.TenFont, 10F));

        // Chủ đề giao diện: dùng lại lựa chọn người dùng đã nhớ (mặc định Tối).
        GiaoDien.ChuyenTheme(toi: CaiDatNguoiDung.LaThemeToi);

        KiemTraKetNoiBanDau();

        // Vòng lặp phiên làm việc: đăng nhập -> mở màn hình chính -> đăng xuất -> đăng nhập lại.
        while (true)
        {
            using var dangNhap = new frmDangNhap();
            if (dangNhap.ShowDialog() != DialogResult.OK) break;

            Form manHinhChinh = PhienLamViec.VaiTro switch
            {
                VaiTro.Admin => new frmAdminMain(),
                VaiTro.NhanVien => new frmNhanVienMain(),
                VaiTro.KhachHang => new frmKhachHangMain(),
                _ => null
            };

            if (manHinhChinh == null) break;
            Application.Run(manHinhChinh);
        }
    }

    private static void KiemTraKetNoiBanDau()
    {
        if (DbHelper.KiemTraKetNoi(out string loi)) return;

        string thongDiep =
            "Không thể kết nối tới cơ sở dữ liệu QLSanTheThao.\n\n" +
            "• Hãy chạy script database/01_TaoCSDL.sql trước.\n" +
            "• Kiểm tra chuỗi kết nối trong file appsettings.json.\n\n" +
            "Chi tiết: " + loi;
        frmThongBao.HienThi(thongDiep, "Lỗi kết nối cơ sở dữ liệu", frmThongBao.LoaiThongBao.Loi);
    }
}
