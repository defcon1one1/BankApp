using BankApp.Infrastructure.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BankApp.Infrastructure.DAL.Seeders;

public class DatabaseSeeder : IDatabaseSeeder
{
    public void SeedDatabase(IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // database will be created if it doesn't exist and migrations will be applied
        dbContext.Database.Migrate(); // please note that it may not be safe after any changes to the infrastructure!

        if (!dbContext.Customers.Any()) // ensure the database is empty before seeding
        {
            CustomerEntity[] customers = new[]
            {
                new CustomerEntity
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Smith",
                    Balance = 10.00m,
                    Username = "johnsmith",
                    // actual password is "pass123" - if you want to change it, you need to enter another's password hash
                    PasswordHash = "9b8769a4a742959a2d0298c36fb70623f2dfacda8436237df08d8dfd5b37374c"
                },
                new CustomerEntity
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Doe",
                    Balance = 100.00m,
                    Username = "janedoe",
                    // actual password is "pass123" - if you want to change it, you need to enter another's password hash
                    PasswordHash = "9b8769a4a742959a2d0298c36fb70623f2dfacda8436237df08d8dfd5b37374c"
                },
            };

            dbContext.Customers.AddRange(customers);
            dbContext.SaveChanges();
        }
    }

}
