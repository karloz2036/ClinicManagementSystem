using ClinicManagementSystem.Application.Features.AppointmentStatuses.DTOs;
using ClinicManagementSystem.Application.Features.AppointmentStatuses.Interfaces;

namespace ClinicManagementSystem.Application.Features.AppointmentStatuses.Services;

public class AppointmentStatusService : IAppointmentStatusService
{
    private readonly IAppointmentStatusRepository _repository;
    public AppointmentStatusService(IAppointmentStatusRepository repository) => _repository = repository;

    public async Task<IReadOnlyList<AppointmentStatusDto>> GetActiveAsync(CancellationToken cancellationToken = default) =>
        (await _repository.GetActiveAsync(cancellationToken))
        .Select(s => new AppointmentStatusDto { Id = s.Id, Name = s.Name }).ToList();
}
