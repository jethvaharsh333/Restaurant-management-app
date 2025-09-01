using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Table
{
    public class UpsertTableRequestDto
    {
        [Required]
        [Range(1, 999)]
        public int TableNumber { get; set; }

        [Required]
        [Range(1, 20)]
        public int Capacity { get; set; }

        [StringLength(255)]
        public string QrCodeUrl { get; set; }
    }
}
