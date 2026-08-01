using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Features.Specialties.Interfaces;

public interface ISpecialtyRepository
{
    Task<IReadOnlyList<Specialty>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Specialty?> GetByIdAsync(int id, bool tracking = false, CancellationToken cancellationToken = default);
    Task<bool> ExistsActiveAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AllActiveAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    Task<bool> NameExistsAsync(string name, int? excludedId = null, CancellationToken cancellationToken = default);
    Task AddAsync(Specialty specialty, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
