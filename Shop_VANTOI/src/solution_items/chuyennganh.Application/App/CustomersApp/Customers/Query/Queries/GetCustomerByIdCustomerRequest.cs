using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.CustomersApp.Customers.Query.Queries
{
    public class GetCustomerByIdCustomerRequest : IRequest<Customer>
    {
        public int? Id { get; set; }
    }
}
