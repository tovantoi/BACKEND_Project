using chuyennganh.Application.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace chuyennganh.Application.App.CustomersApp.CustomerAddress.Command
{
    public record CustomerAddressUpdateRequest : IRequest<ServiceResponse>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public string? Address { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
    }
}
