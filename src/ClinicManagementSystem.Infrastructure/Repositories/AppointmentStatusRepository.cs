using ClinicManagementSystem.Application.Features.AppointmentStatuses.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Infrastructure.Repositories;

public class AppointmentStatusRepository : IAppointmentStatusRepository
{
    private readonly ClinicDbContext _context;
    public AppointmentStatusRepository(ClinicDbContext context) => _context = context;

    public async Task<IReadOnlyList<AppointmentStatus>> GetActiveAsync(CancellationToken cancellationToken = default) =>
        await _context.AppointmentStatuses.AsNoTracking().Where(s => s.IsActive).OrderBy(s => s.Id).ToListAsync(cancellationToken);

    public Task<bool> ExistsActiveAsync(int id, CancellationToken cancellationToken = default) =>
        _context.AppointmentStatuses.AnyAsync(s => s.Id == id && s.IsActive, cancellationToken);
}
