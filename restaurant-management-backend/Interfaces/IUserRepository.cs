using restaurant_management_backend.Dtos.User;
using restaurant_management_backend.Models.UserAndStaff;
using restaurant_management_backend.Utility;
using System.Security.Claims;

namespace restaurant_management_backend.Interfaces
{
    public interface IUserRepository
    {
        Task<ApiResponse<CurrentUserResponseDto>> GetCurrentUserAsync(ClaimsPrincipal user);

        Task<ApiResponse<object>> UpdateProfileAsync(UpdateProfileRequestDto dto);

        Task<ApiResponse<object>> UpdateAvatarAsync(UpdateAvatarRequestDto dto);
        
        Task<ApiResponse<(string NewEmail, string Token)>> UpdateEmailAsync(UpdateEmailRequestDto dto);

        Task<ApiResponse<object>> ChangePasswordAsync(ChangePasswordRequestDto dto);

        Task<ApiResponse<object>> GetAllUsersAsync();

        Task<ApiResponse<UserDto>> GetUserByIdAsync(Guid userId);

        Task<ApiResponse<object>> SetRoleAsync(SetRoleRequestDto dto);

        Task<ApiResponse<object>> DeleteUserByIdAsync(Guid userId);
    }
}
