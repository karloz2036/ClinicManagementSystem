using ClinicManagementSystem.Application.Features.Genders.DTOs;
using ClinicManagementSystem.Application.Features.Genders.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.Features.Genders.Services
{
    public class GenderService : IGenderService
    {
        private readonly IGenderRepository _genderRepository;

        public GenderService(IGenderRepository genderRepository)
        {
            _genderRepository = genderRepository;
        }

        public async Task<IReadOnlyList<GenderDTO>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            var genders = await _genderRepository.GetActiveAsync(cancellationToken);

            return genders
                .Select(gender => new GenderDTO
                {
                    Id = gender.Id,
                    Name = gender.Name
                }).ToList();

        }



    }
}
