using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.ProductApp.Query.Queries
{
    public class GetByIdImageProductQueris : IRequest<List<ProductImage>>
    {
        public int ProductId { get; set; }
    }
}
