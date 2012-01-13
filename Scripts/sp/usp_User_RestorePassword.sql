USE [MonitorDB]
GO
/****** Object:  StoredProcedure [dbo].[usp_User_RestorePassword]    Script Date: 04/04/2011 14:17:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_User_RestorePassword]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_User_RestorePassword]
GO
/****** Object:  StoredProcedure [dbo].[usp_User_RestorePassword]    Script Date: 03/25/2011 20:20:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[usp_User_RestorePassword]
	@Id uniqueidentifier,
	@NewPassword nvarchar(300)
as begin
	update dbo.Users set Password = @NewPassword where Id = @Id
	
	select @@ROWCOUNT
end