using chuyennganh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chuyennganh.Infrasture.Context.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImage");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ProductImageId");

            builder.Property(x => x.ProductId).HasColumnName("ProductId");

            builder.Property(x => x.ImageUrl).HasColumnName("ImageUrl");

            builder.Property(x => x.Color).HasColumnName("Color");

            builder.Property(x => x.SortOrder).HasColumnName("SortOrder");

            builder.HasOne(pi => pi.Product)
                  .WithMany(p => p.ProductImages)
                  .HasForeignKey(pi => pi.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
