using System;

namespace FleetRepairBot.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string VIN { get; set; }
        public string LicensePlate { get; set; }
        public string Model { get; set; }
    }
}
