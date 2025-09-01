using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.Setting
{
    public class RestaurantSettingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RestaurantSettingId { get; set; } // Will always be 1

        // Operating Hours (as strings like "18:00" for 6 PM)
        public string MondayOpen { get; set; }
        public string MondayClose { get; set; }
        public string TuesdayOpen { get; set; }
        public string TuesdayClose { get; set; }
        // ... and so on for all 7 days

        public bool IsClosedOnMonday { get; set; } = false;
        // ... and so on for all 7 days

        // Reservation Rules
        public int TurnTimeMinutes { get; set; } = 90; // How long a table is occupied
        public int MaxPartySizeOnline { get; set; } = 6;
    }
}
