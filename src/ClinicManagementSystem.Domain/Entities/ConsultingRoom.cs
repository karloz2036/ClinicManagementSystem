namespace ClinicManagementSystem.Domain.Entities;

public class ConsultingRoom
{
    private ConsultingRoom() { }

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Location { get; private set; }
    public bool IsActive { get; private set; }
    public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();

    public static ConsultingRoom Create(string name, string? location)
    {
        return new ConsultingRoom
        {
            Name = ValidateText(name, "Consulting room name", 50),
            Location = NormalizeOptional(location, "Location", 150),
            IsActive = true
        };
    }

    public void Update(string name, string? location)
    {
        var normalizedName = ValidateText(name, "Consulting room name", 50);
        var normalizedLocation = NormalizeOptional(location, "Location", 150);
        Name = normalizedName;
        Location = normalizedLocation;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    private static string ValidateText(string value, string field, int maxLength)
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
}
