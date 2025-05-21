using chuyennganh.Application.Response;
using MediatR;

namespace chuyennganh.Application.App.ProductApp.Command
{
    public record DeleteProductCommand : IRequest<ServiceResponse>
    {
        public int? Id { get; set; }
    }
}
