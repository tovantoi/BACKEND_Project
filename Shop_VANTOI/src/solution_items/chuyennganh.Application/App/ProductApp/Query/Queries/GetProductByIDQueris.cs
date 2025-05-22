using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.ProductApp.Query.Queries
{
    public class GetProductByIDQueris : IRequest<Product>
    {
        public int? Id { get; set; }
    }
}
