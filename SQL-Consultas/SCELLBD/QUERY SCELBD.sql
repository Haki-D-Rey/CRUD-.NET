	-- Verificar si la base de datos existe
	IF EXISTS (SELECT name FROM sys.databases WHERE name = 'SCELLBD')
	BEGIN
	    -- Si existe, eliminarla
	    USE master;
	    ALTER DATABASE SCELLBD
	    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	    DROP DATABASE SCELLBD;
	END

	CREATE DATABASE SCELLBD
	GO
	
	USE SCELLBD
	GO

	DBCC CHECKIDENT ('Producto', RESEED, 4);
	DROP TABLE dbo.Cliente
	DROP TABLE dbo.Descuento
	DROP TABLE dbo.Usuario
	DROP TABLE dbo.Producto


	CREATE TABLE Cliente
	(
		IdCliente			INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
		Nombre				VARCHAR(258) NULL,
		Apellido			VARCHAR(258) NULL,
		Telefono			VARCHAR(128) NULL,
		Pais				VARCHAR(128) NULL,
		CodigoInternoPais	VARCHAR(128) NULL,
		Ciudad			 	VARCHAR(128) NULL,
		FechaCreacion		DATETIME NOT NULL DEFAULT GETDATE(),
		FechaModificacion	DATETIME NULL,
		EstadoActivo	 	BIT NOT NULL
	)

	CREATE TABLE Usuario
	(
		IdUsuario			INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
		IdCliente			INT NOT NULL,
		NombreUsuario		VARCHAR(258) NULL,
		Contrasena			VARCHAR(MAX) NOT NULL,
		FechaCreacion		DATETIME NOT NULL DEFAULT GETDATE(),
		FechaModificacion	DATETIME NULL,
		EstadoActivo		BIT NOT NULL
		CONSTRAINT FK_IdCliente FOREIGN KEY (IdCliente) REFERENCES Cliente(IdCliente)
	)

	CREATE TABLE Producto
	(
		IdProducto			INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
		Descripcion			VARCHAR(2056) NULL,
		Cantidad			INT NOT NULL DEFAULT 0,
		Precio     			DECIMAL(18,2) NOT NULL DEFAULT 0.00,
		FechaCreacion		DATETIME NOT NULL DEFAULT GETDATE(),
		FechaModificacion	DATETIME NULL,
		EstadoActivo		BIT NOT NULL
	)

	CREATE TABLE Descuento
	(
		IdDescuento					INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
		IdProducto					INT NOT NULL, 
		IdCliente					INT NOT NULL, 
		Descripcion					VARCHAR(2056) NULL,
		PorcentajeDescuento			DECIMAL(5,2) NOT NULL DEFAULT 0.00,
		FechaPromocion				DATETIME NOT NULL,
		FechaCreacion				DATETIME NOT NULL DEFAULT GETDATE(),
		FechaModificacion			DATETIME NULL,
		EstadoActivo				BIT NOT NULL,
		CONSTRAINT FK_IdDescuentoCliente FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto),
		CONSTRAINT FK_IdDescuentoProducto FOREIGN KEY (IdCliente) REFERENCES Cliente(IdCliente)
	)

