using FleetRepairBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FleetRepairBot.Data
{
    public class FleetRepairDbContext : DbContext
    {
        public FleetRepairDbContext(DbContextOptions<FleetRepairDbContext> options) : base(options) { }

        public DbSet<RepairRequest> RepairRequests { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<RequestPhoto> RequestPhotos { get; set; }
        public DbSet<StatusHistory> StatusHistories { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RepairRequest>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Description).HasMaxLength(2000);
                b.OwnsMany<RequestPhoto>(
                    "Photos",
                    pb =>
                    {
                        pb.WithOwner().HasForeignKey("RepairRequestId");
                        pb.Property<Guid>("Id");
                    });
            });

            modelBuilder.Entity<RequestPhoto>(b =>
            {
                b.HasKey(x => x.Id);
            });

            modelBuilder.Entity<StatusHistory>(b => b.HasKey(x => x.Id));
            modelBuilder.Entity<Driver>(b => b.HasKey(x => x.Id));
            modelBuilder.Entity<Vehicle>(b => b.HasKey(x => x.Id));
            modelBuilder.Entity<AuditLog>(b => b.HasKey(x => x.Id));

            // store enum as int by default
            base.OnModelCreating(modelBuilder);
        }
    }
}
