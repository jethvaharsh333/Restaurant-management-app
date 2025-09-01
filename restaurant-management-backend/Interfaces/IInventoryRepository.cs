using restaurant_management_backend.Dtos.Inventory;
using restaurant_management_backend.Utility;

namespace restaurant_management_backend.Interfaces
{
    public interface IInventoryRepository
    {
        // Ingredient Methods
        public Task<ApiResponse<List<IngredientDto>>> GetIngredientsAsync();

        public Task<ApiResponse<IngredientDto>> AddIngredientAsync(UpsertIngredientRequestDto dto);

        public Task<ApiResponse<IngredientDto>> UpdateIngredientAsync(Guid id, UpsertIngredientRequestDto dto);

        public Task<ApiResponse<object>> DeleteIngredientAsync(Guid id);

        // Supplier Methods
        Task<ApiResponse<List<SupplierDto>>> GetSuppliersAsync();
        Task<ApiResponse<SupplierDto>> AddSupplierAsync(CreateSupplierDto dto);

        // Purchase Order Methods
        Task<ApiResponse<List<PurchaseOrderDto>>> GetPurchaseOrdersAsync();
        Task<ApiResponse<PurchaseOrderDto>> CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto);
        Task<ApiResponse<object>> ReceivePurchaseOrderAsync(Guid poId);

    }
}
