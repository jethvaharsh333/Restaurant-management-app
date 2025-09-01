namespace restaurant_management_backend.Dtos.Inventory
{
    public class PurchaseOrderItemDto
    {
        public Guid IngredientId { get; set; }
        public string IngredientName { get; set; }
        public decimal Quantity { get; set; }
    }
}
