#nullable enable annotations
using System.Globalization;

namespace SportFieldBooking.WinForms.Helpers;

/// <summary>Định dạng DataGridView thống nhất, ưu tiên đọc được dữ liệu trên mọi kích thước.</summary>
public static class Luoi
{
    private static readonly Dictionary<DataGridView, List<Action>> _cauHinh = new();

    private static void Nho(DataGridView luoi, Action thucHien)
    {
        if (luoi == null) return;
        if (!_cauHinh.TryGetValue(luoi, out var ds))
        {
            ds = new List<Action>();
            _cauHinh[luoi] = ds;
        }
        ds.Add(thucHien);
        thucHien();
    }

    public static void ApDungLai(DataGridView luoi)
    {
        if (luoi == null || !_cauHinh.TryGetValue(luoi, out var ds)) return;
        foreach (Action a in ds.ToArray()) a();
    }

    public static void DoiMau(DataGridView luoi)
    {
        if (luoi == null) return;
        luoi.BackgroundColor = GiaoDien.BeMat;
        luoi.GridColor = GiaoDien.LuoiVien;
        luoi.ColumnHeadersDefaultCellStyle.BackColor = GiaoDien.LuoiTieuDe;
        luoi.ColumnHeadersDefaultCellStyle.ForeColor = GiaoDien.LuoiTieuDeChu;
        luoi.DefaultCellStyle.BackColor = GiaoDien.BeMat;
        luoi.DefaultCellStyle.ForeColor = GiaoDien.Chu;
        luoi.DefaultCellStyle.Font = GiaoDien.ChuThuong;
        luoi.DefaultCellStyle.SelectionBackColor = GiaoDien.LuoiChon;
        luoi.DefaultCellStyle.SelectionForeColor = GiaoDien.LuoiChonChu;
        luoi.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
        luoi.AlternatingRowsDefaultCellStyle.BackColor = GiaoDien.LuoiChan;
        luoi.AlternatingRowsDefaultCellStyle.SelectionBackColor = GiaoDien.LuoiChon;
        luoi.AlternatingRowsDefaultCellStyle.SelectionForeColor = GiaoDien.LuoiChonChu;
    }

    public static void Dang(DataGridView luoi)
    {
        if (luoi == null) return;
        luoi.AllowUserToAddRows = false;
        luoi.AllowUserToDeleteRows = false;
        luoi.AllowUserToResizeRows = false;
        luoi.ReadOnly = true;
        luoi.MultiSelect = false;
        luoi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        luoi.BackgroundColor = GiaoDien.BeMat;
        luoi.BorderStyle = BorderStyle.None;
        luoi.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        luoi.GridColor = GiaoDien.LuoiVien;
        luoi.RowHeadersVisible = false;
        luoi.EnableHeadersVisualStyles = false;
        luoi.RowTemplate.Height = 38;
        luoi.ColumnHeadersHeight = 44;
        luoi.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        luoi.ScrollBars = ScrollBars.Both;
        luoi.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        luoi.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

        luoi.DataBindingComplete -= Luoi_DataBindingComplete;
        luoi.DataBindingComplete += Luoi_DataBindingComplete;
        ApDungMau(luoi);
    }

