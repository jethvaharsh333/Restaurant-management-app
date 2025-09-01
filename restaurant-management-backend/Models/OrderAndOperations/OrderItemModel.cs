using restaurant_management_backend.Models.MenuAndInventory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.OrderAndOperations
{
    public class OrderItemModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderItemId { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid MenuItemId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PriceAtTimeOfOrder { get; set; }

        [ForeignKey("OrderId")]
        public virtual OrderModel Order { get; set; }

        [ForeignKey("MenuItemId")]
        public virtual MenuItemModel MenuItem { get; set; }
    }
}
