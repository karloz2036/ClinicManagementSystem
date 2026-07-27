using System.Collections.Generic;

namespace ClinicManagementSystem.Domain.Entities
{
    public class AppointmentStatus
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
