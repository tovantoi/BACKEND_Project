using chuyennganh.Application.App.DTOs;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace chuyennganh.Application.App.ProductApp.Query.Queries
{
    public class GetAllProductsQueris : IRequest<PagedResponse<ProductDTO>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public GetAllProductsQueris(int pageNumber = 1, int pageSize = 8)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
