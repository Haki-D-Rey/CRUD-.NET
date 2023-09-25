USE SCELLBD
GO

CREATE OR ALTER PROCEDURE [dbo].[prActualizarProducto]

@IdProducto		INT,
@Descripcion	VARCHAR,
@Cantidad		INT,
@Precio			DECIMAL,
@EstadoActivo	BIT
AS
BEGIN
		--DECLARE @IdProducto		INT = 5,
		--		@Descripcion	VARCHAR(512) = 'Toalla Nevax',
		--		@Cantidad		INT = 10,
		--		@Precio			DECIMAL(12,5) = 258.09,
		--		@EstadoActivo	BIT = 1

		UPDATE [dbo].[Producto]
		SET Descripcion = @Descripcion, Cantidad = @Cantidad, Precio = @Precio, FechaModificacion = GETDATE(), EstadoActivo = @EstadoActivo
		WHERE IdProducto = @IdProducto

		SELECT *
		FROM [dbo].[Producto]
		WHERE IdProducto = @IdProducto
END