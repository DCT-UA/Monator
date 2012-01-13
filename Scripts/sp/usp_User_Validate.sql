/****** Object:  StoredProcedure [dbo].[usp_User_Validate]    Script Date: 02/21/2011 16:47:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_User_Validate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_User_Validate]
GO

/****** Object:  StoredProcedure [dbo].[usp_User_Validate]    Script Date: 02/21/2011 16:47:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[usp_User_Validate]
	@UserName nvarchar(50),
	@Password nvarchar(300),
	@Id uniqueidentifier out
as begin
	declare @temp uniqueidentifier = null
	select @temp = Id from dbo.Users where Password = @Password and UserName = @UserName
	
	set @Id = @temp
end


GO