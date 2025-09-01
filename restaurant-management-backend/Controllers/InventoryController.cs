using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurant_management_backend.Dtos.Inventory;
using restaurant_management_backend.Interfaces;

namespace restaurant_management_backend.Controllers
{
    [Route("api/inventory")]
    [ApiController]
    [Authorize(Roles = "ADMIN,MANAGER")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN,MANAGER,KITCHEN")] // Allow kitchen staff to view inventory
        public async Task<IActionResult> GetIngredients()
        {
            var response = await _inventoryRepository.GetIngredientsAsync();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddIngredient(UpsertIngredientRequestDto dto)
        {
            var response = await _inventoryRepository.AddIngredientAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(Guid id, UpsertIngredientRequestDto dto)
        {
            var response = await _inventoryRepository.UpdateIngredientAsync(id, dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(Guid id)
        {
            var response = await _inventoryRepository.DeleteIngredientAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("suppliers")]
        public async Task<IActionResult> GetSuppliers() => Ok(await _inventoryRepository.GetSuppliersAsync());

        [HttpPost("suppliers")]
        public async Task<IActionResult> AddSupplier(CreateSupplierDto dto)
        {
            var response = await _inventoryRepository.AddSupplierAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("purchase-orders")]
        public async Task<IActionResult> GetPurchaseOrders() => Ok(await _inventoryRepository.GetPurchaseOrdersAsync());

        [HttpPost("purchase-orders")]
        public async Task<IActionResult> CreatePurchaseOrder(CreatePurchaseOrderDto dto)
        {
            var response = await _inventoryRepository.CreatePurchaseOrderAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("purchase-orders/{id}/receive")]
        public async Task<IActionResult> ReceivePurchaseOrder(Guid id)
        {
            var response = await _inventoryRepository.ReceivePurchaseOrderAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
