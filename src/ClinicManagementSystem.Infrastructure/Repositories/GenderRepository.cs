using ClinicManagementSystem.Application.Features.Genders.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Infrastructure.Repositories
{
    public class GenderRepository : IGenderRepository
    {
        private readonly ClinicDbContext _dbContext;

        public GenderRepository(ClinicDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Gender>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Genders
                .AsNoTracking()
                .Where(g => g.IsActive)
                .OrderBy(g => g.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsActiveAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Genders
                .AsNoTracking()
                .AnyAsync(
                    gender => gender.Id == id && gender.IsActive,
                    cancellationToken);
        }


    }
}
