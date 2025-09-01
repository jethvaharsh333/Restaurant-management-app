using AutoMapper;
using restaurant_management_backend.Dtos.Inventory;
using restaurant_management_backend.Dtos.Menu;
using restaurant_management_backend.Dtos.Table;
using restaurant_management_backend.Dtos.User;
using restaurant_management_backend.Models.MenuAndInventory;
using restaurant_management_backend.Models.OrderAndOperations;
using restaurant_management_backend.Models.UserAndStaff;

namespace restaurant_management_backend.Utility
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<UpdateProfileRequestDto, ApplicationUserModel>()
                // This tells AutoMapper to ignore null values from the DTO,
                // so you don't accidentally wipe out existing data.
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ApplicationUserModel, UserDto>();

            CreateMap<UpsertCategoryRequestDto, CategoryModel>();
            CreateMap<CategoryModel, CategoryDto>();
            CreateMap<CategoryModel, List<CategoryDto>>();

            CreateMap<List<SupplierModel>, List<SupplierDto>>();
            CreateMap<SupplierModel, SupplierDto>();
            CreateMap<CreateSupplierDto, SupplierModel>();
            CreateMap<List<PurchaseOrderModel>, List<PurchaseOrderDto>>();

            CreateMap<List<TableModel>, List<TableDto>>();
            CreateMap<List<ReservationModel>, List<ReservationDto>>();
            CreateMap<ReservationModel, ReservationDto>();

            //CreateMap<>();
            //CreateMap<>();
            //CreateMap<>();
            //CreateMap<>();
            //CreateMap<>();
            //CreateMap<>();
            //CreateMap<>();
            //CreateMap<>();
            //CreateMap<>();
            //CreateMap<>();
            //CreateMap<>();
            //CreateMap<>();
            //CreateMap<>();
            //CreateMap<>();
            //CreateMap<>();
        }
    }
}
