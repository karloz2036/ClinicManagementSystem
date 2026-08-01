using ClinicManagementSystem.Application.Common.DTOs;
using ClinicManagementSystem.Application.Features.Specialties.DTOs;
using ClinicManagementSystem.Application.Features.Specialties.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecialtiesController : ControllerBase
{
    private readonly ISpecialtyService _service;
    public SpecialtiesController(ISpecialtyService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SpecialtyDto>>> GetAll(CancellationToken ct) => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SpecialtyDto>> GetById(int id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<SpecialtyDto>> Create(CreateSpecialtyDto dto, CancellationToken ct)
    {
        var result = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SpecialtyDto>> Update(int id, UpdateSpecialtyDto dto, CancellationToken ct)
    {
        var result = await _service.UpdateAsync(id, dto, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<SpecialtyDto>> UpdateStatus(int id, UpdateStatusDto dto, CancellationToken ct)
    {
        var result = await _service.UpdateStatusAsync(id, dto, ct);
        return result is null ? NotFound() : Ok(result);
    }
}
