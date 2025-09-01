using restaurant_management_backend.Models.CustomerRelationshipAndMarketing;
using restaurant_management_backend.Models.UserAndStaff;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.OrderAndOperations
{
    public class OrderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public Guid? TableId { get; set; }

        [Required]
        public OrderStatusEnum OrderStatus { get; set; }

        [Required]
        public OrderTypeEnum OrderType { get; set; }

        [Required]
        public CurrencyEnum Currency { get; set; } = CurrencyEnum.INR;

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string? SpecialInstructions { get; set; }


        [ForeignKey("UserId")]
        public virtual ApplicationUserModel User { get; set; }

        [ForeignKey("TableId")]
        public virtual TableModel Table { get; set; }

        public virtual ICollection<OrderItemModel> OrderItems { get; set; }
        public virtual ICollection<PaymentModel> Payments { get; set; }
        public virtual DeliveryModel Delivery { get; set; }
        public virtual ICollection<CustomerFeedbackModel> Feedbacks { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }

    public enum OrderTypeEnum
    {
        DineIn = 1,
        Takeaway = 2,
        Delivery = 3
    }

    public enum OrderStatusEnum
    {
        Pending = 1,
        Confirmed = 2,
        Preparing = 3,
        Ready = 4,
        Completed = 5,
        Cancelled = 6
    }
}
