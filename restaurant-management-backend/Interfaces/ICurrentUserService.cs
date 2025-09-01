using restaurant_management_backend.Models.UserAndStaff;

namespace restaurant_management_backend.Interfaces
{
    public interface ICurrentUserService
    {
        public Task<ApplicationUserModel?> GetUser();
    }
}
