using Microsoft.AspNetCore.Identity;
using restaurant_management_backend.Models.UserAndStaff;

namespace restaurant_management_backend.Seed
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRoleModel>>();

            string[] roles = { "ADMIN", "MANAGER", "WAITER", "KITCHEN", "DELIVERY", "CUSTOMER" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var newRole = new ApplicationRoleModel { Name = roleName, NormalizedName = roleName.ToUpper() };
                    await roleManager.CreateAsync(newRole);
                }
            }
        }

        public static async Task SeedAdminUsersAsync(IServiceProvider serviceProvider)
        {
            // Use a scope to get the necessary services
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUserModel>>();

            // --- First Admin User ---
            var adminUserEmail = "admin1@gmail.com";
            if (await userManager.FindByEmailAsync(adminUserEmail) == null)
            {
                var adminUser = new ApplicationUserModel
                {
                    UserName = adminUserEmail,
                    Email = adminUserEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "9999999991"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "ADMIN");
                }
            }

            // --- Second Admin User ---
            var superAdminUserEmail = "admin2@gmail.com";
            if (await userManager.FindByEmailAsync(superAdminUserEmail) == null)
            {
                var superAdminUser = new ApplicationUserModel
                {
                    UserName = superAdminUserEmail,
                    Email = superAdminUserEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "9999999992"
                };

                var result = await userManager.CreateAsync(superAdminUser, "SuperAdmin@123");
                if (result.Succeeded)
                {
                    // You can assign multiple roles if needed
                    await userManager.AddToRoleAsync(superAdminUser, "ADMIN");
                    await userManager.AddToRoleAsync(superAdminUser, "MANAGER");
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUserModel>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRoleModel>>();

            // Define roles
            string[] roles = { "ADMIN", "MANAGER", "WAITER", "KITCHEN", "DELIVERY", "CUSTOMER" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new ApplicationRoleModel { Name = role });
            }

            var users = new List<(string Email, string Phone, string Password, string Role)>
            {
                ("admin@example.com",    "9876543210", "Admin@123",    "ADMIN"),
                ("manager@example.com",  "9876543211", "Manager@123",  "MANAGER"),
                ("waiter1@example.com",  "9876543212", "Waiter@123",   "WAITER"),
                ("kitchen@example.com",  "9876543213", "Kitchen@123",  "KITCHEN"),
                ("delivery1@example.com","9876543214", "Delivery@123", "DELIVERY"),
                ("isha.j@email.com",     "9123456780", "User@123",     "CUSTOMER"),
                ("karan.v@email.com",    "9123456781", "User@123",     "CUSTOMER")
            };

            foreach (var (email, phone, password, role) in users)
            {
                var existingUser = await userManager.FindByEmailAsync(email);
                if (existingUser == null)
                {
                    var user = new ApplicationUserModel
                    {
                        UserName = email,
                        NormalizedUserName = email.ToUpper(),
                        Email = email,
                        NormalizedEmail = email.ToUpper(),
                        PhoneNumber = phone,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, password);
                    if (!result.Succeeded)
                    {
                        throw new Exception(
                            $"Failed to seed user {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}"
                        );
                    }

                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}