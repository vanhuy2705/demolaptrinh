using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Đổi mật khẩu cho chính người dùng đang đăng nhập (mọi vai trò đều được phép).</summary>
public partial class frmDoiMatKhau : Form
{
    public frmDoiMatKhau()
    {
        InitializeComponent();
    }

    private void frmDoiMatKhau_Load(object sender, EventArgs e)
    {
        GiaoDien.ApDung(this);
        StartPosition = FormStartPosition.CenterParent;
        lblNguoiDung.Text = $"{PhienLamViec.HoTen} ({VaiTro.TenHienThi(PhienLamViec.VaiTro)})";
        txtMatKhauCu.Select();
    }

    private async void btnXacNhan_Click(object sender, EventArgs e)
    {
        errLoi.Clear();
        bool loi = false;
        loi |= TroGiup.Rong(txtMatKhauCu, "mật khẩu hiện tại", errLoi);
        loi |= TroGiup.Sai(txtMatKhauMoi.Text.Length < 6, txtMatKhauMoi,
            "Mật khẩu mới phải có ít nhất 6 ký tự.", errLoi);
        loi |= TroGiup.Sai(txtMatKhauMoi.Text != txtXacNhan.Text, txtXacNhan,
            "Mật khẩu xác nhận không khớp.", errLoi);
        if (loi) return;

        int maTK = PhienLamViec.MaTK;
        string matKhauCu = txtMatKhauCu.Text, matKhauMoi = txtMatKhauMoi.Text, xacNhan = txtXacNhan.Text;

        Cursor = Cursors.WaitCursor;
        KetQua ketQua;
        try
        {
            ketQua = await Task.Run(() => ServiceFactory.Auth.DoiMatKhau(maTK, matKhauCu, matKhauMoi, xacNhan));
        }
        finally { Cursor = Cursors.Default; }
        if (IsDisposed) return;

        if (!ketQua.ThanhCong)
        {
            frmThongBao.HienThi(ketQua.ThongBao, "Không thể đổi mật khẩu", frmThongBao.LoaiThongBao.CanhBao, this);
            return;
        }

        frmThongBao.HienThi("Đổi mật khẩu thành công.", "Thành công", frmThongBao.LoaiThongBao.ThanhCong, this);
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnHuy_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
    {
        bool hien = chkHienMatKhau.Checked;
        txtMatKhauCu.UseSystemPasswordChar = !hien;
        txtMatKhauMoi.UseSystemPasswordChar = !hien;
        txtXacNhan.UseSystemPasswordChar = !hien;
    }
}
