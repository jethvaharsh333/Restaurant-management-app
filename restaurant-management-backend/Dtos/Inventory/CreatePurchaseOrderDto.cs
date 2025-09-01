using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Inventory
{
    public class CreatePurchaseOrderDto
    {
        [Required]
        public Guid SupplierId { get; set; }

        [Required]
        [MinLength(1)]
        public List<CreatePurchaseOrderItemDto> Items { get; set; }
    }
}
