USE master
GO

IF EXISTS(SELECT *
          FROM sys.databases
          WHERE NAME = 'Cafeteria')
  BEGIN
    ALTER DATABASE Cafeteria SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE Cafeteria;
  END
GO

CREATE DATABASE Cafeteria
GO

USE Cafeteria;

--Perfiles de acceso del sistema (admin, vendedor, cliente)
CREATE TABLE Rol (
    IdRol TINYINT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE,         -- Nombre del rol
    Descripcion NVARCHAR(250) NULL,              -- Descripción del rol
    Activo BIT NOT NULL DEFAULT(1)               -- Control lógico
);


--Métodos disponibles (Yape, Tarjeta, plin, etc)
CREATE TABLE MetodoPago (
    IdMetodoPago TINYINT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE,
    Activo BIT NOT NULL DEFAULT(1)
); 

--Estados del pedido (Generado, Recogido, Cancelado)
CREATE TABLE EstadoPedido (
    IdEstadoPedido TINYINT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(50) NOT NULL UNIQUE,
    Activo BIT NOT NULL DEFAULT(1)
);


--Clasificación del producto (Café, Sándwich, Postres, etc)
CREATE TABLE Categoria (
    IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(100) NOT NULL,
    Activo BIT NOT NULL DEFAULT(1),
    FechaRegistro DATETIME NOT NULL DEFAULT(SYSDATETIME())
); 

--Grupos de opciones (Leche, Endulzante, Extras, etc)
--TipoControl: Radio (1 opción) / Checkbox (varias)
--EsRequerido: si se debe seleccionar sí o sí esa opcion
--Maximo: cuántas opciones se pueden elegir
CREATE TABLE OpcionGrupo (
    IdGrupo INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(250) NULL,
    EsRequerido BIT NOT NULL DEFAULT(0),
    Maximo INT NULL,
    TipoControl NVARCHAR(50) NOT NULL DEFAULT('Radio'),
    Activo BIT NOT NULL DEFAULT(1)
); 

--Opciones específicas (Ej: Leche Entera, Soya, Almendra)
CREATE TABLE Opcion (
    IdOpcion INT IDENTITY(1,1) PRIMARY KEY,
    IdGrupo INT NOT NULL REFERENCES OpcionGrupo(IdGrupo),
    NombreOpcion NVARCHAR(150) NOT NULL,
    CostoAdicional DECIMAL(10,2) NOT NULL DEFAULT(0),
    Activo BIT NOT NULL DEFAULT(1)
); 

--Usuarios del sistema
CREATE TABLE Usuario (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    Telefono NVARCHAR(15) NULL,
    IdRol TINYINT NOT NULL REFERENCES Rol(IdRol),
    Activo BIT NOT NULL DEFAULT(1),
    FechaRegistro DATETIME NOT NULL DEFAULT(SYSDATETIME()),
    FechaActualizacion DATETIME NULL,
    UsuarioActualizacion NVARCHAR(255) NULL
); 

--Tamaños estándar del local (ej: S, M, L, standar)
CREATE TABLE Tamano (
    IdTamano TINYINT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(150) NULL,
    CostoAdicional DECIMAL(10,2) NOT NULL DEFAULT(0),
    Activo BIT NOT NULL DEFAULT(1)
);

--Productos del catálogo
--PrecioBase: precio sin extras
--IdTamano: si el producto tiene tamaño fijo
--EsPersonalizable: si tiene opciones (leche, extras...)
CREATE TABLE Producto (
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(1000) NULL,
    PrecioBase DECIMAL(10,2) NOT NULL CHECK (PrecioBase >= 0),
    IdTamano TINYINT NULL REFERENCES Tamano(IdTamano), -- Tamaño único opcional
    Stock INT NOT NULL DEFAULT(0),
    IdCategoria INT NOT NULL REFERENCES Categoria(IdCategoria),
    ImagenUrl NVARCHAR(1000) NULL,
    EsPersonalizable BIT NOT NULL DEFAULT(0),
    Activo BIT NOT NULL DEFAULT(1),
    FechaRegistro DATETIME NOT NULL DEFAULT(SYSDATETIME()),
    FechaActualizacion DATETIME NULL,
    UsuarioActualizacion NVARCHAR(255) NULL
); 

