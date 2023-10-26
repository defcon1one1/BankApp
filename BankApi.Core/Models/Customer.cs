using BankApp.Core.Dtos;

namespace BankApp.Core.Models;
public class Customer
{
    public Guid Id { get; set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }

    public Customer(Guid id, string firstName, string lastName, decimal balance)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Balance = balance;
    }

    public static Customer Create(Guid id, string firstName, string lastName, decimal balance)
    {
        return new Customer(id, firstName, lastName, balance);
    }

    public static CustomerDto ToDto(Customer customer)
    {
        return new CustomerDto() { Id = customer.Id, FullName = $"{customer.FirstName} {customer.LastName}", Balance = customer.Balance };
    }
}
