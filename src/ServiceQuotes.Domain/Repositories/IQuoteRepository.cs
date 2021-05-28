using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface IQuoteRepository : IRepository<Quote>
    {
        Task<IEnumerable<Quote>> GetTopQuotesByStatus(Status status, int count);
        Task<IEnumerable<Quote>> GetTopQuotesByCustomerIdAndStatus(Guid customerId, Status status, int count);
    }
}
