
/****** Object:  StoredProcedure [dbo].[usp_User_Create]    Script Date: 04/04/2011 14:17:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_User_Create]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_User_Create]
GO

/****** Object:  StoredProcedure [dbo].[usp_User_Create]    Script Date: 04/04/2011 14:17:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



Create procedure [dbo].[usp_User_Create]
	@UserName nvarchar(50),
	@Password nvarchar(300),
	@Email nvarchar(200),
	@ProviderId int,
	@ProviderUserId nvarchar(500),
	@Id uniqueidentifier output
as begin
	SET @Id = NEWID()
	insert dbo.Users(Id, UserName, Password, Email, ProviderId, ProviderUserId)
	values (@Id, @UserName, @Password, @Email, @ProviderId, @ProviderUserId)
	
	return 0
end


GO


