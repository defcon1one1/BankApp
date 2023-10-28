using BankApp.Infrastructure.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankApp.Infrastructure.DAL.Configurations;

public class EventEntityConfiguration : IEntityTypeConfiguration<EventEntity>
{
    public void Configure(EntityTypeBuilder<EventEntity> builder)
    {
        builder.Property(e => e.AggregateId).IsRequired();
        builder.Property(e => e.EventType).IsRequired();
        builder.Property(e => e.EventData).IsRequired();

        builder.Property(e => e.EventData)
            .HasColumnType("nvarchar(max)"); // json can be stored in nvarcharmax

        builder.Property(e => e.Timestamp).IsRequired();
    }
}
