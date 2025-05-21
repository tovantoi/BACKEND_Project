using chuyennganh.Domain.Base;

namespace chuyennganh.Domain.Entities
{
    public class Coupon : BaseEntity
    {
        public int? Id { get; set; }

        public string? Code { get; set; }
        public string? Description { get; set; }
        public int TimesUsed { get; set; }
        public int MaxUsage { get; set; }
        public string? Discount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CouponStartDate { get; set; }
        public DateTime CouponEndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}