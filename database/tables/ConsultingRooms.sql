IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConsultingRooms]') AND type = N'U')
BEGIN
	CREATE TABLE [dbo].[ConsultingRooms]
	(
		[Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_ConsultingRooms] PRIMARY KEY,
		[Name] NVARCHAR(50) NOT NULL,
		[Location] NVARCHAR(150) NULL,
		[IsActive] BIT NOT NULL CONSTRAINT [DF_ConsultingRooms_IsActive] DEFAULT (1),
		CONSTRAINT [UQ_ConsultingRooms_Name] UNIQUE ([Name])
	);
END
