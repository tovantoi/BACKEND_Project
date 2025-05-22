using chuyennganh.Domain.Base;

namespace chuyennganh.Domain.Entities
{
    public class GoogleAccount : BaseEntity
    {
        public int? Id { get; set; }

        public string GoogleId { get; set; }

        public string Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? AvatarUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
