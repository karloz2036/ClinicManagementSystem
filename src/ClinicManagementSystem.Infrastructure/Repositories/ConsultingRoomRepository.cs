using ClinicManagementSystem.Application.Features.ConsultingRooms.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Infrastructure.Repositories;

public class ConsultingRoomRepository : IConsultingRoomRepository
{
    private readonly ClinicDbContext _context;
    public ConsultingRoomRepository(ClinicDbContext context) => _context = context;

    public async Task<IReadOnlyList<ConsultingRoom>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.ConsultingRooms.AsNoTracking().OrderBy(r => r.Name).ToListAsync(cancellationToken);

    public async Task<ConsultingRoom?> GetByIdAsync(int id, bool tracking = false, CancellationToken cancellationToken = default)
    {
        var query = _context.ConsultingRooms.AsQueryable();
        if (!tracking) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public Task<bool> ExistsActiveAsync(int id, CancellationToken cancellationToken = default) =>
        _context.ConsultingRooms.AnyAsync(r => r.Id == id && r.IsActive, cancellationToken);

    public Task<bool> NameExistsAsync(string name, int? excludedId = null, CancellationToken cancellationToken = default) =>
        _context.ConsultingRooms.AnyAsync(r => r.Name == name && (!excludedId.HasValue || r.Id != excludedId.Value), cancellationToken);

    public async Task AddAsync(ConsultingRoom room, CancellationToken cancellationToken = default)
    {
        await _context.ConsultingRooms.AddAsync(room, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);
}
