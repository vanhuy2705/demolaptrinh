using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Lịch sử đặt sân của chính khách hàng đang đăng nhập.</summary>
public partial class frmLichSuDatSan : BaseForm
{
    public frmLichSuDatSan()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.DatSanXemCuaToi;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvLichSu);
        Luoi.DatTieuDe(dgvLichSu,
            ("MaDat", "Mã"),
            ("NgayDat", "Ngày"),
            ("GioBatDau", "Bắt đầu"),
            ("GioKetThuc", "Kết thúc"),
            ("TenSan", "Sân"),
            ("TienSan", "Tiền sân"),
            ("TrangThai", "Trạng thái"));
        Luoi.DatDoRong(dgvLichSu, "MaDat", 60);
        Luoi.DatDoRong(dgvLichSu, "NgayDat", 100);
        Luoi.DatDoRong(dgvLichSu, "GioBatDau", 90);
        Luoi.DatDoRong(dgvLichSu, "GioKetThuc", 90);
        Luoi.DatDinhDangNgay(dgvLichSu, "dd/MM/yyyy", "NgayDat");
        Luoi.DatDinhDangTien(dgvLichSu, "TienSan");
        Luoi.HienThiTrangThai(dgvLichSu, "TrangThai", TrangThaiDatSan.TenHienThi);
        Luoi.ToMauTrangThai(dgvLichSu, "TrangThai");

        cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Đã đặt", "Đang sử dụng", "Hoàn thành", "Đã hủy" });
        cboTrangThai.SelectedIndex = 0;
    }

    protected override Task TaiDuLieuAsync() => TaiLichSuAsync();

    protected override void CapNhatTrangThaiNut()
    {
        DatSan dangChon = Luoi.LayDongDangChon<DatSan>(dgvLichSu);
        btnChiTiet.Enabled = dangChon != null;
        btnHuy.Enabled = dangChon != null
            && (dangChon.TrangThai == TrangThaiDatSan.DaDat || dangChon.TrangThai == TrangThaiDatSan.DangSuDung)
            && PhanQuyenService.CoQuyen(MaQuyen.DatSanHuy);
    }

    private async Task TaiLichSuAsync()
    {
        int? maKH = PhienLamViec.MaKH;
        string trangThai = cboTrangThai.SelectedIndex switch
        {
            1 => TrangThaiDatSan.DaDat,
            2 => TrangThaiDatSan.DangSuDung,
            3 => TrangThaiDatSan.HoanThanh,
            4 => TrangThaiDatSan.DaHuy,
            _ => null
        };
        bool locTu = dtpTuNgay.Checked; DateTime tuNgay = dtpTuNgay.Value.Date;
        bool locDen = dtpDenNgay.Checked; DateTime denNgay = dtpDenNgay.Value.Date;

        BatDauBan();
        try
        {
            if (maKH == null)
            {
                Luoi.GanDuLieu(dgvLichSu, new List<DatSan>());
                CapNhatTrangThaiNut();
                return;
            }

            var danhSach = await ChayNenAsync(() =>
            {
                ServiceFactory.DatSan.CapNhatBookingDangSuDung();
                return ServiceFactory.DatSan.LayTheoKhachHang(maKH.Value);
            });

            if (trangThai != null) danhSach = danhSach.Where(d => d.TrangThai == trangThai).ToList();
            if (locTu) danhSach = danhSach.Where(d => d.NgayDat.Date >= tuNgay).ToList();
            if (locDen) danhSach = danhSach.Where(d => d.NgayDat.Date <= denNgay).ToList();

            Luoi.GanDuLieu(dgvLichSu, danhSach.OrderByDescending(d => d.NgayDat).ToList());
            lblThongKe.Text = $"Tổng {danhSach.Count} lượt  |  " +
                              $"Đã hoàn thành: {danhSach.Count(d => d.TrangThai == TrangThaiDatSan.HoanThanh)}  |  " +
                              $"Đã hủy: {danhSach.Count(d => d.TrangThai == TrangThaiDatSan.DaHuy)}  |  " +
                              $"Tổng tiền: {TroGiup.Tien(danhSach.Where(d => d.TrangThai != TrangThaiDatSan.DaHuy).Sum(d => d.TienSan))}";
            CapNhatTrangThaiNut();
        }
        catch (Exception ex) { BaoLoi("Không thể tải lịch sử đặt sân", ex); }
        finally { KetThucBan(); }
    }

    private void btnChiTiet_Click(object sender, EventArgs e)
    {
        int maDat = Luoi.LayMaDangChon(dgvLichSu, "MaDat");
        if (maDat <= 0) return;
        using var chiTiet = new frmChiTietDatSan(maDat, coQuyenQuanLy: false);
        if (chiTiet.ShowDialog(this) == DialogResult.OK) _ = TaiLichSuAsync();
    }

    private async void btnHuy_Click(object sender, EventArgs e)
    {
        DatSan dangChon = Luoi.LayDongDangChon<DatSan>(dgvLichSu);
        if (dangChon == null) return;
        if (!CoQuyen(MaQuyen.DatSanHuy)) return;
        if (!XacNhan($"Hủy booking #{dangChon.MaDat} ({dangChon.TenSan}, {dangChon.NgayDat:dd/MM/yyyy})?",
                "Xác nhận hủy booking")) return;

        await ThucHienAsync(() => ServiceFactory.DatSan.HuyDatSan(dangChon.MaDat, "Khách hàng tự hủy"));
        _ = TaiLichSuAsync();
    }

    private void btnLamMoi_Click(object sender, EventArgs e)
    {
        cboTrangThai.SelectedIndex = 0;
        dtpTuNgay.Checked = false;
        dtpDenNgay.Checked = false;
        _ = TaiLichSuAsync();
    }

    private void LocThayDoi(object sender, EventArgs e)
    {
        if (IsHandleCreated) _ = TaiLichSuAsync();
    }

    private void dgvLichSu_SelectionChanged(object sender, EventArgs e) => CapNhatTrangThaiNut();
}
