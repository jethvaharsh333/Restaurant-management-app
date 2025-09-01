namespace restaurant_management_backend.Dtos.Inventory
{
    public class SupplierDto
    {
        public Guid SupplierId { get; set; }
        public string Name { get; set; }
        public string? ContactInfo { get; set; }
    }
}
