using ClinicManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.Infrastructure.Data.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.StartDateTime)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(a => a.EndDateTime)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(a => a.Notes)
                .HasMaxLength(1000);

            builder.Property(a => a.CreatedAt)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId);

            builder.HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId);

            builder.HasOne(a => a.ConsultingRoom)
                .WithMany(cr => cr.Appointments)
                .HasForeignKey(a => a.ConsultingRoomId);

            builder.HasOne(a => a.AppointmentStatus)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.AppointmentStatusId);

            builder.HasIndex(a => new { a.DoctorId, a.StartDateTime, a.EndDateTime });
            builder.HasIndex(a => new { a.ConsultingRoomId, a.StartDateTime, a.EndDateTime });
        }
    }
}
