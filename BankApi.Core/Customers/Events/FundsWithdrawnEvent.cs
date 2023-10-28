namespace BankApp.Core.Customers.Events;
public class FundsWithdrawnEvent : IEvent
{
    public Guid AggregateId { get; set; }
    public decimal Amount { get; set; }
}