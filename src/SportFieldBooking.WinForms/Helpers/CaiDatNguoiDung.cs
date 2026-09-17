using System.Text.Json;

namespace SportFieldBooking.WinForms.Helpers;

/// <summary>
/// Lưu / đọc thiết lập cá nhân của người dùng (chủ đề, tài khoản gần nhất,
/// kích thước cửa sổ chính) vào file JSON trong %AppData%.
/// Giúp ứng dụng "nhớ" thói quen người dùng giữa các lần chạy.
/// Mọi thao tác đều nuốt lỗi để không bao giờ làm hỏng luồng khởi động.
/// </summary>
public static class CaiDatNguoiDung
{
    private sealed class BoCaiDat
    {
        public bool LaThemeToi { get; set; } = true;
        public string TenDangNhapGanNhat { get; set; } = "";
        public int RongCuaSo { get; set; }
        public int CaoCuaSo { get; set; }
        public bool Maximized { get; set; } = true;
    }

    private static readonly object _khoa = new();
    private static BoCaiDat? _boNho;

    private static string DuongDanThuMuc =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SportFieldBooking");

    private static string DuongDanFile => Path.Combine(DuongDanThuMuc, "caidat.json");

    private static BoCaiDat BoHienTai
    {
        get
        {
            if (_boNho != null) return _boNho;
            lock (_khoa)
            {
                if (_boNho != null) return _boNho;
                _boNho = Doc();
                return _boNho;
            }
        }
    }

    private static BoCaiDat Doc()
    {
        try
        {
            if (File.Exists(DuongDanFile))
            {
                var bo = JsonSerializer.Deserialize<BoCaiDat>(File.ReadAllText(DuongDanFile));
                if (bo != null) return bo;
            }
        }
        catch { /* file hỏng / không đọc được: dùng mặc định */ }
        return new BoCaiDat();
    }

    private static void Luu()
    {
        try
        {
            Directory.CreateDirectory(DuongDanThuMuc);
            File.WriteAllText(DuongDanFile,
                JsonSerializer.Serialize(BoHienTai, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch { /* không ghi được (quyền, ổ đĩa): bỏ qua, không làm sập app */ }
    }

    // --- Chủ đề ---
    public static bool LaThemeToi => BoHienTai.LaThemeToi;

    /// <summary>Ghi nhớ chủ đề vừa chuyển.</summary>
    public static void DatTheme(bool toi)
    {
        if (BoHienTai.LaThemeToi == toi) return;
        BoHienTai.LaThemeToi = toi;
        Luu();
    }

    // --- Tài khoản gần nhất ---
    public static string TenDangNhapGanNhat => BoHienTai.TenDangNhapGanNhat ?? "";

    public static void DatTenDangNhap(string ten)
    {
        string giaTri = ten?.Trim() ?? "";
        if (BoHienTai.TenDangNhapGanNhat == giaTri) return;
        BoHienTai.TenDangNhapGanNhat = giaTri;
        Luu();
    }

    // --- Kích thước cửa sổ chính ---
    public static Size? KichThuocCuaSo
    {
        get
        {
            var bo = BoHienTai;
            return bo.RongCuaSo > 400 && bo.CaoCuaSo > 300 ? new Size(bo.RongCuaSo, bo.CaoCuaSo) : null;
        }
    }

    public static bool CuaSoMaximized => BoHienTai.Maximized;

    public static void DatCuaSo(Size kichThuoc, bool maximized)
    {
        var bo = BoHienTai;
        bool doi = bo.RongCuaSo != kichThuoc.Width || bo.CaoCuaSo != kichThuoc.Height || bo.Maximized != maximized;
        bo.RongCuaSo = kichThuoc.Width;
        bo.CaoCuaSo = kichThuoc.Height;
        bo.Maximized = maximized;
        if (doi) Luu();
    }
}
