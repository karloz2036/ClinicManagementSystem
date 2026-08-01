SET NOCOUNT ON;

IF NOT EXISTS (SELECT 1 FROM dbo.Specialties WHERE [Name] = 'General Medicine')
    INSERT INTO dbo.Specialties ([Name], [Description], [IsActive]) VALUES ('General Medicine', 'Primary medical care', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.Specialties WHERE [Name] = 'Cardiology')
    INSERT INTO dbo.Specialties ([Name], [Description], [IsActive]) VALUES ('Cardiology', 'Heart and circulatory system', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.Specialties WHERE [Name] = 'Pediatrics')
    INSERT INTO dbo.Specialties ([Name], [Description], [IsActive]) VALUES ('Pediatrics', 'Medical care for children', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.ConsultingRooms WHERE [Name] = 'Room 1')
    INSERT INTO dbo.ConsultingRooms ([Name], [Location], [IsActive]) VALUES ('Room 1', 'First floor', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.ConsultingRooms WHERE [Name] = 'Room 2')
    INSERT INTO dbo.ConsultingRooms ([Name], [Location], [IsActive]) VALUES ('Room 2', 'First floor', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.Doctors WHERE [ProfessionalLicense] = 'LIC-1001')
    INSERT INTO dbo.Doctors ([FirstName], [LastName], [ProfessionalLicense], [PhoneNumber], [Email], [IsActive])
    VALUES ('Laura', 'Mendoza', 'LIC-1001', '555-2001', 'laura.mendoza@example.com', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.Doctors WHERE [ProfessionalLicense] = 'LIC-1002')
    INSERT INTO dbo.Doctors ([FirstName], [LastName], [ProfessionalLicense], [PhoneNumber], [Email], [IsActive])
    VALUES ('Jorge', 'Castillo', 'LIC-1002', '555-2002', 'jorge.castillo@example.com', 1);

DECLARE @Doctor1 INT = (SELECT Id FROM dbo.Doctors WHERE ProfessionalLicense = 'LIC-1001');
DECLARE @Doctor2 INT = (SELECT Id FROM dbo.Doctors WHERE ProfessionalLicense = 'LIC-1002');
DECLARE @GeneralMedicine INT = (SELECT Id FROM dbo.Specialties WHERE [Name] = 'General Medicine');
DECLARE @Cardiology INT = (SELECT Id FROM dbo.Specialties WHERE [Name] = 'Cardiology');

IF NOT EXISTS (SELECT 1 FROM dbo.DoctorSpecialties WHERE DoctorId = @Doctor1 AND SpecialtyId = @Cardiology)
    INSERT INTO dbo.DoctorSpecialties (DoctorId, SpecialtyId) VALUES (@Doctor1, @Cardiology);
IF NOT EXISTS (SELECT 1 FROM dbo.DoctorSpecialties WHERE DoctorId = @Doctor2 AND SpecialtyId = @GeneralMedicine)
    INSERT INTO dbo.DoctorSpecialties (DoctorId, SpecialtyId) VALUES (@Doctor2, @GeneralMedicine);
