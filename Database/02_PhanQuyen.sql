-- =============================================================
--  PHÂN QUYỀN (chạy sau 01_TaoCSDL.sql hoặc 03_ThemBangHoaDon.sql)
--  Ứng dụng WinForms đăng nhập SQL bằng Windows (Integrated Security)
--  nên file này không bắt buộc để chạy app.
-- =============================================================
USE QuanLyTrangWebTinTuc;
GO

-- 1. Tạo Login trên hệ thống (CHECK_POLICY = OFF để chấp nhận mật khẩu ngắn)
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'Login_NewsAdmin')
    CREATE LOGIN Login_NewsAdmin WITH PASSWORD = 'admin', CHECK_POLICY = OFF;
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'Login_NewsStaff')
    CREATE LOGIN Login_NewsStaff WITH PASSWORD = '111', CHECK_POLICY = OFF;
GO

-- 2. Tạo User tương ứng trong cơ sở dữ liệu hiện tại
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'User_NewsAdmin')
    CREATE USER User_NewsAdmin FOR LOGIN Login_NewsAdmin;
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'User_NewsStaff')
    CREATE USER User_NewsStaff FOR LOGIN Login_NewsStaff;
GO

-- 3. Tạo các Vai trò (Database Roles) cho hệ thống
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'Role_Admin')
    CREATE ROLE Role_Admin;
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'Role_Staff')
    CREATE ROLE Role_Staff;
GO

-- Gán User vào Role
ALTER ROLE Role_Admin ADD MEMBER User_NewsAdmin;
ALTER ROLE Role_Staff ADD MEMBER User_NewsStaff;
GO

-- 4. PHÂN QUYỀN CHI TIẾT (GRANT / REVOKE)

--- a) Nhóm Admin (Role_Admin): Toàn quyền quản trị
GRANT CONTROL, SELECT, INSERT, UPDATE, DELETE ON DATABASE::QuanLyTrangWebTinTuc TO Role_Admin;
GO

--- b) Nhóm Nhân viên/Biên tập viên (Role_Staff):
-- Cho phép đọc các bảng và view
GRANT SELECT ON V_DanhSachChuaXuLy TO Role_Staff;
GRANT SELECT ON V_ThongKeTongHop TO Role_Staff;
GRANT SELECT ON V_HoaDonTongTien TO Role_Staff;
GRANT SELECT ON BaiVietBao TO Role_Staff;
GRANT SELECT ON ChuyenMucTin TO Role_Staff;
GRANT SELECT ON HoaDon TO Role_Staff;
GRANT SELECT ON ChiTietHoaDon TO Role_Staff;

-- Cho phép thực thi Stored Procedure và Function nghiệp vụ
GRANT EXECUTE ON P_DanhSachGiaoDich TO Role_Staff;
GRANT EXECUTE ON P_ThemSuaXoa TO Role_Staff;
GRANT EXECUTE ON F_TinhTongGiaTri TO Role_Staff;

-- Thu hồi quyền DELETE trực tiếp trên bảng bài viết (bắt buộc dùng SP hoặc thông qua Admin)
REVOKE DELETE ON BaiVietBao FROM Role_Staff;
GO
