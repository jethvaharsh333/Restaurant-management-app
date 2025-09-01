using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.UserAndStaff
{
    public class StaffScheduleModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid StaffScheduleId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime ShiftStartTime { get; set; }

        [Required]
        public DateTime ShiftEndTime { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUserModel User { get; set; }
    }
}
