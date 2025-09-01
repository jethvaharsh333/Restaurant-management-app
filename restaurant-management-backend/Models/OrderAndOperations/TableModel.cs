using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.OrderAndOperations
{
    public class TableModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TableId { get; set; }

        [Required]
        public int TableNumber { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public TableStatusEnum Status { get; set; }

        [StringLength(255)]
        public string QrCodeUrl { get; set; }

        // Navigation
        public virtual ICollection<ReservationModel> Reservations { get; set; }
        public virtual ICollection<OrderModel> Orders { get; set; }
    }

    public enum TableStatusEnum
    {
        Available = 1,
        Occupied = 2,
        Reserved = 3,
    }
}

/*
 
Imagine a customer, Karan, has a reservation for 7:30 PM.
    1) At 7:25 PM, the host sees the upcoming booking and updates Table #4's status 
        to Reserved using PUT /api/tables/4/status.
    2) At 7:30 PM, Karan arrives. The host finds his booking in the system and updates 
        the reservation's status to Seated using PUT /api/reservations/1/status.
    3) This action in the system automatically triggers another call to update Table 
        #4's status to Occupied using PUT /api/tables/4/status.
 
 */