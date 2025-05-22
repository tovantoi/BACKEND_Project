using MediatR;

namespace chuyennganh.Application.App.CustomersApp.CustomerAddress.Query.Queries
{
    public class GetAllCustomerAddressRequest : IRequest<List<Domain.Entities.CustomerAddress>>
    {
    }
}
