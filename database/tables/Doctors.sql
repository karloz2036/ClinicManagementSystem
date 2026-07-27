IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Doctors]') AND type = N'U')
BEGIN
	CREATE TABLE [dbo].[Doctors]
	(
		[Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Doctors] PRIMARY KEY,
		[FirstName] NVARCHAR(20) NOT NULL,
		[LastName] NVARCHAR(20) NOT NULL,
		[ProfessionalLicense] NVARCHAR(50) NOT NULL,
		[PhoneNumber] NVARCHAR(20) NULL,
		[Email] NVARCHAR(100) NULL,
		[IsActive] BIT NOT NULL CONSTRAINT [DF_Doctors_IsActive] DEFAULT (1),
		[CreatedAt] DATETIME2 NOT NULL CONSTRAINT [DF_Doctors_CreatedAt] DEFAULT (SYSDATETIME())
	);
END
