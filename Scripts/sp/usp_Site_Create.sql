/****** Object:  StoredProcedure [dbo].[usp_Site_Create]    Script Date: 02/21/2011 16:44:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Site_Create]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Site_Create]
GO

/****** Object:  StoredProcedure [dbo].[usp_Site_Create]    Script Date: 02/21/2011 16:44:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[usp_Site_Create]
	@IgnoreSubdomains bit,
	@UserId uniqueidentifier,
	@Domain nvarchar(max),
	@Id uniqueidentifier output
AS	
BEGIN
	SET @id = NEWID()
	INSERT [dbo].[Domains] VALUES(@Id, @UserId, @Domain, @IgnoreSubdomains);
	SELECT Id, Domain FROM [dbo].[Domains] WHERE Id = @Id;
END


GO