    private static void Luoi_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        if (sender is not DataGridView luoi) return;
        ApDungLai(luoi);
        CanBangCot(luoi);
    }

    private static void ApDungMau(DataGridView luoi)
    {
        luoi.ColumnHeadersDefaultCellStyle.BackColor = GiaoDien.LuoiTieuDe;
        luoi.ColumnHeadersDefaultCellStyle.ForeColor = GiaoDien.LuoiTieuDeChu;
        luoi.ColumnHeadersDefaultCellStyle.Font = GiaoDien.ChuDam;
        luoi.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        luoi.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 4, 0);
        luoi.DefaultCellStyle.BackColor = GiaoDien.BeMat;
        luoi.DefaultCellStyle.ForeColor = GiaoDien.Chu;
        luoi.DefaultCellStyle.Font = GiaoDien.ChuThuong;
        luoi.DefaultCellStyle.SelectionBackColor = GiaoDien.LuoiChon;
        luoi.DefaultCellStyle.SelectionForeColor = GiaoDien.LuoiChonChu;
        luoi.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
        luoi.AlternatingRowsDefaultCellStyle.BackColor = GiaoDien.LuoiChan;
        luoi.AlternatingRowsDefaultCellStyle.SelectionBackColor = GiaoDien.LuoiChon;
        luoi.AlternatingRowsDefaultCellStyle.SelectionForeColor = GiaoDien.LuoiChonChu;
    }

    private static void CanBangCot(DataGridView luoi)
    {
        if (luoi.Columns.Count == 0) return;

        int visible = luoi.Columns.Cast<DataGridViewColumn>().Count(c => c.Visible);
        if (visible <= 8)
        {
            luoi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            foreach (DataGridViewColumn c in luoi.Columns)
                if (c.Visible) c.MinimumWidth = 80;
            return;
        }

        luoi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        foreach (DataGridViewColumn c in luoi.Columns)
        {
            if (!c.Visible) continue;
            int headerWidth = TextRenderer.MeasureText(c.HeaderText ?? string.Empty, GiaoDien.ChuDam).Width + 30;
            int cellWidth = 0;
            if (luoi.Rows.Count > 0)
            {
                int sample = Math.Min(8, luoi.Rows.Count);
                for (int i = 0; i < sample; i++)
                {
                    try
                    {
                        object value = luoi.Rows[i].Cells[c.Index].Value;
                        cellWidth = Math.Max(cellWidth, TextRenderer.MeasureText(value?.ToString() ?? string.Empty, GiaoDien.ChuThuong).Width + 28);
                    }
                    catch { /* ignore */ }
                }
            }
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.MinimumWidth = 82;
            c.Width = Math.Clamp(Math.Max(headerWidth, cellWidth), 92, 210);
        }
    }

    public static void DatTieuDe(DataGridView luoi, params (string ThuocTinh, string TieuDe)[] danhSach)
    {
        Nho(luoi, () =>
        {
            foreach (var (thuocTinh, tieuDe) in danhSach)
                if (luoi.Columns.Contains(thuocTinh)) luoi.Columns[thuocTinh].HeaderText = tieuDe;
        });
    }

    public static void AnCot(DataGridView luoi, params string[] tenCot)
    {
        Nho(luoi, () =>
        {
            foreach (string cot in tenCot)
                if (luoi.Columns.Contains(cot)) luoi.Columns[cot].Visible = false;
        });
    }

    public static void DatDoRong(DataGridView luoi, string tenCot, int doRong)
    {
        Nho(luoi, () =>
        {
            if (!luoi.Columns.Contains(tenCot)) return;
            luoi.Columns[tenCot].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            luoi.Columns[tenCot].Width = Math.Max(60, doRong);
        });
    }

    public static void DatDinhDangTien(DataGridView luoi, params string[] tenCot)
    {
        Nho(luoi, () =>
        {
            foreach (string cot in tenCot)
            {
                if (!luoi.Columns.Contains(cot)) continue;
                luoi.Columns[cot].DefaultCellStyle.Format = "N0";
                luoi.Columns[cot].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                luoi.Columns[cot].DefaultCellStyle.Font = GiaoDien.ChuDam;
            }
        });
    }

    public static void DatDinhDangNgay(DataGridView luoi, string dinhDang, params string[] tenCot)
    {
        Nho(luoi, () =>
        {
            foreach (string cot in tenCot)
                if (luoi.Columns.Contains(cot)) luoi.Columns[cot].DefaultCellStyle.Format = dinhDang;
        });
    }

    public static void ToMauTrangThai(DataGridView luoi, string tenCot)
    {
        luoi.CellFormatting -= TrangThaiFormatting;
        luoi.CellFormatting += TrangThaiFormatting;
        luoi.Tag = $"STATUS:{tenCot}";
    }

    private static void TrangThaiFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (sender is not DataGridView luoi || e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (luoi.Tag is not string tag || !tag.StartsWith("STATUS:", StringComparison.Ordinal)) return;
        string cot = tag[7..];
        if (luoi.Columns[e.ColumnIndex].Name != cot) return;
        string giaTri = e.Value?.ToString() ?? "";
        e.CellStyle.ForeColor = GiaoDien.MauTrangThai(giaTri);
        e.CellStyle.Font = GiaoDien.ChuDam;
        e.CellStyle.SelectionForeColor = GiaoDien.MauTrangThai(giaTri);
    }

    public static void HienThiTrangThai(DataGridView luoi, string tenCot, Func<string, string> chuyenDoi)
    {
        luoi.CellFormatting += (_, e) =>
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (luoi.Columns[e.ColumnIndex].Name != tenCot) return;
            e.Value = chuyenDoi(e.Value?.ToString() ?? "");
            e.FormattingApplied = true;
        };
    }

    public static int LayMaDangChon(DataGridView luoi, string tenCot = "MaDat", int macDinh = 0)
    {
        try
        {
            if (luoi == null || luoi.CurrentRow == null) return macDinh;
            if (luoi.CurrentRow.DataBoundItem == null) return macDinh;
            if (!luoi.Columns.Contains(tenCot)) return macDinh;
            var cell = luoi.CurrentRow.Cells[tenCot];
            if (cell == null) return macDinh;
            object giaTri = cell.Value;
            if (giaTri == null || giaTri == DBNull.Value) return macDinh;
            return Convert.ToInt32(giaTri);
        }
        catch
        {
            return macDinh;
        }
    }

    public static T? LayDongDangChon<T>(DataGridView luoi) where T : class
    {
        try
        {
            return luoi?.CurrentRow?.DataBoundItem as T;
        }
        catch
        {
            return null;
        }
    }

    public static void GanDuLieu<T>(DataGridView luoi, IList<T> danhSach)
    {
        if (luoi == null) return;
        try
        {
            int chiSo = luoi.CurrentRow?.Index ?? 0;
            luoi.DataSource = null;
            luoi.DataSource = danhSach ?? new List<T>();
            if (luoi.Rows.Count == 0 || luoi.Columns.Count == 0) return;

            DataGridViewColumn cotHienThi = luoi.Columns.Cast<DataGridViewColumn>()
                .OrderBy(c => c.DisplayIndex).FirstOrDefault(c => c.Visible);
            if (cotHienThi == null) return;
            try
            {
                luoi.CurrentCell = luoi.Rows[Math.Min(chiSo, luoi.Rows.Count - 1)].Cells[cotHienThi.Index];
            }
            catch (InvalidOperationException) { }
            catch (ArgumentException) { }
        }
        catch
        {
            // Không để lỗi lưới chặn toàn bộ form
            try { luoi.DataSource = danhSach ?? new List<T>(); } catch { }
        }
    }

    public static string Tien(decimal soTien) => soTien.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";
}
