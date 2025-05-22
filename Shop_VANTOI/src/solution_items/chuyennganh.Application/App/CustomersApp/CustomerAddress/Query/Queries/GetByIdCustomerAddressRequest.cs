using MediatR;

namespace chuyennganh.Application.App.CustomersApp.CustomerAddress.Query.Queries
{
    public class GetByIdCustomerAddressRequest : IRequest<Domain.Entities.CustomerAddress>
    {
        public int? Id { get; set; }
    }
}
