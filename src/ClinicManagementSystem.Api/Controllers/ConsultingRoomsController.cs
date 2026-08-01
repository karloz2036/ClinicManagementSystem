using ClinicManagementSystem.Application.Common.DTOs;
using ClinicManagementSystem.Application.Features.ConsultingRooms.DTOs;
using ClinicManagementSystem.Application.Features.ConsultingRooms.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultingRoomsController : ControllerBase
{
    private readonly IConsultingRoomService _service;
    public ConsultingRoomsController(IConsultingRoomService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ConsultingRoomDto>>> GetAll(CancellationToken ct) => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ConsultingRoomDto>> GetById(int id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ConsultingRoomDto>> Create(CreateConsultingRoomDto dto, CancellationToken ct)
    {
        var result = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ConsultingRoomDto>> Update(int id, UpdateConsultingRoomDto dto, CancellationToken ct)
    {
        var result = await _service.UpdateAsync(id, dto, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<ConsultingRoomDto>> UpdateStatus(int id, UpdateStatusDto dto, CancellationToken ct)
    {
        var result = await _service.UpdateStatusAsync(id, dto, ct);
        return result is null ? NotFound() : Ok(result);
    }
}
