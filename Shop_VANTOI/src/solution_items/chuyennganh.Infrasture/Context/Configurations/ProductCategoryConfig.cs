using chuyennganh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chuyennganh.Infrasture.Context.Configurations
{
    public class ProductCategoryConfig : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable("ProductCategory");
            builder.Property(x => x.ProductId).HasColumnName("ProductId");
            builder.Property(x => x.CategoryId).HasColumnName("CategoryId");
            builder.HasKey(x => new
            {
                x.ProductId,
                x.CategoryId
            });
            builder.HasOne(pc => pc.Category)
                   .WithMany(c => c.ProductCategories)
                   .HasForeignKey(pc => pc.CategoryId);
        }
    }
}