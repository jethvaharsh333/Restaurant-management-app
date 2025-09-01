using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Inventory
{
    public class CreatePurchaseOrderItemDto
    {
        [Required]
        public Guid IngredientId { get; set; }

        [Required]
        [Range(0.1, 10000)]
        public decimal Quantity { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal Cost { get; set; }
    }
}
