using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.CustomerRelationshipAndMarketing
{
    public class PromotionsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PromotionsId { get; set; }

        [Required, StringLength(50)]
        public string Code { get; set; }

        [Required, StringLength(255)]
        public string Description { get; set; }

        [Required, Column(TypeName = "decimal(5, 2)")]
        public decimal DiscountPercentage { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        [Required]
        public DayEnum ApplicableDaysOfWeek { get; set; } // e.g., "Monday,Tuesday,Wednesday"
    }

    [Flags]
    public enum DayEnum
    {
        None = 0,   // 0000000
        Monday = 1,   // 0000001
        Tuesday = 2,   // 0000010
        Wednesday = 4,   // 0000100
        Thursday = 8,   // 0001000
        Friday = 16,  // 0010000
        Saturday = 32,  // 0100000
        Sunday = 64   // 1000000
    }

}
