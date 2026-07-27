IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppointmentStatuses]') AND type = N'U')
BEGIN
	CREATE TABLE [dbo].[AppointmentStatus]
	(
		[Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_AppointmentStatuses] PRIMARY KEY,
		[Name] NVARCHAR(20) NOT NULL,
		[IsActive] BIT NOT NULL CONSTRAINT [DF_AppointmentStatuses_IsActive] DEFAULT (1)
	);
END
