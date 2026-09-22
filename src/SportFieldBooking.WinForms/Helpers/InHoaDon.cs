using System.Drawing.Printing;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;

namespace SportFieldBooking.WinForms.Helpers;

/// <summary>In hóa đơn bằng PrintDocument + PrintPreviewDialog (không cần thư viện PDF).</summary>
public static class InHoaDon
{
    public class ThongTinCuaHang
    {
        public string TenTrungTam { get; set; } = "TRUNG TÂM THỂ THAO";
        public string DiaChi { get; set; } = "";
        public string DienThoai { get; set; } = "";
        public string LoiChao { get; set; } = "Cảm ơn quý khách, hẹn gặp lại!";
    }

    public static void XemTruoc(HoaDon hoaDon, ThongTinCuaHang cuaHang = null)
    {
        if (hoaDon == null) return;
        try
        {
            using var taiLieu = new PrintDocument();
            cuaHang ??= new ThongTinCuaHang();
            taiLieu.PrintPage += (_, e) =>
            {
                try { VeTrang(e.Graphics, e.PageBounds, hoaDon, cuaHang); }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Lỗi vẽ hóa đơn: " + ex.Message); }
            };

            using var xemTruoc = new PrintPreviewDialog
            {
                Document = taiLieu,
                Width = 1000,
                Height = 800,
                StartPosition = FormStartPosition.CenterParent
            };
            xemTruoc.ShowDialog();
        }
        catch (Exception ex)
        {
            System.Windows.Forms.MessageBox.Show("Không thể xem trước hóa đơn: " + ex.Message,
                "Lỗi in ấn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    public static void InTrucTiep(HoaDon hoaDon, ThongTinCuaHang cuaHang = null)
    {
        if (hoaDon == null) return;
        try
        {
            using var taiLieu = new PrintDocument();
            cuaHang ??= new ThongTinCuaHang();
            taiLieu.PrintPage += (_, e) =>
            {
                try { VeTrang(e.Graphics, e.PageBounds, hoaDon, cuaHang); }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Lỗi vẽ hóa đơn: " + ex.Message); }
            };

            using var hopThoai = new PrintDialog { Document = taiLieu };
            if (hopThoai.ShowDialog() == DialogResult.OK)
            {
                try { taiLieu.Print(); }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Không thể in hóa đơn: " + ex.Message + "\nVui lòng kiểm tra máy in.",
                        "Lỗi in ấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception ex)
        {
            System.Windows.Forms.MessageBox.Show("Không thể in hóa đơn: " + ex.Message,
                "Lỗi in ấn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private static void VeTrang(Graphics g, Rectangle vung, HoaDon hoaDon, ThongTinCuaHang cuaHang)
    {
        if (g == null || hoaDon == null || cuaHang == null) return;
        float leTrai = 60, leTren = 60;
        float y = leTren;
        using var butDam = new Font("Segoe UI", 16F, FontStyle.Bold);
        using var butThuong = new Font("Segoe UI", 11F);
        using var butNho = new Font("Segoe UI", 9.5F);
        using var butTieuDe = new Font("Segoe UI", 13F, FontStyle.Bold);
        using var co = new SolidBrush(Color.Black);
        using var coXam = new SolidBrush(Color.Gray);
        using var butKe = new Pen(Color.FromArgb(200, 200, 200));

        g.DrawString(cuaHang.TenTrungTam ?? "TRUNG TÂM THỂ THAO", butDam, co, leTrai, y);
        y += 32;
        if (!string.IsNullOrWhiteSpace(cuaHang.DiaChi))
        {
            g.DrawString("Địa chỉ: " + cuaHang.DiaChi, butNho, coXam, leTrai, y);
            y += 20;
        }
        if (!string.IsNullOrWhiteSpace(cuaHang.DienThoai))
        {
            g.DrawString("Điện thoại: " + cuaHang.DienThoai, butNho, coXam, leTrai, y);
            y += 20;
        }

        y += 10;
        g.DrawLine(butKe, leTrai, y, vung.Width - leTrai, y);
        y += 20;

        g.DrawString($"HÓA ĐƠN THANH TOÁN #{hoaDon.MaHD}", butTieuDe, co, leTrai, y);
        y += 26;
        g.DrawString($"Ngày lập: {hoaDon.NgayLap:dd/MM/yyyy HH:mm}", butNho, coXam, leTrai, y);
        y += 24;

        g.DrawString("Khách hàng: " + (hoaDon.TenKH ?? ""), butThuong, co, leTrai, y);
        y += 22;
        g.DrawString("Số điện thoại: " + (hoaDon.SDT ?? ""), butThuong, co, leTrai, y);
        y += 22;
        g.DrawString("Sân: " + (hoaDon.TenSan ?? ""), butThuong, co, leTrai, y);
        y += 22;
        g.DrawString($"Ngày đặt: {hoaDon.NgayDat:dd/MM/yyyy}", butThuong, co, leTrai, y);
        y += 22;
        g.DrawString($"Khung giờ: {hoaDon.GioBatDau:hh\\:mm} - {hoaDon.GioKetThuc:hh\\:mm}", butThuong, co, leTrai, y);
        y += 28;

        g.DrawLine(butKe, leTrai, y, vung.Width - leTrai, y);
        y += 16;

        VeDong(g, "Tiền sân (giá gốc)", TroGiup.Tien(hoaDon.TienGoc), leTrai, ref y, vung.Width, butThuong, co);
        VeDong(g, "Giảm giá (" + LoaiGiamGia.TenHienThi(hoaDon.LoaiGiamGia) + ")",
            hoaDon.TienGiam > 0 ? "-" + TroGiup.Tien(hoaDon.TienGiam) : TroGiup.Tien(0),
            leTrai, ref y, vung.Width, butThuong, co);

        y += 6;
        g.DrawLine(butKe, leTrai, y, vung.Width - leTrai, y);
        y += 16;

        using var butLon = new Font("Segoe UI", 14F, FontStyle.Bold);
        VeDong(g, "TỔNG THANH TOÁN", TroGiup.Tien(hoaDon.TongTien), leTrai, ref y, vung.Width, butLon, co);

        y += 24;
        g.DrawString("Phương thức: " + PhuongThucThanhToan.TenHienThi(hoaDon.PhuongThucThanhToan)
            + "   |   Trạng thái: " + TrangThaiHoaDon.TenHienThi(hoaDon.TrangThai), butThuong, co, leTrai, y);
        y += 30;

        if (!string.IsNullOrWhiteSpace(hoaDon.MaCode))
        {
            g.DrawString("Voucher đã dùng: " + hoaDon.MaCode, butNho, coXam, leTrai, y);
            y += 22;
        }
        if (!string.IsNullOrWhiteSpace(cuaHang.LoiChao))
        {
            y += 16;
            g.DrawString(cuaHang.LoiChao, butThuong, coXam, leTrai, y);
        }
    }

    private static void VeDong(Graphics g, string trai, string phai, float leTrai, ref float y, int doRongTrang,
        Font font, Brush co)
    {
        if (g == null) return;
        try
        {
            g.DrawString(trai ?? "", font, co, leTrai, y);
            SizeF kichThuoc = g.MeasureString(phai ?? "", font);
            g.DrawString(phai ?? "", font, co, doRongTrang - leTrai - kichThuoc.Width, y);
            y += font.Size + 12;
        }
        catch { y += font.Size + 12; }
    }
}
