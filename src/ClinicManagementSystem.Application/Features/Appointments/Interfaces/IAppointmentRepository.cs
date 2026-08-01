using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Features.Appointments.Interfaces;

public interface IAppointmentRepository
{
    Task<IReadOnlyList<Appointment>> GetAsync(DateTime? from, DateTime? to, int? doctorId, int? patientId, CancellationToken cancellationToken = default);
    Task<Appointment?> GetByIdAsync(int id, bool tracking = false, CancellationToken cancellationToken = default);
    Task<bool> HasScheduleConflictAsync(int doctorId, int consultingRoomId, DateTime start, DateTime end, int? excludedAppointmentId = null, CancellationToken cancellationToken = default);
    Task AddAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
