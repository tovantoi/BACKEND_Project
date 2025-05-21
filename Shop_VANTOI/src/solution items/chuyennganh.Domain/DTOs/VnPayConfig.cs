namespace chuyennganh.Domain.DTOs
{
    public class VnPayConfig
    {
        public string TmnCode { get; set; } = null!;
        public string HashSecret { get; set; } = null!;
        public string ReturnUrl { get; set; } = null!;
        public string BaseUrl { get; set; } = null!;
    }
}
