using MediatR;

namespace chuyennganh.Application.App.CategoryApp.Query.Queries
{
    public class GetAllNameCategoryRequest : IRequest<List<CategoryNameDto>> // Trả về Dto chứa tên danh mục
    {
    }
}
