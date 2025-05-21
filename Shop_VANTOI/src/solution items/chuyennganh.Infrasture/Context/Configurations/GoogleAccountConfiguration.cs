using chuyennganh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chuyennganh.Infrasture.Context.Configurations
{
    public class GoogleAccountConfiguration : IEntityTypeConfiguration<GoogleAccount>
    {
        public void Configure(EntityTypeBuilder<GoogleAccount> builder)
        {
            builder.ToTable("GoogleAccounts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.GoogleId)
                   .HasMaxLength(100);

            builder.Property(x => x.Email)
                   .HasMaxLength(255);

            builder.HasIndex(x => x.Email);

            builder.Property(x => x.FirstName)
                   .HasMaxLength(100);

            builder.Property(x => x.LastName)
                   .HasMaxLength(100);

            builder.Property(x => x.AvatarUrl)
                   .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

        }
    }
}
