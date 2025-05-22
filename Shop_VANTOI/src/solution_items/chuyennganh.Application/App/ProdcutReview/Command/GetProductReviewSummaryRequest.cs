using MediatR;

namespace chuyennganh.Application.App.ProdcutReview.Command
{
    public class GetProductReviewSummaryRequest : IRequest<ProductReviewSummaryResponse>
    {
        public int? ProductId { get; set; }
    }
}
