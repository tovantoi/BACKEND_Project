using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.ProductApp.Query.Queries
{
    public class GetByNameProductQueris : IRequest<List<Product>>
    {
        public string? ProductName { get; set; }
    }
}
