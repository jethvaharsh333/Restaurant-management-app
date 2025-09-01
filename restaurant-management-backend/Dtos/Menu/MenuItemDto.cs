using restaurant_management_backend.Models.OrderAndOperations;

namespace restaurant_management_backend.Dtos.Menu
{
    public class MenuItemDto
    {
        public Guid MenuItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public CurrencyEnum Currency { get; set; } = CurrencyEnum.INR;
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

    }
}
