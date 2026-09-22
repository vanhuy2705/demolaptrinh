using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Cấu hình tham số hệ thống: % giảm cuối tuần, block thời gian, giờ mở/đóng cửa, thông tin trung tâm.</summary>
public partial class frmCauHinh : BaseForm
{
    private List<ThamSo> _danhSach = new();

    public frmCauHinh()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.CauHinhXem;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvThamSo);
        Luoi.DatTieuDe(dgvThamSo,
            ("TenThamSo", "Tên tham số"),
            ("GiaTri", "Giá trị"),
            ("MoTa", "Mô tả"));
        Luoi.DatDoRong(dgvThamSo, "TenThamSo", 220);
        Luoi.DatDoRong(dgvThamSo, "GiaTri", 200);

        lblGhiChu.Text = "Hướng dẫn:\n" +
            "- PhanTramGiamCuoiTuan: % giảm T7/CN (0-100)\n" +
            "- ThoiLuongBlockPhut: block tính tiền, mặc định 30 phút\n" +
            "- GioMoCua / GioDongCua: HH:mm (vd 05:00)\n" +
            "- ThoiGianHuyToiDaGio: số giờ tối thiểu trước giờ chơi để khách được tự hủy (0-720)\n" +
            "- SoNgayDatTruoc: số ngày tối đa cho phép đặt trước (1-365)\n" +
            "- TenTrungTam / DiaChi / DienThoai / LoiChaoHoaDon: thông tin in hóa đơn\n" +
            "Voucher luôn ưu tiên hơn khuyến mãi và giảm cuối tuần.";
    }

    protected override async Task TaiDuLieuAsync()
    {
        BatDauBan();
        try
        {
            _danhSach = await ChayNenAsync(() => ServiceFactory.CauHinh.LayTatCa());
            Luoi.GanDuLieu(dgvThamSo, _danhSach);
            if (dgvThamSo.Columns.Contains("TenThamSo")) dgvThamSo.Columns["TenThamSo"].ReadOnly = true;
            if (dgvThamSo.Columns.Contains("MoTa")) dgvThamSo.Columns["MoTa"].ReadOnly = true;
            CapNhatTrangThaiNut();
        }
        catch (Exception ex) { BaoLoi("Không thể tải cấu hình hệ thống", ex); }
        finally { KetThucBan(); }
    }

    protected override void CapNhatTrangThaiNut()
    {
        bool coQuyenSua = PhanQuyenService.CoQuyen(MaQuyen.CauHinhSua);
        btnLuu.Enabled = coQuyenSua;
        btnMacDinh.Enabled = coQuyenSua;
        btnLamMoi.Enabled = true;
        dgvThamSo.ReadOnly = !coQuyenSua;
        if (dgvThamSo.Columns.Contains("GiaTri")) dgvThamSo.Columns["GiaTri"].ReadOnly = !coQuyenSua;
    }

    private async void btnLuu_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.CauHinhSua)) return;
        if (!HopLe()) return;

        var danhSachLuu = _danhSach.Select(t => new KeyValuePair<string, string>(t.TenThamSo, (t.GiaTri ?? "").Trim()))
            .ToList();

        if (!await ThucHienAsync(() => ServiceFactory.CauHinh.LuuNhieu(danhSachLuu), "Đã lưu cấu hình hệ thống.")) return;

        _ = TaiDuLieuAsync();
    }

    private bool HopLe()
    {
        errLoi.Clear();
        foreach (ThamSo thamSo in _danhSach)
        {
            string giaTri = (thamSo.GiaTri ?? "").Trim();
            if (string.IsNullOrWhiteSpace(giaTri))
            {
                CanhBao($"Giá trị của tham số \"{thamSo.TenThamSo}\" không được để trống.", "Dữ liệu không hợp lệ");
                return false;
            }

            if (thamSo.TenThamSo == ThamSoKeys.PhanTramGiamCuoiTuan)
            {
                if (!decimal.TryParse(giaTri, out decimal phanTram) || phanTram < 0 || phanTram > 100)
                {
                    CanhBao("Phần trăm giảm cuối tuần phải nằm trong khoảng 0 - 100.", "Dữ liệu không hợp lệ");
                    return false;
                }
            }
            else if (thamSo.TenThamSo == ThamSoKeys.ThoiLuongBlockPhut)
            {
                if (!int.TryParse(giaTri, out int block) || block <= 0 || block > 480)
                {
                    CanhBao("Thời lượng block phải lớn hơn 0 và không quá 480 phút.", "Dữ liệu không hợp lệ");
                    return false;
                }
            }
            else if (thamSo.TenThamSo == ThamSoKeys.ThoiGianHuyToiDaGio)
            {
                if (!int.TryParse(giaTri, out int gio) || gio < 0 || gio > 720)
                {
                    CanhBao("Thời gian hủy tối đa phải từ 0 đến 720 giờ.", "Dữ liệu không hợp lệ");
                    return false;
                }
            }
            else if (thamSo.TenThamSo == ThamSoKeys.SoNgayDatTruoc)
            {
                if (!int.TryParse(giaTri, out int ngay) || ngay <= 0 || ngay > 365)
                {
                    CanhBao("Số ngày đặt trước phải từ 1 đến 365.", "Dữ liệu không hợp lệ");
                    return false;
                }
            }
            else if (thamSo.TenThamSo == ThamSoKeys.GioMoCua || thamSo.TenThamSo == ThamSoKeys.GioDongCua)
            {
                if (!TimeSpan.TryParse(giaTri, out _))
                {
                    CanhBao($"Tham số \"{thamSo.TenThamSo}\" phải đúng định dạng HH:mm.", "Dữ liệu không hợp lệ");
                    return false;
                }
            }
        }
        return true;
    }

    private void btnMacDinh_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.CauHinhSua)) return;
        if (!XacNhan("Khôi phục toàn bộ tham số về giá trị mặc định?", "Khôi phục mặc định")) return;

        foreach (ThamSo thamSo in _danhSach)
            thamSo.GiaTri = thamSo.TenThamSo switch
            {
                ThamSoKeys.PhanTramGiamCuoiTuan => "10",
                ThamSoKeys.ThoiLuongBlockPhut => "30",
                ThamSoKeys.GioMoCua => "05:00",
                ThamSoKeys.GioDongCua => "23:00",
                ThamSoKeys.TenTrungTam => "TRUNG TÂM THỂ THAO HOÀNG GIA",
                ThamSoKeys.DiaChi => "Số 1 Đường Thể Thao, Quận Hoàn Kiếm, Hà Nội",
                ThamSoKeys.DienThoai => "024 3888 9999",
                ThamSoKeys.LoiChaoHoaDon => "Cảm ơn quý khách, hẹn gặp lại!",
                ThamSoKeys.ThoiGianHuyToiDaGio => "24",
                ThamSoKeys.SoNgayDatTruoc => "30",
                _ => thamSo.GiaTri
            };

        dgvThamSo.Refresh();
        ThongTin("Đã đặt giá trị mặc định, nhấn Lưu để áp dụng.");
    }

    private void btnLamMoi_Click(object sender, EventArgs e)
    {
        errLoi.Clear();
        _ = TaiDuLieuAsync();
    }
}
