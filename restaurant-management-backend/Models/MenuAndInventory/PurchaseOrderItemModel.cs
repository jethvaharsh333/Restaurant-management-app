using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.MenuAndInventory
{
    public class PurchaseOrderItemModel
    {
        public Guid PurchaseOrderId { get; set; }
        public Guid IngredientId { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Cost { get; set; } // Cost per unit at time of purchase

        // Navigation Properties
        public virtual PurchaseOrderModel PurchaseOrder { get; set; }
        public virtual IngredientModel Ingredient { get; set; }
    }
}
