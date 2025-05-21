using chuyennganh.Application.App.DTOs;
using chuyennganh.Application.Response;
using MediatR;

namespace chuyennganh.Application.App.OrderApp.Command
{
    public record CreateOrderRequest : IRequest<ServiceResponse>
    {
        public int? CustomerId { get; set; }
        public List<OrderItemDTO>? OrderItems { get; set; } = new List<OrderItemDTO>();
        public string? CouponCode { get; set; }
        public int? CustomerAddressId { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
