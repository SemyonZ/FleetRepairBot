using System;

namespace FleetRepairBot.Domain.Entities
{
    public class Driver
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }

        protected Driver() { }

        public Driver(Guid id, string name, string phone, string email)
        {
            Id = id == default ? Guid.NewGuid() : id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Phone = phone ?? string.Empty;
            Email = email ?? string.Empty;
        }

        public void UpdateContact(string phone, string email)
        {
            Phone = phone ?? Phone;
            Email = email ?? Email;
        }
    }
}
