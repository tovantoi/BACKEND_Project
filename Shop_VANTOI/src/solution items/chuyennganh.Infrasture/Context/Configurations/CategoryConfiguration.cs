using chuyennganh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chuyennganh.Infrasture.Context.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("CategoryId");
            builder.Property(x => x.ParentId).HasColumnName("ParentId");
            builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(int.MaxValue);
            builder.Property(x => x.Description).HasColumnName("Description").HasMaxLength(int.MaxValue);
            builder.Property(x => x.ImagePath).HasColumnName("ImagePath").HasMaxLength(int.MaxValue);
            builder.Property(x => x.IsActive).HasColumnName("IsActive");
            builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt");

            builder.HasMany(x => x.ProductCategories).WithOne().HasForeignKey(x => x.CategoryId);
        }
    }
}
