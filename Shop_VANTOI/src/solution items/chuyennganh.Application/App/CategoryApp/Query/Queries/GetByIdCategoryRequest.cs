using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.CategoryApp.Query.Queries
{
    public class GetByIdCategoryRequest : IRequest<Category>
    {
        public int? Id { get; set; }
    }
}
