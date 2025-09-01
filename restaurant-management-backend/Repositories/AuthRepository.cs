using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using restaurant_management_backend.Data;
using restaurant_management_backend.Dtos.Auth;
using restaurant_management_backend.Extensions;
using restaurant_management_backend.Interfaces;
using restaurant_management_backend.Models.UserAndStaff;
using restaurant_management_backend.Utility;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Web;

namespace restaurant_management_backend.Repositories
{
    public class AuthRepository: IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly IInfrastructureRepository _infraRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public AuthRepository(ApplicationDbContext context, UserManager<ApplicationUserModel> userManger, IInfrastructureRepository infraRepo, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) { 
            _context = context;
            _userManager = userManger;
            _infraRepo = infraRepo;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public async Task<(string, ApiResponse<LoginResponseDto>)> LoginAsync(LoginRequestDto dto)
        {
            var user = (ApplicationUserModel)null;

            if (dto.Identifier.IsEmail())
                user = await _userManager.FindByEmailAsync(dto.Identifier);
            else
                user = await _userManager.FindByNameAsync(dto.Identifier);

            if (user == null)
                return ("", ApiResponse<LoginResponseDto>.FailureResponse("No user found.", 401));

            var isCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isCorrect)
                return ("", ApiResponse<LoginResponseDto>.FailureResponse("Incorrect password.", 401));

            if (!user.EmailConfirmed)
                return ("", ApiResponse<LoginResponseDto>.FailureResponse("Email not verified", 403));

            var token = await _infraRepo.GenerateToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault()?.ToUpper();

            var response = new LoginResponseDto{
                Email = user.Email,
                Role = role,
            };

            return (token.ToString()!, ApiResponse<LoginResponseDto>.SuccessResponse(response, "Login Successfully!"));
        }

        public async Task<ApiResponse<(string Email, string Token)>> RegisterCustomerAsync(RegisterRequestDto dto)
        {
            var existingUsername = await _userManager.FindByNameAsync(dto.Username);
            if (existingUsername != null)
                return ApiResponse<(string, string)>.FailureResponse("Username already exists", 409);

            var existingEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingEmail != null)
                return ApiResponse<(string, string)>.FailureResponse("Email already exists", 409);

            //var rolesWithId = _context.Roles.Select(u => new { u.Id, u.Name});
            //Debug.WriteLine(rolesWithId);            

            var user = new ApplicationUserModel{
                UserName = dto.Username,
                Email = dto.Email
            };

            var identityResult = await _userManager.CreateAsync(user, dto.Password);

            if (!identityResult.Succeeded){
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                var message = string.Join("\n", errors);
                return ApiResponse<(string, string)>.FailureResponse(message, 400);
            }

