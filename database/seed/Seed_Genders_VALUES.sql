-- Seed data for Genders table using multi-row VALUES
SET NOCOUNT ON;

INSERT INTO [dbo].[Genders] ([Name], [IsActive])
VALUES
	('Male', 1),
	('Female', 1),
	('other', 1),
	('prefer not to say', 1);
