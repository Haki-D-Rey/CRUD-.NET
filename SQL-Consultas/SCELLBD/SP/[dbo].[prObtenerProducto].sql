USE [SCELLBD]
GO
/****** Object:  StoredProcedure [dbo].[prObtenerProducto]    Script Date: 23/09/2023 12:59:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	-- GET PRODUCTO BD
	ALTER   PROCEDURE [dbo].[prObtenerProducto]
	AS
	BEGIN
			SELECT *
			FROM dbo.Producto	
	END