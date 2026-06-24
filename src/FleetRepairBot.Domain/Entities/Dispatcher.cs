using System;

namespace FleetRepairBot.Domain.Entities
{
    public class Dispatcher
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Contact { get; private set; }

        protected Dispatcher() { }

        public Dispatcher(Guid id, string name, string contact)
        {
            Id = id == default ? Guid.NewGuid() : id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Contact = contact ?? string.Empty;
        }
    }
}
