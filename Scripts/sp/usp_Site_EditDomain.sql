/****** Object:  StoredProcedure [dbo].[usp_Site_Update]    Script Date: 02/21/2011 16:44:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Site_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Site_Update]
GO

/****** Object:  StoredProcedure [dbo].[usp_Site_Update]    Script Date: 02/21/2011 16:44:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_Site_Update]
	@Id uniqueidentifier,
	@UserId uniqueidentifier,
	@Domain nvarchar(max),
	@IgnoreSubdomains bit
AS	
BEGIN
	UPDATE [dbo].[Domains] SET Domain = @Domain, IgnoreSubdomains = @IgnoreSubdomains  WHERE Id = @Id;
	select @@ROWCOUNT
END

GO
