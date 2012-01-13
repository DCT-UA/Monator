/****** Object:  StoredProcedure [dbo].[usp_User_GetUserByName]    Script Date: 03/3/2011 16:48:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_User_GetUserByName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_User_GetUserByName]
GO

/****** Object:  StoredProcedure [dbo].[usp_User_GetUserByName]    Script Date: 03/3/2011 16:48:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_User_GetUserByName]
	@UserName nvarchar(100)	
AS	
BEGIN
	SELECT * FROM [dbo].[Users] WHERE UserName = @UserName;
END

GO