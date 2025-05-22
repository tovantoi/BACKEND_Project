using chuyennganh.Domain.Base;

namespace chuyennganh.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public int? Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? AvatarImagePath { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public bool IsActive { get; set; }
        public string? OTP { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? OTPExpiration { get; set; }
    }
}
