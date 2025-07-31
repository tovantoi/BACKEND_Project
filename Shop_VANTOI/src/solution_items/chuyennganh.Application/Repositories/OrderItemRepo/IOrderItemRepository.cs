using chuyennganh.Domain.Entities;
using System.Linq.Expressions;

namespace chuyennganh.Application.Repositories.OrderItemRepo
{
    public interface IOrderItemRepository : IGenericReponsitory<OrderItem>
    {
                Task<OrderItem?> FindSingleAsync(
            Expression<Func<OrderItem, bool>> predicate,
            Func<IQueryable<OrderItem>, IQueryable<OrderItem>>? include = null
        );

    }
}