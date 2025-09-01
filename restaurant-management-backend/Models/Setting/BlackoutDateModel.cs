using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.Setting
{
    public class BlackoutDateModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BlackoutDateId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [StringLength(255)]
        public string Description { get; set; } // e.g., "Private Event", "Diwali Holiday"
    }
}
