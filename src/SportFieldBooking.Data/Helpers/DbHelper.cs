using System.Data;
using Microsoft.Data.SqlClient;

namespace SportFieldBooking.Data.Helpers;

/// <summary>
/// Lớp duy nhất mở kết nối và thực thi lệnh SQL (ADO.NET).
/// Mọi repository đều đi qua DbHelper - không có SqlConnection/SqlCommand nào nằm ở tầng giao diện.
/// Hỗ trợ "giao dịch bao quanh" (ambient transaction): khi tầng nghiệp vụ gọi ChayGiaoDich,
/// mọi lệnh SQL phát sinh bên trong (kể cả trong repository) tự động dùng chung một kết nối/giao dịch.
/// </summary>
public static class DbHelper
{
    // AsyncLocal (thay cho [ThreadStatic]) để giao dịch bao quanh chảy đúng qua
    // các điểm await khi tầng nghiệp vụ chạy bất đồng bộ.
    private static readonly AsyncLocal<SqlConnection?> _ketNoiHienTai = new();
    private static readonly AsyncLocal<SqlTransaction?> _giaoDichHienTai = new();

    public static string ConnectionString
    {
        get => AppSettings.ChuoiKetNoi;
        set => AppSettings.ChuoiKetNoi = value;
    }

    public static SqlConnection TaoKetNoi() => new SqlConnection(ConnectionString);

    /// <summary>Truy vấn trả về bảng dữ liệu (SELECT).</summary>
    public static DataTable TruyVan(string sql, params SqlParameter[] thamSo)
    {
        bool dungKetNoiNgoai = _ketNoiHienTai.Value != null;
        SqlConnection ketNoi = dungKetNoiNgoai ? _ketNoiHienTai.Value! : new SqlConnection(ConnectionString);
        try
        {
            if (!dungKetNoiNgoai) ketNoi.Open();
            using var lenh = TaoLenh(sql, ketNoi, thamSo);
            using var adapter = new SqlDataAdapter(lenh);
            var bang = new DataTable();
            adapter.Fill(bang);
            return bang;
        }
        finally
        {
            if (!dungKetNoiNgoai) ketNoi.Dispose();
        }
    }

    /// <summary>Thực thi INSERT/UPDATE/DELETE, trả về số dòng ảnh hưởng.</summary>
    public static int ThucThi(string sql, params SqlParameter[] thamSo)
    {
        bool dungKetNoiNgoai = _ketNoiHienTai.Value != null;
        SqlConnection ketNoi = dungKetNoiNgoai ? _ketNoiHienTai.Value! : new SqlConnection(ConnectionString);
        try
        {
            if (!dungKetNoiNgoai) ketNoi.Open();
            using var lenh = TaoLenh(sql, ketNoi, thamSo);
            return lenh.ExecuteNonQuery();
        }
        finally
        {
            if (!dungKetNoiNgoai) ketNoi.Dispose();
        }
    }

    /// <summary>Thực thi INSERT và trả về mã vừa sinh (IDENTITY).</summary>
    public static int ThucThiTraVeMa(string sql, params SqlParameter[] thamSo)
    {
        object ketQua = GiaTriDon(sql, thamSo);
        return ketQua == null || ketQua == DBNull.Value ? 0 : Convert.ToInt32(ketQua);
    }

    /// <summary>Truy vấn một giá trị đơn (COUNT, SUM, SCOPE_IDENTITY...).</summary>
    public static object GiaTriDon(string sql, params SqlParameter[] thamSo)
    {
        bool dungKetNoiNgoai = _ketNoiHienTai.Value != null;
        SqlConnection ketNoi = dungKetNoiNgoai ? _ketNoiHienTai.Value! : new SqlConnection(ConnectionString);
        try
        {
            if (!dungKetNoiNgoai) ketNoi.Open();
            using var lenh = TaoLenh(sql, ketNoi, thamSo);
            return lenh.ExecuteScalar();
        }
        finally
        {
            if (!dungKetNoiNgoai) ketNoi.Dispose();
        }
    }

    /// <summary>Kiểm tra kết nối CSDL (dùng ở màn hình khởi động/cấu hình).</summary>
    public static bool KiemTraKetNoi(out string thongBaoLoi)
    {
        try
        {
            using var ketNoi = new SqlConnection(ConnectionString);
            ketNoi.Open();
            thongBaoLoi = "";
            return true;
        }
        catch (Exception ex)
        {
            thongBaoLoi = ex.Message;
            return false;
        }
    }

