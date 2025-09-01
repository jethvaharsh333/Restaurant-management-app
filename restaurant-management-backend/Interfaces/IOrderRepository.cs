using restaurant_management_backend.Dtos.Order;
using restaurant_management_backend.Models.OrderAndOperations;
using restaurant_management_backend.Utility;

namespace restaurant_management_backend.Interfaces
{
    public interface IOrderRepository
    {
        Task<ApiResponse<OrderSummaryDto>> CreateOrderAsync(CreateOrderDto dto);
        Task<ApiResponse<object>> UpdateOrderStatusAsync(Guid orderId, OrderStatusEnum newStatus);
        Task<ApiResponse<List<OrderSummaryDto>>> GetMyOrdersAsync(Guid userId);
        Task<ApiResponse<OrderDetailDto>> GetOrderByIdAsync(Guid orderId);
        Task<ApiResponse<List<KdsOrderDto>>> GetActiveOrdersForKdsAsync();
    }
}
