using chuyennganh.Application.Response;
using MediatR;

namespace chuyennganh.Application.App.CategoryApp.Command
{
    public record DeleteCustomerRequest : IRequest<ServiceResponse>
    {
        public int? Id { get; set; }
    }
}