    /// <summary>
    /// Chạy một khối nghiệp vụ trong giao dịch: thành công thì Commit, có lỗi thì Rollback và ném lại ngoại lệ.
    /// Các repository được gọi bên trong tự động tham gia giao dịch này.
    /// </summary>
    public static void ChayGiaoDich(Action thucHien)
    {
        if (_giaoDichHienTai.Value != null)
        {
            thucHien();          // đang trong giao dịch khác: dùng chung, không lồng
            return;
        }

        using var ketNoi = new SqlConnection(ConnectionString);
        ketNoi.Open();
        using var giaoDich = ketNoi.BeginTransaction();
        _ketNoiHienTai.Value = ketNoi;
        _giaoDichHienTai.Value = giaoDich;
        try
        {
            thucHien();
            giaoDich.Commit();
        }
        catch
        {
            giaoDich.Rollback();
            throw;
        }
        finally
        {
            _ketNoiHienTai.Value = null;
            _giaoDichHienTai.Value = null;
        }
    }

    private static SqlCommand TaoLenh(string sql, SqlConnection ketNoi, SqlParameter[] thamSo)
    {
        var lenh = new SqlCommand(sql, ketNoi, _giaoDichHienTai.Value);
        if (thamSo?.Length > 0) lenh.Parameters.AddRange(thamSo);
        return lenh;
    }

    // ==================================================================
    //  BẢN BẤT ĐỒNG BỘ (async) - dùng cho luồng giao diện để không đứng hình
    // ==================================================================

    /// <summary>Truy vấn SELECT bất đồng bộ.</summary>
    public static async Task<DataTable> TruyVanAsync(string sql, params SqlParameter[] thamSo)
    {
        bool dungKetNoiNgoai = _ketNoiHienTai.Value != null;
        SqlConnection ketNoi = dungKetNoiNgoai ? _ketNoiHienTai.Value! : new SqlConnection(ConnectionString);
        try
        {
            if (!dungKetNoiNgoai) await ketNoi.OpenAsync();
            using var lenh = TaoLenh(sql, ketNoi, thamSo);
            using var adapter = new SqlDataAdapter(lenh);
            var bang = new DataTable();
            // SqlDataAdapter không có FillAsync: đọc bằng DbDataReader async rồi nạp vào DataTable.
            using var doc = await lenh.ExecuteReaderAsync();
            bang.Load(doc);
            return bang;
        }
        finally
        {
            if (!dungKetNoiNgoai) ketNoi.Dispose();
        }
    }

    /// <summary>INSERT/UPDATE/DELETE bất đồng bộ, trả số dòng ảnh hưởng.</summary>
    public static async Task<int> ThucThiAsync(string sql, params SqlParameter[] thamSo)
    {
        bool dungKetNoiNgoai = _ketNoiHienTai.Value != null;
        SqlConnection ketNoi = dungKetNoiNgoai ? _ketNoiHienTai.Value! : new SqlConnection(ConnectionString);
        try
        {
            if (!dungKetNoiNgoai) await ketNoi.OpenAsync();
            using var lenh = TaoLenh(sql, ketNoi, thamSo);
            return await lenh.ExecuteNonQueryAsync();
        }
        finally
        {
            if (!dungKetNoiNgoai) ketNoi.Dispose();
        }
    }

    /// <summary>Truy vấn một giá trị đơn bất đồng bộ.</summary>
    public static async Task<object> GiaTriDonAsync(string sql, params SqlParameter[] thamSo)
    {
        bool dungKetNoiNgoai = _ketNoiHienTai.Value != null;
        SqlConnection ketNoi = dungKetNoiNgoai ? _ketNoiHienTai.Value! : new SqlConnection(ConnectionString);
        try
        {
            if (!dungKetNoiNgoai) await ketNoi.OpenAsync();
            using var lenh = TaoLenh(sql, ketNoi, thamSo);
            return (await lenh.ExecuteScalarAsync())!;
        }
        finally
        {
            if (!dungKetNoiNgoai) ketNoi.Dispose();
        }
    }

    /// <summary>INSERT trả mã vừa sinh, bất đồng bộ.</summary>
    public static async Task<int> ThucThiTraVeMaAsync(string sql, params SqlParameter[] thamSo)
    {
        object ketQua = await GiaTriDonAsync(sql, thamSo);
        return ketQua == null || ketQua == DBNull.Value ? 0 : Convert.ToInt32(ketQua);
    }

    /// <summary>
    /// Chạy một khối nghiệp vụ bất đồng bộ trong giao dịch: Commit khi xong,
    /// Rollback và ném lại ngoại lệ nếu lỗi. Các repository gọi bên trong tự tham gia.
    /// </summary>
    public static async Task ChayGiaoDichAsync(Func<Task> thucHien)
    {
        if (_giaoDichHienTai.Value != null)
        {
            await thucHien();        // đang trong giao dịch khác: dùng chung, không lồng
            return;
        }

        using var ketNoi = new SqlConnection(ConnectionString);
        await ketNoi.OpenAsync();
        using var giaoDich = ketNoi.BeginTransaction();
        _ketNoiHienTai.Value = ketNoi;
        _giaoDichHienTai.Value = giaoDich;
        try
        {
            await thucHien();
            giaoDich.Commit();
        }
        catch
        {
            giaoDich.Rollback();
            throw;
        }
        finally
        {
            _ketNoiHienTai.Value = null;
            _giaoDichHienTai.Value = null;
        }
    }
}
