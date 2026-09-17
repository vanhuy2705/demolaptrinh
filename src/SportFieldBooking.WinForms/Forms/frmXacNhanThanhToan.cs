using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>
/// Hộp thoại xác nhận thanh toán: chọn tiền mặt/chuyển khoản, nhập tiền khách đưa, tính tiền thừa.
/// Chỉ thu thập thông tin thanh toán - việc cập nhật CSDL do HoaDonService.ThanhToan đảm nhiệm.
/// </summary>
public partial class frmXacNhanThanhToan : Form
{
    private readonly HoaDon _hoaDon;

    public frmXacNhanThanhToan(HoaDon hoaDon)
    {
        InitializeComponent();
        _hoaDon = hoaDon;
    }

    /// <summary>Phương thức thanh toán được chọn.</summary>
    public string PhuongThuc => radChuyenKhoan.Checked ? PhuongThucThanhToan.ChuyenKhoan : PhuongThucThanhToan.TienMat;

    /// <summary>Số tiền khách đưa (chỉ có ý nghĩa với tiền mặt).</summary>
    public decimal SoTienKhachDua { get; private set; }

    private void frmXacNhanThanhToan_Load(object sender, EventArgs e)
    {
        GiaoDien.ApDung(this);
        StartPosition = FormStartPosition.CenterParent;
        if (_hoaDon == null) return;

        lblMaHoaDon.Text = "Hóa đơn #" + _hoaDon.MaHD;
        lblKhachHang.Text = $"{_hoaDon.TenKH}  |  {_hoaDon.SDT}";
        lblSan.Text = _hoaDon.TenSan;
        lblThoiGian.Text = $"{_hoaDon.NgayDat:dd/MM/yyyy}   {_hoaDon.GioBatDau:hh\\:mm} - {_hoaDon.GioKetThuc:hh\\:mm}";
        lblTienGoc.Text = TroGiup.Tien(_hoaDon.TienGoc);
        lblGiamGia.Text = _hoaDon.TienGiam > 0
            ? $"-{TroGiup.Tien(_hoaDon.TienGiam)} ({LoaiGiamGia.TenHienThi(_hoaDon.LoaiGiamGia)})"
            : TroGiup.Tien(0);
        lblTongTien.Text = TroGiup.Tien(_hoaDon.TongTien);
        txtSoTienKhachDua.Text = ((long)_hoaDon.TongTien).ToString();
        TinhTienThua();
    }

    private void TinhTienThua()
    {
        if (_hoaDon == null) return;
        decimal.TryParse(txtSoTienKhachDua.Text.Trim().Replace(",", "").Replace(".", ""), out decimal khachDua);
        SoTienKhachDua = khachDua;
        decimal thua = khachDua - _hoaDon.TongTien;
        lblTienThua.Text = thua >= 0 ? TroGiup.Tien(thua) : "Chưa đủ " + TroGiup.Tien(-thua);
        lblTienThua.ForeColor = thua >= 0 ? GiaoDien.ThanhCong : GiaoDien.NguyHiem;
    }

    private void btnXacNhan_Click(object sender, EventArgs e)
    {
        if (_hoaDon == null) return;

        if (radTienMat.Checked)
        {
            if (SoTienKhachDua < _hoaDon.TongTien)
            {
                frmThongBao.HienThi("Số tiền khách đưa chưa đủ để thanh toán hóa đơn.",
                    "Thiếu tiền", frmThongBao.LoaiThongBao.CanhBao, this);
                txtSoTienKhachDua.Select();
                return;
            }
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnHuy_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void txtSoTienKhachDua_TextChanged(object sender, EventArgs e) => TinhTienThua();

    private void radTienMat_CheckedChanged(object sender, EventArgs e)
    {
        bool laTienMat = radTienMat.Checked;
        txtSoTienKhachDua.Enabled = laTienMat;
        lblTienThua.Visible = laTienMat;
        lblNhanTienThua.Visible = laTienMat;
        TinhTienThua();
    }
}
