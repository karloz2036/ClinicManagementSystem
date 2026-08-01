SET NOCOUNT ON;

IF NOT EXISTS (SELECT 1 FROM dbo.Genders WHERE [Name] = 'Male')
    INSERT INTO dbo.Genders ([Name], [IsActive]) VALUES ('Male', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.Genders WHERE [Name] = 'Female')
    INSERT INTO dbo.Genders ([Name], [IsActive]) VALUES ('Female', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.Genders WHERE [Name] = 'Other')
    INSERT INTO dbo.Genders ([Name], [IsActive]) VALUES ('Other', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.Genders WHERE [Name] = 'Prefer not to say')
    INSERT INTO dbo.Genders ([Name], [IsActive]) VALUES ('Prefer not to say', 1);
