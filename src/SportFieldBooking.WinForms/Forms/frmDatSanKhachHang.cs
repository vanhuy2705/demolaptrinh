using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>
/// Cổng khách hàng: tự đặt sân, xem khung giờ trống của sân, áp dụng voucher và xem bảng tính tiền.
/// </summary>
public partial class frmDatSanKhachHang : BaseForm
{
    private ChiTietTien _ketQuaTien;
    private int _phienTinhTien;

    public frmDatSanKhachHang()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.DatSanThem;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvLichTrongNgay);
        Luoi.DatTieuDe(dgvLichTrongNgay,
            ("GioBatDau", "Bắt đầu"),
            ("GioKetThuc", "Kết thúc"),
            ("TenSan", "Sân"),
            ("TrangThai", "Trạng thái"));
        Luoi.DatDoRong(dgvLichTrongNgay, "GioBatDau", 100);
        Luoi.DatDoRong(dgvLichTrongNgay, "GioKetThuc", 100);
        Luoi.HienThiTrangThai(dgvLichTrongNgay, "TrangThai", TrangThaiDatSan.TenHienThi);

        dtpNgayDat.MinDate = DateTime.Today;
        dtpNgayDat.Value = DateTime.Today;
        dtpGioBatDau.Format = DateTimePickerFormat.Custom;
        dtpGioBatDau.CustomFormat = "HH:mm";
        dtpGioBatDau.ShowUpDown = true;
        dtpGioKetThuc.Format = DateTimePickerFormat.Custom;
        dtpGioKetThuc.CustomFormat = "HH:mm";
        dtpGioKetThuc.ShowUpDown = true;
        TroGiup.DatGio(dtpGioBatDau, new TimeSpan(17, 0, 0));
        TroGiup.DatGio(dtpGioKetThuc, new TimeSpan(18, 0, 0));

    }


    protected override void CapNhatTrangThaiNut()
    {
        btnDatSan.Enabled = PhienLamViec.MaKH != null && PhanQuyenService.CoQuyen(MaQuyen.DatSanThem);
        btnLamMoi.Enabled = true;
    }


    protected override async Task TaiDuLieuAsync()
    {
        BatDauBan();
        try
        {
            List<San> danhSachSan = await ChayNenAsync(() =>
            {
                List<San> sanTrong = ServiceFactory.San.LayTatCa("", null, TrangThaiSan.Trong);
                return sanTrong.Count > 0 ? sanTrong : ServiceFactory.San.LayTatCa();
            });
            TroGiup.GanComboBox(cboSan, danhSachSan, "TenSan", "MaSan");
        }
        catch (Exception ex) { BaoLoi("Không thể tải danh sách sân", ex); }
        finally { KetThucBan(); }

        await TaiLichTrongNgayAsync(); // hiện lịch sân ngay khi mở form (trước đây lưới trống)
        await TinhTienAsync();
    }

    private async Task TinhTienAsync()
    {
        errLoi.Clear();
        int maSan = TroGiup.LayGiaTriComboBox(cboSan);
        if (maSan <= 0)
        {
            XoaBangTien();
            return;
        }

        San sanDangChon = cboSan.SelectedItem as San;
        lblThongTinSan.Text = sanDangChon == null
            ? "—"
            : $"{sanDangChon.TenLoaiSan}  |  {TroGiup.Tien(sanDangChon.DonGia)}/giờ";

        int? maKH = PhienLamViec.MaKH;
        DateTime ngay = dtpNgayDat.Value.Date;
        TimeSpan gioBatDau = TroGiup.LayGio(dtpGioBatDau), gioKetThuc = TroGiup.LayGio(dtpGioKetThuc);
        string maVoucher = txtMaVoucher.Text.Trim();
        int phien = ++_phienTinhTien;

        KetQua<ChiTietTien> ketQua;
        List<DatSan> trung;
        try
        {
            (ketQua, trung) = await ChayNenAsync(() => (
                ServiceFactory.TinhTien.TinhTien(maSan, ngay, gioBatDau, gioKetThuc, maVoucher, maKH),
                ServiceFactory.DatSan.LayTrungLich(maSan, ngay, gioBatDau, gioKetThuc)));
        }
        catch (Exception ex) { BaoLoi("Không thể tính tiền", ex); return; }

        if (phien != _phienTinhTien || IsDisposed) return; // đã có yêu cầu mới hơn

        if (!ketQua.ThanhCong)
        {
            _ketQuaTien = null;
            lblTrangThaiUuDai.Text = ketQua.ThongBao;
            lblTrangThaiUuDai.ForeColor = GiaoDien.NguyHiem;
            XoaBangTien();
            return;
        }

        _ketQuaTien = ketQua.DuLieu;
        lblSoGio.Text = $"{ketQua.DuLieu.SoGio:0.##} giờ ({ketQua.DuLieu.SoBlock} block x {ketQua.DuLieu.ThoiLuongBlockPhut} phút)";
        lblTienGoc.Text = TroGiup.Tien(ketQua.DuLieu.TienGoc);
        lblUuDai.Text = ketQua.DuLieu.TenUuDai;
        lblTienGiam.Text = "-" + TroGiup.Tien(ketQua.DuLieu.TienGiam);
        lblTongTien.Text = TroGiup.Tien(ketQua.DuLieu.TongTien);
        lblTrangThaiUuDai.Text = ketQua.ThongBao;
        lblTrangThaiUuDai.ForeColor = ketQua.DuLieu.TienGiam > 0 ? GiaoDien.ThanhCong : GiaoDien.ChuPhu;

        HienThiCanhBaoTrung(trung);
    }

    private void HienThiCanhBaoTrung(List<DatSan> trung)
    {
        lblCanhBaoTrung.Visible = trung.Count > 0;
        if (trung.Count > 0)
            lblCanhBaoTrung.Text = "⚠ " + ServiceFactory.DatSan.TaoThongBaoTrungLich(trung).Replace("\n", " ");
        CapNhatTrangThaiNut();
    }

    private async Task TaiLichTrongNgayAsync()
    {
        int maSan = TroGiup.LayGiaTriComboBox(cboSan);
        if (maSan <= 0) return;
        DateTime ngay = dtpNgayDat.Value.Date;

        BatDauBan();
        try
        {
            var lich = await ChayNenAsync(() => ServiceFactory.DatSan.LayTheoNgay(ngay, maSan)
                .Where(d => d.TrangThai != TrangThaiDatSan.DaHuy).ToList());
            Luoi.GanDuLieu(dgvLichTrongNgay, lich);
        }
        catch (Exception ex) { BaoLoi("Không thể tải lịch sân", ex); }
        finally { KetThucBan(); }
    }

    private void XoaBangTien()
    {
        lblSoGio.Text = "—";
        lblTienGoc.Text = "0 đ";
        lblUuDai.Text = "—";
        lblTienGiam.Text = "0 đ";
        lblTongTien.Text = "0 đ";
    }



    private async void btnDatSan_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.DatSanThem)) return;
        if (PhienLamViec.MaKH == null)
        {
            CanhBao("Tài khoản của bạn chưa gắn với hồ sơ khách hàng, vui lòng liên hệ quầy để được hỗ trợ.",
                "Không thể đặt sân");
            return;
        }
        // Luôn tính lại tiền ngay trước khi đặt: tránh đặt sân với giá cũ hiển thị từ
        // trước khi đổi ngày/giờ/sân/voucher mà quên bấm "Tính tiền".
        await TinhTienAsync();
        if (_ketQuaTien == null)
        {
            CanhBao("Khung giờ chưa hợp lệ, vui lòng kiểm tra lại giờ bắt đầu / kết thúc.", "Chưa thể đặt sân");
            return;
        }

        int maKH = PhienLamViec.MaKH.Value;
        int maSan = TroGiup.LayGiaTriComboBox(cboSan);
        DateTime ngay = dtpNgayDat.Value.Date;
        TimeSpan gioBatDau = TroGiup.LayGio(dtpGioBatDau), gioKetThuc = TroGiup.LayGio(dtpGioKetThuc);
        string ghiChu = txtGhiChu.Text.Trim(), maVoucher = txtMaVoucher.Text.Trim();

        KetQua<DatSan> ketQua = await ChayNenAsync(() => ServiceFactory.DatSan.TaoDatSan(
            maKH, maSan, ngay, gioBatDau, gioKetThuc, ghiChu, maVoucher));
        if (IsDisposed) return;

        if (!ThucHien(ketQua, "Đặt sân thành công! Vui lòng đến quầy thanh toán trước giờ chơi.")) return;

        txtGhiChu.Clear();
        txtMaVoucher.Clear();
        _ = TaiLichTrongNgayAsync();
        _ = TinhTienAsync();
    }

    private void btnLamMoi_Click(object sender, EventArgs e)
    {
        txtGhiChu.Clear();
        txtMaVoucher.Clear();
        _ = TaiLichTrongNgayAsync();
        _ = TinhTienAsync();
    }

    private void cboSan_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!IsHandleCreated) return;
        _ = TaiLichTrongNgayAsync();
        _ = TinhTienAsync();
    }

    private void ThayDoiThoiGian(object sender, EventArgs e)
    {
        if (!IsHandleCreated) return;
        _ = TinhTienAsync();
    }

    private void txtMaVoucher_TextChanged(object sender, EventArgs e)
    {
        if (IsHandleCreated) _ = TinhTienAsync();
    }

    private void btnVoucherCuaToi_Click(object sender, EventArgs e)
    {
        using var voucherCuaToi = new frmVoucherCuaToi();
        voucherCuaToi.ShowDialog(this);
    }
}
