using chuyennganh.Application.App.DTOs;
using MediatR;

namespace chuyennganh.Application.App.OrderApp.Command
{
    public class GetOrderByCustomerIdRequest : IRequest<List<OrderDTO>>
    {
        public int Id { get; set; }
    }
}