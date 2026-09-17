using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Voucher có thể sử dụng của khách hàng và lịch sử đã dùng.</summary>
public partial class frmVoucherCuaToi : BaseForm
{
    public frmVoucherCuaToi()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.VoucherXem;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvVoucher);
        Luoi.DatTieuDe(dgvVoucher,
            ("MaCode", "Mã"),
            ("TenVoucher", "Tên voucher"),
            ("GiaTriGiam", "Mức giảm"),
            ("DonToiThieu", "Đơn tối thiểu"),
            ("SoLuongConLai", "Còn lại"),
            ("NgayKetThuc", "Hết hạn"),
            ("MoTa", "Mô tả"));
        Luoi.DatDoRong(dgvVoucher, "MaCode", 110);
        Luoi.DatDoRong(dgvVoucher, "GiaTriGiam", 90);
        Luoi.DatDoRong(dgvVoucher, "DonToiThieu", 110);
        Luoi.DatDinhDangNgay(dgvVoucher, "dd/MM/yyyy", "NgayKetThuc");
        dgvVoucher.CellFormatting += dgvVoucher_CellFormatting;

        Luoi.Dang(dgvLichSu);
        Luoi.DatTieuDe(dgvLichSu,
            ("MaSuDung", "Mã"),
            ("MaCode", "Mã voucher"),
            ("MaDat", "Booking"),
            ("SoTienGiam", "Số tiền giảm"),
            ("NgaySuDung", "Ngày dùng"));
        Luoi.DatDoRong(dgvLichSu, "MaSuDung", 60);
        Luoi.DatDoRong(dgvLichSu, "MaCode", 130);
        Luoi.DatDoRong(dgvLichSu, "MaDat", 90);
        Luoi.DatDinhDangTien(dgvLichSu, "SoTienGiam");
        Luoi.DatDinhDangNgay(dgvLichSu, "dd/MM/yyyy HH:mm", "NgaySuDung");
    }

    protected override Task TaiDuLieuAsync() => TaiVoucherAsync();

    private async Task TaiVoucherAsync()
    {
        string tuKhoa = txtTimKiem.Text.Trim();
        int? maKH = PhienLamViec.MaKH;

        BatDauBan();
        try
        {
            var (coTheDung, lichSu) = await ChayNenAsync(() =>
            {
                var dung = ServiceFactory.Voucher.LayVoucherCoTheDung(tuKhoa);
                var ls = maKH != null ? ServiceFactory.Voucher.LayLichSuSuDung(maKH.Value) : null;
                return (dung, ls);
            });

            Luoi.GanDuLieu(dgvVoucher, coTheDung);
            if (lichSu != null) Luoi.GanDuLieu(dgvLichSu, lichSu);
            lblThongKe.Text = $"{coTheDung.Count} voucher đang có thể sử dụng";
        }
        catch (Exception ex) { BaoLoi("Không thể tải danh sách voucher", ex); }
        finally { KetThucBan(); }
    }

    private void btnLamMoi_Click(object sender, EventArgs e)
    {
        txtTimKiem.Clear();
        _ = TaiVoucherAsync();
    }

    private void btnTim_Click(object sender, EventArgs e) => _ = TaiVoucherAsync();

    private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) _ = TaiVoucherAsync();
    }

    /// <summary>Hiển thị mức giảm kèm đơn vị (% hoặc tiền).</summary>
    private void dgvVoucher_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || dgvVoucher.Columns[e.ColumnIndex].Name != "GiaTriGiam") return;
        if (dgvVoucher.Rows[e.RowIndex].DataBoundItem is not Voucher voucher) return;

        e.Value = voucher.LoaiGiam == LoaiGiam.PhanTram
            ? voucher.GiaTriGiam.ToString("0") + " %"
            : TroGiup.Tien(voucher.GiaTriGiam);
        e.FormattingApplied = true;
    }
}
