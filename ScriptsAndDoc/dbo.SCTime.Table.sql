USE [DB34]
GO
/****** Object:  Table [dbo].[SCTime]    Script Date: 4/13/2019 4:15:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SCTime](
	[SectionId] [int] NOT NULL,
	[DayOfWeek] [int] NOT NULL,
	[TimeOfDay] [datetime] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[SCTime]  WITH CHECK ADD  CONSTRAINT [FK_SCTime_Lookup] FOREIGN KEY([DayOfWeek])
REFERENCES [dbo].[Lookup] ([Id])
GO
ALTER TABLE [dbo].[SCTime] CHECK CONSTRAINT [FK_SCTime_Lookup]
GO
ALTER TABLE [dbo].[SCTime]  WITH CHECK ADD  CONSTRAINT [FK_SCTime_SectionCourse] FOREIGN KEY([SectionId])
REFERENCES [dbo].[SectionCourse] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SCTime] CHECK CONSTRAINT [FK_SCTime_SectionCourse]
GO
