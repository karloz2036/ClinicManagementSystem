using ClinicManagementSystem.Application.Features.Patients.DTOs;
using ClinicManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.Features.Patients.Interfaces
{
    public interface IPatientService
    {
        Task<IReadOnlyList<PatientDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<PatientDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PatientDto> CreateAsync(CreatePatientDto dto, CancellationToken cancellationToken = default);
        Task<PatientDto?> UpdateAsync(int id, UpdatePatientDto dto, CancellationToken cancellationToken = default);
        Task<PatientDto?> UpdateStatus(int patientId, UpdatePatientStatusDto dto, CancellationToken cancellationToken = default);
    }
}