--Vincula productos con sus opciones válidas
CREATE TABLE ProductoOpcion (
    IdProductoOp INT IDENTITY(1,1) PRIMARY KEY,
    IdProducto INT NOT NULL REFERENCES Producto(IdProducto) ON DELETE CASCADE,
    IdOpcion INT NOT NULL REFERENCES Opcion(IdOpcion) ON DELETE CASCADE,
    UNIQUE (IdProducto, IdOpcion)
); 

--Encabezado de la compra
CREATE TABLE Pedido (
    IdPedido INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL REFERENCES Usuario(IdUsuario),
    FechaPedido DATETIME NOT NULL DEFAULT(SYSDATETIME()),
    IdEstadoPedido TINYINT NOT NULL REFERENCES EstadoPedido(IdEstadoPedido),
    IdMetodoPago TINYINT NULL REFERENCES MetodoPago(IdMetodoPago),
    TotalPagar DECIMAL(12,2) NOT NULL DEFAULT(0),
    NombreClienteRecoge NVARCHAR(250) NOT NULL,
    HoraRecojoEstimado DATETIME NULL, --si escoje un horario para recojerlo 
    NotasGenerales NVARCHAR(1000) NULL,
    Activo BIT NOT NULL DEFAULT(1),
    CodigoRecojo NVARCHAR(100) NOT NULL UNIQUE,   -- Código QR / PIN
    EsRecojoInmediato BIT NOT NULL DEFAULT(1),    -- Si recoge al toque
    NumeroComprobante NVARCHAR(50) NULL,
    FechaActualizacion DATETIME2 NULL,
    UsuarioActualizacion NVARCHAR(255) NULL
);
Go

--Productos pedidos (líneas del pedido)
CREATE TABLE DetallePedido (
    IdDetallePedido INT IDENTITY(1,1) PRIMARY KEY,
    IdPedido INT NOT NULL REFERENCES Pedido(IdPedido) ON DELETE CASCADE,
    IdProducto INT NOT NULL REFERENCES Producto(IdProducto),
    Cantidad INT NOT NULL DEFAULT(1) CHECK (Cantidad > 0),
    PrecioUnitarioFinal DECIMAL(10,2) NOT NULL CHECK (PrecioUnitarioFinal >= 0),
    Subtotal AS (Cantidad * PrecioUnitarioFinal) PERSISTED,
    NotasEspeciales NVARCHAR(500) NULL
); 

--Opciones seleccionadas por cada producto
   --(ej: leche soya + extra vainilla)
CREATE TABLE PedidoOpcionDetalle (
    IdPedidoOpcionDetalle INT IDENTITY(1,1) PRIMARY KEY,
    IdDetallePedido INT NOT NULL REFERENCES DetallePedido(IdDetallePedido) ON DELETE CASCADE,
    IdOpcion INT NOT NULL REFERENCES Opcion(IdOpcion),
    CostoAplicado DECIMAL(10,2) NOT NULL CHECK (CostoAplicado >= 0),
    UNIQUE (IdDetallePedido, IdOpcion)
);

--Inserciones iniciales
INSERT INTO Rol (Nombre, Activo)
VALUES ('admin', DEFAULT),
       ('vendedor', DEFAULT),
       ('cliente', DEFAULT)
GO

INSERT INTO Usuario (Nombre, Apellido, Email, PasswordHash,
                     IdRol, Activo, FechaRegistro)
VALUES ('admin', 'admin', 'admin@admin.com',
        '$2a$12$JF5Uun6rSlIYJRfNrC8C2OiGzk5J9J.39IOFVL71yoqe22eFHhtji', -- Password: 123
        1, DEFAULT, DEFAULT)
GO

--Procedimientos almacenados

--Listado de categorías para el CRUD
CREATE OR ALTER PROCEDURE USP_Listar_Categorias
AS
  BEGIN
    SELECT *
    FROM Categoria
  END
GO

--Listado de categorías para desplegables
CREATE OR ALTER PROCEDURE USP_Listar_Categorias_Descripcion_Asc
AS
  BEGIN
    SELECT *
    FROM Categoria
    ORDER BY Descripcion ASC
  END
GO

