using chuyennganh.Application.App.DTOs;
using MediatR;

namespace chuyennganh.Application.App.ProdcutReview.Command
{
    public class GetDetailReviewRequest : IRequest<List<ProductReviewDetailDTO>>
    {
        public int? ProductId { get; set; }
    }
}
