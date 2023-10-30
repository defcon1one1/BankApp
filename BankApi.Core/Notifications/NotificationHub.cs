using Microsoft.AspNetCore.SignalR;
namespace BankApp.Core.Notifications;

public class NotificationHub : Hub
{
    public async Task SendNotification(string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", message);
    }
}
