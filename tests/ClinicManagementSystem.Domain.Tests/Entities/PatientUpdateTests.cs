using ClinicManagementSystem.Domain.Entities;
using System;
using Xunit;

namespace ClinicManagementSystem.Domain.Tests.Entities
{
    public class PatientUpdateTests
    {
        [Fact]
        public void Update_WithValidData_ShouldUpdateFields()
        {
            // Arrange
            var originalDob = new DateOnly(1990, 1, 1);
            var patient = Patient.Create("Carlos", "Chavez", originalDob, 1, "555-0000", "carlos@example.com", "Addr");

            var newDob = new DateOnly(1991, 2, 2);

            // Act
            patient.Update("Juan", "Perez", newDob, 2, "555-1111", "juan@example.com", "New Addr");

            // Assert
            Assert.Equal("Juan", patient.FirstName);
            Assert.Equal("Perez", patient.LastName);
            Assert.Equal(newDob, patient.DateOfBirth);
            Assert.Equal(2, patient.GenderId);
            Assert.Equal("555-1111", patient.PhoneNumber);
            Assert.Equal("juan@example.com", patient.Email);
            Assert.Equal("New Addr", patient.Address);
        }

        [Fact]
        public void Update_WithEmptyFirstName_ShouldThrowArgumentException()
        {
            // Arrange
            var patient = Patient.Create("Carlos", "Chavez", new DateOnly(1990, 1, 1), 1, null, null, null);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => patient.Update("", "Perez", new DateOnly(1991, 1, 1), 1, null, null, null));
        }

