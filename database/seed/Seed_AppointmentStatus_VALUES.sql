SET NOCOUNT ON;

IF NOT EXISTS (SELECT 1 FROM dbo.AppointmentStatus WHERE [Name] = 'Scheduled')
    INSERT INTO dbo.AppointmentStatus ([Name], [IsActive]) VALUES ('Scheduled', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.AppointmentStatus WHERE [Name] = 'Confirmed')
    INSERT INTO dbo.AppointmentStatus ([Name], [IsActive]) VALUES ('Confirmed', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.AppointmentStatus WHERE [Name] = 'Completed')
    INSERT INTO dbo.AppointmentStatus ([Name], [IsActive]) VALUES ('Completed', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.AppointmentStatus WHERE [Name] = 'Cancelled')
    INSERT INTO dbo.AppointmentStatus ([Name], [IsActive]) VALUES ('Cancelled', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.AppointmentStatus WHERE [Name] = 'No show')
    INSERT INTO dbo.AppointmentStatus ([Name], [IsActive]) VALUES ('No show', 1);
