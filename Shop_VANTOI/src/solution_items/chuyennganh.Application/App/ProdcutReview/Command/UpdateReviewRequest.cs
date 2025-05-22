using chuyennganh.Application.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace chuyennganh.Application.App.ProdcutReview.Command
{
    public record UpdateReviewRequest : IRequest<ServiceResponse>
    {
        [JsonIgnore]
        public int? Id { get; set; } 
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public string? ImageUrl { get; set; } 
        public string? VideoUrl { get; set; }
    }
}
