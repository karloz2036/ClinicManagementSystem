using ClinicManagementSystem.Application.Common.DTOs;
using ClinicManagementSystem.Application.Features.ConsultingRooms.DTOs;
using ClinicManagementSystem.Application.Features.ConsultingRooms.Interfaces;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Features.ConsultingRooms.Services;

public class ConsultingRoomService : IConsultingRoomService
{
    private readonly IConsultingRoomRepository _repository;
    public ConsultingRoomService(IConsultingRoomRepository repository) => _repository = repository;

    public async Task<IReadOnlyList<ConsultingRoomDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await _repository.GetAllAsync(cancellationToken)).Select(Map).ToList();

    public async Task<ConsultingRoomDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken: cancellationToken);
        return entity is null ? null : Map(entity);
    }

    public async Task<ConsultingRoomDto> CreateAsync(CreateConsultingRoomDto dto, CancellationToken cancellationToken = default)
    {
        if (await _repository.NameExistsAsync(dto.Name.Trim(), cancellationToken: cancellationToken))
            throw new ArgumentException("A consulting room with the same name already exists.");
        var entity = ConsultingRoom.Create(dto.Name, dto.Location);
        await _repository.AddAsync(entity, cancellationToken);
        return Map(entity);
    }

    public async Task<ConsultingRoomDto?> UpdateAsync(int id, UpdateConsultingRoomDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, true, cancellationToken);
        if (entity is null) return null;
        if (await _repository.NameExistsAsync(dto.Name.Trim(), id, cancellationToken))
            throw new ArgumentException("A consulting room with the same name already exists.");
        entity.Update(dto.Name, dto.Location);
        await _repository.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task<ConsultingRoomDto?> UpdateStatusAsync(int id, UpdateStatusDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, true, cancellationToken);
        if (entity is null) return null;
        if (dto.IsActive) entity.Activate(); else entity.Deactivate();
        await _repository.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    private static ConsultingRoomDto Map(ConsultingRoom r) => new() { Id = r.Id, Name = r.Name, Location = r.Location, IsActive = r.IsActive };
}
