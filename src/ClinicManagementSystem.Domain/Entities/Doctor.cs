using System.Net.Mail;

namespace ClinicManagementSystem.Domain.Entities;

public class Doctor
{
    private Doctor() { }

    public int Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string ProfessionalLicense { get; private set; } = string.Empty;
    public string? PhoneNumber { get; private set; }
    public string? Email { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();
    public ICollection<DoctorSpecialty> DoctorSpecialties { get; private set; } = new List<DoctorSpecialty>();

    public static Doctor Create(string firstName, string lastName, string professionalLicense, string? phoneNumber, string? email)
    {
        var doctor = new Doctor
        {
            FirstName = ValidateRequired(firstName, "First name", 20),
            LastName = ValidateRequired(lastName, "Last name", 20),
            ProfessionalLicense = ValidateRequired(professionalLicense, "Professional license", 50),
            PhoneNumber = NormalizeOptional(phoneNumber, "Phone number", 20),
            Email = ValidateEmail(email),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        return doctor;
    }

    public void Update(string firstName, string lastName, string professionalLicense, string? phoneNumber, string? email)
    {
        var normalizedFirstName = ValidateRequired(firstName, "First name", 20);
        var normalizedLastName = ValidateRequired(lastName, "Last name", 20);
        var normalizedLicense = ValidateRequired(professionalLicense, "Professional license", 50);
        var normalizedPhone = NormalizeOptional(phoneNumber, "Phone number", 20);
        var normalizedEmail = ValidateEmail(email);

        FirstName = normalizedFirstName;
        LastName = normalizedLastName;
        ProfessionalLicense = normalizedLicense;
        PhoneNumber = normalizedPhone;
        Email = normalizedEmail;
    }

    public void ReplaceSpecialties(IEnumerable<int> specialtyIds)
    {
        var ids = specialtyIds.Distinct().ToList();
        if (ids.Count == 0) throw new ArgumentException("A doctor must have at least one specialty.");
        var currentIds = DoctorSpecialties.Select(ds => ds.SpecialtyId).ToHashSet();
        var removed = DoctorSpecialties.Where(ds => !ids.Contains(ds.SpecialtyId)).ToList();
        foreach (var item in removed)
            DoctorSpecialties.Remove(item);
        foreach (var id in ids.Where(id => !currentIds.Contains(id)))
            DoctorSpecialties.Add(DoctorSpecialty.Create(id));
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    private static string ValidateRequired(string value, string field, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException($"{field} is required.");
        var normalized = value.Trim();
        if (normalized.Length > maxLength) throw new ArgumentException($"{field} cannot exceed {maxLength} characters.");
        return normalized;
    }

    private static string? NormalizeOptional(string? value, string field, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        var normalized = value.Trim();
        if (normalized.Length > maxLength) throw new ArgumentException($"{field} cannot exceed {maxLength} characters.");
        return normalized;
    }

    private static string? ValidateEmail(string? email)
    {
        var normalized = NormalizeOptional(email, "Email", 100);
        if (normalized is null) return null;
        try
        {
            var address = new MailAddress(normalized);
            if (!string.Equals(address.Address, normalized, StringComparison.Ordinal)) throw new FormatException();
            return normalized;
        }
        catch
        {
            throw new ArgumentException("Email format is invalid.");
        }
    }
}
