using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.User
{
    public class ChangePasswordRequestDto
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
    }
}
