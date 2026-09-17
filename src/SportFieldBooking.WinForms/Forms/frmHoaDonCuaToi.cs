using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Hóa đơn của chính khách hàng đang đăng nhập (chỉ xem và in, không được thu tiền).</summary>
public partial class frmHoaDonCuaToi : BaseForm
{
    public frmHoaDonCuaToi()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.HdXemCuaToi;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvHoaDon);
        Luoi.DatTieuDe(dgvHoaDon,
            ("MaHD", "Mã HĐ"),
            ("NgayLap", "Ngày lập"),
            ("TenSan", "Sân"),
            ("NgayDat", "Ngày đá"),
            ("TienGoc", "Tiền sân"),
            ("TienGiam", "Giảm"),
            ("TongTien", "Tổng tiền"),
            ("TrangThai", "Trạng thái"));
        Luoi.DatDoRong(dgvHoaDon, "MaHD", 66);
        Luoi.DatDoRong(dgvHoaDon, "NgayLap", 130);
        Luoi.DatDoRong(dgvHoaDon, "NgayDat", 100);
        Luoi.DatDinhDangTien(dgvHoaDon, "TienGoc", "TienGiam", "TongTien");
        Luoi.DatDinhDangNgay(dgvHoaDon, "dd/MM/yyyy HH:mm", "NgayLap");
        Luoi.DatDinhDangNgay(dgvHoaDon, "dd/MM/yyyy", "NgayDat");
        Luoi.HienThiTrangThai(dgvHoaDon, "TrangThai", TrangThaiHoaDon.TenHienThi);
        Luoi.ToMauTrangThai(dgvHoaDon, "TrangThai");

        cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Chưa thanh toán", "Đã thanh toán", "Đã hủy" });
        cboTrangThai.SelectedIndex = 0;
    }

    protected override void TaiDuLieu() => TaiHoaDon();

    protected override void CapNhatTrangThaiNut()
    {
        bool coChon = Luoi.LayDongDangChon<HoaDon>(dgvHoaDon) != null;
        btnChiTiet.Enabled = coChon;
        btnIn.Enabled = coChon;
    }

    private void TaiHoaDon()
    {
        ThucHien(() =>
        {
            if (PhienLamViec.MaKH == null)
            {
                Luoi.GanDuLieu(dgvHoaDon, new List<HoaDon>());
                return;
            }

            List<HoaDon> danhSach = ServiceFactory.HoaDon.LayTheoKhachHang(PhienLamViec.MaKH.Value);
            string trangThai = cboTrangThai.SelectedIndex switch
            {
                1 => TrangThaiHoaDon.ChuaThanhToan,
                2 => TrangThaiHoaDon.DaThanhToan,
                3 => TrangThaiHoaDon.DaHuy,
                _ => null
            };
            if (trangThai != null) danhSach = danhSach.Where(h => h.TrangThai == trangThai).ToList();

            Luoi.GanDuLieu(dgvHoaDon, danhSach.OrderByDescending(h => h.NgayLap).ToList());
            lblThongKe.Text = $"Tổng {danhSach.Count} hóa đơn  |  " +
                              $"Tổng chi tiêu: {TroGiup.Tien(danhSach.Where(h => h.TrangThai == TrangThaiHoaDon.DaThanhToan).Sum(h => h.TongTien))}  |  " +
                              $"Chưa thanh toán: {TroGiup.Tien(danhSach.Where(h => h.TrangThai == TrangThaiHoaDon.ChuaThanhToan).Sum(h => h.TongTien))}";
            CapNhatTrangThaiNut();
        }, "Không thể tải danh sách hóa đơn");
    }

    private void btnChiTiet_Click(object sender, EventArgs e)
    {
        int maHD = Luoi.LayMaDangChon(dgvHoaDon, "MaHD");
        if (maHD <= 0) return;
        using var chiTiet = new frmChiTietHoaDon(maHD, coQuyenThuTien: false);
        chiTiet.ShowDialog(this);
    }

    private void btnIn_Click(object sender, EventArgs e)
    {
        HoaDon dangChon = Luoi.LayDongDangChon<HoaDon>(dgvHoaDon);
        if (dangChon == null) return;

        ThucHien(() => InHoaDon.XemTruoc(dangChon, new InHoaDon.ThongTinCuaHang
        {
            TenTrungTam = ServiceFactory.CauHinh.LayGiaTri(ThamSoKeys.TenTrungTam, "TRUNG TÂM THỂ THAO"),
            DiaChi = ServiceFactory.CauHinh.LayGiaTri(ThamSoKeys.DiaChi, ""),
            DienThoai = ServiceFactory.CauHinh.LayGiaTri(ThamSoKeys.DienThoai, ""),
            LoiChao = ServiceFactory.CauHinh.LayGiaTri(ThamSoKeys.LoiChaoHoaDon, "Cảm ơn quý khách, hẹn gặp lại!")
        }), "Không thể xem trước hóa đơn");
    }

    private void btnLamMoi_Click(object sender, EventArgs e)
    {
        cboTrangThai.SelectedIndex = 0;
        TaiHoaDon();
    }

    private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IsHandleCreated) TaiHoaDon();
    }

    private void dgvHoaDon_SelectionChanged(object sender, EventArgs e) => CapNhatTrangThaiNut();
}
