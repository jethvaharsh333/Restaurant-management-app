using restaurant_management_backend.Models.OrderAndOperations;

namespace restaurant_management_backend.Dtos.Order
{
    public class OrderSummaryDto
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public OrderTypeEnum OrderType { get; set; }
    }
}
