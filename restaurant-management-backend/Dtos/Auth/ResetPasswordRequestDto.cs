using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Auth
{
    public class ResetPasswordRequestDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
