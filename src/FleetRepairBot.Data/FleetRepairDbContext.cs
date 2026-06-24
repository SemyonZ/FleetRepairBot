using Microsoft.EntityFrameworkCore;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Data
{
    public class FleetRepairDbContext : DbContext
    {
        public FleetRepairDbContext(DbContextOptions<FleetRepairDbContext> options) : base(options) { }

        public DbSet<RepairRequest> RepairRequests { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Dispatcher> Dispatchers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<RequestPhoto> RequestPhotos { get; set; }
        public DbSet<StatusHistory> StatusHistories { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Driver>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.TelegramId).IsRequired();
            });

            modelBuilder.Entity<Dispatcher>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.TelegramId).IsRequired();
            });

            modelBuilder.Entity<Vehicle>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.RegistrationNumber).HasMaxLength(64);
            });

            modelBuilder.Entity<RepairRequest>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Title).HasMaxLength(200);
                b.HasOne(x => x.Vehicle).WithMany().HasForeignKey(x => x.VehicleId).OnDelete(DeleteBehavior.SetNull);
                b.HasOne(x => x.Driver).WithMany().HasForeignKey(x => x.DriverId).OnDelete(DeleteBehavior.SetNull);
                b.HasOne(x => x.CreatedByDispatcher).WithMany().HasForeignKey(x => x.CreatedByDispatcherId).OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<RequestPhoto>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(x => x.RepairRequest).WithMany(r => r.Photos).HasForeignKey(x => x.RepairRequestId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<StatusHistory>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(x => x.RepairRequest).WithMany(r => r.StatusHistories).HasForeignKey(x => x.RepairRequestId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AuditLog>(b =>
            {
                b.HasKey(x => x.Id);
            });
        }
    }
}
