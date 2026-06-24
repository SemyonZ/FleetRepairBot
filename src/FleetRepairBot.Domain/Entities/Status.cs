using System;

namespace FleetRepairBot.Domain.Entities
{
    // Status entity to allow history tracking and seeding
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Common statuses
        public const string New = "New";
        public const string InProgress = "InProgress";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
    }
}
