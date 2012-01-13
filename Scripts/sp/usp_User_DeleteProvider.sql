/****** Object:  StoredProcedure [dbo].[usp_User_DeleteProvider]    Script Date: 02/21/2011 16:44:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_User_DeleteProvider]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_User_DeleteProvider]
GO

/****** Object:  StoredProcedure [dbo].[usp_User_DeleteProvider]    Script Date: 02/21/2011 16:44:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_User_DeleteProvider]
	@ProviderUserId nvarchar(500)
	--@id uniqueidentifier output
AS	
BEGIN
	--SET @id = NEWID()
	UPDATE [dbo].[Users] SET ProviderId = 1, ProviderUserId = '' WHERE ProviderUserId = @ProviderUserId;
	--SELECT Id, Domain FROM [dbo].[Domains] WHERE Id = @id;
	select @@ROWCOUNT
END


GO