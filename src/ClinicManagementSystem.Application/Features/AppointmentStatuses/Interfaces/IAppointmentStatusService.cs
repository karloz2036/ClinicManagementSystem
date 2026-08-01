using ClinicManagementSystem.Application.Features.AppointmentStatuses.DTOs;

namespace ClinicManagementSystem.Application.Features.AppointmentStatuses.Interfaces;

public interface IAppointmentStatusService
{
    Task<IReadOnlyList<AppointmentStatusDto>> GetActiveAsync(CancellationToken cancellationToken = default);
}
