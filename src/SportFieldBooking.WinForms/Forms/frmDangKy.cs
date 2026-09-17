using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Đăng ký tài khoản khách hàng (tự tạo luôn hồ sơ trong KHACH_HANG).</summary>
public partial class frmDangKy : Form
{
    public frmDangKy()
    {
        InitializeComponent();
    }

    private void frmDangKy_Load(object sender, EventArgs e)
    {
        GiaoDien.ApDung(this);
        StartPosition = FormStartPosition.CenterParent;
        txtHoTen.Select();
    }

    private async void btnDangKy_Click(object sender, EventArgs e)
    {
        if (!HopLe()) return;

        string tenDangNhap = txtTenDangNhap.Text.Trim(), matKhau = txtMatKhau.Text,
            xacNhan = txtXacNhanMatKhau.Text, hoTen = txtHoTen.Text.Trim(),
            sdt = txtSoDienThoai.Text.Trim(), email = txtEmail.Text.Trim(), diaChi = txtDiaChi.Text.Trim();

        Cursor = Cursors.WaitCursor;
        KetQua<TaiKhoan> ketQua;
        try
        {
            ketQua = await Task.Run(() => ServiceFactory.Auth.DangKy(
                tenDangNhap, matKhau, xacNhan, hoTen, sdt, email, diaChi));
        }
        finally { Cursor = Cursors.Default; }
        if (IsDisposed) return;

        if (!ketQua.ThanhCong)
        {
            frmThongBao.HienThi(ketQua.ThongBao, "Không thể đăng ký", frmThongBao.LoaiThongBao.CanhBao, this);
            return;
        }

        frmThongBao.HienThi("Đăng ký thành công! Bạn có thể đăng nhập ngay bây giờ.",
            "Đăng ký thành công", frmThongBao.LoaiThongBao.ThanhCong, this);
        DialogResult = DialogResult.OK;
        Close();
    }

    private bool HopLe()
    {
        errLoi.Clear();
        bool loi = false;
        loi |= TroGiup.Rong(txtHoTen, "họ tên", errLoi);
        loi |= TroGiup.SaiDienThoai(txtSoDienThoai, errLoi);
        loi |= TroGiup.SaiEmail(txtEmail, errLoi);
        loi |= TroGiup.Sai(txtTenDangNhap.Text.Trim().Length < 4, txtTenDangNhap,
            "Tên đăng nhập phải có ít nhất 4 ký tự.", errLoi);
        loi |= TroGiup.Sai(txtMatKhau.Text.Length < 6, txtMatKhau,
            "Mật khẩu phải có ít nhất 6 ký tự.", errLoi);
        loi |= TroGiup.Sai(txtMatKhau.Text != txtXacNhanMatKhau.Text, txtXacNhanMatKhau,
            "Mật khẩu xác nhận không khớp.", errLoi);
        return !loi;
    }

    private void btnHuy_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
    {
        bool hien = chkHienMatKhau.Checked;
        txtMatKhau.UseSystemPasswordChar = !hien;
        txtXacNhanMatKhau.UseSystemPasswordChar = !hien;
    }
}
