using System.Security.Cryptography;
using System.Text;

namespace SportFieldBooking.Core.Security;

/// <summary>
/// Mã hóa / kiểm tra mật khẩu bằng PBKDF2 (SHA-256) có muối ngẫu nhiên.
/// Định dạng lưu trong CSDL: iterations.saltBase64.hashBase64
/// Vẫn hỗ trợ so khớp mật khẩu dạng thô (dữ liệu seed/demo nhập tay) để tránh khóa tài khoản.
/// </summary>
public static class PasswordHasher
{
    private const int Iterations = 100_000;
    private const int SaltSize = 16;
    private const int HashSize = 32;

    public static string MaHoa(string matKhau)
    {
        if (matKhau == null) matKhau = "";
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(matKhau), salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    /// <summary>Kiểm tra mật khẩu người dùng nhập có khớp với chuỗi đang lưu hay không.</summary>
    public static bool KiemTra(string matKhau, string duLieuDaLuu)
    {
        if (string.IsNullOrEmpty(duLieuDaLuu) || matKhau == null) return false;

        string[] parts = duLieuDaLuu.Split('.');
        if (parts.Length != 3) return string.Equals(matKhau, duLieuDaLuu, StringComparison.Ordinal);

        if (!int.TryParse(parts[0], out int iterations)) return false;

        byte[] salt, hash;
        try
        {
            salt = Convert.FromBase64String(parts[1]);
            hash = Convert.FromBase64String(parts[2]);
        }
        catch (FormatException)
        {
            return false;
        }

        byte[] hashThu = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(matKhau), salt, iterations, HashAlgorithmName.SHA256, hash.Length);
        return CryptographicOperations.FixedTimeEquals(hashThu, hash);
    }

    /// <summary>True nếu chuỗi đang lưu đã được băm đúng định dạng.</summary>
    public static bool LaMatKhauDaBam(string duLieuDaLuu)
    {
        if (string.IsNullOrEmpty(duLieuDaLuu)) return false;
        string[] parts = duLieuDaLuu.Split('.');
        return parts.Length == 3 && int.TryParse(parts[0], out _);
    }
}
