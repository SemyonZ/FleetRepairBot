namespace FleetRepairBot.Application.Dto
{
    public class RepairRequestCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        // optional vehicle VIN or id; for MVP keep it simple
        public int? VehicleId { get; set; }
        // Telegram id of reporter
        public long ReporterTelegramId { get; set; }
    }
}
