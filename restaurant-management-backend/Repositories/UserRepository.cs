using AutoMapper;
using Microsoft.AspNetCore.Identity;
using restaurant_management_backend.Data;
using restaurant_management_backend.Dtos.User;
using restaurant_management_backend.Interfaces;
using restaurant_management_backend.Models.UserAndStaff;
using restaurant_management_backend.Utility;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;

namespace restaurant_management_backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<ApplicationRoleModel> _roleManager;

        public UserRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUserModel> userManager, IMapper mapper, RoleManager<ApplicationRoleModel> roleManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<ApiResponse<CurrentUserResponseDto>> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            if (user == null || !user.Identity.IsAuthenticated)
                return ApiResponse<CurrentUserResponseDto>.FailureResponse("User is not authenticated.", 401);

            var email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var dto = new CurrentUserResponseDto
            {
                Username = user.Identity?.Name ?? userId,
                Email = email,
                Role = role
            };

            return ApiResponse<CurrentUserResponseDto>.SuccessResponse(dto, "Current user retrieved successfully!");
        }
        
        public async Task<ApiResponse<object>> UpdateProfileAsync(UpdateProfileRequestDto dto)
        {
            var user = await GetCurrentUserHelperAsync();
            if (user == null)
                return ApiResponse<object>.FailureResponse("User not found.", 404);

            _mapper.Map(dto, user);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ApiResponse<object>.FailureResponse($"Failed to update profile: {errors}", 400);
            }

            return ApiResponse<object>.SuccessResponse(null, "Profile updated successfully.");
        }

        public async Task<ApiResponse<object>> UpdateAvatarAsync(UpdateAvatarRequestDto dto)
        {
            var user = await GetCurrentUserHelperAsync();
            if (user == null)
                return ApiResponse<object>.FailureResponse("User not found.", 404);

            user.ProfilePictureUrl = dto.ProfilePictureUrl;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ApiResponse<object>.FailureResponse($"Failed to update profile: {errors}", 400);
            }

            return ApiResponse<object>.SuccessResponse(null, "Avatar changed successfully.");
        }
        
        public async Task<ApiResponse<(string NewEmail, string Token)>> UpdateEmailAsync(UpdateEmailRequestDto dto)
        {
            var user = await GetCurrentUserHelperAsync();
            if (user == null)
                return ApiResponse<(string, string)>.FailureResponse("User not found.", 404);

            var newEmail = dto.NewEmail.Trim();
            if (string.Equals(user.Email, newEmail, StringComparison.OrdinalIgnoreCase))
                return ApiResponse<(string, string)>.FailureResponse("The new email cannot be the same as the current email.");

            var existingUser = await _userManager.FindByEmailAsync(newEmail);
            if (existingUser != null)
                return ApiResponse<(string, string)>.FailureResponse("This email address is already associated with another account.");

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);

            return ApiResponse<(string, string)>.SuccessResponse((newEmail, token));
        }
        
        public async Task<ApiResponse<object>> ChangePasswordAsync(ChangePasswordRequestDto dto)
        {
            var user = await GetCurrentUserHelperAsync();
            if (user == null)
                return ApiResponse<object>.FailureResponse("User not found.", 404);

            var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ApiResponse<object>.FailureResponse($"Password change failed: {errors}", 400);
            }

            return ApiResponse<object>.SuccessResponse(null, "Password changed successfully.");
        }

        public async Task<ApiResponse<object>> DeleteUserByIdAsync(Guid userId)
        {
            var userToDelete = await _userManager.FindByIdAsync(userId.ToString());
            if (userToDelete == null)
                return ApiResponse<object>.FailureResponse($"User with ID {userId} not found.", 404);

            var currentAdminId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userToDelete.Id.ToString().Equals(currentAdminId, StringComparison.OrdinalIgnoreCase))
                return ApiResponse<object>.FailureResponse("Admins cannot delete their own account.", 400);

            var result = await _userManager.DeleteAsync(userToDelete);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ApiResponse<object>.FailureResponse($"Failed to delete user: {errors}", 500);
            }

            return ApiResponse<object>.SuccessResponse(null, $"User {userToDelete.UserName} has been successfully deleted.");
        }

        public Task<ApiResponse<object>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<UserDto>> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return ApiResponse<UserDto>.FailureResponse("User not found.", 404);

            var userDto = _mapper.Map<UserDto>(user);

            userDto.Roles = await _userManager.GetRolesAsync(user);

            return ApiResponse<UserDto>.SuccessResponse(userDto);
        }

        public async Task<ApiResponse<object>> SetRoleAsync(SetRoleRequestDto dto)
        {
            var user = await GetCurrentUserHelperAsync();
            if (user == null)
                return ApiResponse<object>.FailureResponse("User not found.", 404);

            var role = await _roleManager.FindByIdAsync(dto.RoleId.ToString());
            if (role == null)
                return ApiResponse<object>.FailureResponse($"Role with ID {dto.RoleId} does not exist.", 400);

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return ApiResponse<object>.FailureResponse("Failed to remove user from existing roles.", 500);

            var addResult = await _userManager.AddToRoleAsync(user, role.Name);
            if (!addResult.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, currentRoles);
                return ApiResponse<object>.FailureResponse($"Failed to add user to role '{role.Name}'.", 500);
            }

            return ApiResponse<object>.SuccessResponse(null, $"User {user.UserName} has been assigned the role '{role.Name}'.");
        }

        private async Task<ApplicationUserModel?> GetCurrentUserHelperAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return null;

            return await _userManager.FindByIdAsync(userId);
        }
    }
}
