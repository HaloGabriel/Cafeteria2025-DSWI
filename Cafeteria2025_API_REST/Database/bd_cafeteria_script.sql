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

INSERT INTO EstadoPedido (Descripcion)
VALUES ('Generado'),
       ('Preparando'),
       ('Listo para recojo'),
       ('Recogido'),
       ('Cancelado');
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

--Paginación de categorías para el CRUD
CREATE OR ALTER PROCEDURE USP_Paginacion_Categorias
@pagina Int, @tamanoPagina Int
AS
  BEGIN
    SELECT COUNT(*)
    FROM Categoria;

    SELECT *
    FROM Categoria
    ORDER BY IdCategoria
    OFFSET ((@pagina - 1) * @tamanoPagina) ROWS
    FETCH NEXT @tamanoPagina ROWS ONLY;
  END
GO

--Listado de categorías para desplegables
CREATE OR ALTER PROCEDURE USP_Listar_Categorias_Descripcion_Asc
AS
  BEGIN
    SELECT IdCategoria,
           Descripcion
    FROM Categoria
    WHERE Activo = 1
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

--Mostrar en el listado los 5 datos más importantes de los productos (mientas estén activos)
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
    WHERE prod.Activo = 1;
  END
GO

CREATE OR ALTER PROCEDURE USP_PaginacionProductos
@pagina Int, @tamanoPagina Int
AS
BEGIN
    SELECT Count(*)
    From Producto
    WHERE Activo = 1;

    SELECT prod.IdProducto,
           prod.Nombre,
           cate.Descripcion,
           prod.PrecioBase,
           prod.Stock
    FROM Producto prod
    JOIN Categoria cate ON prod.IdCategoria = cate.IdCategoria
    WHERE prod.Activo = 1
    ORDER BY prod.IdProducto
    OFFSET ((@pagina - 1) * @tamanoPagina) ROWS
    FETCH NEXT @tamanoPagina ROWS ONLY;
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
    LEFT JOIN Categoria cate ON prod.IdCategoria = cate.IdCategoria
    LEFT JOIN Tamano tmno ON prod.IdTamano = tmno.IdTamano
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
/* Obtener o crear pedido activo */

CREATE OR ALTER PROCEDURE USP_ObtenerOCrearPedidoGenerado
@idUsuario INT
AS
BEGIN
    DECLARE @idPedido INT;

    SELECT @idPedido = IdPedido
    FROM Pedido
    WHERE IdUsuario = @idUsuario
      AND IdEstadoPedido = 1
      AND Activo = 1;

    IF @idPedido IS NULL
    BEGIN
        INSERT INTO Pedido (
            IdUsuario,
            IdEstadoPedido,
            TotalPagar,
            NombreClienteRecoge,
            CodigoRecojo,
            EsRecojoInmediato
        )
        VALUES (
            @idUsuario,
            1,
            0,
            'Pendiente',
            NEWID(),
            1
        );

        SET @idPedido = SCOPE_IDENTITY();
    END

    SELECT @idPedido;
END
GO

/* Agregar producto al pedido */

CREATE OR ALTER PROCEDURE USP_AgregarProductoPedido
@idPedido INT,
@idProducto INT,
@cantidad INT
AS
BEGIN
    DECLARE @precio DECIMAL(10,2);

    -- Obtener precio base del producto
    SELECT @precio = PrecioBase
    FROM Producto
    WHERE IdProducto = @idProducto
      AND Activo = 1;

    IF @precio IS NULL
    BEGIN
        RAISERROR('Producto no encontrado o inactivo', 16, 1);
        RETURN;
    END

    -- Si el producto ya existe en el pedido, sumamos cantidad
    IF EXISTS (
        SELECT 1
        FROM DetallePedido
        WHERE IdPedido = @idPedido
          AND IdProducto = @idProducto
    )
    BEGIN
        UPDATE DetallePedido
        SET Cantidad = Cantidad + @cantidad
        WHERE IdPedido = @idPedido
          AND IdProducto = @idProducto;
    END
    ELSE
    BEGIN
        INSERT INTO DetallePedido (
            IdPedido,
            IdProducto,
            Cantidad,
            PrecioUnitarioFinal
        )
        VALUES (
            @idPedido,
            @idProducto,
            @cantidad,
            @precio
        );
    END
