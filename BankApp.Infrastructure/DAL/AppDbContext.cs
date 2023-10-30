using BankApp.Infrastructure.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BankApp.Infrastructure.DAL;

public class AppDbContext : DbContext
{
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<EventEntity> Events { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Customers = Set<CustomerEntity>();
        Events = Set<EventEntity>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply entity configurations from the executing assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
