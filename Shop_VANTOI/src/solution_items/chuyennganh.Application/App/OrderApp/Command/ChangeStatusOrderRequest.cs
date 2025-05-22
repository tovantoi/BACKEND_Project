using chuyennganh.Application.Response;
using chuyennganh.Domain.Enumerations;
using MediatR;
using System.Text.Json.Serialization;

namespace chuyennganh.Application.App.OrderApp.Command
{
    public record ChangeStatusOrderRequest : IRequest<ServiceResponse>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public OrderStatus? Status { get; set; }
    }
}
