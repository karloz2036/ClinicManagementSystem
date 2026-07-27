IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Appointments]') AND type = N'U')
BEGIN
	CREATE TABLE [dbo].[Appointments]
	(
		[Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Appointments] PRIMARY KEY,
		[PatientId] INT NOT NULL,
		[DoctorId] INT NOT NULL,
		[ConsultingRoomId] INT NOT NULL,
		[AppointmentStatusId] INT NOT NULL,
		[StartDateTime] DATETIME2 NOT NULL,
		[EndDateTime] DATETIME2 NOT NULL,
		[Notes] NVARCHAR(MAX) NULL,
		[CreatedAt] DATETIME2 NOT NULL CONSTRAINT [DF_Appointments_CreatedAt] DEFAULT (SYSDATETIME()),
		CONSTRAINT [FK_Appointments_Patients] FOREIGN KEY ([PatientId]) REFERENCES [dbo].[Patients]([Id]),
		CONSTRAINT [FK_Appointments_Doctors] FOREIGN KEY ([DoctorId]) REFERENCES [dbo].[Doctors]([Id]),
		CONSTRAINT [FK_Appointments_ConsultingRooms] FOREIGN KEY ([ConsultingRoomId]) REFERENCES [dbo].[ConsultingRooms]([Id]),
		CONSTRAINT [FK_Appointments_AppointmentStatuses] FOREIGN KEY ([AppointmentStatusId]) REFERENCES [dbo].[AppointmentStatus]([Id])
	);
END
