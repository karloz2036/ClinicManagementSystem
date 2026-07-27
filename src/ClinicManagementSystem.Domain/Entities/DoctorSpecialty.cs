namespace ClinicManagementSystem.Domain.Entities
{
    public class DoctorSpecialty
    {
        public int DoctorId { get; set; }

        public int SpecialtyId { get; set; }

        // Navigation properties (non-nullable to match NOT NULL FKs in the database)
        public Doctor Doctor { get; set; } = null!;

        public Specialty Specialty { get; set; } = null!;
    }
}
