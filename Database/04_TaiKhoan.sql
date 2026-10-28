-- =============================================================
--  TÀI KHOẢN ĐĂNG NHẬP + PHÂN QUYỀN TRONG ỨNG DỤNG
--  Chạy được cả trên database mới lẫn database đã có sẵn (chạy lại nhiều lần không lỗi).
--
--  Vai trò:
--    Admin    : toàn quyền + quản lý tài khoản
--    Staff    : xem tất cả; thêm/sửa chuyên mục, tác giả; KHÔNG được xóa;
--               giao dịch chỉ được xem
--
--  Mật khẩu lưu dạng SHA-256 (chuỗi hex in hoa), không lưu mật khẩu gốc.
--  Tài khoản mặc định:  admin / 111  (Admin)   -  nhanvien / 111  (Staff)
-- =============================================================
USE QuanLyTrangWebTinTuc;
GO

IF OBJECT_ID(N'TaiKhoan', N'U') IS NULL
CREATE TABLE TaiKhoan (
    TenDangNhap VARCHAR(50) PRIMARY KEY,
    MatKhau VARCHAR(64) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    VaiTro VARCHAR(20) NOT NULL DEFAULT 'Staff'
        CHECK (VaiTro IN ('Admin', 'Staff')),
    TrangThai BIT NOT NULL DEFAULT 1  -- 1 = hoạt động, 0 = bị khóa
);
GO

IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = 'admin')
    INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen, VaiTro)
    VALUES ('admin', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '111'), 2), N'Quản trị viên', 'Admin');

IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = 'nhanvien')
    INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen, VaiTro)
    VALUES ('nhanvien', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '111'), 2), N'Nhân viên biên tập', 'Staff');
GO
