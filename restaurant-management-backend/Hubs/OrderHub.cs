using Microsoft.AspNetCore.SignalR;
using restaurant_management_backend.Interfaces;

namespace restaurant_management_backend.Hubs
{
    public class OrderHub : Hub<IOrderHubClient>
    {
        public async Task JoinKdsGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "KDS");
        }

    }
}
