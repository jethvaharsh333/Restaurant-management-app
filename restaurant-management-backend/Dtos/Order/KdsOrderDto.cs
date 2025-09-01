namespace restaurant_management_backend.Dtos.Order
{
    public class KdsOrderDto
    {
        public Guid OrderId { get; set; }
        public string OrderType { get; set; }
        public int? TableNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string SpecialInstructions { get; set; }
        public List<KdsOrderItemDto> Items { get; set; }
    }
}
