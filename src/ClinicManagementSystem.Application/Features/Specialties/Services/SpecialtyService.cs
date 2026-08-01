using ClinicManagementSystem.Application.Common.DTOs;
using ClinicManagementSystem.Application.Features.Specialties.DTOs;
using ClinicManagementSystem.Application.Features.Specialties.Interfaces;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Features.Specialties.Services;

public class SpecialtyService : ISpecialtyService
{
    private readonly ISpecialtyRepository _repository;
    public SpecialtyService(ISpecialtyRepository repository) => _repository = repository;

    public async Task<IReadOnlyList<SpecialtyDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await _repository.GetAllAsync(cancellationToken)).Select(Map).ToList();

    public async Task<SpecialtyDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken: cancellationToken);
        return entity is null ? null : Map(entity);
    }

    public async Task<SpecialtyDto> CreateAsync(CreateSpecialtyDto dto, CancellationToken cancellationToken = default)
    {
        if (await _repository.NameExistsAsync(dto.Name.Trim(), cancellationToken: cancellationToken))
            throw new ArgumentException("A specialty with the same name already exists.");
        var entity = Specialty.Create(dto.Name, dto.Description);
        await _repository.AddAsync(entity, cancellationToken);
        return Map(entity);
    }

    public async Task<SpecialtyDto?> UpdateAsync(int id, UpdateSpecialtyDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, true, cancellationToken);
        if (entity is null) return null;
        if (await _repository.NameExistsAsync(dto.Name.Trim(), id, cancellationToken))
            throw new ArgumentException("A specialty with the same name already exists.");
        entity.Update(dto.Name, dto.Description);
        await _repository.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task<SpecialtyDto?> UpdateStatusAsync(int id, UpdateStatusDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, true, cancellationToken);
        if (entity is null) return null;
        if (dto.IsActive) entity.Activate(); else entity.Deactivate();
        await _repository.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    private static SpecialtyDto Map(Specialty s) => new() { Id = s.Id, Name = s.Name, Description = s.Description, IsActive = s.IsActive };
}
