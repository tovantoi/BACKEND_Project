using chuyennganh.Application.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace chuyennganh.Application.App.CouponApp.Command
{
    public record CouponCreateRequest : IRequest<ServiceResponse>
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public int? TimesUsed { get; set; } = 0;
        public int? MaxUsage { get; set; }
        public string? Discount { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CouponStartDate { get; set; }
        public DateTime CouponEndDate { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
