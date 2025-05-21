using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories.OrderRepo
{
    public class ProductReviewRepository : GenericRepository<ProductReview>, IProductReviewRepository
    {
        public ProductReviewRepository(AppDbContext dbContext, ILogger<GenericRepository<ProductReview>> logger) : base(dbContext, logger)
        {
        }
    }
}
