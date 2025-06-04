using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories.ProductRepo
{
    public class ProductImageRepository : GenericRepository<Domain.Entities.ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(AppDbContext dbContext, ILogger<GenericRepository<Domain.Entities.ProductImage>> logger) : base(dbContext, logger)
        {

        }
    }
}
