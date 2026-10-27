using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QuanLyWebTinTuc
{
    public partial class frmTongQuan : Form
    {
        public frmTongQuan()
        {
            InitializeComponent();
        }

        public static class Global
        {
            public static string ConnectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=QuanLyTrangWebTinTuc;Integrated Security=True;TrustServerCertificate=True;";
        }
        private void LoadThongKe()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Global.ConnectionString))
                {
                    conn.Open();

                    // 1. Đếm tổng chuyên mục
                    SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM ChuyenMucTin", conn);
                    lbl_TongChuyenMuc.Text = cmd1.ExecuteScalar().ToString() + " Chuyên mục";

                    // 2. Đếm tổng tác giả
                    SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM TacGiaTinTuc", conn);
                    lbl_TongTacGia.Text = cmd2.ExecuteScalar().ToString() + " Tác giả";

                    // 3. Đếm tổng bài viết
                    SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM BaiVietBao", conn);
                    lbl_TongDoanhThu.Text = cmd3.ExecuteScalar().ToString() + " Bài viết";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu!\nLỗi: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private Form activeForm = null;

        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnl_MainContext.Controls.Clear(); // Xóa sạch khung chứa
            pnl_MainContext.Controls.Add(childForm); // Bỏ form con vào
            childForm.BringToFront();
            childForm.Show();
        }

        // Sự kiện bấm nút Tổng quan
        private void btn_TongQuan_Click(object sender, EventArgs e)
        {
            // 1. Đóng form Chuyên mục đang mở
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm = null;
            }

            // 2. Gắn lại Panel Tổng quan vào khung chứa
            pnl_MainContext.Controls.Clear();
            pnl_MainContext.Controls.Add(pnl_TongQuan);
            pnl_TongQuan.Dock = DockStyle.Fill;
            pnl_TongQuan.Visible = true;

            // 3. Cập nhật số liệu
            LoadThongKe();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Ép Panel Tổng quan nạp thẳng vào Main Content ngay từ đầu
            pnl_MainContext.Controls.Clear();
            pnl_MainContext.Controls.Add(pnl_TongQuan);
            pnl_TongQuan.Dock = DockStyle.Fill;

            LoadThongKe();
        }

        private void btn_ChuyenMuc_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmChuyenMuc());

        }
        private void btn_DangXuat_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có muốn đăng xuất không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                this.Hide();
                frmLogin login = new frmLogin();
                login.ShowDialog();
                this.Close();
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_TacGia_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmTacGia());
        }

        private void btn_GiaoDich_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmGiaoDich());
        }

        private void pnl_MainContext_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnl_TongQuan_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
