using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using restaurant_management_backend.Dtos.Auth;
using restaurant_management_backend.Interfaces;
using restaurant_management_backend.Models.UserAndStaff;
using restaurant_management_backend.Utility;
using System.Net;

namespace restaurant_management_backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IInfrastructureRepository _infraRepo;
        private readonly UserManager<ApplicationUserModel> _userManager;

        public AuthController(IAuthRepository authRepository,  UserManager<ApplicationUserModel> userManger, IInfrastructureRepository infraRepo)
        {
            _authRepository = authRepository;
            _userManager = userManger;
            _infraRepo = infraRepo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var (token, response) = await _authRepository.LoginAsync(dto);

            if (!response.Success || token == null)
                return StatusCode(response.StatusCode, response);

            HttpContext.Response.Cookies.Append("ACCESS_TOKEN", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(1),
            });

            return Ok(response);
        }

        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomer(RegisterRequestDto dto)
        {
            var response = await _authRepository.RegisterCustomerAsync(dto);

            if (!response.Success || response.Data.Token == null)
                return StatusCode(response.StatusCode, response);

            var callbackUrl = Url.Action(
                action: "ConfirmEmail",
                controller: "Auth",
                values: new { email = response.Data.Email, token = response.Data.Token },
                protocol: Request.Scheme
            );

            var subject = "Confirm your email address - Jethva's Maestro";
            var body = $@"
                <html>
                  <body style='font-family: Arial, sans-serif;'>
                    <h2>Welcome to Jethva's Maestro 🍽️</h2>
                    <p>Hi {response.Data.Email},</p>
                    <p>Thank you for registering. Please confirm your email address by clicking the button below:</p>
                    <p>
                      <a href='{callbackUrl}' 
                         style='background-color:#4CAF50;color:white;padding:10px 20px;text-decoration:none;border-radius:5px;'>
                         Confirm Email
                      </a>
                    </p>
                    <p>Best regards,<br/>Jethva's Maestro 🧑🏻‍🍳</p>
                  </body>
                </html>
            ";

            await _infraRepo.SendEmailAsync(response.Data.Email, subject, body);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("register-with-role")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RegisterWithRoleAsync(RegisterWithRoleRequestDto dto)
        {
            var result = await _authRepository.RegisterWithRoleAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendEmailConfirm(ResendEmailConfirmDto dto)
        {
            var response = await _authRepository.ResendEmailConfirmAsync(dto);

            if (!response.Success || response.Data.Token == null)
                return StatusCode(response.StatusCode, response);

            var callbackUrl = Url.Action(
                action: "ConfirmEmail",
                controller: "Auth",
                values: new { email = response.Data.Email, token = response.Data.Token },
                protocol: Request.Scheme
            );

            var subject = "Confirm your email address - Jethva's Maestro [Resend]";
            var body = $@"
                <html>
                  <body style='font-family: Arial, sans-serif;'>
                    <h2>Welcome to Jethva's Maestro 🍽️</h2>
                    <p>Hi {response.Data.Email},</p>
                    <p>Please confirm your email address by clicking the button below:</p>
                    <p>
                      <a href='{callbackUrl}' 
                         style='background-color:#4CAF50;color:white;padding:10px 20px;text-decoration:none;border-radius:5px;'>
                         Confirm Email
                      </a>
                    </p>
                    <p>Best regards,<br/>Jethva's Maestro 🧑🏻‍🍳</p>
                  </body>
                </html>
            ";

            var (success, message) = await _infraRepo.SendEmailAsync(response.Data.Email, subject, body);
            if (!success)
                response = ApiResponse<(string, string)>.FailureResponse(message ?? "Email sending failed");

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var result = await _authRepository.ConfirmEmailAsync(email, token);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("reset-password-request")]
        public async Task<IActionResult> ResetPasswordRequest([FromBody] ResetPasswordRequestDto dto)
        {
            var response = await _authRepository.ResetPasswordRequest(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var response = await _authRepository.ResetPassword(dto);
            return StatusCode(response.StatusCode, response);
        }
        
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("ACCESS_TOKEN");
            return Ok(ApiResponse<object>.SuccessResponse(null, "Logout successful."));
        }

    }
}
