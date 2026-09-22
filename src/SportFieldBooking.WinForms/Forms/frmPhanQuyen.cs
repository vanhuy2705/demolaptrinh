using SportFieldBooking.Business.Common;
using SportFieldBooking.Core.Common;
using SportFieldBooking.Core.Entities;
using SportFieldBooking.WinForms.Base;
using SportFieldBooking.WinForms.Helpers;

namespace SportFieldBooking.WinForms.Forms;

/// <summary>Quản lý phân quyền theo vai trò: chọn vai trò và tick các quyền tương ứng (VAI_TRO_QUYEN).</summary>
public partial class frmPhanQuyen : BaseForm
{
    private List<Quyen> _tatCaQuyen = new();
    private List<VaiTroDb> _vaiTro = new();
    private string _vaiTroDangChon = VaiTro.Admin;

    public frmPhanQuyen()
    {
        InitializeComponent();
    }

    protected override string MaQuyenYeuCau => MaQuyen.TkPhanQuyen;

    protected override void ThietLapGiaoDien()
    {
        Luoi.Dang(dgvQuyen);
        Luoi.DatTieuDe(dgvQuyen,
            ("MaQuyen", "Mã quyền"),
            ("TenQuyen", "Tên quyền"),
            ("NhomQuyen", "Nhóm"),
            ("MoTa", "Mô tả"),
            ("DuocGan", "Được gán"));
        Luoi.DatDoRong(dgvQuyen, "MaQuyen", 150);
        Luoi.DatDoRong(dgvQuyen, "TenQuyen", 200);
        Luoi.DatDoRong(dgvQuyen, "NhomQuyen", 120);
        Luoi.DatDoRong(dgvQuyen, "DuocGan", 80);

        cboVaiTro.DropDownStyle = ComboBoxStyle.DropDownList;
        cboVaiTro.SelectedIndexChanged += cboVaiTro_SelectedIndexChanged;

        // Thêm cột checkbox nếu chưa có
        if (!dgvQuyen.Columns.Contains("DuocGan"))
        {
            var colCheck = new DataGridViewCheckBoxColumn
            {
                Name = "DuocGan",
                HeaderText = "Được gán",
                DataPropertyName = "DuocGan",
                Width = 80
            };
            dgvQuyen.Columns.Add(colCheck);
        }
    }

    protected override async Task TaiDuLieuAsync()
    {
        BatDauBan();
        try
        {
            _vaiTro = await ChayNenAsync(() => ServiceFactory.Quyen.LayVaiTro());
            _tatCaQuyen = await ChayNenAsync(() => ServiceFactory.Quyen.LayTatCa());

            if (_vaiTro.Count == 0)
            {
                // Fallback nếu CSDL chưa có bảng VAI_TRO
                _vaiTro = new List<VaiTroDb>
                {
                    new VaiTroDb { MaVaiTro = VaiTro.Admin, TenVaiTro = "Quản trị viên" },
                    new VaiTroDb { MaVaiTro = VaiTro.NhanVien, TenVaiTro = "Nhân viên" },
                    new VaiTroDb { MaVaiTro = VaiTro.KhachHang, TenVaiTro = "Khách hàng" }
                };
            }

            cboVaiTro.Items.Clear();
            foreach (var vt in _vaiTro)
                cboVaiTro.Items.Add($"{vt.MaVaiTro} - {vt.TenVaiTro}");

            if (cboVaiTro.Items.Count > 0)
            {
                cboVaiTro.SelectedIndex = 0;
                _vaiTroDangChon = _vaiTro[0].MaVaiTro;
            }

            await TaiQuyenTheoVaiTroAsync();
            CapNhatTrangThaiNut();
        }
        catch (Exception ex) { BaoLoi("Không thể tải dữ liệu phân quyền", ex); }
        finally { KetThucBan(); }
    }

