/****** Object:  StoredProcedure [dbo].[usp_PageRequest_Create]    Script Date: 02/21/2011 16:46:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_PageRequest_Create]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_PageRequest_Create]
GO

/****** Object:  StoredProcedure [dbo].[usp_PageRequest_Create]    Script Date: 02/21/2011 16:46:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_PageRequest_Create] 
	@Domain nvarchar(max),
	@Url nvarchar(max),
	@IpAdress nvarchar(50),
	@Browser smallint,
	@SessionId uniqueidentifier,
	@Refferer nvarchar(max),
	@SiteId uniqueidentifier,
	@Id uniqueidentifier output
AS
BEGIN
	SET @Id = NEWID()
	INSERT Requests(Domain, Url, IpAdress, Browser, Id, SessionId, Refferer)
		VALUES (@Domain, @Url, @IpAdress, @Browser, @Id, @SessionId, @Refferer);
END

GO