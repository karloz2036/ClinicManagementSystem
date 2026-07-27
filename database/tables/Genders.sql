IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Genders]') AND type = N'U')
BEGIN
	CREATE TABLE [dbo].[Genders]
	(
		[Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Genders] PRIMARY KEY,
		[Name] NVARCHAR(25) NOT NULL,
		[IsActive] BIT NOT NULL CONSTRAINT [DF_Genders_IsActive] DEFAULT (1)
	);
END
