using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories.ProductRepo
{
    public class ProductSizeRepository : GenericRepository<Domain.Entities.ProductSize>, IProductSizeRepository
    {
        public ProductSizeRepository(AppDbContext dbContext, ILogger<GenericRepository<Domain.Entities.ProductSize>> logger) : base(dbContext, logger)
        {

        }
    }
}
