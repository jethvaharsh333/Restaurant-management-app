using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurant_management_backend.Dtos.Order;
using restaurant_management_backend.Interfaces;
using restaurant_management_backend.Models.OrderAndOperations;
using System.Security.Claims;

namespace restaurant_management_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;

        public OrderController(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            var response = await _orderRepo.CreateOrderAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "ADMIN,MANAGER,WAITER,KITCHEN")]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] OrderStatusEnum newStatus)
        {
            var response = await _orderRepo.UpdateOrderStatusAsync(id, newStatus);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            // You would get the userId from the JWT claims
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var response = await _orderRepo.GetMyOrdersAsync(userId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var response = await _orderRepo.GetOrderByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("kitchen")]
        [Authorize(Roles = "ADMIN,MANAGER,KITCHEN")]
        public async Task<IActionResult> GetActiveOrdersForKds()
        {
            var response = await _orderRepo.GetActiveOrdersForKdsAsync();
            return Ok(response);
        }
    }
}
