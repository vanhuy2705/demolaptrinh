using System.Data;
using Microsoft.Data.SqlClient;
using SportFieldBooking.Data.Helpers;

namespace SportFieldBooking.Data.Helpers;

/// <summary>
/// Lớp cơ sở của mọi repository: cung cấp sẵn các hàm thực thi SQL qua DbHelper.
/// Repository chỉ viết câu lệnh SQL + ánh xạ dữ liệu, không chứa quy tắc nghiệp vụ.
/// </summary>
public abstract class BaseRepository
{
    protected DataTable TruyVan(string sql, params SqlParameter[] thamSo) =>
        DbHelper.TruyVan(sql, thamSo);

    protected List<T> DanhSach<T>(string sql, Func<DataRow, T> anhXa, params SqlParameter[] thamSo) =>
        DbHelper.TruyVan(sql, thamSo).Rows.Cast<DataRow>().Select(anhXa).ToList();

    protected T MotHoacNull<T>(string sql, Func<DataRow, T> anhXa, params SqlParameter[] thamSo) where T : class
    {
        DataTable bang = DbHelper.TruyVan(sql, thamSo);
        return bang.Rows.Count == 0 ? null : anhXa(bang.Rows[0]);
    }

    protected int ThucThi(string sql, params SqlParameter[] thamSo) =>
        DbHelper.ThucThi(sql, thamSo);

    protected int ThemTraVeMa(string sql, params SqlParameter[] thamSo) =>
        DbHelper.ThucThiTraVeMa(sql, thamSo);

    protected object GiaTriDon(string sql, params SqlParameter[] thamSo) =>
        DbHelper.GiaTriDon(sql, thamSo);

    protected static SqlParameter ThamSo(string ten, object giaTri)
    {
        object dung = giaTri ?? DBNull.Value;
        if (giaTri is string s && s.Length == 0) dung = s;
        return new SqlParameter(ten, dung);
    }

    /// <summary>Tạo tham số tìm kiếm dạng LIKE, tự xử lý chuỗi rỗng.</summary>
    protected static SqlParameter ThamSoTimKiem(string ten, string tuKhoa) =>
        new SqlParameter(ten, SqlDbType.NVarChar, 200) { Value = string.IsNullOrWhiteSpace(tuKhoa) ? (object)DBNull.Value : "%" + tuKhoa.Trim() + "%" };
}
