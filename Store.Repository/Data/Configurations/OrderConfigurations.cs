using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.Order_Aggregate;

namespace Store.Repository.Data.Configurations;

internal class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(O => O.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner()); // used to  embedding related data within an entity without make relationship with foreignkey and make another table in databse make ef optimization and speed

        builder.Property(o => o.Status) // used to convert data into database with custom type like using string instead enum in database and otherway to convert string from database to enum again to deals in code
            .HasConversion
            (
            OStatus=>OStatus.ToString(),
            OStatus=> (OrderStatus) Enum.Parse(typeof(OrderStatus),OStatus)
            );

        builder.Property(O => O.SubTotal)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(O => O.DeliveryMethod)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);
    }
}
