using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Inventory
{
    public class UpdateIngredientStockDto
    {
        [Required]
        [Range(0, 10000)]
        public decimal NewStockQuantity { get; set; }
    }
}
