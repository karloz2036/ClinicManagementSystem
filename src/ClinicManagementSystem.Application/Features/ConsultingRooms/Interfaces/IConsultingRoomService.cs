using ClinicManagementSystem.Application.Common.DTOs;
using ClinicManagementSystem.Application.Features.ConsultingRooms.DTOs;

namespace ClinicManagementSystem.Application.Features.ConsultingRooms.Interfaces;

public interface IConsultingRoomService
{
    Task<IReadOnlyList<ConsultingRoomDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ConsultingRoomDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ConsultingRoomDto> CreateAsync(CreateConsultingRoomDto dto, CancellationToken cancellationToken = default);
    Task<ConsultingRoomDto?> UpdateAsync(int id, UpdateConsultingRoomDto dto, CancellationToken cancellationToken = default);
    Task<ConsultingRoomDto?> UpdateStatusAsync(int id, UpdateStatusDto dto, CancellationToken cancellationToken = default);
}
