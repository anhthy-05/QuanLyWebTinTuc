-- =============================================================
--  CƠ SỞ DỮ LIỆU: QuanLyTrangWebTinTuc
--  Chạy file này trên máy CHƯA có database (tạo mới toàn bộ).
--  Nếu database đã có sẵn, chỉ cần chạy file 03_ThemBangHoaDon.sql
--  Sau đó chạy tiếp 04_TaiKhoan.sql để tạo tài khoản đăng nhập + phân quyền.
-- =============================================================
IF DB_ID(N'QuanLyTrangWebTinTuc') IS NULL
    CREATE DATABASE QuanLyTrangWebTinTuc;
GO

USE QuanLyTrangWebTinTuc;
GO

-- 1. Bảng TacGiaTinTuc
CREATE TABLE TacGiaTinTuc (
    MaTacGia VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    SDT VARCHAR(20),
    ButDanh NVARCHAR(50)
);

-- 2. Bảng ChuyenMucTin
CREATE TABLE ChuyenMucTin (
    MaChuyenMuc VARCHAR(20) PRIMARY KEY,
    TenChuyenMuc NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(MAX)
);

-- 3. Bảng DocGiaWeb
CREATE TABLE DocGiaWeb (
    MaDocGia VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    NgayDangKy DATETIME DEFAULT GETDATE()
);

-- 4. Bảng BaiVietBao
CREATE TABLE BaiVietBao (
    MaBaiViet VARCHAR(20) PRIMARY KEY,
    TieuDe NVARCHAR(255) NOT NULL,
    TomTat NVARCHAR(MAX),
    NoiDung NVARCHAR(MAX),
    NgayDang DATETIME DEFAULT GETDATE(),
    LuotXem INT DEFAULT 0,
    MaTacGia VARCHAR(20) NOT NULL,
    MaChuyenMuc VARCHAR(20) NOT NULL,
    FOREIGN KEY (MaTacGia) REFERENCES TacGiaTinTuc(MaTacGia),
    FOREIGN KEY (MaChuyenMuc) REFERENCES ChuyenMucTin(MaChuyenMuc)
);

-- 5. Bảng BinhLuanTinTuc
CREATE TABLE BinhLuanTinTuc (
    MaBinhLuan VARCHAR(20) PRIMARY KEY,
    NoiDungBL NVARCHAR(MAX) NOT NULL,
    ThoiGianBL DATETIME DEFAULT GETDATE(),
    MaDocGia VARCHAR(20) NOT NULL,
    MaBaiViet VARCHAR(20) NOT NULL,
    FOREIGN KEY (MaDocGia) REFERENCES DocGiaWeb(MaDocGia),
    FOREIGN KEY (MaBaiViet) REFERENCES BaiVietBao(MaBaiViet) ON DELETE CASCADE
);

-- 6. Bảng HoaDon (giao dịch)
CREATE TABLE HoaDon (
    MaHoaDon VARCHAR(20) PRIMARY KEY,
    NgayLap DATETIME NOT NULL DEFAULT GETDATE(),
    KhachHang NVARCHAR(100) NOT NULL,
    TrangThai NVARCHAR(30) NOT NULL DEFAULT N'Chờ thanh toán'
        CHECK (TrangThai IN (N'Chờ thanh toán', N'Đã thanh toán', N'Đã hủy')),
    GhiChu NVARCHAR(MAX)
);

-- 7. Bảng ChiTietHoaDon
--    Quảng cáo, Gói bài đăng = tiền thu vào; Nhuận bút = tiền chi ra cho tác giả
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

-- =============================================================
--  DỮ LIỆU MẪU
-- =============================================================

-- 1. Tác giả (Bắt buộc phải chạy trước BaiVietBao)
INSERT INTO TacGiaTinTuc (MaTacGia, HoTen, Email, SDT, ButDanh) VALUES
('TG01', N'Nguyễn Văn A', 'nguyena@newswave.com', '0901234567', N'An Nguyễn'),
('TG02', N'Trần Thị B', 'tranb@newswave.com', '0912345678', N'Bảo Trần'),
('TG03', N'Lê Hoàng Minh', 'minhle@newswave.com', '0938765432', N'Minh Hoàng'),
('TG04', N'Phạm Thu Hà', 'hapham@newswave.com', '0977112233', N'Thu Hà');
GO

