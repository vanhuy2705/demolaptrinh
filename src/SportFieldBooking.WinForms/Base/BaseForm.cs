#nullable enable annotations
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
        }
        catch (Exception ex)
        {
            BaoLoi("Không thể thiết lập giao diện", ex);
        }

        // Nạp dữ liệu BẤT ĐỒNG BỘ để không đứng hình giao diện.
        _ = KhoiDongDuLieuAsync();
    }

    /// <summary>Nạp dữ liệu nền rồi cập nhật nút; bắt lỗi tập trung.</summary>
    private async Task KhoiDongDuLieuAsync()
    {
        BatDauBan();
        try
        {
            await TaiDuLieuAsync();
            CapNhatTrangThaiNut();
        }
        catch (Exception ex)
        {
            BaoLoi("Không thể tải dữ liệu", ex);
        }
        finally
        {
            KetThucBan();
        }
    }

    /// <summary>
    /// Hook nạp dữ liệu bất đồng bộ. Mặc định chạy <see cref="TaiDuLieu"/> đồng bộ
    /// (giữ nguyên hành vi các form chưa chuyển đổi); form nào nặng nên override
    /// và đẩy phần truy vấn xuống nền bằng <see cref="ChayNenAsync{T}"/>.
    /// </summary>
    protected virtual Task TaiDuLieuAsync()
    {
        TaiDuLieu();
        return Task.CompletedTask;
    }

    /// <summary>Chạy một hàm nghiệp vụ (không đụng UI) trên thread nền.</summary>
    protected static Task<T> ChayNenAsync<T>(Func<T> ham) => Task.Run(ham);

    /// <summary>Chạy thao tác không đụng UI trên thread nền.</summary>
    protected static Task ChayNenAsync(Action thaoTac) => Task.Run(thaoTac);

    /// <summary>
    /// Chạy nghiệp vụ dưới nền rồi hiển thị kết quả trên luồng UI (toast nếu thành công).
    /// Trả về true nếu thành công. Không đứng hình cửa sổ trong lúc chờ.
    /// </summary>
    protected async Task<bool> ThucHienAsync(Func<KetQua> thaoTac, string thongBaoThanhCong = "")
    {
        BatDauBan();
        try
        {
            KetQua ketQua = await Task.Run(thaoTac);
            return ThucHien(ketQua, thongBaoThanhCong);
        }
        catch (Exception ex)
        {
            BaoLoi("Thao tác thất bại", ex);
            return false;
        }
        finally
        {
            KetThucBan();
        }
    }

    // --- Lớp phủ "đang tải" ---
    private Panel? _lopPhu;
    private int _soLuotBan;

    /// <summary>Phủ một lớp mờ + chữ "Đang tải…" để chặn thao tác lặp trong lúc chờ.</summary>
    protected void BatDauBan()
    {
        _soLuotBan++;
        if (_lopPhu != null || IsDisposed) return;
        _lopPhu = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(120, GiaoDien.ManHinhNen),
            Cursor = Cursors.WaitCursor
        };
        var nhan = new Label
        {
            Text = "Đang tải…",
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill,
            ForeColor = GiaoDien.ChuPhu,
            Font = GiaoDien.ChuDam,
            BackColor = Color.Transparent
        };
        _lopPhu.Controls.Add(nhan);
        Controls.Add(_lopPhu);
        _lopPhu.BringToFront();
    }

    protected void KetThucBan()
    {
        if (_soLuotBan > 0) _soLuotBan--;
        if (_lopPhu == null || _soLuotBan > 0) return;
        var phu = _lopPhu;
        _lopPhu = null;
        Controls.Remove(phu);
        phu.Dispose();
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
    // Thành công dùng TOAST không chặn để thao tác liền mạch;
    // cảnh báo / lỗi / xác nhận vẫn dùng hộp thoại modal vì cần người dùng chú ý.
    protected void ThanhCong(string noiDung, string tieuDe = "Thành công") =>
        SportFieldBooking.WinForms.Controls.Toast.Hien(noiDung,
            SportFieldBooking.WinForms.Controls.Toast.Loai.ThanhCong);

    protected void ThongTin(string noiDung, string tieuDe = "Thông báo") =>
        SportFieldBooking.WinForms.Controls.Toast.Hien(noiDung,
            SportFieldBooking.WinForms.Controls.Toast.Loai.ThongTin);

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
