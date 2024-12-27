using Microsoft.AspNetCore.SignalR;

namespace HaberPortal.Hubs
{
    public class NotificationHub : Hub
    {
        // Bildirim göndermek için bir metot
        public async Task SendNotification(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", user, message);
        }
    }
}
