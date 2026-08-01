namespace ClinicManagementSystem.Domain.Entities;

public class Specialty
{
    private Specialty() { }

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public ICollection<DoctorSpecialty> DoctorSpecialties { get; private set; } = new List<DoctorSpecialty>();

    public static Specialty Create(string name, string? description)
    {
        return new Specialty
        {
            Name = ValidateName(name),
            Description = ValidateDescription(description),
            IsActive = true
        };
    }

    public void Update(string name, string? description)
    {
        var normalizedName = ValidateName(name);
        var normalizedDescription = ValidateDescription(description);
        Name = normalizedName;
        Description = normalizedDescription;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Specialty name is required.");
        var value = name.Trim();
        if (value.Length > 50)
            throw new ArgumentException("Specialty name cannot exceed 50 characters.");
        return value;
    }

    private static string? ValidateDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description)) return null;
        var value = description.Trim();
        if (value.Length > 500)
            throw new ArgumentException("Specialty description cannot exceed 500 characters.");
        return value;
    }
}
