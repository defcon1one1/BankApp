using BankApp.Core.Models;
using BankApp.Core.Repositories;
using BankApp.Infrastructure.DAL.Entities;
using BankApp.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Infrastructure.DAL.Repositories;
public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _dbContext;
    public CustomerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Customer?> GetCustomerById(Guid id, CancellationToken cancellationToken)
    {
        CustomerEntity? customerEntity = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (customerEntity is not null)
        {
            return customerEntity.ToCustomer();
        }
        return null;
    }

    public async Task<bool> AddToBalance(Guid id, decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidOperationException("Amount must be greater than 0.");
        }

        CustomerEntity? customerEntity = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
        if (customerEntity is not null)
        {
            try
            {
                customerEntity.Balance += amount;
                _dbContext.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseOperationException("Add to balance operation failed.", ex);
            }
        }
        return false;
    }

    public async Task<bool> DeductFromBalance(Guid id, decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidOperationException("Amount must be greater than 0.");
        }
        CustomerEntity? customerEntity = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
        if (customerEntity is not null)
        {
            if (amount > customerEntity.Balance)
            {
                throw new InvalidOperationException("Amount cannot be greater than balance.");
            }
            try
            {
                customerEntity.Balance -= amount;
                _dbContext.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseOperationException("Deduct from balance operation failed.", ex);
            }
        }
        return false;
    }

}
