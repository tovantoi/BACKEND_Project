using chuyennganh.Domain.Base;
using chuyennganh.Domain.Enumerations;
using System.Text.Json.Serialization;

namespace chuyennganh.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int? Id { get; set; }

        public int? CustomerId { get; set; }
        public int? CustomerAddressId { get; set; }
        public int? CouponId { get; set; }
        public string? Payment { get; set; }
        public OrderStatus? Status { get; set; }
       // public int? Status { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [JsonIgnore]
        public ICollection<OrderItem>? OrderItems { get; set; }

        [JsonIgnore]
        public CustomerAddress? CustomerAddress { get; set; }

        [JsonIgnore]
        public Customer? Customer { get; set; }

        [JsonIgnore]
        public Coupon? Coupon { get; set; }
    }
}