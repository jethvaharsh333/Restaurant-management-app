using AutoMapper;
using Microsoft.EntityFrameworkCore;
using restaurant_management_backend.Data;
using restaurant_management_backend.Dtos.Menu;
using restaurant_management_backend.Extensions;
using restaurant_management_backend.Interfaces;
using restaurant_management_backend.Models.MenuAndInventory;
using restaurant_management_backend.Utility;

namespace restaurant_management_backend.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public MenuRepository(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
        }
        
        public async Task<ApiResponse<CategoryDto>> AddCategory(UpsertCategoryRequestDto dto)
        {
            var category = _mapper.Map<CategoryModel>(dto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return ApiResponse<CategoryDto>.SuccessResponse(categoryDto, "Category created successfully.", 201);
        }

        public async Task<ApiResponse<CategoryDto>> UpdateCategory(Guid id, UpsertCategoryRequestDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return ApiResponse<CategoryDto>.FailureResponse("Category not found.", 404);

            _mapper.Map(dto, category);
            await _context.SaveChangesAsync();

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return ApiResponse<CategoryDto>.SuccessResponse(categoryDto, "Category updated successfully.");
        }

        public async Task<ApiResponse<object>> DeleteCategory(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return ApiResponse<object>.FailureResponse("Category not found.", 404);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return ApiResponse<object>.SuccessResponse(null, "Category deleted successfully.");
        }

        public async Task<ApiResponse<List<CategoryDto>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
            return ApiResponse<List<CategoryDto>>.SuccessResponse(categoryDtos);
        }

        public async Task<ApiResponse<MenuItemDto>> AddMenuItem(UpsertMenuItemRequestDto dto)
        {
            var menuItem = _mapper.Map<MenuItemModel>(dto);

            var currentUser = await _currentUser.GetUser();
            if (currentUser == null)
                return ApiResponse<MenuItemDto>.FailureResponse("User not found", 401);

            foreach (var ingredientDto in dto.Ingredients)
            {
                menuItem.MenuItemIngredients.Add(new MenuItemIngredientModel
                {
                    IngredientId = ingredientDto.IngredientId,
                    QuantityUsed = ingredientDto.QuantityUsed
                });
            }

            menuItem.ModifiedBy = currentUser.Id;
            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            var menuItemDto = _mapper.Map<MenuItemDto>(menuItem);
            return ApiResponse<MenuItemDto>.SuccessResponse(menuItemDto, "Menu item created successfully.", 201);
        }

        public async Task<ApiResponse<MenuItemDto>> UpdateMenuItem(Guid id, UpsertMenuItemRequestDto dto)
        {
            var menuItem = await _context.MenuItems.Include(mi => mi.MenuItemIngredients)
                .FirstOrDefaultAsync(mi => mi.MenuItemId == id);

            if (menuItem == null)
                return ApiResponse<MenuItemDto>.FailureResponse("Menu item not found.", 404);

            var currentUser = await _currentUser.GetUser();
            if (currentUser == null)
                return ApiResponse<MenuItemDto>.FailureResponse("User not found", 401);

            _mapper.Map(dto, menuItem);
            menuItem.ModifiedBy = currentUser.Id;
            menuItem.MenuItemIngredients.Clear();
            foreach (var ingredientDto in dto.Ingredients)
            {
                menuItem.MenuItemIngredients.Add(new MenuItemIngredientModel
                {
                    IngredientId = ingredientDto.IngredientId,
                    QuantityUsed = ingredientDto.QuantityUsed
                });
            }

            await _context.SaveChangesAsync();

            var menuItemDto = _mapper.Map<MenuItemDto>(menuItem);
            return ApiResponse<MenuItemDto>.SuccessResponse(menuItemDto, "Menu item updated successfully.");
        }
    
        public async Task<ApiResponse<object>> DeleteMenuItem(Guid id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
                return ApiResponse<object>.FailureResponse("Menu item not found.", 404);

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return ApiResponse<object>.SuccessResponse(null, "Menu item deleted successfully.");
        }

        public async Task<ApiResponse<List<MenuItemDto>>> GetMenuItems()
        {
            var menuItems = await _context.MenuItems.Include(mi => mi.Category).ToListAsync();
            var menuItemDtos = _mapper.Map<List<MenuItemDto>>(menuItems);
            return ApiResponse<List<MenuItemDto>>.SuccessResponse(menuItemDtos);
        }

        //public async Task<ApiResponse<MenuItemDetailDto>> GetMenuItemByIdAsync(Guid id)
        //{
        //    var menuItem = await _context.MenuItems
        //        .Include(mi => mi.Category)
        //        .Include(mi => mi.MenuItemIngredients)
        //            .ThenInclude(mii => mii.Ingredient)
        //        .Include(mi => mi.Modifiers)
        //        .FirstOrDefaultAsync(mi => mi.MenuItemId == id);

        //    if (menuItem == null)
        //    {
        //        return ApiResponse<MenuItemDetailDto>.FailureResponse("Menu item not found.", 404);
        //    }

        //    var menuItemDto = _mapper.Map<MenuItemDetailDto>(menuItem);

        //    return ApiResponse<MenuItemDetailDto>.SuccessResponse(menuItemDto);
        //}

    }
}
