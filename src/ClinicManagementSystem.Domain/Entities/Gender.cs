using System.Collections.Generic;

namespace ClinicManagementSystem.Domain.Entities
{
    public class Gender
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    }
}
