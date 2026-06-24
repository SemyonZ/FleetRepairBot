using Microsoft.EntityFrameworkCore;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Data
{
    public class FleetRepairDbContext : DbContext
    {
        public FleetRepairDbContext(DbContextOptions<FleetRepairDbContext> options) : base(options) { }

        // DbSets for all entities used by repositories and the application
        public DbSet<RepairRequest> RepairRequests { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Dispatcher> Dispatchers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<RequestPhoto> RequestPhotos { get; set; }
        public DbSet<StatusHistory> StatusHistories { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Убираем провайдер-специфичную настройку для PerformedAt, чтобы не полагаться на GETUTCDATE()
            // Предполагается, что AuditLog.PerformedAt инициализируется в C# (например, property = DateTime.UtcNow) или задаётся при создании сущности.

            // Здесь можно добавить дополнительные конфигурации сущностей (индексы, ограничения и т.д.),
            // если это понадобится в будущем.
        }
    }
}
