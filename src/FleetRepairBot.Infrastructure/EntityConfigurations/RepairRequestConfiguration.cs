using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Infrastructure.EntityConfigurations
{
    public class RepairRequestConfiguration : IEntityTypeConfiguration<RepairRequest>
    {
        public void Configure(EntityTypeBuilder<RepairRequest> builder)
        {
            builder.ToTable("RepairRequests");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Title).HasMaxLength(250).IsRequired(false);
            builder.Property(r => r.Description).HasMaxLength(4000).IsRequired(false);
            builder.Property(r => r.CreatedAt).IsRequired();
            builder.Property(r => r.UpdatedAt).IsRequired(false);
            builder.HasOne(r => r.Vehicle).WithMany().HasForeignKey(r => r.VehicleId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(r => r.Driver).WithMany().HasForeignKey(r => r.DriverId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(r => r.Dispatcher).WithMany().HasForeignKey(r => r.DispatcherId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(r => r.Status).WithMany().HasForeignKey(r => r.StatusId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
