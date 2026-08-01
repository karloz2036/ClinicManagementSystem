using ClinicManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.Infrastructure.Data.Configurations
{
    public class ConsultingRoomConfiguration : IEntityTypeConfiguration<ConsultingRoom>
    {
        public void Configure(EntityTypeBuilder<ConsultingRoom> builder)
        {
            builder.ToTable("ConsultingRooms");

            builder.HasKey(cr => cr.Id);

            builder.Property(cr => cr.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(cr => cr.Location)
                .HasMaxLength(150);

            builder.Property(cr => cr.IsActive)
                .IsRequired();

            builder.HasIndex(cr => cr.Name).IsUnique();

            builder.HasMany(cr => cr.Appointments)
                .WithOne(a => a.ConsultingRoom)
                .HasForeignKey(a => a.ConsultingRoomId);
        }
    }
}
