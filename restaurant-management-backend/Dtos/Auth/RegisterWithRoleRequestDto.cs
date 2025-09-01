using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.Auth
{
    public class RegisterWithRoleRequestDto
    {
        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public Guid RoleId { get; set; }
    }
}
