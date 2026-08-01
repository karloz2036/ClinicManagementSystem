using ClinicManagementSystem.Application.Common.DTOs;
using ClinicManagementSystem.Application.Features.Doctors.DTOs;
using ClinicManagementSystem.Application.Features.Doctors.Interfaces;
using ClinicManagementSystem.Application.Features.Specialties.Interfaces;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Features.Doctors.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _repository;
    private readonly ISpecialtyRepository _specialtyRepository;

    public DoctorService(IDoctorRepository repository, ISpecialtyRepository specialtyRepository)
    {
        _repository = repository;
        _specialtyRepository = specialtyRepository;
    }

    public async Task<IReadOnlyList<DoctorDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await _repository.GetAllAsync(cancellationToken)).Select(Map).ToList();

    public async Task<DoctorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var doctor = await _repository.GetByIdAsync(id, cancellationToken: cancellationToken);
        return doctor is null ? null : Map(doctor);
    }

    public async Task<DoctorDto> CreateAsync(CreateDoctorDto dto, CancellationToken cancellationToken = default)
    {
        await ValidateAsync(dto.ProfessionalLicense, dto.SpecialtyIds, null, cancellationToken);
        var doctor = Doctor.Create(dto.FirstName, dto.LastName, dto.ProfessionalLicense, dto.PhoneNumber, dto.Email);
        doctor.ReplaceSpecialties(dto.SpecialtyIds);
        await _repository.AddAsync(doctor, cancellationToken);
        var created = await _repository.GetByIdAsync(doctor.Id, cancellationToken: cancellationToken);
        return Map(created ?? doctor);
    }

    public async Task<DoctorDto?> UpdateAsync(int id, UpdateDoctorDto dto, CancellationToken cancellationToken = default)
    {
        var doctor = await _repository.GetByIdAsync(id, true, cancellationToken);
        if (doctor is null) return null;
        await ValidateAsync(dto.ProfessionalLicense, dto.SpecialtyIds, id, cancellationToken);
        doctor.Update(dto.FirstName, dto.LastName, dto.ProfessionalLicense, dto.PhoneNumber, dto.Email);
        doctor.ReplaceSpecialties(dto.SpecialtyIds);
        await _repository.SaveChangesAsync(cancellationToken);
        var updated = await _repository.GetByIdAsync(id, cancellationToken: cancellationToken);
        return Map(updated ?? doctor);
    }

    public async Task<DoctorDto?> UpdateStatusAsync(int id, UpdateStatusDto dto, CancellationToken cancellationToken = default)
    {
        var doctor = await _repository.GetByIdAsync(id, true, cancellationToken);
        if (doctor is null) return null;
        if (dto.IsActive) doctor.Activate(); else doctor.Deactivate();
        await _repository.SaveChangesAsync(cancellationToken);
        return Map(doctor);
    }

    private async Task ValidateAsync(string license, IEnumerable<int> specialtyIds, int? excludedId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(license)) throw new ArgumentException("Professional license is required.");
        if (await _repository.LicenseExistsAsync(license.Trim(), excludedId, cancellationToken))
            throw new ArgumentException("A doctor with the same professional license already exists.");
        var ids = specialtyIds.ToList();
        if (ids.Count == 0 || !await _specialtyRepository.AllActiveAsync(ids, cancellationToken))
            throw new ArgumentException("Every selected specialty must exist and be active.");
    }

    private static DoctorDto Map(Doctor d) => new()
    {
        Id = d.Id,
        FirstName = d.FirstName,
        LastName = d.LastName,
        ProfessionalLicense = d.ProfessionalLicense,
        PhoneNumber = d.PhoneNumber,
        Email = d.Email,
        IsActive = d.IsActive,
        CreatedAt = d.CreatedAt,
        Specialties = d.DoctorSpecialties.Select(ds => new DoctorSpecialtyDto
        {
            Id = ds.SpecialtyId,
            Name = ds.Specialty?.Name ?? string.Empty
        }).OrderBy(s => s.Name).ToList()
    };
}
