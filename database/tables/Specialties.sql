IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Specialties]') AND type = N'U')
BEGIN
	CREATE TABLE [dbo].[Specialties]
	(
		[Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Specialties] PRIMARY KEY,
		[Name] NVARCHAR(50) NOT NULL,
		[Description] NVARCHAR(500) NULL,
		[IsActive] BIT NOT NULL CONSTRAINT [DF_Specialties_IsActive] DEFAULT (1),
		CONSTRAINT [UQ_Specialties_Name] UNIQUE ([Name])
	);
END
