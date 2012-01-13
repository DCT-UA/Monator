/****** Object:  StoredProcedure [dbo].[usp_User_GetUserByProviderUserId]    Script Date: 04/04/2011 14:15:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_User_GetUserByProviderUserId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_User_GetUserByProviderUserId]
GO

/****** Object:  StoredProcedure [dbo].[usp_User_GetUserByProviderUserId]    Script Date: 04/04/2011 14:15:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_User_GetUserByProviderUserId]
	@ProviderUserId nvarchar(500)	
AS	
BEGIN
	SELECT * FROM [dbo].[Users] WHERE ProviderUserId = @ProviderUserId;
END


GO


