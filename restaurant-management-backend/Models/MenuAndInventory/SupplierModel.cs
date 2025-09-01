using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.MenuAndInventory
{
    public class SupplierModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SupplierId { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [StringLength(255)]
        public string ContactInfo { get; set; }

        public virtual ICollection<PurchaseOrderModel> PurchaseOrders { get; set; }
    }
}