END
GO

/* Listar Pedido (carrito) */

CREATE OR ALTER PROCEDURE USP_ListarPedidoGenerado
@idUsuario INT
AS
BEGIN
    SELECT dp.IdDetallePedido,
           p.Nombre,
           dp.Cantidad,
           dp.PrecioUnitarioFinal,
           dp.Subtotal
    FROM Pedido pe
    JOIN DetallePedido dp ON pe.IdPedido = dp.IdPedido
    JOIN Producto p ON dp.IdProducto = p.IdProducto
    WHERE pe.IdUsuario = @idUsuario
      AND pe.IdEstadoPedido = 1
      AND pe.Activo = 1;
END
GO

/* Desactivar Producto */
CREATE OR ALTER PROCEDURE USP_Desactivar_Producto
@idproducto INT,
@userupdate NVARCHAR(255) = NULL
AS
BEGIN
    UPDATE Producto
    SET Activo = 0,
        FechaActualizacion = SYSDATETIME(),
        UsuarioActualizacion = @userupdate
    WHERE IdProducto = @idproducto;
END
GO

/*  VALIDAR STOCK */
CREATE OR ALTER PROCEDURE USP_ValidarStockPedido
@idPedido INT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM DetallePedido dp
        JOIN Producto p ON dp.IdProducto = p.IdProducto
        WHERE dp.IdPedido = @idPedido
          AND p.Stock < dp.Cantidad
    )
    BEGIN
        RAISERROR('Stock insuficiente para uno o más productos', 16, 1);
        RETURN;
    END
END
GO

/* DESCONTAR STOCK */
CREATE OR ALTER PROCEDURE USP_DescontarStockPedido
@idPedido INT
AS
BEGIN
    UPDATE p
    SET p.Stock = p.Stock - dp.Cantidad
    FROM Producto p
    JOIN DetallePedido dp ON p.IdProducto = dp.IdProducto
    WHERE dp.IdPedido = @idPedido;
END
GO

/* CONFIRMAR PEDIDO */
CREATE OR ALTER PROCEDURE USP_ConfirmarPedido
@idPedido INT,
@idMetodoPago TINYINT
AS
BEGIN
    BEGIN TRY
        BEGIN TRAN;

        /* 1. Validar stock */
        EXEC USP_ValidarStockPedido @idPedido;

        /* 2. Descontar stock */
        EXEC USP_DescontarStockPedido @idPedido;

        /* 3. Calcular TOTAL del pedido */
        DECLARE @total DECIMAL(12,2);

        SELECT @total = ISNULL(SUM(Subtotal), 0)
        FROM DetallePedido
        WHERE IdPedido = @idPedido;

        /* 4. Actualizar pedido */
        UPDATE Pedido
        SET TotalPagar = @total,
            IdEstadoPedido = 2, -- Preparando
            IdMetodoPago = @idMetodoPago,
            FechaActualizacion = SYSDATETIME()
        WHERE IdPedido = @idPedido;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END
GO


/* DEVOLVER STOCK DEL PEDIDO */
CREATE OR ALTER PROCEDURE USP_DevolverStockPedido
@idPedido INT
AS
BEGIN
    UPDATE p
    SET p.Stock = p.Stock + dp.Cantidad
    FROM Producto p
    JOIN DetallePedido dp ON p.IdProducto = dp.IdProducto
    WHERE dp.IdPedido = @idPedido;
END
GO

