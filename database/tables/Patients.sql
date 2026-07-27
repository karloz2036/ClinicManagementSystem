IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Patients]') AND type = N'U')
BEGIN
	CREATE TABLE [dbo].[Patients]
	(
		[Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Patients] PRIMARY KEY,
		[FirstName] NVARCHAR(20) NOT NULL,
		[LastName] NVARCHAR(20) NOT NULL,
		[DateOfBirth] DATE NOT NULL,
		[GenderId] INT NOT NULL,
		[PhoneNumber] NVARCHAR(20) NULL,
		[Email] NVARCHAR(100) NULL,
		[Address] NVARCHAR(250) NULL,
		[IsActive] BIT NOT NULL CONSTRAINT [DF_Patients_IsActive] DEFAULT (1),
		[CreatedAt] DATETIME2 NOT NULL CONSTRAINT [DF_Patients_CreatedAt] DEFAULT (SYSDATETIME()),
		CONSTRAINT [FK_Patients_Genders] FOREIGN KEY ([GenderId]) REFERENCES [dbo].[Genders]([Id])
	);
END
