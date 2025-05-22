using chuyennganh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chuyennganh.Infrasture.Context.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItem");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("OrderItemId");
            builder.Property(x => x.OrderId).HasColumnName("OrderId");
            builder.Property(x => x.ProductId).HasColumnName("ProductId");
            builder.Property(x => x.Quantity).HasColumnName("Quantity");
            builder.Property(x => x.TotalPrice).HasColumnName("TotalPrice");

            builder.Ignore(x => x.Order);
            builder.HasOne(x => x.Order).WithMany(x => x.OrderItems).HasForeignKey(x => x.OrderId);
        }
    }
}
