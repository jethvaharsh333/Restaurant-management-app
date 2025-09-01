using restaurant_management_backend.Models.MenuAndInventory;

namespace restaurant_management_backend.Dtos.Inventory
{
    public class IngredientDto
    {
        public Guid IngredientId { get; set; }
        public string Name { get; set; }
        public decimal StockQuantity { get; set; }
        public UnitOfMeasureEnum UnitOfMeasure { get; set; }
        public decimal LowStockThreshold { get; set; }

    }
}
