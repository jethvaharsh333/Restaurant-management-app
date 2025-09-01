namespace restaurant_management_backend.Dtos.Inventory
{
    public class PurchaseOrderDto
    {
        public Guid PurchaseOrderId { get; set; }
        public Guid SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalCost { get; set; }
        public List<PurchaseOrderItemDto> Items { get; set; }
    }
}
