namespace ClinicManagementSystem.Application.Features.Appointments.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public int ConsultingRoomId { get; set; }
    public string ConsultingRoomName { get; set; } = string.Empty;
    public int AppointmentStatusId { get; set; }
    public string AppointmentStatusName { get; set; } = string.Empty;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateAppointmentDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int ConsultingRoomId { get; set; }
    public int AppointmentStatusId { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Notes { get; set; }
}

public class RescheduleAppointmentDto
{
    public int DoctorId { get; set; }
    public int ConsultingRoomId { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Notes { get; set; }
}

public class UpdateAppointmentStatusDto
{
    public int AppointmentStatusId { get; set; }
}
