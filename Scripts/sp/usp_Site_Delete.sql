
/****** Object:  StoredProcedure [dbo].[usp_Site_Delete]    Script Date: 04/04/2011 14:17:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Site_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Site_Delete]
GO

/****** Object:  StoredProcedure [dbo].[usp_Site_Delete]    Script Date: 04/04/2011 14:17:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



Create procedure [dbo].[usp_Site_Delete]
	@Id uniqueidentifier
as begin
	delete from [dbo].[Domains] where Id = @Id;
	
	return 0
end


GO
