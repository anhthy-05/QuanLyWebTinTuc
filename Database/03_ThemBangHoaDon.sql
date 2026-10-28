-- =============================================================
--  BỔ SUNG CHO DATABASE ĐÃ CÓ SẴN
--  Thêm 2 bảng HoaDon, ChiTietHoaDon + view V_HoaDonTongTien + dữ liệu mẫu.
--  (Máy tạo mới database bằng 01_TaoCSDL.sql thì KHÔNG cần chạy file này.)
--  Chạy xong file này thì chạy tiếp 04_TaiKhoan.sql.
-- =============================================================
USE QuanLyTrangWebTinTuc;
GO

IF OBJECT_ID(N'HoaDon', N'U') IS NULL
CREATE TABLE HoaDon (
    MaHoaDon VARCHAR(20) PRIMARY KEY,
    NgayLap DATETIME NOT NULL DEFAULT GETDATE(),
    KhachHang NVARCHAR(100) NOT NULL,
    TrangThai NVARCHAR(30) NOT NULL DEFAULT N'Chờ thanh toán'
        CHECK (TrangThai IN (N'Chờ thanh toán', N'Đã thanh toán', N'Đã hủy')),
    GhiChu NVARCHAR(MAX)
);
GO

-- Quảng cáo, Gói bài đăng = tiền thu vào; Nhuận bút = tiền chi ra cho tác giả
IF OBJECT_ID(N'ChiTietHoaDon', N'U') IS NULL
CREATE TABLE ChiTietHoaDon (
    MaCTHD INT IDENTITY(1,1) PRIMARY KEY,
    MaHoaDon VARCHAR(20) NOT NULL,
    LoaiGiaoDich NVARCHAR(30) NOT NULL
        CHECK (LoaiGiaoDich IN (N'Quảng cáo', N'Nhuận bút', N'Gói bài đăng')),
    NoiDung NVARCHAR(255),
    SoLuong INT NOT NULL DEFAULT 1 CHECK (SoLuong > 0),
    DonGia DECIMAL(18,0) NOT NULL CHECK (DonGia >= 0),
    ThanhTien AS (SoLuong * DonGia),
    FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon) ON DELETE CASCADE
);
GO

CREATE OR ALTER VIEW V_HoaDonTongTien AS
SELECT
    h.MaHoaDon,
    h.NgayLap,
    h.KhachHang,
    h.TrangThai,
    h.GhiChu,
    ISNULL(SUM(ct.ThanhTien), 0) AS TongTien
FROM HoaDon h
LEFT JOIN ChiTietHoaDon ct ON h.MaHoaDon = ct.MaHoaDon
GROUP BY h.MaHoaDon, h.NgayLap, h.KhachHang, h.TrangThai, h.GhiChu;
GO

-- Dữ liệu mẫu (chỉ thêm khi bảng còn trống)
IF NOT EXISTS (SELECT 1 FROM HoaDon)
BEGIN
    INSERT INTO HoaDon (MaHoaDon, NgayLap, KhachHang, TrangThai, GhiChu) VALUES
    ('HD001', DATEADD(HOUR, -9,  DATEADD(DAY, -40, GETDATE())), N'Công ty TNHH Sao Việt',       N'Đã thanh toán',  N'Banner trang chủ tháng trước'),
    ('HD002', DATEADD(HOUR, -5,  DATEADD(DAY, -32, GETDATE())), N'Nguyễn Văn A (TG01)',         N'Đã thanh toán',  N'Nhuận bút bài BV01, BV03'),
    ('HD003', DATEADD(HOUR, -2,  DATEADD(DAY, -20, GETDATE())), N'Shop Thời Trang Mây',         N'Đã thanh toán',  N'Gói đăng 10 bài PR'),
    ('HD004', DATEADD(HOUR, -7,  DATEADD(DAY, -12, GETDATE())), N'Công ty CP Công Nghệ Lam',    N'Đã hủy',         N'Khách hủy hợp đồng'),
    ('HD005', DATEADD(HOUR, -3,  DATEADD(DAY, -6,  GETDATE())), N'Ngân hàng Phương Nam',        N'Đã thanh toán',  N'Quảng cáo chuyên mục Kinh doanh'),
    ('HD006', DATEADD(HOUR, -1,  DATEADD(DAY, -2,  GETDATE())), N'Trần Thị B (TG02)',           N'Chờ thanh toán', N'Nhuận bút bài BV02'),
    ('HD007', DATEADD(MINUTE, -45, GETDATE()),                 N'Trung tâm Anh ngữ Bình Minh', N'Chờ thanh toán', N'Gói đăng bài + banner');

    INSERT INTO ChiTietHoaDon (MaHoaDon, LoaiGiaoDich, NoiDung, SoLuong, DonGia) VALUES
    ('HD001', N'Quảng cáo',    N'Banner trang chủ (tuần)',        4, 5000000),
    ('HD001', N'Quảng cáo',    N'Popup cuối bài',                 1, 2000000),
    ('HD002', N'Nhuận bút',    N'Bài BV01 - Xu hướng AI năm 2026', 1, 800000),
    ('HD002', N'Nhuận bút',    N'Bài BV03 - Đổi mới giáo dục',     1, 1000000),
    ('HD003', N'Gói bài đăng', N'Gói 10 bài PR',                  1, 15000000),
    ('HD004', N'Quảng cáo',    N'Banner chuyên mục Công nghệ',    2, 3000000),
    ('HD005', N'Quảng cáo',    N'Banner chuyên mục Kinh doanh',   3, 4000000),
    ('HD005', N'Gói bài đăng', N'Gói 3 bài PR',                   1, 5000000),
    ('HD006', N'Nhuận bút',    N'Bài BV02 - Giao thông đô thị',   1, 700000),
    ('HD007', N'Gói bài đăng', N'Gói 5 bài tuyển sinh',           1, 8000000),
    ('HD007', N'Quảng cáo',    N'Banner sidebar (tuần)',          2, 1500000);
END
GO
