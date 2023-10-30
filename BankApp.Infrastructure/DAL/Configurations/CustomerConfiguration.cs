using BankApp.Infrastructure.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankApp.Infrastructure.DAL.Configurations;
public class CustomerConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.FirstName).IsRequired();
        builder.Property(e => e.LastName).IsRequired();
        builder.Property(e => e.Balance).HasColumnType("money");
    }
}