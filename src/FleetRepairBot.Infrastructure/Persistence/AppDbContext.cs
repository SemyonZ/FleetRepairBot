using Microsoft.EntityFrameworkCore;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Infrastructure.Persistence
{
    // Lightweight AppDbContext for infrastructure-level registrations. For MVP it mirrors existing Data context.
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<RepairRequest> RepairRequests { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Dispatcher> Dispatchers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<RequestPhoto> RequestPhotos { get; set; }
        public DbSet<StatusHistory> StatusHistories { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RepairRequest>(e =>
            {
                e.HasKey(r => r.Id);
                e.Property(r => r.Title).HasMaxLength(250);
                e.Property(r => r.Description).HasMaxLength(4000);
                e.HasIndex(r => r.CreatedAt);
            });

            // Additional configuration can be split into separate configuration classes; keep minimal for MVP.
        }
    }
}
