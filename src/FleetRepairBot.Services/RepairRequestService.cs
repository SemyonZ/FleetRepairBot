using System.Collections.Generic;
using System.Threading.Tasks;
using FleetRepairBot.Data.Repositories;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Services
{
    public class RepairRequestService : IRepairRequestService
    {
        private readonly IRepairRequestRepository _repo;
        public RepairRequestService(IRepairRequestRepository repo) { _repo = repo; }

        public Task CreateAsync(RepairRequest request) => _repo.AddAsync(request);
        public Task<RepairRequest> GetAsync(int id) => _repo.GetByIdAsync(id);
        public Task<IEnumerable<RepairRequest>> ListAsync() => _repo.GetAllAsync();
        public Task UpdateAsync(RepairRequest request) => _repo.UpdateAsync(request);
    }
}
