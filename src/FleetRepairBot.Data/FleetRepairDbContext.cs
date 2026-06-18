using Microsoft.EntityFrameworkCore;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Data
{
    public class FleetRepairDbContext : DbContext
    {
        public FleetRepairDbContext(DbContextOptions<FleetRepairDbContext> options) : base(options) { }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Dispatcher> Dispatchers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<RepairRequest> RepairRequests { get; set; }
        public DbSet<RequestPhoto> RequestPhotos { get; set; }
        public DbSet<StatusHistory> StatusHistories { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}
