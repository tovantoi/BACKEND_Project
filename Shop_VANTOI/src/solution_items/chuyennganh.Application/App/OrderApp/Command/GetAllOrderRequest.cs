using MediatR;


namespace chuyennganh.Application.App.OrderApp.Command
{
    public class GetAllOrderRequest : IRequest<List<Domain.Entities.Order>>
    {
    }
}