/* CANCELAR PEDIDO */
CREATE OR ALTER PROCEDURE USP_CancelarPedido
@idPedido INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRAN;

        -- Validar estado cancelable
        IF NOT EXISTS (
            SELECT 1
            FROM Pedido
            WHERE IdPedido = @idPedido
              AND IdEstadoPedido IN (1, 2) -- Generado o Preparando
              AND Activo = 1
        )
        BEGIN
            RAISERROR('El pedido no puede ser cancelado', 16, 1);
            ROLLBACK;
            RETURN;
        END

        -- Devolver stock
        EXEC USP_DevolverStockPedido @idPedido;

        -- Cambiar estado a Cancelado
        UPDATE Pedido
        SET IdEstadoPedido = 5, -- Cancelado
            FechaActualizacion = SYSDATETIME()
        WHERE IdPedido = @idPedido;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END
GO

/* Cambiar estado del pedido */
CREATE OR ALTER PROCEDURE USP_CambiarEstadoPedido
@idPedido INT,
@idNuevoEstado TINYINT
AS
BEGIN
    -- Validar pedido activo
    IF NOT EXISTS (
        SELECT 1
        FROM Pedido
        WHERE IdPedido = @idPedido
          AND Activo = 1
    )
    BEGIN
        RAISERROR('Pedido no existe o no está activo', 16, 1);
        RETURN;
    END

    -- No permitir volver atrás
    IF EXISTS (
        SELECT 1
        FROM Pedido
        WHERE IdPedido = @idPedido
          AND IdEstadoPedido >= @idNuevoEstado
    )
    BEGIN
        RAISERROR('Transición de estado no válida', 16, 1);
        RETURN;
    END

    -- Actualizar estado
    UPDATE Pedido
    SET IdEstadoPedido = @idNuevoEstado,
        FechaActualizacion = SYSDATETIME()
    WHERE IdPedido = @idPedido;
END
GO

/* VER PEDIDOS OPERATIVOS */
CREATE OR ALTER PROCEDURE USP_ListarPedidosOperativos
AS
BEGIN
    SELECT 
        p.IdPedido,
        u.Nombre + ' ' + u.Apellido AS Cliente,
        p.FechaPedido,
        ep.Descripcion AS Estado,
        p.TotalPagar,
        p.CodigoRecojo
    FROM Pedido p
    JOIN Usuario u ON p.IdUsuario = u.IdUsuario
    JOIN EstadoPedido ep ON p.IdEstadoPedido = ep.IdEstadoPedido
    WHERE p.IdEstadoPedido IN (2, 3) -- Preparando, Listo
      AND p.Activo = 1
    ORDER BY p.FechaPedido ASC;
END
GO

/* PAGINACIÓN PEDIDOS OPERATIVOS */
CREATE OR ALTER PROCEDURE USP_PaginacionPedidosOperativos
@pagina Int, @tamanoPagina Int
AS
  BEGIN
    SELECT COUNT(*)
    FROM Pedido p
    WHERE IdEstadoPedido IN (2, 3)
      AND Activo = 1;

    SELECT p.IdPedido,
           u.Nombre + ' ' + u.Apellido AS Cliente,
           p.FechaPedido,
           ep.Descripcion AS Estado,
           p.TotalPagar,
           p.CodigoRecojo
    FROM Pedido p
    JOIN Usuario u ON p.IdUsuario = u.IdUsuario
    JOIN EstadoPedido ep ON p.IdEstadoPedido = ep.IdEstadoPedido
    WHERE p.IdEstadoPedido IN (2, 3)
      AND p.Activo = 1
    ORDER BY p.FechaPedido ASC
    OFFSET ((@pagina - 1) * @tamanoPagina) ROWS
    FETCH NEXT @tamanoPagina ROWS ONLY;
  END
GO




SELECT * FROM MetodoPago

INSERT INTO MetodoPago (Nombre, Activo)
VALUES 
    ('Efectivo', 1),
    ('Transferencia Bancaria', 1),
    ('PayPal', 1),
    ('Criptomonedas', 0) -- Ejemplo de un método inactivo
GO

CREATE OR ALTER PROCEDURE USP_ListarMetodosPagoActivos
AS
  BEGIN
    SELECT CAST(IdMetodoPago AS INT),
           Nombre
    FROM MetodoPago
    WHERE Activo = 1
  END
GO

    /* PERSONALIZACIÓN */

CREATE TYPE TVP_Opciones AS TABLE (
IdOpcion INT);
GO

