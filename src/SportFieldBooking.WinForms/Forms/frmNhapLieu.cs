using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Hộp thoại nhập một dòng dữ liệu (đặt lại mật khẩu, nhập lý do hủy...).</summary>
public partial class frmNhapLieu : Form
{
    public frmNhapLieu()
    {
        InitializeComponent();
    }

    /// <summary>Giá trị người dùng đã nhập (null nếu hủy; chuỗi rỗng nếu cho phép bỏ trống).</summary>
    public string GiaTri { get; private set; }

    private bool _batBuocNhap = true;

    public static string NhapChuoi(string tieuDe, string nhanLoi, string giaTriMacDinh = "",
        bool cheDoMatKhau = false, IWin32Window chuSoHuu = null, bool batBuocNhap = true)
    {
        using var hopThoai = new frmNhapLieu
        {
            Text = tieuDe
        };
        hopThoai.lblTieuDe.Text = tieuDe;
        hopThoai.lblNhanLoi.Text = nhanLoi;
        hopThoai.txtGiaTri.Text = giaTriMacDinh;
        hopThoai.txtGiaTri.UseSystemPasswordChar = cheDoMatKhau;
        hopThoai._batBuocNhap = batBuocNhap;

        DialogResult ketQua = chuSoHuu == null ? hopThoai.ShowDialog() : hopThoai.ShowDialog(chuSoHuu);
        return ketQua == DialogResult.OK ? hopThoai.GiaTri : null;
    }

    private void btnDongY_Click(object sender, EventArgs e)
    {
        if (_batBuocNhap && string.IsNullOrWhiteSpace(txtGiaTri.Text))
        {
            frmThongBao.HienThi("Vui lòng nhập giá trị.", "Thiếu dữ liệu", frmThongBao.LoaiThongBao.CanhBao, this);
            txtGiaTri.Select();
            return;
        }
        GiaTri = txtGiaTri.Text.Trim();
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnHuy_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void txtGiaTri_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) btnDongY_Click(sender, e);
    }
}
