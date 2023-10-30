using BankApp.Core.Hubs;
using Microsoft.AspNetCore.SignalR;

public interface INotificationService
{
    Task SendOperationNotification(string message);
}

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendOperationNotification(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
    }
}