CREATE OR ALTER PROCEDURE USP_AgregarProductoPersonalizado
    @idUsuario INT,
    @idProducto INT,
    @cantidad INT,
    @Opciones TVP_Opciones READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        DECLARE @idPedido INT;
        DECLARE @idDetallePedido INT;
        DECLARE @precioBase DECIMAL(10,2);
        DECLARE @precioFinal DECIMAL(10,2);

        /* 1. Obtener o crear pedido (carrito) */
        SELECT @idPedido = IdPedido
        FROM Pedido
        WHERE IdUsuario = @idUsuario
          AND IdEstadoPedido = 1
          AND Activo = 1;

        IF @idPedido IS NULL
        BEGIN
            INSERT INTO Pedido (
                IdUsuario, IdEstadoPedido, TotalPagar,
                NombreClienteRecoge, CodigoRecojo, EsRecojoInmediato
            )
            VALUES (
                @idUsuario, 1, 0,
                'Pendiente', NEWID(), 1
            );

            SET @idPedido = SCOPE_IDENTITY();
        END

        /* 2. Validar producto */
        SELECT @precioBase = PrecioBase
        FROM Producto
        WHERE IdProducto = @idProducto
          AND Activo = 1;

        IF @precioBase IS NULL
        BEGIN
            RAISERROR('Producto no válido o inactivo', 16, 1);
            ROLLBACK;
            RETURN;
        END

        /* 3. Insertar detalle */
        INSERT INTO DetallePedido (
            IdPedido, IdProducto, Cantidad, PrecioUnitarioFinal
        )
        VALUES (
            @idPedido, @idProducto, @cantidad, @precioBase
        );

        SET @idDetallePedido = SCOPE_IDENTITY();

        /* 4. Insertar opciones */
        INSERT INTO PedidoOpcionDetalle (
            IdDetallePedido, IdOpcion, CostoAplicado
        )
        SELECT
            @idDetallePedido,
            o.IdOpcion,
            o.CostoAdicional
        FROM Opcion o
        JOIN @Opciones tvp ON o.IdOpcion = tvp.IdOpcion
        WHERE o.Activo = 1;

        /* 5. Calcular precio final */
        SELECT @precioFinal =
            @precioBase + ISNULL(SUM(CostoAplicado), 0)
        FROM PedidoOpcionDetalle
        WHERE IdDetallePedido = @idDetallePedido;

        UPDATE DetallePedido
        SET PrecioUnitarioFinal = @precioFinal
        WHERE IdDetallePedido = @idDetallePedido;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END
GO

/* Añadir Opcion Grupos */ 
CREATE OR ALTER PROCEDURE USP_InsertarOpcionGrupo
@nombre NVARCHAR(100),
@descripcion NVARCHAR(250),
@esRequerido BIT,
@maximo INT = NULL,
@tipoControl NVARCHAR(50)
AS
BEGIN
    INSERT INTO OpcionGrupo (
        Nombre, Descripcion, EsRequerido, Maximo, TipoControl, Activo
    )
    VALUES (
        @nombre, @descripcion, @esRequerido, @maximo, @tipoControl, 1
    );
END
GO

/* Añadir opciones */
CREATE OR ALTER PROCEDURE USP_InsertarOpcion
@idGrupo INT,
@nombre NVARCHAR(150),
@costo DECIMAL(10,2)
AS
BEGIN
    INSERT INTO Opcion (
        IdGrupo, NombreOpcion, CostoAdicional, Activo
    )
    VALUES (
        @idGrupo, @nombre, @costo, 1
    );
END
GO

/* Asignar opciones a productos */
CREATE OR ALTER PROCEDURE USP_AsignarOpcionProducto
@idProducto INT,
@idOpcion INT
AS
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM Producto
        WHERE IdProducto = @idProducto AND EsPersonalizable = 1
    )
    BEGIN
        RAISERROR('Producto no es personalizable', 16, 1);
        RETURN;
    END

    INSERT INTO ProductoOpcion (IdProducto, IdOpcion)
    VALUES (@idProducto, @idOpcion);
