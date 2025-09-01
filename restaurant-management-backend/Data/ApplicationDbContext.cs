using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using restaurant_management_backend.Models.CustomerRelationshipAndMarketing;
using restaurant_management_backend.Models.MenuAndInventory;
using restaurant_management_backend.Models.OrderAndOperations;
using restaurant_management_backend.Models.Setting; // Added for settings models
using restaurant_management_backend.Models.UserAndStaff;

namespace restaurant_management_backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUserModel, ApplicationRoleModel, Guid, IdentityUserClaim<Guid>, UserRoleModel, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        // CRM
        public DbSet<CustomerFeedbackModel> CustomerFeedbacks { get; set; }
        public DbSet<PromotionsModel> Promotions { get; set; }

        // Menu & Inventory
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<IngredientModel> Ingredients { get; set; }
        public DbSet<MenuItemModel> MenuItems { get; set; }
        public DbSet<MenuItemIngredientModel> MenuItemIngredients { get; set; }
        public DbSet<PurchaseOrderModel> PurchaseOrders { get; set; }
        public DbSet<SupplierModel> Suppliers { get; set; }
        public DbSet<PurchaseOrderItemModel> PurchaseOrderItems { get; set; }

        //public DbSet<ModifierModel> Modifiers { get; set; }


        // Order & Operations
        public DbSet<DeliveryModel> Deliveries { get; set; }
        public DbSet<OrderItemModel> OrderItems { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<PaymentModel> Payments { get; set; }
        public DbSet<ReservationModel> Reservations { get; set; }
        public DbSet<TableModel> RestaurantTables { get; set; }

        // Settings
        public DbSet<BlackoutDateModel> BlackoutDates { get; set; }
        public DbSet<ReservationSettingModel> ReservationSettings { get; set; }
        public DbSet<RestaurantSettingModel> RestaurantSettings { get; set; }

        // User & Staff
        public DbSet<StaffScheduleModel> StaffSchedules { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            // This MUST be called first to configure the Identity tables
            base.OnModelCreating(builder);

            // =================================================================
            // NEW: Dynamically set default value for all Guid Primary Keys
            // This loop ensures that every table, including Identity tables,
            // uses NEWSEQUENTIALID() for its Guid primary key.
            // =================================================================
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var primaryKey = entityType.FindPrimaryKey();
                if (primaryKey != null)
                {
                    foreach (var property in primaryKey.Properties)
                    {
                        if (property.ClrType == typeof(Guid))
                        {
                            property.SetDefaultValueSql("NEWSEQUENTIALID()");
                        }
                    }
                }
            }

            // Configure the many-to-many relationship for Identity
            builder.Entity<UserRoleModel>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            // Configure the composite key for the MenuItemIngredient junction table
            builder.Entity<MenuItemIngredientModel>()
                .HasKey(mi => new { mi.MenuItemId, mi.IngredientId });

            // Configure one-to-one relationship between Order and Delivery
            builder.Entity<OrderModel>()
                .HasOne(o => o.Delivery)
                .WithOne(d => d.Order)
                .HasForeignKey<DeliveryModel>(d => d.OrderId);

            // Configure unique constraints
            builder.Entity<TableModel>()
                .HasIndex(t => t.TableNumber)
                .IsUnique();

            builder.Entity<PromotionsModel>()
                .HasIndex(p => p.Code)
                .IsUnique();

            // Configure soft delete query filter
            builder.Entity<ApplicationUserModel>().HasQueryFilter(u => !u.IsDeleted);

            // Configure cascade delete behavior to prevent cycles
            builder.Entity<CustomerFeedbackModel>()
                .HasOne(cf => cf.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(cf => cf.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CustomerFeedbackModel>()
                .HasOne(cf => cf.Order)
                .WithMany(o => o.Feedbacks)
                .HasForeignKey(cf => cf.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PurchaseOrderItemModel>()
                .HasKey(poi => new { poi.PurchaseOrderId, poi.IngredientId });
        }
    }
}
