using ClinicManagementSystem.Application.Common.DTOs;
using ClinicManagementSystem.Application.Features.Doctors.DTOs;

namespace ClinicManagementSystem.Application.Features.Doctors.Interfaces;

public interface IDoctorService
{
    Task<IReadOnlyList<DoctorDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DoctorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<DoctorDto> CreateAsync(CreateDoctorDto dto, CancellationToken cancellationToken = default);
    Task<DoctorDto?> UpdateAsync(int id, UpdateDoctorDto dto, CancellationToken cancellationToken = default);
    Task<DoctorDto?> UpdateStatusAsync(int id, UpdateStatusDto dto, CancellationToken cancellationToken = default);
}
