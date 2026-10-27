using Microsoft.Data.SqlClient;
using System.Data;

namespace QuanLyWebTinTuc
{
    // Lớp dùng chung để kết nối và chạy câu lệnh SQL
    public static class DbHelper
    {
        public static string ConnectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=QuanLyTrangWebTinTuc;Integrated Security=True;TrustServerCertificate=True;";

        // Chạy câu SELECT, trả về bảng dữ liệu
        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddRange(parameters);
                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);
                return dt;
            }
        }

        // Chạy INSERT / UPDATE / DELETE, trả về số dòng bị ảnh hưởng
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        // Chạy câu lệnh trả về 1 giá trị (COUNT, SUM, ...)
        public static object? ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }

        // Tạo mã mới theo dạng tiền tố + số tăng dần, VD: CM01 -> CM06, HD007 -> HD008
        public static string TaoMaMoi(string tenBang, string cotMa, string tienTo, int soChuSo)
        {
            string sql = $@"SELECT ISNULL(MAX(TRY_CAST(SUBSTRING({cotMa}, @len + 1, 20) AS INT)), 0)
                            FROM {tenBang} WHERE {cotMa} LIKE @prefix + '%'";
            object? max = ExecuteScalar(sql,
                new SqlParameter("@len", tienTo.Length),
                new SqlParameter("@prefix", tienTo));
            int so = Convert.ToInt32(max) + 1;
            return tienTo + so.ToString().PadLeft(soChuSo, '0');
        }

        // Chuỗi rỗng -> NULL trong database
        public static object ToDbValue(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? DBNull.Value : text.Trim();
        }

        // Cài đặt chung cho các DataGridView: chọn cả dòng, chỉ xem, không cho thêm dòng trống
        public static void SetupGrid(DataGridView grid)
        {
            grid.AutoGenerateColumns = false;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.MultiSelect = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("vi-VN"); // 17.000.000
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = grid.ColumnHeadersDefaultCellStyle.BackColor;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = grid.ColumnHeadersDefaultCellStyle.ForeColor;
            // Không tự tô chọn dòng đầu tiên khi vừa tải dữ liệu
            grid.DataBindingComplete += (s, e) => grid.ClearSelection();
        }

        // Thông báo lỗi SQL dễ hiểu hơn cho người dùng
        public static string GetErrorMessage(Exception ex)
        {
            if (ex is SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case 2627:
                    case 2601: return "Dữ liệu bị trùng (mã hoặc email đã tồn tại)!";
                    case 547: return "Không thể thực hiện vì dữ liệu đang được sử dụng ở bảng khác hoặc không hợp lệ!";
                    case 2:
                    case 53:
                    case 4060: return "Không kết nối được SQL Server / database QuanLyTrangWebTinTuc!\n" + sqlEx.Message;
                }
            }
            return ex.Message;
        }

        public static void ShowError(Exception ex)
        {
            MessageBox.Show(GetErrorMessage(ex), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
