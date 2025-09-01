using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using restaurant_management_backend.Dtos.Infrastructure;
using restaurant_management_backend.Interfaces;
using restaurant_management_backend.Models.UserAndStaff;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Runtime;
using System.Security.Claims;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace restaurant_management_backend.Repositories
{
    public class InfrastructureRepository: IInfrastructureRepository
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly SymmetricSecurityKey _key;
        private readonly MailSettingsDto _mailSettings;

        public InfrastructureRepository(IConfiguration config, UserManager<ApplicationUserModel> userManager, IOptions<MailSettingsDto> options)
        {
            _config = config;
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SigningKey"]));
            _mailSettings = options.Value;
        }

        public async Task<string> GenerateToken(ApplicationUserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1), // Token expiration time
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(bool success, string message)> SendEmailAsync(string toEmail, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                return (false, "Recipient email address cannot be null or empty");
            if (string.IsNullOrWhiteSpace(subject))
                return (false, "Subject code cannot be null or empty");
            if (string.IsNullOrWhiteSpace(body))
                return (false, "Body email address cannot be null or empty");

            MailMessage mail = new MailMessage
            {
                From = new MailAddress(_mailSettings.FromEmail, _mailSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(toEmail);

            using var smtp = new SmtpClient(_mailSettings.SmtpHost, _mailSettings.SmtpPort)
            {
                Credentials = new NetworkCredential(_mailSettings.SmtpUser, _mailSettings.SmtpPass),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);

            return (true, "Succesfully sent verification email.");
        }

    }
}
