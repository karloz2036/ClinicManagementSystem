namespace ClinicManagementSystem.Domain.Entities;

public class Appointment
{
    private Appointment() { }

    public int Id { get; private set; }
    public int PatientId { get; private set; }
    public int DoctorId { get; private set; }
    public int ConsultingRoomId { get; private set; }
    public int AppointmentStatusId { get; private set; }
    public DateTime StartDateTime { get; private set; }
    public DateTime EndDateTime { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Patient Patient { get; private set; } = null!;
    public Doctor Doctor { get; private set; } = null!;
    public ConsultingRoom ConsultingRoom { get; private set; } = null!;
    public AppointmentStatus AppointmentStatus { get; private set; } = null!;

    public static Appointment Create(int patientId, int doctorId, int consultingRoomId, int appointmentStatusId,
        DateTime startDateTime, DateTime endDateTime, string? notes)
    {
        ValidateIds(patientId, doctorId, consultingRoomId, appointmentStatusId);
        ValidateSchedule(startDateTime, endDateTime);
        return new Appointment
        {
            PatientId = patientId,
            DoctorId = doctorId,
            ConsultingRoomId = consultingRoomId,
            AppointmentStatusId = appointmentStatusId,
            StartDateTime = startDateTime,
            EndDateTime = endDateTime,
            Notes = NormalizeNotes(notes),
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Reschedule(int doctorId, int consultingRoomId, DateTime startDateTime, DateTime endDateTime, string? notes)
    {
        ValidateIds(PatientId, doctorId, consultingRoomId, AppointmentStatusId);
        ValidateSchedule(startDateTime, endDateTime);
        var normalizedNotes = NormalizeNotes(notes);
        DoctorId = doctorId;
        ConsultingRoomId = consultingRoomId;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        Notes = normalizedNotes;
    }

    public void ChangeStatus(int appointmentStatusId)
    {
        if (appointmentStatusId <= 0) throw new ArgumentException("Appointment status id must be greater than zero.");
        AppointmentStatusId = appointmentStatusId;
    }

    private static void ValidateIds(int patientId, int doctorId, int roomId, int statusId)
    {
        if (patientId <= 0 || doctorId <= 0 || roomId <= 0 || statusId <= 0)
            throw new ArgumentException("Patient, doctor, consulting room and status ids must be greater than zero.");
    }

    private static void ValidateSchedule(DateTime start, DateTime end)
    {
        if (start >= end) throw new ArgumentException("Appointment end time must be later than start time.");
        if (start < DateTime.Now) throw new ArgumentException("An appointment cannot be scheduled in the past.");
        if ((end - start).TotalHours > 4) throw new ArgumentException("An appointment cannot last more than four hours.");
    }

    private static string? NormalizeNotes(string? notes)
    {
        if (string.IsNullOrWhiteSpace(notes)) return null;
        var value = notes.Trim();
        if (value.Length > 1000) throw new ArgumentException("Notes cannot exceed 1000 characters.");
        return value;
    }
}
