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

        // Lưới an toàn: các handler async void / tác vụ nền không được quan sát
        // nếu ném lỗi sẽ báo hộp thoại thay vì làm ứng dụng thoát đột ngột.
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) => BaoLoiKhongMongMuon(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, e) => BaoLoiKhongMongMuon(e.ExceptionObject as Exception);
        TaskScheduler.UnobservedTaskException += (_, e) => e.SetObserved();

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

    private static void BaoLoiKhongMongMuon(Exception ex)
    {
        try
        {
            frmThongBao.HienThi(
                "Đã xảy ra lỗi ngoài ý muốn:\n\n" + (ex?.Message ?? "Không xác định") +
                "\n\nỨng dụng vẫn tiếp tục hoạt động. Nếu lỗi lặp lại, hãy kiểm tra kết nối cơ sở dữ liệu.",
                "Lỗi không mong muốn", frmThongBao.LoaiThongBao.Loi);
        }
        catch
        {
            // Bản thân trình báo lỗi không được phép ném tiếp.
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
