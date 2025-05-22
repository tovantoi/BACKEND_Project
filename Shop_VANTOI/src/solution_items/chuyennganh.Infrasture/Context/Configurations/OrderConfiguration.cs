using chuyennganh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chuyennganh.Infrasture.Context.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("OrderId");
            builder.Property(x => x.CustomerId).HasColumnName("CustomerId");
            builder.Property(x => x.CustomerAddressId).HasColumnName("CustomerAddressId");
            builder.Property(x => x.Payment).HasColumnName("Payment");
            builder.Property(x => x.Status).HasColumnName("Status");
            builder.Property(x => x.TotalPrice).HasColumnName("TotalPrice");
            builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt");
            builder.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt");

            builder.HasMany(x => x.OrderItems).WithOne().HasForeignKey(x => x.OrderId);
            builder.HasOne(x => x.CustomerAddress).WithMany().HasForeignKey(x => x.CustomerAddressId);
            builder.HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId);
            builder.HasOne(x => x.Coupon).WithMany().HasForeignKey(x => x.CouponId);
        }
    }
}