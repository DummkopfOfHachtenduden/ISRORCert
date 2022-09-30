USE [SILKROAD_CERTIFICATION]
GO
/****** Object:  StoredProcedure [dbo].[_UpdateShardName]    Script Date: 30.09.2022 08:29:44 ******/
DROP PROCEDURE [dbo].[_UpdateShardName]
GO
/****** Object:  StoredProcedure [dbo].[_UpdateShardMaxUser]    Script Date: 30.09.2022 08:29:44 ******/
DROP PROCEDURE [dbo].[_UpdateShardMaxUser]
GO
/****** Object:  StoredProcedure [dbo].[_GetShardList]    Script Date: 30.09.2022 08:29:44 ******/
DROP PROCEDURE [dbo].[_GetShardList]
GO
/****** Object:  StoredProcedure [dbo].[_GetServerMachineList]    Script Date: 30.09.2022 08:29:44 ******/
DROP PROCEDURE [dbo].[_GetServerMachineList]
GO
/****** Object:  StoredProcedure [dbo].[_GetServerCordList]    Script Date: 30.09.2022 08:29:44 ******/
DROP PROCEDURE [dbo].[_GetServerCordList]
GO
/****** Object:  StoredProcedure [dbo].[_GetServerBodyList]    Script Date: 30.09.2022 08:29:44 ******/
DROP PROCEDURE [dbo].[_GetServerBodyList]
GO
/****** Object:  StoredProcedure [dbo].[_GetModuleList]    Script Date: 30.09.2022 08:29:44 ******/
DROP PROCEDURE [dbo].[_GetModuleList]
GO
/****** Object:  StoredProcedure [dbo].[_GetFarmList]    Script Date: 30.09.2022 08:29:44 ******/
DROP PROCEDURE [dbo].[_GetFarmList]
GO
/****** Object:  StoredProcedure [dbo].[_GetFarmContentList]    Script Date: 30.09.2022 08:29:44 ******/
DROP PROCEDURE [dbo].[_GetFarmContentList]
GO
/****** Object:  StoredProcedure [dbo].[_GetDivisionList]    Script Date: 30.09.2022 08:29:44 ******/
DROP PROCEDURE [dbo].[_GetDivisionList]
GO
/****** Object:  StoredProcedure [dbo].[_GetContentList]    Script Date: 30.09.2022 08:29:44 ******/
DROP PROCEDURE [dbo].[_GetContentList]
GO
ALTER TABLE [dbo].[_Shard] DROP CONSTRAINT [FK_Shard_Farm]
GO
ALTER TABLE [dbo].[_Shard] DROP CONSTRAINT [FK_Shard_Content]
GO
ALTER TABLE [dbo].[_ServerMachine] DROP CONSTRAINT [FK_ServerMachine_Division]
GO
ALTER TABLE [dbo].[_ServerCord] DROP CONSTRAINT [FK_ServerCoord_ServerBody1]
GO
ALTER TABLE [dbo].[_ServerCord] DROP CONSTRAINT [FK_ServerCoord_ServerBody]
GO
ALTER TABLE [dbo].[_ServerBody] DROP CONSTRAINT [FK_ServerBody_Shard]
GO
ALTER TABLE [dbo].[_ServerBody] DROP CONSTRAINT [FK_ServerBody_ServerMachine]
GO
ALTER TABLE [dbo].[_ServerBody] DROP CONSTRAINT [FK_ServerBody_ServerBody]
GO
ALTER TABLE [dbo].[_ServerBody] DROP CONSTRAINT [FK_ServerBody_Module]
GO
ALTER TABLE [dbo].[_ServerBody] DROP CONSTRAINT [FK_ServerBody_Farm]
GO
ALTER TABLE [dbo].[_ServerBody] DROP CONSTRAINT [FK_ServerBody_Division]
GO
ALTER TABLE [dbo].[_FarmContent] DROP CONSTRAINT [FK_FarmContent_Farm]
GO
ALTER TABLE [dbo].[_FarmContent] DROP CONSTRAINT [FK_FarmContent_Content]
GO
ALTER TABLE [dbo].[_Farm] DROP CONSTRAINT [FK_Farm_Division]
GO
ALTER TABLE [dbo].[_ServerBody] DROP CONSTRAINT [DF__ServerBod__Modul__145C0A3F]
GO
/****** Object:  Table [dbo].[_Shard]    Script Date: 30.09.2022 08:29:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_Shard]') AND type in (N'U'))
DROP TABLE [dbo].[_Shard]
GO
/****** Object:  Table [dbo].[_ServerMachine]    Script Date: 30.09.2022 08:29:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_ServerMachine]') AND type in (N'U'))
DROP TABLE [dbo].[_ServerMachine]
GO
/****** Object:  Table [dbo].[_ServerCord]    Script Date: 30.09.2022 08:29:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_ServerCord]') AND type in (N'U'))
DROP TABLE [dbo].[_ServerCord]
GO
/****** Object:  Table [dbo].[_ServerBody]    Script Date: 30.09.2022 08:29:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_ServerBody]') AND type in (N'U'))
DROP TABLE [dbo].[_ServerBody]
GO
/****** Object:  Table [dbo].[_Module]    Script Date: 30.09.2022 08:29:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_Module]') AND type in (N'U'))
DROP TABLE [dbo].[_Module]
GO
/****** Object:  Table [dbo].[_FarmContent]    Script Date: 30.09.2022 08:29:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_FarmContent]') AND type in (N'U'))
DROP TABLE [dbo].[_FarmContent]
GO
/****** Object:  Table [dbo].[_Farm]    Script Date: 30.09.2022 08:29:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_Farm]') AND type in (N'U'))
DROP TABLE [dbo].[_Farm]
GO
/****** Object:  Table [dbo].[_Division]    Script Date: 30.09.2022 08:29:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_Division]') AND type in (N'U'))
DROP TABLE [dbo].[_Division]
GO
/****** Object:  Table [dbo].[_Content]    Script Date: 30.09.2022 08:29:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_Content]') AND type in (N'U'))
DROP TABLE [dbo].[_Content]
GO
/****** Object:  Table [dbo].[_Content]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[_Content](
	[nID] [tinyint] NOT NULL,
	[szName] [varchar](64) NOT NULL,
 CONSTRAINT [PK__Content__3214EC2719B8A7C8] PRIMARY KEY CLUSTERED 
(
	[nID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[_Division]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[_Division](
	[nID] [tinyint] NOT NULL,
	[szName] [varchar](32) NOT NULL,
	[szDBConfig] [varchar](256) NULL,
 CONSTRAINT [PK__Division__3214EC271A898B6D] PRIMARY KEY CLUSTERED 
(
	[nID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[_Farm]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[_Farm](
	[nID] [tinyint] NOT NULL,
	[nDivisionID] [tinyint] NOT NULL,
	[szName] [varchar](32) NOT NULL,
	[szDBConfig] [varchar](256) NULL,
 CONSTRAINT [PK__Farm__3214EC278DCA4064] PRIMARY KEY CLUSTERED 
(
	[nID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[_FarmContent]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[_FarmContent](
	[nID] [int] IDENTITY(1,1) NOT NULL,
	[nFarmID] [tinyint] NOT NULL,
	[nContentID] [tinyint] NOT NULL,
 CONSTRAINT [PK__FarmContent] PRIMARY KEY CLUSTERED 
(
	[nID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[_Module]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[_Module](
	[nID] [tinyint] NOT NULL,
	[szName] [varchar](64) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[nID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[_ServerBody]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[_ServerBody](
	[nID] [smallint] NOT NULL,
	[nDivisionID] [tinyint] NULL,
	[nFarmID] [tinyint] NULL,
	[nShardID] [smallint] NULL,
	[nMachineID] [int] NOT NULL,
	[nModuleID] [tinyint] NOT NULL,
	[nModuleType] [tinyint] NOT NULL,
	[nCertifierID] [smallint] NULL,
	[nListenerPort] [smallint] NOT NULL,
 CONSTRAINT [PK__ServerBo__3214EC27C75D694C] PRIMARY KEY CLUSTERED 
(
	[nID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[_ServerCord]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[_ServerCord](
	[nID] [int] IDENTITY(1,1) NOT NULL,
	[nOutletID] [smallint] NOT NULL,
	[nInletID] [smallint] NOT NULL,
	[nBindType] [tinyint] NOT NULL,
 CONSTRAINT [PK__ServerCo__3214EC278202D17A] PRIMARY KEY CLUSTERED 
(
	[nID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[_ServerMachine]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[_ServerMachine](
	[nID] [int] NOT NULL,
	[nDivisionID] [tinyint] NULL,
	[szName] [varchar](32) NOT NULL,
	[szPublicIP] [varchar](16) NOT NULL,
	[szPrivateIP] [varchar](16) NOT NULL,
 CONSTRAINT [PK__ServerMa__3214EC276F13B687] PRIMARY KEY CLUSTERED 
(
	[nID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[_Shard]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[_Shard](
	[nID] [smallint] NOT NULL,
	[nFarmID] [tinyint] NOT NULL,
	[nContentID] [tinyint] NOT NULL,
	[szName] [varchar](32) NOT NULL,
	[szDBConfig] [varchar](256) NULL,
	[szLogDBConfig] [varchar](256) NULL,
	[nMaxUser] [smallint] NOT NULL,
 CONSTRAINT [PK__Shard__3214EC270B93CBD2] PRIMARY KEY CLUSTERED 
(
	[nID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[_Content] ([nID], [szName]) VALUES (0, N'DUMMY')
GO
INSERT [dbo].[_Content] ([nID], [szName]) VALUES (65, N'SRO_R_Global_Offical')
GO
INSERT [dbo].[_Division] ([nID], [szName], [szDBConfig]) VALUES (65, N'SRO_R_Global_Offical', N'DRIVER={SQL Server};SERVER=.\SQLExpress;UID=sa;PWD=1;DATABASE=SILKROAD_R_ACCOUNT')
GO
INSERT [dbo].[_Farm] ([nID], [nDivisionID], [szName], [szDBConfig]) VALUES (65, 65, N'SRO_R_Global_Farm', NULL)
GO
SET IDENTITY_INSERT [dbo].[_FarmContent] ON 
GO
INSERT [dbo].[_FarmContent] ([nID], [nFarmID], [nContentID]) VALUES (2, 65, 65)
GO
SET IDENTITY_INSERT [dbo].[_FarmContent] OFF
GO
INSERT [dbo].[_Module] ([nID], [szName]) VALUES (1, N'Certification')
GO
INSERT [dbo].[_Module] ([nID], [szName]) VALUES (2, N'GlobalManager')
GO
INSERT [dbo].[_Module] ([nID], [szName]) VALUES (3, N'DownloadServer')
GO
INSERT [dbo].[_Module] ([nID], [szName]) VALUES (4, N'GatewayServer')
GO
INSERT [dbo].[_Module] ([nID], [szName]) VALUES (5, N'FarmManager')
GO
INSERT [dbo].[_Module] ([nID], [szName]) VALUES (6, N'AgentServer')
GO
INSERT [dbo].[_Module] ([nID], [szName]) VALUES (7, N'SR_ShardManager')
GO
INSERT [dbo].[_Module] ([nID], [szName]) VALUES (8, N'SR_GameServer')
GO
INSERT [dbo].[_Module] ([nID], [szName]) VALUES (9, N'SR_Client')
GO
INSERT [dbo].[_Module] ([nID], [szName]) VALUES (10, N'ServiceManager')
GO
INSERT [dbo].[_Module] ([nID], [szName]) VALUES (11, N'MachineManager')
GO
INSERT [dbo].[_Module] ([nID], [szName]) VALUES (12, N'JmxMsgSvr')
GO
INSERT [dbo].[_Module] ([nID], [szName]) VALUES (13, N'JmxMessenger')
GO
INSERT [dbo].[_Module] ([nID], [szName]) VALUES (14, N'SMC')
GO
INSERT [dbo].[_ServerBody] ([nID], [nDivisionID], [nFarmID], [nShardID], [nMachineID], [nModuleID], [nModuleType], [nCertifierID], [nListenerPort]) VALUES (1, NULL, NULL, NULL, 1, 1, 0, NULL, 32000)
GO
INSERT [dbo].[_ServerBody] ([nID], [nDivisionID], [nFarmID], [nShardID], [nMachineID], [nModuleID], [nModuleType], [nCertifierID], [nListenerPort]) VALUES (2, NULL, NULL, NULL, 1, 11, 0, 1, 32001)
GO
INSERT [dbo].[_ServerBody] ([nID], [nDivisionID], [nFarmID], [nShardID], [nMachineID], [nModuleID], [nModuleType], [nCertifierID], [nListenerPort]) VALUES (3, 65, NULL, NULL, 2, 2, 0, 1, 15880)
GO
INSERT [dbo].[_ServerBody] ([nID], [nDivisionID], [nFarmID], [nShardID], [nMachineID], [nModuleID], [nModuleType], [nCertifierID], [nListenerPort]) VALUES (4, 65, NULL, NULL, 2, 11, 0, 3, 32002)
GO
INSERT [dbo].[_ServerBody] ([nID], [nDivisionID], [nFarmID], [nShardID], [nMachineID], [nModuleID], [nModuleType], [nCertifierID], [nListenerPort]) VALUES (5, 65, NULL, NULL, 2, 3, 0, 3, 15881)
GO
INSERT [dbo].[_ServerBody] ([nID], [nDivisionID], [nFarmID], [nShardID], [nMachineID], [nModuleID], [nModuleType], [nCertifierID], [nListenerPort]) VALUES (6, 65, NULL, NULL, 2, 4, 0, 3, 15779)
GO
INSERT [dbo].[_ServerBody] ([nID], [nDivisionID], [nFarmID], [nShardID], [nMachineID], [nModuleID], [nModuleType], [nCertifierID], [nListenerPort]) VALUES (7, 65, 65, NULL, 2, 5, 0, 3, 15882)
GO
INSERT [dbo].[_ServerBody] ([nID], [nDivisionID], [nFarmID], [nShardID], [nMachineID], [nModuleID], [nModuleType], [nCertifierID], [nListenerPort]) VALUES (8, 65, 65, 323, 2, 6, 0, 7, 15884)
GO
INSERT [dbo].[_ServerBody] ([nID], [nDivisionID], [nFarmID], [nShardID], [nMachineID], [nModuleID], [nModuleType], [nCertifierID], [nListenerPort]) VALUES (9, 65, 65, 323, 2, 7, 0, 7, 15883)
GO
INSERT [dbo].[_ServerBody] ([nID], [nDivisionID], [nFarmID], [nShardID], [nMachineID], [nModuleID], [nModuleType], [nCertifierID], [nListenerPort]) VALUES (10, 65, 65, 323, 2, 8, 0, 7, 15886)
GO
SET IDENTITY_INSERT [dbo].[_ServerCord] ON 
GO
INSERT [dbo].[_ServerCord] ([nID], [nOutletID], [nInletID], [nBindType]) VALUES (1, 2, 1, 0)
GO
INSERT [dbo].[_ServerCord] ([nID], [nOutletID], [nInletID], [nBindType]) VALUES (2, 3, 1, 0)
GO
INSERT [dbo].[_ServerCord] ([nID], [nOutletID], [nInletID], [nBindType]) VALUES (3, 4, 3, 0)
GO
INSERT [dbo].[_ServerCord] ([nID], [nOutletID], [nInletID], [nBindType]) VALUES (4, 5, 3, 0)
GO
INSERT [dbo].[_ServerCord] ([nID], [nOutletID], [nInletID], [nBindType]) VALUES (5, 6, 3, 0)
GO
INSERT [dbo].[_ServerCord] ([nID], [nOutletID], [nInletID], [nBindType]) VALUES (6, 7, 3, 0)
GO
INSERT [dbo].[_ServerCord] ([nID], [nOutletID], [nInletID], [nBindType]) VALUES (7, 8, 7, 0)
GO
INSERT [dbo].[_ServerCord] ([nID], [nOutletID], [nInletID], [nBindType]) VALUES (8, 9, 7, 0)
GO
INSERT [dbo].[_ServerCord] ([nID], [nOutletID], [nInletID], [nBindType]) VALUES (9, 10, 7, 0)
GO
INSERT [dbo].[_ServerCord] ([nID], [nOutletID], [nInletID], [nBindType]) VALUES (10, 8, 9, 0)
GO
INSERT [dbo].[_ServerCord] ([nID], [nOutletID], [nInletID], [nBindType]) VALUES (11, 10, 9, 0)
GO
INSERT [dbo].[_ServerCord] ([nID], [nOutletID], [nInletID], [nBindType]) VALUES (12, 8, 10, 0)
GO
SET IDENTITY_INSERT [dbo].[_ServerCord] OFF
GO
INSERT [dbo].[_ServerMachine] ([nID], [nDivisionID], [szName], [szPublicIP], [szPrivateIP]) VALUES (1, NULL, N'Certification Manager', N'127.0.0.1', N'127.0.0.1')
GO
INSERT [dbo].[_ServerMachine] ([nID], [nDivisionID], [szName], [szPublicIP], [szPrivateIP]) VALUES (2, 65, N'Machine 1', N'10.0.0.2', N'10.0.0.2')
GO
INSERT [dbo].[_Shard] ([nID], [nFarmID], [nContentID], [szName], [szDBConfig], [szLogDBConfig], [nMaxUser]) VALUES (323, 65, 65, N'3Test', N'DRIVER={SQL Server};SERVER=.\SQLExpress;UID=sa;PWD=1;DATABASE=SILKROAD_R_SHARD', N'DRIVER={SQL Server};SERVER=.\SQLExpress;UID=sa;PWD=1;DATABASE=SILKROAD_R_LOG', 1000)
GO
ALTER TABLE [dbo].[_ServerBody] ADD  CONSTRAINT [DF__ServerBod__Modul__145C0A3F]  DEFAULT ((0)) FOR [nModuleType]
GO
ALTER TABLE [dbo].[_Farm]  WITH CHECK ADD  CONSTRAINT [FK_Farm_Division] FOREIGN KEY([nDivisionID])
REFERENCES [dbo].[_Division] ([nID])
GO
ALTER TABLE [dbo].[_Farm] CHECK CONSTRAINT [FK_Farm_Division]
GO
ALTER TABLE [dbo].[_FarmContent]  WITH CHECK ADD  CONSTRAINT [FK_FarmContent_Content] FOREIGN KEY([nContentID])
REFERENCES [dbo].[_Content] ([nID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[_FarmContent] CHECK CONSTRAINT [FK_FarmContent_Content]
GO
ALTER TABLE [dbo].[_FarmContent]  WITH CHECK ADD  CONSTRAINT [FK_FarmContent_Farm] FOREIGN KEY([nFarmID])
REFERENCES [dbo].[_Farm] ([nID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[_FarmContent] CHECK CONSTRAINT [FK_FarmContent_Farm]
GO
ALTER TABLE [dbo].[_ServerBody]  WITH CHECK ADD  CONSTRAINT [FK_ServerBody_Division] FOREIGN KEY([nDivisionID])
REFERENCES [dbo].[_Division] ([nID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[_ServerBody] CHECK CONSTRAINT [FK_ServerBody_Division]
GO
ALTER TABLE [dbo].[_ServerBody]  WITH CHECK ADD  CONSTRAINT [FK_ServerBody_Farm] FOREIGN KEY([nFarmID])
REFERENCES [dbo].[_Farm] ([nID])
GO
ALTER TABLE [dbo].[_ServerBody] CHECK CONSTRAINT [FK_ServerBody_Farm]
GO
ALTER TABLE [dbo].[_ServerBody]  WITH CHECK ADD  CONSTRAINT [FK_ServerBody_Module] FOREIGN KEY([nModuleID])
REFERENCES [dbo].[_Module] ([nID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[_ServerBody] CHECK CONSTRAINT [FK_ServerBody_Module]
GO
ALTER TABLE [dbo].[_ServerBody]  WITH CHECK ADD  CONSTRAINT [FK_ServerBody_ServerBody] FOREIGN KEY([nCertifierID])
REFERENCES [dbo].[_ServerBody] ([nID])
GO
ALTER TABLE [dbo].[_ServerBody] CHECK CONSTRAINT [FK_ServerBody_ServerBody]
GO
ALTER TABLE [dbo].[_ServerBody]  WITH CHECK ADD  CONSTRAINT [FK_ServerBody_ServerMachine] FOREIGN KEY([nMachineID])
REFERENCES [dbo].[_ServerMachine] ([nID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[_ServerBody] CHECK CONSTRAINT [FK_ServerBody_ServerMachine]
GO
ALTER TABLE [dbo].[_ServerBody]  WITH CHECK ADD  CONSTRAINT [FK_ServerBody_Shard] FOREIGN KEY([nShardID])
REFERENCES [dbo].[_Shard] ([nID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[_ServerBody] CHECK CONSTRAINT [FK_ServerBody_Shard]
GO
ALTER TABLE [dbo].[_ServerCord]  WITH CHECK ADD  CONSTRAINT [FK_ServerCoord_ServerBody] FOREIGN KEY([nOutletID])
REFERENCES [dbo].[_ServerBody] ([nID])
GO
ALTER TABLE [dbo].[_ServerCord] CHECK CONSTRAINT [FK_ServerCoord_ServerBody]
GO
ALTER TABLE [dbo].[_ServerCord]  WITH CHECK ADD  CONSTRAINT [FK_ServerCoord_ServerBody1] FOREIGN KEY([nInletID])
REFERENCES [dbo].[_ServerBody] ([nID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[_ServerCord] CHECK CONSTRAINT [FK_ServerCoord_ServerBody1]
GO
ALTER TABLE [dbo].[_ServerMachine]  WITH CHECK ADD  CONSTRAINT [FK_ServerMachine_Division] FOREIGN KEY([nDivisionID])
REFERENCES [dbo].[_Division] ([nID])
GO
ALTER TABLE [dbo].[_ServerMachine] CHECK CONSTRAINT [FK_ServerMachine_Division]
GO
ALTER TABLE [dbo].[_Shard]  WITH CHECK ADD  CONSTRAINT [FK_Shard_Content] FOREIGN KEY([nContentID])
REFERENCES [dbo].[_Content] ([nID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[_Shard] CHECK CONSTRAINT [FK_Shard_Content]
GO
ALTER TABLE [dbo].[_Shard]  WITH CHECK ADD  CONSTRAINT [FK_Shard_Farm] FOREIGN KEY([nFarmID])
REFERENCES [dbo].[_Farm] ([nID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[_Shard] CHECK CONSTRAINT [FK_Shard_Farm]
GO
/****** Object:  StoredProcedure [dbo].[_GetContentList]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[_GetContentList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT nID, szName FROM _Content WHERE nID > 0
END
GO
/****** Object:  StoredProcedure [dbo].[_GetDivisionList]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[_GetDivisionList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT nID, szName, szDbConfig FROM _Division WHERE nID > 0
END
GO
/****** Object:  StoredProcedure [dbo].[_GetFarmContentList]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[_GetFarmContentList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT nID, nFarmID, nContentID FROM _FarmContent WHERE nID > 0
END
GO
/****** Object:  StoredProcedure [dbo].[_GetFarmList]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[_GetFarmList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT nID, nDivisionID, szName, szDbConfig FROM _Farm WHERE nID > 0
END
GO
/****** Object:  StoredProcedure [dbo].[_GetModuleList]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[_GetModuleList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT nID, szName FROM _Module WHERE nID > 0
END
GO
/****** Object:  StoredProcedure [dbo].[_GetServerBodyList]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[_GetServerBodyList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT nID, nDivisionID, nFarmID, nShardID, nMachineID, nModuleID, nModuleType, nCertifierID, nListenerPort FROM _ServerBody WITH (NOLOCK) WHERE nID > 0
END
GO
/****** Object:  StoredProcedure [dbo].[_GetServerCordList]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[_GetServerCordList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT nID, nOutletID, nInletID, nBindType FROM _ServerCord WHERE nID > 0
END
GO
/****** Object:  StoredProcedure [dbo].[_GetServerMachineList]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[_GetServerMachineList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT nID, nDivisionID, szName, szPublicIP, szPrivateIP FROM _ServerMachine WHERE nID > 0
END
GO
/****** Object:  StoredProcedure [dbo].[_GetShardList]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[_GetShardList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT nID, nFarmID, nContentID, szName, szDBConfig, szLogDBConfig, nMaxUser FROM _Shard WHERE nID > 0
END
GO
/****** Object:  StoredProcedure [dbo].[_UpdateShardMaxUser]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[_UpdateShardMaxUser]
	-- Add the parameters for the stored procedure here
	@nID int,
	@nMaxUser smallint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE _Shard SET nMaxUser = @nMaxUser  WHERE nID = @nID
END
GO
/****** Object:  StoredProcedure [dbo].[_UpdateShardName]    Script Date: 30.09.2022 08:29:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[_UpdateShardName]
	-- Add the parameters for the stored procedure here
	@nID int,
	@szName varchar(32)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE _Shard SET szName = @szName  WHERE nID = @nID
END
GO
