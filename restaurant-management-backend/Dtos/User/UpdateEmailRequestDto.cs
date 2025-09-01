using System.ComponentModel.DataAnnotations;

namespace restaurant_management_backend.Dtos.User
{
    public class UpdateEmailRequestDto
    {
        [Required, EmailAddress] 
        public string NewEmail { get; set; }
    }
}
