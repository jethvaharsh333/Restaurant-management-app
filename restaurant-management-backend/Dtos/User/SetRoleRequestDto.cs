using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.User
{
    public class SetRoleRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid RoleId { get; set; }
    }
}
