using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.MenuAndInventory
{
    public class CategoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CategoryId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        public virtual ICollection<MenuItemModel> MenuItems { get; set; }


        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
