using chuyennganh.Application.Response;
using MediatR;

namespace chuyennganh.Application.App.CategoryApp.Command
{
    public record DeleteCategoryRequest : IRequest<ServiceResponse>
    {
        public int? Id { get; set; }
    }
}
