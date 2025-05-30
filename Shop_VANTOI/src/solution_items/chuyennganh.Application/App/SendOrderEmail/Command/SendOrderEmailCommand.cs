using chuyennganh.Application.Response;
using MediatR;

namespace chuyennganh.Application.App.SendOrderEmail.Command
{
    public class SendOrderEmailCommand : IRequest<ServiceResponse>
    {
        public string Email { get; set; }
        public string CustomerName { get; set; }
        public string OrderCode { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

    public class OrderItemDto
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
