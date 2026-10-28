using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyWebTinTuc.Model
{
    // Lưu thông tin tài khoản đang đăng nhập, các form dựa vào đây để phân quyền
    public static class PhienDangNhap
    {
        // Giá trị cột VaiTro trong bảng TaiKhoan
        public const string VaiTroAdmin = "Admin";
        public const string VaiTroNhanVien = "Staff";

        public static string TenDangNhap { get; private set; } = "";
        public static string HoTen { get; private set; } = "";
        public static string VaiTro { get; private set; } = "";

        public static bool LaAdmin =>
            VaiTro == VaiTroAdmin;

        public static bool LaNhanVien =>
            VaiTro == VaiTroNhanVien;

        public static string TenVaiTro(string vaiTro)
        {
            return vaiTro == VaiTroAdmin ? "Quản trị viên" : "Nhân viên";
        }

        // Kiểm tra tài khoản trong database. Trả về null nếu thành công, ngược lại trả về câu thông báo lỗi
        public static string? DangNhap(string tenDangNhap, string matKhau)
        {
            DataTable dt = DbHelper.ExecuteQuery(
                "SELECT TenDangNhap, HoTen, VaiTro, TrangThai FROM TaiKhoan WHERE TenDangNhap = @ten AND MatKhau = @matKhau",
                new SqlParameter("@ten", tenDangNhap),
                new SqlParameter("@matKhau", MaHoaMatKhau(matKhau)));

            if (dt.Rows.Count == 0)
                return "Tài khoản hoặc mật khẩu không đúng!";

            DataRow row = dt.Rows[0];
            if (!Convert.ToBoolean(row["TrangThai"]))
                return "Tài khoản đã bị khóa, vui lòng liên hệ quản trị viên!";

            TenDangNhap = row["TenDangNhap"].ToString()!;
            HoTen = row["HoTen"].ToString()!;
            VaiTro = row["VaiTro"].ToString()!;
            return null;
        }

        public static void DangXuat()
        {
            TenDangNhap = "";
            HoTen = "";
            VaiTro = "";
        }

        // SHA-256 dạng hex in hoa, giống CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', ...), 2) bên SQL
        public static string MaHoaMatKhau(string matKhau)
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(matKhau)));
        }
    }
}
