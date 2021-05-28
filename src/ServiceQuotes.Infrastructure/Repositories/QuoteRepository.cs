using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Entities.Enums;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class QuoteRepository : Repository<Quote>, IQuoteRepository
    {
        public QuoteRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Quote>> GetTopQuotesByStatus(Status status, int count)
        {
            return await _entities
                        .Include(q => q.ServiceRequest)
                        .Where(q => q.Status == status)
                        .OrderBy(q => q.ServiceRequest.CompletionDate)
                        .Take(count)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Quote>> GetTopQuotesByCustomerIdAndStatus(Guid customerId, Status status, int count)
        {
            return await _entities
                        .Include(q => q.ServiceRequest)
                        .Where(q => q.Status == status && q.ServiceRequest.CustomerId == customerId)
                        .OrderBy(q => q.ServiceRequest.CompletionDate)
                        .Take(count)
                        .ToListAsync();
        }
    }
}
