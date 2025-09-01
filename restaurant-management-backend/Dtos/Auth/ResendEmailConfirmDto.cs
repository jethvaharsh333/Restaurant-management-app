using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Auth
{
    public class ResendEmailConfirmDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
