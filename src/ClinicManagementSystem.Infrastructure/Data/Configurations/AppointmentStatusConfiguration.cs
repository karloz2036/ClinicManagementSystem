using ClinicManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.Infrastructure.Data.Configurations
{
    public class AppointmentStatusConfiguration : IEntityTypeConfiguration<AppointmentStatus>
    {
        public void Configure(EntityTypeBuilder<AppointmentStatus> builder)
        {
            builder.ToTable("AppointmentStatus");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(s => s.IsActive)
                .IsRequired();

            builder.HasIndex(s => s.Name).IsUnique();

            builder.HasMany(s => s.Appointments)
                .WithOne(a => a.AppointmentStatus)
                .HasForeignKey(a => a.AppointmentStatusId);
        }
    }
}
