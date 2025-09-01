using restaurant_management_backend.Models.OrderAndOperations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Dtos.Menu
{
    public class UpsertMenuItemRequestDto
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required, Range(0, 100000)]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Cost { get; set; } = 0.00m;

        public CurrencyEnum Currency { get; set; } = CurrencyEnum.INR;

        public string ImageUrl { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<MenuItemIngredientDto> Ingredients { get; set; } = new List<MenuItemIngredientDto>();
    }
}
