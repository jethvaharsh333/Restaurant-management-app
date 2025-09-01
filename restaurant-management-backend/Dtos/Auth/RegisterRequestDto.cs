using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
