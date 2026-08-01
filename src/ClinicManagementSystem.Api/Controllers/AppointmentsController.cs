using ClinicManagementSystem.Application.Features.Appointments.DTOs;
using ClinicManagementSystem.Application.Features.Appointments.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _service;
    public AppointmentsController(IAppointmentService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> Get(
        [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int? doctorId,
        [FromQuery] int? patientId, CancellationToken ct) =>
        Ok(await _service.GetAsync(from, to, doctorId, patientId, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppointmentDto>> GetById(int id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> Create(CreateAppointmentDto dto, CancellationToken ct)
    {
        var result = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}/schedule")]
    public async Task<ActionResult<AppointmentDto>> Reschedule(int id, RescheduleAppointmentDto dto, CancellationToken ct)
    {
        var result = await _service.RescheduleAsync(id, dto, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<AppointmentDto>> UpdateStatus(int id, UpdateAppointmentStatusDto dto, CancellationToken ct)
    {
        var result = await _service.UpdateStatusAsync(id, dto, ct);
        return result is null ? NotFound() : Ok(result);
    }
}
