using restaurant_management_backend.Models.OrderAndOperations;
using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Table
{
    public class UpdateTableStatusDto
    {
        [Required]
        public TableStatusEnum Status { get; set; }
    }
}
