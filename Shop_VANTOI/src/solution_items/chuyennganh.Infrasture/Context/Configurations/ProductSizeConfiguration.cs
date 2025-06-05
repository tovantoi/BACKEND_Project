using chuyennganh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chuyennganh.Infrasture.Context.Configurations
{
    public class ProductSizeConfiguration : IEntityTypeConfiguration<ProductSize>
    {
        public void Configure(EntityTypeBuilder<ProductSize> builder)
        {
            builder.ToTable("ProductSize");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.ProductId).HasColumnName("ProductId");
            builder.Property(x => x.SizeLabel).HasColumnName("SizeLabel").HasMaxLength(100);
            builder.Property(x => x.StockQuantity).HasColumnName("StockQuantity").HasDefaultValue(0);
            builder.HasOne(pr => pr.Product)
                  .WithMany(p => p.ProductSizes)
                   .HasForeignKey(x => x.ProductId);

        }
    }
}
