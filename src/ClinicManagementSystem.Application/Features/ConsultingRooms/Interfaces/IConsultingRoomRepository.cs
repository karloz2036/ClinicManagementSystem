using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Features.ConsultingRooms.Interfaces;

public interface IConsultingRoomRepository
{
    Task<IReadOnlyList<ConsultingRoom>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ConsultingRoom?> GetByIdAsync(int id, bool tracking = false, CancellationToken cancellationToken = default);
    Task<bool> ExistsActiveAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> NameExistsAsync(string name, int? excludedId = null, CancellationToken cancellationToken = default);
    Task AddAsync(ConsultingRoom room, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
