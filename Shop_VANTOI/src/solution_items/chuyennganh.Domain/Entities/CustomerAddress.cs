using chuyennganh.Domain.Base;

namespace chuyennganh.Domain.Entities
{
    public class CustomerAddress : BaseEntity
    {
        public int? Id { get; set; }

        public int CustomerId { get; set; }
        public string? Address { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
    }
}