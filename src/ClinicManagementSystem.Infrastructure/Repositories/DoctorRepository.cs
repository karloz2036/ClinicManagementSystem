using ClinicManagementSystem.Application.Features.Doctors.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Infrastructure.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ClinicDbContext _context;
    public DoctorRepository(ClinicDbContext context) => _context = context;

    public async Task<IReadOnlyList<Doctor>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Doctors.AsNoTracking()
            .Include(d => d.DoctorSpecialties).ThenInclude(ds => ds.Specialty)
            .OrderBy(d => d.LastName).ThenBy(d => d.FirstName)
            .ToListAsync(cancellationToken);

    public async Task<Doctor?> GetByIdAsync(int id, bool tracking = false, CancellationToken cancellationToken = default)
    {
        var query = _context.Doctors.Include(d => d.DoctorSpecialties).ThenInclude(ds => ds.Specialty).AsQueryable();
        if (!tracking) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public Task<bool> ExistsActiveAsync(int id, CancellationToken cancellationToken = default) =>
        _context.Doctors.AnyAsync(d => d.Id == id && d.IsActive, cancellationToken);

    public Task<bool> LicenseExistsAsync(string license, int? excludedId = null, CancellationToken cancellationToken = default) =>
        _context.Doctors.AnyAsync(d => d.ProfessionalLicense == license && (!excludedId.HasValue || d.Id != excludedId.Value), cancellationToken);

    public async Task AddAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        await _context.Doctors.AddAsync(doctor, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);
}
