using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QuanLyWebTinTuc
{
    public partial class frmLogin : Form
    {
        // Tài khoản đăng nhập mặc định
        private const string TaiKhoan = "admin";
        private const string MatKhau = "111";

        public frmLogin()
        {
            InitializeComponent();
            txt_MatKhau.PasswordChar = '*';
            AcceptButton = btn_DangNhap; // Nhấn Enter để đăng nhập
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void chk_HienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txt_MatKhau.PasswordChar =
            chk_HienMatKhau.Checked ? '\0' : '*';
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_DangNhap_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_TaiKhoan.Text) || string.IsNullOrWhiteSpace(txt_MatKhau.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập đầy đủ tài khoản và mật khẩu!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (txt_TaiKhoan.Text.Trim() != TaiKhoan || txt_MatKhau.Text != MatKhau)
            {
                MessageBox.Show(
                    "Tài khoản hoặc mật khẩu không đúng!",
                    "Đăng nhập thất bại",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                txt_MatKhau.Clear();
                txt_MatKhau.Focus();
                return;
            }

            // Đăng nhập thành công: ẩn form login, mở form chính
            frmTongQuan main = new frmTongQuan();
            main.FormClosed += (s, args) =>
            {
                if (main.DaDangXuat)
                {
                    // Đăng xuất -> quay lại màn hình đăng nhập
                    txt_MatKhau.Clear();
                    chk_HienMatKhau.Checked = false;
                    Show();
                    txt_MatKhau.Focus();
                }
                else
                {
                    // Đóng form chính -> thoát chương trình
                    Close();
                }
            };

            Hide();
            main.Show();
        }
    }
}
