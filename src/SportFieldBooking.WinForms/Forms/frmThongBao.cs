using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>
/// Hộp thoại thông báo dùng chung (thay cho MessageBox mặc định) để giao diện đồng bộ.
/// </summary>
public partial class frmThongBao : Form
{
    public enum LoaiThongBao
    {
        ThongTin,
        ThanhCong,
        CanhBao,
        Loi
    }

    public frmThongBao()
    {
        InitializeComponent();
    }

    private void DatNoiDung(string thongDiep, string tieuDe, LoaiThongBao loai, bool coNutHuy)
    {
        lblTieuDe.Text = tieuDe;
        lblThongDiep.Text = thongDiep;

        (Color mau, string tenBieuTuong) = loai switch
        {
            LoaiThongBao.ThanhCong => (GiaoDien.ThanhCong, "check"),
            LoaiThongBao.CanhBao => (GiaoDien.CanhBao, "warning"),
            LoaiThongBao.Loi => (GiaoDien.NguyHiem, "warning"),
            _ => (GiaoDien.ThongTin, "note")
        };

        picBieuTuong.MauBieuTuong = mau;
        picBieuTuong.TenBieuTuong = tenBieuTuong;
        lblTieuDe.ForeColor = mau;

        btnHuy.Visible = coNutHuy;
        btnDongY.Text = coNutHuy ? "Đồng ý" : "Đóng";
        AcceptButton = coNutHuy ? btnDongY : btnDongY;
    }

    public static void HienThi(string thongDiep, string tieuDe = "Thông báo",
        LoaiThongBao loai = LoaiThongBao.ThongTin, IWin32Window chuSoHuu = null)
    {
        using var hopThoai = new frmThongBao();
        hopThoai.DatNoiDung(thongDiep, tieuDe, loai, false);
        if (chuSoHuu == null) hopThoai.ShowDialog();
        else hopThoai.ShowDialog(chuSoHuu);
    }

    public static bool Hoi(string cauHoi, string tieuDe = "Xác nhận", IWin32Window chuSoHuu = null)
    {
        using var hopThoai = new frmThongBao();
        hopThoai.DatNoiDung(cauHoi, tieuDe, LoaiThongBao.CanhBao, true);
        DialogResult ketQua = chuSoHuu == null ? hopThoai.ShowDialog() : hopThoai.ShowDialog(chuSoHuu);
        return ketQua == DialogResult.Yes;
    }

    private void btnDongY_Click(object sender, EventArgs e)
    {
        DialogResult = btnHuy.Visible ? DialogResult.Yes : DialogResult.OK;
        Close();
    }

    private void btnHuy_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.No;
        Close();
    }
}
