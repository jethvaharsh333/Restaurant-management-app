using Microsoft.AspNetCore.Identity;
using restaurant_management_backend.Models.UserAndStaff;

namespace restaurant_management_backend.Interfaces
{
    public interface IInfrastructureRepository
    {
        Task<string> GenerateToken(ApplicationUserModel user);

        Task<(bool success, string message)> SendEmailAsync(string toEmail, string subject, string body);
    }
}
