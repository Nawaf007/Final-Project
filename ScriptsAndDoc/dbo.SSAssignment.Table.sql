USE [DB34]
GO
/****** Object:  Table [dbo].[SSAssignment]    Script Date: 4/13/2019 4:15:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SSAssignment](
	[AssignmentId] [int] NOT NULL,
	[SSId] [int] NOT NULL,
	[ObtainedMarks] [int] NOT NULL,
	[FilePath] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[SSAssignment]  WITH CHECK ADD  CONSTRAINT [FK_SSAssignment_Assignment] FOREIGN KEY([AssignmentId])
REFERENCES [dbo].[Assignment] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SSAssignment] CHECK CONSTRAINT [FK_SSAssignment_Assignment]
GO
ALTER TABLE [dbo].[SSAssignment]  WITH CHECK ADD  CONSTRAINT [FK_SSAssignment_SectionStudent] FOREIGN KEY([SSId])
REFERENCES [dbo].[SectionStudent] ([Id])
GO
ALTER TABLE [dbo].[SSAssignment] CHECK CONSTRAINT [FK_SSAssignment_SectionStudent]
GO