END
GO

CREATE OR ALTER PROCEDURE USP_ListarOpcionesPorProducto
@idProducto INT
AS
BEGIN
    SELECT 
        og.IdGrupo,
        og.Nombre AS Grupo,
        og.TipoControl,
        og.EsRequerido,
        og.Maximo,
        o.IdOpcion,
        o.NombreOpcion,
        o.CostoAdicional
    FROM ProductoOpcion po
    JOIN Opcion o ON po.IdOpcion = o.IdOpcion
    JOIN OpcionGrupo og ON o.IdGrupo = og.IdGrupo
    WHERE po.IdProducto = @idProducto
      AND o.Activo = 1
      AND og.Activo = 1;
END
GO

/* TAMAÑOS */
/* LISTAR */
CREATE OR ALTER PROCEDURE USP_Listar_Tamanos
AS
BEGIN
    SELECT
        IdTamano,
        Nombre,
        Descripcion,
        CostoAdicional,
        Activo
    FROM Tamano
    WHERE Activo = 1;
END
GO


/* INSERTAR TAMAÑOS */
CREATE OR ALTER PROCEDURE USP_Insertar_Tamano
@nom NVARCHAR(50),
@desc NVARCHAR(150) = NULL,
@costo DECIMAL(10,2),
@activo BIT
AS
BEGIN
    INSERT INTO Tamano (
        Nombre,
        Descripcion,
        CostoAdicional,
        Activo
    )
    VALUES (
        @nom,
        @desc,
        @costo,
        @activo
    );
END
GO


/* BUSCAR TAMAÑO POR ID */
CREATE OR ALTER PROCEDURE USP_Buscar_Tamano_Por_Id
@id TINYINT
AS
BEGIN
    SELECT
        IdTamano,
        Nombre,
        Descripcion,
        CostoAdicional,
        Activo
    FROM Tamano
    WHERE IdTamano = @id;
END
GO


/* ACTUALIZAR TAMAÑO */
CREATE OR ALTER PROCEDURE USP_Actualizar_Tamano
@id TINYINT,
@nom NVARCHAR(50),
@desc NVARCHAR(150) = NULL,
@costo DECIMAL(10,2),
@activo BIT
AS
BEGIN
    UPDATE Tamano
    SET Nombre = @nom,
        Descripcion = @desc,
        CostoAdicional = @costo,
        Activo = @activo
    WHERE IdTamano = @id;
END
GO


/* ELIMINAR LOGICAMENTE */
CREATE OR ALTER PROCEDURE USP_Desactivar_Tamano
@idTamano TINYINT
AS
BEGIN
    UPDATE Tamano
    SET Activo = 0
    WHERE IdTamano = @idTamano;
END
GO

/* USUARIO */
/* LISTAR USUARIO */
CREATE OR ALTER PROCEDURE USP_Listar_Usuarios
AS
BEGIN
    SELECT
        IdUsuario,
        Nombre,
        Apellido,
        Email,
        PasswordHash,
        Telefono,
        IdRol,                 
        Activo,
        FechaRegistro,
        FechaActualizacion,
        UsuarioActualizacion
    FROM Usuario
    WHERE Activo = 1;
END
GO



/* BUSCAR POR ID */
CREATE OR ALTER PROCEDURE USP_Buscar_Usuario_Por_Id
@id INT
AS
BEGIN
    SELECT 
        IdUsuario,
        Nombre,
        Apellido,
        Email,
        PasswordHash,
        Telefono,
        IdRol,
        Activo,
        FechaRegistro,
        FechaActualizacion,
        UsuarioActualizacion
    FROM Usuario
    WHERE IdUsuario = @id;
END
GO