CREATE OR ALTER PROCEDURE USP_Buscar_Categoria_Por_ID
@idcategoria INT
AS
  BEGIN
    SELECT *
    FROM Categoria
    WHERE IdCategoria = @idcategoria
  END
GO

CREATE OR ALTER PROCEDURE USP_Insertar_Categoria
@desc NVARCHAR(100)
AS
  BEGIN
    INSERT INTO Categoria (Descripcion, Activo, FechaRegistro)
    VALUES (@desc, DEFAULT, DEFAULT)
  END
GO

CREATE OR ALTER PROCEDURE USP_Actualizar_Categoria
@idcategoria INT,
@desc NVARCHAR(100),
@activo BIT
AS
  BEGIN
    UPDATE Categoria
    SET Descripcion = @desc,
        Activo = @activo
    WHERE IdCategoria = @idcategoria
  END
GO

--Mostrar en el listado los 5 datos más importantes de los productos
CREATE OR ALTER PROCEDURE USP_Listar_Productos
AS
  BEGIN
    SELECT prod.IdProducto,
           prod.Nombre,
           cate.Descripcion,
           prod.PrecioBase,
           prod.Stock
    FROM Producto prod
    JOIN Categoria cate ON prod.IdCategoria = cate.IdCategoria
  END
GO

CREATE OR ALTER PROCEDURE USP_Insertar_Producto
@nombre NVARCHAR(150), @desc NVARCHAR(1000) = NULL,
@precio DECIMAL(10, 2), @idtamano TINYINT = NULL,
@stock INT, @idcategoria INT, @imgurl NVARCHAR(1000) = NULL,
@espersonalizable BIT
AS
  BEGIN
    INSERT INTO Producto (Nombre, Descripcion, PrecioBase,
                          IdTamano, Stock, IdCategoria,
                          ImagenUrl, EsPersonalizable,
                          Activo, FechaRegistro)
    VALUES (@nombre, @desc, @precio, @idtamano, @stock,
            @idcategoria, @imgurl, @espersonalizable,
            DEFAULT, DEFAULT)
  END
GO

--Buscar un producto para leer sus datos
CREATE OR ALTER PROCEDURE USP_Buscar_Producto_Por_ID
@idproducto INT
AS
  BEGIN
    SELECT prod.IdProducto,
           prod.Nombre,
           prod.Descripcion,
           prod.PrecioBase,
           tmno.Nombre,
           prod.Stock,
           cate.Descripcion,
           prod.ImagenUrl,
           prod.EsPersonalizable,
           prod.Activo,
           prod.FechaRegistro
    FROM Producto prod
    JOIN Categoria cate ON prod.IdCategoria = cate.IdCategoria
    JOIN Tamano tmno ON prod.IdTamano = tmno.IdTamano
    WHERE prod.IdProducto = @idproducto
  END
GO

--Buscar un producto para llenar un formulario de actualización
CREATE OR ALTER PROCEDURE USP_Buscar_Producto_Por_ID2
@idproducto INT
AS
  BEGIN
    SELECT IdProducto,
           Nombre,
           Descripcion,
           PrecioBase,
           IdTamano,
           Stock,
           IdCategoria,
           ImagenUrl,
           EsPersonalizable,
           Activo
    FROM Producto
    WHERE IdProducto = @idproducto
  END
GO

CREATE OR ALTER PROCEDURE USP_Actualizar_Producto
@idproducto INT, @nombre NVARCHAR(150),
@desc NVARCHAR(1000) = NULL, @precio DECIMAL(10, 2),
@idtamano TINYINT = NULL, @stock INT, @idcategoria INT,
@imgurl NVARCHAR(1000) = NULL,
@espersonalizable BIT, @activo BIT,
@userupdate NVARCHAR(255)
AS
  BEGIN
    UPDATE Producto
    SET Nombre = @nombre,
        Descripcion = @desc,
        PrecioBase = @precio,
        IdTamano = @idtamano,
        Stock = @stock,
        IdCategoria = @idcategoria,
        ImagenUrl = @imgurl,
        EsPersonalizable = @espersonalizable,
        Activo = @activo,
        FechaActualizacion = SYSDATETIME(),
        UsuarioActualizacion = @userupdate
    WHERE IdProducto = @idproducto
  END
GO

Select * From Categoria