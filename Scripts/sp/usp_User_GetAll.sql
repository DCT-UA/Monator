/****** Object:  StoredProcedure [dbo].[usp_User_GetAll]    Script Date: 02/27/2011 16:45:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_User_GetAll]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_User_GetAll]
GO

/****** Object:  StoredProcedure [dbo].[usp_User_GetAll]    Script Date: 02/27/2011 16:45:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_User_GetAll]
AS
BEGIN
	SELECT * FROM [MonitorDB].[dbo].[Users];
END

GO
