using chuyennganh.Application.App.DTOs;
using MediatR;
using System.Text.Json.Serialization;

namespace chuyennganh.Application.App.CustomersApp.CustomerAddress.Query.Queries
{
    public class GetCustomerAddressByCustomerIdRequest : IRequest<List<CustomerAddressDTO>>
    {
        [JsonIgnore]
        public int? CustomerId { get; set; }
    }
}