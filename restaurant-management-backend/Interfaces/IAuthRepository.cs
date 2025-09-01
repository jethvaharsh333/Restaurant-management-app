using Microsoft.AspNetCore.Identity.Data;
using restaurant_management_backend.Dtos.Auth;
using restaurant_management_backend.Utility;

namespace restaurant_management_backend.Interfaces
{
    public interface IAuthRepository
    {
        Task<(string, ApiResponse<LoginResponseDto>)> LoginAsync(LoginRequestDto dto);

        Task<ApiResponse<(string Email, string Token)>> RegisterCustomerAsync(RegisterRequestDto dto);
        
        Task<ApiResponse<object>> RegisterWithRoleAsync(RegisterWithRoleRequestDto dto);

        Task<ApiResponse<object>> ConfirmEmailAsync(string email, string token);

        Task<ApiResponse<(string Email, string Token)>> ResendEmailConfirmAsync(ResendEmailConfirmDto dto);

        Task<ApiResponse<object>> ResetPasswordRequest(ResetPasswordRequestDto dto);

        Task<ApiResponse<object>> ResetPassword(ResetPasswordDto dto);
    }
}
