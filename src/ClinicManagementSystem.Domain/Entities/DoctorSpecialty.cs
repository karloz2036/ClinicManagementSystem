namespace ClinicManagementSystem.Domain.Entities;

public class DoctorSpecialty
{
    private DoctorSpecialty() { }

    public int DoctorId { get; private set; }
    public int SpecialtyId { get; private set; }
    public Doctor Doctor { get; private set; } = null!;
    public Specialty Specialty { get; private set; } = null!;

    public static DoctorSpecialty Create(int specialtyId)
    {
        if (specialtyId <= 0) throw new ArgumentException("Specialty id must be greater than zero.");
        return new DoctorSpecialty { SpecialtyId = specialtyId };
    }
}
