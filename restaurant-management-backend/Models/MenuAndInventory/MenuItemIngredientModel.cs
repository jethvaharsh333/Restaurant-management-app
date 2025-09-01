using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.MenuAndInventory
{
    public class MenuItemIngredientModel
    {
        public Guid MenuItemId { get; set; }
        [ForeignKey("MenuItemId")]
        public virtual MenuItemModel MenuItem { get; set; }

        public Guid IngredientId { get; set; }

        [ForeignKey("IngredientId")]
        public virtual IngredientModel Ingredient { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal QuantityUsed { get; set; }
    }
}
