using chuyennganh.Domain.Entities;
using chuyennganh.Domain.Enumerations;

namespace chuyennganh.Application.Repositories.OrderRepo
{
    public interface IOrderRepository : IGenericReponsitory<Order>
    {
        Task<bool> ExistsAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);

    }
}
