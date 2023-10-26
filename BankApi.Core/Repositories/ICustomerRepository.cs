using BankApp.Core.Models;

namespace BankApp.Core.Repositories;
public interface ICustomerRepository
{
    Task<Customer?> GetCustomerById(Guid id, CancellationToken cancellationToken);
    Task<bool> AddToBalance(Guid id, decimal amount);
    Task<bool> DeductFromBalance(Guid id, decimal amount);
}
