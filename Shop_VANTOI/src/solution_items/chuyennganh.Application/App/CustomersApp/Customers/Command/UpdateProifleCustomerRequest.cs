using chuyennganh.Application.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace chuyennganh.Application.App.CustomersApp.Customers.Command
{
    public record UpdateProifleCustomerRequest : IRequest<ServiceResponse>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? ImageData { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        [JsonIgnore]
        public bool? IsActive { get; set; } = true;
    }
}