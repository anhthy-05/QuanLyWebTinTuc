using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace QuanLyWebTinTuc
{
    public partial class frmGiaoDich : Form
    {
        private string? maHoaDonDangChon;   // Hóa đơn đang xem chi tiết
        private int? maCTHDDangChon;        // Dòng chi tiết đang chọn
        private bool dangKhoiTao;           // Chặn tải dữ liệu nhiều lần khi đang gán giá trị bộ lọc

        public frmGiaoDich()
        {
            InitializeComponent();
            DbHelper.SetupGrid(dgv_HoaDon);
            DbHelper.SetupGrid(dgv_ChiTiet);
        }

        private void frmGiaoDich_Load(object sender, EventArgs e)
        {
            DatLaiBoLoc();
            XoaTrangHoaDon();
            XoaTrangChiTiet();
            LoadData();
        }

        // ================== BỘ LỌC + DANH SÁCH HÓA ĐƠN ==================

        // Mặc định lọc từ ngày của hóa đơn cũ nhất đến hôm nay
        private void DatLaiBoLoc()
        {
            dangKhoiTao = true;
            try
            {
                object? ngayDauTien = DbHelper.ExecuteScalar("SELECT MIN(NgayLap) FROM HoaDon");
                dtp_TuNgay.Value = ngayDauTien == null || ngayDauTien == DBNull.Value
                    ? DateTime.Today
                    : Convert.ToDateTime(ngayDauTien).Date;
            }
            catch (Exception ex)
            {
                dtp_TuNgay.Value = DateTime.Today;
                DbHelper.ShowError(ex);
            }
            dtp_DenNgay.Value = DateTime.Today;
            cbo_LocTrangThai.SelectedIndex = 0; // Tất cả
            txt_TimKiem.Clear();
            dangKhoiTao = false;
        }

        private void LoadData()
        {
            if (dangKhoiTao) return;

            try
            {
                DataTable dt = DbHelper.ExecuteQuery(@"
                    SELECT h.MaHoaDon, h.NgayLap, h.KhachHang, h.TrangThai, h.GhiChu,
                           ISNULL(SUM(ct.ThanhTien), 0) AS TongTien
                    FROM HoaDon h
                    LEFT JOIN ChiTietHoaDon ct ON h.MaHoaDon = ct.MaHoaDon
                    WHERE h.NgayLap >= @tuNgay AND h.NgayLap < @denNgay
                      AND (@trangThai = N'Tất cả' OR h.TrangThai = @trangThai)
                      AND (h.MaHoaDon LIKE @tuKhoa OR h.KhachHang LIKE @tuKhoa OR h.GhiChu LIKE @tuKhoa)
                    GROUP BY h.MaHoaDon, h.NgayLap, h.KhachHang, h.TrangThai, h.GhiChu
                    ORDER BY h.NgayLap DESC",
                    new SqlParameter("@tuNgay", dtp_TuNgay.Value.Date),
                    new SqlParameter("@denNgay", dtp_DenNgay.Value.Date.AddDays(1)),
                    new SqlParameter("@trangThai", cbo_LocTrangThai.SelectedItem?.ToString() ?? "Tất cả"),
                    new SqlParameter("@tuKhoa", "%" + txt_TimKiem.Text.Trim() + "%"));

                dgv_HoaDon.DataSource = dt;
                guna2HtmlLabel3.Text = "Danh sách hóa đơn (" + dt.Rows.Count + ")";
                ChonLaiHoaDon();
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        // Sau khi tải lại danh sách, tô chọn lại hóa đơn đang xem
        private void ChonLaiHoaDon()
        {
            dgv_HoaDon.ClearSelection();
            if (maHoaDonDangChon == null) return;

            foreach (DataGridViewRow row in dgv_HoaDon.Rows)
            {
                if (row.Cells["Column1"].Value?.ToString() == maHoaDonDangChon)
                {
                    row.Selected = true;
                    dgv_HoaDon.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }

        private void BoLoc_Changed(object? sender, EventArgs e)
        {
            LoadData();
        }

        private void btn_TimKiem_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        // ================== THÔNG TIN HÓA ĐƠN ==================

        private void XoaTrangHoaDon()
        {
            txt_MaHoaDon.Clear();
            txt_MaHoaDon.ReadOnly = false;
            txt_KhachHang.Clear();
            txt_GhiChu.Clear();
            dtp_NgayLap.Value = DateTime.Now;
            cbo_TrangThai.SelectedIndex = 0; // Chờ thanh toán
            dgv_HoaDon.ClearSelection();
        }

        private bool KiemTraHoaDon()
        {
            if (string.IsNullOrWhiteSpace(txt_KhachHang.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_KhachHang.Focus();
                return false;
            }
            if (cbo_TrangThai.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn trạng thái hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        // Bấm vào 1 hóa đơn -> đổ thông tin lên form + tải chi tiết
        private void dgv_HoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgv_HoaDon.Rows[e.RowIndex];
            maHoaDonDangChon = row.Cells["Column1"].Value?.ToString();
            txt_MaHoaDon.Text = maHoaDonDangChon;
            txt_MaHoaDon.ReadOnly = true; // Không cho sửa khóa chính
            dtp_NgayLap.Value = Convert.ToDateTime(row.Cells["Column2"].Value);
            txt_KhachHang.Text = row.Cells["Column3"].Value?.ToString();
            cbo_TrangThai.SelectedItem = row.Cells["Column4"].Value?.ToString();
            txt_GhiChu.Text = row.Cells["Column6"].Value?.ToString();

            LoadChiTiet();
        }

        // Nút "Thêm HĐ": chuẩn bị form trống với mã mới để nhập hóa đơn
        private void btn_ThemHD_Click(object sender, EventArgs e)
        {
            maHoaDonDangChon = null;
            XoaTrangHoaDon();
            LoadChiTiet();
            try
            {
                txt_MaHoaDon.Text = DbHelper.TaoMaMoi("HoaDon", "MaHoaDon", "HD", 3);
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
            txt_KhachHang.Focus();
        }

        private void btn_Them_Click(object sender, EventArgs e)
        {
            if (!KiemTraHoaDon()) return;

            try
            {
                // Đang chọn 1 hóa đơn có sẵn thì tạo mã mới, không ghi đè
                string ma = txt_MaHoaDon.ReadOnly || string.IsNullOrWhiteSpace(txt_MaHoaDon.Text)
                    ? DbHelper.TaoMaMoi("HoaDon", "MaHoaDon", "HD", 3)
                    : txt_MaHoaDon.Text.Trim();

                DbHelper.ExecuteNonQuery(
                    "INSERT INTO HoaDon (MaHoaDon, NgayLap, KhachHang, TrangThai, GhiChu) VALUES (@ma, @ngayLap, @khachHang, @trangThai, @ghiChu)",
                    TaoThamSoHoaDon(ma));

                maHoaDonDangChon = ma;
                txt_MaHoaDon.Text = ma;
                txt_MaHoaDon.ReadOnly = true;
                MoRongBoLocTheoNgay(dtp_NgayLap.Value);
                LoadData();
                LoadChiTiet();

                MessageBox.Show("Đã thêm hóa đơn " + ma + ".\nHãy thêm các dòng chi tiết ở khung \"Chi tiết hóa đơn\".",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_LoaiGD.Focus();
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            if (maHoaDonDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần sửa trong danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!KiemTraHoaDon()) return;

            try
            {
                DbHelper.ExecuteNonQuery(
                    "UPDATE HoaDon SET NgayLap = @ngayLap, KhachHang = @khachHang, TrangThai = @trangThai, GhiChu = @ghiChu WHERE MaHoaDon = @ma",
                    TaoThamSoHoaDon(maHoaDonDangChon));

                MoRongBoLocTheoNgay(dtp_NgayLap.Value);
                LoadData();
                LoadChiTiet();
                MessageBox.Show("Cập nhật hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            if (maHoaDonDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xóa trong danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Xóa hóa đơn " + maHoaDonDangChon + " và toàn bộ chi tiết của nó?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                // Chi tiết hóa đơn tự xóa theo (ON DELETE CASCADE)
                DbHelper.ExecuteNonQuery("DELETE FROM HoaDon WHERE MaHoaDon = @ma", new SqlParameter("@ma", maHoaDonDangChon));
                maHoaDonDangChon = null;
                XoaTrangHoaDon();
                LoadData();
                LoadChiTiet();
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        private void btn_LamMoi_Click(object sender, EventArgs e)
        {
            maHoaDonDangChon = null;
            DatLaiBoLoc();
            XoaTrangHoaDon();
            LoadData();
            LoadChiTiet();
        }

        private SqlParameter[] TaoThamSoHoaDon(string ma)
        {
            return new[]
            {
                new SqlParameter("@ma", ma),
                new SqlParameter("@ngayLap", dtp_NgayLap.Value),
                new SqlParameter("@khachHang", txt_KhachHang.Text.Trim()),
                new SqlParameter("@trangThai", cbo_TrangThai.SelectedItem!.ToString()),
                new SqlParameter("@ghiChu", DbHelper.ToDbValue(txt_GhiChu.Text))
            };
        }

        // Hóa đơn vừa thêm/sửa nằm ngoài khoảng ngày đang lọc thì nới bộ lọc để vẫn thấy nó
        private void MoRongBoLocTheoNgay(DateTime ngay)
        {
            dangKhoiTao = true;
            if (ngay.Date < dtp_TuNgay.Value.Date) dtp_TuNgay.Value = ngay.Date;
            if (ngay.Date > dtp_DenNgay.Value.Date) dtp_DenNgay.Value = ngay.Date;
            dangKhoiTao = false;
        }

        // ================== CHI TIẾT HÓA ĐƠN ==================

        private void LoadChiTiet()
        {
            XoaTrangChiTiet();

            if (maHoaDonDangChon == null)
            {
                dgv_ChiTiet.DataSource = null;
                lbl_ChiTietHD.Text = "Chọn một hóa đơn trong danh sách để xem chi tiết";
                lbl_TongTien.Text = "Tổng: 0 đ";
                return;
            }

            try
            {
                DataTable dt = DbHelper.ExecuteQuery(@"
                    SELECT MaCTHD, LoaiGiaoDich, NoiDung, SoLuong, DonGia, ThanhTien
                    FROM ChiTietHoaDon
                    WHERE MaHoaDon = @ma
                    ORDER BY MaCTHD",
                    new SqlParameter("@ma", maHoaDonDangChon));

                dgv_ChiTiet.DataSource = dt;
                dgv_ChiTiet.ClearSelection();

                decimal tong = 0;
                foreach (DataRow r in dt.Rows)
                    tong += Convert.ToDecimal(r["ThanhTien"]);

                lbl_ChiTietHD.Text = "Hóa đơn " + maHoaDonDangChon + " - " + txt_KhachHang.Text.Trim() + " (" + dt.Rows.Count + " dòng)";
                lbl_TongTien.Text = "Tổng: " + frmTongQuan.FormatTien(tong);
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        private void XoaTrangChiTiet()
        {
            maCTHDDangChon = null;
            cbo_LoaiGD.SelectedIndex = 0;
            txt_NoiDung.Clear();
            nud_SoLuong.Value = 1;
            txt_DonGia.Clear();
            dgv_ChiTiet.ClearSelection();
        }

        private bool KiemTraChiTiet(out decimal donGia)
        {
            donGia = 0;
            if (maHoaDonDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn (hoặc thêm) một hóa đơn trước khi nhập chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cbo_LoaiGD.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn loại giao dịch!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Cho phép nhập "5000000", "5.000.000" hoặc "5,000,000"
            string text = txt_DonGia.Text.Replace(".", "").Replace(",", "").Replace("đ", "").Trim();
            if (!decimal.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out donGia) || donGia < 0)
            {
                MessageBox.Show("Đơn giá phải là số và không âm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_DonGia.Focus();
                return false;
            }
            return true;
        }

        // Bấm vào 1 dòng chi tiết -> đổ lên các ô để sửa
        private void dgv_ChiTiet_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgv_ChiTiet.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;

            maCTHDDangChon = Convert.ToInt32(row["MaCTHD"]);
            cbo_LoaiGD.SelectedItem = row["LoaiGiaoDich"].ToString();
            txt_NoiDung.Text = row["NoiDung"].ToString();
            nud_SoLuong.Value = Math.Min(nud_SoLuong.Maximum, Convert.ToDecimal(row["SoLuong"]));
            txt_DonGia.Text = Convert.ToDecimal(row["DonGia"]).ToString("0");
        }

        private void btn_ThemCT_Click(object? sender, EventArgs e)
        {
            if (!KiemTraChiTiet(out decimal donGia)) return;

            try
            {
                DbHelper.ExecuteNonQuery(
                    "INSERT INTO ChiTietHoaDon (MaHoaDon, LoaiGiaoDich, NoiDung, SoLuong, DonGia) VALUES (@ma, @loai, @noiDung, @soLuong, @donGia)",
                    new SqlParameter("@ma", maHoaDonDangChon),
                    new SqlParameter("@loai", cbo_LoaiGD.SelectedItem!.ToString()),
                    new SqlParameter("@noiDung", DbHelper.ToDbValue(txt_NoiDung.Text)),
                    new SqlParameter("@soLuong", (int)nud_SoLuong.Value),
                    new SqlParameter("@donGia", donGia));

                LoadData();     // Cập nhật cột Tổng tiền của hóa đơn
                LoadChiTiet();
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        private void btn_SuaCT_Click(object? sender, EventArgs e)
        {
            if (maCTHDDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn dòng chi tiết cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!KiemTraChiTiet(out decimal donGia)) return;

            try
            {
                DbHelper.ExecuteNonQuery(
                    "UPDATE ChiTietHoaDon SET LoaiGiaoDich = @loai, NoiDung = @noiDung, SoLuong = @soLuong, DonGia = @donGia WHERE MaCTHD = @id",
                    new SqlParameter("@id", maCTHDDangChon),
                    new SqlParameter("@loai", cbo_LoaiGD.SelectedItem!.ToString()),
                    new SqlParameter("@noiDung", DbHelper.ToDbValue(txt_NoiDung.Text)),
                    new SqlParameter("@soLuong", (int)nud_SoLuong.Value),
                    new SqlParameter("@donGia", donGia));

                LoadData();
                LoadChiTiet();
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        private void btn_XoaCT_Click(object? sender, EventArgs e)
        {
            if (maCTHDDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn dòng chi tiết cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Xóa dòng chi tiết đang chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                DbHelper.ExecuteNonQuery("DELETE FROM ChiTietHoaDon WHERE MaCTHD = @id", new SqlParameter("@id", maCTHDDangChon));
                LoadData();
                LoadChiTiet();
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        private void btn_LamMoiCT_Click(object? sender, EventArgs e)
        {
            XoaTrangChiTiet();
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
