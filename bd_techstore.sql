-- ==============================================================
-- BASE DE DATOS: bd_techstore
-- SISTEMA POS DE REGISTRO DE VENTAS (TECHSTORE)
-- DESAFÍO - SEMANA 11: ARQUITECTURA N-CAPAS, SINGLETON Y TRANSACCIONES
-- ==============================================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'bd_techstore')
BEGIN
    ALTER DATABASE bd_techstore SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE bd_techstore;
END
GO

CREATE DATABASE bd_techstore;
GO

USE bd_techstore;
GO

-- ==============================================================
-- 1. TABLA: Usuarios
-- Almacena credenciales cifradas con SHA-256 + Salt
-- ==============================================================
CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    ClaveHash VARCHAR(256) NOT NULL,
    ClaveSalt VARCHAR(64) NOT NULL,
    NombreCompleto VARCHAR(100) NOT NULL,
    Rol VARCHAR(30) NOT NULL, -- 'Administrador', 'Cajero'
    Estado BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

-- ==============================================================
-- 2. TABLA: Categorias
-- Categorías de artículos de tecnología
-- ==============================================================
CREATE TABLE Categorias (
    IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL UNIQUE,
    Descripcion VARCHAR(200) NULL
);
GO

-- ==============================================================
-- 3. TABLA: Productos
-- Inventario con control estricto de Stock
-- ==============================================================
CREATE TABLE Productos (
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
    Codigo VARCHAR(20) NOT NULL UNIQUE,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    Precio DECIMAL(10,2) NOT NULL CHECK (Precio >= 0),
    Stock INT NOT NULL CHECK (Stock >= 0),
    IdCategoria INT NULL FOREIGN KEY REFERENCES Categorias(IdCategoria),
    Estado BIT NOT NULL DEFAULT 1
);
GO

-- ==============================================================
-- 4. TABLA: Ventas (Cabecera)
-- Según enunciado: IdVenta, Fecha, Total, IdUsuario, IdProducto
-- ==============================================================
CREATE TABLE Ventas (
    IdVenta INT IDENTITY(1,1) PRIMARY KEY,
    NumeroFactura VARCHAR(20) NOT NULL UNIQUE,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Total DECIMAL(10,2) NOT NULL CHECK (Total >= 0),
    IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuarios(IdUsuario),
    IdProducto INT NULL FOREIGN KEY REFERENCES Productos(IdProducto),
    Estado VARCHAR(20) NOT NULL DEFAULT 'COMPLETADA'
);
GO

-- ==============================================================
-- 5. TABLA: DetalleVentas (Detalles)
-- Según enunciado: IdDetalle, IdVenta, IdProducto, Cantidad, PrecioUnitario, Subtotal
-- ==============================================================
CREATE TABLE DetalleVentas (
    IdDetalle INT IDENTITY(1,1) PRIMARY KEY,
    IdVenta INT NOT NULL FOREIGN KEY REFERENCES Ventas(IdVenta),
    IdProducto INT NOT NULL FOREIGN KEY REFERENCES Productos(IdProducto),
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario DECIMAL(10,2) NOT NULL CHECK (PrecioUnitario >= 0),
    Subtotal DECIMAL(10,2) NOT NULL CHECK (Subtotal >= 0)
);
GO

-- ==============================================================
-- DATOS SEMILLA (SEEDS)
-- ==============================================================

-- Categorías
INSERT INTO Categorias (Nombre, Descripcion) VALUES 
('Laptops y PCs', 'Equipos portátiles y de escritorio de alta gama'),
('Smartphones y Tablets', 'Dispositivos móviles inteligentes y tablets'),
('Accesorios y Periféricos', 'Teclados, ratones, auriculares y pads'),
('Componentes de Hardware', 'Memorias RAM, discos SSD y tarjetas gráficas'),
('Monitores y Pantallas', 'Monitores gamer y de productividad');
GO

-- Usuarios con Claves Cifradas mediante SHA-256 + Salt
-- Admin:    admin / admin123
-- Cajero 1: cajero1 / 123456
-- Cajero 2: cajero2 / 123456
INSERT INTO Usuarios (NombreUsuario, ClaveHash, ClaveSalt, NombreCompleto, Rol, Estado) VALUES
('admin',   'B53FFF066DD0D45AF16AEB07C26F30E38D674E279E44A247D3C930DDA09CDE61', 'FVvTVGwpv+WZmr9lxRgMiA==', 'Carlos Mendoza Silva', 'Administrador', 1),
('cajero1', '68AF7BB0D1CE9573F6D8F5D554940CD225D1C1E2D2BF905598794F3F007FCE41', 'fWvdnuhq6vkm4iw9z76Bug==', 'Ana Lucia Ramirez',   'Cajero',        1),
('cajero2', 'BFA0225E26FEA2C3266C693E7E1EABFB70AC678A448ED1E0CFDE88CE321172A2', 'EFpHRmFK0sfObE6VQ5nRxg==', 'Jorge Perez Torres',   'Cajero',        1);
GO