        [Fact]
        public void Update_WithTooLongLastName_ShouldThrowArgumentException()
        {
            // Arrange
            var patient = Patient.Create("Carlos", "Chavez", new DateOnly(1990, 1, 1), 1, null, null, null);
            var longLast = new string('X', 21);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => patient.Update("Carlos", longLast, new DateOnly(1991, 1, 1), 1, null, null, null));
        }

        [Fact]
        public void Update_WithFutureDateOfBirth_ShouldThrowArgumentException()
        {
            // Arrange
            var patient = Patient.Create("Carlos", "Chavez", new DateOnly(1990, 1, 1), 1, null, null, null);
            var future = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

            // Act & Assert
            Assert.Throws<ArgumentException>(() => patient.Update("Carlos", "Chavez", future, 1, null, null, null));
        }

        [Fact]
        public void Update_WithNonPositiveGenderId_ShouldThrowArgumentException()
        {
            // Arrange
            var patient = Patient.Create("Carlos", "Chavez", new DateOnly(1990, 1, 1), 1, null, null, null);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => patient.Update("Carlos", "Chavez", new DateOnly(1991, 1, 1), 0, null, null, null));
        }

        [Fact]
        public void Update_ShouldTrimNamesAndOptionalText()
        {
            // Arrange
            var patient = Patient.Create("Carlos", "Chavez", new DateOnly(1990, 1, 1), 1, null, null, null);

            // Act
            patient.Update("  Juan  ", "  Perez  ", new DateOnly(1991, 1, 1), 1, "  555-1111  ", "  juan@example.com  ", "  Addr  ");

            // Assert
            Assert.Equal("Juan", patient.FirstName);
            Assert.Equal("Perez", patient.LastName);
            Assert.Equal("555-1111", patient.PhoneNumber);
            Assert.Equal("juan@example.com", patient.Email);
            Assert.Equal("Addr", patient.Address);
        }

        [Fact]
        public void Update_ShouldNormalizeWhitespaceOnlyOptionalValuesToNull()
        {
            // Arrange
            var patient = Patient.Create("Carlos", "Chavez", new DateOnly(1990, 1, 1), 1, "555-0000", "carlos@example.com", "Addr");

            // Act
            patient.Update("Carlos", "Chavez", new DateOnly(1991, 1, 1), 1, "   ", "\t\r\n", "    ");

            // Assert
            Assert.Null(patient.PhoneNumber);
            Assert.Null(patient.Email);
            Assert.Null(patient.Address);
        }

        [Theory]
        [InlineData("AAAAAAAAAAAAAAAAAAAA", "BBBBBBBBBBBBBBBBBBBB")] // 20 chars
        public void Update_WithBoundaryNameLengths_ShouldSucceed(string firstName, string lastName)
        {
            // Arrange
            var patient = Patient.Create("Init", "User", new DateOnly(1990, 1, 1), 1, null, null, null);

            // Act
            patient.Update(firstName, lastName, new DateOnly(1991, 1, 1), 1, null, null, null);

            // Assert
            Assert.Equal(firstName, patient.FirstName);
            Assert.Equal(lastName, patient.LastName);
        }

        [Fact]
        public void Update_WithVeryLongNames_ShouldThrowArgumentException()
        {
            // Arrange
            var patient = Patient.Create("Init", "User", new DateOnly(1990, 1, 1), 1, null, null, null);
            var veryLong = new string('Z', 1000);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => patient.Update(veryLong, "Last", new DateOnly(1991, 1, 1), 1, null, null, null));
            Assert.Throws<ArgumentException>(() => patient.Update("First", veryLong, new DateOnly(1991, 1, 1), 1, null, null, null));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void Update_WithLargePositiveGenderIds_ShouldSucceed(int genderId)
        {
            // Arrange
            var patient = Patient.Create("Init", "User", new DateOnly(1990, 1, 1), 1, null, null, null);

            // Act
            patient.Update("First", "Last", new DateOnly(1991, 1, 1), genderId, null, null, null);

            // Assert
            Assert.Equal(genderId, patient.GenderId);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Update_WithExtremeNegativeGenderIds_ShouldThrowArgumentException(int genderId)
        {
            // Arrange
            var patient = Patient.Create("Init", "User", new DateOnly(1990, 1, 1), 1, null, null, null);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => patient.Update("First", "Last", new DateOnly(1991, 1, 1), genderId, null, null, null));
        }

        [Theory]
        [InlineData(1800, 1, 1)]
        [InlineData(2100, 1, 1)]
        public void Update_WithAncientOrFarFutureDates_CheckBehavior(int year, int month, int day)
        {
            // Arrange
            var patient = Patient.Create("Init", "User", new DateOnly(1990, 1, 1), 1, null, null, null);
            var date = new DateOnly(year, month, day);

            if (date > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                Assert.Throws<ArgumentException>(() => patient.Update("First", "Last", date, 1, null, null, null));
            }
            else
            {
                patient.Update("First", "Last", date, 1, null, null, null);
                Assert.Equal(date, patient.DateOfBirth);
            }
        }

        [Theory]
        [InlineData("unicode@例え.テスト")]
        [InlineData("missingatsymbol.example.com")]
        [InlineData("user@ localhost")]
        public void Update_WithInvalidEmails_ShouldThrowArgumentException(string badEmail)
        {
            // Arrange
            var patient = Patient.Create("Init", "User", new DateOnly(1990, 1, 1), 1, null, null, null);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => patient.Update("First", "Last", new DateOnly(1991, 1, 1), 1, null, badEmail, null));
        }

        [Fact]
        public void Update_WithInvalidData_ShouldThrowAndPreserveOriginalData()
        {
            // Arrange
            var originalDateOfBirth = new DateOnly(1990, 1, 1);

            var patient = Patient.Create(
                "Carlos",
                "Chavez",
                originalDateOfBirth,
                1,
                "555-0000",
                "carlos@example.com",
                "Original Address");

            var futureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

            // Act
            Assert.Throws<ArgumentException>(() =>
                patient.Update(
                    "Juan",
                    "Perez",
                    futureDate,
                    2,
                    "555-1111",
                    "juan@example.com",
                    "New Address"));

            // Assert
            Assert.Equal("Carlos", patient.FirstName);
            Assert.Equal("Chavez", patient.LastName);
            Assert.Equal(originalDateOfBirth, patient.DateOfBirth);
            Assert.Equal(1, patient.GenderId);
            Assert.Equal("555-0000", patient.PhoneNumber);
            Assert.Equal("carlos@example.com", patient.Email);
            Assert.Equal("Original Address", patient.Address);
        }

        //pruebas de una nueva rama
    }
}
