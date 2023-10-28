using BankApp.Core.Customers.Events;
using System.Text.Json;

namespace BankApp.Infrastructure.DAL.Entities;

public class EventEntity
{
    public Guid Id { get; set; }
    public Guid AggregateId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string EventData { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }

    public static EventEntity FromEvent(IEvent @event)
    {
        return new EventEntity
        {
            AggregateId = @event.AggregateId,
            EventType = @event.GetType().Name,
            EventData = JsonSerializer.Serialize(new
            {
                @event.AggregateId,
                @event.Amount
            }),
            Timestamp = DateTime.UtcNow
        };
    }


    public static IEvent? ToEvent(EventEntity eventEntity)
    {
        Type? eventType = Type.GetType($"BankApp.Core.Customers.Events.{eventEntity.EventType}");
        return eventType is not null ? JsonSerializer.Deserialize(eventEntity.EventData, eventType) as IEvent : default!;
    }
}
