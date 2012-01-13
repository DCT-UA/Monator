IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Domain_Users1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Domains]'))
ALTER TABLE [dbo].[Domains] DROP CONSTRAINT [FK_Domain_Users1]
GO

/****** Object:  Table [dbo].[Domains]    Script Date: 03/28/2011 14:53:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Domains]') AND type in (N'U'))
DROP TABLE [dbo].[Domains]
GO

/****** Object:  Table [dbo].[Domains]    Script Date: 03/28/2011 14:53:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Domains](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Domain] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Domain] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Domains]  WITH CHECK ADD  CONSTRAINT [FK_Domain_Users1] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Domains] CHECK CONSTRAINT [FK_Domain_Users1]
GO
