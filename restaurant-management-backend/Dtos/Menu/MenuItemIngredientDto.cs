using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Menu
{
    public class MenuItemIngredientDto
    {
        [Required]
        public Guid IngredientId { get; set; }

        [Required]
        [Range(0.01, 1000)]
        public decimal QuantityUsed { get; set; }
    }
}
