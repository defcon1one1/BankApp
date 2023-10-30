using BankApp.Core.Customers.Events;

namespace BankApp.Core.Models;

public class Customer
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public decimal Balance { get; private set; }

    private readonly List<IEvent> pendingEvents = new();

    public Customer(Guid id, string firstName, string lastName, decimal balance)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Balance = balance;
    }

    public bool Deposit(decimal amount)
    {
        if (amount >= 0 && decimal.TryParse(amount.ToString(), out _))
        {
            Apply(new FundsDepositedEvent { AggregateId = Id, CustomerId = Id, Amount = amount });
            return true;
        }
        return false;
    }

    public bool Withdraw(decimal amount)
    {

        if (amount >= 0 && decimal.TryParse(amount.ToString(), out _))
        {
            Apply(new FundsWithdrawnEvent { AggregateId = Id, Amount = amount });
            return true;
        }
        return false;
    }

    private void Apply(IEvent @event)
    {
        switch (@event)
        {
            case FundsDepositedEvent e:
                Balance += e.Amount;
                break;
            case FundsWithdrawnEvent e:
                Balance -= e.Amount;
                break;
        }
        pendingEvents.Add(@event);
    }

    public IEnumerable<IEvent> GetPendingEvents()
    {
        return pendingEvents.AsReadOnly();
    }

    public void ClearPendingEvents()
    {
        pendingEvents.Clear();
    }

    public static Customer LoadFromEvents(Guid customerId, IEnumerable<IEvent> events)
    {
        Customer customer = new(customerId, "", "", 0);
        foreach (var @event in events)
        {
            customer.Apply(@event);
        }
        customer.ClearPendingEvents();
        return customer;
    }
}
