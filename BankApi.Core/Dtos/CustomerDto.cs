using BankApp.Core.Models;

namespace BankApp.Core.Dtos;
public class CustomerDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public decimal Balance { get; set; }

    internal static CustomerDto FromCustomer(Customer customer)
    {
        return new() { Id = customer.Id, FullName = $"{customer.FirstName} {customer.LastName}", Balance = customer.Balance };
    }
}
