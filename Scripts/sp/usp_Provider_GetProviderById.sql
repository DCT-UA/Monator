/****** Object:  StoredProcedure [dbo].[usp_Provider_GetById]    Script Date: 04/04/2011 14:18:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Provider_GetById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Provider_GetById]
GO

/****** Object:  StoredProcedure [dbo].[usp_Provider_GetById]    Script Date: 04/04/2011 14:18:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_Provider_GetById]
	@Id int
AS	
BEGIN
	SELECT * FROM [dbo].[Providers] WHERE Id = @Id;
END


GO