-- Productos Tecnológicos con Códigos y Stocks Iniciales
INSERT INTO Productos (Codigo, Nombre, Descripcion, Precio, Stock, IdCategoria, Estado) VALUES
('PROD-001', 'Laptop ASUS ROG Strix G16 i7 16GB 1TB RTX4060', 'Laptop gamer de alto rendimiento con pantalla de 165Hz', 5499.00, 10, 1, 1),
('PROD-002', 'Laptop Lenovo ThinkPad E14 Ryzen 7 16GB 512GB', 'Laptop corporativa ultrarresistente para productividad', 3299.00, 15, 1, 1),
('PROD-003', 'iPhone 15 Pro Max 256GB Titanio Natural',       'Smartphone premium Apple con chip A17 Pro y cámara 48MP', 5899.00, 8,  2, 1),
('PROD-004', 'Samsung Galaxy S24 Ultra 256GB Gris Titanio',   'Smartphone insignia con IA integrada y S-Pen',           5199.00, 12, 2, 1),
('PROD-005', 'Monitor Gamer LG UltraGear 27" IPS 165Hz 1ms',  'Monitor QHD con soporte FreeSync y G-Sync Compatible',    1199.00, 20, 5, 1),
('PROD-006', 'Teclado Mecánico Logitech G PRO RGB Lightsync', 'Teclado para esports con switches GX Blue audibles',       459.00, 25, 3, 1),
('PROD-007', 'Mouse Inalámbrico Logitech MX Master 3S',        'Mouse ergonómico para productividad y diseño gráfico',     399.00, 30, 3, 1),
('PROD-008', 'Audífonos Sony WH-1000XM5 con Noise Cancelling', 'Audífonos inalámbricos líderes en cancelación de ruido', 1399.00, 14, 3, 1),
('PROD-009', 'SSD Kingston NV2 1TB M.2 PCIe 4.0 NVMe',        'Unidad de estado sólido de alta velocidad de lectura',     289.00, 40, 4, 1),
('PROD-010', 'Memoria RAM Kingston Fury Beast 16GB DDR5 5600', 'Módulo de memoria RAM de alto rendimiento para gaming',    249.00, 35, 4, 1),
('PROD-011', 'Tablet iPad Air 11" M2 128GB Wi-Fi Azul',        'Tablet versátil de última generación Apple',              3099.00, 6,  2, 1),
('PROD-012', 'Webcam Logitech Brio 4K Ultra HD HDR',          'Cámara web profesional para streaming y videollamadas',    749.00, 18, 3, 1);
GO

-- ==============================================================
-- PROCEDIMIENTOS ALMACENADOS
-- ==============================================================

-- 1. Obtener usuario por nombre de usuario para validación de Login en BLL
CREATE OR ALTER PROCEDURE SP_ObtenerUsuarioPorLogin
    @NombreUsuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdUsuario, NombreUsuario, ClaveHash, ClaveSalt, NombreCompleto, Rol, Estado, FechaCreacion
    FROM Usuarios
    WHERE NombreUsuario = @NombreUsuario AND Estado = 1;
END;
GO

-- 2. Listar productos con su categoría y stock actual
CREATE OR ALTER PROCEDURE SP_ListarProductos
    @SoloActivos BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        p.IdProducto,
        p.Codigo,
        p.Nombre,
        p.Descripcion,
        p.Precio,
        p.Stock,
        p.IdCategoria,
        ISNULL(c.Nombre, 'Sin Categoría') AS NombreCategoria,
        p.Estado
    FROM Productos p
    LEFT JOIN Categorias c ON p.IdCategoria = c.IdCategoria
    WHERE (@SoloActivos = 0 OR p.Estado = 1)
    ORDER BY p.Nombre ASC;
END;
GO

-- 3. Obtener producto por ID
CREATE OR ALTER PROCEDURE SP_ObtenerProductoPorId
    @IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        p.IdProducto,
        p.Codigo,
        p.Nombre,
        p.Descripcion,
        p.Precio,
        p.Stock,
        p.IdCategoria,
        ISNULL(c.Nombre, 'Sin Categoría') AS NombreCategoria,
        p.Estado
    FROM Productos p
    LEFT JOIN Categorias c ON p.IdCategoria = c.IdCategoria
    WHERE p.IdProducto = @IdProducto;
END;
GO

-- 4. Obtener último número de factura para correlativo
CREATE OR ALTER PROCEDURE SP_ObtenerUltimoNumeroFactura
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 NumeroFactura 
    FROM Ventas 
    ORDER BY IdVenta DESC;
END;
GO

-- 5. Listar ventas para historial en UI
CREATE OR ALTER PROCEDURE SP_ListarVentas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        v.IdVenta,
        v.NumeroFactura,
        v.Fecha,
        v.Total,
        v.IdUsuario,
        u.NombreCompleto AS Cajero,
        v.IdProducto,
        v.Estado,
        (SELECT COUNT(*) FROM DetalleVentas dv WHERE dv.IdVenta = v.IdVenta) AS CantidadItems
    FROM Ventas v
    INNER JOIN Usuarios u ON v.IdUsuario = u.IdUsuario
    ORDER BY v.IdVenta DESC;
END;
GO

-- 6. Obtener detalle de una venta específica
CREATE OR ALTER PROCEDURE SP_ObtenerDetallesPorVenta
    @IdVenta INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        dv.IdDetalle,
        dv.IdVenta,
        dv.IdProducto,
        p.Codigo AS CodigoProducto,
        p.Nombre AS NombreProducto,
        dv.Cantidad,
        dv.PrecioUnitario,
        dv.Subtotal
    FROM DetalleVentas dv
    INNER JOIN Productos p ON dv.IdProducto = p.IdProducto
    WHERE dv.IdVenta = @IdVenta
    ORDER BY dv.IdDetalle ASC;
END;
GO

