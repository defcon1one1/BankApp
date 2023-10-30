using BankApp.Core.Customers.Events;
using BankApp.Core.Repositories;
using BankApp.Infrastructure.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Infrastructure.DAL.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _dbContext;

    public EventRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(IEvent @event)
    {
        var eventEntity = EventEntity.FromEvent(@event);
        _dbContext.Events.Add(eventEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<IEvent>> GetAllByAggregateId(Guid aggregateId)
    {
        List<EventEntity> eventEntities = await _dbContext.Events
            .Where(e => e.AggregateId == aggregateId)
            .OrderBy(e => e.Timestamp)
            .ToListAsync();

        IEnumerable<IEvent> events = eventEntities.Select(EventEntity.ToEvent).Where(e => e is not null).Cast<IEvent>();
        return events;
    }
}