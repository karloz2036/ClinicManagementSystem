using ClinicManagementSystem.Application.Common.DTOs;
using ClinicManagementSystem.Application.Features.Specialties.DTOs;

namespace ClinicManagementSystem.Application.Features.Specialties.Interfaces;

public interface ISpecialtyService
{
    Task<IReadOnlyList<SpecialtyDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SpecialtyDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<SpecialtyDto> CreateAsync(CreateSpecialtyDto dto, CancellationToken cancellationToken = default);
    Task<SpecialtyDto?> UpdateAsync(int id, UpdateSpecialtyDto dto, CancellationToken cancellationToken = default);
    Task<SpecialtyDto?> UpdateStatusAsync(int id, UpdateStatusDto dto, CancellationToken cancellationToken = default);
}
