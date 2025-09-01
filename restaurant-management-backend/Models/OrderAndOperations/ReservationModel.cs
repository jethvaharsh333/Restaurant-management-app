using restaurant_management_backend.Models.UserAndStaff;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.OrderAndOperations
{
    public class ReservationModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ReservationId { get; set; }

        [Required]
        public Guid UserId { get; set; }
        
        public Guid? TableId { get; set; }

        [Required]
        public DateTime ReservationTime { get; set; }

        [Required]
        public int PartySize { get; set; }

        [Required]
        public ReservationStatusEnum Status { get; set; }

        // Navigation
        [ForeignKey("UserId")]
        public virtual ApplicationUserModel User { get; set; }
        
        [ForeignKey("TableId")]
        public virtual TableModel Table { get; set; }

        public DateTime CreatedAt;

        public DateTime ModifiedAt = DateTime.UtcNow;
    }

    public enum ReservationStatusEnum
    {
        Pending = 1,    // Customer has requested, not yet confirmed
        Confirmed = 2,  // Restaurant has confirmed the booking
        Seated = 3,     // Customer has arrived and is at the table
        Cancelled = 4,  // Customer cancelled the booking
        NoShow = 5      // Customer did not arrive for their booking
    }   
}
