using Microsoft.AspNetCore.Identity;

namespace restaurant_management_backend.Models.UserAndStaff
{
    public class ApplicationRoleModel : IdentityRole<Guid>
    {
        public virtual ICollection<UserRoleModel> UserRoles { get; set; }
    }
}
