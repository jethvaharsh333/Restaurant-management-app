namespace restaurant_management_backend.Dtos.Order
{
    public class OrderItemDto
    {
        public string MenuItemName { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtTimeOfOrder { get; set; }
    }
}
