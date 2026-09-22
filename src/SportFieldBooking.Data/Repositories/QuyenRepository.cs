using System.Data;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.Data.Helpers;
using SportFieldBooking.Data.Interfaces;

namespace SportFieldBooking.Data.Repositories;

public class QuyenRepository : BaseRepository, IQuyenRepository
{
    public List<Quyen> LayTatCa()
    {
        try
        {
            return DanhSach("SELECT MaQuyen, TenQuyen, NhomQuyen, ISNULL(MoTa,'') AS MoTa FROM QUYEN ORDER BY NhomQuyen, MaQuyen",
                dong => new Quyen
                {
                    MaQuyen = dong.Chuoi("MaQuyen"),
                    TenQuyen = dong.Chuoi("TenQuyen"),
                    NhomQuyen = dong.Chuoi("NhomQuyen"),
                    MoTa = dong.Chuoi("MoTa")
                });
        }
        catch { return new List<Quyen>(); }
    }

    public List<VaiTroDb> LayVaiTro()
    {
        try
        {
            return DanhSach("SELECT MaVaiTro, TenVaiTro, ISNULL(MoTa,'') AS MoTa, ISNULL(LaMacDinh,0) AS LaMacDinh FROM VAI_TRO ORDER BY MaVaiTro",
                dong => new VaiTroDb
                {
                    MaVaiTro = dong.Chuoi("MaVaiTro"),
                    TenVaiTro = dong.Chuoi("TenVaiTro"),
                    MoTa = dong.Chuoi("MoTa"),
                    LaMacDinh = dong.SoNguyen("LaMacDinh") == 1
                });
        }
        catch { return new List<VaiTroDb>(); }
    }

    public List<VaiTroQuyen> LayMaTran()
    {
        try
        {
            return DanhSach(@"
                SELECT vtq.MaVaiTro, vtq.MaQuyen, ISNULL(vt.TenVaiTro,'') AS TenVaiTro,
                       ISNULL(q.TenQuyen,'') AS TenQuyen, ISNULL(q.NhomQuyen,'') AS NhomQuyen
                FROM VAI_TRO_QUYEN vtq
                LEFT JOIN VAI_TRO vt ON vt.MaVaiTro = vtq.MaVaiTro
                LEFT JOIN QUYEN q ON q.MaQuyen = vtq.MaQuyen
                ORDER BY vtq.MaVaiTro, q.NhomQuyen, vtq.MaQuyen",
                dong => new VaiTroQuyen
                {
                    MaVaiTro = dong.Chuoi("MaVaiTro"),
                    MaQuyen = dong.Chuoi("MaQuyen"),
                    TenVaiTro = dong.Chuoi("TenVaiTro"),
                    TenQuyen = dong.Chuoi("TenQuyen"),
                    NhomQuyen = dong.Chuoi("NhomQuyen")
                });
        }
        catch { return new List<VaiTroQuyen>(); }
    }

    public List<string> LayQuyenTheoVaiTro(string maVaiTro)
    {
        try
        {
            DataTable bang = TruyVan("SELECT MaQuyen FROM VAI_TRO_QUYEN WHERE MaVaiTro = @VaiTro",
                ThamSo("@VaiTro", maVaiTro));
            var ds = new List<string>();
            foreach (DataRow r in bang.Rows) ds.Add(r["MaQuyen"]?.ToString() ?? "");
            return ds.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
        }
        catch { return new List<string>(); }
    }

    public bool KiemTraQuyen(string maVaiTro, string maQuyen)
    {
        try
        {
            object kq = GiaTriDon("SELECT COUNT(1) FROM VAI_TRO_QUYEN WHERE MaVaiTro = @VaiTro AND MaQuyen = @Quyen",
                ThamSo("@VaiTro", maVaiTro), ThamSo("@Quyen", maQuyen));
            return kq != null && Convert.ToInt32(kq) > 0;
        }
        catch { return false; }
    }

    public void ThemQuyenVaoVaiTro(string maVaiTro, string maQuyen)
    {
        try
        {
            ThucThi("IF NOT EXISTS (SELECT 1 FROM VAI_TRO_QUYEN WHERE MaVaiTro = @VaiTro AND MaQuyen = @Quyen) INSERT INTO VAI_TRO_QUYEN (MaVaiTro, MaQuyen) VALUES (@VaiTro, @Quyen)",
                ThamSo("@VaiTro", maVaiTro), ThamSo("@Quyen", maQuyen));
        }
        catch { }
    }

    public void XoaQuyenKhoiVaiTro(string maVaiTro, string maQuyen)
    {
        try
        {
            ThucThi("DELETE FROM VAI_TRO_QUYEN WHERE MaVaiTro = @VaiTro AND MaQuyen = @Quyen",
                ThamSo("@VaiTro", maVaiTro), ThamSo("@Quyen", maQuyen));
        }
        catch { }
    }

    public void CapNhatMaTranVaiTro(string maVaiTro, IEnumerable<string> danhSachQuyen)
    {
        try
        {
            DbHelper.ChayGiaoDich(() =>
            {
                ThucThi("DELETE FROM VAI_TRO_QUYEN WHERE MaVaiTro = @VaiTro", ThamSo("@VaiTro", maVaiTro));
                foreach (string q in danhSachQuyen.Distinct())
                {
                    ThucThi("INSERT INTO VAI_TRO_QUYEN (MaVaiTro, MaQuyen) VALUES (@VaiTro, @Quyen)",
                        ThamSo("@VaiTro", maVaiTro), ThamSo("@Quyen", q));
                }
            });
        }
        catch { }
    }
}
