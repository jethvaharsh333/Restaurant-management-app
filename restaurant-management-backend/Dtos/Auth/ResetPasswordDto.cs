using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Auth
{
    public class ResetPasswordDto
    {
        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string Token { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
