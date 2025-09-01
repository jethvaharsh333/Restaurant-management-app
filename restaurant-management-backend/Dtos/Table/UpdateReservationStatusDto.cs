using restaurant_management_backend.Models.OrderAndOperations;
using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Table
{
    public class UpdateReservationStatusDto
    {
        [Required]
        public ReservationStatusEnum NewStatus { get; set; }

        public Guid? TableId { get; set; } = null;
    }
}
