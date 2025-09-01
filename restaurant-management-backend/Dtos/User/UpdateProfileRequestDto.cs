using restaurant_management_backend.Models.UserAndStaff;

namespace restaurant_management_backend.Dtos.User
{
    public class UpdateProfileRequestDto
    {
        public string Username { get; set; }

        public string? PhoneNumber { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public GenderEnum? Gender { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }

        public string? PreferredLanguage { get; set; }
    }
}
