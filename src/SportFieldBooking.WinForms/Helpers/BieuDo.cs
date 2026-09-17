using ScottPlot;
using ScottPlot.WinForms;

namespace SportFieldBooking.WinForms.Helpers;

/// <summary>
/// Vẽ biểu đồ bằng ScottPlot (MIT) cho Dashboard và Thống kê.
/// Tất cả thao tác vẽ tập trung ở đây để các Form chỉ cần truyền dữ liệu.
/// </summary>
public static class BieuDo
{
    private const string MauMacDinh = "#2563EB";

    /// <summary>Bảng màu dữ liệu - chủ đề Sáng.</summary>
    private static readonly string[] BangMauSang =
    {
        "#2563EB", "#16A34A", "#D97706", "#7C3AED", "#DC2626", "#0891B2", "#DB2777"
    };

    /// <summary>Bảng màu dữ liệu - chủ đề Tối (azure chủ đạo, tương phản trên nền #11161D).</summary>
    private static readonly string[] BangMauToi =
    {
        "#4C8DFF", "#34D399", "#FBBF24", "#A78BFA", "#F87171", "#22D3EE", "#F472B6"
    };

    private static string[] BangMau => GiaoDien.LaThemeToi ? BangMauToi : BangMauSang;
    private static string MauChinh => GiaoDien.LaThemeToi ? "#4C8DFF" : MauMacDinh;

    private static string Hex(System.Drawing.Color mau) => ColorTranslator.ToHtml(mau);

    /// <summary>Đổi màu nền/lưới/chữ của biểu đồ theo chủ đề hiện tại (tránh biểu đồ trắng trên nền tối).</summary>
    private static void ApDungChuDe(FormsPlot bieuDo)
    {
        if (bieuDo == null) return;

        // Lấy màu trực tiếp từ bảng màu của ứng dụng để biểu đồ luôn đồng bộ chủ đề.
        var bNenHinh = ScottPlot.Color.FromHex(Hex(GiaoDien.BeMat));
        var bNenDuLieu = ScottPlot.Color.FromHex(Hex(GiaoDien.LaThemeToi ? GiaoDien.ManHinhNen : System.Drawing.Color.White));
        var bChu = ScottPlot.Color.FromHex(Hex(GiaoDien.Chu));
        var bLuoi = ScottPlot.Color.FromHex(Hex(GiaoDien.LuoiVien));

        bieuDo.Plot.FigureBackground.Color = bNenHinh;
        bieuDo.Plot.DataBackground.Color = bNenDuLieu;
        bieuDo.Plot.Axes.Color(bChu);
        bieuDo.Plot.Grid.MajorLineColor = bLuoi;
        bieuDo.Plot.Legend.FontColor = bChu;
        bieuDo.Plot.Legend.BackgroundColor = bNenDuLieu;
        bieuDo.Plot.Legend.OutlineColor = bLuoi;
        bieuDo.Plot.Axes.Title.Label.ForeColor = bChu;
    }

    /// <summary>Biểu đồ cột (doanh thu theo ngày/tháng, số booking...).</summary>
    public static void VeCot(FormsPlot bieuDo, IList<string> nhan, IList<double> giaTri,
        string tenDay = "Giá trị", string mauHex = "", string tieuDe = "", string nhanTrucY = "")
    {
        if (bieuDo == null) return;
        bieuDo.Plot.Clear();

        if (nhan == null || nhan.Count == 0 || giaTri == null || giaTri.Count == 0)
        {
            bieuDo.Plot.Title("Chưa có dữ liệu trong khoảng thời gian này");
            ApDungChuDe(bieuDo);
            bieuDo.Refresh();
            return;
        }

        double[] viTri = Enumerable.Range(0, nhan.Count).Select(i => (double)i).ToArray();
        var cot = bieuDo.Plot.Add.Bars(viTri, giaTri.ToArray());
        cot.Color = ScottPlot.Color.FromHex((string.IsNullOrWhiteSpace(mauHex) ? MauChinh : mauHex));
        cot.LegendText = tenDay;

        bieuDo.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(viTri, nhan.ToArray());
        if (!string.IsNullOrWhiteSpace(tieuDe)) bieuDo.Plot.Title(tieuDe);
        if (!string.IsNullOrWhiteSpace(nhanTrucY)) bieuDo.Plot.Axes.Left.Label.Text = nhanTrucY;
        bieuDo.Plot.ShowLegend();
        ApDungChuDe(bieuDo);
        bieuDo.Refresh();
    }

    /// <summary>Biểu đồ đường (xu hướng doanh thu).</summary>
    public static void VeDuong(FormsPlot bieuDo, IList<string> nhan, IList<double> giaTri,
        string tenDay = "Doanh thu", string mauHex = "", string tieuDe = "")
    {
        if (bieuDo == null) return;
        bieuDo.Plot.Clear();

        if (nhan == null || nhan.Count == 0 || giaTri == null || giaTri.Count == 0)
        {
            bieuDo.Plot.Title("Chưa có dữ liệu trong khoảng thời gian này");
            ApDungChuDe(bieuDo);
            bieuDo.Refresh();
            return;
        }

        double[] viTri = Enumerable.Range(0, nhan.Count).Select(i => (double)i).ToArray();
        var duong = bieuDo.Plot.Add.Scatter(viTri, giaTri.ToArray());
        duong.LineWidth = 3;
        duong.MarkerSize = 6;
        duong.Color = ScottPlot.Color.FromHex((string.IsNullOrWhiteSpace(mauHex) ? MauChinh : mauHex));
        duong.LegendText = tenDay;

        bieuDo.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(viTri, nhan.ToArray());
        if (!string.IsNullOrWhiteSpace(tieuDe)) bieuDo.Plot.Title(tieuDe);
        bieuDo.Plot.ShowLegend();
        ApDungChuDe(bieuDo);
        bieuDo.Refresh();
    }

    /// <summary>Biểu đồ tròn (tỷ lệ loại giảm giá, cơ cấu doanh thu...).</summary>
    public static void VeTron(FormsPlot bieuDo, IList<string> nhan, IList<double> giaTri, string tieuDe = "")
    {
        if (bieuDo == null) return;
        bieuDo.Plot.Clear();

        if (nhan == null || giaTri == null || nhan.Count == 0 || giaTri.Sum() <= 0)
        {
            bieuDo.Plot.Title("Chưa có dữ liệu trong khoảng thời gian này");
            ApDungChuDe(bieuDo);
            bieuDo.Refresh();
            return;
        }

        var hinhTron = bieuDo.Plot.Add.Pie(giaTri.ToArray());
        for (int i = 0; i < hinhTron.Slices.Count && i < nhan.Count; i++)
        {
            hinhTron.Slices[i].Label = nhan[i];
            hinhTron.Slices[i].FillColor = ScottPlot.Color.FromHex(BangMau[i % BangMau.Length]);
            hinhTron.Slices[i].LabelFontColor = ScottPlot.Color.FromHex(Hex(GiaoDien.LaThemeToi ? GiaoDien.ChuTrenNenDam : System.Drawing.Color.White));
        }

        if (!string.IsNullOrWhiteSpace(tieuDe)) bieuDo.Plot.Title(tieuDe);
        bieuDo.Plot.Legend.IsVisible = true;
        ApDungChuDe(bieuDo);
        bieuDo.Refresh();
    }

    public static void XoaTrang(FormsPlot bieuDo)
    {
        if (bieuDo == null) return;
        bieuDo.Plot.Clear();
        ApDungChuDe(bieuDo);
        bieuDo.Refresh();
    }
}
