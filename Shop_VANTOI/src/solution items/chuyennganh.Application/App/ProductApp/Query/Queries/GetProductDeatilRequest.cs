using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.ProductApp.Query.Queries
{
    public class GetProductDeatilRequest : IRequest<Product>
    {
        public int? Id { get; set; }
    }
}