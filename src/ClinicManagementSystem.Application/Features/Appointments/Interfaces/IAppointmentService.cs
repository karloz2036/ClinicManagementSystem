using ClinicManagementSystem.Application.Features.Appointments.DTOs;

namespace ClinicManagementSystem.Application.Features.Appointments.Interfaces;

public interface IAppointmentService
{
    Task<IReadOnlyList<AppointmentDto>> GetAsync(DateTime? from, DateTime? to, int? doctorId, int? patientId, CancellationToken cancellationToken = default);
    Task<AppointmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto, CancellationToken cancellationToken = default);
    Task<AppointmentDto?> RescheduleAsync(int id, RescheduleAppointmentDto dto, CancellationToken cancellationToken = default);
    Task<AppointmentDto?> UpdateStatusAsync(int id, UpdateAppointmentStatusDto dto, CancellationToken cancellationToken = default);
}
