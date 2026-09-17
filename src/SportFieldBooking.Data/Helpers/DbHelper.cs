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
    [ThreadStatic] private static SqlConnection _ketNoiHienTai;
    [ThreadStatic] private static SqlTransaction _giaoDichHienTai;

    public static string ConnectionString
    {
        get => AppSettings.ChuoiKetNoi;
        set => AppSettings.ChuoiKetNoi = value;
    }

    public static SqlConnection TaoKetNoi() => new SqlConnection(ConnectionString);

    /// <summary>Truy vấn trả về bảng dữ liệu (SELECT).</summary>
    public static DataTable TruyVan(string sql, params SqlParameter[] thamSo)
    {
        bool dungKetNoiNgoai = _ketNoiHienTai != null;
        SqlConnection ketNoi = dungKetNoiNgoai ? _ketNoiHienTai : new SqlConnection(ConnectionString);
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
        bool dungKetNoiNgoai = _ketNoiHienTai != null;
        SqlConnection ketNoi = dungKetNoiNgoai ? _ketNoiHienTai : new SqlConnection(ConnectionString);
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
        bool dungKetNoiNgoai = _ketNoiHienTai != null;
        SqlConnection ketNoi = dungKetNoiNgoai ? _ketNoiHienTai : new SqlConnection(ConnectionString);
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
        if (_giaoDichHienTai != null)
        {
            thucHien();          // đang trong giao dịch khác: dùng chung, không lồng
            return;
        }

        using var ketNoi = new SqlConnection(ConnectionString);
        ketNoi.Open();
        using var giaoDich = ketNoi.BeginTransaction();
        _ketNoiHienTai = ketNoi;
        _giaoDichHienTai = giaoDich;
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
            _ketNoiHienTai = null;
            _giaoDichHienTai = null;
        }
    }

    private static SqlCommand TaoLenh(string sql, SqlConnection ketNoi, SqlParameter[] thamSo)
    {
        var lenh = new SqlCommand(sql, ketNoi, _giaoDichHienTai);
        if (thamSo?.Length > 0) lenh.Parameters.AddRange(thamSo);
        return lenh;
    }
}
