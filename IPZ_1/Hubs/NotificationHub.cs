using IPZ_1.Models.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace IPZ_1.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task RefreshProducts(string message)
        {
            await Clients.All.SendAsync("RefreshProducts", message);
        }
    }
}
