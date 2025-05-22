using chuyennganh.Domain.Base;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace chuyennganh.Application.Repositories
{
    public interface IGenericReponsitory<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity);
        void Create (T entity);
        Task<T?> GetByIdAsync(object id);
        Task<bool> ExistsAsync(int orderId);
        Task DeleteAsync(object id);
        void RemoveMultiple(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
     //   Task<T> GetById(int ProductId);
        Task<IReadOnlyList<T>> GetAll(); 
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindAll(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includeProperties);
        Task<(List<int>? existingIds, List<int>? missingIds)> CheckIdsExistAsync(List<int>? ids);
        Task<T?> FindSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
        Task<bool> HasUserPurchasedProductAsync(int customerId, int productId);

        IDbContextTransaction BeginTransaction();

    }
}
