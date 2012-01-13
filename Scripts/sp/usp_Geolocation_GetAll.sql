
/****** Object:  StoredProcedure [dbo].[usp_Geolocation_GetAll]    Script Date: 02/21/2011 16:45:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Geolocation_GetAll]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Geolocation_GetAll]
GO

/****** Object:  StoredProcedure [dbo].[usp_Geolocation_GetAll]    Script Date: 02/21/2011 16:45:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_Geolocation_GetAll]
AS
BEGIN
	SELECT * FROM [MonitorDB].[dbo].[Geolocation] order by IpMin;
END

GO