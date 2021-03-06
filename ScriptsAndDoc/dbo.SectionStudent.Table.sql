USE [DB34]
GO
/****** Object:  Table [dbo].[SectionStudent]    Script Date: 4/13/2019 4:15:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SectionStudent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SectionId] [int] NOT NULL,
	[StudentId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_SectionStudent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[SectionStudent]  WITH CHECK ADD  CONSTRAINT [FK_SectionStudent_Section] FOREIGN KEY([SectionId])
REFERENCES [dbo].[Section] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SectionStudent] CHECK CONSTRAINT [FK_SectionStudent_Section]
GO
ALTER TABLE [dbo].[SectionStudent]  WITH CHECK ADD  CONSTRAINT [FK_SectionStudent_Student] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SectionStudent] CHECK CONSTRAINT [FK_SectionStudent_Student]
GO
