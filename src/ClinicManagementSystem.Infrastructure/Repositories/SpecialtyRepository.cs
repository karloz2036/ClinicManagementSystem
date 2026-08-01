using ClinicManagementSystem.Application.Features.Specialties.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Infrastructure.Repositories;

public class SpecialtyRepository : ISpecialtyRepository
{
    private readonly ClinicDbContext _context;
    public SpecialtyRepository(ClinicDbContext context) => _context = context;

    public async Task<IReadOnlyList<Specialty>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Specialties.AsNoTracking().OrderBy(s => s.Name).ToListAsync(cancellationToken);

    public async Task<Specialty?> GetByIdAsync(int id, bool tracking = false, CancellationToken cancellationToken = default)
    {
        var query = _context.Specialties.AsQueryable();
        if (!tracking) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public Task<bool> ExistsActiveAsync(int id, CancellationToken cancellationToken = default) =>
        _context.Specialties.AnyAsync(s => s.Id == id && s.IsActive, cancellationToken);

    public async Task<bool> AllActiveAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        var requested = ids.Distinct().ToList();
        if (requested.Count == 0) return false;
        var count = await _context.Specialties.CountAsync(s => requested.Contains(s.Id) && s.IsActive, cancellationToken);
        return count == requested.Count;
    }

    public Task<bool> NameExistsAsync(string name, int? excludedId = null, CancellationToken cancellationToken = default) =>
        _context.Specialties.AnyAsync(s => s.Name == name && (!excludedId.HasValue || s.Id != excludedId.Value), cancellationToken);

    public async Task AddAsync(Specialty specialty, CancellationToken cancellationToken = default)
    {
        await _context.Specialties.AddAsync(specialty, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);
}
