using BankApp.Core.Models;

namespace BankApp.Core.Repositories;
public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> AddToBalanceAsync(Guid id, decimal amount);
    Task<bool> DeductFromBalanceAsync(Guid id, decimal amount);
    Task<bool> CustomerExistsAsync(Guid id);
    Task<Customer?> GetByUsernameAsync(string username);
    Task<bool> VerifyLoginAsync(string username, string passwordHash);
    Task AddAsync(Customer customer);
}