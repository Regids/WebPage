USE DBCARRITO
GO

IF EXISTS (SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = N'SP_RegisterUser')
	DROP PROCEDURE SP_RegisterUser
GO

CREATE PROCEDURE SP_RegisterUser(
	@Nombres VARCHAR(100),
	@Apellidos VARCHAR(100),
	@Correo VARCHAR(100),
	@Clave VARCHAR(100),
	@Activo BIT

)AS BEGIN

	DECLARE @Resultado INT
	DECLARE @Mensaje VARCHAR(500)

	SET @Resultado = 0
		IF NOT EXISTS (SELECT * FROM USUARIO WHERE Correo = @Correo)
		BEGIN
		INSERT INTO USUARIO(
			Nombres,
			Apellidos,
			Correo,
			Clave,
			Activo)
		VALUES(
			@Nombres,
			@Apellidos,
			@Correo,
			@Clave,
			@Activo)

	SET @Resultado = SCOPE_IDENTITY()
END
	ELSE
	SET @Mensaje= 'Mail in use.'
END

GO

IF EXISTS (SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = N'SP_EditUser')
	DROP PROCEDURE SP_EditUser
GO

CREATE PROCEDURE SP_EditUser(
	@IdUsuario INT,
	@Nombres VARCHAR(100),
	@Apellidos VARCHAR(100),
	@Correo VARCHAR(100),
	@Clave VARCHAR(100),
	@Activo BIT
)
AS BEGIN

	DECLARE @Resultado BIT = 0
	DECLARE @Mensaje VARCHAR(500)

	IF NOT EXISTS (SELECT * FROM USUARIO WHERE CORREO = @CORREO AND IdUsuario != @IdUsuario)
	BEGIN

	UPDATE TOP(1) USUARIO SET
	Nombres = @Nombres,
	Apellidos = @Apellidos,
	Correo = @Correo,
	Activo = @Activo
	WHERE IdUsuario = @IdUsuario

	SET @Resultado = 1
END
	ELSE

	SET @Mensaje = 'Mail in use.'
END
GO

IF EXISTS (SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = N'SP_RegisterCategory')
	DROP PROCEDURE SP_RegisterCategory
GO

CREATE PROCEDURE SP_RegisterCategory(
	@Descripcion VARCHAR(100),
	@Activo BIT,
	@Mensaje VARCHAR(500) output,
	@Resultado INT OUTPUT
)
AS BEGIN
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion)
		BEGIN
			INSERT INTO CATEGORIA (
			Descripcion,
			Activo) 
			VALUES
			(@Descripcion,
			@Activo)
			SET @Resultado = SCOPE_IDENTITY()
		END
	ELSE
		SET @Mensaje ='La categoria ya existe'
END
GO

IF EXISTS (SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = N'SP_EditCategory')
	DROP PROCEDURE SP_EditCategory
GO

CREATE PROCEDURE SP_EditCategory(
	@IdCategoria INT,
	@Descripcion VARCHAR(100),
	@Activo BIT,
	@Mensaje VARCHAR(500) output,
	@Resultado INT OUTPUT
)
AS BEGIN
	SET @Resultado = 0
		IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion AND IdCategoria != @IdCategoria)
		BEGIN
			UPDATE TOP (1) CATEGORIA SET
			Descripcion = @Descripcion,
			Activo = @Activo
			WHERE IdCategoria = @IdCategoria
			SET @Resultado = 1
		END
	ELSE
		SET @Mensaje ='La categoria ya existe' 
END
GO

IF EXISTS (SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = N'SP_DeleteCategory')
	DROP PROCEDURE SP_DeleteCategory
GO

CREATE PROCEDURE SP_DeleteCategory(
	@IdCategoria INT,
	@Mensaje VARCHAR(500) output,
	@Resultado INT OUTPUT
)
AS BEGIN
	SET @Resultado = 0
		IF NOT EXISTS (SELECT * FROM PRODUCTO P
			INNER JOIN CATEGORIA C ON C.IdCategoria = P.IdCategoria
			WHERE P.IdCategoria = @IdCategoria)
		BEGIN
			DELETE TOP(1) FROM CATEGORIA WHERE IdCategoria = @IdCategoria
			SET @Resultado = 1
		END
		ELSE
			SET @Mensaje ='La categoria se encuentra relacionada con el producto'
		END
GO

IF EXISTS (SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = N'SP_RegisterBrands')
	DROP PROCEDURE SP_RegisterBrands
GO

CREATE PROCEDURE SP_RegisterBrands(
	@Descripcion VARCHAR(100),
	@Activo BIT,
	@Mensaje VARCHAR(500) output,
	@Resultado INT OUTPUT
)
AS BEGIN
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM MARCA WHERE Descripcion = @Descripcion)
		BEGIN
			INSERT INTO MARCA (
			Descripcion,
			Activo) 
			VALUES
			(@Descripcion,
			@Activo)
			SET @Resultado = SCOPE_IDENTITY()
		END
	ELSE
		SET @Mensaje ='La marca ya existe'
END
GO

IF EXISTS (SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = N'SP_EditBrand')
	DROP PROCEDURE SP_EditBrand
GO

CREATE PROCEDURE SP_EditBrand(
	@IdMarca INT,
	@Descripcion VARCHAR(100),
	@Activo BIT,
	@Mensaje VARCHAR(500) output,
	@Resultado INT OUTPUT
)
AS BEGIN
	SET @Resultado = 0
		IF NOT EXISTS (SELECT * FROM MARCA WHERE Descripcion = @Descripcion AND IdMarca != @IdMarca)
		BEGIN
			UPDATE TOP (1) MARCA SET
			Descripcion = @Descripcion,
			Activo = @Activo
			WHERE IdMarca = @IdMarca

			SET @Resultado = 1
		END
	ELSE
		SET @Mensaje ='La categoria ya existe' 
