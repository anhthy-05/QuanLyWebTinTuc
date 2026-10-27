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
        // true khi người dùng bấm Đăng xuất (frmLogin dựa vào đây để hiện lại màn hình đăng nhập)
        public bool DaDangXuat { get; private set; }

        public frmTongQuan()
        {
            InitializeComponent();
        }

        private void LoadThongKe()
        {
            try
            {
                // 1. Chuyên mục
                int tongChuyenMuc = Convert.ToInt32(DbHelper.ExecuteScalar("SELECT COUNT(*) FROM ChuyenMucTin"));
                int tongBaiViet = Convert.ToInt32(DbHelper.ExecuteScalar("SELECT COUNT(*) FROM BaiVietBao"));
                lbl_TongChuyenMuc.Text = tongChuyenMuc + " Chuyên mục";
                lbl_ChuyenMucPhu.Text = "Tổng " + tongBaiViet + " bài viết";

                // 2. Tác giả
                int tongTacGia = Convert.ToInt32(DbHelper.ExecuteScalar("SELECT COUNT(*) FROM TacGiaTinTuc"));
                int tacGiaCoBai = Convert.ToInt32(DbHelper.ExecuteScalar("SELECT COUNT(DISTINCT MaTacGia) FROM BaiVietBao"));
                lbl_TongTacGia.Text = tongTacGia + " Tác giả";
                lbl_TacGiaPhu.Text = tacGiaCoBai + " tác giả đã có bài viết";

                // 3. Giao dịch (hóa đơn) theo trạng thái
                DataTable dtGiaoDich = DbHelper.ExecuteQuery(@"
                    SELECT COUNT(*) AS Tong,
                           SUM(CASE WHEN TrangThai = N'Chờ thanh toán' THEN 1 ELSE 0 END) AS ChoThanhToan,
                           SUM(CASE WHEN TrangThai = N'Đã thanh toán' THEN 1 ELSE 0 END) AS DaThanhToan,
                           SUM(CASE WHEN TrangThai = N'Đã hủy' THEN 1 ELSE 0 END) AS DaHuy
                    FROM HoaDon");
                DataRow gd = dtGiaoDich.Rows[0];
                lbl_TongGiaoDich.Text = gd["Tong"] + " Giao dịch";
                lbl_GiaoDichPhu.Text = string.Format("Chờ TT: {0}  |  Đã TT: {1}  |  Hủy: {2}",
                    ToInt(gd["ChoThanhToan"]), ToInt(gd["DaThanhToan"]), ToInt(gd["DaHuy"]));

                // 4. Tổng tiền: chỉ tính hóa đơn đã thanh toán.
                //    Quảng cáo + Gói bài đăng là tiền thu, Nhuận bút là tiền chi.
                DataTable dtTien = DbHelper.ExecuteQuery(@"
                    SELECT ISNULL(SUM(CASE WHEN ct.LoaiGiaoDich <> N'Nhuận bút' THEN ct.ThanhTien END), 0) AS TongThu,
                           ISNULL(SUM(CASE WHEN ct.LoaiGiaoDich = N'Nhuận bút' THEN ct.ThanhTien END), 0) AS TongChi
                    FROM ChiTietHoaDon ct
                    INNER JOIN HoaDon h ON ct.MaHoaDon = h.MaHoaDon
                    WHERE h.TrangThai = N'Đã thanh toán'");
                decimal tongThu = Convert.ToDecimal(dtTien.Rows[0]["TongThu"]);
                decimal tongChi = Convert.ToDecimal(dtTien.Rows[0]["TongChi"]);
                lbl_TongDoanhThu.Text = FormatTien(tongThu - tongChi);
                lbl_TongTienPhu.Text = "Thu: " + FormatTien(tongThu) + "  |  Nhuận bút: " + FormatTien(tongChi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu!\nLỗi: " + DbHelper.GetErrorMessage(ex), "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static int ToInt(object value)
        {
            return value == DBNull.Value ? 0 : Convert.ToInt32(value);
        }

        public static string FormatTien(decimal soTien)
        {
            return soTien.ToString("#,##0", new System.Globalization.CultureInfo("vi-VN")) + " đ";
        }

        private Form? activeForm = null;

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
                // Đóng form chính, frmLogin sẽ tự hiện lại
                DaDangXuat = true;
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
