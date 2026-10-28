using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using QuanLyWebTinTuc.Model;

namespace QuanLyWebTinTuc
{
    public partial class frmChuyenMuc : Form
    {
        public frmChuyenMuc()
        {
            InitializeComponent();
            DbHelper.SetupGrid(dgv_ChuyenMuc);
            btn_Xoa.Enabled = PhienDangNhap.LaAdmin; // Chỉ Admin được xóa
        }

        private void frmChuyenMuc_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // Tải danh sách chuyên mục lên DataGridView (có lọc theo ô tìm kiếm)
        private void LoadData()
        {
            try
            {
                dgv_ChuyenMuc.DataSource = DbHelper.ExecuteQuery(@"
                    SELECT MaChuyenMuc, TenChuyenMuc, MoTa
                    FROM ChuyenMucTin
                    WHERE MaChuyenMuc LIKE @tuKhoa OR TenChuyenMuc LIKE @tuKhoa OR MoTa LIKE @tuKhoa
                    ORDER BY MaChuyenMuc",
                    new SqlParameter("@tuKhoa", "%" + txt_TimKiem.Text.Trim() + "%"));
                dgv_ChuyenMuc.ClearSelection();
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        private void XoaTrang()
        {
            txt_MaChuyenMuc.Clear();
            txt_TenChuyenMuc.Clear();
            txt_MoTa.Clear();
            txt_MaChuyenMuc.ReadOnly = false;
            dgv_ChuyenMuc.ClearSelection();
            txt_TenChuyenMuc.Focus();
        }

        private bool KiemTraNhap()
        {
            if (string.IsNullOrWhiteSpace(txt_TenChuyenMuc.Text))
            {
                MessageBox.Show("Vui lòng nhập tên chuyên mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_TenChuyenMuc.Focus();
                return false;
            }
            return true;
        }

        // Bấm vào 1 dòng -> đổ dữ liệu lên các ô nhập
        private void dgv_ChuyenMuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgv_ChuyenMuc.Rows[e.RowIndex];
            txt_MaChuyenMuc.Text = row.Cells["Column1"].Value?.ToString();
            txt_TenChuyenMuc.Text = row.Cells["Column2"].Value?.ToString();
            txt_MoTa.Text = row.Cells["Column3"].Value?.ToString();
            txt_MaChuyenMuc.ReadOnly = true; // Không cho sửa khóa chính
        }

        private void btn_Them_Click(object sender, EventArgs e)
        {
            if (!KiemTraNhap()) return;

            try
            {
                // Đang chọn 1 dòng có sẵn thì tạo mã mới, không ghi đè
                string ma = txt_MaChuyenMuc.ReadOnly || string.IsNullOrWhiteSpace(txt_MaChuyenMuc.Text)
                    ? DbHelper.TaoMaMoi("ChuyenMucTin", "MaChuyenMuc", "CM", 2)
                    : txt_MaChuyenMuc.Text.Trim();

                DbHelper.ExecuteNonQuery(
                    "INSERT INTO ChuyenMucTin (MaChuyenMuc, TenChuyenMuc, MoTa) VALUES (@ma, @ten, @moTa)",
                    new SqlParameter("@ma", ma),
                    new SqlParameter("@ten", txt_TenChuyenMuc.Text.Trim()),
                    new SqlParameter("@moTa", DbHelper.ToDbValue(txt_MoTa.Text)));

                MessageBox.Show("Thêm chuyên mục " + ma + " thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (string.IsNullOrWhiteSpace(txt_MaChuyenMuc.Text))
            {
                MessageBox.Show("Vui lòng chọn chuyên mục cần sửa trong danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!KiemTraNhap()) return;

            try
            {
                int soDong = DbHelper.ExecuteNonQuery(
                    "UPDATE ChuyenMucTin SET TenChuyenMuc = @ten, MoTa = @moTa WHERE MaChuyenMuc = @ma",
                    new SqlParameter("@ma", txt_MaChuyenMuc.Text.Trim()),
                    new SqlParameter("@ten", txt_TenChuyenMuc.Text.Trim()),
                    new SqlParameter("@moTa", DbHelper.ToDbValue(txt_MoTa.Text)));

                if (soDong == 0)
                {
                    MessageBox.Show("Không tìm thấy chuyên mục để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("Cập nhật chuyên mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            string ma = txt_MaChuyenMuc.Text.Trim();
            if (string.IsNullOrEmpty(ma))
            {
                MessageBox.Show("Vui lòng chọn chuyên mục cần xóa trong danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Chuyên mục đang có bài viết thì không xóa được (khóa ngoại)
                int soBai = Convert.ToInt32(DbHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM BaiVietBao WHERE MaChuyenMuc = @ma", new SqlParameter("@ma", ma)));
                if (soBai > 0)
                {
                    MessageBox.Show("Chuyên mục " + ma + " đang có " + soBai + " bài viết, không thể xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Bạn có chắc muốn xóa chuyên mục " + ma + "?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                DbHelper.ExecuteNonQuery("DELETE FROM ChuyenMucTin WHERE MaChuyenMuc = @ma", new SqlParameter("@ma", ma));
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

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
