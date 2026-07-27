-- Seed data for AppointmentStatuses table using multi-row VALUES
SET NOCOUNT ON;

INSERT INTO [dbo].[AppointmentStatus] ([Name], [IsActive])
VALUES
	('schedule', 1),
	('confirmed', 1),
	('completed', 1),
	('cancelled', 1),
	('not show', 1);
