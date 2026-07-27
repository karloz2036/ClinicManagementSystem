IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DoctorSpecialties]') AND type = N'U')
BEGIN
	CREATE TABLE [dbo].[DoctorSpecialties]
	(
		[DoctorId] INT NOT NULL,
		[SpecialtyId] INT NOT NULL,
		CONSTRAINT [PK_DoctorSpecialties] PRIMARY KEY ([DoctorId], [SpecialtyId]),
		CONSTRAINT [FK_DoctorSpecialties_Doctors] FOREIGN KEY ([DoctorId]) REFERENCES [dbo].[Doctors]([Id]),
		CONSTRAINT [FK_DoctorSpecialties_Specialties] FOREIGN KEY ([SpecialtyId]) REFERENCES [dbo].[Specialties]([Id])
	);
END