-- 2. Chuyên mục (Bắt buộc phải chạy trước BaiVietBao)
INSERT INTO ChuyenMucTin (MaChuyenMuc, TenChuyenMuc, MoTa) VALUES
('CM01', N'Thời sự', N'Tin tức thời sự trong và ngoài nước'),
('CM02', N'Công nghệ', N'Xu hướng và tin tức công nghệ mới'),
('CM03', N'Kinh doanh', N'Thị trường, doanh nghiệp và tài chính'),
('CM04', N'Thể thao', N'Bóng đá, quần vợt và các môn thể thao khác'),
('CM05', N'Giải trí', N'Âm nhạc, điện ảnh và đời sống nghệ sĩ');
GO

-- 3. Độc giả
INSERT INTO DocGiaWeb (MaDocGia, HoTen, Email, MatKhau) VALUES
('DG01', N'Lê Văn C', 'lec@gmail.com', 'hashed_pass_123'),
('DG02', N'Phạm Thị D', 'phamd@gmail.com', 'hashed_pass_456');
GO

-- 4. Bài viết
INSERT INTO BaiVietBao (MaBaiViet, TieuDe, TomTat, NoiDung, NgayDang, LuotXem, MaTacGia, MaChuyenMuc) VALUES
('BV01', N'Xu hướng AI năm 2026', N'Trí tuệ nhân tạo phát triển vượt bậc', N'Nội dung chi tiết bài viết AI...', GETDATE(), 150, 'TG01', 'CM02'),
('BV02', N'Tình hình giao thông đô thị', N'Cập nhật hạ tầng giao thông', N'Nội dung giao thông...', GETDATE(), 45, 'TG02', 'CM01'),
('BV03', N'Đổi mới giáo dục đại học', N'Áp dụng công nghệ vào giảng dạy', N'Nội dung đổi mới giáo dục...', DATEADD(DAY, -10, GETDATE()), 300, 'TG01', 'CM01'),
('BV04', N'Thị trường chứng khoán cuối năm', N'Nhà đầu tư thận trọng', N'Nội dung chứng khoán...', DATEADD(DAY, -3, GETDATE()), 210, 'TG03', 'CM03'),
('BV05', N'Đội tuyển chuẩn bị cho vòng loại', N'Danh sách tập trung mới', N'Nội dung thể thao...', DATEADD(DAY, -1, GETDATE()), 520, 'TG04', 'CM04');
GO

-- 5. Bình luận
INSERT INTO BinhLuanTinTuc (MaBinhLuan, NoiDungBL, ThoiGianBL, MaDocGia, MaBaiViet) VALUES
('BL01', N'Bài viết rất hay và bổ ích!', GETDATE(), 'DG01', 'BV01'),
('BL02', N'Tôi đồng ý với quan điểm này.', GETDATE(), 'DG02', 'BV01'),
('BL03', N'Cần cải thiện thêm giao thông công cộng.', GETDATE(), 'DG01', 'BV02');
GO

