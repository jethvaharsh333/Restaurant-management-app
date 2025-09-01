using restaurant_management_backend.Models.OrderAndOperations;
using restaurant_management_backend.Models.UserAndStaff;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.MenuAndInventory
{
    public class MenuItemModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MenuItemId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; } = "";

        [Required, Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Cost { get; set; } = 0.00m;

        public CurrencyEnum Currency { get; set; } = CurrencyEnum.INR;

        [Required]
        public string ImageUrl { get; set; } = "https://images.pexels.com/photos/376464/pexels-photo-376464.jpeg";

        public bool IsAvailable { get; set; } = true;

        [ForeignKey("CategoryId")]
        public virtual CategoryModel Category { get; set; }
        public virtual ICollection<MenuItemIngredientModel> MenuItemIngredients { get; set; }
        public virtual ICollection<OrderItemModel> OrderItems { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid ModifiedBy { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual ApplicationUserModel User { get; set; }
    }
}
