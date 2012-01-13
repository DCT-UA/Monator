/****** Object:  StoredProcedure [dbo].[usp_User_GetProviderByName]    Script Date: 02/27/2011 16:48:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_User_GetProviderByName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_User_GetProviderByName]
GO

/****** Object:  StoredProcedure [dbo].[usp_User_GetProviderByName]    Script Date: 02/27/2011 16:48:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_User_GetProviderByName]
	@providerName nvarchar(100)	
AS	
BEGIN
	SELECT * FROM [dbo].[Providers] WHERE ProviderName = @providerName;
END

GO