    private async Task TaiQuyenTheoVaiTroAsync()
    {
        if (string.IsNullOrWhiteSpace(_vaiTroDangChon)) return;

        BatDauBan();
        try
        {
            List<string> quyenDaGan = await ChayNenAsync(() => ServiceFactory.Quyen.LayQuyenTheoVaiTro(_vaiTroDangChon));

            // Nếu DB chưa có dữ liệu, fallback về PhanQuyenService
            if (quyenDaGan.Count == 0)
            {
                quyenDaGan = PhanQuyenService.LayQuyenTheoVaiTro(_vaiTroDangChon);
            }

            var hienThi = _tatCaQuyen.Select(q => new QuyenHienThi
            {
                MaQuyen = q.MaQuyen,
                TenQuyen = q.TenQuyen,
                NhomQuyen = q.NhomQuyen,
                MoTa = q.MoTa,
                DuocGan = quyenDaGan.Contains(q.MaQuyen, StringComparer.OrdinalIgnoreCase)
            }).ToList();

            // Nếu _tatCaQuyen rỗng (DB chưa có bảng QUYEN), lấy từ MaQuyen constants
            if (hienThi.Count == 0)
            {
                var tatCaMa = PhanQuyenService.LayTatCaQuyen();
                hienThi = tatCaMa.Select(ma => new QuyenHienThi
                {
                    MaQuyen = ma,
                    TenQuyen = ma,
                    NhomQuyen = ma.Contains('_') ? ma.Split('_')[0] : "KHAC",
                    MoTa = PhanQuyenService.TenChucNang(ma),
                    DuocGan = quyenDaGan.Contains(ma, StringComparer.OrdinalIgnoreCase)
                }).ToList();
            }

            Luoi.GanDuLieu(dgvQuyen, hienThi);
            lblThongKe.Text = $"Vai trò {_vaiTroDangChon}: {hienThi.Count(h => h.DuocGan)}/{hienThi.Count} quyền";
        }
        catch (Exception ex) { BaoLoi("Không thể tải quyền theo vai trò", ex); }
        finally { KetThucBan(); }
    }

    protected override void CapNhatTrangThaiNut()
    {
        bool coQuyen = PhanQuyenService.CoQuyen(MaQuyen.TkPhanQuyen);
        btnLuu.Enabled = coQuyen;
        btnLamMoi.Enabled = true;
        btnChonTatCa.Enabled = coQuyen;
        btnBoChon.Enabled = coQuyen;
        dgvQuyen.ReadOnly = false;
        if (dgvQuyen.Columns.Contains("MaQuyen")) dgvQuyen.Columns["MaQuyen"].ReadOnly = true;
        if (dgvQuyen.Columns.Contains("TenQuyen")) dgvQuyen.Columns["TenQuyen"].ReadOnly = true;
        if (dgvQuyen.Columns.Contains("NhomQuyen")) dgvQuyen.Columns["NhomQuyen"].ReadOnly = true;
        if (dgvQuyen.Columns.Contains("MoTa")) dgvQuyen.Columns["MoTa"].ReadOnly = true;
        if (dgvQuyen.Columns.Contains("DuocGan")) dgvQuyen.Columns["DuocGan"].ReadOnly = false;
    }

    private async void cboVaiTro_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cboVaiTro.SelectedIndex < 0 || cboVaiTro.SelectedIndex >= _vaiTro.Count) return;
        _vaiTroDangChon = _vaiTro[cboVaiTro.SelectedIndex].MaVaiTro;
        await TaiQuyenTheoVaiTroAsync();
    }

    private async void btnLuu_Click(object sender, EventArgs e)
    {
        if (!CoQuyen(MaQuyen.TkPhanQuyen)) return;

        var ds = new List<string>();
        foreach (DataGridViewRow row in dgvQuyen.Rows)
        {
            if (row.DataBoundItem is QuyenHienThi qh && qh.DuocGan)
                ds.Add(qh.MaQuyen);
            else if (row.Cells["DuocGan"].Value is bool b && b)
            {
                var ma = row.Cells["MaQuyen"].Value?.ToString();
                if (!string.IsNullOrWhiteSpace(ma)) ds.Add(ma);
            }
        }

        if (!XacNhan($"Lưu {ds.Count} quyền cho vai trò {_vaiTroDangChon}?", "Xác nhận lưu")) return;

        if (!await ThucHienAsync(() => ServiceFactory.Quyen.CapNhatMaTran(_vaiTroDangChon, ds), $"Đã cập nhật quyền cho {_vaiTroDangChon}.")) return;

        await TaiQuyenTheoVaiTroAsync();
    }

    private void btnChonTatCa_Click(object sender, EventArgs e)
    {
        foreach (DataGridViewRow row in dgvQuyen.Rows)
        {
            if (row.DataBoundItem is QuyenHienThi qh) qh.DuocGan = true;
            row.Cells["DuocGan"].Value = true;
        }
        dgvQuyen.Refresh();
    }

    private void btnBoChon_Click(object sender, EventArgs e)
    {
        foreach (DataGridViewRow row in dgvQuyen.Rows)
        {
            if (row.DataBoundItem is QuyenHienThi qh) qh.DuocGan = false;
            row.Cells["DuocGan"].Value = false;
        }
        dgvQuyen.Refresh();
    }

    private void btnLamMoi_Click(object sender, EventArgs e)
    {
        _ = TaiQuyenTheoVaiTroAsync();
    }

    private class QuyenHienThi
    {
        public string MaQuyen { get; set; } = "";
        public string TenQuyen { get; set; } = "";
        public string NhomQuyen { get; set; } = "";
        public string MoTa { get; set; } = "";
        public bool DuocGan { get; set; }
    }
}
