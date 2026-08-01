using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Features.Patients.Interfaces
{
    public interface IPatientRepository
    {
        Task<IReadOnlyList<Patient>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Patient?> GetByIdForUpdateAsync(int id, CancellationToken cancellation = default);
        Task<bool> ExistsActiveAsync(int id, CancellationToken cancellationToken = default);
        Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default);
        Task<Patient> UpdateAsync(Patient patient, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
