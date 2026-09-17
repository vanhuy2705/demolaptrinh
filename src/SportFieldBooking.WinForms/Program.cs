using SportFieldBooking.Core.Common;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Repositories;
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

        if (KiemTraKetNoiBanDau())
            NangCapLuocDoBanDau();

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

    /// <summary>Kiểm tra kết nối CSDL lúc khởi động. Trả về true nếu kết nối được.</summary>
    private static bool KiemTraKetNoiBanDau()
    {
        if (DbHelper.KiemTraKetNoi(out string loi)) return true;

        string thongDiep =
            "Không thể kết nối tới cơ sở dữ liệu QLSanTheThao.\n\n" +
            "• Cài mới: chạy database/01_TaoCSDL_v2.sql rồi database/02_DuLieuMau_v2.sql.\n" +
            "• Đã có CSDL cũ: chạy database/04_NangCapCSDL_v2.sql (ứng dụng cũng tự nâng cấp phần còn thiếu).\n" +
            "• Kiểm tra chuỗi kết nối trong file appsettings.json.\n\n" +
            "Chi tiết: " + loi;
        frmThongBao.HienThi(thongDiep, "Lỗi kết nối cơ sở dữ liệu", frmThongBao.LoaiThongBao.Loi);
        return false;
    }

    /// <summary>
    /// Tự bổ sung phần lược đồ CSDL mà phiên bản ứng dụng này cần (ví dụ cột DAT_SAN.MaVoucher).
    /// Người dùng chỉ cần git pull rồi chạy - không phải nhớ chạy thêm script SQL.
    /// Thao tác này idempotent và chỉ THÊM, không xoá/sửa dữ liệu cũ.
    /// </summary>
    private static void NangCapLuocDoBanDau()
    {
        NangCapCSDL.KetQua ketQua = NangCapCSDL.KiemTraVaCapNhat();

        if (ketQua.DaThayDoi)
        {
            // Vừa thay đổi lược đồ -> bỏ kết quả kiểm tra cột đã nhớ trong repository.
            DatSanRepository.DatLaiKiemTraCot();
            return;
        }

        if (!ketQua.ThanhCong)
        {
            // Không tự nâng cấp được (thường do tài khoản SQL thiếu quyền ALTER).
            // Ứng dụng vẫn chạy: DatSanRepository tự bỏ cột mới khỏi câu truy vấn.
            frmThongBao.HienThi(
                "Ứng dụng không tự nâng cấp được cơ sở dữ liệu (có thể do tài khoản SQL thiếu quyền ALTER).\n\n" +
                "Chương trình vẫn chạy bình thường, nhưng voucher gắn với lượt đặt sân sẽ không được lưu.\n" +
                "Để bật lại: nhờ quản trị chạy script database/05_BoSungVoucherDatSan_v3.sql.\n\n" +
                "Chi tiết: " + ketQua.Loi,
                "Cần nâng cấp cơ sở dữ liệu", frmThongBao.LoaiThongBao.CanhBao);
        }
    }
}
