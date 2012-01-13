/****** Object:  Table [dbo].[Providers]    Script Date: 02/24/2011 14:51:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Providers]') AND type in (N'U'))
DROP TABLE [dbo].[Providers]
GO

/****** Object:  Table [dbo].[Providers]    Script Date: 02/24/2011 14:51:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Providers](
	[Id] int NOT NULL IDENTITY(1,1),
	[ProviderName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_dbo.Providers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

set identity_insert Providers on;

insert Providers(Id, ProviderName) values(1, 'Undefined');
insert Providers(Id, ProviderName) values(2, 'Twitter');
insert Providers(Id, ProviderName) values(3, 'OpenId');
insert Providers(Id, ProviderName) values(4, 'Facebook');
insert Providers(Id, ProviderName) values(5, 'VKontakte');
insert Providers(Id, ProviderName) values(6, 'Linkedin');
insert Providers(Id, ProviderName) values(7, 'MySpace');

set identity_insert Providers off;
go
