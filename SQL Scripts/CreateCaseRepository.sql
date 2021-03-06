USE [CaseRepository]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaseAppearInfoes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileNameId] [nvarchar](50) NOT NULL,
	[Appear_Type] [nvarchar](50) NULL,
	[Party] [nvarchar](max) NULL,
	[Bar] [nvarchar](max) NULL,
	[Solicitor] [nvarchar](max) NULL,
 CONSTRAINT [PK_CaseAppearInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaseCatchwords](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileNameId] [nvarchar](50) NOT NULL,
	[Catchword_Key] [nvarchar](max) NULL,
	[Catchword_KeySum] [nvarchar](max) NULL,
 CONSTRAINT [PK_CaseCatchword] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaseJudgments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileNameId] [nvarchar](50) NOT NULL,
	[JudgmentTitle] [nvarchar](max) NULL,
	[JudgmentText] [nvarchar](max) NULL,
 CONSTRAINT [PK_CaseJudgment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaseRepoes](
	[FileNameId] [nvarchar](50) NOT NULL,
	[Series] [nvarchar](50) NULL,
	[Year] [nvarchar](50) NULL,
	[Bcnum] [nvarchar](50) NULL,
	[Jurisdiction] [nvarchar](50) NULL,
	[CaseName] [nvarchar](800) NOT NULL,
	[Party1] [nvarchar](800) NOT NULL,
	[Party2] [nvarchar](800) NULL,
	[CourtId] [nvarchar](50) NULL,
	[CourtName] [nvarchar](800) NULL,
	[Mnc_Year] [nvarchar](50) NULL,
	[Mnc_CourtId] [nvarchar](50) NULL,
	[Mnc_Casenum] [nvarchar](800) NULL,
	[Mnc_Name] [nvarchar](max) NULL,
	[FileNo] [nvarchar](max) NULL,
	[DecisionDate] [datetime] NULL,
	[Judges] [nvarchar](max) NULL,
	[JudgmentJudges] [nvarchar](max) NULL,
	[LegislationRef] [nvarchar](max) NULL,
	[CaseOrder] [nvarchar](max) NULL,
 CONSTRAINT [PK_CaseRepo] PRIMARY KEY CLUSTERED 
(
	[FileNameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CaseAppearInfoes]  WITH CHECK ADD  CONSTRAINT [FK_CaseAppearInfo_CaseAppearInfo] FOREIGN KEY([FileNameId])
REFERENCES [dbo].[CaseRepoes] ([FileNameId])
GO
ALTER TABLE [dbo].[CaseAppearInfoes] CHECK CONSTRAINT [FK_CaseAppearInfo_CaseAppearInfo]
GO
ALTER TABLE [dbo].[CaseCatchwords]  WITH CHECK ADD  CONSTRAINT [FK_CaseCatchword_CaseRepo] FOREIGN KEY([FileNameId])
REFERENCES [dbo].[CaseRepoes] ([FileNameId])
GO
ALTER TABLE [dbo].[CaseCatchwords] CHECK CONSTRAINT [FK_CaseCatchword_CaseRepo]
GO
ALTER TABLE [dbo].[CaseJudgments]  WITH CHECK ADD  CONSTRAINT [FK_CaseJudgment_CaseRepo] FOREIGN KEY([FileNameId])
REFERENCES [dbo].[CaseRepoes] ([FileNameId])
GO
ALTER TABLE [dbo].[CaseJudgments] CHECK CONSTRAINT [FK_CaseJudgment_CaseRepo]
GO