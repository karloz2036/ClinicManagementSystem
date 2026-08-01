using ClinicManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.Infrastructure.Data.Configurations
{
    public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.ToTable("Specialties");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(s => s.Description)
                .HasMaxLength(500);

            builder.Property(s => s.IsActive)
                .IsRequired();

            builder.HasIndex(s => s.Name).IsUnique();

            builder.HasMany(s => s.DoctorSpecialties)
                .WithOne(ds => ds.Specialty)
                .HasForeignKey(ds => ds.SpecialtyId);
        }
    }
}
