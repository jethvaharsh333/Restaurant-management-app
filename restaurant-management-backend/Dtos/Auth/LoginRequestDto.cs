using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Auth
{
    public class LoginRequestDto
    {
        [Required]
        public string Identifier { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
