using chuyennganh.Application.Repositories;
using chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories
{
    public class PaymentTransactionRepository : GenericRepository<PaymentTransaction>, IPaymentTransactionRepository
    {
        public PaymentTransactionRepository(AppDbContext dbContext, ILogger<GenericRepository<PaymentTransaction>> logger) : base(dbContext, logger)
        {
        }
    }
}
