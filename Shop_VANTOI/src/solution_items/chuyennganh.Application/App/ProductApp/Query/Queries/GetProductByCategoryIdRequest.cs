using chuyennganh.Application.App.DTOs;
using MediatR;
using chuyennganh.Domain.ExceptionEx;

namespace chuyennganh.Application.App.ProductApp.Query.Queries
{
    public class GetProductByCategoryIdRequest : IRequest<PagedResponse<ProductDTO>>
    {
        public int? CategoryId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public GetProductByCategoryIdRequest(int pageNumber = 1, int pageSize = 6)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
