using restaurant_management_backend.Dtos.Order;

namespace restaurant_management_backend.Interfaces
{
    public interface IOrderHubClient
    {
        Task NewOrderReceived(KdsOrderDto newOrder);
        Task OrderStatusUpdated(Guid orderId, string newStatus);
    }
}
