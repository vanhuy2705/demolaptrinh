using SportFieldBooking.Business.Common;
using SportFieldBooking.Business.Services;
using SportFieldBooking.WinForms.Forms;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Base;

/// <summary>
/// Lớp cơ sở cho mọi Form: áp dụng giao diện, kiểm tra quyền khi mở chức năng
/// (không chỉ ẩn nút) và cung cấp sẵn các hàm thông báo/xử lý lỗi thân thiện.
/// Các Form con chỉ cần override ThietLapGiaoDien/TaiDuLieu/CapNhatNut.
/// </summary>
public partial class BaseForm : Form
{
    /// <summary>Mã quyền tối thiểu để mở Form (null = không yêu cầu quyền riêng).</summary>
    protected virtual string MaQuyenYeuCau => null;

    private bool _daKhoiDong;

    public BaseForm()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (_daKhoiDong) return;
        _daKhoiDong = true;

        GiaoDien.ApDung(this);
        ResponsiveLayout.ApDung(this);

        if (!CoQuyenMoForm())
        {
            VoHieuHoa($"Bạn không có quyền sử dụng chức năng {PhanQuyenService.TenChucNang(MaQuyenYeuCau)}.\nVui lòng liên hệ quản trị viên.");
            return;
        }

        try
        {
            ThietLapGiaoDien();
            TaiDuLieu();
            CapNhatTrangThaiNut();
        }
        catch (Exception ex)
        {
            BaoLoi("Không thể tải dữ liệu", ex);
        }
    }

    /// <summary>Thiết lập dữ liệu cho combobox, trạng thái ban đầu...</summary>
    protected virtual void ThietLapGiaoDien() { }

    /// <summary>Nạp dữ liệu chính của Form.</summary>
    protected virtual void TaiDuLieu() { }

    /// <summary>Bật/tắt nút theo trạng thái hiện tại.</summary>
    protected virtual void CapNhatTrangThaiNut() { }

    protected bool CoQuyenMoForm() =>
        string.IsNullOrWhiteSpace(MaQuyenYeuCau) || PhanQuyenService.CoQuyen(MaQuyenYeuCau);

    /// <summary>Kiểm tra quyền thao tác (dùng cho từng nút) - trả về false và thông báo nếu không được phép.</summary>
    protected bool CoQuyen(string maQuyen)
    {
        if (PhanQuyenService.CoQuyen(maQuyen)) return true;
        CanhBao($"Bạn không có quyền thực hiện thao tác này ({PhanQuyenService.TenChucNang(maQuyen)}).", "Không đủ quyền");
        return false;
    }

    protected void VoHieuHoa(string lyDo)
    {
        foreach (Control con in Controls) VoHieuHoaControl(con);

        var nhan = new Label
        {
            Dock = DockStyle.Fill,
            Text = lyDo,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = GiaoDien.NguyHiem,
            Font = GiaoDien.ChuLon,
            BackColor = GiaoDien.ManHinhNen
        };
        Controls.Add(nhan);
        nhan.BringToFront();
    }

    private static void VoHieuHoaControl(Control o)
    {
        o.Enabled = false;
        foreach (Control con in o.Controls) VoHieuHoaControl(con);
    }

    // --- Thông báo ---
    protected void ThanhCong(string noiDung, string tieuDe = "Thành công") =>
        frmThongBao.HienThi(noiDung, tieuDe,frmThongBao.LoaiThongBao.ThanhCong, this);

    protected void ThongTin(string noiDung, string tieuDe = "Thông báo") =>
        frmThongBao.HienThi(noiDung, tieuDe, frmThongBao.LoaiThongBao.ThongTin, this);

    protected void CanhBao(string noiDung, string tieuDe = "Cảnh báo") =>
        frmThongBao.HienThi(noiDung, tieuDe, frmThongBao.LoaiThongBao.CanhBao, this);

    protected void BaoLoi(string tieuDe, Exception ex) =>
        frmThongBao.HienThi(RutGonLoi(ex), tieuDe, frmThongBao.LoaiThongBao.Loi, this);

    protected bool XacNhan(string cauHoi, string tieuDe = "Xác nhận") =>
        frmThongBao.Hoi(cauHoi, tieuDe, this);

    /// <summary>Chạy một thao tác có khả năng lỗi: tự try/catch và hiển thị thông báo thân thiện.</summary>
    protected void ThucHien(Action thaoTac, string tieuDeLoi = "Thao tác thất bại", string thongBaoThanhCong = "")
    {
        try
        {
            thaoTac();
            if (!string.IsNullOrWhiteSpace(thongBaoThanhCong)) ThanhCong(thongBaoThanhCong);
        }
        catch (Exception ex)
        {
            BaoLoi(tieuDeLoi, ex);
        }
    }

    /// <summary>Chạy thao tác trả về KetQua: hiện thông báo theo kết quả nghiệp vụ.</summary>
    protected bool ThucHien(KetQua ketQua, string thongBaoThanhCong = "")
    {
        if (ketQua == null) return false;
        if (ketQua.ThanhCong)
        {
            ThanhCong(string.IsNullOrWhiteSpace(thongBaoThanhCong) ? ketQua.ThongBao : thongBaoThanhCong);
            return true;
        }
        CanhBao(ketQua.ThongBao, "Không thể thực hiện");
        return false;
    }

    private static string RutGonLoi(Exception ex)
    {
        string thongDiep = ex.InnerException?.Message ?? ex.Message;
        if (thongDiep.Contains("A network-related") || thongDiep.Contains("error occurred while establishing"))
            return "Không kết nối được cơ sở dữ liệu. Vui lòng kiểm tra SQL Server và chuỗi kết nối trong appsettings.json.";
        if (thongDiep.Contains("Login failed"))
            return "Sai thông tin đăng nhập SQL Server. Vui lòng kiểm tra lại chuỗi kết nối.";
        if (thongDiep.Contains("Cannot open database"))
            return "Không mở được cơ sở dữ liệu QLSanTheThao. Hãy chạy script tạo CSDL trước.";
        return thongDiep;
    }
}