END
GO

IF EXISTS (SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = N'SP_DeleteBrand')
	DROP PROCEDURE SP_DeleteBrand
GO

CREATE PROCEDURE SP_DeleteBrand(
	@IdMarca INT,
	@Mensaje VARCHAR(500) output,
	@Resultado INT OUTPUT
)
AS BEGIN
	SET @Resultado = 0
		IF NOT EXISTS (SELECT * FROM PRODUCTO P
			INNER JOIN MARCA C ON C.IdMarca = P.IdMarca
			WHERE P.IdMarca = @IdMarca)
		BEGIN
			DELETE TOP(1) FROM MARCA WHERE IdMarca = @IdMarca
			SET @Resultado = 1
		END
		ELSE
			SET @Mensaje ='La categoria se encuentra relacionada con el producto'
		END
GO


IF EXISTS (SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = N'SP_ProductRegister')
	DROP PROCEDURE SP_ProductRegister
GO

CREATE PROCEDURE SP_ProductRegister(
	  @Nombre varchar(100),
      @Descripcion varchar(500),
      @IdMarca int,
      @IdCategoria int,
      @Precio decimal(10,2),
      @Stock int,
      @Activo bit,
	  @Mensaje VARCHAR(500) output,
	  @Resultado INT OUTPUT
)
AS BEGIN 
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE Nombre = @Nombre)
		BEGIN
			INSERT INTO PRODUCTO(
			[Nombre]
		   ,[Descripcion]
		   ,[IdMarca]
		   ,[IdCategoria]
		   ,[Precio]
		   ,[Stock]
		   ,[Activo])
		   VALUES(
		   @Nombre,
		   @Descripcion,
		   @IdMarca,
		   @IdCategoria,
		   @Precio,
		   @Stock,
		   @Activo)
		   SET @Resultado = SCOPE_IDENTITY()
	   END
	ELSE
		SET @Mensaje = 'El producto ya existe'
	END
GO

IF EXISTS (SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = N'SP_ProductEdit')
	DROP PROCEDURE SP_ProductEdit
GO

CREATE PROCEDURE SP_ProductEdit(
	  @IdProducto INT,
	  @Nombre varchar(100),
      @Descripcion varchar(500),
      @IdMarca int,
      @IdCategoria int,
      @Precio decimal(10,2),
      @Stock int,
      @Activo bit,
	  @Mensaje VARCHAR(500) output,
	  @Resultado INT OUTPUT
)
AS BEGIN
	SET  @Resultado = 0
	IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE Nombre = @Nombre AND IdProducto != @IdProducto)
	BEGIN
		UPDATE PRODUCTO 
		SET
			Nombre = @Nombre,
			Descripcion =@Descripcion,
			IdMarca = @IdMarca,
			IdCategoria = @IdCategoria,
			Precio = @Precio,
			Stock = @Stock,
			Activo = @Activo
		where IdProducto = @IdProducto
		SET  @Resultado = 1
	END
	ELSE
		SET @Mensaje = 'El producto ya existe'
	END
GO

IF EXISTS (SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = N'SP_ProductDelete')
	DROP PROCEDURE SP_ProductDelete
GO

CREATE PROCEDURE SP_ProductDelete(
	 @IdProducto INT,
	 @Mensaje VARCHAR(500) output,
	 @Resultado INT OUTPUT
)
AS BEGIN
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM DETALLE_VENTA dv
		INNER JOIN Producto p on p.IdProducto = dv.IdProducto
		WHERE p.IdProducto = @IdProducto)
		BEGIN
			DELETE TOP(1) FROM Producto where IdProducto = @IdProducto
			SET @Resultado = 1
		END
	ELSE
		SET @Mensaje = 'El producto ya existe'
	END
GO

IF EXISTS (SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = N'SP_FullData')
	DROP PROCEDURE SP_FullData
GO

CREATE PROCEDURE SP_FULLDATA
AS BEGIN

	SELECT

	(SELECT COUNT(*) FROM CLIENTE) [TOTALCLIENTE],
	(SELECT ISNULL(SUM(CANTIDAD),0) FROM DETALLE_VENTA) [TOTALVENTA],
	(SELECT COUNT(*) FROM PRODUCTO) [TOTALPRODUCTO]
	
END

GO

IF EXISTS (SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_NAME = N'SP_DetalleVentaCliente')
	DROP PROCEDURE SP_DetalleVentaCliente
GO

CREATE PROCEDURE SP_DetalleVentaCliente(
@FechaInicio VARCHAR (10),
@FechaFin VARCHAR(10),
@idtransaccion VARCHAR(50)
)

AS BEGIN

SET DATEFORMAT  dmy;

SELECT CONVERT (char(10),v.FechaVenta,103) AS FechaVenta,CONCAT (c.Nombres,' ', c.Apellidos) AS Cliente,
p.Nombre AS Producto, p.Precio, dv.Cantidad, dv.Total, v.IdTransaccion
FROM DETALLE_VENTA dv
INNER JOIN PRODUCTO p ON P.IdProducto = dv.IdProducto
INNER JOIN VENTA v ON v.IdVenta = dv.IdVenta
INNER JOIN CLIENTE c ON c.IdCliente = v.IdCliente
WHERE CONVERT (date,v.FechaVenta) between @FechaInicio AND @FechaFin 
AND v.IdTransaccion = iif(@idtransaccion= '', v.IdTransaccion,@idtransaccion) 
END
GO