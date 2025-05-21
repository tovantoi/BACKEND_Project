using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.ProdcutReview.Command
{
    public class GetAllReviewRequest : IRequest<List<ProductReview>>
    {
    }
}
