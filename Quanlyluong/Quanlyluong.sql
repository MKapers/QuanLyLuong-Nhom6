CREATE DATABASE QuanLyLuong;
GO

USE QuanLyLuong;
GO

CREATE TABLE PhongBans
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TenPhong NVARCHAR(100) NOT NULL,
    GhiChu NVARCHAR(MAX) NULL
);
GO

INSERT INTO PhongBans (TenPhong, GhiChu)
VALUES
(N'Kế toán', N'Phụ trách kế toán và tiền lương'),
(N'Hành chính', N'Phụ trách công việc hành chính');
GO

SELECT * FROM PhongBans;

--nv
USE QuanLyLuong;
GO

CREATE TABLE NhanViens
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MaNV VARCHAR(15) NOT NULL UNIQUE,
    HoTen NVARCHAR(100) NOT NULL,
    PhongBanId INT NOT NULL,
    LuongCoBan DECIMAL(18,0) NOT NULL,
    PhuCap DECIMAL(18,0) NOT NULL DEFAULT 0,

    CONSTRAINT FK_NhanViens_PhongBans
        FOREIGN KEY (PhongBanId)
        REFERENCES PhongBans(Id),

    CONSTRAINT CK_NhanViens_Luong
        CHECK (LuongCoBan > 0 AND PhuCap >= 0)
);
GO

SELECT * FROM NhanViens;

--luong
USE QuanLyLuong;
GO

CREATE TABLE BangLuongs
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NhanVienId INT NOT NULL,

    Thang INT NOT NULL CHECK (Thang BETWEEN 1 AND 12),
    Nam INT NOT NULL CHECK (Nam BETWEEN 2020 AND 2100),
    SoNgayCong INT NOT NULL CHECK (SoNgayCong BETWEEN 0 AND 26),

    LuongCoBan DECIMAL(18,0) NOT NULL,
    PhuCap DECIMAL(18,0) NOT NULL,
    Thuong DECIMAL(18,0) NOT NULL DEFAULT 0 CHECK (Thuong >= 0),
    KhauTru DECIMAL(18,0) NOT NULL DEFAULT 0 CHECK (KhauTru >= 0),
    ThucLinh DECIMAL(18,0) NOT NULL CHECK (ThucLinh >= 0),

    CONSTRAINT FK_BangLuongs_NhanViens
        FOREIGN KEY (NhanVienId)
        REFERENCES NhanViens(Id),

    CONSTRAINT UQ_BangLuongs_Thang
        UNIQUE (NhanVienId, Thang, Nam)
);
GO

SELECT * FROM BangLuongs;

--tk truy cap
USE QuanLyLuong;
GO

CREATE TABLE TaiKhoans
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap NVARCHAR(50) NOT NULL UNIQUE,
    MatKhauHash NVARCHAR(500) NOT NULL,
    Quyen VARCHAR(20) NOT NULL,
    NhanVienId INT NULL,

    CONSTRAINT FK_TaiKhoans_NhanViens
        FOREIGN KEY (NhanVienId)
        REFERENCES NhanViens(Id),

    CONSTRAINT CK_TaiKhoans_Quyen
        CHECK
        (
            (Quyen = 'Admin' AND NhanVienId IS NULL)
            OR
            (Quyen = 'NhanVien' AND NhanVienId IS NOT NULL)
        )
);
GO

--tk
USE QuanLyLuong;
GO

SELECT
    t.TenDangNhap,
    t.Quyen,
    t.NhanVienId,
    n.HoTen
FROM TaiKhoans t
LEFT JOIN NhanViens n ON t.NhanVienId = n.Id;