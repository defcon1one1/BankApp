using BankApp.Core.Customers.Events;
using BankApp.Core.Repositories;

namespace BankApp.Core.Events;

public class EventStore
{
    private readonly IEventRepository _eventRepository;
    private readonly object _lockObject = new();

    public EventStore(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public void AppendEvent(IEvent @event)
    {
        lock (_lockObject)
        {
            _eventRepository.Add(@event);
        }
    }

    public async Task<IEnumerable<object>> GetEventsForAggregate(Guid aggregateId)
    {
        lock (_lockObject)
        {
            return await _eventRepository.GetAllByAggregateId(aggregateId);
        }
    }
}
