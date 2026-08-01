using ClinicManagementSystem.Application.Features.Appointments.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ClinicDbContext _context;
    public AppointmentRepository(ClinicDbContext context) => _context = context;

    public async Task<IReadOnlyList<Appointment>> GetAsync(DateTime? from, DateTime? to, int? doctorId, int? patientId, CancellationToken cancellationToken = default)
    {
        var query = CompleteQuery().AsNoTracking();
        if (from.HasValue) query = query.Where(a => a.StartDateTime >= from.Value);
        if (to.HasValue) query = query.Where(a => a.StartDateTime <= to.Value);
        if (doctorId.HasValue) query = query.Where(a => a.DoctorId == doctorId.Value);
        if (patientId.HasValue) query = query.Where(a => a.PatientId == patientId.Value);
        return await query.OrderBy(a => a.StartDateTime).ToListAsync(cancellationToken);
    }

    public async Task<Appointment?> GetByIdAsync(int id, bool tracking = false, CancellationToken cancellationToken = default)
    {
        var query = CompleteQuery();
        if (!tracking) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public Task<bool> HasScheduleConflictAsync(int doctorId, int consultingRoomId, DateTime start, DateTime end,
        int? excludedAppointmentId = null, CancellationToken cancellationToken = default) =>
        _context.Appointments.AnyAsync(a =>
            a.DoctorId == doctorId && a.ConsultingRoomId == consultingRoomId &&
            (!excludedAppointmentId.HasValue || a.Id != excludedAppointmentId.Value) &&
            a.StartDateTime <= end && a.EndDateTime >= start,
            cancellationToken);

    public async Task AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        await _context.Appointments.AddAsync(appointment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);

    private IQueryable<Appointment> CompleteQuery() => _context.Appointments
        .Include(a => a.Patient)
        .Include(a => a.Doctor)
        .Include(a => a.ConsultingRoom)
        .Include(a => a.AppointmentStatus);
}
