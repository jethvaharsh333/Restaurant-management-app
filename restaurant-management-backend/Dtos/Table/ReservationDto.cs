using restaurant_management_backend.Models.OrderAndOperations;

namespace restaurant_management_backend.Dtos.Table
{
    public class ReservationDto
    {
        public Guid ReservationId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TableId { get; set; }
        public DateTime ReservationTime { get; set; }
        public int PartySize { get; set; }
        public ReservationStatusEnum Status { get; set; }
        public string CustomerName { get; set; }
        public int? TableNumber { get; set; }
    }
}
