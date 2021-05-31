using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface IQuoteRepository : IRepository<Quote>
    {
        Task<Quote> GetWithServiceDetails(Guid id);
        Task<IEnumerable<Quote>> FindWithServiceDetails(Expression<Func<Quote, bool>> predicate);
        Task<IEnumerable<Quote>> GetTopQuotesByStatus(Status status, int count);
        Task<IEnumerable<Quote>> GetTopQuotesByCustomerIdAndStatus(Guid customerId, Status status, int count);
    }
}
