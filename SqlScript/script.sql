USE [Test]
GO
/****** Object:  Table [dbo].[AvailabilityTime]    Script Date: 7/23/2019 11:00:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AvailabilityTime](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
	[Rrule] [nvarchar](1000) NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_AvailabilityTime] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[AvailabilityTime] ON 

INSERT [dbo].[AvailabilityTime] ([Id], [StartDate], [EndDate], [StartTime], [EndTime], [Rrule], [CreateUser], [CreateTime]) VALUES (2, CAST(N'2019-01-01' AS Date), CAST(N'2019-12-31' AS Date), CAST(N'10:00:00' AS Time), CAST(N'13:00:00' AS Time), N'FREQ=WEEKLY;INTERVAL=1;WKST=MO;BYDAY=MO,TU,TH', N'Died', CAST(N'2019-01-01 00:00:00.000' AS DateTime))
INSERT [dbo].[AvailabilityTime] ([Id], [StartDate], [EndDate], [StartTime], [EndTime], [Rrule], [CreateUser], [CreateTime]) VALUES (3, CAST(N'2019-01-01' AS Date), CAST(N'2019-12-31' AS Date), CAST(N'12:00:00' AS Time), CAST(N'16:00:00' AS Time), N'FREQ=WEEKLY;INTERVAL=1;WKST=MO;BYDAY=FR
', N'Died', CAST(N'2019-01-01 00:00:00.000' AS DateTime))
INSERT [dbo].[AvailabilityTime] ([Id], [StartDate], [EndDate], [StartTime], [EndTime], [Rrule], [CreateUser], [CreateTime]) VALUES (4, CAST(N'2019-01-01' AS Date), CAST(N'2019-12-31' AS Date), CAST(N'14:00:00' AS Time), CAST(N'17:00:00' AS Time), N'FREQ=WEEKLY;INTERVAL=1;WKST=MO;BYDAY=TH', N'Died', CAST(N'2019-01-01 00:00:00.000' AS DateTime))
INSERT [dbo].[AvailabilityTime] ([Id], [StartDate], [EndDate], [StartTime], [EndTime], [Rrule], [CreateUser], [CreateTime]) VALUES (5, CAST(N'2019-07-01' AS Date), CAST(N'2019-07-31' AS Date), CAST(N'10:00:00' AS Time), CAST(N'19:00:00' AS Time), N'FREQ=WEEKLY;INTERVAL=1;WKST=MO;BYDAY=MO,TU
', N'Died', CAST(N'2019-01-01 00:00:00.000' AS DateTime))
INSERT [dbo].[AvailabilityTime] ([Id], [StartDate], [EndDate], [StartTime], [EndTime], [Rrule], [CreateUser], [CreateTime]) VALUES (6, CAST(N'2019-07-18' AS Date), CAST(N'2019-07-18' AS Date), CAST(N'15:00:00' AS Time), CAST(N'19:00:00' AS Time), N'FREQ=WEEKLY;INTERVAL=1;WKST=MO;BYDAY=TH
', N'Died', CAST(N'2019-01-01 00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[AvailabilityTime] OFF
