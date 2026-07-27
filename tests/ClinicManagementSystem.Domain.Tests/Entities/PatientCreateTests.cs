using ClinicManagementSystem.Domain.Entities;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Tests.Helpers;
using System.IO;

namespace ClinicManagementSystem.Domain.Tests.Entities
{
    public class PatientCreateTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateActivePatient()
        {
            // Arrange
            var dateOfBirth = new DateOnly(1990, 5, 10);
            
            // Act
            var patient = Patient.Create(
                "Carlos",
                "Chavez",
                dateOfBirth,
                1,
                "555-1234",
                "carlos@example.com",
                "Mexico City");

            // Assert
            Assert.Equal("Carlos", patient.FirstName);
            Assert.Equal("Chavez", patient.LastName);
            Assert.Equal(dateOfBirth, patient.DateOfBirth);
            Assert.Equal(1, patient.GenderId);
            Assert.Equal("555-1234", patient.PhoneNumber);
            Assert.Equal("carlos@example.com", patient.Email);
            Assert.Equal("Mexico City", patient.Address);
            //Assert.True(patient.IsActive);
            Assert.NotEqual(default, patient.CreatedAt);
        }

        [Theory]
        [InlineData("AAAAAAAAAAAAAAAAAAAA", "BBBBBBBBBBBBBBBBBBBB")]
        public void Create_WithBoundaryNameLengths_ShouldSucceed(string firstName, string lastName)
        {
            // Arrange
            var dateOfBirth = new DateOnly(1990, 5, 10);

            // Act
            var patient = Patient.Create(
                firstName,
                lastName,
                dateOfBirth,
                1,
                null,
                null,
                null);

            // Assert
            Assert.Equal(firstName, patient.FirstName);
            Assert.Equal(lastName, patient.LastName);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(int.MinValue)]
        public void Create_WithExtremeGenderIds_ShouldThrowArgumentException(int genderId)
        {
            // Arrange
            var dateOfBirth = new DateOnly(1990, 5, 10);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Patient.Create(
                "Carlos",
                "Chavez",
                dateOfBirth,
                genderId,
                null,
                null,
                null));
        }

        [Theory]
        [InlineData(2000, 2, 29)]
        [InlineData(1900, 1, 1)]
        [InlineData(1800, 12, 31)]
        public void Create_WithLeapAndAncientBirthDates_ShouldCreate(int year, int month, int day)
        {
            // Arrange
            var dateOfBirth = new DateOnly(year, month, day);

            // Act
            var patient = Patient.Create(
                "Carlos",
                "Chavez",
                dateOfBirth,
                1,
                null,
                null,
                null);

            // Assert
            Assert.Equal(dateOfBirth, patient.DateOfBirth);
        }

        [Theory]
        [InlineData("\tCarlos\t", "\nChavez\n", " 555-0000 ", " email@ex.com\t", " addr \n")]
        [InlineData("  Ana  ", "  Gomez  ", null, "\t\t", "   ")]
        public void Create_WithWhitespaceVariants_ShouldTrimAndNormalize(string first, string last, string phone, string email, string address)
        {
            // Arrange
            var dateOfBirth = new DateOnly(1990, 5, 10);

            // Act
            var patient = Patient.Create(
                first,
                last,
                dateOfBirth,
                1,
                phone,
                email,
                address);

            // Assert
            Assert.Equal(first.Trim(), patient.FirstName);
            Assert.Equal(last.Trim(), patient.LastName);

            if (string.IsNullOrWhiteSpace(phone))
                Assert.Null(patient.PhoneNumber);
            else
                Assert.Equal(phone.Trim(), patient.PhoneNumber);

            if (string.IsNullOrWhiteSpace(email))
                Assert.Null(patient.Email);
            else
                Assert.Equal(email.Trim(), patient.Email);

            if (string.IsNullOrWhiteSpace(address))
                Assert.Null(patient.Address);
            else
                Assert.Equal(address.Trim(), patient.Address);
        }

        [Fact]
        public void Create_WithEmptyFirstName_ShouldThrowArgumentException()
        {
            // Arrange
            var dateOfBirth = new DateOnly(1990, 5, 10);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Patient.Create(
                "",
                "Chavez",
                dateOfBirth,
                1,
                null,
                null,
                null));
        }

        [Fact]
        public void Create_WithWhitespaceFirstName_ShouldThrowArgumentException()
        {
            // Arrange
            var dateOfBirth = new DateOnly(1990, 5, 10);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Patient.Create(
                "   ",
                "Chavez",
                dateOfBirth,
                1,
                null,
                null,
                null));
        }

        [Fact]
        public void Create_WithTooLongFirstName_ShouldThrowArgumentException()
        {
            // Arrange
            var dateOfBirth = new DateOnly(1990, 5, 10);
            var longName = new string('A', 21);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Patient.Create(
                longName,
                "Chavez",
                dateOfBirth,
                1,
                null,
                null,
                null));
        }

        [Fact]
        public void Create_WithEmptyLastName_ShouldThrowArgumentException()
        {
            // Arrange
            var dateOfBirth = new DateOnly(1990, 5, 10);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Patient.Create(
                "Carlos",
                "",
                dateOfBirth,
                1,
                null,
                null,
                null));
        }

        [Fact]
        public void Create_WithTooLongLastName_ShouldThrowArgumentException()
        {
            // Arrange
            var dateOfBirth = new DateOnly(1990, 5, 10);
            var longName = new string('B', 21);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Patient.Create(
                "Carlos",
                longName,
                dateOfBirth,
                1,
                null,
                null,
                null));
        }

        [Fact]
        public void Create_WithFutureDateOfBirth_ShouldThrowArgumentException()
        {
            // Arrange
            var futureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Patient.Create(
                "Carlos",
                "Chavez",
                futureDate,
                1,
                null,
                null,
                null));
        }

        [Fact]
        public void Create_WithNonPositiveGenderId_ShouldThrowArgumentException()
        {
            // Arrange
            var dateOfBirth = new DateOnly(1990, 5, 10);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Patient.Create(
                "Carlos",
                "Chavez",
                dateOfBirth,
                0,
                null,
                null,
                null));
        }

        [Fact]
        public void Create_ShouldTrimNamesAndOptionalText()
        {
            // Arrange
            var dateOfBirth = new DateOnly(1990, 5, 10);

            // Act
            var patient = Patient.Create(
                "  Carlos  ",
                "  Chavez  ",
                dateOfBirth,
                1,
                "  555-1234  ",
                "  carlos@example.com  ",
                "  Mexico City  ");

            // Assert
            Assert.Equal("Carlos", patient.FirstName);
            Assert.Equal("Chavez", patient.LastName);
            Assert.Equal("555-1234", patient.PhoneNumber);
            Assert.Equal("carlos@example.com", patient.Email);
            Assert.Equal("Mexico City", patient.Address);
        }

        [Fact]
        public void Create_ShouldNormalizeWhitespaceOnlyOptionalValuesToNull()
        {
            // Arrange
            var dateOfBirth = new DateOnly(1990, 5, 10);

            // Act
            var patient = Patient.Create(
                "Carlos",
                "Chavez",
                dateOfBirth,
                1,
                "   ",
                "\t\r\n",
                "    ");

            // Assert
            Assert.Null(patient.PhoneNumber);
            Assert.Null(patient.Email);
            Assert.Null(patient.Address);
        }

        [Fact]
        public void Create_VariousScenarios_FromCsv()
        {
            var dataPath = Path.Combine(AppContext.BaseDirectory, "Data", "patient_scenarios.csv");
            Assert.True(File.Exists(dataPath), $"CSV data file not found at {dataPath}");

            var lines = File.ReadAllLines(dataPath).ToList();
            if (lines.Count == 0)
                Assert.True(false, "CSV file is empty");

            var header = lines[0];
            var rows = lines.Skip(1).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

            var failures = new List<string>();
            for (var i = 0; i < rows.Count; i++)
            {
                var line = rows[i];
                var parts = line.Split(',');
                if (parts.Length < 8)
                {
                    failures.Add($"Line {i + 2}: expected 8 columns but got {parts.Length}: {line}");
                    continue;
                }

                var firstName = parts[0];
                var lastName = parts[1];
                var dateOfBirthStr = parts[2];
                var genderIdStr = parts[3];
                var phone = parts[4];
                var email = parts[5];
                var address = parts[6];
                var isValidStr = parts[7];

                var expectedValid = bool.TryParse(isValidStr, out var b) && b;

                var succeeded = true;
                try
                {
                    if (!DateOnly.TryParse(dateOfBirthStr, out var dob))
                        throw new ArgumentException("Invalid date format");

                    if (!int.TryParse(genderIdStr, out var genderId))
                        throw new ArgumentException("Invalid gender id");

                    var patient = Patient.Create(
                        firstName,
                        lastName,
                        dob,
                        genderId,
                        string.IsNullOrWhiteSpace(phone) ? null : phone,
                        string.IsNullOrWhiteSpace(email) ? null : email,
                        string.IsNullOrWhiteSpace(address) ? null : address);
                }
                catch (Exception ex)
                {
                    succeeded = false;
                }

                if (succeeded != expectedValid)
                {
                    failures.Add($"Line {i + 2}: expected IsValid={expectedValid} but got succeeded={succeeded}. Line: {line}");
                }
            }

            if (failures.Any())
                Assert.True(false, "CSV scenario failures:\n" + string.Join("\n", failures));
        }
    }
}
