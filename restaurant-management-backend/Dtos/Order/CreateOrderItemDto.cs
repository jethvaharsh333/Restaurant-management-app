using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Order
{
    public class CreateOrderItemDto
    {
        [Required]
        public Guid MenuItemId { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

    }
}
