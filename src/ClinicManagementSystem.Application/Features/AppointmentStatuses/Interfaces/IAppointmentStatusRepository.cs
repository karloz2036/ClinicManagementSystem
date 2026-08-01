using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Features.AppointmentStatuses.Interfaces;

public interface IAppointmentStatusRepository
{
    Task<IReadOnlyList<AppointmentStatus>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsActiveAsync(int id, CancellationToken cancellationToken = default);
}
