using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.UserAndStaff
{
    public class UserRoleModel : IdentityUserRole<Guid>
    {
        [ForeignKey("UserId")]
        public virtual ApplicationUserModel User { get; set; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRoleModel Role { get; set; }
    }
}
