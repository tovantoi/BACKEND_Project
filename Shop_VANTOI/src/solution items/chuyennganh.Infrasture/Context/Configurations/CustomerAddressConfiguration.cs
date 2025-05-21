using chuyennganh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chuyennganh.Infrasture.Context.Configurations
{
    public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
    {
        public void Configure(EntityTypeBuilder<CustomerAddress> builder)
        {
            builder.ToTable("CustomerAddresses");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("CustomerAddressesId");
            builder.Property(x => x.CustomerId).HasColumnName("CustomerId");
            builder.Property(x => x.Address).HasColumnName("Address");
            builder.Property(x => x.FullName).HasColumnName("FullName");
            builder.Property(x => x.Phone).HasColumnName("Phone");
            builder.Property(x => x.Province).HasColumnName("Province");
            builder.Property(x => x.District).HasColumnName("District");
            builder.Property(x => x.Ward).HasColumnName("Ward");
        }
    }
}
