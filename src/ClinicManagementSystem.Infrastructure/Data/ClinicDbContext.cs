using Microsoft.EntityFrameworkCore;
using ClinicManagementSystem.Domain.Entities;

namespace ClinicManagementSystem.Infrastructure.Data;

public class ClinicDbContext : DbContext
{
    public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
        : base(options)
    {
    }

    public DbSet<Gender> Genders => Set<Gender>();

    public DbSet<Patient> Patients => Set<Patient>();

    public DbSet<Doctor> Doctors => Set<Doctor>();

    public DbSet<Specialty> Specialties => Set<Specialty>();

    public DbSet<DoctorSpecialty> DoctorSpecialties => Set<DoctorSpecialty>();

    public DbSet<ConsultingRoom> ConsultingRooms => Set<ConsultingRoom>();

    public DbSet<AppointmentStatus> AppointmentStatuses => Set<AppointmentStatus>();

    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClinicDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }


}
