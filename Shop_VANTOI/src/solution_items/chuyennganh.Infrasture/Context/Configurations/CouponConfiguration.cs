using chuyennganh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chuyennganh.Infrasture.Context.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.ToTable("Coupons");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("CouponId");
            builder.Property(x => x.Code).HasColumnName("Code");
            builder.Property(x => x.Description).HasColumnName("Description");
            builder.Property(x => x.TimesUsed).HasColumnName("TimesUsed");
            builder.Property(x => x.MaxUsage).HasColumnName("MaxUsage");
            builder.Property(x => x.Discount).HasColumnName("Discount");
            builder.Property(x => x.IsActive).HasColumnName("IsActive");
            builder.Property(x => x.CouponStartDate).HasColumnName("CouponStartDate");
            builder.Property(x => x.CouponEndDate).HasColumnName("CouponEndDate");
            builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt");
            builder.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
