using ClinicManagementSystem.Application.Features.Genders.DTOs;
using ClinicManagementSystem.Application.Features.Genders.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenderController : ControllerBase
    {
        private readonly IGenderService _genderService;

        public GenderController(IGenderService genderService)
        { 
            _genderService = genderService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GenderDTO>>> GetActive(CancellationToken cancellationToken)
        {
            var gender = await _genderService.GetActiveAsync(cancellationToken);
            return Ok(gender);
        }  
    }
}
