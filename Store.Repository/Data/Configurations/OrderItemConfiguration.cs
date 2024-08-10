using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.Order_Aggregate;

namespace Store.Repository.Data.Configurations;

internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.OwnsOne(O => O.Product, Product => Product.WithOwner()); //[1:1] Total Participation

        builder.Property(O => O.Price)
            .HasColumnType("decimal(18,2)");
    }
}
