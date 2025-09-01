using Microsoft.AspNetCore.Identity;
using System.Text;

namespace restaurant_management_backend.Extensions
{
    public class PasswordGenerator
    {
        public static string GeneratePassword(PasswordOptions opts = null)
        {
            if (opts == null)
            {
                opts = new PasswordOptions
                {
                    RequiredLength = 8,
                    RequiredUniqueChars = 4,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireNonAlphanumeric = true,
                    RequireUppercase = true
                };
            }

            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@$?_-",                       // non-alphanumeric
        };

            Random rand = new Random(Environment.TickCount);
            var chars = new StringBuilder();

            if (opts.RequireUppercase)
                chars.Append(randomChars[0][rand.Next(randomChars[0].Length)]);
            if (opts.RequireLowercase)
                chars.Append(randomChars[1][rand.Next(randomChars[1].Length)]);
            if (opts.RequireDigit)
                chars.Append(randomChars[2][rand.Next(randomChars[2].Length)]);
            if (opts.RequireNonAlphanumeric)
                chars.Append(randomChars[3][rand.Next(randomChars[3].Length)]);

            while(chars.Length < opts.RequiredLength || chars.ToString().Distinct().Count() < opts.RequiredUniqueChars){
                string rcs = randomChars[rand.Next(randomChars.Length)];
                chars.Append(rcs[rand.Next(rcs.Length)]);
            }

            return new string(chars.ToString().OrderBy(c => rand.Next()).ToArray());
        }
    }
}
