using chuyennganh.Application.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace chuyennganh.Application.App.CouponApp.Command
{
    public record CouponUpdateRequest : IRequest<ServiceResponse>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int? TimesUsed { get; set; }
        public int? MaxUsage { get; set; }
        public string? Discount { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CouponStartDate { get; set; }
        public DateTime CouponEndDate { get; set; }
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
