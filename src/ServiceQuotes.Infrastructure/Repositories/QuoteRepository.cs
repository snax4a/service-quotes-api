using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Entities.Enums;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class QuoteRepository : Repository<Quote>, IQuoteRepository
    {
        public QuoteRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Quote> GetWithServiceDetails(Guid id)
        {
            return await _entities
                    .Include(q => q.ServiceRequest)
                        .ThenInclude(sr => sr.Customer)
                    .Include(q => q.ServiceRequest)
                        .ThenInclude(sr => sr.Address)
                    .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Quote>> FindWithServiceDetails(Expression<Func<Quote, bool>> predicate)
        {
            return await _entities
                        .Include(q => q.ServiceRequest)
                            .ThenInclude(sr => sr.Customer)
                                .ThenInclude(c => c.Account)
                        .Include(q => q.ServiceRequest)
                            .ThenInclude(sr => sr.Address)
                        .Where(predicate)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Quote>> GetTopQuotesByStatus(Status status, int count)
        {
            return await _entities
                        .Include(q => q.ServiceRequest)
                            .ThenInclude(sr => sr.Customer)
                                .ThenInclude(c => c.Account)
                        .Include(q => q.ServiceRequest)
                            .ThenInclude(sr => sr.Address)
                        .Where(q => q.Status == status)
                        .OrderBy(q => q.ServiceRequest.CompletionDate)
                        .Take(count)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Quote>> GetTopQuotesByCustomerIdAndStatus(Guid customerId, Status status, int count)
        {
            return await _entities
                        .Include(q => q.ServiceRequest)
                            .ThenInclude(sr => sr.Customer)
                        .Include(q => q.ServiceRequest)
                            .ThenInclude(sr => sr.Address)
                        .Where(q => q.Status == status && q.ServiceRequest.CustomerId == customerId)
                        .OrderBy(q => q.ServiceRequest.CompletionDate)
                        .Take(count)
                        .ToListAsync();
        }
    }
}
