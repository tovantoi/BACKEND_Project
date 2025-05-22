using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.CategoryApp.Query.Queries
{
    public class GetAllCategoryRequest : IRequest<List<Category>>
    {
    }
}
