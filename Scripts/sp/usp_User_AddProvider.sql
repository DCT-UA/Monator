/****** Object:  StoredProcedure [dbo].[usp_User_AddProvider]    Script Date: 02/21/2011 16:44:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_User_AddProvider]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_User_AddProvider]
GO

/****** Object:  StoredProcedure [dbo].[usp_User_AddProvider]    Script Date: 02/21/2011 16:44:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_User_AddProvider]
	@ProviderId int,
	@ProviderUserId nvarchar(500),
	@Id uniqueidentifier
	--@id uniqueidentifier output
AS	
BEGIN
	--SET @id = NEWID()
	UPDATE [dbo].[Users] SET ProviderId = @ProviderId, ProviderUserId = @ProviderUserId WHERE Id = @Id;
	--SELECT Id, Domain FROM [dbo].[Domains] WHERE Id = @id;
	select @@ROWCOUNT
END


GO