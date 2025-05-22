using chuyennganh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace chuyennganh.Infrasture.Context.Configurations
{
    public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
        {
            builder.ToTable("PaymentTransaction");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id");

            builder.Property(x => x.OrderId).HasColumnName("OrderId");

            builder.Property(x => x.Amount).HasColumnName("Amount").HasColumnType("decimal(18,2)");

            builder.Property(x => x.PaymentMethod).HasColumnName("PaymentMethod").HasMaxLength(50);

            builder.Property(x => x.Status).HasColumnName("Status").HasMaxLength(50);

            builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.PaymentUrl).HasColumnName("PaymentUrl");

            builder.Property(x => x.TransactionId).HasColumnName("TransactionId").HasMaxLength(100);

        }
    }
}
