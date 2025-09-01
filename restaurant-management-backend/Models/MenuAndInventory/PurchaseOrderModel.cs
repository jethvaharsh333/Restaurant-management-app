using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.MenuAndInventory
{
    public class PurchaseOrderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PurchaseOrderId { get; set; }

        [Required]
        public Guid SupplierId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required, StringLength(50)]
        public PurchaseOrderStatusEnum Status { get; set; }

        [Required, Column(TypeName = "decimal(10, 2)")]
        public decimal TotalCost { get; set; }

        
        [ForeignKey("SupplierId")]
        public virtual SupplierModel Supplier { get; set; }

        public virtual ICollection<PurchaseOrderItemModel> Items { get; set; }
    }

    public enum PurchaseOrderStatusEnum
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Completed = 3
    }

}
