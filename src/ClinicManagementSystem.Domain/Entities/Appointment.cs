using System;

namespace ClinicManagementSystem.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public int ConsultingRoomId { get; set; }

        public int AppointmentStatusId { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties (non-nullable to match NOT NULL FKs in the database)
        public Patient Patient { get; set; } = null!;

        public Doctor Doctor { get; set; } = null!;

        public ConsultingRoom ConsultingRoom { get; set; } = null!;

        public AppointmentStatus AppointmentStatus { get; set; } = null!;
    }
}
