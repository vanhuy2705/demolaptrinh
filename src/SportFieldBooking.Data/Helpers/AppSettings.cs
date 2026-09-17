using System.Text.Json;

namespace SportFieldBooking.Data.Helpers;

/// <summary>
/// Đọc cấu hình ứng dụng (chuỗi kết nối) từ file appsettings.json nằm cạnh file chạy.
/// Nếu không tìm thấy file/hỏng định dạng thì dùng chuỗi kết nối mặc định để ứng dụng vẫn khởi động.
/// </summary>
public static class AppSettings
{
    private const string MacDinh =
        @"Data Source=.\SQLEXPRESS;Initial Catalog=QLSanTheThao;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

    private static string _chuoiKetNoi;

    public static string ChuoiKetNoi
    {
        get => string.IsNullOrWhiteSpace(_chuoiKetNoi) ? (_chuoiKetNoi = DocTuFile()) : _chuoiKetNoi;
        set => _chuoiKetNoi = value;
    }

    private static string DocTuFile()
    {
        try
        {
            string duongDan = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(duongDan)) return MacDinh;

            using var taiLieu = JsonDocument.Parse(File.ReadAllText(duongDan));
            if (taiLieu.RootElement.TryGetProperty("ConnectionStrings", out var phanKetNoi)
                && phanKetNoi.TryGetProperty("DefaultConnection", out var giaTri))
            {
                string ketQua = giaTri.GetString();
                return string.IsNullOrWhiteSpace(ketQua) ? MacDinh : ketQua;
            }
        }
        catch (Exception)
        {
            // Không để lỗi cấu hình làm sập ứng dụng: dùng giá trị mặc định.
        }
        return MacDinh;
    }
}