/* INSERTAR USUARIO */
CREATE OR ALTER PROCEDURE USP_Insertar_Usuario
@nom NVARCHAR(100),
@ape NVARCHAR(100),
@email NVARCHAR(255),
@pass NVARCHAR(500),
@tel NVARCHAR(15) = NULL,
@rol TINYINT,
@user NVARCHAR(255)
AS
BEGIN
    INSERT INTO Usuario (
        Nombre,
        Apellido,
        Email,
        PasswordHash,
        Telefono,
        IdRol,
        Activo,
        FechaRegistro,
        UsuarioActualizacion
    )
    VALUES (
        @nom,
        @ape,
        @email,
        @pass,
        @tel,
        @rol,
        1,
        SYSDATETIME(),
        @user
    );
END
GO


/* ACTUALIZR USUARIO */
CREATE OR ALTER PROCEDURE USP_Actualizar_Usuario
@id INT,
@nom NVARCHAR(100),
@ape NVARCHAR(100),
@email NVARCHAR(255),
@pass NVARCHAR(500),
@tel NVARCHAR(15) = NULL,
@rol TINYINT,
@user NVARCHAR(255)
AS
BEGIN
    UPDATE Usuario
    SET Nombre = @nom,
        Apellido = @ape,
        Email = @email,
        PasswordHash = @pass,
        Telefono = @tel,
        IdRol = @rol,
        FechaActualizacion = SYSDATETIME(),
        UsuarioActualizacion = @user
    WHERE IdUsuario = @id;
END
GO


/* DESACTIVAR */
CREATE OR ALTER PROCEDURE USP_Desactivar_Usuario
@idUsuario INT
AS
BEGIN
    UPDATE Usuario
    SET Activo = 0,
        FechaActualizacion = SYSDATETIME()
    WHERE IdUsuario = @idUsuario;
END
GO

/* LISTAR ROLES */
CREATE OR ALTER PROCEDURE USP_Listar_Roles
AS
BEGIN
    SELECT
        IdRol,
        Nombre,
        Descripcion,
        Activo
    FROM Rol
    WHERE Activo = 1;
END
GO

/* HISTORIAL POR USUARIO */
CREATE OR ALTER PROCEDURE USP_Listar_Historial_Pedidos_Usuario
@idUsuario INT
AS
BEGIN

    SELECT
        p.IdPedido,
        p.FechaPedido,
        ep.Descripcion AS Estado,
        p.TotalPagar,
        p.NombreClienteRecoge,
        p.CodigoRecojo
    FROM Pedido p
    JOIN EstadoPedido ep ON p.IdEstadoPedido = ep.IdEstadoPedido
    WHERE p.IdUsuario = @idUsuario
      AND p.Activo = 1
    ORDER BY p.FechaPedido DESC;
END
GO

/* PAGINACIÓN DE HISTORIAL POR USUARIO */
CREATE OR ALTER PROCEDURE USP_Paginacion_Historial_Pedidos_Usuario
@idUsuario INT, @pagina INT, @tamanoPagina INT
AS
BEGIN
    SELECT COUNT(*)
    FROM Pedido
    WHERE IdUsuario = @idUsuario
      AND Activo = 1;

    SELECT
        p.IdPedido,
        p.FechaPedido,
        ep.Descripcion AS Estado,
        p.TotalPagar,
        p.NombreClienteRecoge,
        p.CodigoRecojo
    FROM Pedido p
    JOIN EstadoPedido ep ON p.IdEstadoPedido = ep.IdEstadoPedido
    WHERE p.IdUsuario = @idUsuario
      AND p.Activo = 1
    ORDER BY p.FechaPedido DESC
    OFFSET ((@pagina - 1) * @tamanoPagina) ROWS
    FETCH NEXT @tamanoPagina ROWS ONLY;
END
GO

/* REPORTE GENERAL DE PEDIDOS */
CREATE OR ALTER PROCEDURE USP_Reporte_Pedidos_General
AS
BEGIN
    SELECT
        ep.Descripcion AS Estado,
        COUNT(*) AS CantidadPedidos,
        ISNULL(SUM(p.TotalPagar), 0) AS TotalRecaudado
    FROM Pedido p
    JOIN EstadoPedido ep ON p.IdEstadoPedido = ep.IdEstadoPedido
    WHERE p.Activo = 1
    GROUP BY ep.Descripcion;
END
GO
