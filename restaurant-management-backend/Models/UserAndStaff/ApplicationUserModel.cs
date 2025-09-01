using Microsoft.AspNetCore.Identity;
using restaurant_management_backend.Models.CustomerRelationshipAndMarketing;
using restaurant_management_backend.Models.OrderAndOperations;

namespace restaurant_management_backend.Models.UserAndStaff
{
    public class ApplicationUserModel: IdentityUser<Guid>
    {
        public bool IsDeleted { get; set; } = false;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public GenderEnum? Gender { get; set; }

        public string? Address { get; set; }
        
        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public string? PreferredLanguage { get; set; }

        public AccountTypeEnum AccountType { get; set; } = AccountTypeEnum.Basic;

        public virtual ICollection<UserRoleModel> UserRoles { get; set; }
        public virtual ICollection<StaffScheduleModel> Schedules { get; set; }
        public virtual ICollection<OrderModel> Orders { get; set; }
        public virtual ICollection<ReservationModel> Reservations { get; set; }
        public virtual ICollection<DeliveryModel> Deliveries { get; set; }
        public virtual ICollection<CustomerFeedbackModel> Feedbacks { get; set; }
    }

    public enum GenderEnum
    {
        Male = 1,
        Female = 2,
        Prefer_Not_To_Say = 3,
    }

    public enum AccountTypeEnum
    {
        Basic = 1,
        Premium = 2, 
        Enterprise = 3
    }
}
