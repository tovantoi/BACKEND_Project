using chuyennganh.Application.Response;
using chuyennganh.Domain.Enumerations;
using MediatR;

namespace chuyennganh.Application.App.OrderApp.Command
{
    public record ChangeStatusOrderUserRequest : IRequest<ServiceResponse>
    {
        public int? Id { get; set; }
        public OrderStatus? Status { get; set; }
    }
}