-- 6. Hóa đơn + chi tiết hóa đơn
INSERT INTO HoaDon (MaHoaDon, NgayLap, KhachHang, TrangThai, GhiChu) VALUES
('HD001', DATEADD(HOUR, -9,  DATEADD(DAY, -40, GETDATE())), N'Công ty TNHH Sao Việt',       N'Đã thanh toán',  N'Banner trang chủ tháng trước'),
('HD002', DATEADD(HOUR, -5,  DATEADD(DAY, -32, GETDATE())), N'Nguyễn Văn A (TG01)',         N'Đã thanh toán',  N'Nhuận bút bài BV01, BV03'),
('HD003', DATEADD(HOUR, -2,  DATEADD(DAY, -20, GETDATE())), N'Shop Thời Trang Mây',         N'Đã thanh toán',  N'Gói đăng 10 bài PR'),
('HD004', DATEADD(HOUR, -7,  DATEADD(DAY, -12, GETDATE())), N'Công ty CP Công Nghệ Lam',    N'Đã hủy',         N'Khách hủy hợp đồng'),
('HD005', DATEADD(HOUR, -3,  DATEADD(DAY, -6,  GETDATE())), N'Ngân hàng Phương Nam',        N'Đã thanh toán',  N'Quảng cáo chuyên mục Kinh doanh'),
('HD006', DATEADD(HOUR, -1,  DATEADD(DAY, -2,  GETDATE())), N'Lê Hoàng Minh (TG03)',        N'Chờ thanh toán', N'Nhuận bút bài BV04'),
('HD007', DATEADD(MINUTE, -45, GETDATE()),                 N'Trung tâm Anh ngữ Bình Minh', N'Chờ thanh toán', N'Gói đăng bài + banner');
GO

INSERT INTO ChiTietHoaDon (MaHoaDon, LoaiGiaoDich, NoiDung, SoLuong, DonGia) VALUES
('HD001', N'Quảng cáo',    N'Banner trang chủ (tuần)',        4, 5000000),
('HD001', N'Quảng cáo',    N'Popup cuối bài',                 1, 2000000),
('HD002', N'Nhuận bút',    N'Bài BV01 - Xu hướng AI năm 2026', 1, 800000),
('HD002', N'Nhuận bút',    N'Bài BV03 - Đổi mới giáo dục',     1, 1000000),
('HD003', N'Gói bài đăng', N'Gói 10 bài PR',                  1, 15000000),
('HD004', N'Quảng cáo',    N'Banner chuyên mục Công nghệ',    2, 3000000),
('HD005', N'Quảng cáo',    N'Banner chuyên mục Kinh doanh',   3, 4000000),
('HD005', N'Gói bài đăng', N'Gói 3 bài PR',                   1, 5000000),
('HD006', N'Nhuận bút',    N'Bài BV04 - Thị trường chứng khoán', 1, 900000),
('HD007', N'Gói bài đăng', N'Gói 5 bài tuyển sinh',           1, 8000000),
('HD007', N'Quảng cáo',    N'Banner sidebar (tuần)',          2, 1500000);
GO

-- =============================================================
--  VIEW / PROCEDURE / FUNCTION / TRIGGER
-- =============================================================

CREATE VIEW V_DanhSachChuaXuLy AS
SELECT
    b.MaBaiViet,
    b.TieuDe,
    b.NgayDang,
    b.LuotXem,
    t.HoTen AS TenTacGia,
    c.TenChuyenMuc
FROM BaiVietBao b
INNER JOIN TacGiaTinTuc t ON b.MaTacGia = t.MaTacGia
INNER JOIN ChuyenMucTin c ON b.MaChuyenMuc = c.MaChuyenMuc
WHERE b.LuotXem < 100
  AND b.NgayDang >= DATEADD(DAY, -7, GETDATE());
GO

CREATE VIEW V_ThongKeTongHop AS
SELECT
    c.MaChuyenMuc,
    c.TenChuyenMuc,
    COUNT(DISTINCT b.MaBaiViet) AS TongSoBaiViet,
    ISNULL(SUM(b.LuotXem), 0) AS TongLuotXem,
    COUNT(bl.MaBinhLuan) AS TongSoBinhLuan
FROM ChuyenMucTin c
LEFT JOIN BaiVietBao b ON c.MaChuyenMuc = b.MaChuyenMuc
LEFT JOIN BinhLuanTinTuc bl ON b.MaBaiViet = bl.MaBaiViet
GROUP BY c.MaChuyenMuc, c.TenChuyenMuc;
GO

-- Danh sách hóa đơn kèm tổng tiền (form Giao dịch dùng view này)
CREATE VIEW V_HoaDonTongTien AS
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