            var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "CUSTOMER");
            if (customerRole == null)
                return ApiResponse<(string, string)>.FailureResponse("Customer role not found in database", 500);

            await _userManager.AddToRoleAsync(user, customerRole.Name);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);

            return ApiResponse<(string, string)>.SuccessResponse((dto.Email, encodedToken), "Confirmation email sent.", 201);
        }

        public async Task<ApiResponse<object>> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return ApiResponse<object>.FailureResponse("Invalid user.");

            var decodedToken = WebUtility.UrlDecode(token);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
                return ApiResponse<object>.FailureResponse(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );


            await _context.SaveChangesAsync();

            return ApiResponse<object>.SuccessResponse(null, "Email confirmed successfully!");
        }

        public async Task<ApiResponse<(string Email, string Token)>> ResendEmailConfirmAsync(ResendEmailConfirmDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return ApiResponse<(string, string)>.FailureResponse("Register first.", 409);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);

            return ApiResponse<(string, string)>.SuccessResponse((dto.Email, encodedToken), "Rsend confirmation email sent.", 201);
        }

        public async Task<ApiResponse<object>> RegisterWithRoleAsync(RegisterWithRoleRequestDto dto)
        {
            var existingUsername = await _userManager.FindByNameAsync(dto.Username);
            if (existingUsername != null)
                return ApiResponse<object>.FailureResponse("Username already exists", 409);

            var existingEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingEmail != null)
                return ApiResponse<object>.FailureResponse("Email already exists", 409);

            //var rolesWithId = _context.Roles.Select(u => new { u.Id, u.Name});   

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == dto.RoleId);
            if (role == null)
                return ApiResponse<object>.FailureResponse("Invalid role selected", 400);

            var user = new ApplicationUserModel{
                UserName = dto.Username,
                Email = dto.Email,
                EmailConfirmed = true
            };

            var password = PasswordGenerator.GeneratePassword();

            var identityResult = await _userManager.CreateAsync(user, password);

            if (!identityResult.Succeeded){
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                var message = string.Join("\n", errors);
                return ApiResponse<object>.FailureResponse(message, 400);
            }

            await _userManager.AddToRoleAsync(user, role.Name);
            var addedUser = await _userManager.FindByEmailAsync(dto.Email);

            var subject = "Your Account Credentials - Jethva's Maestro";
            var body = $@"
                <html>
                  <body style='font-family: Arial, sans-serif; background-color:#f8f8f8; padding:20px;'>
                    <div style='max-width:600px; margin:auto; background:white; border-radius:10px; padding:30px; box-shadow:0 2px 8px rgba(0,0,0,0.1);'>
      
                      <h2 style='color:#4CAF50; text-align:center;'>Your Account Has Been Created 🎉</h2>
      
                      <p style='font-size:16px; color:#333;'>Hello {addedUser.UserName},</p>
      
                      <p style='font-size:15px; color:#555;'>
                        Your account has been created successfully for <b>Jethva's Maestro</b>.  
                        Below are your login credentials:
                      </p>
      
                      <div style='background-color:#f4f4f4; padding:15px; border-radius:5px; margin:20px 0; font-size:15px; color:#333;'>
                        <p><b>Username:</b> {dto.Username}</p>
                        <p><b>Password:</b> {password}</p>
                        <p><b>Role:</b> {dto.RoleId}</p>
                      </div>
      
                      <p style='font-size:15px; color:#555;'>
                        ⚠️ For security reasons, please change your password immediately after your first login.
                      </p>
      
                      <hr style='border:none; border-top:1px solid #eee; margin:20px 0;' />
      
                      <p style='font-size:14px; color:#555; text-align:center;'>
                        Best regards,<br/>
                        <span style='color:#FF9800; font-weight:bold;'>Jethva's Maestro 🧑🏻‍🍳</span>
                      </p>
                    </div>
                  </body>
                </html>";

            await _infraRepo.SendEmailAsync(dto.Email, subject, body);

            return ApiResponse<object>.SuccessResponse(null, "User registered successfully. Credentials has been sent.", 201);
        }

        public async Task<ApiResponse<object>> ResetPasswordRequest(ResetPasswordRequestDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return ApiResponse<object>.FailureResponse("User not found.", 401);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);

            var resetLink = $"https://yourfrontend.com/reset-password?email={user.Email}&token={encodedToken}";

            var subject = "Reset your password - Jethva's Maestro";
            var body = $@"
                <html>
                  <body style='font-family: Arial, sans-serif;'>
                    <h2>Password Reset Request 🔑</h2>
                    <p>Hi {user.UserName},</p>
                    <p>We received a request to reset your password. Use the link below to proceed:</p>
                    <p>
                      <a href='{resetLink}' 
                         style='background-color:#4CAF50;color:white;padding:10px 20px;text-decoration:none;border-radius:5px;'>
                         Click here
                      </a>
                    </p>
                    <p>This link will expire in <b>10 minutes</b>.</p>
                    <p>If you didn’t request this, please ignore this email or contact support.</p>
                    <p>Best regards,<br/>Jethva's Maestro 🧑🏻‍🍳</p>
                  </body>
                </html>
               ";

            var (success, message) = await _infraRepo.SendEmailAsync(dto.Email, subject, body);
            if (!success)
                return ApiResponse<object>.FailureResponse(message ?? "Email sending failed");

            return ApiResponse<object>.SuccessResponse(null, "Please check your inbox.", 201);
        }

        public async Task<ApiResponse<object>> ResetPassword(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return ApiResponse<object>.FailureResponse("Password reset failed. Please try again.", 400);

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

            if (!result.Succeeded){
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ApiResponse<object>.FailureResponse("Password reset failed. The link may have expired. Please try again.", 400);
            }

            return ApiResponse<object>.SuccessResponse(null, "Your password has been reset successfully. You can now log in.");
        }

    }
}
