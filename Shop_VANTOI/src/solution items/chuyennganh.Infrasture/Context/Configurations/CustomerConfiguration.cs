using chuyennganh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chuyennganh.Infrasture.Context.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("CustomerId");
            builder.Property(x => x.FirstName).HasColumnName("FirstName");
            builder.Property(x => x.LastName).HasColumnName("LastName");
            builder.Property(x => x.PhoneNumber).HasColumnName("PhoneNumber");
            builder.Property(x => x.Email).HasColumnName("Email");
            builder.Property(x => x.AvatarImagePath).HasColumnName("AvatarImagePath");
            builder.Property(x => x.Password).HasColumnName("Password");
            builder.Property(x => x.IsActive).HasColumnName("IsActive");
            builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt");
            builder.Property(x => x.Role).HasColumnName("Role");
            builder.Property(x => x.OTP).HasColumnName("OTP");
            builder.Property(x => x.OTPExpiration).HasColumnName("OTPExpiration");
        }
    }
}
