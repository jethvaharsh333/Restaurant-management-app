using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Menu
{
    public class UpsertCategoryRequestDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
