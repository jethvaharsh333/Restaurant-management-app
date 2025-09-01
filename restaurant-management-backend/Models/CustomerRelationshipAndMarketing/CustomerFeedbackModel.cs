using restaurant_management_backend.Models.OrderAndOperations;
using restaurant_management_backend.Models.UserAndStaff;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.CustomerRelationshipAndMarketing
{
    public class CustomerFeedbackModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CustomerFeedbackId { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("OrderId")]
        public virtual OrderModel Order { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUserModel User { get; set; }
    }
}
