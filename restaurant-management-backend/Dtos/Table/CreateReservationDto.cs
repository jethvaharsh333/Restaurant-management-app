using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Table
{
    public class CreateReservationDto
    {
        [Required]
        public DateTime ReservationTime { get; set; }

        [Required]
        [Range(1, 20)]
        public int PartySize { get; set; }

        public DateTime CreatedAt = DateTime.UtcNow;
    }
}
