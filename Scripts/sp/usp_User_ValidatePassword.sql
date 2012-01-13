USE [MonitorDB]
GO
/****** Object:  StoredProcedure [dbo].[usp_User_ValidatePassword]    Script Date: 03/25/2011 20:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[usp_User_ValidatePassword]
	@IdIn uniqueidentifier,
	@Password nvarchar(300),
	@IdOut uniqueidentifier out
as begin
	declare @temp uniqueidentifier = null
	select @temp = Id from dbo.Users where Password = @Password and Id = @IdIn
	
	set @IdOut = @temp
end


