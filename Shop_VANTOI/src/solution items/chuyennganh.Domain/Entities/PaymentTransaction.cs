using chuyennganh.Domain.Base;

namespace chuyennganh.Domain.Entities
{
    public class PaymentTransaction : BaseEntity
    {
        public int? Id { get; set; }

        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; } // Pending, Success, Failed
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? PaymentUrl { get; set; }
        public string? TransactionId { get; set; }
    }

}
