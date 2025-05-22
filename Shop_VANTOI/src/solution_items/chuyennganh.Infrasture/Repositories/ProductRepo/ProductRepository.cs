using chuyennganh.Application.Repositories.ProductRepo;
using Entities = chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories.ProductRepo
{
    public class ProductRepository : GenericRepository<Entities.Product>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext, ILogger<GenericRepository<Entities.Product>> logger) : base(dbContext, logger)
        {

        }
    }
}
