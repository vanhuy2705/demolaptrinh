using SportFieldBooking.Business.Common;
using SportFieldBooking.WinForms.Forms;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Base;

/// <summary>
/// Lớp cơ sở cho 3 màn hình chính (Admin/Nhân viên/Khách hàng):
/// chứa sẵn logic mở Form con bên trong khung nội dung và đăng xuất.
/// </summary>
public partial class BaseMainForm : BaseForm
{
    private Form _formConHienTai;

    public BaseMainForm()
    {
        InitializeComponent();
    }

    /// <summary>Mở một Form con bên trong khung nội dung (giao diện dạng một cửa sổ).</summary>
    protected void MoFormCon(Form formCon, Panel khungNoiDung)
    {
        if (khungNoiDung == null || formCon == null) return;

        try
        {
            if (_formConHienTai != null)
            {
                _formConHienTai.Close();
                _formConHienTai.Dispose();
                _formConHienTai = null;
            }

            khungNoiDung.Controls.Clear();
            formCon.TopLevel = false;
            formCon.FormBorderStyle = FormBorderStyle.None;
            formCon.Dock = DockStyle.Fill;
            khungNoiDung.Controls.Add(formCon);
            formCon.BringToFront();
            formCon.Show();
            _formConHienTai = formCon;
        }
        catch (Exception ex)
        {
            BaoLoi("Không thể mở chức năng", ex);
        }
    }

    /// <summary>Đăng xuất và quay về màn hình đăng nhập.</summary>
    protected void DangXuat()
    {
        if (!XacNhan("Bạn có chắc muốn đăng xuất khỏi hệ thống?", "Đăng xuất")) return;

        ServiceFactory.Auth.DangXuat();
        DialogResult = DialogResult.Abort;
        Close();
    }

    /// <summary>Đặt lại màu cho các nút menu theo nút đang được chọn.</summary>
    protected void DanhDauMenuDangChon(SportFieldBooking.WinForms.Controls.SidebarButton nutDangChon,
        params SportFieldBooking.WinForms.Controls.SidebarButton[] tatCaNut)
    {
        foreach (var nut in tatCaNut)
            if (nut != null) nut.KichHoat = nut == nutDangChon;
    }
}
