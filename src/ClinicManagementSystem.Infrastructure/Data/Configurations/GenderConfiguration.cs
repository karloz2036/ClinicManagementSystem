using ClinicManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.Infrastructure.Data.Configurations
{
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.ToTable("Genders");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .HasMaxLength(25)
                .IsRequired();

            builder.Property(g => g.IsActive)
                .IsRequired();

            builder.HasIndex(g => g.Name).IsUnique();

            builder.HasMany(g => g.Patients)
                .WithOne(p => p.Gender)
                .HasForeignKey(p => p.GenderId);
        }
    }
}
