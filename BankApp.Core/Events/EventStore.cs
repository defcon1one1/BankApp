using BankApp.Core.Customers.Events;
using BankApp.Core.Repositories;

namespace BankApp.Core.Events;

public class EventStore
{
    private readonly IEventRepository _eventRepository;

    public EventStore(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public void AppendEvent(IEvent @event)
    {
        _eventRepository.Add(@event);
    }

    public async Task<IEnumerable<object>> GetEventsForAggregate(Guid aggregateId)
    {
        return await _eventRepository.GetAllByAggregateId(aggregateId);
    }
}
