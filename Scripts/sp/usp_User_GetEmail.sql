
/****** Object:  StoredProcedure [dbo].[usp_User_GetEmail]    Script Date: 04/04/2011 14:17:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_User_GetEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_User_GetEmail]
GO

/****** Object:  StoredProcedure [dbo].[usp_User_GetEmail]    Script Date: 04/04/2011 14:17:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create procedure [dbo].[usp_User_GetEmail]
	@UserName nvarchar(50)
as begin
	SELECT Email from [dbo].[Users] where UserName = @UserName;
end


GO