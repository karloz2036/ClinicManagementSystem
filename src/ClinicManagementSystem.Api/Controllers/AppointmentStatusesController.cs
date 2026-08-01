using ClinicManagementSystem.Application.Features.AppointmentStatuses.DTOs;
using ClinicManagementSystem.Application.Features.AppointmentStatuses.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Api.Controllers;

[ApiController]
[Route("api/appointment-statuses")]
public class AppointmentStatusesController : ControllerBase
{
    private readonly IAppointmentStatusService _service;
    public AppointmentStatusesController(IAppointmentStatusService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AppointmentStatusDto>>> GetActive(CancellationToken ct) =>
        Ok(await _service.GetActiveAsync(ct));
}
