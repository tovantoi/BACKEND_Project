using chuyennganh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chuyennganh.Infrasture.Context.Configurations
{
    public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
    {
        public void Configure(EntityTypeBuilder<ProductReview> builder)
        {
            builder.ToTable("ProductReview");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.ProductId).HasColumnName("ProductId");
            builder.Property(x => x.UserId).HasColumnName("UserId").HasMaxLength(int.MaxValue);
            builder.Property(x => x.Rating).HasColumnName("Rating").HasMaxLength(int.MaxValue);
            builder.Property(x => x.Comment).HasColumnName("Comment").HasMaxLength(int.MaxValue);
            builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt");

        }
    }
}
