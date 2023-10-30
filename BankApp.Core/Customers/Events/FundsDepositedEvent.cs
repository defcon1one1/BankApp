namespace BankApp.Core.Customers.Events;
public class FundsDepositedEvent : IEvent
{
    public Guid AggregateId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
}