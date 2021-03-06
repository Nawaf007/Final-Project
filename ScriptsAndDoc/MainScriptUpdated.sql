USE [DB34]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[User_Id] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NULL,
	[TwoFactorEnabled] [bit] NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NULL,
	[AccessFailedCount] [int] NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[Gender] [int] NULL,
	[Admin] [nvarchar](50) NULL,
	[Discriminator] [nvarchar](max) NOT NULL,
	[FirstName] [nvarchar](20) NULL,
	[LastName] [nvarchar](20) NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Assignment]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Assignment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SCId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DueDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[TotalMarks] [int] NOT NULL,
	[Weightage] [int] NOT NULL,
	[FilePath] [nvarchar](max) NULL,
 CONSTRAINT [PK_Assignment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Attendance]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendance](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_Attendance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Course]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Status] [int] NOT NULL,
	[Class] [int] NOT NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Exam]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exam](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SCId] [int] NOT NULL,
	[ExamType] [int] NOT NULL,
	[ExamDate] [datetime] NOT NULL,
	[TotalMarks] [int] NOT NULL,
	[Weightage] [int] NOT NULL,
 CONSTRAINT [PK_Exam] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Fees]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fees](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [int] NOT NULL,
	[DueDate] [datetime] NOT NULL,
	[Class] [int] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Fees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FeesStudent]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeesStudent](
	[FeeId] [int] NOT NULL,
	[StudentId] [nvarchar](128) NOT NULL,
	[AmountPaid] [int] NOT NULL,
	[SubmitDate] [datetime] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_FeesStudent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Lookup]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lookup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Category] [nchar](20) NOT NULL,
	[Values] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Lookup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LookupCategory]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_LookupCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Request]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Request](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[AspId] [nvarchar](128) NOT NULL,
	[Topic] [nvarchar](20) NOT NULL,
	[Acknowledged] [nvarchar](128) NULL,
 CONSTRAINT [PK_Request] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SalaryPaid]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalaryPaid](
	[TeacherId] [nvarchar](128) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_SalaryPaid] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SCTime]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SCTime](
	[SectionId] [int] NOT NULL,
	[DayOfWeek] [int] NOT NULL,
	[TimeOfDay] [datetime] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_SCTime] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Section]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Section](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[MaxCount] [int] NOT NULL,
	[Class] [int] NOT NULL,
 CONSTRAINT [PK_Section] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SectionCourse]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SectionCourse](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SectionId] [int] NOT NULL,
	[CourseId] [int] NOT NULL,
	[TeacherId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_SectionCourse] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SectionStudent]    Script Date: 5/4/2019 1:47:04 PM ******/
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
/****** Object:  Table [dbo].[SSAssignment]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SSAssignment](
	[AssignmentId] [int] NOT NULL,
	[SSId] [int] NOT NULL,
	[ObtainedMarks] [int] NULL,
	[FilePath] [nvarchar](max) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_SSAssignment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SSExam]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SSExam](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExamId] [int] NOT NULL,
	[ObtainedMarks] [int] NOT NULL,
	[SSId] [int] NOT NULL,
 CONSTRAINT [PK_SSExam] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Student]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[Id] [nvarchar](128) NOT NULL,
	[EnrollementDate] [datetime] NOT NULL,
	[RegistrationNo] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StudentAttendance]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentAttendance](
	[AttendanceId] [int] NOT NULL,
	[StudentId] [nvarchar](128) NOT NULL,
	[Status] [int] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_StudentAttendance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Teacher]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teacher](
	[Id] [nvarchar](128) NOT NULL,
	[Salary] [int] NULL,
 CONSTRAINT [PK_dbo.Teacher] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TeacherAttendance]    Script Date: 5/4/2019 1:47:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeacherAttendance](
	[AttendanceId] [int] NOT NULL,
	[TeacherId] [nvarchar](128) NOT NULL,
	[Status] [int] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_TeacherAttendance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Gender], [Admin], [Discriminator], [FirstName], [LastName]) VALUES (N'3ae33e00-fb84-4a33-9f77-3c08538ea8e4', N'Nawaf.sheikh009@gmail.com', 0, N'ALBPyynlw+8RnFPM897Xr4tD8z+mNuBPkKDSAbet1gR3S0nfZfnc67yiXagO83vrcg==', N'1717db6d-9785-4e60-b6b7-62cba7b0a147', N'03474530032', 0, NULL, NULL, NULL, NULL, N'Nawaf009', 1, N'6', N'ApplicationUser', N'Nawaf', N'Sheikh')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Gender], [Admin], [Discriminator], [FirstName], [LastName]) VALUES (N'6582b089-fbca-4f4c-90dd-2ec370e14bc6', N'Nawaf.sheikh009@gmail.com', 0, N'AEOnJr3yE97QO0K8Y1ggXeT9DsMGtHKdjy6KL1bazEviJakc8V4MAfDwK7eZryGcjA==', N'cde44204-3d3d-417e-99e8-038a8acc60fc', N'03474530032', 0, NULL, NULL, NULL, NULL, N'Hamza1122', 1, N'5', N'ApplicationUser', N'hamza', N'Ahmad')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Gender], [Admin], [Discriminator], [FirstName], [LastName]) VALUES (N'ae1bac60-4e33-4556-9bd3-a9f6dce1fbde', N'Nawaf.sheikh009@gmail.com', 0, N'ACyexQiF76cqmMQ4j/bsjKdMic4yXdO4oPQgLDG4/+vrTJYAm9d1v0fHgCx0jJ9Mbg==', N'879b6d54-ecde-45bb-a273-1a96f3af8026', N'03474530032', 0, NULL, NULL, NULL, NULL, N'Ali1100', 1, N'7', N'ApplicationUser', N'Ali', N'Bhatti')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Gender], [Admin], [Discriminator], [FirstName], [LastName]) VALUES (N'b79e3f98-e25d-4d90-bdd1-4de1ffc1d75e', N'Nawaf.sheikh009@gmail.com', 0, N'AMKFDsGq71qjzZ1uLXHsdbdgSMvWlt1RCX5XF7F4cMJf/v+P75uiAChRTjP5rdvJ8g==', N'20655dc9-01cd-43ab-bea2-465cc4d644c5', N'03474530032', 0, NULL, NULL, NULL, NULL, N'Nawaf010', 1, N'5', N'ApplicationUser', N'Nawaf', N'Sheikh')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Gender], [Admin], [Discriminator], [FirstName], [LastName]) VALUES (N'c6525a88-d553-48fe-bb72-92da8adfdbd7', N'Nawaf.sheikh009@gmail.com', 0, N'AA2xlpJ45q9WHFqfBbYyEQ7SfERa6zeIvXaMuprJvXyeT3c5HxKTdJGmDP1IFFDGHA==', N'6be54e33-3604-479c-868a-1350ab2d9ce3', N'03474530032', 0, NULL, NULL, NULL, NULL, N'Nawaf008', 1, N'7', N'ApplicationUser', N'Ali', N'Bhatti')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Gender], [Admin], [Discriminator], [FirstName], [LastName]) VALUES (N'ddfc0b02-8026-45fe-9cf4-bd28a26bf366', N'Nawaf.sheikh009@gmail.com', 0, N'AEJATc6qJoKkRpOyjV9d5d/x2myFcNubwxw4tKUj3217ON1ZGdTwEY3cxvIAgxPyLg==', N'cd0f2244-cda2-4b73-935e-69b780233122', N'03474530032', 0, NULL, NULL, NULL, NULL, N'AliHaider12', 1, N'6', N'ApplicationUser', N'Ali', N'Bhatti')
SET IDENTITY_INSERT [dbo].[Assignment] ON 

INSERT [dbo].[Assignment] ([Id], [SCId], [Name], [DueDate], [Status], [TotalMarks], [Weightage], [FilePath]) VALUES (6, 4, N'A1', CAST(N'1998-12-22 00:00:00.000' AS DateTime), 35, 100, 30, N'/Assignments/Manuals/6.pdf')
INSERT [dbo].[Assignment] ([Id], [SCId], [Name], [DueDate], [Status], [TotalMarks], [Weightage], [FilePath]) VALUES (7, 4, N'A2', CAST(N'1998-12-22 00:00:00.000' AS DateTime), 35, 100, 30, N' ')
INSERT [dbo].[Assignment] ([Id], [SCId], [Name], [DueDate], [Status], [TotalMarks], [Weightage], [FilePath]) VALUES (8, 4, N'A3', CAST(N'1998-12-22 00:00:00.000' AS DateTime), 35, 100, 30, N' ')
SET IDENTITY_INSERT [dbo].[Assignment] OFF
SET IDENTITY_INSERT [dbo].[Attendance] ON 

INSERT [dbo].[Attendance] ([Id], [Date], [Type]) VALUES (3, CAST(N'2019-04-22 00:00:00.000' AS DateTime), 23)
SET IDENTITY_INSERT [dbo].[Attendance] OFF
SET IDENTITY_INSERT [dbo].[Course] ON 

INSERT [dbo].[Course] ([Id], [Title], [Status], [Class]) VALUES (4, N'Physics', 19, 17)
INSERT [dbo].[Course] ([Id], [Title], [Status], [Class]) VALUES (5, N'Chemistry', 19, 18)
SET IDENTITY_INSERT [dbo].[Course] OFF
SET IDENTITY_INSERT [dbo].[Exam] ON 

INSERT [dbo].[Exam] ([Id], [SCId], [ExamType], [ExamDate], [TotalMarks], [Weightage]) VALUES (3, 4, 34, CAST(N'2021-12-10 00:00:00.000' AS DateTime), 100, 50)
INSERT [dbo].[Exam] ([Id], [SCId], [ExamType], [ExamDate], [TotalMarks], [Weightage]) VALUES (4, 4, 33, CAST(N'2019-04-27 11:00:00.000' AS DateTime), 100, 40)
INSERT [dbo].[Exam] ([Id], [SCId], [ExamType], [ExamDate], [TotalMarks], [Weightage]) VALUES (5, 5, 33, CAST(N'2019-04-29 11:00:00.000' AS DateTime), 100, 60)
INSERT [dbo].[Exam] ([Id], [SCId], [ExamType], [ExamDate], [TotalMarks], [Weightage]) VALUES (6, 5, 34, CAST(N'2021-12-10 00:00:00.000' AS DateTime), 100, 40)
SET IDENTITY_INSERT [dbo].[Exam] OFF
SET IDENTITY_INSERT [dbo].[Fees] ON 

INSERT [dbo].[Fees] ([Id], [Amount], [DueDate], [Class], [Name]) VALUES (1, 2000, CAST(N'1998-12-22 00:00:00.000' AS DateTime), 17, N'1st')
INSERT [dbo].[Fees] ([Id], [Amount], [DueDate], [Class], [Name]) VALUES (2, 2000, CAST(N'1998-12-22 00:00:00.000' AS DateTime), 12, N'2nd')
INSERT [dbo].[Fees] ([Id], [Amount], [DueDate], [Class], [Name]) VALUES (3, 2000, CAST(N'2019-05-04 00:00:00.000' AS DateTime), 18, N'3rd')
SET IDENTITY_INSERT [dbo].[Fees] OFF
SET IDENTITY_INSERT [dbo].[FeesStudent] ON 

INSERT [dbo].[FeesStudent] ([FeeId], [StudentId], [AmountPaid], [SubmitDate], [Id]) VALUES (1, N'c6525a88-d553-48fe-bb72-92da8adfdbd7', 20000, CAST(N'1998-12-22 00:00:00.000' AS DateTime), 3)
INSERT [dbo].[FeesStudent] ([FeeId], [StudentId], [AmountPaid], [SubmitDate], [Id]) VALUES (3, N'c6525a88-d553-48fe-bb72-92da8adfdbd7', 2000, CAST(N'2019-05-04 00:00:00.000' AS DateTime), 4)
SET IDENTITY_INSERT [dbo].[FeesStudent] OFF
SET IDENTITY_INSERT [dbo].[Lookup] ON 

INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (1, N'GENDER              ', N'Male')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (5, N'ADMIN               ', N'Admin')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (6, N'ADMIN               ', N'Teacher')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (7, N'ADMIN               ', N'Student')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (8, N'CLASS               ', N'1')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (9, N'CLASS               ', N'2')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (11, N'CLASS               ', N'3')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (12, N'CLASS               ', N'4')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (13, N'CLASS               ', N'5')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (14, N'CLASS               ', N'6')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (15, N'CLASS               ', N'7')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (16, N'CLASS               ', N'8')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (17, N'CLASS               ', N'9')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (18, N'CLASS               ', N'10')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (19, N'COURSESTATUS        ', N'Active')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (20, N'COURSESTATUS        ', N'InActive')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (21, N'ATTENDANCETYPE      ', N'Student')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (22, N'ATTENDANCETYPE      ', N'Teacher')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (23, N'ATTENDANCETYPE      ', N'Both')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (24, N'ATTENDANCESTATUS    ', N'Present')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (25, N'ATTENDANCESTATUS    ', N'Absent')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (26, N'SALARYSTATUS        ', N'Paid')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (27, N'SALARYSTATUS        ', N'Not Paid')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (28, N'WORKINGDAYS         ', N'Monday')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (29, N'WORKINGDAYS         ', N'Tuesday')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (30, N'WORKINGDAYS         ', N'Wednesday')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (31, N'WORKINGDAYS         ', N'Thursday')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (32, N'WORKINGDAYS         ', N'Friday')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (33, N'EXAM                ', N'Mid-Term')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (34, N'EXAM                ', N'Final-Term')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (35, N'ASSIGNMENTSTATUS    ', N'Active')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (36, N'ASSIGNMENTSTATUS    ', N'InActive')
INSERT [dbo].[Lookup] ([Id], [Category], [Values]) VALUES (37, N'GENDER              ', N'Female')
SET IDENTITY_INSERT [dbo].[Lookup] OFF
SET IDENTITY_INSERT [dbo].[LookupCategory] ON 

INSERT [dbo].[LookupCategory] ([Id], [Name]) VALUES (1, N'GENDER')
INSERT [dbo].[LookupCategory] ([Id], [Name]) VALUES (2, N'ADMIN')
SET IDENTITY_INSERT [dbo].[LookupCategory] OFF
SET IDENTITY_INSERT [dbo].[Request] ON 

INSERT [dbo].[Request] ([Id], [Description], [AspId], [Topic], [Acknowledged]) VALUES (1, N'hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh', N'c6525a88-d553-48fe-bb72-92da8adfdbd7', N'hello', N'b79e3f98-e25d-4d90-bdd1-4de1ffc1d75e')
INSERT [dbo].[Request] ([Id], [Description], [AspId], [Topic], [Acknowledged]) VALUES (2, N'jgvdbvbdhv', N'3ae33e00-fb84-4a33-9f77-3c08538ea8e4', N'bscscsb', N'b79e3f98-e25d-4d90-bdd1-4de1ffc1d75e')
SET IDENTITY_INSERT [dbo].[Request] OFF
SET IDENTITY_INSERT [dbo].[SalaryPaid] ON 

INSERT [dbo].[SalaryPaid] ([TeacherId], [Date], [Status], [Id]) VALUES (N'3ae33e00-fb84-4a33-9f77-3c08538ea8e4', CAST(N'1998-12-22 00:00:00.000' AS DateTime), 26, 1)
INSERT [dbo].[SalaryPaid] ([TeacherId], [Date], [Status], [Id]) VALUES (N'ddfc0b02-8026-45fe-9cf4-bd28a26bf366', CAST(N'2019-04-22 00:00:00.000' AS DateTime), 26, 2)
SET IDENTITY_INSERT [dbo].[SalaryPaid] OFF
SET IDENTITY_INSERT [dbo].[SCTime] ON 

INSERT [dbo].[SCTime] ([SectionId], [DayOfWeek], [TimeOfDay], [Id]) VALUES (4, 28, CAST(N'2019-04-25 14:00:00.000' AS DateTime), 2)
INSERT [dbo].[SCTime] ([SectionId], [DayOfWeek], [TimeOfDay], [Id]) VALUES (4, 28, CAST(N'2019-04-25 13:00:00.000' AS DateTime), 3)
SET IDENTITY_INSERT [dbo].[SCTime] OFF
SET IDENTITY_INSERT [dbo].[Section] ON 

INSERT [dbo].[Section] ([Id], [Name], [MaxCount], [Class]) VALUES (1, N'A', 40, 17)
INSERT [dbo].[Section] ([Id], [Name], [MaxCount], [Class]) VALUES (2, N'A', 40, 18)
SET IDENTITY_INSERT [dbo].[Section] OFF
SET IDENTITY_INSERT [dbo].[SectionCourse] ON 

INSERT [dbo].[SectionCourse] ([Id], [SectionId], [CourseId], [TeacherId]) VALUES (4, 1, 4, N'3ae33e00-fb84-4a33-9f77-3c08538ea8e4')
INSERT [dbo].[SectionCourse] ([Id], [SectionId], [CourseId], [TeacherId]) VALUES (5, 2, 5, N'ddfc0b02-8026-45fe-9cf4-bd28a26bf366')
SET IDENTITY_INSERT [dbo].[SectionCourse] OFF
SET IDENTITY_INSERT [dbo].[SectionStudent] ON 

INSERT [dbo].[SectionStudent] ([Id], [SectionId], [StudentId]) VALUES (6, 1, N'c6525a88-d553-48fe-bb72-92da8adfdbd7')
INSERT [dbo].[SectionStudent] ([Id], [SectionId], [StudentId]) VALUES (7, 2, N'ae1bac60-4e33-4556-9bd3-a9f6dce1fbde')
INSERT [dbo].[SectionStudent] ([Id], [SectionId], [StudentId]) VALUES (8, 1, N'ae1bac60-4e33-4556-9bd3-a9f6dce1fbde')
SET IDENTITY_INSERT [dbo].[SectionStudent] OFF
SET IDENTITY_INSERT [dbo].[SSAssignment] ON 

INSERT [dbo].[SSAssignment] ([AssignmentId], [SSId], [ObtainedMarks], [FilePath], [Id]) VALUES (6, 6, 100, N'/Assignments/Solutions/5.cc', 5)
INSERT [dbo].[SSAssignment] ([AssignmentId], [SSId], [ObtainedMarks], [FilePath], [Id]) VALUES (7, 6, 100, N'/Assignments/Solutions/6.cpp', 6)
SET IDENTITY_INSERT [dbo].[SSAssignment] OFF
SET IDENTITY_INSERT [dbo].[SSExam] ON 

INSERT [dbo].[SSExam] ([Id], [ExamId], [ObtainedMarks], [SSId]) VALUES (5, 3, 100, 6)
INSERT [dbo].[SSExam] ([Id], [ExamId], [ObtainedMarks], [SSId]) VALUES (7, 3, 100, 8)
INSERT [dbo].[SSExam] ([Id], [ExamId], [ObtainedMarks], [SSId]) VALUES (8, 4, 100, 8)
INSERT [dbo].[SSExam] ([Id], [ExamId], [ObtainedMarks], [SSId]) VALUES (9, 4, 100, 6)
INSERT [dbo].[SSExam] ([Id], [ExamId], [ObtainedMarks], [SSId]) VALUES (10, 5, 100, 7)
INSERT [dbo].[SSExam] ([Id], [ExamId], [ObtainedMarks], [SSId]) VALUES (11, 6, 100, 7)
SET IDENTITY_INSERT [dbo].[SSExam] OFF
INSERT [dbo].[Student] ([Id], [EnrollementDate], [RegistrationNo]) VALUES (N'ae1bac60-4e33-4556-9bd3-a9f6dce1fbde', CAST(N'1998-12-22 00:00:00.000' AS DateTime), N'2016-CE-99')
INSERT [dbo].[Student] ([Id], [EnrollementDate], [RegistrationNo]) VALUES (N'c6525a88-d553-48fe-bb72-92da8adfdbd7', CAST(N'1998-12-22 00:00:00.000' AS DateTime), N'2016-CE-07')
SET IDENTITY_INSERT [dbo].[StudentAttendance] ON 

INSERT [dbo].[StudentAttendance] ([AttendanceId], [StudentId], [Status], [Id]) VALUES (3, N'ae1bac60-4e33-4556-9bd3-a9f6dce1fbde', 24, 4)
INSERT [dbo].[StudentAttendance] ([AttendanceId], [StudentId], [Status], [Id]) VALUES (3, N'c6525a88-d553-48fe-bb72-92da8adfdbd7', 24, 5)
SET IDENTITY_INSERT [dbo].[StudentAttendance] OFF
INSERT [dbo].[Teacher] ([Id], [Salary]) VALUES (N'3ae33e00-fb84-4a33-9f77-3c08538ea8e4', 200008)
INSERT [dbo].[Teacher] ([Id], [Salary]) VALUES (N'ddfc0b02-8026-45fe-9cf4-bd28a26bf366', 20000)
SET IDENTITY_INSERT [dbo].[TeacherAttendance] ON 

INSERT [dbo].[TeacherAttendance] ([AttendanceId], [TeacherId], [Status], [Id]) VALUES (3, N'ddfc0b02-8026-45fe-9cf4-bd28a26bf366', 25, 22)
INSERT [dbo].[TeacherAttendance] ([AttendanceId], [TeacherId], [Status], [Id]) VALUES (3, N'3ae33e00-fb84-4a33-9f77-3c08538ea8e4', 24, 23)
SET IDENTITY_INSERT [dbo].[TeacherAttendance] OFF
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([User_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUsers_Lookup] FOREIGN KEY([Gender])
REFERENCES [dbo].[Lookup] ([Id])
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_AspNetUsers_Lookup]
GO
ALTER TABLE [dbo].[Assignment]  WITH CHECK ADD  CONSTRAINT [FK_Assignment_Lookup] FOREIGN KEY([Status])
REFERENCES [dbo].[Lookup] ([Id])
GO
ALTER TABLE [dbo].[Assignment] CHECK CONSTRAINT [FK_Assignment_Lookup]
GO
ALTER TABLE [dbo].[Assignment]  WITH CHECK ADD  CONSTRAINT [FK_Assignment_SectionCourse] FOREIGN KEY([SCId])
REFERENCES [dbo].[SectionCourse] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Assignment] CHECK CONSTRAINT [FK_Assignment_SectionCourse]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [FK_Attendance_Lookup] FOREIGN KEY([Type])
REFERENCES [dbo].[Lookup] ([Id])
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_Attendance_Lookup]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Lookup] FOREIGN KEY([Status])
REFERENCES [dbo].[Lookup] ([Id])
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Lookup]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Lookup1] FOREIGN KEY([Class])
REFERENCES [dbo].[Lookup] ([Id])
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Lookup1]
GO
ALTER TABLE [dbo].[Exam]  WITH CHECK ADD  CONSTRAINT [FK_Exam_Lookup] FOREIGN KEY([ExamType])
REFERENCES [dbo].[Lookup] ([Id])
GO
ALTER TABLE [dbo].[Exam] CHECK CONSTRAINT [FK_Exam_Lookup]
GO
ALTER TABLE [dbo].[Exam]  WITH CHECK ADD  CONSTRAINT [FK_Exam_SectionCourse] FOREIGN KEY([SCId])
REFERENCES [dbo].[SectionCourse] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Exam] CHECK CONSTRAINT [FK_Exam_SectionCourse]
GO
ALTER TABLE [dbo].[Fees]  WITH CHECK ADD  CONSTRAINT [FK_Fees_Lookup] FOREIGN KEY([Class])
REFERENCES [dbo].[Lookup] ([Id])
GO
ALTER TABLE [dbo].[Fees] CHECK CONSTRAINT [FK_Fees_Lookup]
GO
ALTER TABLE [dbo].[FeesStudent]  WITH CHECK ADD  CONSTRAINT [FK_FeesStudent_Fees] FOREIGN KEY([FeeId])
REFERENCES [dbo].[Fees] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FeesStudent] CHECK CONSTRAINT [FK_FeesStudent_Fees]
GO
ALTER TABLE [dbo].[FeesStudent]  WITH CHECK ADD  CONSTRAINT [FK_FeesStudent_Student] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FeesStudent] CHECK CONSTRAINT [FK_FeesStudent_Student]
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD  CONSTRAINT [FK_Request_AspNetUsers] FOREIGN KEY([AspId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Request] CHECK CONSTRAINT [FK_Request_AspNetUsers]
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD  CONSTRAINT [FK_Request_AspNetUsers1] FOREIGN KEY([Acknowledged])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Request] CHECK CONSTRAINT [FK_Request_AspNetUsers1]
GO
ALTER TABLE [dbo].[SalaryPaid]  WITH CHECK ADD  CONSTRAINT [FK_SalaryPaid_Lookup] FOREIGN KEY([Status])
REFERENCES [dbo].[Lookup] ([Id])
GO
ALTER TABLE [dbo].[SalaryPaid] CHECK CONSTRAINT [FK_SalaryPaid_Lookup]
GO
ALTER TABLE [dbo].[SalaryPaid]  WITH CHECK ADD  CONSTRAINT [FK_SalaryPaid_Teacher] FOREIGN KEY([TeacherId])
REFERENCES [dbo].[Teacher] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SalaryPaid] CHECK CONSTRAINT [FK_SalaryPaid_Teacher]
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
ALTER TABLE [dbo].[Section]  WITH CHECK ADD  CONSTRAINT [FK_Section_Lookup] FOREIGN KEY([Class])
REFERENCES [dbo].[Lookup] ([Id])
GO
ALTER TABLE [dbo].[Section] CHECK CONSTRAINT [FK_Section_Lookup]
GO
ALTER TABLE [dbo].[SectionCourse]  WITH CHECK ADD  CONSTRAINT [FK_SectionCourse_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SectionCourse] CHECK CONSTRAINT [FK_SectionCourse_Course]
GO
ALTER TABLE [dbo].[SectionCourse]  WITH CHECK ADD  CONSTRAINT [FK_SectionCourse_Section] FOREIGN KEY([SectionId])
REFERENCES [dbo].[Section] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SectionCourse] CHECK CONSTRAINT [FK_SectionCourse_Section]
GO
ALTER TABLE [dbo].[SectionCourse]  WITH CHECK ADD  CONSTRAINT [FK_SectionCourse_Teacher] FOREIGN KEY([TeacherId])
REFERENCES [dbo].[Teacher] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SectionCourse] CHECK CONSTRAINT [FK_SectionCourse_Teacher]
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
ALTER TABLE [dbo].[SSExam]  WITH CHECK ADD  CONSTRAINT [FK_SSExam_Exam] FOREIGN KEY([ExamId])
REFERENCES [dbo].[Exam] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SSExam] CHECK CONSTRAINT [FK_SSExam_Exam]
GO
ALTER TABLE [dbo].[SSExam]  WITH CHECK ADD  CONSTRAINT [FK_SSExam_SectionStudent] FOREIGN KEY([SSId])
REFERENCES [dbo].[SectionStudent] ([Id])
GO
ALTER TABLE [dbo].[SSExam] CHECK CONSTRAINT [FK_SSExam_SectionStudent]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_AspNetUsers] FOREIGN KEY([Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_AspNetUsers]
GO
ALTER TABLE [dbo].[StudentAttendance]  WITH CHECK ADD  CONSTRAINT [FK_StudentAttendance_Attendance] FOREIGN KEY([AttendanceId])
REFERENCES [dbo].[Attendance] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StudentAttendance] CHECK CONSTRAINT [FK_StudentAttendance_Attendance]
GO
ALTER TABLE [dbo].[StudentAttendance]  WITH CHECK ADD  CONSTRAINT [FK_StudentAttendance_Lookup] FOREIGN KEY([Status])
REFERENCES [dbo].[Lookup] ([Id])
GO
ALTER TABLE [dbo].[StudentAttendance] CHECK CONSTRAINT [FK_StudentAttendance_Lookup]
GO
ALTER TABLE [dbo].[StudentAttendance]  WITH CHECK ADD  CONSTRAINT [FK_StudentAttendance_Student] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StudentAttendance] CHECK CONSTRAINT [FK_StudentAttendance_Student]
GO
ALTER TABLE [dbo].[Teacher]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Teacher_dbo.AspNetUsers_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Teacher] CHECK CONSTRAINT [FK_dbo.Teacher_dbo.AspNetUsers_Id]
GO
ALTER TABLE [dbo].[TeacherAttendance]  WITH CHECK ADD  CONSTRAINT [FK_TeacherAttendance_Attendance] FOREIGN KEY([AttendanceId])
REFERENCES [dbo].[Attendance] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TeacherAttendance] CHECK CONSTRAINT [FK_TeacherAttendance_Attendance]
GO
ALTER TABLE [dbo].[TeacherAttendance]  WITH CHECK ADD  CONSTRAINT [FK_TeacherAttendance_Lookup] FOREIGN KEY([Status])
REFERENCES [dbo].[Lookup] ([Id])
GO
ALTER TABLE [dbo].[TeacherAttendance] CHECK CONSTRAINT [FK_TeacherAttendance_Lookup]
GO
ALTER TABLE [dbo].[TeacherAttendance]  WITH CHECK ADD  CONSTRAINT [FK_TeacherAttendance_Teacher] FOREIGN KEY([TeacherId])
REFERENCES [dbo].[Teacher] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TeacherAttendance] CHECK CONSTRAINT [FK_TeacherAttendance_Teacher]
GO
