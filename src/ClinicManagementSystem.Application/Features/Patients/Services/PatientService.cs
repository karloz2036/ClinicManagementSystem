using ClinicManagementSystem.Application.Features.Genders.Interfaces;
using ClinicManagementSystem.Application.Features.Patients.DTOs;
using ClinicManagementSystem.Application.Features.Patients.Interfaces;
using ClinicManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.Features.Patients.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IGenderRepository _genderRepository;

        public PatientService(IPatientRepository patientRepository,
                                IGenderRepository genderRepository)
        {
            _patientRepository = patientRepository;
            _genderRepository = genderRepository;
        }

        public async Task<IReadOnlyList<PatientDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var patients = await _patientRepository.GetAllAsync(cancellationToken);

            return patients.Select(p => MapToDto(p)).ToList();

            /*
            return patients.Select(p => new PatientDto
            {
                Id = p.Id,
                Name = p.FirstName,
                LastName = p.LastName,
                BirthDate = p.DateOfBirth,
                GenderId = p.GenderId,
                GenderName = p.Gender.Name,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                IsActive = p.IsActive
            }).ToList();
            */

        }

        public async Task<PatientDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var patient = await _patientRepository.GetByIdAsync(id, cancellationToken);

            if (patient == null)
                return null;

            return MapToDto(patient);

            /*
            return new PatientDto
            {
                Id = patient.Id,
                Name = patient.FirstName,
                LastName = patient.LastName,
                BirthDate = patient.DateOfBirth,
                GenderId = patient.GenderId,
                GenderName = patient.Gender.Name,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                IsActive = patient.IsActive
            };
            */
        }

        public async Task<PatientDto> CreateAsync(CreatePatientDto dto, CancellationToken cancellationToken = default)
        {
            var genderExists = await _genderRepository.ExistsActiveAsync(dto.GenderId, cancellationToken);

            if (!genderExists)
                throw new ArgumentException("The selected gender does not exist or is inactive.", nameof(dto.GenderId));

            var patient = Patient.Create(
                dto.FirstName,
                dto.LastName,
                dto.DateOfBirth,
                dto.GenderId,
                dto.PhoneNumber,
                dto.Email,
                dto.Address);

            await _patientRepository.AddAsync(patient, cancellationToken);

            var createdPatient = await _patientRepository.GetByIdAsync(patient.Id, cancellationToken);

            if (createdPatient is null)
                throw new InvalidOperationException("The patient was created but could not be retrieved.");

            return MapToDto(createdPatient);
        }

        public async Task<PatientDto?> UpdateAsync(int id, UpdatePatientDto dto, CancellationToken cancellationToken = default)
        {
            //var existingPatient = await _patientRepository.GetByIdAsync(id, cancellationToken);
            var existingPatient = await _patientRepository.GetByIdForUpdateAsync(id, cancellationToken);

            if (existingPatient is null)
                return null;

            var genderExists = await _genderRepository.ExistsActiveAsync(dto.GenderId, cancellationToken);

            if (!genderExists)
                throw new ArgumentException("The selected gender not exists or is inactive", nameof(dto.GenderId));

            existingPatient.Update(dto.FirstName, dto.LastName, dto.DateOfBirth, dto.GenderId, dto.PhoneNumber, dto.Email, dto.Address);

            //await _patientRepository.UpdateAsync(existingPatient, cancellationToken);
            await _patientRepository.SaveChangesAsync(cancellationToken);

            var updatedPatient = await _patientRepository.GetByIdAsync(id, cancellationToken);

            if (updatedPatient is null)
                throw new InvalidOperationException("The patient was updated but  could not be retrieved");

            return MapToDto(updatedPatient);
        }

        public async Task<PatientDto?> UpdateStatusAsync(int patientId, UpdatePatientStatusDto dto, CancellationToken cancellationToken = default)
        {
            var patient = await _patientRepository.GetByIdForUpdateAsync(patientId, cancellationToken);

            if (patient is null)
                return null;

            if (dto.IsActive)
                patient.Activate();
            else
                patient.Deactivate();

            await _patientRepository.SaveChangesAsync(cancellationToken);

            var updatedPatient = await _patientRepository.GetByIdAsync(patientId, cancellationToken);

            if (updatedPatient is null)
                throw new ArgumentException("Tha patient status was updated but the patient could not be retrieved");

            return MapToDto(updatedPatient);
        }

        private static PatientDto MapToDto(Patient patient)
        {
            return new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                GenderId = patient.GenderId,
                GenderName = patient.Gender.Name,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                Address = patient.Address,
                IsActive = patient.IsActive,
                CreatedAt = patient.CreatedAt
            };
        }


    }
}
