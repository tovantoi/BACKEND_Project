using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.ProdcutReview.Command
{
    public class GetDetailReviewRequest : IRequest<List<ProductReview>>
    {
        public int? ProductId { get; set; }
    }
}
