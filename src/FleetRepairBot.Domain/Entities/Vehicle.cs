using System;

namespace FleetRepairBot.Domain.Entities
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public string Vin { get; set; }
        public string Notes { get; set; }
    }
}
