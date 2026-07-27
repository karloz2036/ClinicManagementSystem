using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClinicManagementSystem.Domain.Entities
{
    public class Patient
    {
        private Patient()
        { }

        public int Id { get; private set; }

        public string FirstName { get; private set; } = string.Empty;

        public string LastName { get; private set; } = string.Empty;

        public DateOnly DateOfBirth { get; private set; }

        public int GenderId { get; private set; }

        public string? PhoneNumber { get; private set; }

        public string? Email { get; private set; }

        public string? Address { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreatedAt { get; private set; }

        // Navigation properties (non-nullable to match NOT NULL FK in the database)
        public Gender Gender { get; private set; } = null!;

        public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();



        public static Patient Create(string firstName, string lastName, DateOnly dateOfBirth, int genderId, string? phoneNumber, string? email, string? address)
        {
            var normalizedFirstName = ValidateAndNormalizeName(firstName, "First name");
            var normalizedLastName = ValidateAndNormalizeName(lastName, "Last name");
            ValidateDateOfBirth(dateOfBirth);
            ValidateGenderId(genderId);

            var normalizedPhone = NormalizeOptionalText(phoneNumber);
            var normalizedEmail = NormalizeOptionalText(email);
            var normalizedAddress = NormalizeOptionalText(address);

            if (normalizedEmail != null && !IsValidEmail(normalizedEmail))
                throw new ArgumentException("Email format is invalid.");

            return new Patient
            {
                FirstName = normalizedFirstName,
                LastName = normalizedLastName,
                DateOfBirth = dateOfBirth,
                GenderId = genderId,
                PhoneNumber = normalizedPhone,
                Email = normalizedEmail,
                Address = normalizedAddress,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void Update(string firstName, string lastName, DateOnly dateOfBirth, int genderId, string? phoneNumber, string? email, string? address)
        {
            var normalizedFirstName = ValidateAndNormalizeName(firstName, "First name");
            var normalizedLasttName = ValidateAndNormalizeName(lastName, "Last name");

            ValidateDateOfBirth(dateOfBirth);
            ValidateGenderId(genderId);

            var normalizedPhone = NormalizeOptionalText(phoneNumber);
            var normalizedEmail = NormalizeOptionalText(email);
            var normalizedAddress = NormalizeOptionalText(address);

            if (normalizedEmail != null && !IsValidEmail(normalizedEmail))
                throw new ArgumentException("Email format is invalid.");

            FirstName = normalizedFirstName;
            LastName = normalizedLasttName;
            DateOfBirth = dateOfBirth;
            GenderId = genderId;
            PhoneNumber = normalizedPhone;
            Email = normalizedEmail;
            Address = normalizedAddress;
        }

        public void Activate()
        {
            this.IsActive = true;
        }
        public void Deactivate()
        {
            this.IsActive = false;
        }



        private static string? NormalizeOptionalText(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }

        private static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var trimmed = email.Trim();

            // Reject emails containing non-ASCII characters
            if (trimmed.Any(c => c > 127))
                return false;

            // Extra validation: do not allow two consecutive dots in the local part
            var atIndex = trimmed.IndexOf('@');
            if (atIndex > 0)
            {
                var localPart = trimmed.Substring(0, atIndex);
                if (localPart.Contains(".."))
                    return false;
            }

            var domainPart = trimmed.Substring(atIndex + 1);
            if (!Regex.IsMatch(domainPart, @"^[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(trimmed);
                return string.Equals(addr.Address, trimmed, StringComparison.Ordinal);
            }
            catch
            {
                return false;
            }
        }

        private static string ValidateAndNormalizeName(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{fieldName} is required.");

            var normalizedValue = value.Trim();

            if (normalizedValue.Length > 20)
                throw new ArgumentException($"{fieldName} cannot exceed 20 characters.");

            return normalizedValue;
        }

        private static void ValidateDateOfBirth(DateOnly dateOfBirth)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

            if (dateOfBirth > currentDate)
                throw new ArgumentException("Date of birth cannot be in the future.");
        }

        private static void ValidateGenderId(int genderId)
        {
            if (genderId <= 0)
                throw new ArgumentException("Gender id must be greater than zero.");
        }

    }
}