CREATE PROCEDURE P_DanhSachGiaoDich
    @MaChuyenMuc VARCHAR(20),
    @TongSoBai INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Lấy danh sách bài viết thuộc chuyên mục
    SELECT
        b.MaBaiViet,
        b.TieuDe,
        b.NgayDang,
        b.LuotXem,
        t.HoTen AS TenTacGia
    FROM BaiVietBao b
    INNER JOIN TacGiaTinTuc t ON b.MaTacGia = t.MaTacGia
    WHERE b.MaChuyenMuc = @MaChuyenMuc
    ORDER BY b.NgayDang DESC;

    -- Gán giá trị cho tham số Output
    SELECT @TongSoBai = COUNT(*)
    FROM BaiVietBao
    WHERE MaChuyenMuc = @MaChuyenMuc;
END;
GO

CREATE PROCEDURE P_ThemSuaXoa
    @Action VARCHAR(10), -- 'INSERT', 'UPDATE', 'DELETE'
    @MaBaiViet VARCHAR(20),
    @TieuDe NVARCHAR(255) = NULL,
    @TomTat NVARCHAR(MAX) = NULL,
    @NoiDung NVARCHAR(MAX) = NULL,
    @MaTacGia VARCHAR(20) = NULL,
    @MaChuyenMuc VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'INSERT'
    BEGIN
        IF EXISTS (SELECT 1 FROM BaiVietBao WHERE MaBaiViet = @MaBaiViet)
        BEGIN
            RAISERROR(N'Mã bài viết đã tồn tại trong hệ thống!', 16, 1);
            RETURN;
        END

        INSERT INTO BaiVietBao (MaBaiViet, TieuDe, TomTat, NoiDung, NgayDang, LuotXem, MaTacGia, MaChuyenMuc)
        VALUES (@MaBaiViet, @TieuDe, @TomTat, @NoiDung, GETDATE(), 0, @MaTacGia, @MaChuyenMuc);
    END
    ELSE IF @Action = 'UPDATE'
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM BaiVietBao WHERE MaBaiViet = @MaBaiViet)
        BEGIN
            RAISERROR(N'Không tìm thấy bài viết để cập nhật!', 16, 1);
            RETURN;
        END

        UPDATE BaiVietBao
        SET TieuDe = ISNULL(@TieuDe, TieuDe),
            TomTat = ISNULL(@TomTat, TomTat),
            NoiDung = ISNULL(@NoiDung, NoiDung),
            MaChuyenMuc = ISNULL(@MaChuyenMuc, MaChuyenMuc)
        WHERE MaBaiViet = @MaBaiViet;
    END
    ELSE IF @Action = 'DELETE'
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM BaiVietBao WHERE MaBaiViet = @MaBaiViet)
        BEGIN
            RAISERROR(N'Không tìm thấy bài viết để xóa!', 16, 1);
            RETURN;
        END

        DELETE FROM BaiVietBao WHERE MaBaiViet = @MaBaiViet;
    END
    ELSE
    BEGIN
        RAISERROR(N'Hành động không hợp lệ! Chỉ chấp nhận INSERT, UPDATE, hoặc DELETE.', 16, 1);
    END
END;
GO

CREATE FUNCTION F_TinhTongGiaTri
(
    @MaTacGia VARCHAR(20)
)
RETURNS INT
AS
BEGIN
    DECLARE @TongLuotTuongTac INT;

    SELECT @TongLuotTuongTac = COUNT(bl.MaBinhLuan)
    FROM BinhLuanTinTuc bl
    INNER JOIN BaiVietBao b ON bl.MaBaiViet = b.MaBaiViet
    WHERE b.MaTacGia = @MaTacGia;

    RETURN ISNULL(@TongLuotTuongTac, 0);
END;
GO

CREATE TRIGGER TR_KiemTraNgayDangBai
ON BaiVietBao
FOR INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted
        WHERE NgayDang > GETDATE()
    )
    BEGIN
        RAISERROR(N'Lỗi: Ngày đăng bài viết không được lớn hơn thời gian hiện tại', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
GO
