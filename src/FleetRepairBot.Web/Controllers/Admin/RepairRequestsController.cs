using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FleetRepairBot.Data;
using FleetRepairBot.Application.Interfaces;

namespace FleetRepairBot.Web.Controllers.Admin
{
    [Route("admin/repairrequests")]
    public class RepairRequestsController : Controller
    {
        private readonly FleetRepairDbContext _db;
        private readonly IRepairRequestService _service;
        private readonly ILogger<RepairRequestsController> _logger;

        public RepairRequestsController(FleetRepairDbContext db, IRepairRequestService service, ILogger<RepairRequestsController> logger)
        {
            _db = db;
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = await _db.RepairRequests.Include(r => r.Status).Include(r => r.Vehicle).OrderByDescending(r => r.CreatedAt).Select(r => new
            {
                r.Id,
                r.Title,
                r.Description,
                Vehicle = r.Vehicle != null ? r.Vehicle.LicensePlate : null,
                Status = r.Status != null ? r.Status.Name : null,
                r.CreatedAt,
                r.UpdatedAt
            }).ToListAsync();

            return Ok(list);
        }

        [HttpPost("{id}/changestatus")] 
        public async Task<IActionResult> ChangeStatus(int id, [FromQuery]int statusId)
        {
            try
            {
                // Placeholder performedByTelegramId = 0 for admin action
                await _service.ChangeStatusAsync(id, statusId, null, 0, "Changed via admin", default);
                return Ok(new { success = true });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to change status for request {Id}", id);
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
