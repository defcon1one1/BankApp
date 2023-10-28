namespace BankApp.Core.Customers.Events;
public interface IEvent
{
    Guid AggregateId { get; }
    public decimal Amount { get; set; }
}
