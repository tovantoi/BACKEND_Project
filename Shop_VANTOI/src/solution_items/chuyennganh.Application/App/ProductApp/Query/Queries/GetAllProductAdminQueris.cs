using chuyennganh.Application.App.DTOs;
using MediatR;

namespace chuyennganh.Application.App.ProductApp.Query.Queries
{
    public class GetAllProductAdminQueris : IRequest<List<ProductDTO>>
    {
    }
}
