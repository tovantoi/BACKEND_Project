using chuyennganh.Application.Response;
using MediatR;

namespace chuyennganh.Application.App.ProdcutReview.Command
{
    public record CreateReviewRequest : IRequest<ServiceResponse>
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public string? ImageUrl { get; set; } // Lưu URL ảnh (nếu có)
        public string? VideoUrl { get; set; }
    }

}
