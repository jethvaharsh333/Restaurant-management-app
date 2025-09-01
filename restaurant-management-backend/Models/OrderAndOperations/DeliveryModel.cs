using restaurant_management_backend.Models.UserAndStaff;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.OrderAndOperations
{
    public class DeliveryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DeliveryId { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        public Guid? DeliveryPersonId { get; set; }

        [Required]
        public DeliveryStatusEnum DeliveryStatus { get; set; }

        public DateTime? EstimatedDeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }

        // Navigation
        [ForeignKey("OrderId")]
        public virtual OrderModel Order { get; set; }

        [ForeignKey("DeliveryPersonId")]
        public virtual ApplicationUserModel DeliveryPerson { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }

    public enum DeliveryStatusEnum
    {
        PendingAssignment = 1,
        DriverAssigned = 2,
        OutForDelivery = 3,
        Delivered = 4,
        Failed = 5
    }
}
