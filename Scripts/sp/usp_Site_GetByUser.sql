/****** Object:  StoredProcedure [dbo].[usp_Site_GetByUser]    Script Date: 02/21/2011 16:46:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Site_GetByUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Site_GetByUser]
GO

/****** Object:  StoredProcedure [dbo].[usp_Site_GetByUser]    Script Date: 02/21/2011 16:46:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_Site_GetByUser]
	@userId uniqueidentifier	
AS	
BEGIN
	SELECT * FROM [dbo].[Domains] WHERE UserId = @userId;
END

GO