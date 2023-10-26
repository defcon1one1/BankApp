using BankApp.Core.Models;

namespace BankApp.Infrastructure.DAL.Entities;
public class CustomerEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public decimal Balance { get; set; }

    public Customer ToCustomer()
    {
        return new Customer(Id, FirstName, LastName, Balance);
    }

    public static CustomerEntity FromCustomer(Customer customer)
    {
        return new CustomerEntity() { Id = customer.Id, FirstName = customer.FirstName, LastName = customer.LastName, Balance = customer.Balance };
    }

}
