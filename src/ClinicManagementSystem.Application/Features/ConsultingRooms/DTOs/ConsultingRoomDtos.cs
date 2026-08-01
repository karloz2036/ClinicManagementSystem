namespace ClinicManagementSystem.Application.Features.ConsultingRooms.DTOs;

public class ConsultingRoomDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public bool IsActive { get; set; }
}

public class CreateConsultingRoomDto
{
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
}

public class UpdateConsultingRoomDto : CreateConsultingRoomDto { }
