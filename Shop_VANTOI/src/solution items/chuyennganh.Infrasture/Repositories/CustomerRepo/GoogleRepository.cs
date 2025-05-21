using chuyennganh.Application.Repositories.CustomerRPRepo;
using chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories.CustomerRepo
{
    public class GoogleRepository : GenericRepository<GoogleAccount>, IGoogleRepository
    {
        public GoogleRepository(AppDbContext dbContext, ILogger<GenericRepository<GoogleAccount>> logger) : base(dbContext, logger)
        {
        }
    }
}
