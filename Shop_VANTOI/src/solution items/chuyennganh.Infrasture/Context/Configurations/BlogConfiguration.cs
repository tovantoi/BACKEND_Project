using chuyennganh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chuyennganh.Infrasture.Context.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("Blogs");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id");

            builder.Property(x => x.Title).HasColumnName("Title").HasMaxLength(500);

            builder.Property(x => x.Description).HasColumnName("Description").HasMaxLength(int.MaxValue);

            builder.Property(x => x.CoverImage).HasColumnName("CoverImage");

            builder.Property(x => x.Slug).HasColumnName("Slug");

            builder.Property(x => x.VideoUrl).HasColumnName("VideoUrl");

            builder.Property(x => x.IsActive).HasColumnName("IsActive").HasDefaultValue(true);

            builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasDefaultValueSql("GETDATE()");
        }
    }
}
