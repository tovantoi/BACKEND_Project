using chuyennganh.Application.Repositories.BlogRepo;
using chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories.BlogRepo
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(AppDbContext dbContext, ILogger<GenericRepository<Blog>> logger) : base(dbContext, logger)
        {
        }
    }
}
