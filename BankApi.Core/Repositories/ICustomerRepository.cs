using BankApp.Core.Customers.Commands.LoginCommand;
using BankApp.Core.Models;

namespace BankApp.Core.Repositories;
public interface ICustomerRepository
{
    Task<Customer?> GetById(Guid id, CancellationToken cancellationToken);
    Task<bool> AddToBalance(Guid id, decimal amount);
    Task<bool> DeductFromBalance(Guid id, decimal amount);
    Task<bool> CustomerExists(Guid id);
    Task<Customer?> GetByUsername(string username);
    Task<bool> VerifyLogin(LoginRequest loginRequest);
}
