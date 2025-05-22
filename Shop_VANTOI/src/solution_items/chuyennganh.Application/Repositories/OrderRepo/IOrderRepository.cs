using chuyennganh.Domain.Entities;

namespace chuyennganh.Application.Repositories.OrderRepo
{
    public interface IOrderRepository : IGenericReponsitory<Order>
    {
        Task<bool> ExistsAsync(int id);
    }
}
