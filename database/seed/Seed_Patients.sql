-- Seed data: 10 test patients
SET NOCOUNT ON;

INSERT INTO dbo.Patients (FirstName, LastName, DateOfBirth, GenderId, PhoneNumber, Email, Address, IsActive)
VALUES
	('Carlos', 'Ramirez', '1985-03-12', 1, '555-0100', 'carlos.ramirez@example.com', '123 Main St, City', 1),
	('Ana', 'Gomez', '1990-07-22', 2, '555-0101', 'ana.gomez@example.com', '45 Oak Ave, City', 1),
	('Luis', 'Martinez', '1978-11-05', 1, '555-0102', 'luis.martinez@example.com', '78 Pine Rd, City', 1),
	('Mariana', 'Lopez', '1995-02-17', 2, '555-0103', 'mariana.lopez@example.com', '9 Elm St, City', 1),
	('Jose', 'Fernandez', '1982-09-30', 1, '555-0104', 'jose.fernandez@example.com', '200 Cedar Blvd, City', 1),
	('Sofia', 'Diaz', '2000-01-15', 2, '555-0105', 'sofia.diaz@example.com', '77 Maple Ln, City', 1),
	('Miguel', 'Torres', '1970-06-08', 1, '555-0106', 'miguel.torres@example.com', '12 Birch St, City', 1),
	('Valeria', 'Ruiz', '1988-12-03', 2, '555-0107', 'valeria.ruiz@example.com', '34 Spruce Ave, City', 1),
	('Alex', 'Santos', '1993-05-25', 3, '555-0108', 'alex.santos@example.com', '56 Willow Rd, City', 1),
	('Taylor', 'Morgan', '1999-10-10', 4, '555-0109', 'taylor.morgan@example.com', '101 River St, City', 1);
