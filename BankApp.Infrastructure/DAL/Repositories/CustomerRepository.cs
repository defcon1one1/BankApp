using BankApp.Core.Models;
using BankApp.Core.Repositories;
using BankApp.Infrastructure.DAL.Entities;
using BankApp.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankApp.Infrastructure.DAL.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _dbContext;
    public CustomerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        CustomerEntity? customerEntity = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken: cancellationToken);
        return customerEntity?.ToCustomer();
    }

    public async Task<bool> CustomerExistsAsync(Guid id)
    {
        return await _dbContext.Customers.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> AddToBalanceAsync(Guid id, decimal amount)
    {
        using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            CustomerEntity? customerEntity = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customerEntity is not null)
            {
                customerEntity.Balance += amount;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
                return true;
            }
        }
        catch (DbUpdateException ex)
        {
            transaction.Rollback();
            throw new DatabaseOperationException("Add to balance operation failed.", ex);
        }
        return false;
    }

    public async Task<bool> DeductFromBalanceAsync(Guid id, decimal amount)
    {
        using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            CustomerEntity? customerEntity = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customerEntity is not null)
            {
                customerEntity.Balance -= amount;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
                return true;
            }
        }
        catch (DbUpdateException ex)
        {
            transaction.Rollback();
            throw new DatabaseOperationException("Deduct from balance operation failed.", ex);
        }
        return false;
    }

    public async Task<Customer?> GetByUsernameAsync(string username)
    {
        CustomerEntity? customerEntity = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Username == username);
        return customerEntity?.ToCustomer();
    }

    public async Task<bool> VerifyLoginAsync(string username, string passwordHash)
    {
        CustomerEntity? customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Username == username);
        if (customer is not null)
        {
            return customer.Username == username && customer.PasswordHash == passwordHash;
        }
        return false;
    }


    // methods for testing
    public async Task AddAsync(Customer customer)
    {
        CustomerEntity customerEntity = CustomerEntity.FromCustomer(customer);
        await _dbContext.Customers.AddAsync(customerEntity);
        await _dbContext.SaveChangesAsync();
    }
}