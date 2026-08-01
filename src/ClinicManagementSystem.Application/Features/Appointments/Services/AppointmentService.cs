using ClinicManagementSystem.Application.Features.Appointments.DTOs;
using ClinicManagementSystem.Application.Features.Appointments.Interfaces;
using ClinicManagementSystem.Application.Features.AppointmentStatuses.Interfaces;
using ClinicManagementSystem.Application.Features.ConsultingRooms.Interfaces;
using ClinicManagementSystem.Application.Features.Doctors.Interfaces;
using ClinicManagementSystem.Application.Features.Patients.Interfaces;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Application.Features.Appointments.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repository;
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IConsultingRoomRepository _roomRepository;
    private readonly IAppointmentStatusRepository _statusRepository;

    public AppointmentService(IAppointmentRepository repository, IPatientRepository patientRepository,
        IDoctorRepository doctorRepository, IConsultingRoomRepository roomRepository,
        IAppointmentStatusRepository statusRepository)
    {
        _repository = repository;
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _roomRepository = roomRepository;
        _statusRepository = statusRepository;
    }

    public async Task<IReadOnlyList<AppointmentDto>> GetAsync(DateTime? from, DateTime? to, int? doctorId, int? patientId, CancellationToken cancellationToken = default) =>
        (await _repository.GetAsync(from, to, doctorId, patientId, cancellationToken)).Select(Map).ToList();

    public async Task<AppointmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken: cancellationToken);
        return entity is null ? null : Map(entity);
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto, CancellationToken cancellationToken = default)
    {
        await ValidateReferencesAsync(dto.PatientId, dto.DoctorId, dto.ConsultingRoomId, dto.AppointmentStatusId, cancellationToken);
        await EnsureAvailableAsync(dto.DoctorId, dto.ConsultingRoomId, dto.StartDateTime, dto.EndDateTime, null, cancellationToken);
        var entity = Appointment.Create(dto.PatientId, dto.DoctorId, dto.ConsultingRoomId, dto.AppointmentStatusId,
            dto.StartDateTime, dto.EndDateTime, dto.Notes);
        await _repository.AddAsync(entity, cancellationToken);
        var created = await _repository.GetByIdAsync(entity.Id, cancellationToken: cancellationToken);
        if (created is null) throw new InvalidOperationException("The appointment was created but could not be retrieved.");
        return Map(created);
    }

    public async Task<AppointmentDto?> RescheduleAsync(int id, RescheduleAppointmentDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, true, cancellationToken);
        if (entity is null) return null;
        if (!await _doctorRepository.ExistsActiveAsync(dto.DoctorId, cancellationToken))
            throw new ArgumentException("The selected doctor does not exist or is inactive.");
        if (!await _roomRepository.ExistsActiveAsync(dto.ConsultingRoomId, cancellationToken))
            throw new ArgumentException("The selected consulting room does not exist or is inactive.");
        await EnsureAvailableAsync(dto.DoctorId, dto.ConsultingRoomId, dto.StartDateTime, dto.EndDateTime, id, cancellationToken);
        entity.Reschedule(dto.DoctorId, dto.ConsultingRoomId, dto.StartDateTime, dto.EndDateTime, dto.Notes);
        await _repository.SaveChangesAsync(cancellationToken);
        var updated = await _repository.GetByIdAsync(id, cancellationToken: cancellationToken);
        return Map(updated ?? entity);
    }

    public async Task<AppointmentDto?> UpdateStatusAsync(int id, UpdateAppointmentStatusDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, true, cancellationToken);
        if (entity is null) return null;
        if (!await _statusRepository.ExistsActiveAsync(dto.AppointmentStatusId, cancellationToken))
            throw new ArgumentException("The selected appointment status does not exist or is inactive.");
        entity.ChangeStatus(dto.AppointmentStatusId);
        await _repository.SaveChangesAsync(cancellationToken);
        var updated = await _repository.GetByIdAsync(id, cancellationToken: cancellationToken);
        return Map(updated ?? entity);
    }

    private async Task ValidateReferencesAsync(int patientId, int doctorId, int roomId, int statusId, CancellationToken cancellationToken)
    {
        if (!await _patientRepository.ExistsActiveAsync(patientId, cancellationToken))
            throw new ArgumentException("The selected patient does not exist or is inactive.");
        if (!await _doctorRepository.ExistsActiveAsync(doctorId, cancellationToken))
            throw new ArgumentException("The selected doctor does not exist or is inactive.");
        if (!await _roomRepository.ExistsActiveAsync(roomId, cancellationToken))
            throw new ArgumentException("The selected consulting room does not exist or is inactive.");
        if (!await _statusRepository.ExistsActiveAsync(statusId, cancellationToken))
            throw new ArgumentException("The selected appointment status does not exist or is inactive.");
    }

    private async Task EnsureAvailableAsync(int doctorId, int roomId, DateTime start, DateTime end, int? excludedId, CancellationToken cancellationToken)
    {
        if (await _repository.HasScheduleConflictAsync(doctorId, roomId, start, end, excludedId, cancellationToken))
            throw new ArgumentException("The doctor or consulting room is not available during the selected period.");
    }

    private static AppointmentDto Map(Appointment a) => new()
    {
        Id = a.Id,
        PatientId = a.PatientId,
        PatientName = $"{a.Patient?.FirstName} {a.Patient?.LastName}".Trim(),
        DoctorId = a.DoctorId,
        DoctorName = $"{a.Doctor?.FirstName} {a.Doctor?.LastName}".Trim(),
        ConsultingRoomId = a.ConsultingRoomId,
        ConsultingRoomName = a.ConsultingRoom?.Name ?? string.Empty,
        AppointmentStatusId = a.AppointmentStatusId,
        AppointmentStatusName = a.AppointmentStatus?.Name ?? string.Empty,
        StartDateTime = a.StartDateTime,
        EndDateTime = a.EndDateTime,
        Notes = a.Notes,
        CreatedAt = a.CreatedAt
    };
}
