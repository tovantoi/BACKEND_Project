using chuyennganh.Application.Response;
using MediatR;

namespace chuyennganh.Application.App.CustomersApp.Customers.Command
{
    public record LogoutCustomerRequest : IRequest<ServiceResponse>
    {
    }
}