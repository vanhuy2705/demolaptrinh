using System.ComponentModel;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Controls;

/// <summary>Thẻ số liệu tổng quan trên Dashboard (icon + tiêu đề + giá trị + mô tả phụ).</summary>
public partial class KpiCard : UserControl
{
    public KpiCard()
    {
        InitializeComponent();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Giao dien")]
    public string TieuDe
    {
        get => lblTieuDe.Text;
        set => lblTieuDe.Text = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Giao dien")]
    public string GiaTri
    {
        get => lblGiaTri.Text;
        set => lblGiaTri.Text = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Giao dien")]
    public string PhuDe
    {
        get => lblPhuDe.Text;
        set => lblPhuDe.Text = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Giao dien"), Description("Tên icon: home, calendar, san, users, hoadon, voucher...")]
    public string TenBieuTuong
    {
        get => picBieuTuong.TenBieuTuong;
        set => picBieuTuong.TenBieuTuong = value;
    }

    /// <summary>Màu nhấn của thẻ (vạch trái + màu icon).</summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Giao dien")]
    public Color MauNhan
    {
        get => pnlMau.BackColor;
        set
        {
            pnlMau.BackColor = value;
            picBieuTuong.MauBieuTuong = value;
            lblGiaTri.ForeColor = GiaoDien.Chu;
        }
    }

    /// <summary>Cập nhật nhanh cả tiêu đề, giá trị và mô tả phụ.</summary>
    public void DatNoiDung(string tieuDe, string giaTri, string phuDe = "")
    {
        lblTieuDe.Text = tieuDe;
        lblGiaTri.Text = giaTri;
        lblPhuDe.Text = phuDe;
    }

    /// <summary>Cập nhật lại màu theo chủ đề hiện tại (giữ nguyên màu nhấn riêng của thẻ).</summary>
    public void LamMoiMau()
    {
        pnlThe.BackColor = GiaoDien.BeMat;
        lblTieuDe.ForeColor = GiaoDien.ChuPhu;
        lblPhuDe.ForeColor = GiaoDien.ChuPhu;
        lblGiaTri.ForeColor = GiaoDien.Chu;
        Invalidate();
    }
}
