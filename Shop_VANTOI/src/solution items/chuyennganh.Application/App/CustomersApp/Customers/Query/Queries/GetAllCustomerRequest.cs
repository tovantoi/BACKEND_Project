using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.CustomersApp.Customers.Query.Queries
{
    public class GetAllCustomerRequest : IRequest<List<Customer>>
    {
    }
}
