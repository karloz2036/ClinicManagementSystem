using ClinicManagementSystem.Application.Features.Patients.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ClinicDbContext _context;

        public PatientRepository(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var patient = await _context.Patients
                .AsNoTracking()
                .Include(p => p.Gender)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return patient;
        }

        public async Task<Patient?> GetByIdForUpdateAsync(int id, CancellationToken cancellation = default)
        {
            var patient = await _context.Patients
                .Include(p => p.Gender)
                .FirstOrDefaultAsync(p => p.Id == id);

            return patient;
        }

        public async Task<IReadOnlyList<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var patients = await _context.Patients
                .AsNoTracking()
                .Include(p => p.Gender)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToListAsync(cancellationToken);

            return patients;
        }

        public async Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default)
        {
            await _context.Patients.AddAsync(patient, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return patient;
        }

        public async Task<Patient> UpdateAsync(Patient patient, CancellationToken cancellationToken = default)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync(cancellationToken);
            return patient;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
