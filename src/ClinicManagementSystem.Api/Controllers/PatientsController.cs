using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClinicManagementSystem.Application.Features.Patients.Interfaces;
using ClinicManagementSystem.Application.Features.Patients.DTOs;


namespace ClinicManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PatientDto>>> GetAllPatients(CancellationToken cancellationToken)
        {
            var patients = await _patientService.GetAllAsync(cancellationToken);
            return Ok(patients);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PatientDto>> GetPatientById(int id, CancellationToken cancellationToken)
        {
            var patient = await _patientService.GetByIdAsync(id, cancellationToken);

            if (patient == null)
                return NotFound();

            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult<PatientDto>> CreatePatient(CreatePatientDto dto, CancellationToken cancellationToken)
        {
            var patient = await _patientService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, patient);

            //este codigo se comenta porque ya se agrego un manejador global de excepciones.
            #region MyRegion
            /*
            try
            {
                var patient = await _patientService.CreateAsync(dto, cancellationToken);

                return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, patient);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new
                {
                    message = exception.Message
                });
            }
            */


            #endregion
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PatientDto>> UpdatePatient(int id, UpdatePatientDto dto, CancellationToken cancellationToken)
        {
            var patient = await _patientService.UpdateAsync(id, dto, cancellationToken);

            if (patient is null)
                return NotFound();

            return Ok(patient);
        }

        [HttpPatch("{patientId}/status")]
        public async Task<ActionResult<PatientDto>> UpdateStatusPatientStatus(int patientId, UpdatePatientStatusDto dto, CancellationToken cancellationToken)
        {
            var patient = await _patientService.UpdateStatus(patientId, dto, cancellationToken);

            if (patient is null)
                return NotFound();

            return Ok(patient);

        }
    }
}
