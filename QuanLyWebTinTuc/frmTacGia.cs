using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QuanLyWebTinTuc
{
    public partial class frmTacGia : Form
    {
        public frmTacGia()
        {
            InitializeComponent();
            DbHelper.SetupGrid(dgv_TacGia);
        }

        private void frmTacGia_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // Tải danh sách tác giả lên DataGridView (có lọc theo ô tìm kiếm)
        private void LoadData()
        {
            try
            {
                dgv_TacGia.DataSource = DbHelper.ExecuteQuery(@"
                    SELECT MaTacGia, HoTen, Email, SDT, ButDanh
                    FROM TacGiaTinTuc
                    WHERE MaTacGia LIKE @tuKhoa OR HoTen LIKE @tuKhoa OR Email LIKE @tuKhoa
                       OR SDT LIKE @tuKhoa OR ButDanh LIKE @tuKhoa
                    ORDER BY MaTacGia",
                    new SqlParameter("@tuKhoa", "%" + txt_TimKiem.Text.Trim() + "%"));
                dgv_TacGia.ClearSelection();
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        private void XoaTrang()
        {
            txt_MaTacGia.Clear();
            txt_HoTen.Clear();
            txt_Email.Clear();
            txt_SDT.Clear();
            txt_ButDanh.Clear();
            txt_MaTacGia.ReadOnly = false;
            dgv_TacGia.ClearSelection();
            txt_HoTen.Focus();
        }

        private bool KiemTraNhap()
        {
            if (string.IsNullOrWhiteSpace(txt_HoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên tác giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_HoTen.Focus();
                return false;
            }

            string email = txt_Email.Text.Trim();
            if (email != "" && !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_Email.Focus();
                return false;
            }

            string sdt = txt_SDT.Text.Trim();
            if (sdt != "" && !Regex.IsMatch(sdt, @"^0\d{9,10}$"))
            {
                MessageBox.Show("Số điện thoại phải bắt đầu bằng 0 và có 10-11 chữ số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_SDT.Focus();
                return false;
            }
            return true;
        }

        private SqlParameter[] TaoThamSo(string ma)
        {
            return new[]
            {
                new SqlParameter("@ma", ma),
                new SqlParameter("@hoTen", txt_HoTen.Text.Trim()),
                new SqlParameter("@email", DbHelper.ToDbValue(txt_Email.Text)),
                new SqlParameter("@sdt", DbHelper.ToDbValue(txt_SDT.Text)),
                new SqlParameter("@butDanh", DbHelper.ToDbValue(txt_ButDanh.Text))
            };
        }

        // Bấm vào 1 dòng -> đổ dữ liệu lên các ô nhập
        private void dgv_TacGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgv_TacGia.Rows[e.RowIndex];
            txt_MaTacGia.Text = row.Cells["Column1"].Value?.ToString();
            txt_HoTen.Text = row.Cells["Column2"].Value?.ToString();
            txt_Email.Text = row.Cells["Column3"].Value?.ToString();
            txt_SDT.Text = row.Cells["Column4"].Value?.ToString();
            txt_ButDanh.Text = row.Cells["Column5"].Value?.ToString();
            txt_MaTacGia.ReadOnly = true; // Không cho sửa khóa chính
        }

        private void btn_Them_Click(object sender, EventArgs e)
        {
            if (!KiemTraNhap()) return;

            try
            {
                // Đang chọn 1 dòng có sẵn thì tạo mã mới, không ghi đè
                string ma = txt_MaTacGia.ReadOnly || string.IsNullOrWhiteSpace(txt_MaTacGia.Text)
                    ? DbHelper.TaoMaMoi("TacGiaTinTuc", "MaTacGia", "TG", 2)
                    : txt_MaTacGia.Text.Trim();

                DbHelper.ExecuteNonQuery(
                    "INSERT INTO TacGiaTinTuc (MaTacGia, HoTen, Email, SDT, ButDanh) VALUES (@ma, @hoTen, @email, @sdt, @butDanh)",
                    TaoThamSo(ma));

                MessageBox.Show("Thêm tác giả " + ma + " thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                XoaTrang();
                LoadData();
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_MaTacGia.Text))
            {
                MessageBox.Show("Vui lòng chọn tác giả cần sửa trong danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!KiemTraNhap()) return;

            try
            {
                int soDong = DbHelper.ExecuteNonQuery(
                    "UPDATE TacGiaTinTuc SET HoTen = @hoTen, Email = @email, SDT = @sdt, ButDanh = @butDanh WHERE MaTacGia = @ma",
                    TaoThamSo(txt_MaTacGia.Text.Trim()));

                if (soDong == 0)
                {
                    MessageBox.Show("Không tìm thấy tác giả để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("Cập nhật tác giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            string ma = txt_MaTacGia.Text.Trim();
            if (string.IsNullOrEmpty(ma))
            {
                MessageBox.Show("Vui lòng chọn tác giả cần xóa trong danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Tác giả đang có bài viết thì không xóa được (khóa ngoại)
                int soBai = Convert.ToInt32(DbHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM BaiVietBao WHERE MaTacGia = @ma", new SqlParameter("@ma", ma)));
                if (soBai > 0)
                {
                    MessageBox.Show("Tác giả " + ma + " đang có " + soBai + " bài viết, không thể xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Bạn có chắc muốn xóa tác giả " + ma + "?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                DbHelper.ExecuteNonQuery("DELETE FROM TacGiaTinTuc WHERE MaTacGia = @ma", new SqlParameter("@ma", ma));
                XoaTrang();
                LoadData();
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        private void btn_LamMoi_Click(object sender, EventArgs e)
        {
            txt_TimKiem.Clear();
            XoaTrang();
            LoadData();
        }

        private void btn_TimKiem_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txt_TimKiem_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
