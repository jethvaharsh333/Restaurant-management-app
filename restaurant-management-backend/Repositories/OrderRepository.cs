using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using restaurant_management_backend.Data;
using restaurant_management_backend.Dtos.Order;
using restaurant_management_backend.Hubs;
using restaurant_management_backend.Interfaces;
using restaurant_management_backend.Models.OrderAndOperations;
using restaurant_management_backend.Utility;

namespace restaurant_management_backend.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<OrderHub, IOrderHubClient> _orderHubContext;

        public OrderRepository(ApplicationDbContext context, IMapper mapper, IHubContext<OrderHub, IOrderHubClient> orderHubContext)
        {
            _context = context;
            _mapper = mapper;
            _orderHubContext = orderHubContext;
        }

        public async Task<ApiResponse<OrderSummaryDto>> CreateOrderAsync(CreateOrderDto dto)
        {
            var order = _mapper.Map<OrderModel>(dto);
            order.OrderStatus = OrderStatusEnum.Pending;

            decimal totalAmount = 0;

            foreach (var itemDto in dto.Items)
            {
                var menuItem = await _context.MenuItems.FindAsync(itemDto.MenuItemId);
                if (menuItem == null || !menuItem.IsAvailable)
                    return ApiResponse<OrderSummaryDto>.FailureResponse($"Menu item with ID {itemDto.MenuItemId} is not available.", 400);

                var orderItem = new OrderItemModel
                {
                    MenuItemId = itemDto.MenuItemId,
                    Quantity = itemDto.Quantity,
                    PriceAtTimeOfOrder = menuItem.Price,
                    //Modifiers = itemDto.Modifiers
                };

                totalAmount += menuItem.Price * itemDto.Quantity;
                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = totalAmount;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var kdsDto = _mapper.Map<KdsOrderDto>(order);
            //await _orderHubContext.Clients.Group("KDS").SendAsync("NewOrderReceived", kdsDto);
            await _orderHubContext.Clients.Group("KDS").NewOrderReceived(kdsDto);

            var summaryDto = _mapper.Map<OrderSummaryDto>(order);
            return ApiResponse<OrderSummaryDto>.SuccessResponse(summaryDto, "Order created successfully.", 201);
        }

        public async Task<ApiResponse<object>> UpdateOrderStatusAsync(Guid orderId, OrderStatusEnum newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return ApiResponse<object>.FailureResponse("Order not found.", 404);

            order.OrderStatus = newStatus;
            await _context.SaveChangesAsync();

            //await _orderHubContext.Clients.User(order.UserId.ToString()).SendAsync("OrderStatusUpdated", new { orderId, newStatus = newStatus.ToString() });
            await _orderHubContext.Clients.User(order.UserId.ToString()).OrderStatusUpdated(orderId, newStatus.ToString());
            // If the status is now "Ready", you might also notify waiters

            return ApiResponse<object>.SuccessResponse(null, "Order status updated.");
        }

        public async Task<ApiResponse<List<OrderSummaryDto>>> GetMyOrdersAsync(Guid userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var orderDtos = _mapper.Map<List<OrderSummaryDto>>(orders);
            return ApiResponse<List<OrderSummaryDto>>.SuccessResponse(orderDtos);
        }

        public async Task<ApiResponse<OrderDetailDto>> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Table)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return ApiResponse<OrderDetailDto>.FailureResponse("Order not found.", 404);
            }

            var orderDto = _mapper.Map<OrderDetailDto>(order);
            return ApiResponse<OrderDetailDto>.SuccessResponse(orderDto);
        }

        public async Task<ApiResponse<List<KdsOrderDto>>> GetActiveOrdersForKdsAsync()
        {
            var activeStatuses = new[] { OrderStatusEnum.Pending, OrderStatusEnum.Confirmed, OrderStatusEnum.Preparing };

            var orders = await _context.Orders
                .Include(o => o.Table)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .Where(o => activeStatuses.Contains(o.OrderStatus))
                .OrderBy(o => o.OrderDate)
                .ToListAsync();

            var kdsDtos = _mapper.Map<List<KdsOrderDto>>(orders);
            return ApiResponse<List<KdsOrderDto>>.SuccessResponse(kdsDtos);
        }


    }
}
