using System;

namespace FleetRepairBot.Domain.Entities
{
    public class Vehicle
    {
        public Guid Id { get; private set; }
        public string PlateNumber { get; private set; }
        public string Model { get; private set; }

        protected Vehicle() { }

        public Vehicle(Guid id, string plateNumber, string model)
        {
            Id = id == default ? Guid.NewGuid() : id;
            PlateNumber = plateNumber ?? throw new ArgumentNullException(nameof(plateNumber));
            Model = model ?? string.Empty;
        }

        public void UpdateModel(string model)
        {
            Model = model ?? Model;
        }
    }
}
