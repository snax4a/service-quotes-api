using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Payment>> FindWithQuoteAndCustomer(Expression<Func<Payment, bool>> predicate)
        {
            return await _entities
                        .Include(p => p.Customer)
                        .Include(p => p.Quote)
                        .Where(predicate)
                        .OrderByDescending(q => q.Created)
                        .ToListAsync();
        }

        public async Task<Payment> GetWithQuoteAndCustomer(Guid id)
        {
            return await _entities
                    .Include(p => p.Customer)
                        .ThenInclude(p => p.Account)
                    .Include(p => p.Quote)
                    .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Payment> GetWithQuoteAndCustomer(string id)
        {
            return await _entities
                    .Include(p => p.Customer)
                        .ThenInclude(p => p.Account)
                    .Include(p => p.Quote)
                    .SingleOrDefaultAsync(e => e.TransactionId == id);
        }
    }
}
