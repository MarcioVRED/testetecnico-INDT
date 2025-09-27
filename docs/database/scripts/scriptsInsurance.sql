USE [InsuranceDb]
GO

/****** Object:  Table [dbo].[Propostas]    Script Date: 26/09/2025 21:54:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Propostas]') AND type in (N'U'))
DROP TABLE [dbo].[Propostas]
GO

/****** Object:  Table [dbo].[Propostas]    Script Date: 26/09/2025 21:54:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Propostas](
	[Id] [uniqueidentifier] NOT NULL,
	[Cliente] [nvarchar](max) NOT NULL,
	[Valor] [decimal](18, 2) NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Propostas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/****** Object:  Table [dbo].[Contratacoes]    Script Date: 26/09/2025 21:55:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contratacoes]') AND type in (N'U'))
DROP TABLE [dbo].[Contratacoes]
GO

/****** Object:  Table [dbo].[Contratacoes]    Script Date: 26/09/2025 21:55:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Contratacoes](
	[Id] [uniqueidentifier] NOT NULL,
	[PropostaId] [uniqueidentifier] NOT NULL,
	[DataContratacao] [datetime2](7) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Contratacoes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

