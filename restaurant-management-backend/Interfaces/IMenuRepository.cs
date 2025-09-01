using restaurant_management_backend.Dtos.Menu;
using restaurant_management_backend.Utility;

namespace restaurant_management_backend.Interfaces
{
    public interface IMenuRepository
    {
        Task<ApiResponse<CategoryDto>> AddCategory(UpsertCategoryRequestDto dto);
        Task<ApiResponse<CategoryDto>> UpdateCategory(Guid categoryId, UpsertCategoryRequestDto dto);
        Task<ApiResponse<object>> DeleteCategory(Guid categoryId);
        Task<ApiResponse<List<CategoryDto>>> GetCategories();

        Task<ApiResponse<MenuItemDto>> AddMenuItem(UpsertMenuItemRequestDto dto);
        Task<ApiResponse<MenuItemDto>> UpdateMenuItem(Guid id, UpsertMenuItemRequestDto dto);
        Task<ApiResponse<object>> DeleteMenuItem(Guid menuItemId);
        Task<ApiResponse<List<MenuItemDto>>> GetMenuItems();
    }
}
