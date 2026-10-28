using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using QuanLyWebTinTuc.Model;

namespace QuanLyWebTinTuc
{
    // Quản lý tài khoản đăng nhập + vai trò (chỉ Admin mới mở được form này)
    public partial class frmTaiKhoan : Form
    {
        public frmTaiKhoan()
        {
            InitializeComponent();
            DbHelper.SetupGrid(dgv_TaiKhoan);
            txt_MatKhau.PasswordChar = '*';
        }

        private void frmTaiKhoan_Load(object sender, EventArgs e)
        {
            XoaTrang();
            LoadData();
        }

        // Tải danh sách tài khoản lên DataGridView (có lọc theo ô tìm kiếm)
        private void LoadData()
        {
            try
            {
                dgv_TaiKhoan.DataSource = DbHelper.ExecuteQuery(@"
                    SELECT TenDangNhap, HoTen, VaiTro, TrangThai,
                           CASE VaiTro WHEN 'Admin' THEN N'Quản trị viên' ELSE N'Nhân viên' END AS TenVaiTro,
                           CASE TrangThai WHEN 1 THEN N'Hoạt động' ELSE N'Đã khóa' END AS TenTrangThai
                    FROM TaiKhoan
                    WHERE TenDangNhap LIKE @tuKhoa OR HoTen LIKE @tuKhoa
                    ORDER BY VaiTro, TenDangNhap",
                    new SqlParameter("@tuKhoa", "%" + txt_TimKiem.Text.Trim() + "%"));
                dgv_TaiKhoan.ClearSelection();
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        // Combobox vai trò: 0 = Quản trị viên, 1 = Nhân viên
        private string VaiTroDangChon => cbo_VaiTro.SelectedIndex == 0 ? PhienDangNhap.VaiTroAdmin : PhienDangNhap.VaiTroNhanVien;

        private static bool LaTaiKhoanDangDangNhap(string tenDangNhap)
        {
            return string.Equals(tenDangNhap, PhienDangNhap.TenDangNhap, StringComparison.OrdinalIgnoreCase);
        }

        private void XoaTrang()
        {
            txt_TenDangNhap.Clear();
            txt_TenDangNhap.ReadOnly = false;
            txt_HoTen.Clear();
            txt_MatKhau.Clear();
            cbo_VaiTro.SelectedIndex = 1; // Mặc định là Nhân viên
            chk_HoatDong.Checked = true;
            dgv_TaiKhoan.ClearSelection();
            txt_TenDangNhap.Focus();
        }

        private bool KiemTraNhap(bool themMoi)
        {
            if (!Regex.IsMatch(txt_TenDangNhap.Text.Trim(), @"^[A-Za-z0-9_.]{3,50}$"))
            {
                MessageBox.Show("Tên đăng nhập dài 3-50 ký tự, chỉ gồm chữ không dấu, số, dấu _ và dấu .!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_TenDangNhap.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_HoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_HoTen.Focus();
                return false;
            }
            if (themMoi && txt_MatKhau.Text == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu cho tài khoản mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_MatKhau.Focus();
                return false;
            }
            if (txt_MatKhau.Text != "" && txt_MatKhau.Text.Length < 3)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 3 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_MatKhau.Focus();
                return false;
            }
            return true;
        }

        // Bấm vào 1 dòng -> đổ dữ liệu lên các ô nhập
        private void dgv_TaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgv_TaiKhoan.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;

            txt_TenDangNhap.Text = row["TenDangNhap"].ToString();
            txt_TenDangNhap.ReadOnly = true; // Không cho sửa khóa chính
            txt_HoTen.Text = row["HoTen"].ToString();
            txt_MatKhau.Clear();             // Để trống = giữ mật khẩu cũ
            cbo_VaiTro.SelectedIndex = row["VaiTro"].ToString() == PhienDangNhap.VaiTroAdmin ? 0 : 1;
            chk_HoatDong.Checked = Convert.ToBoolean(row["TrangThai"]);
        }

        private void btn_Them_Click(object sender, EventArgs e)
        {
            if (txt_TenDangNhap.ReadOnly)
            {
                MessageBox.Show("Đang chọn tài khoản có sẵn, bấm \"Làm mới\" để nhập tài khoản mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!KiemTraNhap(true)) return;

            try
            {
                string ten = txt_TenDangNhap.Text.Trim();
                DbHelper.ExecuteNonQuery(
                    "INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen, VaiTro, TrangThai) VALUES (@ten, @matKhau, @hoTen, @vaiTro, @trangThai)",
                    new SqlParameter("@ten", ten),
                    new SqlParameter("@matKhau", PhienDangNhap.MaHoaMatKhau(txt_MatKhau.Text)),
                    new SqlParameter("@hoTen", txt_HoTen.Text.Trim()),
                    new SqlParameter("@vaiTro", VaiTroDangChon),
                    new SqlParameter("@trangThai", chk_HoatDong.Checked));

                MessageBox.Show("Thêm tài khoản " + ten + " thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (!txt_TenDangNhap.ReadOnly)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần sửa trong danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!KiemTraNhap(false)) return;

            string ten = txt_TenDangNhap.Text.Trim();

            // Không cho tự hạ quyền / tự khóa chính mình -> luôn còn ít nhất 1 Admin dùng được
            if (LaTaiKhoanDangDangNhap(ten) && (VaiTroDangChon != PhienDangNhap.VaiTroAdmin || !chk_HoatDong.Checked))
            {
                MessageBox.Show("Không thể tự hạ quyền hoặc khóa tài khoản đang đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Ô mật khẩu để trống thì giữ nguyên mật khẩu cũ
                object matKhau = txt_MatKhau.Text == "" ? DBNull.Value : PhienDangNhap.MaHoaMatKhau(txt_MatKhau.Text);

                int soDong = DbHelper.ExecuteNonQuery(@"
                    UPDATE TaiKhoan
                    SET HoTen = @hoTen, VaiTro = @vaiTro, TrangThai = @trangThai,
                        MatKhau = ISNULL(@matKhau, MatKhau)
                    WHERE TenDangNhap = @ten",
                    new SqlParameter("@ten", ten),
                    new SqlParameter("@hoTen", txt_HoTen.Text.Trim()),
                    new SqlParameter("@vaiTro", VaiTroDangChon),
                    new SqlParameter("@trangThai", chk_HoatDong.Checked),
                    new SqlParameter("@matKhau", SqlDbType.VarChar, 64) { Value = matKhau });

                if (soDong == 0)
                {
                    MessageBox.Show("Không tìm thấy tài khoản để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("Cập nhật tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_MatKhau.Clear();
                LoadData();
            }
            catch (Exception ex)
            {
                DbHelper.ShowError(ex);
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            string ten = txt_TenDangNhap.Text.Trim();
            if (!txt_TenDangNhap.ReadOnly || ten == "")
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa trong danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (LaTaiKhoanDangDangNhap(ten))
            {
                MessageBox.Show("Không thể xóa tài khoản đang đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa tài khoản " + ten + "?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                DbHelper.ExecuteNonQuery("DELETE FROM TaiKhoan WHERE TenDangNhap = @ten", new SqlParameter("@ten", ten));
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
