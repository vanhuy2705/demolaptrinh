using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Trang chủ cổng khách hàng: thống kê cá nhân, lịch sắp tới, voucher đang có.</summary>
public partial class frmTrangChuKhachHang : BaseForm
{
    public frmTrangChuKhachHang()
    {
        InitializeComponent();
    }

    /// <summary>Phát sinh khi khách nhấn nút đặt sân (màn hình chính sẽ chuyển trang).</summary>
    public event EventHandler MuonDatSan;

    protected override string MaQuyenYeuCau => MaQuyen.ThongKeCaNhan;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvSapToi);
        Luoi.DatTieuDe(dgvSapToi,
            ("MaDat", "Mã"),
            ("NgayDat", "Ngày"),
            ("GioBatDau", "Bắt đầu"),
            ("GioKetThuc", "Kết thúc"),
            ("TenSan", "Sân"),
            ("TienSan", "Tiền sân"),
            ("TrangThai", "Trạng thái"));
        Luoi.DatDoRong(dgvSapToi, "MaDat", 60);
        Luoi.DatDoRong(dgvSapToi, "NgayDat", 100);
        Luoi.DatDoRong(dgvSapToi, "GioBatDau", 90);
        Luoi.DatDoRong(dgvSapToi, "GioKetThuc", 90);
        Luoi.DatDinhDangNgay(dgvSapToi, "dd/MM/yyyy", "NgayDat");
        Luoi.DatDinhDangTien(dgvSapToi, "TienSan");
        Luoi.HienThiTrangThai(dgvSapToi, "TrangThai", TrangThaiDatSan.TenHienThi);
        Luoi.ToMauTrangThai(dgvSapToi, "TrangThai");

        Luoi.Dang(dgvVoucher);
        Luoi.DatTieuDe(dgvVoucher,
            ("MaCode", "Mã"),
            ("TenVoucher", "Tên voucher"),
            ("GiaTriGiam", "Giá trị"),
            ("NgayKetThuc", "Hết hạn"));
        Luoi.DatDoRong(dgvVoucher, "MaCode", 110);
        Luoi.DatDinhDangNgay(dgvVoucher, "dd/MM/yyyy", "NgayKetThuc");
    }

    protected override void TaiDuLieu() => TaiTrangChu();

    private void TaiTrangChu()
    {
        ThucHien(() =>
        {
            lblLoiChao.Text = $"Xin chào, {PhienLamViec.HoTen}!";

            if (PhienLamViec.MaKH == null)
            {
                lblThongBao.Text = "Tài khoản của bạn chưa được gắn với hồ sơ khách hàng. Vui lòng liên hệ quầy.";
                lblThongBao.Visible = true;
                return;
            }

            int maKH = PhienLamViec.MaKH.Value;
            ServiceFactory.DatSan.CapNhatBookingDangSuDung();

            ThongKeCaNhan thongKe = ServiceFactory.ThongKe.LayThongKeCaNhan(maKH);
            kpiSoLanDat.DatNoiDung("Số lần đặt", thongKe.SoLanDat.ToString(), $"Hoàn thành: {thongKe.SoLanHoanThanh}");
            kpiChiTieu.DatNoiDung("Tổng chi tiêu", TroGiup.Tien(thongKe.TongChiTieu), $"Đã hủy: {thongKe.SoLanHuy}");
            kpiVoucher.DatNoiDung("Voucher đã dùng", thongKe.SoLanDungVoucher.ToString(), "");
            kpiSanYeuThich.DatNoiDung("Sân yêu thích", thongKe.SanYeuThich, "");

            List<DatSan> sapToi = ServiceFactory.DatSan.LaySapDienRa(30)
                .Where(d => d.MaKH == maKH).ToList();
            Luoi.GanDuLieu(dgvSapToi, sapToi);

            Luoi.GanDuLieu(dgvVoucher, ServiceFactory.Voucher.LayVoucherCoTheDung());
            lblThongBao.Visible = sapToi.Count == 0;
            if (sapToi.Count == 0)
                lblThongBao.Text = "Bạn chưa có lịch đặt sân sắp tới. Nhấn \"Đặt sân ngay\" để tạo lịch mới.";
        }, "Không thể tải dữ liệu trang chủ");
    }

    private void btnDatSanNgay_Click(object sender, EventArgs e) => MuonDatSan?.Invoke(this, EventArgs.Empty);

    private void btnLamMoi_Click(object sender, EventArgs e) => TaiTrangChu();
}
