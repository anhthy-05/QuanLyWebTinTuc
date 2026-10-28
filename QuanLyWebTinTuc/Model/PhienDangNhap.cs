using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLyWebTinTuc.Model
{
    public class PhienDangNhap
    {
        public static string TenDangNhap { get; set; } = "";
        public static string VaiTro { get; set; } = "";

        public static bool LaAdmin =>
            VaiTro == "Admin";

        public static bool LaNhanVien =>
            VaiTro == "Staff";
    }
}

