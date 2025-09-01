using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.Setting
{
    public class ReservationSettingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ReservationSettingId { get; set; }

        [Required]
        public DateTime ReservationStartDate { get; set; }

        [Required]
        public DateTime ReservationEndDate { get; set; }
    }
}
