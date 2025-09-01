using restaurant_management_backend.Models.OrderAndOperations;
using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Order
{
    public class CreateOrderDto
    {
        [Required]
        public Guid UserId { get; set; }

        public Guid? TableId { get; set; }

        [Required]
        public OrderTypeEnum OrderType { get; set; }

        public string? SpecialInstructions { get; set; }

        public CurrencyEnum Currency { get; set; } = CurrencyEnum.INR;

        [Required]
        [MinLength(1)]
        public List<CreateOrderItemDto> Items { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
