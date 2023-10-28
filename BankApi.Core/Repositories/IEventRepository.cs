using BankApp.Core.Customers.Events;

namespace BankApp.Core.Repositories;
public interface IEventRepository
{
    Task Add(IEvent @event);
    Task<IEnumerable<IEvent>> GetAllByAggregateId(Guid aggregateId);
}
