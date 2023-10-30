using BankApp.Core.Repositories;
using BankApp.Infrastructure.DAL;
using BankApp.Infrastructure.DAL.Repositories;
using BankApp.Infrastructure.DAL.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankApp.Infrastructure.Extensions;
public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
    }
}