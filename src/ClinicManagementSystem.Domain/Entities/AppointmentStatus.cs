namespace ClinicManagementSystem.Domain.Entities;

public class AppointmentStatus
{
    private AppointmentStatus() { }
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();
}
