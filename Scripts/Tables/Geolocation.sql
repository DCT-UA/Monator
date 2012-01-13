USE [MonitorDB]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Geolocation]') AND type in (N'P', N'PC'))
DROP TABLE [dbo].[Geolocation]
GO
/****** Object:  Table [dbo].[Geolocation]    Script Date: 04/11/2011 16:28:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Geolocation](
	[IpMin] [bigint] NOT NULL,
	[IpMax] [bigint] NOT NULL,
	[Latitude] [float] NULL,
	[Longitude] [float]NULL,
) ON [PRIMARY]

GO

