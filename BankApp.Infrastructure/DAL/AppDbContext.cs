using BankApp.Infrastructure.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BankApp.Infrastructure.DAL;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<EventEntity> Events { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
