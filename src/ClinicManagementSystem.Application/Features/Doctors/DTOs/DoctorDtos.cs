namespace ClinicManagementSystem.Application.Features.Doctors.DTOs;

public class DoctorDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ProfessionalLicense { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public IReadOnlyList<DoctorSpecialtyDto> Specialties { get; set; } = new List<DoctorSpecialtyDto>();
}

public class DoctorSpecialtyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CreateDoctorDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ProfessionalLicense { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public List<int> SpecialtyIds { get; set; } = new();
}

public class UpdateDoctorDto : CreateDoctorDto { }
