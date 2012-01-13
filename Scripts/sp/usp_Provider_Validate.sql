/****** Object:  StoredProcedure [dbo].[usp_Provider_Validate]    Script Date: 03/09/2011 16:47:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Provider_Validate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Provider_Validate]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[usp_Provider_Validate]
	@ProviderName nvarchar(50),
	@Id int out
as begin
	declare @temp int = null
	select @temp = Id from dbo.Providers where ProviderName = @ProviderName
	
	set @Id = @temp
end


GO