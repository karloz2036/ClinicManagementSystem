using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Features.Doctors.Interfaces;

public interface IDoctorRepository
{
    Task<IReadOnlyList<Doctor>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Doctor?> GetByIdAsync(int id, bool tracking = false, CancellationToken cancellationToken = default);
    Task<bool> ExistsActiveAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> LicenseExistsAsync(string license, int? excludedId = null, CancellationToken cancellationToken = default);
    Task AddAsync(Doctor doctor, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
