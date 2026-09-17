using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.WinForms.Forms;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Màn hình đăng nhập: gọi AuthService, mở màn hình tương ứng với vai trò.</summary>
public partial class frmDangNhap : Form
{
    public frmDangNhap()
    {
        InitializeComponent();
    }

    private void frmDangNhap_Load(object sender, EventArgs e)
    {
        GiaoDien.ApDung(this);
        ResponsiveLayout.ApDung(this);
        StartPosition = FormStartPosition.CenterScreen;
        lblPhienBan.Text = "Phiên bản 1.0  |  .NET 10 Windows Forms";
        txtTenDangNhap.Select();
    }

    private void btnDangNhap_Click(object sender, EventArgs e)
    {
        string tenDangNhap = txtTenDangNhap.Text.Trim();
        string matKhau = txtMatKhau.Text;

        if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(matKhau))
        {
            frmThongBao.HienThi("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.", "Thiếu thông tin",
                frmThongBao.LoaiThongBao.CanhBao, this);
            return;
        }

        btnDangNhap.Enabled = false;
        try
        {
            KetQua<TaiKhoan> ketQua = ServiceFactory.Auth.DangNhap(tenDangNhap, matKhau);
            if (!ketQua.ThanhCong)
            {
                frmThongBao.HienThi(ketQua.ThongBao, "Đăng nhập thất bại", frmThongBao.LoaiThongBao.Loi, this);
                txtMatKhau.Clear();
                txtMatKhau.Select();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            frmThongBao.HienThi("Lỗi đăng nhập: " + ex.Message, "Lỗi", frmThongBao.LoaiThongBao.Loi, this);
        }
        finally
        {
            btnDangNhap.Enabled = true;
        }
    }

    private void btnDangKy_Click(object sender, EventArgs e)
    {
        using var dangKy = new frmDangKy();
        dangKy.ShowDialog(this);
    }

    private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e) =>
        txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;

    private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) btnDangNhap_Click(sender, e);
    }

    private void txtTenDangNhap_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) txtMatKhau.Select();
    }
}
