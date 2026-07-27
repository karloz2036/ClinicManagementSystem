using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Features.Genders.Interfaces
{
    public interface IGenderRepository
    {
        Task<IReadOnlyList<Gender>> GetActiveAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsActiveAsync(int id, CancellationToken cancellationToken = default);
    }
}
