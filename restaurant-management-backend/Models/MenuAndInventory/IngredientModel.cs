using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.MenuAndInventory
{
    public class IngredientModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid IngredientId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, Column(TypeName = "decimal(10, 2)")]
        public decimal StockQuantity { get; set; }

        [Required]
        public UnitOfMeasureEnum UnitOfMeasure { get; set; }

        [Required, Column(TypeName = "decimal(10, 2)")]
        public decimal LowStockThreshold { get; set; }

        public virtual ICollection<MenuItemIngredientModel> MenuItemIngredients { get; set; }
        public virtual ICollection<PurchaseOrderItemModel> PurchaseOrderItems { get; set; }
    }

    public enum UnitOfMeasureEnum
    {
        Piece = 1,
        Dozen = 2,
        Slice = 3,
        Cup = 4,
        Bottle = 5,
        Can = 6,

        Gram = 10,
        Kilogram = 11,
        Pound = 12,
        Ounce = 13,

        Milliliter = 20,
        Liter = 21,
        Teaspoon = 22,
        Tablespoon = 23,
        Gallon = 24,

        Pack = 30,
        Box = 31,
        Tray = 32
    }

}
