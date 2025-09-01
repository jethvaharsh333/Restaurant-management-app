namespace restaurant_management_backend.Dtos.Order
{
    public class OrderDetailDto : OrderSummaryDto
    {
        public string CustomerName { get; set; }
        public int? TableNumber { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
