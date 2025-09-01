using restaurant_management_backend.Models.MenuAndInventory;
using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Inventory
{
    public class UpsertIngredientRequestDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal StockQuantity { get; set; }

        [Required]
        public UnitOfMeasureEnum UnitOfMeasure { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal LowStockThreshold { get; set; }
    }
}
