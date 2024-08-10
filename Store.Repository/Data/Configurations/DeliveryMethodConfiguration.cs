using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.Order_Aggregate;

namespace Store.Repository.Data.Configurations;

internal class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.Property(O => O.Cost)
         .HasColumnType("decimal(18,2)");
    }
}
