using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Inventory
{
    public class CreateSupplierDto
    {
        [Required, StringLength(150)]
        public string Name { get; set; }

        [StringLength(255)]
        public string? ContactInfo { get; set; }
    }
}
