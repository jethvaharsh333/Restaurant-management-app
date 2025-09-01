using AutoMapper;
using Microsoft.EntityFrameworkCore;
using restaurant_management_backend.Data;
using restaurant_management_backend.Dtos.Inventory;
using restaurant_management_backend.Interfaces;
using restaurant_management_backend.Models.MenuAndInventory;
using restaurant_management_backend.Utility;

namespace restaurant_management_backend.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public InventoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<IngredientDto>>> GetIngredientsAsync()
        {
            var ingredients = await _context.Ingredients.ToListAsync();
            var ingredientDtos = _mapper.Map<List<IngredientDto>>(ingredients);
            return ApiResponse<List<IngredientDto>>.SuccessResponse(ingredientDtos);
        }

        public async Task<ApiResponse<IngredientDto>> AddIngredientAsync(UpsertIngredientRequestDto dto)
        {
            var ingredient = _mapper.Map<IngredientModel>(dto);
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            var ingredientDto = _mapper.Map<IngredientDto>(ingredient);
            return ApiResponse<IngredientDto>.SuccessResponse(ingredientDto, "Ingredient added successfully.", 201);
        }

        public async Task<ApiResponse<IngredientDto>> UpdateIngredientAsync(Guid id, UpsertIngredientRequestDto dto)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
                return ApiResponse<IngredientDto>.FailureResponse("Ingredient not found.", 404);

            _mapper.Map(dto, ingredient);
            await _context.SaveChangesAsync();
            var ingredientDto = _mapper.Map<IngredientDto>(ingredient);
            return ApiResponse<IngredientDto>.SuccessResponse(ingredientDto, "Ingredient updated successfully.");
        }

        public async Task<ApiResponse<object>> DeleteIngredientAsync(Guid id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
                return ApiResponse<object>.FailureResponse("Ingredient not found.", 404);

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
            return ApiResponse<object>.SuccessResponse(null, "Ingredient deleted successfully.");
        }

        // --- Supplier Methods ---
        public async Task<ApiResponse<List<SupplierDto>>> GetSuppliersAsync()
        {
            var suppliers = await _context.Suppliers.ToListAsync();
            var supplierDtos = _mapper.Map<List<SupplierDto>>(suppliers);
            return ApiResponse<List<SupplierDto>>.SuccessResponse(supplierDtos);
        }

        public async Task<ApiResponse<SupplierDto>> AddSupplierAsync(CreateSupplierDto dto)
        {
            var supplier = _mapper.Map<SupplierModel>(dto);
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            var supplierDto = _mapper.Map<SupplierDto>(supplier);
            return ApiResponse<SupplierDto>.SuccessResponse(supplierDto, "Supplier added.", 201);
        }

        // --- Purchase Order Methods ---
        public async Task<ApiResponse<List<PurchaseOrderDto>>> GetPurchaseOrdersAsync()
        {
            var purchaseOrders = await _context.PurchaseOrders.Include(po => po.Supplier).ToListAsync();
            var poDtos = _mapper.Map<List<PurchaseOrderDto>>(purchaseOrders);
            return ApiResponse<List<PurchaseOrderDto>>.SuccessResponse(poDtos);
        }

        public async Task<ApiResponse<PurchaseOrderDto>> CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto)
        {
            var supplier = await _context.Suppliers.FindAsync(dto.SupplierId);
            if (supplier == null) return ApiResponse<PurchaseOrderDto>.FailureResponse("Supplier not found.", 404);

            var totalCost = dto.Items.Sum(item => item.Cost * item.Quantity);

            var purchaseOrder = new PurchaseOrderModel
            {
                SupplierId = dto.SupplierId,
                TotalCost = totalCost,
                Status = PurchaseOrderStatusEnum.Pending,
                Items = new List<PurchaseOrderItemModel>()
            };

            foreach (var itemDto in dto.Items)
            {
                var ingredientExists = await _context.Ingredients.AnyAsync(i => i.IngredientId == itemDto.IngredientId);
                if (!ingredientExists)
                {
                    return ApiResponse<PurchaseOrderDto>.FailureResponse($"Ingredient with ID {itemDto.IngredientId} not found.", 400);
                }

                purchaseOrder.Items.Add(new PurchaseOrderItemModel
                {
                    IngredientId = itemDto.IngredientId,
                    Quantity = itemDto.Quantity,
                    Cost = itemDto.Cost
                });
            }

            _context.PurchaseOrders.Add(purchaseOrder);
            await _context.SaveChangesAsync();

            var poDto = _mapper.Map<PurchaseOrderDto>(purchaseOrder);
            return ApiResponse<PurchaseOrderDto>.SuccessResponse(poDto, "Purchase Order created.", 201);
        }

        public async Task<ApiResponse<object>> ReceivePurchaseOrderAsync(Guid poId)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(poId);
            if (purchaseOrder == null) 
                return ApiResponse<object>.FailureResponse("Purchase Order not found.", 404);

            if (purchaseOrder.Status == PurchaseOrderStatusEnum.Completed) 
                return ApiResponse<object>.FailureResponse("This order has already been received.", 400);

            var poItems = await _context.PurchaseOrderItems.Where(i => i.PurchaseOrderId == poId).ToListAsync();

            foreach (var item in poItems)
            {
                var ingredient = await _context.Ingredients.FindAsync(item.IngredientId);
                if (ingredient != null)
                {
                    ingredient.StockQuantity += item.Quantity;
                }
            }

            purchaseOrder.Status = PurchaseOrderStatusEnum.Completed;
            await _context.SaveChangesAsync();
            return ApiResponse<object>.SuccessResponse(null, "Purchase Order marked as received and stock updated.");
        }

    }
}
