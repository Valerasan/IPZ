using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace IPZ_1.Hubs
{
    public class NotificationHub : Hub
    {
        public Task SendUpdate()
        {
            return Clients.Others.SendAsync("RefreshProducts");
        }
    }
}
