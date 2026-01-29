using EventDriven.OrderProcessing.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventDriven.OrderProcessing.Infrastructure.Persistence.Configurations;
public sealed class OrderItemConfiguration
    : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Price)
            .HasPrecision(18, 2);

        builder.Property(i => i.ProductName)
            .IsRequired();

        builder.Property(i => i.Quantity)
            .IsRequired();
    }
}
