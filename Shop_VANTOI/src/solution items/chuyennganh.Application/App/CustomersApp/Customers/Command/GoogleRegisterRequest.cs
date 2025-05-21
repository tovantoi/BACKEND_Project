using chuyennganh.Application.Response;
using MediatR;

namespace chuyennganh.Application.App.CustomersApp.Customers.Command
{
    public record GoogleRegisterRequest : IRequest<ServiceResponse>
    {
        public string GoogleCredential { get; set; } = string.Empty;
    }

}
