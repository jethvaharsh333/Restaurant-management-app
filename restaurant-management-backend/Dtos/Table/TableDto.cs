namespace restaurant_management_backend.Dtos.Table
{
    public class TableDto
    {
        public Guid TableId { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
    }
}
