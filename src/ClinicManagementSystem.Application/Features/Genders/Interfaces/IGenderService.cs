using ClinicManagementSystem.Application.Features.Genders.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.Features.Genders.Interfaces
{
    public interface IGenderService
    {
        Task<IReadOnlyList<GenderDTO>> GetActiveAsync(CancellationToken cancellationToken = default);

    }
}
